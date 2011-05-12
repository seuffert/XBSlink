using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Linq;
using PacketDotNet;
using PacketDotNet.Utils;
using MiscUtil.Conversion;
using System.Text.RegularExpressions;

namespace XBSlink
{
    class xbs_nat_entry
    {
        volatile public IPAddress original_source_ip;
        public byte[] original_source_ip_bytes;
        volatile public IPAddress natted_source_ip;
        public byte[] natted_source_ip_bytes;
        volatile public IPAddress natted_source_ip_netmask;
        public byte[] natted_source_ip_netmask_bytes;
        volatile public PhysicalAddress source_mac;
        public DateTime last_change_time = DateTime.Now;
        volatile public IPAddress natted_broadcast;
        public byte[] natted_broadcast_bytes;

        public xbs_nat_entry(PhysicalAddress mac, IPAddress original_ip, IPAddress natted_ip, IPAddress netmask)
        {
            this.original_source_ip = original_ip;
            if (original_ip!=null)
                this.original_source_ip_bytes = original_ip.GetAddressBytes();
            this.natted_source_ip = natted_ip;
            if (natted_ip!=null)
                this.natted_source_ip_bytes = natted_ip.GetAddressBytes();
            this.natted_source_ip_netmask = netmask;
            if (netmask != null)
                this.natted_source_ip_netmask_bytes = netmask.GetAddressBytes();
            this.source_mac = mac;
            if (netmask != null && natted_ip != null)
            {
                this.natted_broadcast = xbs_nat.calculateBroadcastFromIPandNetmask(natted_ip, netmask);
                this.natted_broadcast_bytes = this.natted_broadcast.GetAddressBytes();
            }
        }
    }

    class xbs_nat_ippool
    {
        private List<xbs_nat_entry> ip_pool = new List<xbs_nat_entry>();
        public int Count { get { lock (ip_pool) { return ip_pool.Count; } } }
        public volatile int CountOfUsedIPs = 0;
        private IPAddress _tmpIP = null;
        private DateTime _last_update = DateTime.Now;
        public DateTime last_update { get { lock (ip_pool) { return _last_update; } } private set { lock (ip_pool) { _last_update = value; } } }

        public xbs_nat_ippool()
        {
        }

        public int addIPRangeToPool(IPAddress start, IPAddress end, IPAddress netmask)
        {
            byte[] data;
            data = start.GetAddressBytes();
            UInt32 range_start = EndianBitConverter.Big.ToUInt32( data, 0 );
            data = end.GetAddressBytes();
            UInt32 range_stop = EndianBitConverter.Big.ToUInt32(data, 0);
            int count = 0;
            if (range_start <= range_stop)
            {
                IPAddress ip;
                lock (ip_pool)
                {
                    for (UInt32 i = range_start; i <= range_stop; i++)
                    {
                        ip = new IPAddress(EndianBitConverter.Big.GetBytes(i));
                        if (!hasNattedIP(ip))
                        {
                            ip_pool.Add(new xbs_nat_entry(null, null, ip, netmask));
                            count++;
                        }
                    }
                }
                xbs_messages.addDebugMessage("% NAT IP pool filled with " + count + " ip addresses. Total count of IPs in pool: " + ip_pool.Count, xbs_message_sender.NAT);
            }
            return count;
        }

        public bool addIPToPool(String IPstr, String NetmaskString)
        {
            IPAddress IP;
            IPAddress netmask;
            if (!IPAddress.TryParse(IPstr, out IP))
                return false;
            if (!IPAddress.TryParse(NetmaskString, out netmask))
                return false;
            if (!hasNattedIP(IP))
                ip_pool.Add(new xbs_nat_entry(null, null, IP, netmask));
            return true;
        }

