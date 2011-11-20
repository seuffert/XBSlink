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
        public static volatile UInt32 packet_count = 0;
        private static Object _locker = new Object();
        public static UInt64 NAT_timeInCode = 0;
        public static UInt32 NAT_callCount = 0;
        public static UInt64 deNAT_timeInCode = 0;
        public static UInt32 deNAT_callCount = 0;
    }

    class xbs_sniffer
    {
        private SharpPcap.LibPcap.LibPcapLiveDevice pdev = null;
        public int readTimeoutMilliseconds = 1000;
        public bool pdev_filter_use_special_macs = true;
        public bool pdev_filter_only_forward_special_macs = true;
        public bool pdev_filter_wellknown_ports = true;
        public bool pdev_filter_exclude_gatway_ips = true;
        public bool pdev_filter_forward_high_port_broadcasts = false;

        private const String pdev_filter_template_include = "{include_filters}";
        private const String pdev_filter_template_exclude = "{exlude_filters}";
        private String pdev_filter_template = "(" + pdev_filter_template_include + ") and not (" + pdev_filter_template_exclude + ")";
        private String pdev_filter_template_only_include = "(" + pdev_filter_template_include + ")";
        private const String pdev_filter_gameconsoles = "(udp and ((ip host 0.0.0.1) or (dst portrange 3074-3075) or (dst portrange 14000-14001))) ";
        private String pdev_filter_gateways = "";
        private String pdev_filter_fallback = pdev_filter_gameconsoles;

        private List<IPAddress> gateway_ips = new List<IPAddress>();

        private List<PhysicalAddress> pdev_filter_known_macs_from_remote_nodes = new List<PhysicalAddress>();
        private List<PhysicalAddress> pdev_filter_special_macs = new List<PhysicalAddress>();

        private Thread dispatcher_thread = null;
        private volatile bool exiting = false;

        public static Queue<SharpPcap.RawCapture> packets = new Queue<SharpPcap.RawCapture>();

        private List<int> injected_macs_hash = new List<int>();
        private List<int> sniffed_macs_hash = new List<int>();
        private List<PhysicalAddress> sniffed_macs = new List<PhysicalAddress>();

        private xbs_node_list node_list = null;
        private xbs_nat NAT = null;

        public xbs_sniffer(ICaptureDevice dev, bool use_special_mac_filter, List<PhysicalAddress> filter_special_macs, bool only_forward_special_macs, xbs_node_list node_list, xbs_nat NAT, GatewayIPAddressInformationCollection gateways, bool exclude_gateway_ips)
        {
            this.NAT = NAT;
            this.pdev_filter_use_special_macs = use_special_mac_filter;
            if (filter_special_macs!=null)
                this.pdev_filter_special_macs = filter_special_macs;
            this.pdev_filter_only_forward_special_macs = only_forward_special_macs;
            this.pdev_filter_exclude_gatway_ips = exclude_gateway_ips;
            create_gateway_filter(gateways);
            injected_macs_hash.Capacity = 40;
            sniffed_macs_hash.Capacity = 10;
            sniffed_macs.Capacity = 10;

            this.node_list = node_list;

            if (!(dev is SharpPcap.LibPcap.LibPcapLiveDevice))
                throw new ArgumentException("pcap caputure device is not a LibPcapLiveDevice");
            this.pdev = (SharpPcap.LibPcap.LibPcapLiveDevice)dev;
            pdev.OnPacketArrival +=
                new PacketArrivalEventHandler(OnPacketArrival);
            pdev.Open(DeviceMode.Promiscuous, readTimeoutMilliseconds);
            setPdevFilter();

            if (System.Environment.OSVersion.Platform == PlatformID.Win32NT && pdev is SharpPcap.WinPcap.WinPcapDevice)
                ((SharpPcap.WinPcap.WinPcapDevice)pdev).MinToCopy = 10;

            xbs_messages.addInfoMessage(" - sniffer created on device " + pdev.Description, xbs_message_sender.SNIFFER);

            dispatcher_thread = new Thread(new ThreadStart(dispatcher));
            dispatcher_thread.IsBackground = true;
            dispatcher_thread.Priority = ThreadPriority.AboveNormal;
            dispatcher_thread.Start();
        }

        private void create_gateway_filter( GatewayIPAddressInformationCollection gateways )
        {
            if (gateways == null)
                return;
            if (gateways.Count == 0)
                return;
            String[] ips = new String[gateways.Count];
            for (int i = 0; i < gateways.Count; i++)
            {
                ips[i] = gateways[i].Address.ToString();
                gateway_ips.Add(gateways[i].Address);
            }
            pdev_filter_gateways = "host " + String.Join(" or host ", ips);
        }

        public void start_capture()
        {
#if DEBUG
            xbs_messages.addInfoMessage(" - start capturing packets", xbs_message_sender.SNIFFER);
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
            if (dispatcher_thread.ThreadState != System.Threading.ThreadState.Stopped )
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
                xbs_messages.addInfoMessage("!! InvalidOperationException in sniffer (OnPacketArrival)!", xbs_message_sender.SNIFFER, xbs_message_type.FATAL_ERROR);
                return;
            }
        }

        public void dispatcher()
        {
            xbs_messages.addInfoMessage(" - sniffer dispatcher thread starting...", xbs_message_sender.SNIFFER);
            int count = 0;
            RawCapture p = null;

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

        public void dispatch_packet(ref RawCapture rawPacket)
        {
            byte[] src_mac = new byte[6];
            byte[] dst_mac = new byte[6];
            byte[] packet_data = rawPacket.Data;

            // copy source and destination MAC addresses from sniffed packet
            Buffer.BlockCopy(rawPacket.Data, 0, dst_mac, 0, 6);
            PhysicalAddress dstMAC = new PhysicalAddress(dst_mac);
            Buffer.BlockCopy(rawPacket.Data, 6, src_mac, 0, 6);
            PhysicalAddress srcMAC = new PhysicalAddress(src_mac);

#if DEBUG
            xbs_messages.addDebugMessage("s> " + srcMAC + "=>" + dstMAC + "Len:" + rawPacket.Data.Length, xbs_message_sender.SNIFFER);
            Packet p = Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
            xbs_messages.addDebugMessage("s> " + p, xbs_message_sender.SNIFFER);
#endif

            // if sniffed packet has MAC of packet we injected, discard
            bool is_injected_packet = false;
            lock (injected_macs_hash)
                is_injected_packet = injected_macs_hash.Contains(srcMAC.GetHashCode());
            if (is_injected_packet) 
                return;

            if (NAT.NAT_enabled)
            {
#if DEBUG
                System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
                stopWatch.Start();
#endif
                EthernetPacketType p_type = NAT.deNAT_outgoing_packet_PacketDotNet(ref packet_data, dstMAC, srcMAC);
#if DEBUG
                stopWatch.Stop();
                if (p_type == EthernetPacketType.IpV4)
                {
                    xbs_sniffer_statistics.deNAT_callCount++;
                    if (xbs_sniffer_statistics.deNAT_callCount > 1)
                    {
                        xbs_sniffer_statistics.deNAT_timeInCode += (UInt64)stopWatch.ElapsedTicks;
                        UInt32 average = (UInt32)(xbs_sniffer_statistics.deNAT_timeInCode / (xbs_sniffer_statistics.deNAT_callCount - 1));
                        double average_ms = new TimeSpan(average).TotalMilliseconds;
                        xbs_messages.addDebugMessage("- deNAT time: " + stopWatch.ElapsedTicks + " deNAT count: " + (xbs_sniffer_statistics.deNAT_callCount - 1) + " Total Time: " + xbs_sniffer_statistics.deNAT_timeInCode + "=> " + average + " / " + average_ms + "ms", xbs_message_sender.SNIFFER);
                    }
                }
                p = Packet.ParsePacket(rawPacket.LinkLayerType, packet_data);
                xbs_messages.addDebugMessage("s> " + p, xbs_message_sender.SNIFFER);
#endif
            }

            // count the sniffed packets from local xboxs
            xbs_sniffer_statistics.packet_count++;

            // find node with destination MAC Address in network and send packet
            node_list.distributeDataPacket(dstMAC, packet_data);

            int srcMac_hash = srcMAC.GetHashCode();
            bool pdevfilter_needs_change = false;
            lock (sniffed_macs_hash)
            {
                if (!sniffed_macs_hash.Contains(srcMac_hash))
                {
                    sniffed_macs_hash.Add(srcMac_hash);
                    lock (sniffed_macs)
                        sniffed_macs.Add(srcMAC);
                    pdevfilter_needs_change = true;
                }
            }
            if (pdevfilter_needs_change)
                setPdevFilter();
        }

        public void injectRemotePacket(ref byte[] data, PhysicalAddress dstMAC, PhysicalAddress srcMAC)
        {
            Packet p = null;
            ARPPacket p_arp = null;
            IpPacket p_ipv4 = null;
            ICMPv4Packet p_icmp = null;
            UdpPacket p_udp = null;
            TcpPacket p_tcp = null;

            int srcMac_hash = srcMAC.GetHashCode();
            // collect all injected source MACs. sniffer needs this to filter packets out
            lock (injected_macs_hash)
            {
                if (!injected_macs_hash.Contains(srcMac_hash))
                {
                    injected_macs_hash.Add(srcMac_hash);
                    addMacToKnownMacListFromRemoteNodes(srcMAC);
                }
            }

            try
            {
                p = Packet.ParsePacket(LinkLayers.Ethernet, data);
            }
            catch (PcapException pcex)
            {
#if DEBUG
                xbs_messages.addDebugMessage("parse packet failed in injectRemotePacket (1): " + pcex.ToString(), xbs_message_sender.SNIFFER, xbs_message_type.ERROR);
#endif
                return;
            }
            catch (NotImplementedException niex)
            {
#if DEBUG
                xbs_messages.addDebugMessage("parse packet failed in injectRemotePacket (2): " + niex.ToString(), xbs_message_sender.SNIFFER, xbs_message_type.ERROR);
#endif
                return;
            }

            // DETERMINE PACKET TYPE
            if (p.PayloadPacket is IPv4Packet)
                p_ipv4 = p as IPv4Packet;
            else if (p.PayloadPacket is ARPPacket)
                p_arp = p as ARPPacket;
            else if (p.PayloadPacket is ICMPv4Packet)
                p_icmp = p as ICMPv4Packet;
            else
            {
                // UNKNOWN OR UNSUPPORTED PACKET TYPE
#if DEBUG
                xbs_messages.addDebugMessage("unknown incoming packet type: " + p.ToString(), xbs_message_sender.SNIFFER, xbs_message_type.WARNING);
#endif
                return;
            }

            // FILTER ARP PACKETS
            if (p_arp != null)
            {
                // FILTER ARP PACKETS TO OR FROM GATEWAY IPs
                foreach (IPAddress ip in gateway_ips)
                {
                    if (p_arp.TargetProtocolAddress.Equals(ip))
                        return;
                    else if (p_arp.SenderProtocolAddress.Equals(ip))
                        return;
                }
            }

            // FILTER IPv4 PACKETS
            if (p_ipv4 != null)
            {
                // FILTER IP PACKETS TO OR FROM GATEWAY IPs
                if (pdev_filter_exclude_gatway_ips)
                {
                    foreach (IPAddress ip in gateway_ips)
                    {
                        if (p_ipv4.DestinationAddress.Equals(ip))
                            return;
                        else if (p_ipv4.SourceAddress.Equals(ip))
                            return;
                    }
                }

                if (p_ipv4.PayloadPacket is UdpPacket)
                    p_udp = p_ipv4.PayloadPacket as UdpPacket;
                else if (p_ipv4.PayloadPacket is TcpPacket)
                    p_tcp = p_ipv4.PayloadPacket as TcpPacket;
            }

            // FILTER UDP PACKETS
            if (p_udp != null)
            {
                // FILTER UDP PACKETS TO WELL KNOWN PORTS
                if (pdev_filter_wellknown_ports && p_udp.DestinationPort > 1023)
                    return;
            }
            // FILTER TCP PACKETS
            if (p_tcp != null)
            {
                // FILTER TCP PACKETS TO WELL KNOWN PORTS
                if (pdev_filter_wellknown_ports && p_tcp.DestinationPort > 1023)
                    return;
            }
#if DEBUG
            xbs_messages.addDebugMessage("i> " + p, xbs_message_sender.SNIFFER);
#endif

            if (NAT.NAT_enabled)
            {
#if DEBUG
                System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
                stopWatch.Start();
#endif
                EthernetPacketType p_type = NAT.NAT_incoming_packet_PacketDotNet(ref data, dstMAC, srcMAC);
#if DEBUG
                stopWatch.Stop();
                if (p_type == EthernetPacketType.IpV4)
                {
                    xbs_sniffer_statistics.NAT_callCount++;
                    if (xbs_sniffer_statistics.NAT_callCount > 1)
                    {
                        xbs_sniffer_statistics.NAT_timeInCode += (UInt64)stopWatch.ElapsedTicks;
                        UInt32 average = (UInt32)(xbs_sniffer_statistics.NAT_timeInCode / (xbs_sniffer_statistics.NAT_callCount - 1));
                        double average_ms = new TimeSpan(average).TotalMilliseconds;
                        xbs_messages.addDebugMessage("- NAT time: " + stopWatch.ElapsedTicks + "t/" + stopWatch.ElapsedMilliseconds + "ms | NAT count: " + (xbs_sniffer_statistics.NAT_callCount - 1) + " Total Time: " + xbs_sniffer_statistics.NAT_timeInCode + "t=> Average " + average + "t / " + average_ms + "ms", xbs_message_sender.SNIFFER);
                    }
                }
                p = Packet.ParsePacket(LinkLayers.Ethernet, data);
                xbs_messages.addDebugMessage("i> " + p, xbs_message_sender.SNIFFER);
#endif
            }

            // inject the packet 
            try
            {
                pdev.SendPacket(data, data.Length);
            }
            catch (PcapException pex)
            {
                xbs_messages.addInfoMessage("!! error while injecting packet from " + srcMAC + " to " + dstMAC + " (" + data.Length + ") : " + pex.Message, xbs_message_sender.SNIFFER, xbs_message_type.FATAL_ERROR);
            }
            catch (ArgumentException aex)
            {
                xbs_messages.addInfoMessage("!! error while injecting packet from " + srcMAC + " to " + dstMAC + " (" + data.Length + ") : " + aex.Message, xbs_message_sender.SNIFFER, xbs_message_type.FATAL_ERROR);
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

        public void clearKnownMACsFromRemoteNodes()
        {
            lock (pdev_filter_known_macs_from_remote_nodes)
                pdev_filter_known_macs_from_remote_nodes.Clear();
        }

        public void addMacToKnownMacListFromRemoteNodes(PhysicalAddress mac)
        {
            lock (pdev_filter_known_macs_from_remote_nodes)
                if (!pdev_filter_known_macs_from_remote_nodes.Contains(mac))
                    pdev_filter_known_macs_from_remote_nodes.Add(mac);
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
            List<String> exclude_filter_list = new List<string>();
            List<String> include_filter_list = new List<string>();

            // always exclude local sniffing interface
            String local_mac = PhysicalAddressToString(pdev.MacAddress);
            if (local_mac.Length>0)
                exclude_filter_list.Add("ether host "+PhysicalAddressToString(pdev.MacAddress));
            // exclude gatway IPs, just to be on the safe side
            if (pdev_filter_exclude_gatway_ips && pdev_filter_gateways.Length > 0)
                exclude_filter_list.Add(pdev_filter_gateways);
            // exclude well known ports, just to be on the safe side
            if (pdev_filter_wellknown_ports)
                exclude_filter_list.Add("ip and (dst portrange 1-1023 or src portrange 1-1023)");
            // include packets to game console specific ports/IPs
            if (!(pdev_filter_use_special_macs && pdev_filter_only_forward_special_macs))
            {
                include_filter_list.Add(pdev_filter_gameconsoles);
                if (pdev_filter_forward_high_port_broadcasts)
                    include_filter_list.Add("ether dst ff:ff:ff:ff:ff:ff and udp and dst portrange 1024-65535");
            }

            // include packets TO MACs from remote users
            // exclude packets FROM MACs from remote users
            lock (pdev_filter_known_macs_from_remote_nodes)
            {
                if ((pdev_filter_known_macs_from_remote_nodes.Count > 0))
                {
                    // we want all packets send to MACs we know of from other XBSlink nodes
                    if (!(pdev_filter_use_special_macs && pdev_filter_only_forward_special_macs))
                        include_filter_list.Add( "ether dst " + String.Join(" or ether dst ", pdev_filter_known_macs_from_remote_nodes.ConvertAll<string>(delegate(PhysicalAddress pa) { return PhysicalAddressToString(pa); }).ToArray()) );
                    // we do NOT want packets injected by us, send from other other nodes to our network
                    exclude_filter_list.Add( "ether src " + String.Join(" or ether src ", pdev_filter_known_macs_from_remote_nodes.ConvertAll<string>(delegate(PhysicalAddress pa) { return PhysicalAddressToString(pa); }).ToArray()) );
                }
            }
            // include special MACs provided by the user
            if (pdev_filter_use_special_macs)
                lock (pdev_filter_special_macs)
                    if (pdev_filter_special_macs.Count > 0)
                        include_filter_list.Add("ether src " + String.Join(" or ether src ", pdev_filter_special_macs.ConvertAll<string>(delegate(PhysicalAddress pa) { return PhysicalAddressToString(pa); }).ToArray()));
            // include packets from already known/sniffed devices
            lock (sniffed_macs)
                if ((sniffed_macs.Count > 0) && !(pdev_filter_use_special_macs && pdev_filter_only_forward_special_macs))
                    include_filter_list.Add( "ether src " + String.Join(" or ether src ", sniffed_macs.ConvertAll<string>(delegate(PhysicalAddress pa) { return PhysicalAddressToString(pa); }).ToArray()) );

            String exlude_filter_string = "( "+ String.Join(" ) or (", exclude_filter_list.ToArray()) + " ) ";
            String include_filter_string = "( " + String.Join(" ) or (", include_filter_list.ToArray()) + " ) ";

            String f = (exclude_filter_list.Count>0) ? pdev_filter_template : pdev_filter_template_only_include;
            f = f.Replace(pdev_filter_template_include, include_filter_string);
            if (exclude_filter_list.Count>0)
                f = f.Replace(pdev_filter_template_exclude, exlude_filter_string);
#if DEBUG
            xbs_messages.addInfoMessage("- pdev filter: " + f, xbs_message_sender.SNIFFER);
#endif
            try
            {
                    pdev.Filter = f;
            }
            catch (PcapException)
            {
                xbs_messages.addInfoMessage("!! - ERROR setting pdev filter, using fallback - please inform developer. pdev_filter: "+f, xbs_message_sender.SNIFFER, xbs_message_type.FATAL_ERROR);
                pdev.Filter = pdev_filter_fallback;
            }
        }

        public void setSpecialMacPacketFilter(List<PhysicalAddress> mac_list)
        {
            lock (pdev_filter_special_macs)
                pdev_filter_special_macs = mac_list;
            setPdevFilter();
        }

        public static xbs_sniffer getInstance()
        {
            return (FormMain.sniffer != null) ? FormMain.sniffer : xbs_console_app.sniffer;
        }
    }
}