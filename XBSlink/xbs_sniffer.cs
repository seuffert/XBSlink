/**
 * Project: XBSlink: A XBox360 & PS3/2 System Link Proxy
 * File name: xbs_sniffer.cs
 *   
 * @author Oliver Seuffert, Copyright (C) 2011.
 */
/* 
 * XBSlink is free software; you can redistribute it and/or modify 
 * it under the terms of the GNU General Public License as published by 
 * the Free Software Foundation; either version 2 of the License, or 
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful, but 
 * WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
 * or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License 
 * for more details.
 * 
 * You should have received a copy of the GNU General Public License along 
 * with this program; If not, see <http://www.gnu.org/licenses/>
 */

using System;
using System.Collections.Generic;
using System.Text;
using PacketDotNet;
using SharpPcap;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace XBSlink
{
    class xbs_sniffer_statistics
    {
        private static UInt32 packet_count = 0;
        private static Object _locker = new Object();

        public static void increase_packet_count()
        {
            lock (_locker)
                packet_count++;
        }

        public static UInt32 getPacketCount()
        {
            UInt32 pcount;
            lock (_locker)
                pcount = packet_count;
            return pcount;
        }
    }

    class xbs_sniffer
    {
        private SharpPcap.LibPcap.LibPcapLiveDevice pdev = null;
        public int readTimeoutMilliseconds = 1000;
        public bool pdev_sniff_additional_broadcast = true;
        public bool pdev_filter_use_special_macs = true;
        public bool pdev_filter_only_forward_special_macs = true;

        private String pdev_filter = "udp and ((ip host 0.0.0.1) or (dst port 3074)) ";
        private String pdev_filter_all_broadcast = "udp and ((ip host 0.0.0.1) or (dst port 3074) or (ether host FF:FF:FF:FF:FF:FF and ip dst host 255.255.255.255)) ";
        private List<PhysicalAddress> pdev_filter_sniffed_macs = new List<PhysicalAddress>();
        private List<PhysicalAddress> pdev_filter_special_macs = new List<PhysicalAddress>();

        private Thread dispatcher_thread = null;
        private bool exiting = false;

        public static Queue<RawPacket> packets = new Queue<RawPacket>();

        private List<int> injected_macs_hash = new List<int>();
        private List<int> sniffed_macs_hash = new List<int>();
        private List<PhysicalAddress> sniffed_macs = new List<PhysicalAddress>();

        private xbs_node_list node_list = null;

        public xbs_sniffer(SharpPcap.LibPcap.LibPcapLiveDevice dev, bool sniff_additional_broadcast, bool use_special_mac_filter, bool only_forward_special_macs)
        {
            this.pdev_sniff_additional_broadcast = sniff_additional_broadcast;
            this.pdev_filter_use_special_macs = use_special_mac_filter;
            this.pdev_filter_only_forward_special_macs = only_forward_special_macs;
            injected_macs_hash.Capacity = 10;
            sniffed_macs_hash.Capacity = 10;
            sniffed_macs.Capacity = 10;

            node_list = FormMain.node_list;

            this.pdev = dev;
            pdev.OnPacketArrival +=
                new PacketArrivalEventHandler(OnPacketArrival);
            pdev.Open(DeviceMode.Promiscuous, readTimeoutMilliseconds);
            setPdevFilter();

            if (System.Environment.OSVersion.Platform == PlatformID.Win32NT && pdev is SharpPcap.WinPcap.WinPcapDevice)
                ((SharpPcap.WinPcap.WinPcapDevice)pdev).MinToCopy = 10;

            FormMain.addMessage(" - sniffer created on device " + pdev.Description);

            dispatcher_thread = new Thread(new ThreadStart(dispatcher));
            dispatcher_thread.IsBackground = true;
            dispatcher_thread.Priority = ThreadPriority.AboveNormal;
            dispatcher_thread.Start();
        }

        public void start_capture()
        {
#if DEBUG
            FormMain.addMessage(" - start capturing packets");
#endif
            pdev.StartCapture();
        }

        public void stop_capture()
        {
            if (pdev.Started)
            {
                try
                {
                    pdev.StopCapture();
                }
                catch (PcapException)
                { }
            }
        }

        public void close()
        {
            stop_capture();
            if (pdev.Opened)
            {
                try
                {
                    pdev.Close();
                }
                catch (PcapException)
                { }
            }
            exiting = true;
            lock (packets)
                Monitor.PulseAll(packets);
            if (dispatcher_thread.ThreadState != ThreadState.Stopped )
                dispatcher_thread.Join();
        }

        private static void OnPacketArrival(object sender, CaptureEventArgs packet)
        {
            try
            {
                lock (xbs_sniffer.packets)
                {
                    xbs_sniffer.packets.Enqueue(packet.Packet);
                    Monitor.PulseAll(packets);
                }
            }
            catch (InvalidOperationException)
            {
                FormMain.addMessage("!! InvalidOperationException in sniffer (OnPacketArrival)!");
                return;
            }
        }

        public void dispatcher()
        {
            FormMain.addMessage(" - sniffer dispatcher thread starting...");
            int count = 0;
            RawPacket p = null;

#if !DEBUG
            try
            {
#endif
                // loop dispatcher thread until exiting flag is raised
                while (exiting == false)
                {
                    lock (packets)
                        count = packets.Count;

                    // dispatch all packets in queue
                    while (count > 0 && exiting == false)
                    {
                        lock (packets)
                            p = packets.Dequeue();
                        dispatch_packet(ref p);
                        lock (packets)
                            count = packets.Count;
                    }

                    // goto sleep until new packets arrive
                    if (!exiting)
                        lock (packets)
                            Monitor.Wait(packets);
                }
#if !DEBUG
            }
            catch (Exception ex)
            {
                ExceptionMessage.ShowExceptionDialog("sniffer dispatcher service", ex);
            }
#endif
        }

        public void dispatch_packet(ref RawPacket rawPacket)
        {
            byte[] src_mac = new byte[6];
            byte[] dst_mac = new byte[6];

            // copy source and destination MAC addresses from sniffed packet
            Buffer.BlockCopy(rawPacket.Data, 0, dst_mac, 0, 6);
            PhysicalAddress dstMAC = new PhysicalAddress(dst_mac);
            Buffer.BlockCopy(rawPacket.Data, 6, src_mac, 0, 6);
            PhysicalAddress srcMAC = new PhysicalAddress(src_mac);

#if DEBUG
            DebugWindow.addMessage(" - new ethernet packet from "+srcMAC+" => "+dstMAC);
#endif

            // if sniffed packet has MAC of packet we injected, discard
            lock (injected_macs_hash)
                if (injected_macs_hash.Contains(srcMAC.GetHashCode()))
                    return;
            
            // count the sniffed packets from local xboxs
            xbs_sniffer_statistics.increase_packet_count();

            // find node with destination MAC Address in network and send packet
            node_list.distributeDataPacket(dstMAC, rawPacket.Data);

            int srcMac_hash = srcMAC.GetHashCode();
            lock (sniffed_macs_hash)
            {
                if (!sniffed_macs_hash.Contains(srcMac_hash))
                {
                    sniffed_macs_hash.Add(srcMac_hash);
                    lock (sniffed_macs)
                        sniffed_macs.Add(srcMAC);
                }
            }
        }

        public void injectRemotePacket(ref byte[] data, PhysicalAddress dstMAC, PhysicalAddress srcMAC)
        {
            int srcMac_hash = srcMAC.GetHashCode();
            // collect all injected source MACs. sniffer needs this to filter packets out
            lock (injected_macs_hash)
                if (!injected_macs_hash.Contains(srcMac_hash))
                {
                    injected_macs_hash.Add(srcMac_hash);
                    addMacToSniffedPacketFilter(srcMAC);
                }
            // inject tha packet 
            try
            {
                pdev.SendPacket(data, data.Length);
            }
            catch (PcapException pex)
            {
                FormMain.addMessage("!! error while injecting packet from "+srcMAC+" to "+dstMAC+" ("+data.Length+") : "+pex.Message);
            }
            catch (ArgumentException aex)
            {
                FormMain.addMessage("!! error while injecting packet from " + srcMAC + " to " + dstMAC + " (" + data.Length + ") : " + aex.Message);
            }
        }

        public PhysicalAddress[] getSniffedMACs()
        {
            PhysicalAddress[] mac_array;
            lock (sniffed_macs)
            {
                mac_array = new PhysicalAddress[ sniffed_macs.Count ];
                sniffed_macs.CopyTo(mac_array);
            }
            return mac_array;
        }

        public void clearSniffedMACs()
        {
            lock (pdev_filter_sniffed_macs)
                pdev_filter_sniffed_macs.Clear();
        }

        public void addMacToSniffedPacketFilter(PhysicalAddress mac)
        {
            lock (pdev_filter_sniffed_macs)
                if (!pdev_filter_sniffed_macs.Contains(mac))
                    pdev_filter_sniffed_macs.Add(mac);
            setPdevFilter();
        }

        public void addMacToSpecialPacketFilter(PhysicalAddress mac)
        {
            lock (pdev_filter_special_macs)
                if (!pdev_filter_special_macs.Contains(mac))
                    pdev_filter_special_macs.Add(mac);
            setPdevFilter();
        }

        public void removeMacFromSpecialPacketFilter(PhysicalAddress mac)
        {
            lock (pdev_filter_special_macs)
            {
                if (pdev_filter_special_macs.Contains(mac))
                    pdev_filter_special_macs.Remove(mac);
            }
            setPdevFilter();
        }

        public static String PhysicalAddressToString(PhysicalAddress mac)
        {
            return BitConverter.ToString(mac.GetAddressBytes()).Replace('-',':');
        }

        public void setPdevFilter()
        {
            String filter_sniffed_macs = "";
            String filter_special_macs = "";
            lock (pdev_filter_sniffed_macs)
                foreach (PhysicalAddress mac in pdev_filter_sniffed_macs)
                    filter_sniffed_macs += " or ether host " + PhysicalAddressToString(mac);
            if (pdev_filter_use_special_macs)
                lock (pdev_filter_special_macs)
                {
                    if (pdev_filter_special_macs.Count > 0)
                    {
                        filter_special_macs = (pdev_filter_only_forward_special_macs == false) ? " or ether host " : "ether host ";
                        filter_special_macs += String.Join(" or ether host ", pdev_filter_special_macs.ConvertAll<string>(delegate(PhysicalAddress pa) { return PhysicalAddressToString(pa); }).ToArray());
                    }
                }
            try
            {
                String f = (pdev_sniff_additional_broadcast ? pdev_filter_all_broadcast : pdev_filter) + filter_special_macs + filter_sniffed_macs;
                if (pdev_filter_use_special_macs && pdev_filter_only_forward_special_macs && filter_special_macs.Length>0)
                    f = filter_special_macs;
#if DEBUG
                FormMain.addMessage(" - pdev filter: " + f);
#endif
                pdev.Filter = f;
            }
            catch (PcapException)
            {
                FormMain.addMessage("!! - ERROR setting pdev filter");
            }
        }

        public void setSpecialMacPacketFilter(List<PhysicalAddress> mac_list)
        {
            lock (pdev_filter_special_macs)
                pdev_filter_special_macs = mac_list;
            setPdevFilter();
        }
    }
}