        public xbs_nat_entry requestIP(IPAddress originalIP, PhysicalAddress mac)
        {
            xbs_nat_entry entry = null;
            lock (ip_pool)
            {
                if (CountOfUsedIPs == ip_pool.Count)
                    return null;
                entry = ip_pool.Find(isFreeEntry);
                if (entry == null)
                    return null;
                entry.original_source_ip = originalIP;
                entry.original_source_ip_bytes = originalIP.GetAddressBytes();
                entry.source_mac = mac;
                entry.last_change_time = DateTime.Now;
                last_update = entry.last_change_time;
                CountOfUsedIPs++;
            }
            return entry;
        }

        private bool isFreeEntry(xbs_nat_entry entry)
        {
            return (entry.source_mac == null);
        }

        private bool compare_tmpIP(xbs_nat_entry entry)
        {
            bool ret = (entry.natted_source_ip.Equals(_tmpIP));
            return ret;
        }

        public void freeIP(xbs_nat_entry entry)
        {
            lock (ip_pool)
            {
                entry.original_source_ip = null;
                entry.original_source_ip_bytes = null;
                entry.source_mac = null;
                entry.last_change_time = DateTime.Now;
                last_update = entry.last_change_time;
                CountOfUsedIPs--;
            }
        }

        public bool removeIPFromPool(IPAddress ip)
        {
            _tmpIP = ip;
            lock (ip_pool)
            {
                int index = ip_pool.FindIndex(compare_tmpIP);
                if (index >= 0)
                {
                    ip_pool.RemoveAt(index);

                }
            }
            return true;
        }

        public xbs_nat_entry[] getEntriesArray()
        {
            xbs_nat_entry[] entries;
            lock (ip_pool)
                entries = ip_pool.ToArray();
            return entries;
        }

        public bool hasNattedIP(IPAddress ip)
        {
            lock (ip_pool)
            {
                foreach (xbs_nat_entry entry in ip_pool)
                    if (entry.natted_source_ip.Equals(ip))
                        return true;
            }
            return false;
        }

        public void Clear()
        {
            lock (ip_pool)
                ip_pool.Clear();
        }

        public void freeAllIPs()
        {
            if (CountOfUsedIPs == 0)
                return;
#if DEBUG
            xbs_messages.addDebugMessage("% freeing remaining NAT IPs", xbs_message_sender.NAT);
#endif
            lock (ip_pool)
            {
                for (int i = 0; i < ip_pool.Count; i++)
                    if (ip_pool[i].source_mac != null)
                        freeIP(ip_pool[i]);
            }
        }
    }

    class xbs_nat
    {
        public volatile bool NAT_enabled = false;
        public volatile bool NAT_enablePS3mode = false;

        private const int ETHERNET_HEADER_LENGTH            = 14;
        private const int ETHERNET_HEADER_TYPE_OFFSET       = 12;
        private const int ARP_HEADER_SOURCE_OFFSET          = ETHERNET_HEADER_LENGTH + 14;
        private const int ARP_HEADER_DESTINATION_OFFSET     = ETHERNET_HEADER_LENGTH + 24;
        private const int IP_HEADER_IHL_OFFSET              = ETHERNET_HEADER_LENGTH;
        private const int IP_HEADER_PROTOCOL_OFFSET         = IP_HEADER_IHL_OFFSET + 9;
        private const int IP_HEADER_CHECKSUM_OFFSET         = IP_HEADER_IHL_OFFSET + 10;
        private const int IP_HEADER_SOURCE_OFFSET           = IP_HEADER_IHL_OFFSET + 12;
        private const int IP_HEADER_DESTINATION_OFFSET      = IP_HEADER_IHL_OFFSET + 16;
        private const int UDP_HEADER_CHECKSUM_OFFSET        = 6;

        private static byte[] broadcast_mac_bytes = new byte[6] { 255, 255, 255, 255, 255, 255 };
        public static PhysicalAddress broadcast_mac = new PhysicalAddress(broadcast_mac_bytes);
        private static byte[] zero_mac_bytes = new byte[6] { 0, 0, 0, 0, 0, 0 };
        public static PhysicalAddress zero_mac = new PhysicalAddress(zero_mac_bytes);
        private IPAddress ip_zero = new IPAddress(0);
        public xbs_nat_ippool ip_pool = new xbs_nat_ippool();
        private Dictionary<PhysicalAddress, xbs_nat_entry> NAT_list = new Dictionary<PhysicalAddress, xbs_nat_entry>();

