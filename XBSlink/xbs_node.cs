﻿/**
 * Project: XBSlink: A XBox360 & PS3/2 System Link Proxy
 * File name: xbs_node.cs
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
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Mono.Nat;

namespace XBSlink
{
    public enum xbs_node_type : ushort
    {
        UDP = 0x00,
        TCP = 0x01
    }

    public class xbs_xbox
    {
        // MAC address identifying this device
        public PhysicalAddress mac;
        
        // hash of device MAC address
        public int hash;
        
        // all IPs related to this device are saved here
        public List<IPAddress> ip_addresses = new List<IPAddress>();
        private List<int> ip_hashes = new List<int>();

        public xbs_xbox(PhysicalAddress phy)
        {
            mac = phy;
            hash = mac.GetHashCode();
        }

        public bool Equals(xbs_xbox xbox)
        {
            return (hash==xbox.hash);
        }

        public override string ToString()
        {
            return this.mac.ToString();
        }

        public bool addIPAddress( IPAddress ip )
        {
            int new_hash = ip.GetHashCode();
            if (ip_hashes.Contains(new_hash))
                return false;
            ip_addresses.Add(ip);
            ip_hashes.Add(new_hash);
            return true;
        }
    }

    public class xbs_node_statistics
    {
        public UInt64 packet_count_in = 0;
        public UInt64 packet_count_out = 0;
        public UInt64 bytes_in = 0;
        public UInt64 bytes_out = 0;
        DateTime last_update = DateTime.Now;
#if DEBUG
        public xbs_node node = null;
#endif
        public void sentPacket(uint bytes)
        {
            packet_count_out++;
            bytes_out += bytes;
            last_update = DateTime.Now;
#if DEBUG
            log_stats();
#endif
        }
        public void receivedPacket(uint bytes)
        {
            packet_count_in++;
            bytes_in += bytes;
            last_update = DateTime.Now;
#if DEBUG
            log_stats();
#endif
        }

#if DEBUG
        public void log_stats()
        {
            //xbs_messages.addDebugMessage(" ~ STATS for \""+node.nickname+"\" IN:"+packet_count_in+"/"+bytes_in+" OUT:"+packet_count_out+"/"+bytes_out);
        }
#endif
    }

    public class xbs_node
    {
        public IPAddress ip_public;
        public int port_public;
        
        public IPAddress ip_announced;
        public int port_announced;
        public IPAddress ip_sendfrom;
        public int port_sendfrom;

        public xbs_node_type node_type;
        public const String CLIENT_VERSION_UNKNOWN = "(unknown)";
        public volatile String _client_version;
        public String client_version { get { return _client_version; } set { _client_version = value; changed(); } }
        private volatile String _nickname;
        public String nickname { get { return _nickname; } set { _nickname = value; changed(); } }
        private volatile bool _nickname_received;
        public bool nickname_received { get { return _nickname_received; } set { _nickname_received = value; changed(); } } 

        public volatile xbs_node_statistics statistics = new xbs_node_statistics();

        private List<xbs_xbox> xbox_list;

        private volatile int _last_ping_delay_ms;
        public int last_ping_delay_ms { get { return _last_ping_delay_ms; } private set { _last_ping_delay_ms = value; } }
        private DateTime lastPingTime = DateTime.MinValue;
        public TimeSpan timeSinceLastPing { get { return (DateTime.Now - lastPingTime); } }
        private DateTime lastPongTime = DateTime.Now;
        public TimeSpan timeSinceLastPong { get { return (DateTime.Now - lastPongTime); } }

        public DateTime addedTime = DateTime.Now;
        public DateTime lastChangeTime = DateTime.Now;

        // used to count the trys when cloudhelper is asked to help adding this node
        public int ask_cloudhelper_count = 0;

        public xbs_node(IPAddress ip_from, int port_from)
        {
#if DEBUG
            statistics.node = this;
#endif
            this.ip_announced = null;
            this.port_announced = -1;
            this.ip_sendfrom = ip_from;
            this.port_sendfrom = port_from;
            this.ip_public = ip_from;
            this.port_public = port_from;
            
            client_version = CLIENT_VERSION_UNKNOWN;
            nickname = "{Anonymous}";
            nickname_received = false;
            last_ping_delay_ms = -1;

            this.xbox_list = new List<xbs_xbox>();
            this.node_type = xbs_node_type.UDP;
        }

        public xbs_node(IPAddress ip_announced, int port_announced, IPAddress ip_sendfrom, int port_sendfrom)
            : this ( ip_sendfrom, port_sendfrom)
        {
            this.ip_announced = ip_announced;
            this.port_announced = port_announced;
            if (!NatUtility.IsPrivateAddressSpace(ip_announced) && ip_announced.ToString()!="0.0.0.0")
            {
                this.ip_public = ip_announced;
                this.port_public = port_announced;
            }
        }


        public void sendMessagePM(String chat_message)
        {
            xbs_node_message_msgpm msg = new xbs_node_message_msgpm(this, chat_message);
            sendNodeMessage(msg);
        }

        public override string ToString()
        {
            String port2 = (port_sendfrom == port_public) ? "" : "/"+port_sendfrom;
            return ip_public + ":" + port_public + port2;
        }

        public bool has_xbox(PhysicalAddress xbox_addr)
        {
            if (xbox_addr.Equals(xbs_nat.broadcast_mac) || xbox_addr.Equals(xbs_nat.zero_mac))
                return true;
            int hash = xbox_addr.GetHashCode();
            bool ret = false;
            lock (this)
            {
                foreach (xbs_xbox xbox in xbox_list)
                    if (xbox.hash == hash)
                        ret = true;
            }
            return ret;
        }

        public void sendNodeMessage(xbs_node_message msg)
        {
            if (xbs_udp_listener.getInstance() != null)
            {
                xbs_udp_listener.getInstance().send_xbs_node_message(msg);
                statistics.sentPacket(msg.getMessageByteArraySize());
            }
        }
        public void sendNodeMessageHighPrio(xbs_node_message msg)
        {
            if (xbs_udp_listener.getInstance() != null)
            {
                xbs_udp_listener.getInstance().send_xbs_node_message_high_prio(msg);
                statistics.sentPacket(msg.getMessageByteArraySize());
            }
        }

        public void sendDataMessage( ref byte[] data)
        {
            xbs_node_message_data msg = new xbs_node_message_data(this, data);
            sendNodeMessageHighPrio( msg );
        }

        public void sendAddNodeMessage(IPAddress ip, int port)
        {
            xbs_node_message_addnode msg_addnode = new xbs_node_message_addnode( ip, (UInt16)port);
            msg_addnode.receiver = this;
            sendNodeMessage(msg_addnode);
        }

        public void sendAddNodeMessage(xbs_node node)
        {
            sendAddNodeMessage(node.ip_public, (UInt16)node.port_public);
        }

        public void sendKnownNodeMessage(IPAddress ip, int port)
        {
            xbs_node_message_knownnode msg_knownnode = new xbs_node_message_knownnode(ip, (UInt16)port);
            msg_knownnode.receiver = this;
            sendNodeMessage(msg_knownnode);
            
        }

        public void sendKnownNodeMessage(xbs_node node)
        {
            sendKnownNodeMessage(node.ip_public, (UInt16)node.port_public);
        }

        public bool addXbox(PhysicalAddress mac)
        {
            int hash = mac.GetHashCode();
            bool xbox_found = false;
            lock (this)
            {
                foreach (xbs_xbox xbox in xbox_list)
                    if (hash == xbox.hash)
                        xbox_found = true;
                if (!xbox_found)
                    xbox_list.Add(new xbs_xbox(mac));
            }
            if (!xbox_found)
            {
                xbs_messages.addInfoMessage(" ~ added new device " + mac + " for node " + this, xbs_message_sender.NODE);
                changed();
            }
            return (!xbox_found);
        }

        public bool addIPtoXbox(PhysicalAddress mac, IPAddress ip)
        {
            int hash = mac.GetHashCode();
            lock (this)
            {
                foreach (xbs_xbox xbox in xbox_list)
                    if (hash == xbox.hash)
                        return xbox.addIPAddress(ip);
            }
            return false;
        }

        public bool Equals(xbs_node node)
        {
            if (ip_announced!=null && node.ip_announced!=null)
                if (ip_announced.Equals(node.ip_announced) && port_announced == node.port_announced)
                    return true;
            if (ip_sendfrom!=null && node.ip_sendfrom!=null)
                if (ip_sendfrom.Equals(node.ip_sendfrom) && port_sendfrom == node.port_sendfrom)
                    return true;
            if (ip_public != null && node.ip_public != null)
                if (ip_public.Equals(node.ip_public) && port_public == node.port_public)
                    return true;
            return false;
        }

        public void sendDelNodeMessage(xbs_node node)
        {
#if DEBUG
            xbs_messages.addInfoMessage(" ~ sending DelNodeMessage to " + this + " for node " + node, xbs_message_sender.NODE);
#endif
            xbs_node_message_delnode msg = new xbs_node_message_delnode(node.ip_public, (UInt16)node.port_public);
            msg.receiver = this;
            sendNodeMessage(msg);
        }

        public void sendPing()
        {
            xbs_node_message_ping msg_ping = new xbs_node_message_ping(this);
            sendNodeMessage(msg_ping);
            lastPingTime = DateTime.Now;
        }

        public void sendGetClientVersion()
        {
            xbs_node_message_getclientversion msg = new xbs_node_message_getclientversion(this);
            sendNodeMessage(msg);
        }

        public void sendChatMessage(String chat_message)
        {
            xbs_node_message_chatmsg msg = new xbs_node_message_chatmsg(this, chat_message);
            sendNodeMessage(msg);
        }

        public void sendGetNickname()
        {
            xbs_node_message_getnickname msg = new xbs_node_message_getnickname(this);
            sendNodeMessage(msg);
        }

        public IPAddress getSendToIP()
        {
            return (ip_sendfrom != null) ? ip_sendfrom : ip_public;
        }

        public int getSendToPort()
        {
            return (ip_sendfrom != null) ? port_sendfrom : port_public;
        }

        public void pong( int ms )
        {
            if (last_ping_delay_ms != ms)
            {
                changed();
                last_ping_delay_ms = ms;
            }
            lastPongTime = DateTime.Now;
        }

        public void sendToCloudHelper_HelpWithNode(xbs_node node)
        {
            xbs_node_message_toCloudHelper_HelpWithNode msg = new xbs_node_message_toCloudHelper_HelpWithNode(node.ip_public, (UInt16)node.port_public);
            msg.receiver = this;
            sendNodeMessage(msg);
        }

        public void send_fromCloudhelper_helpWithAddingNode(xbs_node node)
        {
            xbs_node_message_fromCloudHelper_ContactNode msg = new xbs_node_message_fromCloudHelper_ContactNode(node.ip_public, (UInt16)node.port_public);
            msg.receiver = this;
            sendNodeMessage(msg);
        }

        public int get_xbox_count()
        {
            int count = 0;
            lock (this)
                count = this.xbox_list.Count;
            return count;
        }

        private void changed()
        {
            lastChangeTime = DateTime.Now;
        }

        public xbs_xbox[] getXboxArray()
        {
            lock (this)
                return xbox_list.ToArray();
        }
    }
}
