/**
 * Project: XBSlink: A XBox360 & PS3/2 System Link Proxy
 * File name: Form1.cs
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Permissions;
using System.Threading;
using PacketDotNet;
using PacketDotNet.Utils;
using SharpPcap;
using SharpPcap.LibPcap;
using SharpPcap.WinPcap;
using MiscUtil.Conversion;
using XBSlink.Properties;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Linq;
using System.Xml.Linq;


[assembly: RegistryPermissionAttribute(SecurityAction.RequestMinimum,
    ViewAndModify = "HKEY_CURRENT_USER")]

namespace XBSlink
{
    partial class FormMain : Form
    {
        public static xbs_udp_listener udp_listener = null;
        public static xbs_sniffer sniffer = null;
        public static xbs_node_list node_list = null;
        public static xbs_nat NAT = null;
        private xbs_upnp upnp = null;

        public DebugWindow debug_window = null;

        public static IPAddress external_ip = null;
        public static IPAddress internal_ip = null;

        private bool use_UPnP = true;

        private uint old_sniffer_packet_count = 0;
        private uint old_udp_in_count = 0;
        private uint old_udp_out_count = 0;

        private NotifyIcon notify_icon = null;
        private bool notify_icon_error_message_shown = false;
        private bool notify_icon_fatalerror_message_shown = false;
        private bool notify_icon_warning_message_shown = false;

        private int form1_width;

        private const int MAX_WAIT_START_ENGINE_SECONDS = 6;
        private DateTime app_start_time;
        private DateTime start_engine_started_at;
        public static bool abort_start_engine = false;
        private bool engine_started = false;

        private TabPage switch_tab = null;
        private bool autoswitch_on_chat_message = false;

        public static xbs_cloudlist cloudlist = null;
        public static Object askedCloudServerForHelloMessage_locker = new Object();

        private DateTime last_update_check = new DateTime(0);
        private DateTime last_nodelist_update = new DateTime(0);
        private DateTime last_nat_ippool_update = new DateTime(0);

        private DateTime last_resizeNATIPPoolHeaderHeader = DateTime.Now;
        private DateTime last_resizeCloudListHeader = DateTime.Now;
        private DateTime last_resizeNodeListHeader = DateTime.Now;

        SharpPcap.CaptureDeviceList pcap_devices = null;

        private Dictionary<IPAddress, GatewayIPAddressInformationCollection> network_device_gateways = new Dictionary<IPAddress, GatewayIPAddressInformationCollection>();

        private TabPage tab_newsfeed = null;

        public FormMain()
        {
#if DEBUG
            debug_window = new DebugWindow();
            debug_window.Show();
#endif
            InitializeComponent();
            if (System.Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                this.MaximumSize = new System.Drawing.Size(450, this.MaximumSize.Height);
                this.MinimumSize = new System.Drawing.Size(this.MaximumSize.Width, this.MinimumSize.Height);
            }

            
            if (!initializeCaptureDeviceList())
            {
                //throw new ApplicationException("no capture devices found");
            }

            app_start_time = DateTime.Now;
        }

        private void initializeCloudListView()
        {
            int width = listView_clouds.Width-20;
            for (int i = 1; i < listView_clouds.Columns.Count; i++)
                width -= listView_clouds.Columns[i].Width;
            listView_clouds.Columns[0].Width = width-listView_clouds.Columns.Count-1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // globally turn off Proxy auto detection
            WebRequest.DefaultWebProxy = null;

            node_list = new xbs_node_list();
            cloudlist = new xbs_cloudlist();
            upnp = new xbs_upnp();
            NAT = new xbs_nat();

            initializeCloudListView();

            timer_messages.Start();
            form1_width = this.Width;
            ShowVersionInfoMessages();

            textBox_local_Port.Text = xbs_udp_listener.standard_port.ToString();
            textBox_remote_port.Text = xbs_udp_listener.standard_port.ToString();

            initializeLocalIPList();
            initializeTrayIcon();
            initWithRegistryValues();
            initializeAboutWindow();

            tabControl1.SelectedTab = tabPage_settings;
            autoswitch_on_chat_message = checkBox_chatAutoSwitch.Checked;

            if (checkBox_showNewsFeed.Checked)
                loadNewsFeed(textBox_newsFeedUri.Text);
        }

        private void ShowVersionInfoMessages()
        {
            this.Text += " - Version " + xbs_settings.xbslink_version;
            xbs_messages.addInfoMessage("using pcap lib version : " + SharpPcap.Pcap.Version, xbs_message_sender.GENERAL);
#if DEBUG
            xbs_messages.addInfoMessage(".NET version : " + Environment.Version.ToString(), xbs_message_sender.GENERAL);
            xbs_messages.addInfoMessage("using PacketDotNet version " + System.Reflection.Assembly.GetAssembly(typeof(PacketDotNet.IpPacket)).GetName().Version.ToString(), xbs_message_sender.GENERAL);
            xbs_messages.addInfoMessage("using SharpPcap version " + SharpPcap.Version.VersionString, xbs_message_sender.GENERAL);
            xbs_messages.addInfoMessage("using Mono.NAT version " + System.Reflection.Assembly.GetAssembly(typeof(Mono.Nat.NatUtility)).GetName().Version.ToString(), xbs_message_sender.GENERAL);
#endif
        }

        private void initializeAboutWindow()
        {
            richTextBox_about.Rtf = Properties.Resources.about_xbslink;
        }

        private void initializeTrayIcon()
        {
            try
            {
                notify_icon = new NotifyIcon();
                notify_icon.Icon = new Icon(Properties.Resources.XBSlink, new Size(16, 16));
                notify_icon.Text = "XBSlink " + xbs_settings.xbslink_version;
                notify_icon.Visible = true;
                notify_icon.DoubleClick += new EventHandler(NotifyIconDoubleClick);
                notify_icon.ContextMenuStrip = contextMenuStrip_systray;
            }
            catch (Exception)
            {
                notify_icon = null;
            }
        }

        private bool initializeCaptureDeviceList()
        {
            DialogResult res = DialogResult.No;
            try
            {
                pcap_devices = CaptureDeviceList.Instance;
            }
            catch (PcapException pex)
            {
            }
            if (System.Environment.OSVersion.Platform == PlatformID.Win32NT && (pcap_devices==null || pcap_devices.Count < 1) )
            {
                res = MessageBox.Show(Resources.message_no_capture_devices_startNPF, "XBSlink error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (res == DialogResult.Yes)
                {
                    startNPFdriver();
                    try
                    {
                        pcap_devices = CaptureDeviceList.New();
                    }
                    catch (PcapException pex)
                    {
                    }
                }
            }

            if (pcap_devices!=null)
                foreach (LibPcapLiveDevice dev in pcap_devices)
                    comboBox_captureDevice.Items.Add(dev.Interface.FriendlyName + " (" + dev.Interface.Description + ")");

            if (comboBox_captureDevice.Items.Count > 0)
                comboBox_captureDevice.SelectedIndex = 0;
            else
            {
                if (System.Environment.OSVersion.Platform == PlatformID.Unix)
                    MessageBox.Show(Resources.message_no_capture_devices_unix, "XBSlink error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show(Resources.message_no_capture_devices, "XBSlink error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void initializeLocalIPList()
        {
            NetworkInterface[] network_interfaces = NetworkInterface.GetAllNetworkInterfaces();
            int local_ip_count = 0;
            int preferred_local_ip = -1;
            IPInterfaceProperties ip_properties;
            foreach (NetworkInterface ni in network_interfaces)
            {
                ip_properties = ni.GetIPProperties();
                foreach (UnicastIPAddressInformation uniCast in ip_properties.UnicastAddresses)
                    if (!IPAddress.IsLoopback(uniCast.Address) && uniCast.Address.AddressFamily != AddressFamily.InterNetworkV6)
                    {
                        if (uniCast.Address.ToString().Split('.')[0] != "169")
                        {
                            local_ip_count++;
                            comboBox_localIP.Items.Add(uniCast.Address.ToString());
                            network_device_gateways[uniCast.Address] = ip_properties.GatewayAddresses;
                            if (ni.OperationalStatus == OperationalStatus.Up && (ip_properties.GatewayAddresses.Count > 0))
                            {
                                if (!ip_properties.GatewayAddresses[0].Address.Equals(new IPAddress(0)))
                                    preferred_local_ip = local_ip_count;
                            }
                        }
                    }
            }
            if (comboBox_localIP.Items.Count > 0)
            {
                comboBox_localIP.SelectedIndex = (preferred_local_ip == -1) ? 0 : preferred_local_ip - 1;
            }

        }

        private void initWithRegistryValues()
        {
            Settings s = xbs_settings.settings;

            if (s.REG_SPECIAL_MAC_LIST != null)
                setMacListFromString( s.REG_SPECIAL_MAC_LIST );
            if (s.REG_REMOTE_HOST_HISTORY != null)
                setRemoteHostHistoryFromString( s.REG_REMOTE_HOST_HISTORY );
            if (s.REG_NAT_IP_POOL != null)
            {
                setNATIPPoolFromString( s.REG_NAT_IP_POOL );
                updateNATIPPoolListView();
            }

            comboBox_captureDevice.Text = s.REG_CAPTURE_DEVICE_NAME;
            comboBox_localIP.Text = s.REG_LOCAL_IP;
            textBox_local_Port.Text = s.REG_LOCAL_PORT.ToString();
            comboBox_RemoteHost.Text = s.REG_REMOTE_HOST;
            textBox_remote_port.Text = s.REG_REMOTE_PORT.ToString();
            checkbox_UPnP.Checked = s.REG_USE_UPNP;
            checkBox_enable_MAC_list.Checked = s.REG_ENABLE_SPECIAL_MAC_LIST;
            checkBox_mac_restriction.Checked = s.REG_ONLY_FORWARD_SPECIAL_MACS;
            checkBox_chatAutoSwitch.Checked = s.REG_CHAT_AUTOSWITCH;
            checkBox_chat_notify.Checked = s.REG_CHAT_SOUND_NOTIFICATION;
            checkBox_newNodeSound.Checked = s.REG_NEW_NODE_SOUND_NOTIFICATION;
            textBox_cloudlist.Text = (s.REG_CLOUDLIST_SERVER.Length!=0) ? s.REG_CLOUDLIST_SERVER : xbs_cloudlist.DEFAULT_CLOUDLIST_SERVER;
            checkBox_useCloudServerForPortCheck.Checked = s.REG_USE_CLOUDLIST_SERVER_TO_CHECK_INCOMING_PORT;
            textBox_chatNickname.Text = (s.REG_CHAT_NICKNAME.Length!=0) ? s.REG_CHAT_NICKNAME : xbs_chat.STANDARD_NICKNAME;
            checkBox_checkForUpdates.Checked = s.REG_CHECK4UPDATES;
            checkBox_nat_enable.Checked = s.REG_NAT_ENABLE;
            checkBox_filter_wellknown_ports.Checked = s.REG_FILTER_WELLKNOWN_PORTS;
            checkBox_NAT_enablePS3mode.Checked = s.REG_PS3_COMPAT_MODE_ENABLE;
            checkBox_excludeGatewayIPs.Checked = s.REG_SNIFFER_EXCLUDE_GATWAY_IPS;
            checkBox_chat_nodeInfoMessagesInChat.Checked = s.REG_CHAT_NODEINFOMESSAGES;
            checkBox_forward_all_high_port_broadcast.Checked = s.REG_SNIFFER_FORWARD_ALL_HIGH_PORT_BROADCASTS;
            checkBox_minimize2systray.Checked = s.REG_MINIMIZE2SYSTRAY;
            checkBox_preventSystemStandby.Checked = s.REG_PREVENT_SYSTEM_STANDY;
            xbs_chat.message_when_nodes_join_or_leave = s.REG_CHAT_NODEINFOMESSAGES;
            checkBox_showNewsFeed.Checked = s.REG_SHOW_NEWS_FEED;
            textBox_newsFeedUri.Text = s.REG_NEWS_FEED_URI;
            checkBox_switchToNewsTab.Checked = s.REG_NEWS_FEED_SWITCH_TO_TAB;

            if (checkBox_enable_MAC_list.Checked)
                checkBox_mac_restriction.Enabled = true;

            if (textBox_chatNickname.Text == "")
                textBox_chatNickname.Text = xbs_chat.STANDARD_NICKNAME;
        }
       
        private void saveRegistryValues()
        {
            Settings s = xbs_settings.settings;
            int out_int;
			if (comboBox_captureDevice.SelectedItem!=null)
            	s.REG_CAPTURE_DEVICE_NAME = comboBox_captureDevice.SelectedItem.ToString();
            s.REG_LOCAL_IP = comboBox_localIP.SelectedItem.ToString();
            if (int.TryParse(textBox_local_Port.Text, out out_int)) 
                s.REG_LOCAL_PORT = out_int;
            s.REG_REMOTE_HOST = comboBox_RemoteHost.Text;
            if (int.TryParse(textBox_remote_port.Text, out out_int)) 
                s.REG_REMOTE_PORT = out_int;            
            s.REG_USE_UPNP = checkbox_UPnP.Checked;
            s.REG_ENABLE_SPECIAL_MAC_LIST = checkBox_enable_MAC_list.Checked;
            s.REG_ONLY_FORWARD_SPECIAL_MACS = checkBox_mac_restriction.Checked;
            s.REG_SPECIAL_MAC_LIST = getMacListString();
            s.REG_REMOTE_HOST_HISTORY = getRemoteHostHistoryString();
            s.REG_CHAT_NICKNAME = textBox_chatNickname.Text;
            s.REG_CHAT_AUTOSWITCH = checkBox_chatAutoSwitch.Checked;
            s.REG_CHAT_SOUND_NOTIFICATION = checkBox_chat_notify.Checked;
            s.REG_NEW_NODE_SOUND_NOTIFICATION = checkBox_newNodeSound.Checked;
            s.REG_USE_CLOUDLIST_SERVER_TO_CHECK_INCOMING_PORT = checkBox_useCloudServerForPortCheck.Checked;
            s.REG_CHECK4UPDATES = checkBox_checkForUpdates.Checked;
            s.REG_NAT_ENABLE = checkBox_nat_enable.Checked;
            s.REG_NAT_IP_POOL = getNATIPPoolString();
            s.REG_FILTER_WELLKNOWN_PORTS = checkBox_filter_wellknown_ports.Checked;
            s.REG_PS3_COMPAT_MODE_ENABLE = checkBox_NAT_enablePS3mode.Checked;
            s.REG_SNIFFER_EXCLUDE_GATWAY_IPS = checkBox_excludeGatewayIPs.Checked;
            s.REG_CHAT_NODEINFOMESSAGES = checkBox_chat_nodeInfoMessagesInChat.Checked;
            s.REG_SNIFFER_FORWARD_ALL_HIGH_PORT_BROADCASTS = checkBox_forward_all_high_port_broadcast.Checked;
            s.REG_MINIMIZE2SYSTRAY = checkBox_minimize2systray.Checked;
            s.REG_PREVENT_SYSTEM_STANDY = checkBox_preventSystemStandby.Checked;
            s.REG_SHOW_NEWS_FEED = checkBox_showNewsFeed.Checked;
            s.REG_NEWS_FEED_URI = textBox_newsFeedUri.Text;
            s.REG_NEWS_FEED_SWITCH_TO_TAB = checkBox_switchToNewsTab.Checked;
            s.Save();
        }

        // -----------------------------------------------------

        private NetworkInterface getNetworkInterfaceForPDEV(ICaptureDevice pdev)
        {
            List<PcapAddress> addresses = new List<PcapAddress>();
            PhysicalAddress mac = null;
            if (pdev is LibPcapLiveDevice)
            {
                foreach (PcapAddress pa in ((LibPcapLiveDevice)pdev).Addresses)
                    if (pa.Addr.type == Sockaddr.AddressTypes.HARDWARE)
                        mac = pa.Addr.hardwareAddress;
            }
            else
            {
                return null;
            }
            if (mac == null)
                return null;
            NetworkInterface[] network_interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface ni in network_interfaces)
            {
                if (ni.GetPhysicalAddress().Equals(mac))
                    return ni;
            }
            return null;
        }

        private void comboBox_captureDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            button_start_engine.Enabled = true;
            if (pcap_devices!=null)
                if (pcap_devices.Count > 0)
                {
                    ICaptureDevice pdev = pcap_devices[comboBox_captureDevice.SelectedIndex];
                    NetworkInterface ni = getNetworkInterfaceForPDEV( pdev );
                    if (ni!=null)
                        if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                            MessageBox.Show("Please note that using a wireless connection as the capture device is very likely not going to work on Windows. sorry...", "XBSlink warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
        }

        private void button_start_engine_Click(object sender, EventArgs e)
        {
            if (!engine_started)
                engine_start();
            else
                engine_stop();
        }

        private void resume_start_engine()
        {
            if (ExceptionMessage.ABORTING)
                return;
            ICaptureDevice pdev;
            if (pcap_devices.Count == 0)
            {
                MessageBox.Show("XBSlink did not find any available network adapters in your system."
                    + Environment.NewLine
                    + "Does your user have enough system rights?", "XBSlink error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            try {
                pdev = pcap_devices[comboBox_captureDevice.SelectedIndex];
            } 
            catch (Exception)
            {
                MessageBox.Show("XBSlink could not set the capture device.", "XBSlink error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            try
            {
                udp_listener = new xbs_udp_listener(internal_ip, UInt16.Parse(textBox_local_Port.Text), node_list);
            }
            catch (Exception e)
            {
                xbs_messages.addInfoMessage("!! Socket Exception: could not bind to port " + textBox_local_Port.Text, xbs_message_sender.GENERAL, xbs_message_type.FATAL_ERROR);
                xbs_messages.addInfoMessage("!! the UDP socket is not ready to send or receive packets. Please check if another application is running on this port.", xbs_message_sender.GENERAL, xbs_message_type.FATAL_ERROR);
                System.Windows.Forms.MessageBox.Show(e.Message);
                abort_start_engine = true;
            }
            if (abort_start_engine || ExceptionMessage.ABORTING)
            {
                udp_listener = null;
                return;
            }

            try
            {
                if (use_UPnP && upnp.isUPnPavailable())
                {
                    external_ip = upnp.upnp_getPublicIP();
                    upnp.upnp_create_mapping(Mono.Nat.Protocol.Udp, udp_listener.udp_socket_port, udp_listener.udp_socket_port);
                }
            }
            catch (Exception)
            {
                xbs_messages.addInfoMessage("!! UPnP port mapping failed", xbs_message_sender.GENERAL, xbs_message_type.ERROR);
            }
            if (external_ip==null)
                external_ip = xbs_upnp.getExternalIPAddressFromWebsite();                        

            IPAddress local_node_ip = (external_ip == null) ? internal_ip : external_ip;
            node_list.local_node = new xbs_node(local_node_ip, udp_listener.udp_socket_port);
            node_list.local_node.nickname = textBox_chatNickname.Text;
            try
            {
                sniffer = new xbs_sniffer((LibPcapLiveDevice)pdev, checkBox_enable_MAC_list.Checked, generateSnifferMacList(), checkBox_mac_restriction.Checked, node_list, NAT, network_device_gateways[internal_ip], checkBox_excludeGatewayIPs.Checked);
                sniffer.start_capture();
            }
            catch (ArgumentException aex)
            {
                xbs_messages.addInfoMessage("!! starting Packet sniffer failed (1): " + aex.Message, xbs_message_sender.GENERAL, xbs_message_type.ERROR);
                abort_start_engine = true;
                udp_listener = null;
                sniffer = null;
                return;
            }
            catch (PcapException pcex)
            {
                xbs_messages.addInfoMessage("!! starting Packet sniffer failed (2): " + pcex.Message, xbs_message_sender.GENERAL, xbs_message_type.ERROR);
                abort_start_engine = true;
                udp_listener = null;
                sniffer = null;
                return;
            }

            if (ExceptionMessage.ABORTING)
                return;

            try
            {
                if (checkBox_useCloudServerForPortCheck.Checked)
                    checkIncomingPortWithCloudServer();
            }
            catch (Exception)
            {
                xbs_messages.addInfoMessage("!! open port check failed", xbs_message_sender.GENERAL, xbs_message_type.WARNING);
            }

            if (ExceptionMessage.ABORTING)
                return;

            timer1.Enabled = true;
            button_announce.Enabled = true;
            saveRegistryValues();
            xbs_messages.addInfoMessage("engine ready. waiting for incoming requests.", xbs_message_sender.GENERAL);
            switch_tab = tabPage_clouds;
            textBox_chatEntry.ReadOnly = false;
            textBox_chatEntry.Clear();
            autoswitch_on_chat_message = checkBox_chatAutoSwitch.Checked;

            textBox_CloudName.Enabled = true;
            textBox_CloudPassword.Enabled = true;
            textBox_CloudMaxNodes.Enabled = true;
            button_CloudJoin.Enabled = true;
            button_CloudLeave.Enabled = false;

            engine_started = true;
            button_start_engine.Enabled = true;
            button_start_engine.Text = "Stop Engine";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            engine_stop();
            if (notify_icon != null)
                if (notify_icon.Visible)
                    notify_icon.Visible = false;
            xbs_system_functions.restoreSystemSleepState();
        }

        private void engine_start()
        {
            clearMessagesAndNotifications();
            // show Messages to User
            tabControl1.SelectedTab = tabPage_messages;
            xbs_messages.addInfoMessage("starting Engine", xbs_message_sender.GENERAL);
            if (checkbox_UPnP.Checked)
                upnp.upnp_startDiscovery();
            start_engine_started_at = DateTime.Now;
            timer_startEngine.Start();
            button_start_engine.Enabled = false;
            textBox_chatNickname.ReadOnly = true;
            button_reset_settings.Enabled = false;
        }

        private void engine_stop()
        {
            button_announce.Enabled = false;
            if (cloudlist!=null)
                if (cloudlist.part_of_cloud)
                    cloudlist.LeaveCloud();
            timer1.Stop();
            xbs_settings.settings.Save();
            if (sniffer != null)
            {
                sniffer.close();
                sniffer = null;
            }
            if (udp_listener != null)
            {
                node_list.sendLogOff();
                udp_listener.shutdown();
                udp_listener = null;
            }
            if (upnp != null)
            {
                if (upnp.isUPnPavailable())
                    upnp.upnp_deleteAllPortMappings();
                upnp.upnp_stopDiscovery();
            }
            engine_started = false;
            xbs_messages.addInfoMessage("Engine stopped.", xbs_message_sender.GENERAL);

            listView_nodes.Items.Clear();
            NAT.ip_pool.freeAllIPs();
            updateNATIPPoolListView();

            button_start_engine.Text = "Start Engine";
            textBox_maininfo.Text = "Engine not started.";
            textBox_chatEntry.ReadOnly = true;
            textBox_chatEntry.Clear();
            textBox_CloudName.Enabled = false;
            textBox_CloudPassword.Enabled = false;
            textBox_CloudMaxNodes.Enabled = false;
            button_CloudJoin.Enabled = false;
            button_CloudLeave.Enabled = false;
            textBox_chatNickname.ReadOnly = false;
            button_reset_settings.Enabled = true;
        }

        private IPAddress Resolver(string Hostname)
        {
            IPHostEntry hostEintrag;
            IPAddress direct_ip = null;

            if (IPAddress.TryParse(Hostname, out direct_ip))
                return direct_ip;

            try
            {
                hostEintrag = Dns.GetHostEntry(Hostname);
            }
            catch (SocketException)
            {
                MessageBox.Show("Non existing host: " + Hostname, "XBSlink error", MessageBoxButtons.OK);
                return null;
            }
            foreach (IPAddress ip in hostEintrag.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip;
            return null;
        }

        private void button_announce_Click(object sender, EventArgs e)
        {
            if (comboBox_RemoteHost.Text.Length <= 3)
            {
                MessageBox.Show("remote host to short!");
                return;
            }
            IPAddress remote_ip = Resolver(comboBox_RemoteHost.Text);
            if (remote_ip == null)
            {
                MessageBox.Show("Could not resolve remote host!");
                return;
            }

            xbs_node_message_announce msg = new xbs_node_message_announce(remote_ip, int.Parse(textBox_remote_port.Text));
            udp_listener.send_xbs_node_message(msg);

            String remote_host = comboBox_RemoteHost.Text;
            if (!comboBox_RemoteHost.Items.Contains(remote_host))
            {
                while (comboBox_RemoteHost.Items.Count > 10)
                    comboBox_RemoteHost.Items.RemoveAt(10);
                comboBox_RemoteHost.Items.Insert(0,remote_host);
                saveRegistryValues();
            }
            else
            {
                comboBox_RemoteHost.Items.Remove(remote_host);
                comboBox_RemoteHost.Items.Insert(0, remote_host);
                comboBox_RemoteHost.Text = remote_host;
            }
        }

        private IPAddress getIPAddressForAdpater(SharpPcap.LibPcap.LibPcapLiveDevice pdev)
        {
            IPAddress ip;
            foreach (SharpPcap.LibPcap.PcapAddress pcap_ip in pdev.Addresses)
            {
                ip = pcap_ip.Addr.ipAddress;
                if (ip != null)
                    if (ip.AddressFamily != AddressFamily.InterNetworkV6)
                        return ip;
            }
            return null;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (switch_tab != null)
            {
                tabControl1.SelectedTab = switch_tab;
                switch_tab = null;
            }

            List<xbs_node> nodes = node_list.getXBSNodeListCopy();

            if (tabControl1.SelectedTab == tabPage_info)
                updateMainInfo(nodes);

            DateTime last_change_time = node_list.getLastChangeTime();
            if (last_change_time > last_nodelist_update)
            {
                if (tabControl1.SelectedTab == tabPage_info)
                    updateMainInfoListview(nodes, false);
                if (tabControl1.SelectedTab == tabPage_chat)
                    updateChatUserList(nodes);
                last_nodelist_update = last_change_time;
            }
            last_change_time = NAT.ip_pool.last_update;
            if (last_change_time > last_nat_ippool_update && tabControl1.SelectedTab == tabPage_nat)
            {
                updateNATIPPoolListView();
                last_nat_ippool_update = last_change_time;
            }

            updateStatusBar();
        }

        private void statusStrip1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
#if DEBUG
            if (debug_window != null)
            {
                if (debug_window.Visible)
                    debug_window.Close();
            }
            debug_window = new DebugWindow();
            debug_window.Show();
#endif
        }

        private void comboBox_external_SelectedIndexChanged(object sender, EventArgs e)
        {
			if (comboBox_localIP.SelectedItem!=null)
            	internal_ip = IPAddress.Parse(comboBox_localIP.SelectedItem.ToString());
        }

        private void checkbox_UPnP_CheckedChanged(object sender, EventArgs e)
        {
            use_UPnP = checkbox_UPnP.Checked;
        }

        private void NotifyIconDoubleClick(Object sender, EventArgs e)
        {
            this.Visible = (this.Visible) ? false : true;
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
                this.Activate();
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized && checkBox_minimize2systray.Checked)
                this.Visible = false;
        }

        private void updateMainInfo(List<xbs_node> nodes)
        {
            if (tabControl1.SelectedTab != tabPage_info)
                return;
            String text = "";

            if (cloudlist.part_of_cloud)
            {
                text += "Part of cloud \"" + cloudlist.current_cloudname + "\" (" + nodes.Count + " nodes)" + Environment.NewLine;
            }

            if (nodes.Count < 1)
            {
                text += "no XBSlink nodes known yet." + Environment.NewLine + Environment.NewLine;
                text += "to join another person enter the hostname and press Announce or " + Environment.NewLine;
                text += "load the cloud list and join a XBSlink cloud." + Environment.NewLine;
            }

            if (node_list!=null)
                if (node_list.local_node!=null)
                    text += Environment.NewLine + "Local node: " + node_list.local_node;

            if (sniffer != null)
            {
                PhysicalAddress[] local_xbox_macs = sniffer.getSniffedMACs();
                if (local_xbox_macs.Length > 0)
                {
                    text += Environment.NewLine + "Discovered local device(s):" + Environment.NewLine;
                    foreach (PhysicalAddress phy in local_xbox_macs)
                        text += " => " + phy + Environment.NewLine;
                }
            }
            textBox_maininfo.Text = text;

        }

        private void addUserToChatUserList(String name)
        {
            lock (listBox_chatUserList)
            {
                for (int i=1; i<=listBox_chatUserList.Items.Count; i++)
                    if ( ((String)listBox_chatUserList.Items[i-1]).CompareTo(name) >= 0 )
                    {
                        listBox_chatUserList.Items.Insert(i-1, name);
                        return;
                    }
                listBox_chatUserList.Items.Add(name);
            }
        }

        private void updateChatUserList( List<xbs_node> nodes )
        {
            if (tabControl1.SelectedTab != tabPage_chat)
                return;
            
            List<String> chat_names = new List<string>();
            foreach (xbs_node node in nodes)
                chat_names.Add(node.nickname);

            label_num_persons_in_chat.Text = nodes.Count.ToString();

            foreach (String name in chat_names)
                if (!listBox_chatUserList.Items.Contains(name))
                    addUserToChatUserList(name);
            
            List<String> to_remove = new List<String>();
            foreach (String name in listBox_chatUserList.Items)
                if (!chat_names.Contains(name))
                    to_remove.Add(name);
            lock (listBox_chatUserList)
                foreach (String name in to_remove)
                    listBox_chatUserList.Items.Remove(name);
        }

        private void updateMainInfoListview(List<xbs_node> nodes, bool update_all)
        {
            if (tabControl1.SelectedTab != tabPage_info)
                return;
            listView_nodes.BeginUpdate();
            foreach (xbs_node node in nodes)
                if (node.lastChangeTime > last_nodelist_update || update_all)
                    updateNodeInMainInfoList(node);
            if (node_list.getNodeCount() < listView_nodes.Items.Count)
                purgeDeletedNodesInMainInfo();
            listView_nodes.EndUpdate();
        }

        private void purgeDeletedNodesInMainInfo()
        {
            if (listView_nodes.Items.Count==0)
                return;
            try
            {
                for (int i = listView_nodes.Items.Count - 1; i >= 0; i--)
                {
                    ListViewItem lv_item = listView_nodes.Items[i];
                    IPAddress ip = IPAddress.Parse(lv_item.Text);
                    int port = int.Parse(lv_item.SubItems[1].Text.Split('/')[0]);
                    if (node_list.findNode(ip, port) == null)
                    {
#if DEBUG
                        xbs_messages.addDebugMessage(String.Format("purged Node {0}:{1} from nodeListView", ip, port), xbs_message_sender.GENERAL);
#endif
                        listView_nodes.Items.RemoveAt(i);
                    }
                }
            }
            catch (Exception ex)
            {
                xbs_messages.addInfoMessage( "!! Error while purging node from main nodeListView", xbs_message_sender.GENERAL, xbs_message_type.ERROR);
            }
        }

        private void updateNodeInMainInfoList(xbs_node node)
        {
            ListViewItem lv_item = new ListViewItem(node.ip_public.ToString());
            
            lv_item.SubItems.Add((node.port_sendfrom == node.port_public) ? node.port_public.ToString() : node.port_public + "/" + node.port_sendfrom);
            String ping = (node.last_ping_delay_ms >= 0) ? node.last_ping_delay_ms + "ms" : "N/A";
            lv_item.SubItems.Add(ping);
            lv_item.SubItems.Add(node.client_version);
            lv_item.SubItems.Add(node.nickname);
            lv_item.BackColor = (node.get_xbox_count() == 0) ? Color.FromArgb(255, 235, 235) : Color.FromArgb(235, 255, 235);
            lv_item.Name = lv_item.Text + lv_item.SubItems[1];

            int index = listView_nodes.Items.IndexOfKey(lv_item.Name);
            ListViewItem lv_item_in_list = (index>=0) ? listView_nodes.Items[index] : null;
            if (lv_item_in_list != null)
            {
                for (int i=2; i<=4; i++)
                    if (lv_item_in_list.SubItems[i].Text != lv_item.SubItems[i].Text)
                        lv_item_in_list.SubItems[i].Text = lv_item.SubItems[i].Text;
                if (lv_item.BackColor != lv_item_in_list.BackColor)
                    lv_item_in_list.BackColor = lv_item.BackColor;
            }
            else
                listView_nodes.Items.Add(lv_item);
        }

        private void updateStatusBar()
        {
            UInt32 sniffer_packet_count = xbs_sniffer_statistics.packet_count;
            uint pps = sniffer_packet_count - old_sniffer_packet_count;
            toolStripStatusLabel_sniffer_in.Text = pps.ToString();
            old_sniffer_packet_count = sniffer_packet_count;

            uint udp_in, udp_out;
            udp_in = xbs_udp_listener_statistics.getPacketsIn();
            udp_out = xbs_udp_listener_statistics.getPacketsOut();
            uint udp_in_pps = udp_in - old_udp_in_count;
            uint udp_out_pps = udp_out - old_udp_out_count;
            toolStripStatusLabel_udp_in.Text = udp_in_pps.ToString();
            toolStripStatusLabel_udp_out.Text = udp_out_pps.ToString();
            old_udp_in_count = udp_in;
            old_udp_out_count = udp_out;
        }

        private void button_save_settings_Click(object sender, EventArgs e)
        {
            saveRegistryValues();
            toolTip2.Show("settings saved.", button_save_settings, 0, -20, 2000);
        }

        private void button_add_MAC_Click(object sender, EventArgs e)
        {
            if (button_add_MAC.Enabled == false)
                return;
            PhysicalAddress mac = parseMacAddress(textBox_add_MAC.Text);
            if (mac != null)
            {
                if (!listBox_MAC_list.Items.Contains(mac.ToString()))
                    addMacToMacList(mac.ToString());
                textBox_add_MAC.Text = "";
            }
        }

        private void listBox_MAC_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            button_del_MAC.Enabled = (listBox_MAC_list.SelectedIndex >= 0);
        }

        private void textBox_add_MAC_TextChanged(object sender, EventArgs e)
        {
            button_add_MAC.Enabled = (parseMacAddress(textBox_add_MAC.Text) != null);
        }

        private PhysicalAddress parseMacAddress(String mac_str)
        {
            String mac_text = mac_str.Replace(":", "").ToUpper().Replace("-","");
            PhysicalAddress mac;

            if (mac_text.Length != 12)
                return null;
            try
            {
                mac = PhysicalAddress.Parse(mac_text);
            }
            catch (System.FormatException)
            {
                return null;
            }
            return mac;
        }

        private void button_del_MAC_Click(object sender, EventArgs e)
        {
            if (listBox_MAC_list.SelectedIndex < 0)
                return;
            textBox_add_MAC.Text = (String)listBox_MAC_list.Items[listBox_MAC_list.SelectedIndex];
            listBox_MAC_list.Items.RemoveAt(listBox_MAC_list.SelectedIndex);
            if (listBox_MAC_list.Items.Count < 1)
            {
                button_del_MAC.Enabled = false;
                //checkBox_mac_restriction.Checked = false;
                checkBox_enable_MAC_list.Checked = false;
                sniffer.pdev_filter_use_special_macs = false;
            }
            setSnifferMacList();
        }

        private String getMacListString()
        {
            if (listBox_MAC_list.Items.Count == 0)
                return "";
            String[] strs = new String[listBox_MAC_list.Items.Count];
            listBox_MAC_list.Items.CopyTo(strs, 0);
            return String.Join(",", strs);
        }

        private void setMacListFromString(String mac_list)
        {
            listBox_MAC_list.Items.Clear();
            if (mac_list.Length==0)
                return;
            String[] macs = mac_list.Split(',');
            foreach (String mac in macs)
                addMacToMacList(mac);
        }

        private void setNATIPPoolFromString(String data)
        {
            listView_nat_IPpool.Items.Clear();
            NAT.ip_pool.Clear();
            if (data.Length == 0)
                return;

            foreach (String ip_str in data.Split(';'))
                if (ip_str.Length >= 15)
                {
                    String[] ip_mask = ip_str.Split('/');
                    if (ip_mask.Length == 2)
                        NAT.ip_pool.addIPToPool(ip_mask[0], ip_mask[1]);
                }
        }

        private void setRemoteHostHistoryFromString(String remoteHostList)
        {
            comboBox_RemoteHost.Items.Clear();
            if (remoteHostList.Length == 0)
                return;
            foreach (String remoteHost in remoteHostList.Split(','))
                comboBox_RemoteHost.Items.Add(remoteHost);
        }

        private String getRemoteHostHistoryString()
        {
            if (comboBox_RemoteHost.Items.Count == 0)
                return "";
            String[] strs = new String[comboBox_RemoteHost.Items.Count];
            comboBox_RemoteHost.Items.CopyTo(strs, 0);
            return String.Join(",", strs);
        }

        private String getNATIPPoolString()
        {
            xbs_nat_entry[] items = NAT.ip_pool.getEntriesArray();
            if (items.Length == 0)
                return null;
            String ip_str = "";
            for (int i = 0; i < items.Length; i++)
                ip_str += items[i].natted_source_ip + "/" + items[i].natted_source_ip_netmask + ";";
            return ip_str;
        }

        private void addMacToMacList(String mac)
        {
            if (!listBox_MAC_list.Items.Contains(mac))
            {
                listBox_MAC_list.Items.Add(mac);
                setSnifferMacList();
            }
        }

        private List<PhysicalAddress> generateSnifferMacList()
        {
            List<PhysicalAddress> mac_list = new List<PhysicalAddress>();
            foreach (String mac in listBox_MAC_list.Items)
                mac_list.Add(PhysicalAddress.Parse(mac));
            return mac_list;
        }

        private void setSnifferMacList()
        {
            if (sniffer != null)
                sniffer.setSpecialMacPacketFilter( generateSnifferMacList() );
        }

        private void checkBox_enable_MAC_list_CheckedChanged(object sender, EventArgs e)
        {
            if (listBox_MAC_list.Items.Count != 0)
            {
                if (sniffer != null)
                {
                    sniffer.pdev_filter_use_special_macs = checkBox_enable_MAC_list.Checked;
                    sniffer.setPdevFilter();
                }
            }
            else
            {
                if (checkBox_enable_MAC_list.Checked)
                {
                    MessageBox.Show(Resources.message_specialmaclist_empty, "XBSlink information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    checkBox_enable_MAC_list.Checked = false;
                }
            }
        }

        private void clearMessagesAndNotifications()
        {
            lock (listBox_messages)
                listBox_messages.Items.Clear();
            toolStripStatusLabel_icon.Image = Resources.ok_16;
            notify_icon_warning_message_shown = false;
            notify_icon_fatalerror_message_shown = false;
            notify_icon_error_message_shown = false;
        }
        private void button_clearMessages_Click(object sender, EventArgs e)
        {
            clearMessagesAndNotifications();
        }

        private void timer_messages_Tick(object sender, EventArgs e)
        {
            bool added_messages = false;
#if !DEBUG
            try
            {
#endif
            bool fatal_error_message = false;
            bool error_message = false;
            bool warning_message = false;
            while (xbs_messages.getInfoMessageCount() > 0)
            {
                added_messages = true;
                xbs_message msg = xbs_messages.DequeueInfoMessage();
                listBox_messages.Items.Add( msg.ToString() );
                if (msg.type == xbs_message_type.FATAL_ERROR)
                    fatal_error_message = true;
                else if (msg.type == xbs_message_type.ERROR)
                    error_message = true;
                else if (msg.type == xbs_message_type.WARNING)
                    warning_message = true;
            }
            if (added_messages)
                listBox_messages.SelectedIndex = listBox_messages.Items.Count - 1;

            if (error_message || fatal_error_message && toolStripStatusLabel_icon.Image != Resources.error_16)
                toolStripStatusLabel_icon.Image = Resources.error_16;
            else if (warning_message && toolStripStatusLabel_icon.Image!=Resources.error_16)
                toolStripStatusLabel_icon.Image = Resources.warning_16;

            if (notify_icon != null)
            {
                if (fatal_error_message && notify_icon_fatalerror_message_shown == false)
                {
                    notify_icon_fatalerror_message_shown = true;
                    notify_icon.ShowBalloonTip(10000, "XBSlink fatal error", Resources.notifyicon_fatal_error_message, ToolTipIcon.Error);
                }
                else if (error_message && notify_icon_error_message_shown == false && notify_icon_fatalerror_message_shown == false)
                {
                    notify_icon_error_message_shown = true;
                    notify_icon.ShowBalloonTip(10000, "XBSlink error", Resources.notifyicon_error_message, ToolTipIcon.Error);
                }
                else if (warning_message && notify_icon_warning_message_shown == false && notify_icon_error_message_shown == false && notify_icon_fatalerror_message_shown == false)
                {
                    notify_icon_warning_message_shown = true;
                    notify_icon.ShowBalloonTip(10000, "XBSlink warning", Resources.notifyicon_warning_message, ToolTipIcon.Warning);
                }
            }

            added_messages = false;
            while (xbs_messages.getChatMessageCount() > 0)
            {
                added_messages = true;
                textBox_chatMessages.Text += xbs_messages.DequeueChatMessage().text;
            }
            if (added_messages)
            {
                if (autoswitch_on_chat_message)
                    tabControl1.SelectedTab = tabPage_chat;
                textBox_chatMessages.SelectionStart = textBox_chatMessages.Text.Length;
                textBox_chatMessages.ScrollToCaret();
            }

            if ((DateTime.Now - app_start_time).TotalSeconds >= 5)
                if (checkBox_checkForUpdates.Checked && ((DateTime.Now - last_update_check).TotalHours >= xbs_settings.PROGRAM_UPDATE_CHECK_HOURS_INTERVAL))
                    checkForProgramUpdates();
#if !DEBUG
            }
            catch (Exception ex)
            {
                ExceptionMessage.ShowExceptionDialog("main timer service", ex);
            }
#endif
        }

        private bool isDigitOrControlChar( char c )
        {
            return (Char.IsControl(c) || Char.IsDigit(c));
        }

        private void textBox_remote_port_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (handlePlusMinusInTextBox((char)e.KeyChar, textBox_remote_port, 1, 65536) || !isDigitOrControlChar(e.KeyChar));
        }

        private void textBox_local_Port_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = ( handlePlusMinusInTextBox( (char)e.KeyChar, textBox_local_Port, 1, 65536) || !isDigitOrControlChar(e.KeyChar));
        }

        private void timer_startEngine_Tick(object sender, EventArgs e)
        {
            TimeSpan elapes_time = DateTime.Now - start_engine_started_at;
            bool upnp_discovery_finished = (checkbox_UPnP.Checked && upnp.isUPnPavailable()) || checkbox_UPnP.Checked==false;
            if (upnp_discovery_finished || elapes_time.TotalSeconds >= MAX_WAIT_START_ENGINE_SECONDS)
            {
                timer_startEngine.Stop();
                resume_start_engine();
            }
        }

        private void textBox_local_Port_Leave(object sender, EventArgs e)
        {
            if (textBox_local_Port.Text.Length == 0)
                textBox_local_Port.Text = xbs_udp_listener.standard_port.ToString();
        }

        private void textBox_remote_port_Leave(object sender, EventArgs e)
        {
            if (textBox_remote_port.Text.Length == 0)
                textBox_remote_port.Text = xbs_udp_listener.standard_port.ToString();
        }

        private void textBox_chatNickname_Leave(object sender, EventArgs e)
        {
            if (textBox_chatNickname.Text.Length == 0)
                textBox_chatNickname.Text = xbs_chat.STANDARD_NICKNAME;
        }

        private void button_clearChat_Click(object sender, EventArgs e)
        {
            textBox_chatMessages.Clear();
        }

        private void textBox_chatEntry_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                if (textBox_chatEntry.Text.Length > 0)
                {
                    xbs_chat.sendChatMessage(textBox_chatEntry.Text);
                    xbs_chat.addLocalMessage(textBox_chatEntry.Text);
                }
                textBox_chatEntry.Clear();
            }
        }

        private void checkBox_chatAutoSwitch_CheckedChanged(object sender, EventArgs e)
        {
            this.autoswitch_on_chat_message = checkBox_chatAutoSwitch.Checked;
        }

        private bool handlePlusMinusInTextBox(char pressed_key, TextBox tb, int min, int max)
        {
            int i;
            try
            {
                i = int.Parse(tb.Text);
            }
            catch (Exception)
            {
                return false;
            }

            switch (pressed_key)
            {
                case '+':
                    i++;
                    break;
                case '-':
                    i--;
                    break;
                default:
                    return false;
            }
            if (i > max)
                i = max;
            else if (i<min)
                i = min;
            tb.Text = i.ToString();
            return true;
        }

        private void buttonLoadCloudlist_Click(object sender, EventArgs e)
        {
            bool ret = cloudlist.loadCloudlistFromURL( textBox_cloudlist.Text );
            if (ret)
            {
                xbs_cloud[] clouds = cloudlist.getCloudlistArray();
                if (clouds.Length > 0)
                {
                    initCloudListView();
                    foreach (xbs_cloud cloud in clouds)
                    {
                        ListViewItem lv_item = new ListViewItem(cloud.name);
                        lv_item.SubItems.Add(cloud.node_count.ToString());
                        lv_item.SubItems.Add(cloud.max_nodes.ToString());
                        if (cloud.isPrivate)
                            lv_item.ImageIndex = 0;
                        listView_clouds.Items.Add(lv_item);
                    }
                    toolTip2.Show(clouds.Length + " clouds loaded.", buttonLoadCloudlist, 0, -20, 2000);
                }
                else
                    toolTip2.Show("no clouds available on server.", buttonLoadCloudlist, 0, -20, 2000);

                xbs_settings.settings.REG_CLOUDLIST_SERVER = textBox_cloudlist.Text;
            }
        }

        private void initCloudListView()
        {
            listView_clouds.Items.Clear();
            ImageList il = new ImageList();
            il.Images.Add(Properties.Resources.icon_key);
            listView_clouds.SmallImageList = il;
        }

        private void button_CloudJoin_Click(object sender, EventArgs e)
        {
            if (textBox_CloudName.Text.Length>=xbs_cloudlist.MIN_CLOUDNAME_LENGTH)
                join_cloud();
            else
                toolTip2.Show("The cloudlist name must be at least 3 letters!", button_CloudJoin, 0, -20, 2000);
        }

        private void textBox_CloudMaxNodes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textBox_CloudMaxNodes.Text.Length == 0)
                textBox_CloudMaxNodes.Text = "10";
            e.Handled = (handlePlusMinusInTextBox((char)e.KeyChar, textBox_CloudMaxNodes, 2, 65536) || !isDigitOrControlChar(e.KeyChar));
        }

        private void button_CloudLeave_Click(object sender, EventArgs e)
        {
            bool ret = cloudlist.LeaveCloud();
            if (ret)
            {
                toolTip2.Show("left " + textBox_CloudName.Text, button_CloudLeave, 0, -20, 2000);
                button_CloudLeave.Enabled = false;
                button_CloudJoin.Enabled = true;
                textBox_CloudName.Enabled = true;
                textBox_CloudMaxNodes.Enabled = true;
                textBox_CloudPassword.Enabled = true;
                sniffer.clearKnownMACsFromRemoteNodes();
                sniffer.setPdevFilter();
                purgeDeletedNodesInMainInfo();
            }
        }

        private void join_cloud()
        {
            bool ret = cloudlist.JoinOrCreateCloud(textBox_cloudlist.Text, textBox_CloudName.Text, textBox_CloudMaxNodes.Text, textBox_CloudPassword.Text, node_list.local_node.ip_public, node_list.local_node.port_public, node_list.local_node.nickname, xbs_upnp.isPortReachable, xbs_settings.xbslink_version);
            if (ret)
            {
                toolTip2.Show("joined " + textBox_CloudName.Text, button_CloudJoin, 0, -20, 2000);
                button_CloudLeave.Enabled = true;
                button_CloudJoin.Enabled = false;
                textBox_CloudName.Enabled = false;
                textBox_CloudMaxNodes.Enabled = false;
                textBox_CloudPassword.Enabled = false;

                switch_tab = tabPage_info;
            }
        }

        private void listView_clouds_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cloudlist!=null && cloudlist.part_of_cloud==false)
                if (listView_clouds.SelectedItems.Count == 1)
                {
                    textBox_CloudName.Text = listView_clouds.SelectedItems[0].Text;
                    textBox_CloudMaxNodes.Text = listView_clouds.SelectedItems[0].SubItems[2].Text;
                }
        }

        private void textBox_cloudlist_Leave(object sender, EventArgs e)
        {
            if (textBox_cloudlist.Text.Length == 0)
                textBox_cloudlist.Text = xbs_cloudlist.DEFAULT_CLOUDLIST_SERVER;
        }

        private void checkBox_chat_notify_CheckedChanged(object sender, EventArgs e)
        {
            xbs_chat.notify_on_incoming_message = checkBox_chat_notify.Checked;
        }

        private void checkBox_newNodeSound_CheckedChanged(object sender, EventArgs e)
        {
            node_list.notify_on_new_node = checkBox_newNodeSound.Checked;
        }

        private void button_messages_copy_Click(object sender, EventArgs e)
        {
            String str = "";
            foreach (String s in listBox_messages.Items)
                str += s + Environment.NewLine;
            Clipboard.SetText(str);
        }

        private void checkIncomingPortWithCloudServer()
        {
            xbs_messages.addInfoMessage("contacting cloud server...", xbs_message_sender.GENERAL);
            if (xbs_cloudlist.askCloudServerForHello(textBox_cloudlist.Text, node_list.local_node.ip_public, node_list.local_node.port_public))
            {
                lock (udp_listener._locker_HELLO)
                {
                    if (!xbs_upnp.isPortReachable)
                        Monitor.Wait(udp_listener._locker_HELLO, 1000);
                }

                if (xbs_upnp.isPortReachable == false)
                {
                    xbs_messages.addInfoMessage("!! cloudlist server HELLO timeout. incoming Port is CLOSED", xbs_message_sender.GENERAL, xbs_message_type.WARNING);
                    MessageBox.Show(
                        "Your XBSlink is not reachable from the internet (port closed)." + Environment.NewLine +
                        "Please configure your router and firewall to forward a port to your computer or use UPnP where available." + Environment.NewLine + Environment.NewLine + 
                        "Other XBSlink will not be able to connect to you directly!",
                        "XBSlink incoming port information",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation
                    );
                }
                else
                    xbs_messages.addInfoMessage("incoming Port is OPEN", xbs_message_sender.GENERAL);
            }
        }

        private void checkForProgramUpdates()
        {
            last_update_check = DateTime.Now;
            String result = xbs_settings.getOnlineProgramVersion();
            if (result != null)
            {
                int new_version_found = result.CompareTo(xbs_settings.xbslink_version);
                if (new_version_found > 0)
                {
                    DialogResult res = MessageBox.Show("A new version of XBSlink is available! (v" + result + ")" + Environment.NewLine + "Would you like to visit the homepage now?", "XBSlink update available", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (res == DialogResult.Yes)
                        System.Diagnostics.Process.Start(Resources.url_xbslink_website);
                }
                else if (new_version_found < 0)
                    xbs_messages.addInfoMessage("Latest XBSlink version found: v" + result, xbs_message_sender.GENERAL);
                else
                    xbs_messages.addInfoMessage("You are using the latest XBSlink version.", xbs_message_sender.GENERAL);
            }
        }

        public String getAllMessages()
        {
            String[] ret_array = new String[ listBox_messages.Items.Count ];
            listBox_messages.Items.CopyTo(ret_array,0);            
            return String.Join(Environment.NewLine, ret_array);
        }

        private void startNPFdriver()
        {
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("cmd", "/C " + "net start npf");
            psi.UseShellExecute = true;
            psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            psi.Verb = "runas";
            System.Diagnostics.Process.Start(psi).WaitForExit();
        }

        private void checkBox_mac_restriction_CheckedChanged(object sender, EventArgs e)
        {
            if (sniffer != null)
            {
                sniffer.pdev_filter_only_forward_special_macs = checkBox_mac_restriction.Checked;
                sniffer.setPdevFilter();
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<xbs_node> nodes = node_list.getXBSNodeListCopy();
            if (tabControl1.SelectedTab == tabPage_chat)
            {
                textBox_chatMessages.SelectionStart = textBox_chatMessages.Text.Length;
                textBox_chatMessages.ScrollToCaret();
                updateChatUserList(nodes);
            }
            else if (tabControl1.SelectedTab == tabPage_info)
            {
                updateMainInfo(nodes);
                updateMainInfoListview(nodes, true);
            }
            else if (tabControl1.SelectedTab == tabPage_nat)
                updateNATIPPoolListView();
            else if (tabControl1.SelectedTab == tabPage_clouds)
                resizeCloudListHeader();
        }

        private void resizeNodeListHeader()
        {
            if ((DateTime.Now - last_resizeNodeListHeader).TotalMilliseconds < 100)
                return;
            last_resizeNodeListHeader = DateTime.Now;
            int size = listView_nodes.ClientSize.Width - columnHeader_nodeIP.Width - columnHeader_nodePort.Width - columnHeader_nodePing.Width - columnHeader_nodeVersion.Width - 2;
            if (size > 0 && columnHeader_nodeNickname.Width != size)
                columnHeader_nodeNickname.Width = size;
        }

        private void resizeCloudListHeader()
        {
            if ((DateTime.Now - last_resizeCloudListHeader).TotalMilliseconds < 100)
                return;
            last_resizeCloudListHeader = DateTime.Now;
            int size = listView_clouds.ClientSize.Width - columnHeader_cloudlistmaxnodes.Width - columnHeader_cloudlistnodecount.Width - 2;
            if (size>0 && columnHeader_cloudlistname.Width != size)
                columnHeader_cloudlistname.Width = size;
        }

        private void resizeNATIPPoolHeaderHeader()
        {
            if ((DateTime.Now - last_resizeNATIPPoolHeaderHeader).TotalMilliseconds < 100)
                return;
            last_resizeNATIPPoolHeaderHeader = DateTime.Now;
            int size = listView_nat_IPpool.ClientSize.Width - columnHeader_nat_ippool_localIP.Width - columnHeader_nat_ippool_device.Width - columnHeader_nat_ippool__originalIP.Width - 2;
            if (size > 0 && columnHeader_nat_ippool_node.Width != size)
                columnHeader_nat_ippool_node.Width = size;
        }

        private void listView_nodes_Resize(object sender, EventArgs e)
        {
            try
            {
                listView_nodes.BeginUpdate();
                resizeNodeListHeader();
                listView_nodes.Refresh();
                listView_nodes.EndUpdate();
            }
            catch (Exception)
            {
            }
        }

        private void listView_clouds_Resize(object sender, EventArgs e)
        {
            try
            {
                listView_clouds.BeginUpdate();
                resizeCloudListHeader();
                listView_clouds.Refresh();
                listView_clouds.EndUpdate();
            }
            catch (Exception)
            {
            }
        }

        private void listView_nat_IPpool_Resize(object sender, EventArgs e)
        {
            try
            {
                listView_nat_IPpool.BeginUpdate();
                resizeNATIPPoolHeaderHeader();
                listView_nat_IPpool.Refresh();
                listView_nat_IPpool.EndUpdate();
            }
            catch (Exception)
            {
            }

        }

        private void textBox_nat_iprange_from_TextChanged(object sender, EventArgs e)
        {
            if (textBox_nat_iprange_from.Text == "From")
                textBox_nat_iprange_from.ForeColor = Color.LightGray;
            else
                textBox_nat_iprange_from.ForeColor = SystemColors.WindowText;
        }

        private void textBox_nat_iprange_from_Enter(object sender, EventArgs e)
        {
            if (textBox_nat_iprange_from.Text == "From")
                textBox_nat_iprange_from.Text = "";
        }

        private void textBox_nat_iprange_from_Leave(object sender, EventArgs e)
        {
            if (textBox_nat_iprange_from.Text == "")
                textBox_nat_iprange_from.Text = "From";
        }

        private void textBox_nat_iprange_to_TextChanged(object sender, EventArgs e)
        {
            if (textBox_nat_iprange_to.Text == "To")
                textBox_nat_iprange_to.ForeColor = Color.LightGray;
            else
                textBox_nat_iprange_to.ForeColor = SystemColors.WindowText;
        }

        private void textBox_nat_iprange_to_Enter(object sender, EventArgs e)
        {
            if (textBox_nat_iprange_to.Text == "To")
                textBox_nat_iprange_to.Text = (textBox_nat_iprange_from.Text.Length >= 7) ? textBox_nat_iprange_from.Text : "";
        }

        private void textBox_nat_iprange_to_Leave(object sender, EventArgs e)
        {
            if (textBox_nat_iprange_to.Text == "")
                textBox_nat_iprange_to.Text = "To";
        }

        private void checkBox_nat_enable_CheckedChanged(object sender, EventArgs e)
        {
            NAT.NAT_enabled = checkBox_nat_enable.Checked;
            //checkBox_NAT_enablePS3mode.Enabled = NAT.NAT_enabled;
        }

        private void button_nat_add_iprange_Click(object sender, EventArgs e)
        {
            IPAddress ip_start;
            IPAddress ip_end;
            IPAddress ip_netmask;
            if (!IPAddress.TryParse(textBox_nat_iprange_from.Text, out ip_start))
            {
                MessageBox.Show("Error! Malformed IP address in IP range FROM field.", "XBSlink error", MessageBoxButtons.OK ,MessageBoxIcon.Error);
                return;
            }
            if (!IPAddress.TryParse(textBox_nat_iprange_to.Text, out ip_end))
            {
                MessageBox.Show("Error! Malformed IP address in IP range TO field.", "XBSlink error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!IPAddress.TryParse(comboBox_nat_netmask.Text, out ip_netmask))
            {
                MessageBox.Show("Error! Malformed address in IP netmask field.", "XBSlink error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            byte[] data;
            data = ip_start.GetAddressBytes();
            UInt32 range_start = EndianBitConverter.Big.ToUInt32(data, 0);
            data = ip_end.GetAddressBytes();
            UInt32 range_stop = EndianBitConverter.Big.ToUInt32(data, 0);
            if (range_start>range_stop)
            {
                MessageBox.Show("Error! IP address range start is higher than IP address range end. ", "XBSlink error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int count = NAT.ip_pool.addIPRangeToPool(ip_start, ip_end, ip_netmask);
            if (count<=0)
                MessageBox.Show("Error! could not add IPs to NAT IP pool.", "XBSlink error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                toolTip2.Show("Added "+count+" IPs to the pool.", button_nat_add_iprange, 0, -20, 2000);
            updateNATIPPoolListView();
        }

        private void updateNATIPPoolListView()
        {
            if (tabControl1.SelectedTab != tabPage_nat)
                return;
            listView_nat_IPpool.BeginUpdate();
            listView_nat_IPpool.Items.Clear();
            xbs_nat_entry[] entries = NAT.ip_pool.getEntriesArray();
            foreach (xbs_nat_entry entry in entries)
            {
                ListViewItem lv_item = listView_nat_IPpool.Items.Add(entry.natted_source_ip.ToString() + "/" + entry.natted_source_ip_netmask.ToString());
                if (entry.original_source_ip!=null)
                {
                    lv_item.SubItems.Add(entry.source_mac.ToString());
                    lv_item.SubItems.Add(entry.original_source_ip.ToString());
                    xbs_node node = node_list.findNode(entry.source_mac);
                    if (node != null)
                        lv_item.SubItems.Add( node.nickname + " | "+node.ip_public+":"+node.port_public);
                }
            }
            listView_nat_IPpool.EndUpdate();
        }

        private void button_nat_ippool_del_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection items = listView_nat_IPpool.SelectedItems;
            if (items == null || items.Count <= 0)
                return;
            IPAddress ip;
            foreach (ListViewItem lv_item in items)
            {
                if (IPAddress.TryParse(lv_item.Text.Split('/')[0], out ip))
                    NAT.ip_pool.removeIPFromPool(ip);
#if DEBUG
                else
                    xbs_messages.addDebugMessage("!! could not delete NAT IP from pool. Error 0. " + lv_item.Text, xbs_message_sender.GENERAL, xbs_message_type.ERROR);
#endif
            }
            updateNATIPPoolListView();
        }

        private void button_reset_settings_Click(object sender, EventArgs e)
        {
            if (engine_started)
                return;
            if (MessageBox.Show("Do you really want to discard your personal settings and reset to default values?", "XBSlink", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                xbs_settings.settings.Reset();
                initWithRegistryValues();
                xbs_messages.addInfoMessage("settings have been reset to default values.", xbs_message_sender.GENERAL);
            }
        }

        private void checkBox_filter_wellknown_ports_CheckedChanged(object sender, EventArgs e)
        {
            if (sniffer != null)
            {
                sniffer.pdev_filter_wellknown_ports = checkBox_filter_wellknown_ports.Checked;
                if (engine_started)
                    sniffer.setPdevFilter();
            }
        }

        private void checkBox_nat_useDHCP_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox_NAT_enablePS3mode_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_NAT_enablePS3mode.Checked)
                checkBox_NAT_enablePS3mode.Checked = false;
            //NAT.NAT_enablePS3mode = checkBox_NAT_enablePS3mode.Checked;
        }

        private void checkBox_excludeGatewayIPs_CheckedChanged(object sender, EventArgs e)
        {
            if (sniffer != null)
            {
                sniffer.pdev_filter_exclude_gatway_ips = checkBox_excludeGatewayIPs.Checked;
                sniffer.setPdevFilter();
            }
        }

        private void checkBox_chat_nodeInfoMessagesInChat_CheckedChanged(object sender, EventArgs e)
        {
            xbs_chat.message_when_nodes_join_or_leave = checkBox_chat_nodeInfoMessagesInChat.Checked;
        }

        private void checkBox_forward_all_high_port_broadcast_CheckedChanged(object sender, EventArgs e)
        {
            if (sniffer != null)
            {
                sniffer.pdev_filter_forward_high_port_broadcasts = checkBox_forward_all_high_port_broadcast.Checked;
                sniffer.setPdevFilter();
            }

        }

        private void listView_clouds_DoubleClick(object sender, EventArgs e)
        {
            if (engine_started && cloudlist.part_of_cloud==false)
                if (listView_clouds.SelectedItems.Count>0)
                    join_cloud();
        }

        private void checkBox_minimize2systray_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox_preventSystemStandby_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_preventSystemStandby.Checked)
                xbs_system_functions.PreventSystemFromSleeping();
            else
                xbs_system_functions.restoreSystemSleepState();
        }

        private void loadNewsFeed( String url )
        {
            try
            {

                richTextBox_newsFeed.Clear();

                string result = null;
                WebClient client = new WebClient();
                client.Proxy = null;
                try
                {
                    result = client.DownloadString(url);
                }
                catch (WebException wex)
                {
                    richTextBox_newsFeed.AppendText("Error loading news feed: " + wex.Message);
                    return;
                }
                if (result == null || result.Length == 0)
                {
                    richTextBox_newsFeed.AppendText("Error loading news feed. no information returned.");
                    return;
                }

                String first_id = feedXDocument(result);

                richTextBox_newsFeed.SelectionStart = 0;
                richTextBox_newsFeed.ScrollToCaret();

                if ((first_id != xbs_settings.settings.REG_NEWS_FEED_NEWEST_ID) && checkBox_switchToNewsTab.Checked)
                {
                    tabControl1.SelectedTab = tabPage_newsFeed;
                    xbs_settings.settings.REG_NEWS_FEED_NEWEST_ID = first_id;
                    xbs_settings.settings.Save();
                }
            }
            catch (Exception ex)
            {
                xbs_messages.addInfoMessage("!!ERROR!! could not initialize news feed: "+ex.Message, xbs_message_sender.GENERAL, xbs_message_type.ERROR);
            }
        }

        private void addFeedEntry( String title, String summary, DateTimeOffset publish_date, String link_uri)
        {
            FontFamily myFontFamily = new FontFamily("Arial");
            Font font_head = new Font(myFontFamily, 14, FontStyle.Bold, GraphicsUnit.Pixel);
            Font font_summary = new Font(myFontFamily, 10, FontStyle.Regular, GraphicsUnit.Pixel);

            String date = publish_date.Month + "-" + publish_date.Day;

            richTextBox_newsFeed.SelectionFont = font_head;
            richTextBox_newsFeed.SelectionColor = Color.DarkRed;
            richTextBox_newsFeed.AppendText(date + " " + title + Environment.NewLine);

            richTextBox_newsFeed.SelectionFont = font_summary;
            richTextBox_newsFeed.SelectionColor = Color.Black;
            richTextBox_newsFeed.AppendText(summary + Environment.NewLine);

            if (link_uri != null && link_uri.Length>0)
                richTextBox_newsFeed.AppendText(link_uri + Environment.NewLine);
            richTextBox_newsFeed.AppendText(Environment.NewLine);
        }
		
        private String feedXDocument(string feed_str)
        {
            XDocument document = XDocument.Parse(feed_str);
            String first_id = null;
            String title, summary, link;
            DateTimeOffset publish_date;
            foreach (XElement unit in document.Root.Element("channel").Elements("item"))
            {
                if (first_id == null)
                    first_id = unit.Element("guid").Value;
                publish_date = DateTimeOffset.Parse( unit.Element("pubDate").Value );
                title = unit.Element("title").Value;
                summary = unit.Element("description").Value;
                link = unit.Element("link").Value;
                addFeedEntry(title, summary, publish_date, link);
            }
            return first_id;
        }

        private void richTextBox_newsFeed_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void richTextBox_about_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void checkBox_showNewsFeed_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_showNewsFeed.Checked)
            {
                tabControl1.TabPages.Insert(0, tabPage_newsFeed);
                loadNewsFeed( textBox_newsFeedUri.Text );
            }
            else
            {
                tabControl1.TabPages.Remove(tabPage_newsFeed);
            }
        }

        private void textBox_newsFeedUri_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox_newsFeedUri_Leave(object sender, EventArgs e)
        {
            if (textBox_newsFeedUri.Text.Length == 0)
                textBox_newsFeedUri.Text = Settings.Default.REG_NEWS_FEED_URI;
        }

        private void listView_nodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection indexes = listView_nodes.SelectedIndices;
            if (indexes.Count!=1)
                return;
            ListViewItem lv_item = listView_nodes.Items[indexes[0]];
            IPAddress ip = IPAddress.Parse(lv_item.Text);
            int port = int.Parse(lv_item.SubItems[1].Text.Split('/')[0]);
            xbs_node node = node_list.findNode(ip, port);
            if (node == null)
                return;
            StringBuilder str = new StringBuilder();
            str.Append("Name: " + node.nickname + Environment.NewLine);
            str.Append("IP: " + node.ip_public + Environment.NewLine);
            xbs_xbox[] devices = node.getXboxArray();
            if (devices.Length > 0 )
            {
                str.Append("Devices: "+ Environment.NewLine);
                foreach (xbs_xbox device in devices)
                {
                    str.Append(" * " + device.mac + Environment.NewLine);
                    if (device.ip_addresses.Count > 0)
                    {
                        foreach (IPAddress device_ip in device.ip_addresses)
                            str.Append("   # " + device_ip.ToString() + Environment.NewLine);
                    }
                }
            }
            textBox_nodeinfo.Text = str.ToString();
        }
    }
}