        public xbs_nat()
        {
        }
         
        /*
        public EthernetPacketType NAT_incoming_packet(ref byte[] data, PhysicalAddress dstMAC, PhysicalAddress srcMAC)
        {
            EthernetPacketType ethernet_packet_type = getEthernetPacketType(ref data);
            if (!isIpOrArpPacket(ethernet_packet_type))
                return ethernet_packet_type;
            xbs_nat_entry nat_entry;
            lock (NAT_list)
            {
                if (!NAT_list.TryGetValue(srcMAC, out nat_entry))
                {
                    IPAddress sourceIP = getSourceIPFromRawPacketData(ref data, ethernet_packet_type);
                    if (sourceIP.Equals(ip_zero))
                        return ethernet_packet_type;
                    IPAddress destinationIP = getDestinationIPFromRawPacketData(ref data, ethernet_packet_type);
                    nat_entry = ip_pool.requestIP( sourceIP, srcMAC );
                    if (nat_entry == null)
                    {
                        xbs_messages.addInfoMessage("!! % out of NAT IPs. Could not nat incoming packet");
                        return ethernet_packet_type;
                    }
                    NAT_list.Add(srcMAC, nat_entry);
#if DEBUG
                    xbs_messages.addDebugMessage("% new device in NAT list: " + srcMAC + " " + nat_entry.original_source_ip + "=>" + nat_entry.natted_source_ip);
#endif
                }
                else
                {
#if DEBUG
                    xbs_messages.addDebugMessage("% found device in NAT list: " + srcMAC + " " + nat_entry.original_source_ip + "=>" + nat_entry.natted_source_ip);
#endif
                }
            }
            replaceSourceIpWithNATSourceIP(ref data, ethernet_packet_type, ref nat_entry);
            if (ethernet_packet_type == EthernetPacketType.IpV4)
            {
                if (dstMAC.Equals(broadcast_mac) && nat_entry.natted_broadcast!=null)
                    replaceBroadcastIPAddress(ref data, ref nat_entry.natted_broadcast_bytes);
                updateIPChecksums(ref data);
            }
            return ethernet_packet_type;
        }
        */
        public EthernetPacketType NAT_incoming_packet_PacketDotNet(ref byte[] data, PhysicalAddress dstMAC, PhysicalAddress srcMAC)
        {
            EthernetPacket p = (EthernetPacket)EthernetPacket.ParsePacket(LinkLayers.Ethernet, data);
            EthernetPacketType p_type = p.Type;
            IPv4Packet p_IPV4 = null;
            ARPPacket p_ARP = null;
            if (p_type == EthernetPacketType.IpV4)
                p_IPV4 = (IPv4Packet)p.PayloadPacket;
            else if (p_type == EthernetPacketType.Arp)
                p_ARP = (ARPPacket)p.PayloadPacket;
            else
                return p_type;
            xbs_nat_entry nat_entry;
            lock (NAT_list)
            {
                if (!NAT_list.TryGetValue(srcMAC, out nat_entry))
                {
                    IPAddress sourceIP = (p_IPV4!=null) ? p_IPV4.SourceAddress : p_ARP.SenderProtocolAddress;
                    if (sourceIP.Equals(ip_zero))
                        return p_type;
                    IPAddress destinationIP = (p_IPV4 != null) ? p_IPV4.DestinationAddress : p_ARP.TargetProtocolAddress;
                    nat_entry = ip_pool.requestIP(sourceIP, srcMAC);
                    if (nat_entry == null)
                    {
#if DEBUG
                        xbs_messages.addInfoMessage("!! % out of NAT IPs. Could not nat incoming packet", xbs_message_sender.NAT, xbs_message_type.WARNING);
#endif
                        return p_type;
                    }
                    NAT_list.Add(srcMAC, nat_entry);
#if DEBUG
                    xbs_messages.addDebugMessage("% new device in NAT list: " + srcMAC + " " + nat_entry.original_source_ip + "=>" + nat_entry.natted_source_ip, xbs_message_sender.NAT);
#endif
                }
                else
                {
#if DEBUG
                    xbs_messages.addDebugMessage("% found device in NAT list: " + srcMAC + " " + nat_entry.original_source_ip + "=>" + nat_entry.natted_source_ip, xbs_message_sender.NAT);
#endif
                }
            }
            if (p_IPV4 != null)
            {
                p_IPV4.SourceAddress = nat_entry.natted_source_ip;
                if (dstMAC.Equals(broadcast_mac) && nat_entry.natted_broadcast != null)
                    p_IPV4.DestinationAddress = nat_entry.natted_broadcast;
                p_IPV4.UpdateIPChecksum();
                if (p_IPV4.Protocol == IPProtocolType.UDP)
                {
                    UdpPacket p_UDP = (UdpPacket)p_IPV4.PayloadPacket;
                    if (NAT_enablePS3mode)
                        PS3_replaceInfoResponse_hostAddr(dstMAC, ref p_UDP, nat_entry.natted_source_ip);
                    p_UDP.UpdateUDPChecksum();
                }
                else if (p_IPV4.Protocol == IPProtocolType.TCP)
                    ((TcpPacket)p_IPV4.PayloadPacket).UpdateTCPChecksum();
            }
            else
            {
                p_ARP.SenderProtocolAddress = nat_entry.natted_source_ip;
            }
            data = p.BytesHighPerformance.ActualBytes();
            return p_type;
        }

