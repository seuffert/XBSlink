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
        public const int MAX_ASK_CLOUDHELPER_COUNT = 3;
        public const int MIN_REFRESH_NODE_DELAY = 2;
        private volatile bool run_ping_nodes_loop = true;

        public volatile bool notify_on_new_node = true;
        public const String NOTIFICATION_SOUND_NODE_JOINED = "sounds/new_node.wav";
        public const String NOTIFICATION_SOUND_NODE_LEFT = "sounds/node_left.wav";

        private DateTime last_change_time;
        private Object last_change_lock = new Object();

        private Random my_random = new Random();

        public delegate void DeleteNodeHandler(string nickname);
        public event DeleteNodeHandler DeleteNode;


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

        public List<xbs_node> getXBSNodeListCopy()
        {
            List<xbs_node> node_list_copy;
            lock (this)
                node_list_copy = new List<xbs_node>(node_list);
            return node_list_copy;
            //return node_list;
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
            xbs_messages.addInfoMessage(" + Added new node: " + node, xbs_message_sender.NODELIST);
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
                        xbs_messages.addInfoMessage(" + removed node " + n, xbs_message_sender.NODELIST);
                        deleted_node = n;
                        node_list.Remove(n);

                        if (DeleteNode != null)
                            DeleteNode(n.nickname);

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
            if (node.get_xbox_count() > 0)
                foreach (xbs_xbox xbox in node.getXboxArray())
                    xbs_nat.getInstance().informOfRemovedDevice(xbox.mac);
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

        /// <summary>
        /// TODO: CHAT NEW METHOD
        /// </summary>
        /// <param name="NickName"></param>
        /// <returns></returns>
        public xbs_node findNode(string NickName)
        {

            try
            {
                lock (node_list)
                {
                    foreach (var item in node_list)
                    {
                        if (item.nickname == NickName)
                            return item;
                    }
                }
            }
            catch (Exception)
            {
            }
            return null;
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

        public xbs_node findNode(PhysicalAddress mac)
        {
            lock (this)
            {
                foreach (xbs_node n in node_list)
                    if (n.has_xbox(mac))
                        return n;
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
            xbs_messages.addInfoMessage(" + sending Nodelist to " + new_node, xbs_message_sender.NODELIST);
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
            xbs_messages.addInfoMessage(" + informing all nodes of new node " + new_node, xbs_message_sender.NODELIST);
            lock (this)
            {
                foreach (xbs_node node in node_list)
                    if (!node.Equals(new_node))
                        node.sendAddNodeMessage(new_node);
            }
        }

        public void informNodesOnDelNode(xbs_node node)
        {
            xbs_messages.addInfoMessage(" + informing all nodes of removed node " + node, xbs_message_sender.NODELIST);
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

        public void refreshAllNodes()
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
                    time_since_last_ping = (int)n.timeSinceLastPing.TotalSeconds;
                    time_since_last_pong = (int)n.timeSinceLastPong.TotalSeconds;
                    if (time_since_last_ping > xbs_node_list.MIN_PING_DELAY_SECONDS || time_since_last_pong > xbs_node_list.MIN_PING_DELAY_SECONDS)
                        n.sendPing();
                    
                    if (time_since_last_pong > xbs_node_list.MAX_PING_NO_RESPONSE_SECONDS)
                        del_list.Add(n);
                    else
                        if ((DateTime.Now.Second % 2) == 0) // every 2 seconds check if all node data is available
                        {
                            if (n.client_version == xbs_node.CLIENT_VERSION_UNKNOWN)
                                n.sendGetClientVersion();
                            if (n.nickname_received == false)
                                n.sendGetNickname();
                            if (n.last_ping_delay_ms < 0 )
                                n.sendPing();
                        }
                }
                foreach (xbs_node n in del_list)
                {
                    xbs_messages.addInfoMessage(" + removing node " + n + " (ping timeout)", xbs_message_sender.NODELIST);
                    n.sendDelNodeMessage(local_node);
                    node_list.Remove(n);
                    listHasJustChanged();
                }
            }
        }

        public xbs_node distributeDataPacket(PhysicalAddress dstMac, byte[] data)
        {
            lock (this)
            {
                foreach (xbs_node node in node_list)
                    if (node.has_xbox(dstMac))
                    {
                        node.sendDataMessage(ref data);
                        return node;
                    }
            }
            return null;
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
            xbs_messages.addInfoMessage(" + starting node survey service", xbs_message_sender.NODELIST);
#if !DEBUG
            try
            {
#endif
                while (run_ping_nodes_loop)
                {
                    if(xbs_udp_listener.getInstance()!=null)
                        refreshAllNodes();
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
                    foreach (xbs_node node in node_list)
                    {
                        if (node.get_xbox_count() > 0)
                            foreach (xbs_xbox xbox in node.getXboxArray())
                                xbs_nat.getInstance().informOfRemovedDevice(xbox.mac);

                        if (DeleteNode != null)
                            DeleteNode(node.nickname);

                    } node_list.Clear();
                }
            }
            listHasJustChanged();
        }

        public void tryAddingNode(xbs_node new_node, String cloudname)
        {
            if (findNodeInAddingList(new_node.ip_public, new_node.port_public) != null)
            {
#if DEBUG
                xbs_messages.addDebugMessage(" + node already in addingList: " + new_node, xbs_message_sender.NODELIST);
#endif
                return;
            }
            lock (node_list_adding)
                node_list_adding.Add(new_node);
            //new_node.sendAddNodeMessage(local_node);
            xbs_node_message_announce msg = new xbs_node_message_announce(new_node.getSendToIP(), new_node.getSendToPort());
            if (cloudname!=null)
                msg.addOption(xbs_node_message_announce.OPTION_CLOUDNAME, cloudname);
            new_node.sendNodeMessage(msg);
        }

        private void purgeAddedNodeFromAddingList(xbs_node new_node)
        {
#if DEBUG
            xbs_messages.addInfoMessage("+ trying to purged node from node_list_adding : " + new_node, xbs_message_sender.NODELIST);
#endif
            lock (node_list_adding)
            {
                for (int i=0;i<node_list_adding.Count;i++)
                    if (node_list_adding[i].Equals(new_node))
                    {
                        node_list_adding.RemoveAt(i);
#if DEBUG
                        xbs_messages.addInfoMessage("+ purged node from node_list_adding : " + new_node, xbs_message_sender.NODELIST);
#endif
                        break;
                    }
            }
        }

        private void checkNodesInAddingList()
        {
            TimeSpan time_elapsed;
            List<xbs_node> cloud_helper_node_list = new List<xbs_node>();
            List<xbs_node> nodes_to_remove_list = new List<xbs_node>();

            lock (node_list_adding)
            {
                if (node_list_adding.Count == 0)
                    return;
                foreach (xbs_node node in node_list_adding)
                {
                    time_elapsed = DateTime.Now - node.addedTime;
                    if (time_elapsed.TotalSeconds >= MAX_ADD_NODE_TIMEOUT_SECONDS)
                    {
                        if (node.ask_cloudhelper_count < MAX_ASK_CLOUDHELPER_COUNT)
                        {
                            cloud_helper_node_list.Add(node);
                            node.ask_cloudhelper_count++;
                            node.addedTime = DateTime.Now;
                        }
                        else
                        {
                            nodes_to_remove_list.Add(node);
#if DEBUG
                            xbs_messages.addDebugMessage("+ asked cloudhelper " + node.ask_cloudhelper_count + " times to help with node " + node + ", giving up.", xbs_message_sender.NODELIST, xbs_message_type.GENERAL);
#endif
                        }
                    }
                }

                // remove on reachable nodes from adding list
                foreach (xbs_node node in nodes_to_remove_list)
                    node_list_adding.Remove(node);
            }

            foreach (xbs_node node in cloud_helper_node_list)
                askCloudHelperToSendAddNodeMessage(node);
        }

        private void askCloudHelperToSendAddNodeMessage(xbs_node node)
        {
            xbs_node cloud_helper;
            lock (node_list)
            {
                cloud_helper = node_list.Count == 0 ? null : node_list[my_random.Next(0, node_list.Count - 1)];
            }
            if (cloud_helper == null)
                return;
            xbs_messages.addInfoMessage("+ asking ("+node.ask_cloudhelper_count+") cloud_helper (" + cloud_helper + ") for help to add node " + node, xbs_message_sender.NODELIST);
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

        public static xbs_node_list getInstance()
        {
            return (FormMain.node_list!=null) ? FormMain.node_list : xbs_console_app.node_list;
        }

        public int getNodeCount()
        {
            int node_count;
            lock (node_list)
                node_count = node_list.Count;
            return node_count;
        }
    }
}
