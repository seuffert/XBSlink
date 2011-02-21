/**
 * Project: XBSlink: A XBox360 & PS3/2 System Link Proxy
 * File name: xbs_node_message.cs
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
using System.Net;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Text;

namespace XBSlink
{
    enum xbs_node_message_type : byte
    {
        ANNOUNCE = 0x00,
        GETNODELIST = 0x01,
        ADDNODE = 0x02,
        DELNODE = 0x03,
        PING = 0x04,
        PONG = 0x05,
        DATA = 0x06,
        ADDMASTERBROWSER = 0x07, //obsolete!
        GETCLIENTVERSION = 0x08,
        CLIENTVERSION = 0x09,
        CHATMSG = 0x0A,
        NICKNAME = 0x0B,
        GETNICKNAME = 0x0C,
        KNOWNNODE = 0x0D,
        TO_CLOUDHELPER_HELPWITHNODE = 0x0E,
        FROM_CLOUDHELPER_CONTACTNODE = 0x0F,
        SERVERHELLO = 0xFF
    }

    class xbs_node_message
    {
        public xbs_node receiver;
        public xbs_node_message_type type;
        public UInt16 data_len = 0;
        public byte[] data;

        public static xbs_node_message_type getMessageTypeFromUDPPacket(byte[] bytes)
        {
            return (xbs_node_message_type)bytes[0];
        }

        public byte[] getByteArray()
        {
            byte[] ret;
            ret = (data==null) ? new byte[1] : new byte[sizeof(xbs_node_message_type) + sizeof(UInt16) + data.Length];
            ret[0] = (byte)type;
            if (ret.Length > 1)
            {
                UInt16 len = (UInt16)data.Length;
                byte[] len_arr = BitConverter.GetBytes(len);
                Buffer.BlockCopy(len_arr, 0, ret, 1, 2);
                Buffer.BlockCopy(data, 0, ret, 3, data.Length);
            }
            return ret;
        }

        public static byte[] getUTF8BytesFromString(String str)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] ret = Encoding.UTF8.GetBytes(str);
            return ret;
        }

        public static String getStringFromUTF8Bytes(byte[] bytes)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            String utf8String = encoder.GetString(bytes);
            return utf8String;
        }

    }

    class xbs_node_message_data : xbs_node_message
    {
        public xbs_node_message_type ttype = xbs_node_message_type.DATA;

        public xbs_node_message_data(IPAddress ip, int port, byte[] data_bytes)
            : this (new xbs_node(ip, port), data_bytes)
        {
        }

        public xbs_node_message_data(xbs_node node, byte[] data_bytes)
        {
            type = ttype;
            data = data_bytes;
            data_len = (UInt16)data.Length;
            receiver = node;
        }
    }

    class xbs_node_message_announce : xbs_node_message
    {
        public xbs_node_message_type ttype = xbs_node_message_type.ANNOUNCE;

        public xbs_node_message_announce(IPAddress ip, int port)
            : this(new xbs_node(ip, port))
        {
        }

        public xbs_node_message_announce(xbs_node node)
        {
            type = ttype;
            data_len = 0;
            receiver = node;
        }
    }

    class xbs_node_message_addnode : xbs_node_message
    {
        public IPAddress ip;
        public UInt16 port;
        public xbs_node_message_type ttype = xbs_node_message_type.ADDNODE;

        public xbs_node_message_addnode( byte[] packet_data )
        {
            type = ttype;
            data_len = 4 + sizeof(UInt16);
            byte[] ip_bytes = new byte[4];
            Buffer.BlockCopy(packet_data, 0, ip_bytes, 0, 4);
            ip = new IPAddress(ip_bytes);
            port = BitConverter.ToUInt16(packet_data, 4);
        }

        public xbs_node_message_addnode( IPAddress ip, UInt16 port)
        {
            type = ttype;
            data_len = 4 + sizeof(UInt16);
            data = new byte[data_len];
            var ip_array = ip.GetAddressBytes();
            var port_array = BitConverter.GetBytes(port);
            Buffer.BlockCopy(ip_array, 0, data, 0, 4);
            Buffer.BlockCopy(port_array, 0, data, 4, 2);
            this.ip = ip;
            this.port = port;
        }
    }

    class xbs_node_message_delnode : xbs_node_message_addnode
    {
        public new xbs_node_message_type ttype = xbs_node_message_type.DELNODE;

        public xbs_node_message_delnode(byte[] packet_data)
            : base(packet_data)
        {
            type = xbs_node_message_type.DELNODE;
        }

        public xbs_node_message_delnode(IPAddress ip, UInt16 port)
            : base(ip, port)
        {
            type = xbs_node_message_type.DELNODE;
        }
    }

    class xbs_node_message_ping : xbs_node_message
    {
        public xbs_node_message_type ttype = xbs_node_message_type.PING;

        public xbs_node_message_ping(IPAddress ip, int port)
            : this (new xbs_node(ip, port))
        {
        }

        public xbs_node_message_ping(xbs_node node)
        {
            DateTime now = DateTime.Now;
            data = BitConverter.GetBytes(now.Ticks);
            data_len = (UInt16)data.Length;

            type = ttype;
            receiver = node;
        }
    }

    class xbs_node_message_pong : xbs_node_message
    {
        public xbs_node_message_type ttype = xbs_node_message_type.PONG;

        public xbs_node_message_pong(IPAddress ip, int port, byte[] data)
            : this (new xbs_node(ip, port), data)
        {
        }

        public xbs_node_message_pong(xbs_node node, byte[] data)
        {
            this.data = data;
            data_len = (UInt16)data.Length;
            type = ttype;
            data_len = 0;
            receiver = node;
        }

        public static TimeSpan getDelay(byte[] data)
        {
            long delay;
            long now = DateTime.Now.Ticks;
            delay = now - BitConverter.ToInt64(data, 0);
            return new TimeSpan(delay);
        }
    }

    class xbs_node_message_getclientversion : xbs_node_message
    {
        public xbs_node_message_type ttype = xbs_node_message_type.GETCLIENTVERSION;

        public xbs_node_message_getclientversion(IPAddress ip, int port) 
            : this (new xbs_node(ip, port))
        {
        }

        public xbs_node_message_getclientversion(xbs_node node)
        {
            type = ttype;
            data_len = 0;
            receiver = node;
        }
    }

    class xbs_node_message_clientversion : xbs_node_message
    {
        public xbs_node_message_type ttype = xbs_node_message_type.CLIENTVERSION;
        public String version_string = "0.0.0.0";

        public xbs_node_message_clientversion(IPAddress ip, int port, String version_string) 
            : this (new xbs_node(ip, port), version_string)
        {
        }

        public xbs_node_message_clientversion(xbs_node node, String version_string)
        {
            type = ttype;
            data = getVersionStringByteArray(version_string);
            data_len = 4;
            receiver = node;
        }

        public xbs_node_message_clientversion(byte[] packet_data)
        {
            type = ttype;
            data_len = 4;
            String[] str = new String[4];
            if (packet_data.Length == 4)
            {
                for (int i = 0; i < 4; i++)
                    str[i] = packet_data[i].ToString();
                version_string =  String.Join(".", str);
            }
        }

        private byte[] getVersionStringByteArray( String version_string )
        {
            byte[] ret = new byte[4] { 0,0,0,0 };
            string[] sa = version_string.Split('.');
            if (sa.Length == 4)
                for (int i = 0; i < 4; i++)
                    ret[i] = Byte.Parse(sa[i]);
            return ret;
        }
    }

    class xbs_node_message_string : xbs_node_message
    {
        public String message_string = "";

        public xbs_node_message_string( xbs_node node, String str)
        {
            this.message_string = str;
            data = xbs_node_message.getUTF8BytesFromString(str);
            data_len = (UInt16)data.Length;
            receiver = node;
        }

        public xbs_node_message_string(byte[] packet_data)
        {
            if (packet_data != null)
            {
                data_len = (UInt16)packet_data.Length;
                this.message_string = xbs_node_message.getStringFromUTF8Bytes(packet_data);
            }
        }
    }

    class xbs_node_message_chatmsg : xbs_node_message_string
    {
        public xbs_node_message_type ttype = xbs_node_message_type.CHATMSG;

        public xbs_node_message_chatmsg(IPAddress ip, int port, String chat_message) 
            : this( new xbs_node( ip, port) , chat_message )
        {
        }

        public xbs_node_message_chatmsg(xbs_node node, String chat_message) 
            : base (node, chat_message)
        {
            type = ttype;
        }

        public xbs_node_message_chatmsg(byte[] packet_data) : base(packet_data)
        {
            type = ttype;
        }

        public String getChatMessage()
        {
            return this.message_string;
        }
    }

    class xbs_node_message_nickname: xbs_node_message_string
    {
        public xbs_node_message_type ttype = xbs_node_message_type.NICKNAME;

        public xbs_node_message_nickname(IPAddress ip, int port, String nickname)
            : this(new xbs_node(ip, port), nickname)
        {
        }

        public xbs_node_message_nickname(xbs_node node, String nickname)
            : base(node, nickname)
        {
            type = ttype;
        }

        public xbs_node_message_nickname(byte[] packet_data)
            : base(packet_data)
        {
            type = ttype;
        }

        public String getNickname()
        {
            return this.message_string;
        }
    }

    class xbs_node_message_getnickname : xbs_node_message
    {
        public xbs_node_message_type ttype = xbs_node_message_type.GETNICKNAME;

        public xbs_node_message_getnickname(IPAddress ip, int port)
            : this(new xbs_node(ip, port))
        {
        }

        public xbs_node_message_getnickname(xbs_node node)
        {
            type = ttype;
            data_len = 0;
            receiver = node;
        }
    }

    class xbs_node_message_knownnode : xbs_node_message_addnode
    {
        public new xbs_node_message_type ttype = xbs_node_message_type.KNOWNNODE;

        public xbs_node_message_knownnode(byte[] packet_data)
            : base(packet_data)
        {
            type = ttype;
        }

        public xbs_node_message_knownnode(IPAddress ip, UInt16 port)
            : base(ip, port)
        {
            type = ttype;
        }
    }

    class xbs_node_message_toCloudHelper_HelpWithNode : xbs_node_message_addnode
    {
        public new xbs_node_message_type ttype = xbs_node_message_type.TO_CLOUDHELPER_HELPWITHNODE;

        public xbs_node_message_toCloudHelper_HelpWithNode(byte[] packet_data)
            : base(packet_data)
        {
            type = ttype;
        }

        public xbs_node_message_toCloudHelper_HelpWithNode(IPAddress ip, UInt16 port)
            : base(ip, port)
        {
            type = ttype;
        }
    }

    class xbs_node_message_fromCloudHelper_ContactNode : xbs_node_message_addnode
    {
        public new xbs_node_message_type ttype = xbs_node_message_type.FROM_CLOUDHELPER_CONTACTNODE;

        public xbs_node_message_fromCloudHelper_ContactNode(byte[] packet_data)
            : base(packet_data)
        {
            type = ttype;
        }

        public xbs_node_message_fromCloudHelper_ContactNode(IPAddress ip, UInt16 port)
            : base(ip, port)
        {
            type = ttype;
        }
    }

}