        /*
        public EthernetPacketType deNAT_outgoing_packet(ref byte[] data, PhysicalAddress dstMAC, PhysicalAddress srcMAC)
        {
            EthernetPacketType ethernet_packet_type = getEthernetPacketType(ref data);
            if (!isIpOrArpPacket(ethernet_packet_type))
                return ethernet_packet_type;
            xbs_nat_entry nat_entry = null;
            lock (NAT_list)
            {
                if (NAT_list.ContainsKey(dstMAC))
                    nat_entry = NAT_list[dstMAC];
            }

            if (nat_entry != null)
            {
                replaceDestinationIpWithOriginalIP(ref data, ethernet_packet_type, ref nat_entry);
                updateIPChecksums(ref data);
            }
            return ethernet_packet_type;
        }
        */
        public EthernetPacketType deNAT_outgoing_packet_PacketDotNet(ref byte[] data, PhysicalAddress dstMAC, PhysicalAddress srcMAC)
        {
            EthernetPacket p = (EthernetPacket)EthernetPacket.ParsePacket(LinkLayers.Ethernet, data);
            EthernetPacketType ethernet_packet_type = getEthernetPacketType(ref data);
            IPv4Packet p_IPV4 = null;
            ARPPacket p_ARP = null;
            bool packet_changed = false;
            if (ethernet_packet_type == EthernetPacketType.IpV4)
                p_IPV4 = (IPv4Packet)p.PayloadPacket;
            else if (ethernet_packet_type == EthernetPacketType.Arp)
                p_ARP = (ARPPacket)p.PayloadPacket;
            else
                return ethernet_packet_type;

            xbs_nat_entry nat_entry = null;
            lock (NAT_list)
            {
                if (dstMAC.Equals(broadcast_mac))
                {
                    if (p_ARP != null)
                    {
                        foreach (xbs_nat_entry ne in NAT_list.Values)
                        {
                            if (ne.natted_source_ip.Equals(p_ARP.TargetProtocolAddress))
                            {
                                nat_entry = ne;
                                break;
                            }
                        }
                    }
                }
                else if (NAT_list.ContainsKey(dstMAC))
                {
                    nat_entry = NAT_list[dstMAC];
                }
            }
#if DEBUG
            if (nat_entry!=null)
                xbs_messages.addDebugMessage("% found device in deNAT list: " + dstMAC + " " + nat_entry.natted_source_ip + "=>" + nat_entry.original_source_ip, xbs_message_sender.NAT);
#endif

            if (nat_entry != null)
            {
                if (p_IPV4 != null)
                {
                    p_IPV4.DestinationAddress = nat_entry.original_source_ip;
                    p_IPV4.UpdateIPChecksum();
                    if (p_IPV4.Protocol == IPProtocolType.UDP)
                    {
                        ((UdpPacket)p_IPV4.PayloadPacket).UpdateUDPChecksum();
                        packet_changed = true;
                    }
                    else if (p_IPV4.Protocol == IPProtocolType.TCP)
                    {
                        ((TcpPacket)p_IPV4.PayloadPacket).UpdateTCPChecksum();
                        packet_changed = true;
                    }
                }
                else if (p_ARP != null)
                {
                    p_ARP.TargetProtocolAddress = nat_entry.original_source_ip;
                    packet_changed = true;
                }
            }
            if (packet_changed)
                data = p.BytesHighPerformance.ActualBytes();

            return (packet_changed) ? ethernet_packet_type : EthernetPacketType.None;
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
            return (EthernetPacketType)EndianBitConverter.Big.ToInt16(data, ETHERNET_HEADER_TYPE_OFFSET);
        }

