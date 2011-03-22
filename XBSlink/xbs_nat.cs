using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using PacketDotNet;
using PacketDotNet.Utils;
using MiscUtil.Conversion;

namespace XBSlink
{
    class xbs_nat_entry
    {
        public IPAddress original_source_ip;
        public byte[] original_source_ip_bytes;
        public IPAddress natted_source_ip;
        public byte[] natted_source_ip_bytes;
        public PhysicalAddress source_mac;

        public xbs_nat_entry(PhysicalAddress mac, IPAddress original_ip, IPAddress natted_ip)
        {
            this.original_source_ip = original_ip;
            this.original_source_ip_bytes = original_ip.GetAddressBytes();
            this.natted_source_ip = natted_ip;
            this.natted_source_ip_bytes = natted_ip.GetAddressBytes();
            this.source_mac = mac;
        }
    }

    class xbs_nat
    {
        public volatile bool NAT_enabled = false;

        private const int HEADER_TYPE_OFFSET = 12;
        private const int IP_HEADER_SOURCE_OFFSET = 26;
        private const int IP_HEADER_DESTINATION_OFFSET = 30;
        private const int ARP_HEADER_SOURCE_OFFSET = 28;
        private const int ARP_HEADER_DESTINATION_OFFSET = 38;
        private static byte[] broadcast_mac_bytes = new byte[6] { 255, 255, 255, 255, 255, 255 };
        private static PhysicalAddress broadcast_mac = new PhysicalAddress(broadcast_mac_bytes);

        private Queue<IPAddress> ip_pool = new Queue<IPAddress>();
        private IPAddress pool_start = null;
        private IPAddress pool_end = null;

        private Dictionary<PhysicalAddress, xbs_nat_entry> NAT_list = new Dictionary<PhysicalAddress, xbs_nat_entry>();

        public xbs_nat()
        {
        }
         
        public void fillIPPool(IPAddress start, IPAddress end)
        {
            byte[] data;
            data = start.GetAddressBytes();
            UInt32 range_start = EndianBitConverter.Big.ToUInt32( data, 0 );
            data = end.GetAddressBytes();
            UInt32 range_stop = EndianBitConverter.Big.ToUInt32(data, 0);
            if ( range_start <= range_stop)
            {
                lock (ip_pool)
                {
                    pool_start = start;
                    pool_end = end;
                    IPAddress ip;
                    for (UInt32 count = range_start; count <= range_stop; count++)
                    {
                        ip = new IPAddress(EndianBitConverter.Big.GetBytes(count));
                        ip_pool.Enqueue(ip);
                    }
                    xbs_messages.addDebugMessage("% filles NAT IP pool with " + ip_pool.Count + " ip addresses");
                }
            }
        }

        private IPAddress requestNewIpfromPool()
        {
            IPAddress ip = null;
            lock (ip_pool)
            {
                if (ip_pool.Count > 0)
                {
                    ip = ip_pool.Dequeue();
#if DEBUG
                    xbs_messages.addDebugMessage("% assigned new NAT ip from Pool : " + ip + " - " + ip_pool.Count + " left");
#endif
                }
                else
                {
#if DEBUG
                    xbs_messages.addDebugMessage("!! % could not assigned new NAT ip from Pool! no IPs left in pool");
#endif
                }
            }
            return ip;
        }

        private void freeRequestedIP(IPAddress ip)
        {
            lock (ip_pool)
                ip_pool.Enqueue(ip);
        }

