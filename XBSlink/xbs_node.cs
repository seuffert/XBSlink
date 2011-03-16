/**
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

    class xbs_xbox
    {
        public PhysicalAddress mac;
        public int hash;

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
    }

    class xbs_node
    {
        public IPAddress ip_public;
        public int port_public;
        
        public IPAddress ip_announced;
        public int port_announced;
        public IPAddress ip_sendfrom;
        public int port_sendfrom;

        public xbs_node_type node_type;
        public const String CLIENT_VERSION_UNKNOWN = "(unknown)";
        public String client_version = CLIENT_VERSION_UNKNOWN;
        public String nickname = "Anonymous";
        public bool nickname_received = false;

        private List<xbs_xbox> xbox_list;

        private static PhysicalAddress broadcast_mac = new PhysicalAddress(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });

        public long last_ping_delay_ms = -1;
        public DateTime lastPingTime = new DateTime(0);
        public DateTime lastPongTime = DateTime.Now;

        public DateTime addedTime = DateTime.Now;

        public xbs_node(IPAddress ip_from, int port_from)
        {
            this.ip_announced = null;
            this.port_announced = -1;
            this.ip_sendfrom = ip_from;
            this.port_sendfrom = port_from;
            this.ip_public = ip_from;
            this.port_public = port_from;

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


        public override string ToString()
        {
            String port2 = (port_sendfrom == port_public) ? "" : "/"+port_sendfrom;
            return ip_public + ":" + port_public + port2;
        }

        public bool has_xbox(PhysicalAddress xbox_addr)
        {
            if (xbox_addr.Equals(broadcast_mac))
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

        public void sendDataMessage( ref byte[] data)
        {
            xbs_node_message_data msg = new xbs_node_message_data(this, data);
            FormMain.udp_listener.send_xbs_node_message_high_prio(msg);
        }

        public void sendAddNodeMessage(IPAddress ip, int port)
        {
            xbs_node_message_addnode msg_addnode = new xbs_node_message_addnode( ip, (UInt16)port);
            msg_addnode.receiver = this;
            FormMain.udp_listener.send_xbs_node_message(msg_addnode);
        }

        public void sendAddNodeMessage(xbs_node node)
        {
            sendAddNodeMessage(node.ip_public, (UInt16)node.port_public);
        }

        public void sendKnownNodeMessage(IPAddress ip, int port)
        {
            xbs_node_message_knownnode msg_knownnode = new xbs_node_message_knownnode(ip, (UInt16)port);
            msg_knownnode.receiver = this;
            FormMain.udp_listener.send_xbs_node_message(msg_knownnode);
        }

        public void sendKnownNodeMessage(xbs_node node)
        {
            sendKnownNodeMessage(node.ip_public, (UInt16)node.port_public);
        }

        public void addXbox(PhysicalAddress mac)
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
                xbs_messages.addInfoMessage(" ~ added new device " + mac + " for node " + this);
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
            xbs_messages.addInfoMessage(" ~ sending DelNodeMessage to " + this + " for node " + node);
#endif
            xbs_node_message_delnode msg = new xbs_node_message_delnode(node.ip_public, (UInt16)node.port_public);
            msg.receiver = this;
            FormMain.udp_listener.send_xbs_node_message(msg);
        }

        public void sendPing()
        {
            xbs_node_message_ping msg_ping = new xbs_node_message_ping(this);
            FormMain.udp_listener.send_xbs_node_message(msg_ping);
        }

        public void sendGetClientVersion()
        {
            xbs_node_message_getclientversion msg = new xbs_node_message_getclientversion(this);
            FormMain.udp_listener.send_xbs_node_message(msg);
        }

        public void sendChatMessage(String chat_message)
        {
            xbs_node_message_chatmsg msg = new xbs_node_message_chatmsg(this, chat_message);
            FormMain.udp_listener.send_xbs_node_message(msg);
        }

        public void sendGetNickname()
        {
            xbs_node_message_getnickname msg = new xbs_node_message_getnickname(this);
            FormMain.udp_listener.send_xbs_node_message(msg);
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
            last_ping_delay_ms = ms;
            lastPongTime = DateTime.Now;
        }

        public void sendToCloudHelper_HelpWithNode(xbs_node node)
        {
            xbs_node_message_toCloudHelper_HelpWithNode msg = new xbs_node_message_toCloudHelper_HelpWithNode(node.ip_public, (UInt16)node.port_public);
            msg.receiver = this;
            FormMain.udp_listener.send_xbs_node_message(msg);
        }

        public void send_fromCloudhelper_helpWithAddingNode(xbs_node node)
        {
            xbs_node_message_fromCloudHelper_ContactNode msg = new xbs_node_message_fromCloudHelper_ContactNode(node.ip_public, (UInt16)node.port_public);
            msg.receiver = this;
            FormMain.udp_listener.send_xbs_node_message(msg);
        }

        public int get_xbox_count()
        {
            int count = 0;
            lock (this)
                count = this.xbox_list.Count;
            return count;
        }
    }
}