        private void replaceSourceIpWithNATSourceIP( ref byte[] data, EthernetPacketType ethernet_packet_type, ref xbs_nat_entry nat_entry)
        {
            int offset = (ethernet_packet_type == EthernetPacketType.Arp) ? ARP_HEADER_SOURCE_OFFSET : IP_HEADER_SOURCE_OFFSET;
            Buffer.BlockCopy(nat_entry.natted_source_ip_bytes, 0, data, offset, 4);
        }

        private void replaceDestinationIpWithOriginalIP(ref byte[] data, EthernetPacketType ethernet_packet_type, ref xbs_nat_entry nat_entry)
        {
            replaceDestinationIP(ref data, ethernet_packet_type, ref nat_entry.original_source_ip_bytes);
        }

        private void replaceDestinationIP(ref byte[] data, EthernetPacketType ethernet_packet_type, ref byte[] new_destinationIP)
        {
            int offset = (ethernet_packet_type == EthernetPacketType.Arp) ? ARP_HEADER_DESTINATION_OFFSET : IP_HEADER_DESTINATION_OFFSET;
            Buffer.BlockCopy(new_destinationIP, 0, data, offset, 4);
        }

        private void replaceBroadcastIPAddress(ref byte[] data, ref byte[] broadcastIPbytes)
        {
            replaceDestinationIP(ref data, EthernetPacketType.IpV4, ref broadcastIPbytes);
        }

        public static IPAddress calculateBroadcastFromIPandNetmask(IPAddress ip, IPAddress netmask)
        {
            IPAddress ip_broadcast = null;
            byte[] ip_bytes = ip.GetAddressBytes();
            byte[] netmask_bytes = netmask.GetAddressBytes();
            byte[] ip_broadcast_bytes = new byte[4];
            for (int i = 0; i < ip_bytes.Length; i++)
                ip_broadcast_bytes[i] = (byte)(ip_bytes[i] | (~netmask_bytes[i]));
            ip_broadcast = new IPAddress(ip_broadcast_bytes);
            return ip_broadcast;
        }

        public void informOfRemovedDevice(PhysicalAddress srcMAC)
        {
            xbs_nat_entry nat_entry;
            lock (NAT_list)
            {
                if (!NAT_list.TryGetValue(srcMAC, out nat_entry))
                {
#if DEBUG
                    xbs_messages.addDebugMessage("% removal not needed, device not present in NAT IP pool.  " + srcMAC, xbs_message_sender.NAT);
#endif
                    return;
                }
#if DEBUG
                xbs_messages.addDebugMessage("% removing device " + srcMAC + " from NAT list, freeing NAT IP " + nat_entry.natted_source_ip, xbs_message_sender.NAT);
#endif
                NAT_list.Remove(srcMAC);
                ip_pool.freeIP(nat_entry);
            }
        }