        public void NAT_incoming_packet(ref byte[] data, PhysicalAddress dstMAC, PhysicalAddress srcMAC)
        {
            if (!NAT_enabled)
                return;
            EthernetPacketType ethernet_packet_type = getEthernetPacketType(ref data);
            if (!isIpOrArpPacket(ethernet_packet_type))
                return;
            xbs_nat_entry nat_entry = null;
            lock (NAT_list)
            {
                if (!NAT_list.ContainsKey(srcMAC))
                {
                    IPAddress sourceIP = getSourceIPFromRawPacketData(ref data, ethernet_packet_type);
                    IPAddress destinationIP = getDestinationIPFromRawPacketData(ref data, ethernet_packet_type);
                    IPAddress natted_sourceIP = requestNewIpfromPool();
                    nat_entry = new xbs_nat_entry(srcMAC, sourceIP, natted_sourceIP);
                    NAT_list.Add(srcMAC, nat_entry);
#if DEBUG
                    xbs_messages.addDebugMessage("% new device in NAT list: " + srcMAC + " " + nat_entry.original_source_ip + "=>" + nat_entry.natted_source_ip);
#endif
                }
                else
                {
                    nat_entry = NAT_list[srcMAC];
#if DEBUG
                    xbs_messages.addDebugMessage("% found device in NAT list: " + srcMAC + " " + nat_entry.original_source_ip + "=>" + nat_entry.natted_source_ip);
#endif
                }
            }
            replaceSourceIpWithNATSourceIP(ref data, ethernet_packet_type, ref nat_entry);
        }

        public void deNAT_outgoing_packet(ref byte[] data, PhysicalAddress dstMAC, PhysicalAddress srcMAC)
        {
            if (!NAT_enabled)
                return;
            EthernetPacketType ethernet_packet_type = getEthernetPacketType(ref data);
            if (!isIpOrArpPacket(ethernet_packet_type))
                return;
            xbs_nat_entry nat_entry = null;
            lock (NAT_list)
            {
                if (NAT_list.ContainsKey(dstMAC))
                    nat_entry = NAT_list[dstMAC];
            }

            if (nat_entry != null)
                replaceDestinationIpWithOriginalIP(ref data, ethernet_packet_type, ref nat_entry);
        }

        private IPAddress getSourceIPFromRawPacketData(ref byte[] data, EthernetPacketType ethernet_packet_type)
        {
            int offset = (ethernet_packet_type == EthernetPacketType.Arp) ?  ARP_HEADER_SOURCE_OFFSET : IP_HEADER_SOURCE_OFFSET;
            return IpPacket.GetIPAddress(System.Net.Sockets.AddressFamily.InterNetwork, offset, data );
        }

        private IPAddress getDestinationIPFromRawPacketData(ref byte[] data, EthernetPacketType ethernet_packet_type)
        {
            int offset = (ethernet_packet_type == EthernetPacketType.Arp) ? ARP_HEADER_DESTINATION_OFFSET : IP_HEADER_DESTINATION_OFFSET;
            return IpPacket.GetIPAddress(System.Net.Sockets.AddressFamily.InterNetwork, offset, data);
        }

        private bool isIpOrArpPacket(EthernetPacketType ethernet_packet_type)
        {
            return (ethernet_packet_type == EthernetPacketType.IpV4 || ethernet_packet_type == EthernetPacketType.Arp);
        }

        private EthernetPacketType getEthernetPacketType(ref byte[] data)
        {
            return (EthernetPacketType)EndianBitConverter.Big.ToInt16(data, HEADER_TYPE_OFFSET);
        }

        private void replaceSourceIpWithNATSourceIP( ref byte[] data, EthernetPacketType ethernet_packet_type, ref xbs_nat_entry nat_entry)
        {
            int offset = (ethernet_packet_type == EthernetPacketType.Arp) ? ARP_HEADER_SOURCE_OFFSET : IP_HEADER_SOURCE_OFFSET;
            Buffer.BlockCopy(nat_entry.natted_source_ip_bytes, 0, data, offset, 4);
        }

        private void replaceDestinationIpWithOriginalIP(ref byte[] data, EthernetPacketType ethernet_packet_type, ref xbs_nat_entry nat_entry)
        {
            int offset = (ethernet_packet_type == EthernetPacketType.Arp) ? ARP_HEADER_DESTINATION_OFFSET : IP_HEADER_DESTINATION_OFFSET;
            Buffer.BlockCopy(nat_entry.natted_source_ip_bytes, 0, data, offset, 4);
        }
    }
}
