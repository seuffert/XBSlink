/**
 * Project: XBSlink: A XBox360 & PS3/2 System Link Proxy
 * File name: xbs_cloudlist.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Net;
using System.Windows.Forms;
using System.Web;
using System.Threading;
using System.Security.Cryptography;

namespace XBSlink
{
    class xbs_cloudlist_returncode
    {
        public const String RETURN_CODE_OK = "OK:";
        public const String RETURN_CODE_ERROR = "ERROR:";
    }

    class xbs_cloudlist_command
    {
        public const String CMD_GETLIST = "GETLIST";
        public const String CMD_JOIN    = "JOIN";
        public const String CMD_LEAVE   = "LEAVE";
        public const String CMD_UPDATE = "UPDATE";
        public const String CMD_STATS = "STATS";
        public const String CMD_SENDHELLO = "SENDHELLO";
    }

    class xbs_cloudlist_getparameters
    {
        public const String CMD         = "cmd";
        public const String CLOUDNAME   = "cloudname";
        public const String PASSWORD    = "password";
        public const String MAXNODES    = "maxnodes";
        public const String NODEIP      = "node_ip";
        public const String NODEPORT    = "node_port";
        public const String NICKNAME    = "nick";
        public const String COUNTNODES  = "countnodes";
        public const String UUID        = "uuid";
        public const String REACHABLE   = "reachable";
        public const String GETALLNODES = "getallnodes";
    }

    class xbs_cloud
    {
        public String name;
        public int node_count;
        public int max_nodes;
        public bool isPrivate = false;

        public xbs_cloud(String name, int node_count, int max_nodes, bool isPrivate)
        {
            this.name = name;
            this.node_count = node_count;
            this.max_nodes = max_nodes;
            this.isPrivate = isPrivate;
        }
    }

    class xbs_cloudlist
    {
        public const String DEFAULT_CLOUDLIST_SERVER = "http://www.secudb.de/~seuffert/xbslink/cloudlist/";
        public const int MIN_CLOUDNAME_LENGTH = 3;
        public const int UPDATE_INTERVAL_SECONDS = 29;

        public bool part_of_cloud = false;
        public String current_cloudname = null;
        public String uuid = null;
        public String cloudlist_url = null;

        private Thread update_thread = null;

        private List<xbs_cloud> cloudlist = new List<xbs_cloud>();

        public xbs_cloudlist()
        {
        }

        public bool loadCloudlistFromURL(String url)
        {
            string result = null;
            url = url + "?" + xbs_cloudlist_getparameters.CMD + "=" + xbs_cloudlist_command.CMD_GETLIST;
            WebClient client = new WebClient();
            client.Proxy = null;
            try
            {
                result = client.DownloadString(url);
            }
            catch (WebException wex)
            {
                // handle error
                MessageBox.Show(wex.Message);
                return false;
            }
            return parse_cloudlist(result);
        }

        private bool parse_cloudlist(String str)
        {
            String[] ret_array = str.Split(new char[]{'\n'}, StringSplitOptions.RemoveEmptyEntries);
            if (ret_array[0].StartsWith(xbs_cloudlist_returncode.RETURN_CODE_ERROR))
            {
                xbs_messages.addInfoMessage(" x cloudlist server error: " + ret_array[0]);
                return false;
            }
            else if (!ret_array[0].StartsWith(xbs_cloudlist_returncode.RETURN_CODE_OK))
            {
                xbs_messages.addInfoMessage(" x unknown response from cloudlist server ");
                return false;
            }
            lock (cloudlist)
            {
                cloudlist.Clear();
                for (int i = 1; i < ret_array.Length; i++)
                {
                    try
                    {
                        parseAndAddCloudFromURLString(ret_array[i]);
                    }
                    catch (Exception ex)
                    {
                        xbs_messages.addInfoMessage(" x error adding cloud to cloudlist: " + ex.ToString());
                    }
                }
            }
            return true;
        }

        private void parseAndAddCloudFromURLString( String s )
        {
            NameValueCollection query = HttpUtility.ParseQueryString(s);
            List<String> keys = new List<String>(query.AllKeys);
            if (keys.Contains(xbs_cloudlist_getparameters.CLOUDNAME) && keys.Contains(xbs_cloudlist_getparameters.MAXNODES) & keys.Contains(xbs_cloudlist_getparameters.PASSWORD) & keys.Contains(xbs_cloudlist_getparameters.COUNTNODES))
            {
                String cloudname = query[xbs_cloudlist_getparameters.CLOUDNAME];
                int node_count;
                int.TryParse(query[xbs_cloudlist_getparameters.COUNTNODES], out node_count);
                int max_nodes;
                int.TryParse(query[xbs_cloudlist_getparameters.MAXNODES], out max_nodes);
                bool isPrivate = query[xbs_cloudlist_getparameters.PASSWORD].ToUpper() == "TRUE";
                cloudlist.Add( new xbs_cloud(cloudname, node_count, max_nodes, isPrivate) );
            }
        }

        public static String getMD5hash( String str )
        {
            MD5 hasher = MD5.Create();
            byte[] bytes = hasher.ComputeHash( xbs_node_message.getUTF8BytesFromString(str) );
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            for (int i = 0; i < bytes.Length; i++)
                hex.Append(bytes[i].ToString("X2"));
            return hex.ToString();
        }

        public bool JoinOrCreateCloud(String url, String cloudname, String max_nodes, String password, IPAddress node_ip, int node_port, String nickname, bool reachable)
        {
            string result = null;

            if (password.Length > 0)
                password = xbs_cloudlist.getMD5hash(password);

            List<String> get_params = new List<String>();
            get_params.Add(xbs_cloudlist_getparameters.CMD + "=" + xbs_cloudlist_command.CMD_JOIN);
            get_params.Add(xbs_cloudlist_getparameters.CLOUDNAME + "=" + HttpUtility.UrlEncode(cloudname));
            get_params.Add(xbs_cloudlist_getparameters.MAXNODES + "=" + HttpUtility.UrlEncode(max_nodes));
            get_params.Add(xbs_cloudlist_getparameters.PASSWORD + "=" + HttpUtility.UrlEncode(password));
            get_params.Add(xbs_cloudlist_getparameters.NODEIP + "=" + HttpUtility.UrlEncode(node_ip.ToString()));
            get_params.Add(xbs_cloudlist_getparameters.NODEPORT + "=" + HttpUtility.UrlEncode(node_port.ToString()));
            get_params.Add(xbs_cloudlist_getparameters.NICKNAME + "=" + HttpUtility.UrlEncode(nickname));
            get_params.Add(xbs_cloudlist_getparameters.REACHABLE + "=" + (reachable ? 1 : 0));
            get_params.Add(xbs_cloudlist_getparameters.GETALLNODES + "=1");
            String full_url = url + "?" + String.Join("&", get_params.ToArray());
#if DEBUG
            xbs_messages.addDebugMessage(" x joining cloud: " + full_url);
#endif
            WebClient client = new WebClient();
            client.Proxy = null;
            try
            {
                result = client.DownloadString(full_url);
            }
            catch (WebException wex)
            {
                // handle error
                MessageBox.Show(wex.Message);
                return false;
            }
            if (!result.StartsWith(xbs_cloudlist_returncode.RETURN_CODE_OK))
            {
                MessageBox.Show("could not join cloud "+cloudname);
                return false;
            }
#if DEBUG
            xbs_messages.addDebugMessage(" x cloudlist server result: " + result.Replace("\n","|"));
#endif
            part_of_cloud = true;
            current_cloudname = cloudname;
            xbs_messages.addInfoMessage(" x joined cloud " + cloudname);
            String[] result_rows = result.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            uuid = result_rows[0].Split(':')[1];
#if DEBUG
            xbs_messages.addDebugMessage(" x cloud node UUID: " + uuid);
#endif
            cloudlist_url = url;
            if (result_rows.Length >= 2)
            {
                for (int row_num = 1; row_num < result_rows.Length; row_num++)
                {
                    IPAddress ip;
                    int port;
                    try
                    {
                        NameValueCollection node_data = HttpUtility.ParseQueryString(result_rows[row_num]);
                        ip = IPAddress.Parse(node_data[xbs_cloudlist_getparameters.NODEIP]);
                        port = int.Parse(node_data[xbs_cloudlist_getparameters.NODEPORT]);
                    }
                    catch (Exception)
                    {
                        xbs_messages.addInfoMessage("!! Error getting node contact from cloudlist server: " + result_rows[1]);
                        return false;
                    }
                    xbs_node_message_announce msg = new xbs_node_message_announce(ip, port);
                    if (xbs_udp_listener.getInstance()!=null)
                        xbs_udp_listener.getInstance().send_xbs_node_message(msg);
                }
            }
            startUpdateThread();
            return true;
        }

        public bool LeaveCloud()
        {
            string result = null;
            List<String> get_params = new List<String>();
            get_params.Add(xbs_cloudlist_getparameters.CMD + "=" + xbs_cloudlist_command.CMD_LEAVE);
            get_params.Add(xbs_cloudlist_getparameters.CLOUDNAME + "=" + HttpUtility.UrlEncode(current_cloudname));
            get_params.Add(xbs_cloudlist_getparameters.UUID + "=" + HttpUtility.UrlEncode(uuid));
            String url = cloudlist_url + "?" + String.Join("&", get_params.ToArray());
            WebClient client = new WebClient();
            client.Proxy = null;
            try
            {
                result = client.DownloadString(url);
            }
            catch (WebException wex)
            {
                // handle error
                MessageBox.Show(wex.Message);
                return false;
            }
            if (result.StartsWith(xbs_cloudlist_returncode.RETURN_CODE_ERROR))
            {
                MessageBox.Show(result);
                return false;
            }
            xbs_messages.addInfoMessage(" x left cloud " + current_cloudname);
            part_of_cloud = false;
            uuid = null;
            current_cloudname = null;
            if (update_thread!=null)
                if (update_thread.ThreadState != ThreadState.Stopped )
                    update_thread.Join();
            update_thread = null;
            if (xbs_node_list.getInstance() != null)
            {
                xbs_node_list.getInstance().sendLogOff();
                xbs_node_list.getInstance().clear_nodes();
            }
            cloudlist_url = null;
            return true;
        }

        private void startUpdateThread()
        {
            update_thread = new Thread(new ThreadStart(update_cloudlist_threadstart));
            update_thread.IsBackground = true;
            update_thread.Priority = ThreadPriority.AboveNormal;
            update_thread.Start();
        }

        private void update_cloudlist_threadstart()
        {
            DateTime last_update = DateTime.Now;
            List<String> get_params = new List<String>();
            TimeSpan ts = new TimeSpan();
            get_params.Add(xbs_cloudlist_getparameters.CMD + "=" + xbs_cloudlist_command.CMD_UPDATE);
            get_params.Add(xbs_cloudlist_getparameters.CLOUDNAME + "=" + HttpUtility.UrlEncode(current_cloudname));
            get_params.Add(xbs_cloudlist_getparameters.UUID + "=" + HttpUtility.UrlEncode(uuid));
            string result = null;
            String url = cloudlist_url + "?" + String.Join("&", get_params.ToArray());
            xbs_messages.addInfoMessage(" x started cloudlist updater");
#if !DEBUG
            try
            {
#endif
                while (part_of_cloud)
                {
                    ts = DateTime.Now - last_update;
                    if (ts.TotalSeconds > xbs_cloudlist.UPDATE_INTERVAL_SECONDS)
                    {
                        url = cloudlist_url + "?" + String.Join("&", get_params.ToArray());
                        xbs_messages.addDebugMessage(" x update of cloud status: "+url);
                        WebClient client = new WebClient();
                        client.Proxy = null;
                        try
                        {
                            result = client.DownloadString(url);
                        }
                        catch (WebException wex)
                        {
                            xbs_messages.addInfoMessage("!! could not updated status on cloudlist server. Error: "+wex.Message);
                        }
                        if (result==null || !(result is String))
                        {
                            
                        }
                        else if (!result.StartsWith(xbs_cloudlist_returncode.RETURN_CODE_OK))
                            xbs_messages.addInfoMessage("!! cloudlist server returned error on update");
                        else
                        {
                            String[] result_rows = result.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            xbs_messages.addDebugMessage(" x cloud list update returned rows: " + result_rows.Length);
                            if (result_rows.Length >= 2)
                                for (int row_num = 1; row_num < result_rows.Length; row_num++)
                                    updateNodeInCloud(result_rows[row_num]);
                        }
                        last_update = DateTime.Now;
                    }
                    Thread.Sleep(1000);
                }
#if !DEBUG
            }
            catch (Exception ex)
            {
                ExceptionMessage.ShowExceptionDialog("update_cloudlist service", ex);
            }
#endif
        }

        public void updateNodeInCloud( String row_data )
        {
            IPAddress ip;
            int port;
            try
            {
                NameValueCollection node_data = HttpUtility.ParseQueryString( row_data );
                ip = IPAddress.Parse(node_data[xbs_cloudlist_getparameters.NODEIP]);
                port = int.Parse(node_data[xbs_cloudlist_getparameters.NODEPORT]);
            }
            catch (Exception e)
            {
                xbs_messages.addInfoMessage("!! Error getting updating node data: " + e.Message);
                return;
            }
            xbs_node_list node_list = xbs_node_list.getInstance();
            if (node_list != null)
            {
                xbs_node node = node_list.findNode(ip, port);
                if (node == null)
                {
                    node = new xbs_node(ip, port);
                    if (!node_list.local_node.Equals(node))
                    {
#if DEBUG
                        xbs_messages.addDebugMessage(" x found new node in cloudlist update: " + node);
#endif
                        node_list.tryAddingNode(node);
                    }
                }
            }
        }

        public static bool askCloudServerForHello( String server, IPAddress node_ip, int node_port)
        {
            string result = null;
            List<String> get_params = new List<String>();
            get_params.Add(xbs_cloudlist_getparameters.CMD + "=" + xbs_cloudlist_command.CMD_SENDHELLO);
            get_params.Add(xbs_cloudlist_getparameters.NODEIP + "=" + HttpUtility.UrlEncode(node_ip.ToString()));
            get_params.Add(xbs_cloudlist_getparameters.NODEPORT + "=" + HttpUtility.UrlEncode(node_port.ToString()));
            String full_url = server + "?" + String.Join("&", get_params.ToArray());
            WebClient client = new WebClient();
            client.Proxy = null;
            try
            {
                result = client.DownloadString(full_url);
            }
            catch (WebException wex)
            {
                // handle error
                MessageBox.Show(wex.Message);
                return false;
            }
            return (result.StartsWith(xbs_cloudlist_returncode.RETURN_CODE_OK));
        }

        public xbs_cloud[] getCloudlistArray()
        {
            xbs_cloud[] clouds;
            lock (cloudlist)
                clouds = cloudlist.ToArray();
            return clouds;
        }

        public int cloud_count()
        {
            int count;
            lock (cloudlist)
                count = cloudlist.Count;
            return count;
        }
    }
}