        public static xbs_nat getInstance()
        {
            return (FormMain.NAT != null) ? FormMain.NAT : xbs_console_app.NAT;
        }

        private void updateIPChecksums( ref byte[] data )
        {
            int header_len = (data[IP_HEADER_IHL_OFFSET] & 0x0F)*4;
            //reset the checksum field (checksum is calculated when this field is zeroed)
            data[IP_HEADER_CHECKSUM_OFFSET] = 0;
            data[IP_HEADER_CHECKSUM_OFFSET+1] = 0;
            //calculate the one's complement sum of the ip header
            var val = (UInt16)ChecksumUtils.OnesComplementSum(data, IP_HEADER_IHL_OFFSET, header_len);
            EndianBitConverter.Big.CopyBytes(val, data, IP_HEADER_CHECKSUM_OFFSET);

            // disable UDP checksum
            if (data[IP_HEADER_PROTOCOL_OFFSET] == (byte)IPProtocolType.UDP)
            {
                data[IP_HEADER_IHL_OFFSET + header_len + UDP_HEADER_CHECKSUM_OFFSET] = 0;
                data[IP_HEADER_IHL_OFFSET + header_len + UDP_HEADER_CHECKSUM_OFFSET+1] = 0;
            }
        }

        ushort GetTCPChecksum_org(byte[] IPHeader, byte[] TCPHeader)
        {
            uint sum = 0;
            // TCP Header
            for (int x = 0; x < TCPHeader.Length; x += 2)
                sum += ntoh(BitConverter.ToUInt16(TCPHeader, x));
            // Pseudo header - Source Address
            sum += ntoh(BitConverter.ToUInt16(IPHeader, 12));
            sum += ntoh(BitConverter.ToUInt16(IPHeader, 14));
            // Pseudo header - Dest Address
            sum += ntoh(BitConverter.ToUInt16(IPHeader, 16));
            sum += ntoh(BitConverter.ToUInt16(IPHeader, 18));
            // Pseudo header - Protocol
            sum += ntoh(BitConverter.ToUInt16(new byte[] { 0, IPHeader[9] }, 0));
            // Pseudo header - TCP Header length
            sum += (UInt16)TCPHeader.Length;
            // 16 bit 1's compliment
            while ((sum >> 16) != 0) { sum = ((sum & 0xFFFF) + (sum >> 16)); }
            sum = ~sum;
            return (ushort)ntoh((UInt16)sum);
        }

        private ushort ntoh(UInt16 In)
        {
            int x = IPAddress.NetworkToHostOrder(In);
            return (ushort)(x >> 16);
        }

