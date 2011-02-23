/**
 * Project: XBSlink: A XBox360 & PS3/2 System Link Proxy
 * File name: xbs_node_list.cs
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
using Mono.Nat;

namespace XBSlink
{
    class xbs_node_list
    {
        private List<xbs_node> node_list;
        private List<xbs_node> node_list_adding;

        public xbs_node local_node = null;

        private Thread ping_thread = null;
        public const int MIN_PING_DELAY_SECONDS = 20;
        public const int MAX_PING_NO_RESPONSE_SECONDS = 30;
        public const int MAX_ADD_NODE_TIMEOUT_SECONDS = 2;
        private bool run_ping_nodes_loop = true;

        public bool notify_on_new_node = true;
        public const String NOTIFICATION_SOUND_NODE_JOINED = "sounds/new_node.wav";
        public const String NOTIFICATION_SOUND_NODE_LEFT = "sounds/node_left.wav";

        private DateTime last_change_time;
        private Object last_change_lock = new Object();

        public xbs_node_list()
        {
            last_change_time = DateTime.Now;
            this.node_list = new List<xbs_node>();
            this.node_list_adding = new List<xbs_node>();
            ping_thread = new Thread(new ThreadStart(ping_nodes_thread));
            ping_thread.IsBackground = true;
            ping_thread.Priority = ThreadPriority.Normal;
            ping_thread.Start();
        }

        ~xbs_node_list()
        {
            run_ping_nodes_loop = false;
            if (ping_thread.ThreadState != ThreadState.Stopped)
                ping_thread.Join();
        }

        public List<xbs_node> getList()
        {
            return node_list;
        }

        public void addNode(xbs_node node)
        {
            lock (this)
            {
                foreach (xbs_node n in node_list)
                    if (n.Equals(node))
                        return;
                node_list.Add(node);
            }
            FormMain.addMessage(" + Added new node: " + node);
            node.sendGetClientVersion();
            node.sendPing();
            node.sendGetNickname();
            if (notify_on_new_node)
            {
                System.Media.SoundPlayer sound_player = new System.Media.SoundPlayer();
                sound_player.SoundLocation = xbs_node_list.NOTIFICATION_SOUND_NODE_JOINED;
                try
                {
                    sound_player.Play();
                }
                catch (Exception)
                {
                }
            }
            purgeAddedNodeFromAddingList(node);
            listHasJustChanged();
        }
    
        public xbs_node addNode(IPAddress ip_announced, int port_announced, IPAddress ip_from, int port_from)
        {
            xbs_node node = new xbs_node(ip_announced, port_announced, ip_from, port_from);          
            addNode( node );
            return node;
        }

        public void delNode(xbs_node node)
        {
            xbs_node deleted_node = null;
            lock (this)
            {
                foreach (xbs_node n in node_list)
                    if (n.Equals(node))
                    {
                        FormMain.addMessage(" + removed node " + n);
                        deleted_node = n;
                        node_list.Remove(n);
                        if (notify_on_new_node)
                        {
                            System.Media.SoundPlayer sound_player = new System.Media.SoundPlayer();
                            sound_player.SoundLocation = xbs_node_list.NOTIFICATION_SOUND_NODE_LEFT;
                            try
                            {
                                sound_player.Play();
                            }
                            catch (Exception)
                            {
                            }
                        }
                        listHasJustChanged();
                        break;
                    }
            }
        }

        public xbs_node delNode(IPAddress ip, UInt16 port)
        {
            xbs_node node;
            lock (this)
                node = findNode(ip, port);
            if (node != null)
                delNode(node);
            return node;
        }

        public xbs_node findNode(IPAddress ip, int port)
        {
            lock (this)
            {
                foreach (xbs_node n in node_list)
                {
                    if (n.ip_announced!=null)
                        if (n.ip_announced.Equals(ip) && (n.port_announced == port))
                            return n;
                    if (n.ip_sendfrom.Equals(ip) && (n.port_sendfrom == port))
                        return n;
                }

            }
            return null;
        }

        public xbs_node findNodeInAddingList(IPAddress ip, int port)
        {
            lock (node_list_adding)
            {
                foreach (xbs_node n in node_list_adding)
                    if (n.ip_sendfrom.Equals(ip) && n.port_sendfrom == port)
                        return n;
            }
            return null;
        }

        public void sendNodeListToNode(xbs_node new_node)
        {
#if DEBUG
            FormMain.addMessage(" + sending Nodelist to " + new_node);
#endif
            lock (this)
            {
                foreach (xbs_node node in node_list)
                    if (!new_node.Equals(node))
                        new_node.sendKnownNodeMessage(node);
            }
        }

        public void informNodesOnAddNode(xbs_node new_node)
        {
            FormMain.addMessage(" + informing all nodes of new node " + new_node);
            lock (this)
            {
                foreach (xbs_node node in node_list)
                    if (!node.Equals(new_node))
                        node.sendAddNodeMessage(new_node);
            }
        }

        public void informNodesOnDelNode(xbs_node node)
        {
            FormMain.addMessage(" + informing all nodes of removed node " + node);
            lock (this)
            {
                foreach (xbs_node n in node_list)
                    if (!node.Equals(n))
                        n.sendDelNodeMessage(node);
            }
        }

        public void sendLogOff()
        {
            informNodesOnDelNode(local_node);
        }

        public void pingAllnodes()
        {
            DateTime now = DateTime.Now;
            List<xbs_node> del_list = new List<xbs_node>();
            int time_since_last_ping = 0;
            int time_since_last_pong = 0;
            lock (this)
            {
                foreach (xbs_node n in node_list)
                {
                    if (!run_ping_nodes_loop)
                        break;
                    time_since_last_ping = (int)(now - n.lastPingTime).TotalSeconds;
                    time_since_last_pong = (int)(now - n.lastPongTime).TotalSeconds;
                    if (time_since_last_ping > xbs_node_list.MIN_PING_DELAY_SECONDS || time_since_last_pong > xbs_node_list.MIN_PING_DELAY_SECONDS)
                    {
                        n.sendPing();
                        n.lastPingTime = now;
                        if (n.client_version == xbs_node.CLIENT_VERSION_UNKNOWN)
                            n.sendGetClientVersion();
                        if (n.nickname_received == false)
                            n.sendGetNickname();
                    }
                    if (time_since_last_pong > xbs_node_list.MAX_PING_NO_RESPONSE_SECONDS)
                        del_list.Add(n);
                }
                foreach (xbs_node n in del_list)
                {
                    FormMain.addMessage(" + removing node "+n+" (ping timeout)");
                    n.sendDelNodeMessage(local_node);
                    node_list.Remove(n);
                    listHasJustChanged();
                }
            }
        }

        public void distributeDataPacket(PhysicalAddress dstMac, byte[] data)
        {
            lock (this)
            {
                foreach (xbs_node node in node_list)
                    if (node.has_xbox(dstMac))
                        node.sendDataMessage(ref data);
            }
        }

        public void sendChatMessageToAllNodes( String chat_message )
        {
            lock (this)
            {
                foreach (xbs_node n in node_list)
                    n.sendChatMessage(chat_message);
            }
        }

        private void ping_nodes_thread()
        {
            FormMain.addMessage(" + starting node survey service");
#if !DEBUG
            try
            {
#endif
                while (run_ping_nodes_loop)
                {
                    pingAllnodes();
                    checkNodesInAddingList();
                    if (run_ping_nodes_loop)
                        Thread.Sleep(1000);
                }
#if !DEBUG
            }
            catch (Exception ex)
            {
                ExceptionMessage.ShowExceptionDialog("node survey service", ex);
            }
#endif
        }

        public void clear_nodes()
        {
            lock (this)
            {
                lock (node_list)
                {
                    node_list.Clear();
                }
            }
            listHasJustChanged();
        }

        public void tryAddingNode(xbs_node new_node)
        {
            if (findNodeInAddingList(new_node.ip_public, new_node.port_public) != null)
            {
#if DEBUG
                FormMain.addMessage(" + node already in addingList: "+new_node);
#endif
                return;
            }
            lock (node_list_adding)
                node_list_adding.Add(new_node);
            new_node.sendAddNodeMessage(local_node);
        }

        private void purgeAddedNodeFromAddingList(xbs_node new_node)
        {
#if DEBUG
            FormMain.addMessage("+ trying to purged node from node_list_adding : " + new_node);
#endif
            lock (node_list_adding)
            {
                for (int i=0;i<node_list_adding.Count;i++)
                    if (node_list_adding[i].Equals(new_node))
                    {
                        node_list_adding.RemoveAt(i);
#if DEBUG
                        FormMain.addMessage("+ purged node from node_list_adding : "+new_node);
#endif
                        break;
                    }
            }
        }

        private void checkNodesInAddingList()
        {
            int count;
            TimeSpan time_elapsed;
            List<xbs_node> cloud_helper_node_list = new List<xbs_node>();

            lock (node_list_adding)
                count = node_list_adding.Count;
            if (count == 0)
                return;
            lock (node_list_adding)
            {
                foreach (xbs_node node in node_list_adding)
                {
                    time_elapsed = DateTime.Now - node.addedTime;
                    if (time_elapsed.TotalSeconds>=MAX_ADD_NODE_TIMEOUT_SECONDS)
                        cloud_helper_node_list.Add(node);                        
                }
                foreach (xbs_node node in cloud_helper_node_list)
                    node_list_adding.Remove(node);
            }

            foreach (xbs_node node in cloud_helper_node_list)
            {
                askCloudHelperToSendAddNodeMessage(node);
            }

        }

        private void askCloudHelperToSendAddNodeMessage(xbs_node node)
        {
            xbs_node cloud_helper;
            lock (node_list_adding)
                cloud_helper = node_list.Count == 0 ? null : node_list[0];
            if (cloud_helper == null)
                return;
            FormMain.addMessage("+ asking cloud_helper ("+cloud_helper+") for help to add node "+node);
            cloud_helper.sendToCloudHelper_HelpWithNode(node);
        }

        public void cloudhelper_helpWithNode(IPAddress ip_from, int port_from, IPAddress ip_to, int port_to)
        {
            xbs_node node_from = findNode(ip_from, port_from);
            xbs_node node_to = findNode(ip_to, port_to);
            if (node_from != null && node_to != null)
                node_to.send_fromCloudhelper_helpWithAddingNode(node_from);
        }

        public void listHasJustChanged()
        {
            lock (last_change_lock)
                last_change_time = DateTime.Now;
        }

        public DateTime getLastChangeTime()
        {
            DateTime lct;
            lock (last_change_lock)
            {
                lct = this.last_change_time;
            }
            return lct;
        }
    }
}
