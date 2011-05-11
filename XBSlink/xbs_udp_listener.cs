/**
 * Project: XBSlink: A XBox360 & PS3/2 System Link Proxy
 * File name: xbs_udp_listener.cs
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
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using PacketDotNet;

namespace XBSlink
{
    class xbs_udp_message
    {
        public IPAddress src_ip;
        public int src_port;
        public xbs_node_message_type msg_type;
        public UInt16 data_len;
        public byte[] data;
    }

    class xbs_udp_listener_statistics
    {
        private static uint packets_in;
        private static uint packets_out;
        private static Object _locker = new Object();

        public static void increasePacketsIn()
        {
            lock (_locker)
                packets_in++;
        }

        public static void increasePacketsOut()
        {
            lock (_locker)
                packets_out++;
        }

        public static uint getPacketsIn()
        {
            uint count;
            lock (_locker)
                count = packets_in;
            return count;
        }

        public static uint getPacketsOut()
        {
            uint count;
            lock (_locker)
                count = packets_out;
            return count;
        }

    }

    class xbs_udp_listener
    {
        public const int standard_port = 31415;
        public int udp_socket_port;
        private Socket udp_socket = null;

        private IPEndPoint local_endpoint = null;
        private Thread dispatcher_thread = null;
        private Thread dispatcher_thread_out = null;
        private Thread receive_thread = null;
        private readonly Object _locker = new Object();
        private readonly Object _locker_out = new Object();
        private volatile bool exiting = false;

        private Queue<xbs_node_message> out_msgs = new Queue<xbs_node_message>();
        private Queue<xbs_node_message> out_msgs_high_prio = new Queue<xbs_node_message>();
        private Queue<xbs_udp_message> in_msgs = new Queue<xbs_udp_message>();
        private Queue<xbs_udp_message> in_msgs_high_prio = new Queue<xbs_udp_message>();

        private xbs_node_list node_list = null;

        public readonly Object _locker_HELLO = new Object();

        public xbs_udp_listener(xbs_node_list nl)
        {
            initialize(IPAddress.Any, xbs_udp_listener.standard_port, nl);
        }

        public xbs_udp_listener( IPAddress ip_endpoint, int port, xbs_node_list nl)
        {
            initialize(ip_endpoint, port, nl);
        }

        private bool initialize(IPAddress ip_endpoint, int port, xbs_node_list nl)
        {
            this.node_list = nl;
            dispatcher_thread = new Thread(new ThreadStart(dispatcher));
            dispatcher_thread.IsBackground = true;
            dispatcher_thread.Priority = ThreadPriority.AboveNormal;
            dispatcher_thread.Start();
            dispatcher_thread_out = new Thread(new ThreadStart(dispatcher_out));
            dispatcher_thread_out.IsBackground = true;
            dispatcher_thread_out.Priority = ThreadPriority.AboveNormal;
            dispatcher_thread_out.Start();

            udp_socket_port = port;
            local_endpoint = new IPEndPoint(ip_endpoint, udp_socket_port);

            udp_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            try
            {
                udp_socket.Bind(local_endpoint);
            }
            catch (SocketException)
            {
                throw new Exception("an error occured while initializing the UDP socket.\r\nPlease see the messages tab.");
            }
            udp_socket.ReceiveTimeout = 1000;
            receive_thread = new Thread( new ThreadStart(udp_receiver) );
            receive_thread.IsBackground = true;
            receive_thread.Priority = ThreadPriority.AboveNormal;
            receive_thread.Start();

            xbs_messages.addInfoMessage(" * initialized udp listener on port " + port);
            return true;
        }

        public void shutdown()
        {
            exiting = true;
            lock (_locker)
                Monitor.PulseAll(_locker);
            lock (_locker_out)
                Monitor.PulseAll(_locker_out);
            if (dispatcher_thread.ThreadState != ThreadState.Stopped)
                dispatcher_thread.Join();
            if (dispatcher_thread_out.ThreadState != ThreadState.Stopped)
                dispatcher_thread_out.Join();
            udp_socket.Close();
        }

        public void udp_receiver()
        {
            xbs_messages.addInfoMessage(" * udp receiver thread started");
            byte[] data = new byte[2048];
            byte[] data2 = new byte[2048];
            IPEndPoint remote_endpoint = new IPEndPoint(IPAddress.Any, 0);
            EndPoint ep = (EndPoint)remote_endpoint;
            int bytes = 0;
            xbs_udp_message msg = null;
#if !DEBUG
            try
            {
#endif
                while (!exiting)
                {
                    try
                    {
                        bytes = udp_socket.ReceiveFrom(data, ref ep);
                    }
                    catch (SocketException)
                    {
                        bytes = 0;
                    }
                    if (!exiting && bytes > 0)
                    {
                        xbs_node_message_type command = xbs_node_message.getMessageTypeFromUDPPacket(data);
                        msg = new xbs_udp_message();
                        msg.msg_type = command;
                        if (bytes > 3)
                        {
                            msg.data = new byte[bytes - 3];
                            Buffer.BlockCopy(data, 3, msg.data, 0, bytes - 3);
                            msg.data_len = (UInt16)(bytes - 3); // TODO: FIXME?
                        }
                        else
                            msg.data_len = 0;
                        remote_endpoint = (IPEndPoint)ep;
                        msg.src_ip = remote_endpoint.Address;
                        msg.src_port = remote_endpoint.Port;

#if DEBUG
                        //xbs_messages.addDebugMessage(" * added UDP packet size "+msg.data_len+" command "+msg.msg_type);
#endif
                        lock (_locker)
                        {
                            if (command == xbs_node_message_type.DATA)
                                lock (in_msgs_high_prio)
                                    in_msgs_high_prio.Enqueue(msg);
                            else
                                lock (in_msgs)
                                    in_msgs.Enqueue(msg);
                            Monitor.PulseAll(_locker);
                        }
                    }
                }
#if !DEBUG
            }
            catch (Exception ex)
            {
                ExceptionMessage.ShowExceptionDialog("udp_receiver service", ex);
            }
#endif
#if DEBUG
                xbs_messages.addDebugMessage(" * udp receiver thread stopped");
#endif
        }

        public void send_xbs_node_message(xbs_node_message msg)
        {
            if (node_list.local_node != null)
                if (msg.receiver.Equals(node_list.local_node))
                    return;
            lock (out_msgs)
                out_msgs.Enqueue(msg);
            lock (_locker_out)
                Monitor.PulseAll(_locker_out);
        }

        public void send_xbs_node_message_high_prio(xbs_node_message msg)
        {
            lock (out_msgs_high_prio)
                out_msgs_high_prio.Enqueue(msg);
            lock (_locker_out)
                Monitor.PulseAll(_locker_out);
        }

        public void dispatcher()
        {
            xbs_messages.addInfoMessage(" * udp listener dispatcher thread starting...");
#if !DEBUG
            try
            {
#endif
                while (!exiting)
                {
                    lock (_locker)
                    {
                        Monitor.Wait(_locker);
                    }
                    if (!exiting)
                    {
                        dispatch_in_qeue();
                    }
                }
#if !DEBUG
            }
            catch (Exception ex)
            {
                ExceptionMessage.ShowExceptionDialog("udp dispatcher service", ex);
            }
#endif
#if DEBUG
                xbs_messages.addDebugMessage(" * udp listener dispatcher thread stopped.");
#endif
        }

        public void dispatch_in_qeue()
        {
            xbs_udp_message udp_msg = null;
            int count_msgs = 0;
            int count_msgs_hp = 0;
            lock (in_msgs)
                count_msgs = in_msgs.Count;
            lock (in_msgs_high_prio)
                count_msgs_hp = in_msgs_high_prio.Count;

            while (count_msgs > 0 || count_msgs_hp > 0)
            {
                xbs_udp_listener_statistics.increasePacketsIn();
                try
                {
                    if (count_msgs_hp > 0)
                    {
                        lock (in_msgs_high_prio)
                        {
                            udp_msg = in_msgs_high_prio.Dequeue();
                            count_msgs_hp = in_msgs_high_prio.Count;
                        }
                    }
                    else
                    {
                        lock (in_msgs)
                        {
                            udp_msg = in_msgs.Dequeue();
                            count_msgs = in_msgs.Count;
                        }
                    }
                }
                catch (Exception e)
                {
                    xbs_messages.addDebugMessage("!! ERROR in UDP dispatch_in_qeue(): "+e.Message);
                }

                if (udp_msg != null)
                {
                    dispatch_in_msg(ref udp_msg);
                    udp_msg = null;
                }
                else
                    xbs_messages.addDebugMessage("!! ERROR in UDP dispatch_in_qeue(): NULL reference in udp_msg ");


                // only recheck for new packets if all known packets are delivered
                if (count_msgs_hp == 0 && count_msgs==0)
                {
                    lock (in_msgs_high_prio)
                        count_msgs_hp = in_msgs_high_prio.Count;
                    lock (in_msgs)
                        count_msgs = in_msgs.Count;
                }
            }
        }

        public void dispatch_in_msg(ref xbs_udp_message udp_msg)
        {
            xbs_node tmp_node = null;
            xbs_node sending_node = node_list.findNode(udp_msg.src_ip, udp_msg.src_port);
# if DEBUG
            if (udp_msg.msg_type != xbs_node_message_type.PING && udp_msg.msg_type != xbs_node_message_type.PONG)
            {
                String str_send_node = (sending_node == null) ? udp_msg.src_ip + ":" + udp_msg.src_port : sending_node.ToString() + " " + sending_node.nickname;
                xbs_messages.addDebugMessage(" * IN " + udp_msg.msg_type + " " + str_send_node);
            }
# endif
            switch (udp_msg.msg_type)
            {
                case xbs_node_message_type.DATA:
                    dispatch_DATA_message(ref udp_msg, ref sending_node);
                    break;

                case xbs_node_message_type.ANNOUNCE:
                    tmp_node = new xbs_node(udp_msg.src_ip, udp_msg.src_port);
                    tmp_node.sendAddNodeMessage(node_list.local_node);
                    node_list.sendNodeListToNode(tmp_node);
                    break;

                case xbs_node_message_type.KNOWNNODE:
                    xbs_node_message_knownnode msg_knownnode = new xbs_node_message_knownnode(udp_msg.data);
                    tmp_node = node_list.findNode(msg_knownnode.ip, msg_knownnode.port);
                    if (tmp_node == null)
                    {
                        tmp_node = new xbs_node(msg_knownnode.ip, msg_knownnode.port);
#if DEBUG
                        xbs_messages.addDebugMessage(" * trying to add known node: " + tmp_node);
#endif
                        node_list.tryAddingNode(tmp_node);
                    }
#if DEBUG
                    else
                        xbs_messages.addDebugMessage(" * already in contact with node: " + tmp_node);
#endif
                    break;

                case xbs_node_message_type.ADDNODE:
                    xbs_node_message_addnode msg_addnode = new xbs_node_message_addnode(udp_msg.data);
# if DEBUG
                    xbs_messages.addDebugMessage(" * received ADDNODE from " + udp_msg.src_ip + ":" + udp_msg.src_port + " for " + msg_addnode.ip + ":" + msg_addnode.port);
# endif
                    if (sending_node == null)
                    {   // node not known, add to nodelist
                        tmp_node = node_list.addNode(msg_addnode.ip, msg_addnode.port, udp_msg.src_ip, udp_msg.src_port);
                        tmp_node.sendAddNodeMessage(node_list.local_node);
                    }
                    break;

                case xbs_node_message_type.DELNODE:
                    xbs_node_message_delnode msg_delnode = new xbs_node_message_delnode(udp_msg.data);
# if DEBUG  
                    xbs_messages.addDebugMessage(" * received DELNODE from " + udp_msg.src_ip + ":" + udp_msg.src_port + " for " + msg_delnode.ip + ":" + msg_delnode.port);
# endif
                    try
                    {
                        tmp_node = node_list.delNode(udp_msg.src_ip, (UInt16)udp_msg.src_port);
                    }
                    catch (Exception ex)
                    {
                        xbs_messages.addInfoMessage("!! error on deleting node: "+ex.Message);
                    }
                    if (tmp_node != null && xbs_chat.message_when_nodes_join_or_leave)
                        xbs_chat.addSystemMessage(tmp_node.nickname + " left.");
                    break;

                case xbs_node_message_type.PING:
                    tmp_node = (sending_node != null) ? sending_node : new xbs_node(udp_msg.src_ip, udp_msg.src_port);
                    xbs_node_message_pong msg_pong = new xbs_node_message_pong(tmp_node, udp_msg.data);
                    tmp_node.sendNodeMessage(msg_pong);
                    break;

                case xbs_node_message_type.PONG:
                    if (sending_node != null)
                    {
                        sending_node.pong(xbs_node_message_pong.getDelay(udp_msg.data).Milliseconds);
                        node_list.listHasJustChanged();
                    }
                    break;

                case xbs_node_message_type.GETNODELIST:
                    tmp_node = (sending_node != null) ? sending_node : new xbs_node(udp_msg.src_ip, udp_msg.src_port);
                    node_list.sendNodeListToNode(tmp_node);
                    break;
                
                case xbs_node_message_type.GETCLIENTVERSION:
                    tmp_node = (sending_node != null) ? sending_node : new xbs_node(udp_msg.src_ip, udp_msg.src_port);
                    xbs_node_message_clientversion msg_gcv = new xbs_node_message_clientversion(tmp_node, xbs_settings.xbslink_version);
                    tmp_node.sendNodeMessage(msg_gcv);
                    break;

                case xbs_node_message_type.CLIENTVERSION:
                    if (sending_node != null)
                    {
                        xbs_node_message_clientversion msg_cv = new xbs_node_message_clientversion(udp_msg.data);
                        sending_node.client_version = msg_cv.version_string;
                        node_list.listHasJustChanged();
                    }
                    break;
                case xbs_node_message_type.CHATMSG:
                    if (sending_node != null)
                    {
                        xbs_node_message_chatmsg msg_chat = new xbs_node_message_chatmsg(udp_msg.data);
                        xbs_chat.addChatMessage(sending_node.nickname, msg_chat.getChatMessage());
                    }
                    break;
                case xbs_node_message_type.NICKNAME:
                    if (sending_node != null)
                    {
                        xbs_node_message_nickname msg_nick = new xbs_node_message_nickname(udp_msg.data);
                        sending_node.nickname = msg_nick.getNickname();
                        sending_node.nickname_received = true;
                        node_list.listHasJustChanged();
                        if ( xbs_chat.message_when_nodes_join_or_leave )
                            xbs_chat.addSystemMessage(sending_node.nickname + " joined.");
                    }
                    break;
                case xbs_node_message_type.GETNICKNAME:
                    tmp_node = (sending_node!=null) ? sending_node : new xbs_node(udp_msg.src_ip, udp_msg.src_port);
                    xbs_node_message_nickname msg_snick = new xbs_node_message_nickname(tmp_node, node_list.local_node.nickname);
                    tmp_node.sendNodeMessage(msg_snick);
                    break;
                case xbs_node_message_type.SERVERHELLO:
                    lock (_locker_HELLO)
                    {
                        xbs_natstun.isPortReachable = true;
                        Monitor.PulseAll(_locker_HELLO);
                    }
                    break;
                case xbs_node_message_type.TO_CLOUDHELPER_HELPWITHNODE:
                    xbs_node_message_toCloudHelper_HelpWithNode msg_toCloudHelpWith = new xbs_node_message_toCloudHelper_HelpWithNode(udp_msg.data);
                    node_list.cloudhelper_helpWithNode(udp_msg.src_ip, udp_msg.src_port, msg_toCloudHelpWith.ip, msg_toCloudHelpWith.port);
                    break;
                case xbs_node_message_type.FROM_CLOUDHELPER_CONTACTNODE:
                    xbs_node_message_fromCloudHelper_ContactNode msg_fromCloudContactNode = new xbs_node_message_fromCloudHelper_ContactNode(udp_msg.data);
                    tmp_node = new xbs_node(msg_fromCloudContactNode.ip, msg_fromCloudContactNode.port);
                    tmp_node.sendAddNodeMessage(node_list.local_node);
                    break;
            }

            if (sending_node != null)
                sending_node.statistics.receivedPacket((uint)udp_msg.data_len+3);        }

        private void dispatch_DATA_message(ref xbs_udp_message udp_msg, ref xbs_node sending_node)
        {
            byte[] src_mac = new byte[6];
            byte[] dst_mac = new byte[6];
            Buffer.BlockCopy(udp_msg.data, 0, dst_mac, 0, 6);
            PhysicalAddress dstMAC = new PhysicalAddress(dst_mac);
            Buffer.BlockCopy(udp_msg.data, 6, src_mac, 0, 6);
            PhysicalAddress srcMAC = new PhysicalAddress(src_mac);
#if DEBUG
            xbs_messages.addDebugMessage(" * DATA (" + udp_msg.data.Length + ") | "+ srcMAC + " => " + dstMAC);
#endif
            xbs_sniffer.getInstance().injectRemotePacket(ref udp_msg.data, dstMAC, srcMAC);
            if (sending_node != null)
                if (sending_node.addXbox(srcMAC))
                    node_list.listHasJustChanged();
        }

        public void dispatcher_out()
        {
            xbs_messages.addInfoMessage(" * udp outgoing dispatcher thread starting...");
#if !DEBUG
            try
            {
#endif
                while (!exiting)
                {
                    lock (_locker_out)
                    {
                        Monitor.Wait(_locker_out);
                    }
                    dispatch_out_queue();
                }
#if !DEBUG
            }
            catch (Exception ex)
            {
                ExceptionMessage.ShowExceptionDialog("udp dispatcher_out service", ex);
            }
#endif
        }

        public void dispatch_out_queue()
        {
            int count = 0, count_hp = 0;
            xbs_node_message msg = null;
            lock (out_msgs)
                count = out_msgs.Count;
            lock (out_msgs_high_prio)
                count_hp = out_msgs_high_prio.Count;
            while (count > 0 || count_hp > 0)
            {
                // High PRIO packets first (mostly DATA packets)
                if (count_hp>0)
                {
                    lock (out_msgs_high_prio)
                        msg = out_msgs_high_prio.Dequeue();
                    count_hp--;
                }
                else if (count > 0)
                {
                    lock (out_msgs)
                        msg = out_msgs.Dequeue();
                    count--;
                }

# if DEBUG
                if (msg.type != xbs_node_message_type.PING && msg.type != xbs_node_message_type.PONG)
                    xbs_messages.addDebugMessage(" * OUT MSG " + msg.type + " " + msg.receiver);
# endif
                xbs_udp_listener_statistics.increasePacketsOut();
                byte[] bytes = msg.getByteArray();
                EndPoint ep = (EndPoint)new IPEndPoint(msg.receiver.getSendToIP(), msg.receiver.getSendToPort());
                try
                {
                    udp_socket.SendTo(bytes, bytes.Length, SocketFlags.None, ep);
                }
                catch (SocketException sock_ex)
                {
                    xbs_messages.addInfoMessage("!! ERROR in dispatch_out_queue SendTo: "+sock_ex.Message);
                }
                
                // recheck for new packets
                if (count_hp == 0)
                {
                    lock (out_msgs_high_prio)
                        count_hp = out_msgs_high_prio.Count;
                    if (count == 0)
                        lock (out_msgs)
                            count = out_msgs.Count;
                }
            }
        }

        public static xbs_udp_listener getInstance()
        {
            return (FormMain.udp_listener != null) ? FormMain.udp_listener : xbs_console_app.udp_listener;
        }

    }
}