        private void PS3_replaceInfoResponse_hostAddr(PhysicalAddress dstMAC, ref UdpPacket udp_packet, IPAddress natted_ip)
        {
            byte[] data = udp_packet.PayloadData;
            // infoResponse packets are on port 3075 send to broadcast
            if (udp_packet.DestinationPort != 3075 || data.Length < 100)
                return;

            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            String hostaddr_string = "\\hostaddr\\";
            byte[] hostaddr_string_bytearray = enc.GetBytes(hostaddr_string);
            String hostaddr_base64;
            int hostaddr_base64_byte_count = 36;

            int index_of_hostaddr = findIndexOfArrayInArray( ref data, ref hostaddr_string_bytearray);
            if (index_of_hostaddr < 0)
            {
#if DEBUG
                xbs_messages.addDebugMessage("!! % PS3_replaceInfoResponse_hostAddr could not find index_of_hostaddr in Packet", xbs_message_sender.NAT, xbs_message_type.WARNING);
#endif
                return;
            }
            index_of_hostaddr += hostaddr_string_bytearray.Length;

            try
            {
                hostaddr_base64 = enc.GetString(data, index_of_hostaddr, hostaddr_base64_byte_count);
            }
            catch (Exception e)
            {
#if DEBUG
                xbs_messages.addDebugMessage("!! % PS3_replaceInfoResponse_hostAddr could not convert hostaddr_entry_string in Packet : " + e.ToString(), xbs_message_sender.NAT, xbs_message_type.ERROR);
#endif                
                return;
            }

            byte[] hostaddr_all_bytes = Convert.FromBase64String(hostaddr_base64);
#if DEBUG
            byte[] hostaddr_ip_bytes = new byte[4];
            Buffer.BlockCopy(hostaddr_all_bytes, 0, hostaddr_ip_bytes, 0, 4);
            IPAddress hostaddr_ip = new IPAddress(hostaddr_ip_bytes);
#endif
            byte[] natted_ip_bytes = natted_ip.GetAddressBytes();
            Buffer.BlockCopy(natted_ip_bytes, 0, hostaddr_all_bytes, 0, 4);
            String hostaddr_new_base64 = Convert.ToBase64String(hostaddr_all_bytes);
#if DEBUG
            xbs_messages.addDebugMessage("% PS3_replaceInfoResponse_hostAddr hostAddr : " + hostaddr_ip + "|" + hostaddr_base64 + " => " + natted_ip + "|" + hostaddr_new_base64, xbs_message_sender.NAT);
#endif
            if (hostaddr_new_base64.Length != hostaddr_base64.Length)
            {
#if DEBUG
                xbs_messages.addDebugMessage(String.Format("!! % PS3_replaceInfoResponse_hostAddr hostaddr_new_base64 is to BIG! : {0} => {1}", hostaddr_base64.Length, hostaddr_new_base64.Length), xbs_message_sender.NAT, xbs_message_type.ERROR);
#endif                
                return;
            }
            byte[] hostaddr_new_base64_bytes = enc.GetBytes(hostaddr_new_base64);
            if (hostaddr_new_base64_bytes.Length > hostaddr_base64_byte_count)
            {
#if DEBUG
                xbs_messages.addDebugMessage(String.Format("!! % PS3_replaceInfoResponse_hostAddr hostaddr_new_base64_bytes is to BIG! : {0}", hostaddr_new_base64_bytes.Length), xbs_message_sender.NAT, xbs_message_type.ERROR);
#endif
                return;
            }
            Buffer.BlockCopy(hostaddr_new_base64_bytes, 0, data, index_of_hostaddr, hostaddr_new_base64_bytes.Length);
            UInt16 checksum = calc_ps3_packet_checksum_faster(ref data, 2);
            EndianBitConverter.Big.CopyBytes(checksum, data, data.Length - 2);
            udp_packet.PayloadData = data;
        }

        private int findIndexOfArrayInArray( ref byte[] haystack, ref byte[] needle)
        {

            for (int i = 0; i < haystack.Length-needle.Length; i++)
                if (findIndexOfArrayInArray_isMatch(ref haystack, i, ref needle))
                    return i;
            return -1;
        }
        private bool findIndexOfArrayInArray_isMatch(ref byte[] haystack, int position, ref byte[] needle)
        {
            for (int i=0; i<needle.Length; i++)
                if (haystack[position+i]!=needle[i])
                    return false;
            return true;
        }

        private UInt16 calc_ps3_packet_checksum_faster(ref byte[] data, int index_start)
        {
            UInt32 checksum = 0;
            int i;
            int max_index = data.Length - 2 - sizeof(UInt16);
            for (i = index_start; i <= max_index; i += sizeof(UInt16))
                checksum += (UInt16)(EndianBitConverter.Big.ToUInt16(data, i));
            if (i == max_index + 1)
                checksum += data[max_index];
            while (checksum > 0xFFFF)
                checksum = (checksum & 0x0000FFFF) + (checksum >> 16);
            return (UInt16)((checksum != 0) ? ~checksum : checksum);
        }

    }
}
