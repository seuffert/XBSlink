/**
 * Project: XBSlink: A XBox360 & PS3/2 System Link Proxy
 * File name: Form1.Designer.cs
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

namespace XBSlink
{
    partial class FormMain
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_sniffer_in = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_udp_in = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_udp_out = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_icon = new System.Windows.Forms.ToolStripStatusLabel();
            this.comboBox_captureDevice = new System.Windows.Forms.ComboBox();
            this.button_start_engine = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_local_Port = new System.Windows.Forms.TextBox();
            this.textBox_remote_port = new System.Windows.Forms.TextBox();
            this.button_announce = new System.Windows.Forms.Button();
            this.comboBox_localIP = new System.Windows.Forms.ComboBox();
            this.checkbox_UPnP = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_newsFeed = new System.Windows.Forms.TabPage();
            this.richTextBox_newsFeed = new System.Windows.Forms.RichTextBox();
            this.tabPage_info = new System.Windows.Forms.TabPage();
            this.label15 = new System.Windows.Forms.Label();
            this.listView_nodes = new System.Windows.Forms.ListView();
            this.columnHeader_nodeIP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_nodePort = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_nodePing = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_nodeVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_nodeNickname = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tabPage_clouds = new System.Windows.Forms.TabPage();
            this.button_CloudLeave = new System.Windows.Forms.Button();
            this.button_CloudJoin = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.textBox_CloudPassword = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textBox_CloudMaxNodes = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox_CloudName = new System.Windows.Forms.TextBox();
            this.buttonLoadCloudlist = new System.Windows.Forms.Button();
            this.textBox_cloudlist = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.listView_clouds = new System.Windows.Forms.ListView();
            this.columnHeader_cloudlistname = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_cloudlistnodecount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_cloudlistmaxnodes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage_chat = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.textBox_chatMessages = new System.Windows.Forms.TextBox();
            this.listBox_chatUserList = new System.Windows.Forms.ListBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label_num_persons_in_chat = new System.Windows.Forms.Label();
            this.button_clearChat = new System.Windows.Forms.Button();
            this.textBox_chatEntry = new System.Windows.Forms.TextBox();
            this.tabPage_messages = new System.Windows.Forms.TabPage();
            this.button_messages_copy = new System.Windows.Forms.Button();
            this.listBox_messages = new System.Windows.Forms.ListBox();
            this.button_clearMessages = new System.Windows.Forms.Button();
            this.tabPage_settings = new System.Windows.Forms.TabPage();
            this.tabControl_settings = new System.Windows.Forms.TabControl();
            this.tabPageSettings_general = new System.Windows.Forms.TabPage();
            this.groupBox_newsFeed = new System.Windows.Forms.GroupBox();
            this.checkBox_switchToNewsTab = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.checkBox_showNewsFeed = new System.Windows.Forms.CheckBox();
            this.textBox_newsFeedUri = new System.Windows.Forms.TextBox();
            this.groupBox_usersettings = new System.Windows.Forms.GroupBox();
            this.label_nickname = new System.Windows.Forms.Label();
            this.textBox_chatNickname = new System.Windows.Forms.TextBox();
            this.groupBox_updates = new System.Windows.Forms.GroupBox();
            this.checkBox_preventSystemStandby = new System.Windows.Forms.CheckBox();
            this.checkBox_minimize2systray = new System.Windows.Forms.CheckBox();
            this.checkBox_checkForUpdates = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.checkBox_chat_notify = new System.Windows.Forms.CheckBox();
            this.checkBox_newNodeSound = new System.Windows.Forms.CheckBox();
            this.groupBox_chatsettings = new System.Windows.Forms.GroupBox();
            this.checkBox_chat_nodeInfoMessagesInChat = new System.Windows.Forms.CheckBox();
            this.checkBox_chatAutoSwitch = new System.Windows.Forms.CheckBox();
            this.tabPageSettings_network = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox_portoptions = new System.Windows.Forms.GroupBox();
            this.checkBox_useCloudServerForPortCheck = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox_packetfilter = new System.Windows.Forms.GroupBox();
            this.checkBox_forward_all_high_port_broadcast = new System.Windows.Forms.CheckBox();
            this.checkBox_filter_wellknown_ports = new System.Windows.Forms.CheckBox();
            this.checkBox_excludeGatewayIPs = new System.Windows.Forms.CheckBox();
            this.groupBox_MAClist = new System.Windows.Forms.GroupBox();
            this.checkBox_mac_restriction = new System.Windows.Forms.CheckBox();
            this.checkBox_enable_MAC_list = new System.Windows.Forms.CheckBox();
            this.button_del_MAC = new System.Windows.Forms.Button();
            this.button_add_MAC = new System.Windows.Forms.Button();
            this.textBox_add_MAC = new System.Windows.Forms.TextBox();
            this.listBox_MAC_list = new System.Windows.Forms.ListBox();
            this.button_reset_settings = new System.Windows.Forms.Button();
            this.button_save_settings = new System.Windows.Forms.Button();
            this.tabPage_nat = new System.Windows.Forms.TabPage();
            this.label18 = new System.Windows.Forms.Label();
            this.comboBox_nat_netmask = new System.Windows.Forms.ComboBox();
            this.button_nat_ippool_del = new System.Windows.Forms.Button();
            this.button_nat_add_iprange = new System.Windows.Forms.Button();
            this.textBox_nat_iprange_to = new System.Windows.Forms.TextBox();
            this.textBox_nat_iprange_from = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.listView_nat_IPpool = new System.Windows.Forms.ListView();
            this.columnHeader_nat_ippool_localIP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_nat_ippool_device = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_nat_ippool__originalIP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_nat_ippool_node = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBox_NAT_enablePS3mode = new System.Windows.Forms.CheckBox();
            this.checkBox_nat_useDHCP = new System.Windows.Forms.CheckBox();
            this.checkBox_nat_enable = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.tabPage_about = new System.Windows.Forms.TabPage();
            this.richTextBox_about = new System.Windows.Forms.RichTextBox();
            this.timer_messages = new System.Windows.Forms.Timer(this.components);
            this.comboBox_RemoteHost = new System.Windows.Forms.ComboBox();
            this.timer_startEngine = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip_systray = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem_exit = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage_newsFeed.SuspendLayout();
            this.tabPage_info.SuspendLayout();
            this.tabPage_clouds.SuspendLayout();
            this.tabPage_chat.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabPage_messages.SuspendLayout();
            this.tabPage_settings.SuspendLayout();
            this.tabControl_settings.SuspendLayout();
            this.tabPageSettings_general.SuspendLayout();
            this.groupBox_newsFeed.SuspendLayout();
            this.groupBox_usersettings.SuspendLayout();
            this.groupBox_updates.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox_chatsettings.SuspendLayout();
            this.tabPageSettings_network.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox_portoptions.SuspendLayout();
            this.groupBox_packetfilter.SuspendLayout();
            this.groupBox_MAClist.SuspendLayout();
            this.tabPage_nat.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabPage_about.SuspendLayout();
            this.contextMenuStrip_systray.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel_sniffer_in,
            this.toolStripStatusLabel4,
            this.toolStripStatusLabel_udp_in,
            this.toolStripStatusLabel_udp_out,
            this.toolStripStatusLabel_icon});
            this.statusStrip.Location = new System.Drawing.Point(0, 588);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(386, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.statusStrip1_MouseDoubleClick);
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(99, 17);
            this.toolStripStatusLabel3.Text = "Packets/s sniffed:";
            // 
            // toolStripStatusLabel_sniffer_in
            // 
            this.toolStripStatusLabel_sniffer_in.AutoSize = false;
            this.toolStripStatusLabel_sniffer_in.MergeIndex = 2;
            this.toolStripStatusLabel_sniffer_in.Name = "toolStripStatusLabel_sniffer_in";
            this.toolStripStatusLabel_sniffer_in.Size = new System.Drawing.Size(100, 17);
            this.toolStripStatusLabel_sniffer_in.Text = "0";
            this.toolStripStatusLabel_sniffer_in.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(72, 17);
            this.toolStripStatusLabel4.Text = "UDP in/out :";
            // 
            // toolStripStatusLabel_udp_in
            // 
            this.toolStripStatusLabel_udp_in.AutoSize = false;
            this.toolStripStatusLabel_udp_in.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabel_udp_in.Name = "toolStripStatusLabel_udp_in";
            this.toolStripStatusLabel_udp_in.Size = new System.Drawing.Size(23, 17);
            this.toolStripStatusLabel_udp_in.Text = "0";
            this.toolStripStatusLabel_udp_in.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolStripStatusLabel_udp_out
            // 
            this.toolStripStatusLabel_udp_out.AutoSize = false;
            this.toolStripStatusLabel_udp_out.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.toolStripStatusLabel_udp_out.Name = "toolStripStatusLabel_udp_out";
            this.toolStripStatusLabel_udp_out.Size = new System.Drawing.Size(23, 17);
            this.toolStripStatusLabel_udp_out.Text = "0";
            this.toolStripStatusLabel_udp_out.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel_icon
            // 
            this.toolStripStatusLabel_icon.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripStatusLabel_icon.Image = global::XBSlink.Properties.Resources.ok_16;
            this.toolStripStatusLabel_icon.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolStripStatusLabel_icon.Name = "toolStripStatusLabel_icon";
            this.toolStripStatusLabel_icon.Size = new System.Drawing.Size(54, 17);
            this.toolStripStatusLabel_icon.Spring = true;
            this.toolStripStatusLabel_icon.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBox_captureDevice
            // 
            this.comboBox_captureDevice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_captureDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_captureDevice.FormattingEnabled = true;
            this.comboBox_captureDevice.Location = new System.Drawing.Point(6, 32);
            this.comboBox_captureDevice.Name = "comboBox_captureDevice";
            this.comboBox_captureDevice.Size = new System.Drawing.Size(314, 21);
            this.comboBox_captureDevice.TabIndex = 3;
            this.comboBox_captureDevice.SelectedIndexChanged += new System.EventHandler(this.comboBox_captureDevice_SelectedIndexChanged);
            // 
            // button_start_engine
            // 
            this.button_start_engine.Enabled = false;
            this.button_start_engine.Location = new System.Drawing.Point(10, 12);
            this.button_start_engine.Name = "button_start_engine";
            this.button_start_engine.Size = new System.Drawing.Size(87, 23);
            this.button_start_engine.TabIndex = 4;
            this.button_start_engine.Text = "Start Engine";
            this.button_start_engine.UseVisualStyleBackColor = true;
            this.button_start_engine.Click += new System.EventHandler(this.button_start_engine_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Remote Host:";
            // 
            // textBox_local_Port
            // 
            this.textBox_local_Port.Location = new System.Drawing.Point(206, 32);
            this.textBox_local_Port.MaxLength = 5;
            this.textBox_local_Port.Name = "textBox_local_Port";
            this.textBox_local_Port.Size = new System.Drawing.Size(41, 20);
            this.textBox_local_Port.TabIndex = 7;
            this.textBox_local_Port.Text = "31415";
            this.textBox_local_Port.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_local_Port_KeyPress);
            this.textBox_local_Port.Leave += new System.EventHandler(this.textBox_local_Port_Leave);
            // 
            // textBox_remote_port
            // 
            this.textBox_remote_port.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_remote_port.Location = new System.Drawing.Point(337, 39);
            this.textBox_remote_port.MaxLength = 5;
            this.textBox_remote_port.Name = "textBox_remote_port";
            this.textBox_remote_port.Size = new System.Drawing.Size(36, 20);
            this.textBox_remote_port.TabIndex = 8;
            this.textBox_remote_port.Text = "31415";
            this.textBox_remote_port.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_remote_port_KeyPress);
            this.textBox_remote_port.Leave += new System.EventHandler(this.textBox_remote_port_Leave);
            // 
            // button_announce
            // 
            this.button_announce.Enabled = false;
            this.button_announce.Location = new System.Drawing.Point(103, 12);
            this.button_announce.Name = "button_announce";
            this.button_announce.Size = new System.Drawing.Size(162, 23);
            this.button_announce.TabIndex = 9;
            this.button_announce.Text = "directly connect to remote host";
            this.button_announce.UseVisualStyleBackColor = true;
            this.button_announce.Click += new System.EventHandler(this.button_announce_Click);
            // 
            // comboBox_localIP
            // 
            this.comboBox_localIP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_localIP.FormattingEnabled = true;
            this.comboBox_localIP.Location = new System.Drawing.Point(6, 32);
            this.comboBox_localIP.Name = "comboBox_localIP";
            this.comboBox_localIP.Size = new System.Drawing.Size(184, 21);
            this.comboBox_localIP.TabIndex = 10;
            this.comboBox_localIP.SelectedIndexChanged += new System.EventHandler(this.comboBox_external_SelectedIndexChanged);
            // 
            // checkbox_UPnP
            // 
            this.checkbox_UPnP.AutoSize = true;
            this.checkbox_UPnP.Checked = true;
            this.checkbox_UPnP.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkbox_UPnP.Location = new System.Drawing.Point(6, 62);
            this.checkbox_UPnP.Name = "checkbox_UPnP";
            this.checkbox_UPnP.Size = new System.Drawing.Size(227, 17);
            this.checkbox_UPnP.TabIndex = 11;
            this.checkbox_UPnP.Text = "use UPnP NAT (automatic port forwarding)";
            this.checkbox_UPnP.UseVisualStyleBackColor = true;
            this.checkbox_UPnP.CheckedChanged += new System.EventHandler(this.checkbox_UPnP_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(305, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Capture Device (connected to same network as game console)";
            this.toolTip1.SetToolTip(this.label2, "select the network device thats connected to the same network as your Xbox360. Wi" +
        "reless adpaters will most likely NOT work!");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Bind to IP";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(194, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(10, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = ":";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(317, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(10, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = ":";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage_newsFeed);
            this.tabControl1.Controls.Add(this.tabPage_info);
            this.tabControl1.Controls.Add(this.tabPage_clouds);
            this.tabControl1.Controls.Add(this.tabPage_chat);
            this.tabControl1.Controls.Add(this.tabPage_messages);
            this.tabControl1.Controls.Add(this.tabPage_settings);
            this.tabControl1.Controls.Add(this.tabPage_nat);
            this.tabControl1.Controls.Add(this.tabPage_about);
            this.tabControl1.Location = new System.Drawing.Point(10, 65);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(368, 519);
            this.tabControl1.TabIndex = 17;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage_newsFeed
            // 
            this.tabPage_newsFeed.Controls.Add(this.richTextBox_newsFeed);
            this.tabPage_newsFeed.Location = new System.Drawing.Point(4, 22);
            this.tabPage_newsFeed.Name = "tabPage_newsFeed";
            this.tabPage_newsFeed.Size = new System.Drawing.Size(360, 493);
            this.tabPage_newsFeed.TabIndex = 7;
            this.tabPage_newsFeed.Text = "News";
            this.tabPage_newsFeed.UseVisualStyleBackColor = true;
            // 
            // richTextBox_newsFeed
            // 
            this.richTextBox_newsFeed.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox_newsFeed.Location = new System.Drawing.Point(0, 0);
            this.richTextBox_newsFeed.Name = "richTextBox_newsFeed";
            this.richTextBox_newsFeed.Size = new System.Drawing.Size(360, 490);
            this.richTextBox_newsFeed.TabIndex = 0;
            this.richTextBox_newsFeed.Text = "";
            this.richTextBox_newsFeed.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBox_newsFeed_LinkClicked);
            // 
            // tabPage_info
            // 
            this.tabPage_info.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_info.Controls.Add(this.label15);
            this.tabPage_info.Controls.Add(this.listView_nodes);
            this.tabPage_info.Controls.Add(this.textBox1);
            this.tabPage_info.Location = new System.Drawing.Point(4, 22);
            this.tabPage_info.Name = "tabPage_info";
            this.tabPage_info.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_info.Size = new System.Drawing.Size(360, 493);
            this.tabPage_info.TabIndex = 0;
            this.tabPage_info.Text = "Info";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(7, 4);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(151, 13);
            this.label15.TabIndex = 3;
            this.label15.Text = "List of known XBSlink nodes : ";
            // 
            // listView_nodes
            // 
            this.listView_nodes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView_nodes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_nodeIP,
            this.columnHeader_nodePort,
            this.columnHeader_nodePing,
            this.columnHeader_nodeVersion,
            this.columnHeader_nodeNickname});
            this.listView_nodes.GridLines = true;
            this.listView_nodes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView_nodes.LabelWrap = false;
            this.listView_nodes.Location = new System.Drawing.Point(0, 22);
            this.listView_nodes.MultiSelect = false;
            this.listView_nodes.Name = "listView_nodes";
            this.listView_nodes.ShowGroups = false;
            this.listView_nodes.Size = new System.Drawing.Size(348, 374);
            this.listView_nodes.TabIndex = 2;
            this.listView_nodes.UseCompatibleStateImageBehavior = false;
            this.listView_nodes.View = System.Windows.Forms.View.Details;
            this.listView_nodes.Resize += new System.EventHandler(this.listView_nodes_Resize);
            // 
            // columnHeader_nodeIP
            // 
            this.columnHeader_nodeIP.Text = "Address";
            this.columnHeader_nodeIP.Width = 86;
            // 
            // columnHeader_nodePort
            // 
            this.columnHeader_nodePort.Text = "Port";
            this.columnHeader_nodePort.Width = 51;
            // 
            // columnHeader_nodePing
            // 
            this.columnHeader_nodePing.Text = "Ping";
            this.columnHeader_nodePing.Width = 44;
            // 
            // columnHeader_nodeVersion
            // 
            this.columnHeader_nodeVersion.Text = "Version";
            this.columnHeader_nodeVersion.Width = 54;
            // 
            // columnHeader_nodeNickname
            // 
            this.columnHeader_nodeNickname.Text = "Nickname";
            this.columnHeader_nodeNickname.Width = 109;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(0, 397);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(348, 93);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "Engine not started.";
            // 
            // tabPage_clouds
            // 
            this.tabPage_clouds.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_clouds.Controls.Add(this.button_CloudLeave);
            this.tabPage_clouds.Controls.Add(this.button_CloudJoin);
            this.tabPage_clouds.Controls.Add(this.label14);
            this.tabPage_clouds.Controls.Add(this.textBox_CloudPassword);
            this.tabPage_clouds.Controls.Add(this.label13);
            this.tabPage_clouds.Controls.Add(this.textBox_CloudMaxNodes);
            this.tabPage_clouds.Controls.Add(this.label12);
            this.tabPage_clouds.Controls.Add(this.textBox_CloudName);
            this.tabPage_clouds.Controls.Add(this.buttonLoadCloudlist);
            this.tabPage_clouds.Controls.Add(this.textBox_cloudlist);
            this.tabPage_clouds.Controls.Add(this.label11);
            this.tabPage_clouds.Controls.Add(this.listView_clouds);
            this.tabPage_clouds.Location = new System.Drawing.Point(4, 22);
            this.tabPage_clouds.Margin = new System.Windows.Forms.Padding(1);
            this.tabPage_clouds.Name = "tabPage_clouds";
            this.tabPage_clouds.Size = new System.Drawing.Size(360, 493);
            this.tabPage_clouds.TabIndex = 5;
            this.tabPage_clouds.Text = "Clouds";
            // 
            // button_CloudLeave
            // 
            this.button_CloudLeave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_CloudLeave.Enabled = false;
            this.button_CloudLeave.Location = new System.Drawing.Point(270, 449);
            this.button_CloudLeave.Name = "button_CloudLeave";
            this.button_CloudLeave.Size = new System.Drawing.Size(75, 21);
            this.button_CloudLeave.TabIndex = 11;
            this.button_CloudLeave.Text = "leave";
            this.button_CloudLeave.UseVisualStyleBackColor = true;
            this.button_CloudLeave.Click += new System.EventHandler(this.button_CloudLeave_Click);
            // 
            // button_CloudJoin
            // 
            this.button_CloudJoin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_CloudJoin.Enabled = false;
            this.button_CloudJoin.Location = new System.Drawing.Point(270, 470);
            this.button_CloudJoin.Name = "button_CloudJoin";
            this.button_CloudJoin.Size = new System.Drawing.Size(75, 21);
            this.button_CloudJoin.TabIndex = 10;
            this.button_CloudJoin.Text = "join/create";
            this.button_CloudJoin.UseVisualStyleBackColor = true;
            this.button_CloudJoin.Click += new System.EventHandler(this.button_CloudJoin_Click);
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(185, 451);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 13);
            this.label14.TabIndex = 9;
            this.label14.Text = "Password";
            // 
            // textBox_CloudPassword
            // 
            this.textBox_CloudPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_CloudPassword.Enabled = false;
            this.textBox_CloudPassword.Location = new System.Drawing.Point(188, 467);
            this.textBox_CloudPassword.Name = "textBox_CloudPassword";
            this.textBox_CloudPassword.PasswordChar = '*';
            this.textBox_CloudPassword.Size = new System.Drawing.Size(76, 20);
            this.textBox_CloudPassword.TabIndex = 8;
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(146, 451);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(27, 13);
            this.label13.TabIndex = 7;
            this.label13.Text = "Max";
            // 
            // textBox_CloudMaxNodes
            // 
            this.textBox_CloudMaxNodes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_CloudMaxNodes.Enabled = false;
            this.textBox_CloudMaxNodes.Location = new System.Drawing.Point(149, 467);
            this.textBox_CloudMaxNodes.Name = "textBox_CloudMaxNodes";
            this.textBox_CloudMaxNodes.Size = new System.Drawing.Size(33, 20);
            this.textBox_CloudMaxNodes.TabIndex = 6;
            this.textBox_CloudMaxNodes.Text = "10";
            this.textBox_CloudMaxNodes.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_CloudMaxNodes_KeyPress);
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 451);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(63, 13);
            this.label12.TabIndex = 5;
            this.label12.Text = "Cloud name";
            // 
            // textBox_CloudName
            // 
            this.textBox_CloudName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_CloudName.Enabled = false;
            this.textBox_CloudName.Location = new System.Drawing.Point(6, 467);
            this.textBox_CloudName.Name = "textBox_CloudName";
            this.textBox_CloudName.Size = new System.Drawing.Size(137, 20);
            this.textBox_CloudName.TabIndex = 4;
            // 
            // buttonLoadCloudlist
            // 
            this.buttonLoadCloudlist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLoadCloudlist.Location = new System.Drawing.Point(270, 0);
            this.buttonLoadCloudlist.Name = "buttonLoadCloudlist";
            this.buttonLoadCloudlist.Size = new System.Drawing.Size(75, 23);
            this.buttonLoadCloudlist.TabIndex = 3;
            this.buttonLoadCloudlist.Text = "load";
            this.buttonLoadCloudlist.UseVisualStyleBackColor = true;
            this.buttonLoadCloudlist.Click += new System.EventHandler(this.buttonLoadCloudlist_Click);
            // 
            // textBox_cloudlist
            // 
            this.textBox_cloudlist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_cloudlist.Location = new System.Drawing.Point(61, 2);
            this.textBox_cloudlist.Name = "textBox_cloudlist";
            this.textBox_cloudlist.Size = new System.Drawing.Size(203, 20);
            this.textBox_cloudlist.TabIndex = 2;
            this.textBox_cloudlist.Leave += new System.EventHandler(this.textBox_cloudlist_Leave);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 5);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(52, 13);
            this.label11.TabIndex = 1;
            this.label11.Text = "Cloud list:";
            // 
            // listView_clouds
            // 
            this.listView_clouds.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView_clouds.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView_clouds.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_cloudlistname,
            this.columnHeader_cloudlistnodecount,
            this.columnHeader_cloudlistmaxnodes});
            this.listView_clouds.FullRowSelect = true;
            this.listView_clouds.GridLines = true;
            this.listView_clouds.Location = new System.Drawing.Point(0, 28);
            this.listView_clouds.Name = "listView_clouds";
            this.listView_clouds.Size = new System.Drawing.Size(348, 415);
            this.listView_clouds.TabIndex = 0;
            this.listView_clouds.UseCompatibleStateImageBehavior = false;
            this.listView_clouds.View = System.Windows.Forms.View.Details;
            this.listView_clouds.SelectedIndexChanged += new System.EventHandler(this.listView_clouds_SelectedIndexChanged);
            this.listView_clouds.DoubleClick += new System.EventHandler(this.listView_clouds_DoubleClick);
            this.listView_clouds.Resize += new System.EventHandler(this.listView_clouds_Resize);
            // 
            // columnHeader_cloudlistname
            // 
            this.columnHeader_cloudlistname.Text = "Name";
            this.columnHeader_cloudlistname.Width = 204;
            // 
            // columnHeader_cloudlistnodecount
            // 
            this.columnHeader_cloudlistnodecount.Text = "Nodes";
            // 
            // columnHeader_cloudlistmaxnodes
            // 
            this.columnHeader_cloudlistmaxnodes.Text = "Max";
            // 
            // tabPage_chat
            // 
            this.tabPage_chat.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_chat.Controls.Add(this.splitContainer1);
            this.tabPage_chat.Controls.Add(this.button_clearChat);
            this.tabPage_chat.Controls.Add(this.textBox_chatEntry);
            this.tabPage_chat.Location = new System.Drawing.Point(4, 22);
            this.tabPage_chat.Name = "tabPage_chat";
            this.tabPage_chat.Size = new System.Drawing.Size(360, 493);
            this.tabPage_chat.TabIndex = 4;
            this.tabPage_chat.Text = "Chat";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.textBox_chatMessages);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listBox_chatUserList);
            this.splitContainer1.Panel2.Controls.Add(this.label16);
            this.splitContainer1.Panel2.Controls.Add(this.label_num_persons_in_chat);
            this.splitContainer1.Size = new System.Drawing.Size(348, 465);
            this.splitContainer1.SplitterDistance = 250;
            this.splitContainer1.SplitterWidth = 2;
            this.splitContainer1.TabIndex = 7;
            // 
            // textBox_chatMessages
            // 
            this.textBox_chatMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_chatMessages.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textBox_chatMessages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_chatMessages.Location = new System.Drawing.Point(3, 0);
            this.textBox_chatMessages.Multiline = true;
            this.textBox_chatMessages.Name = "textBox_chatMessages";
            this.textBox_chatMessages.ReadOnly = true;
            this.textBox_chatMessages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_chatMessages.Size = new System.Drawing.Size(244, 462);
            this.textBox_chatMessages.TabIndex = 3;
            // 
            // listBox_chatUserList
            // 
            this.listBox_chatUserList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_chatUserList.FormattingEnabled = true;
            this.listBox_chatUserList.IntegralHeight = false;
            this.listBox_chatUserList.Location = new System.Drawing.Point(3, 17);
            this.listBox_chatUserList.Name = "listBox_chatUserList";
            this.listBox_chatUserList.Size = new System.Drawing.Size(93, 445);
            this.listBox_chatUserList.TabIndex = 6;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(-1, 2);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(47, 13);
            this.label16.TabIndex = 5;
            this.label16.Text = "Online:";
            // 
            // label_num_persons_in_chat
            // 
            this.label_num_persons_in_chat.AutoSize = true;
            this.label_num_persons_in_chat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_num_persons_in_chat.Location = new System.Drawing.Point(49, 2);
            this.label_num_persons_in_chat.Name = "label_num_persons_in_chat";
            this.label_num_persons_in_chat.Size = new System.Drawing.Size(14, 13);
            this.label_num_persons_in_chat.TabIndex = 4;
            this.label_num_persons_in_chat.Text = "0";
            // 
            // button_clearChat
            // 
            this.button_clearChat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_clearChat.Location = new System.Drawing.Point(309, 469);
            this.button_clearChat.Name = "button_clearChat";
            this.button_clearChat.Size = new System.Drawing.Size(39, 21);
            this.button_clearChat.TabIndex = 2;
            this.button_clearChat.Text = "clear";
            this.button_clearChat.UseVisualStyleBackColor = true;
            this.button_clearChat.Click += new System.EventHandler(this.button_clearChat_Click);
            // 
            // textBox_chatEntry
            // 
            this.textBox_chatEntry.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_chatEntry.Location = new System.Drawing.Point(0, 470);
            this.textBox_chatEntry.MaxLength = 1400;
            this.textBox_chatEntry.Name = "textBox_chatEntry";
            this.textBox_chatEntry.ReadOnly = true;
            this.textBox_chatEntry.Size = new System.Drawing.Size(305, 20);
            this.textBox_chatEntry.TabIndex = 1;
            this.textBox_chatEntry.Text = "The chat is enabled after the engine is started.";
            this.textBox_chatEntry.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_chatEntry_KeyPress);
            // 
            // tabPage_messages
            // 
            this.tabPage_messages.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_messages.Controls.Add(this.button_messages_copy);
            this.tabPage_messages.Controls.Add(this.listBox_messages);
            this.tabPage_messages.Controls.Add(this.button_clearMessages);
            this.tabPage_messages.Location = new System.Drawing.Point(4, 22);
            this.tabPage_messages.Name = "tabPage_messages";
            this.tabPage_messages.Size = new System.Drawing.Size(360, 493);
            this.tabPage_messages.TabIndex = 2;
            this.tabPage_messages.Text = "Messages";
            // 
            // button_messages_copy
            // 
            this.button_messages_copy.Location = new System.Drawing.Point(99, 7);
            this.button_messages_copy.Name = "button_messages_copy";
            this.button_messages_copy.Size = new System.Drawing.Size(100, 23);
            this.button_messages_copy.TabIndex = 2;
            this.button_messages_copy.Text = "copy to clipboard";
            this.button_messages_copy.UseVisualStyleBackColor = true;
            this.button_messages_copy.Click += new System.EventHandler(this.button_messages_copy_Click);
            // 
            // listBox_messages
            // 
            this.listBox_messages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_messages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBox_messages.FormattingEnabled = true;
            this.listBox_messages.HorizontalScrollbar = true;
            this.listBox_messages.IntegralHeight = false;
            this.listBox_messages.Location = new System.Drawing.Point(3, 36);
            this.listBox_messages.Name = "listBox_messages";
            this.listBox_messages.ScrollAlwaysVisible = true;
            this.listBox_messages.Size = new System.Drawing.Size(342, 454);
            this.listBox_messages.TabIndex = 1;
            // 
            // button_clearMessages
            // 
            this.button_clearMessages.Location = new System.Drawing.Point(3, 7);
            this.button_clearMessages.Name = "button_clearMessages";
            this.button_clearMessages.Size = new System.Drawing.Size(90, 23);
            this.button_clearMessages.TabIndex = 0;
            this.button_clearMessages.Text = "clear messages";
            this.button_clearMessages.UseVisualStyleBackColor = true;
            this.button_clearMessages.Click += new System.EventHandler(this.button_clearMessages_Click);
            // 
            // tabPage_settings
            // 
            this.tabPage_settings.AutoScroll = true;
            this.tabPage_settings.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_settings.Controls.Add(this.tabControl_settings);
            this.tabPage_settings.Controls.Add(this.button_reset_settings);
            this.tabPage_settings.Controls.Add(this.button_save_settings);
            this.tabPage_settings.Location = new System.Drawing.Point(4, 22);
            this.tabPage_settings.Name = "tabPage_settings";
            this.tabPage_settings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_settings.Size = new System.Drawing.Size(360, 493);
            this.tabPage_settings.TabIndex = 1;
            this.tabPage_settings.Text = "Settings";
            // 
            // tabControl_settings
            // 
            this.tabControl_settings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl_settings.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl_settings.Controls.Add(this.tabPageSettings_general);
            this.tabControl_settings.Controls.Add(this.tabPageSettings_network);
            this.tabControl_settings.Location = new System.Drawing.Point(0, 6);
            this.tabControl_settings.Name = "tabControl_settings";
            this.tabControl_settings.SelectedIndex = 0;
            this.tabControl_settings.Size = new System.Drawing.Size(348, 452);
            this.tabControl_settings.TabIndex = 38;
            // 
            // tabPageSettings_general
            // 
            this.tabPageSettings_general.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabPageSettings_general.Controls.Add(this.groupBox_newsFeed);
            this.tabPageSettings_general.Controls.Add(this.groupBox_usersettings);
            this.tabPageSettings_general.Controls.Add(this.groupBox_updates);
            this.tabPageSettings_general.Controls.Add(this.groupBox5);
            this.tabPageSettings_general.Controls.Add(this.groupBox_chatsettings);
            this.tabPageSettings_general.Location = new System.Drawing.Point(4, 25);
            this.tabPageSettings_general.Name = "tabPageSettings_general";
            this.tabPageSettings_general.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSettings_general.Size = new System.Drawing.Size(340, 423);
            this.tabPageSettings_general.TabIndex = 1;
            this.tabPageSettings_general.Text = "General";
            this.tabPageSettings_general.UseVisualStyleBackColor = true;
            // 
            // groupBox_newsFeed
            // 
            this.groupBox_newsFeed.Controls.Add(this.checkBox_switchToNewsTab);
            this.groupBox_newsFeed.Controls.Add(this.label7);
            this.groupBox_newsFeed.Controls.Add(this.checkBox_showNewsFeed);
            this.groupBox_newsFeed.Controls.Add(this.textBox_newsFeedUri);
            this.groupBox_newsFeed.Location = new System.Drawing.Point(6, 311);
            this.groupBox_newsFeed.Name = "groupBox_newsFeed";
            this.groupBox_newsFeed.Size = new System.Drawing.Size(326, 106);
            this.groupBox_newsFeed.TabIndex = 37;
            this.groupBox_newsFeed.TabStop = false;
            this.groupBox_newsFeed.Text = "news feed";
            // 
            // checkBox_switchToNewsTab
            // 
            this.checkBox_switchToNewsTab.AutoSize = true;
            this.checkBox_switchToNewsTab.Checked = true;
            this.checkBox_switchToNewsTab.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_switchToNewsTab.Location = new System.Drawing.Point(6, 42);
            this.checkBox_switchToNewsTab.Name = "checkBox_switchToNewsTab";
            this.checkBox_switchToNewsTab.Size = new System.Drawing.Size(236, 17);
            this.checkBox_switchToNewsTab.TabIndex = 3;
            this.checkBox_switchToNewsTab.Text = "switch to news tab if new news are available";
            this.toolTip1.SetToolTip(this.checkBox_switchToNewsTab, "set the news feed tab active if updated news are available.");
            this.checkBox_switchToNewsTab.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 62);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "news feed URI";
            // 
            // checkBox_showNewsFeed
            // 
            this.checkBox_showNewsFeed.AutoSize = true;
            this.checkBox_showNewsFeed.Checked = true;
            this.checkBox_showNewsFeed.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_showNewsFeed.Location = new System.Drawing.Point(6, 19);
            this.checkBox_showNewsFeed.Name = "checkBox_showNewsFeed";
            this.checkBox_showNewsFeed.Size = new System.Drawing.Size(121, 17);
            this.checkBox_showNewsFeed.TabIndex = 1;
            this.checkBox_showNewsFeed.Text = "show news feed tab";
            this.checkBox_showNewsFeed.UseVisualStyleBackColor = true;
            this.checkBox_showNewsFeed.CheckedChanged += new System.EventHandler(this.checkBox_showNewsFeed_CheckedChanged);
            // 
            // textBox_newsFeedUri
            // 
            this.textBox_newsFeedUri.Location = new System.Drawing.Point(6, 78);
            this.textBox_newsFeedUri.Name = "textBox_newsFeedUri";
            this.textBox_newsFeedUri.Size = new System.Drawing.Size(314, 20);
            this.textBox_newsFeedUri.TabIndex = 0;
            this.textBox_newsFeedUri.Text = "http://www.secudb.de/~seuffert/xbslink/feed";
            this.textBox_newsFeedUri.TextChanged += new System.EventHandler(this.textBox_newsFeedUri_TextChanged);
            this.textBox_newsFeedUri.Leave += new System.EventHandler(this.textBox_newsFeedUri_Leave);
            // 
            // groupBox_usersettings
            // 
            this.groupBox_usersettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_usersettings.Controls.Add(this.label_nickname);
            this.groupBox_usersettings.Controls.Add(this.textBox_chatNickname);
            this.groupBox_usersettings.Location = new System.Drawing.Point(6, 6);
            this.groupBox_usersettings.Name = "groupBox_usersettings";
            this.groupBox_usersettings.Size = new System.Drawing.Size(326, 60);
            this.groupBox_usersettings.TabIndex = 36;
            this.groupBox_usersettings.TabStop = false;
            this.groupBox_usersettings.Text = "user";
            // 
            // label_nickname
            // 
            this.label_nickname.AutoSize = true;
            this.label_nickname.Location = new System.Drawing.Point(6, 16);
            this.label_nickname.Name = "label_nickname";
            this.label_nickname.Size = new System.Drawing.Size(55, 13);
            this.label_nickname.TabIndex = 1;
            this.label_nickname.Text = "Nickname";
            // 
            // textBox_chatNickname
            // 
            this.textBox_chatNickname.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_chatNickname.Location = new System.Drawing.Point(6, 32);
            this.textBox_chatNickname.Name = "textBox_chatNickname";
            this.textBox_chatNickname.Size = new System.Drawing.Size(314, 20);
            this.textBox_chatNickname.TabIndex = 0;
            this.toolTip1.SetToolTip(this.textBox_chatNickname, "the nickname will appear in the chat as well as in the node list");
            this.textBox_chatNickname.Leave += new System.EventHandler(this.textBox_chatNickname_Leave);
            // 
            // groupBox_updates
            // 
            this.groupBox_updates.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_updates.Controls.Add(this.checkBox_preventSystemStandby);
            this.groupBox_updates.Controls.Add(this.checkBox_minimize2systray);
            this.groupBox_updates.Controls.Add(this.checkBox_checkForUpdates);
            this.groupBox_updates.Location = new System.Drawing.Point(6, 216);
            this.groupBox_updates.Name = "groupBox_updates";
            this.groupBox_updates.Size = new System.Drawing.Size(326, 89);
            this.groupBox_updates.TabIndex = 35;
            this.groupBox_updates.TabStop = false;
            this.groupBox_updates.Text = "system";
            // 
            // checkBox_preventSystemStandby
            // 
            this.checkBox_preventSystemStandby.AutoSize = true;
            this.checkBox_preventSystemStandby.Location = new System.Drawing.Point(6, 42);
            this.checkBox_preventSystemStandby.Name = "checkBox_preventSystemStandby";
            this.checkBox_preventSystemStandby.Size = new System.Drawing.Size(203, 17);
            this.checkBox_preventSystemStandby.TabIndex = 35;
            this.checkBox_preventSystemStandby.Text = "prevent system from going into standy";
            this.checkBox_preventSystemStandby.UseVisualStyleBackColor = true;
            this.checkBox_preventSystemStandby.CheckedChanged += new System.EventHandler(this.checkBox_preventSystemStandby_CheckedChanged);
            // 
            // checkBox_minimize2systray
            // 
            this.checkBox_minimize2systray.AutoSize = true;
            this.checkBox_minimize2systray.Checked = true;
            this.checkBox_minimize2systray.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_minimize2systray.Location = new System.Drawing.Point(6, 19);
            this.checkBox_minimize2systray.Name = "checkBox_minimize2systray";
            this.checkBox_minimize2systray.Size = new System.Drawing.Size(112, 17);
            this.checkBox_minimize2systray.TabIndex = 34;
            this.checkBox_minimize2systray.Text = "minimize to systray";
            this.toolTip1.SetToolTip(this.checkBox_minimize2systray, "minimize XBSlink to the system tray");
            this.checkBox_minimize2systray.UseVisualStyleBackColor = true;
            this.checkBox_minimize2systray.CheckedChanged += new System.EventHandler(this.checkBox_minimize2systray_CheckedChanged);
            // 
            // checkBox_checkForUpdates
            // 
            this.checkBox_checkForUpdates.AutoSize = true;
            this.checkBox_checkForUpdates.Checked = true;
            this.checkBox_checkForUpdates.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_checkForUpdates.Location = new System.Drawing.Point(6, 65);
            this.checkBox_checkForUpdates.Name = "checkBox_checkForUpdates";
            this.checkBox_checkForUpdates.Size = new System.Drawing.Size(143, 17);
            this.checkBox_checkForUpdates.TabIndex = 33;
            this.checkBox_checkForUpdates.Text = "check online for updates";
            this.toolTip1.SetToolTip(this.checkBox_checkForUpdates, "Periodically check the XBSlink website for program updates.");
            this.checkBox_checkForUpdates.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.checkBox_chat_notify);
            this.groupBox5.Controls.Add(this.checkBox_newNodeSound);
            this.groupBox5.Location = new System.Drawing.Point(6, 143);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(326, 67);
            this.groupBox5.TabIndex = 34;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "sound";
            // 
            // checkBox_chat_notify
            // 
            this.checkBox_chat_notify.AutoSize = true;
            this.checkBox_chat_notify.Checked = true;
            this.checkBox_chat_notify.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_chat_notify.Location = new System.Drawing.Point(6, 19);
            this.checkBox_chat_notify.Name = "checkBox_chat_notify";
            this.checkBox_chat_notify.Size = new System.Drawing.Size(160, 17);
            this.checkBox_chat_notify.TabIndex = 3;
            this.checkBox_chat_notify.Text = "play sound on new message";
            this.checkBox_chat_notify.UseVisualStyleBackColor = true;
            this.checkBox_chat_notify.CheckedChanged += new System.EventHandler(this.checkBox_chat_notify_CheckedChanged);
            // 
            // checkBox_newNodeSound
            // 
            this.checkBox_newNodeSound.AutoSize = true;
            this.checkBox_newNodeSound.Checked = true;
            this.checkBox_newNodeSound.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_newNodeSound.Location = new System.Drawing.Point(6, 42);
            this.checkBox_newNodeSound.Name = "checkBox_newNodeSound";
            this.checkBox_newNodeSound.Size = new System.Drawing.Size(203, 17);
            this.checkBox_newNodeSound.TabIndex = 31;
            this.checkBox_newNodeSound.Text = "play sound when node joins or leaves";
            this.checkBox_newNodeSound.UseVisualStyleBackColor = true;
            this.checkBox_newNodeSound.CheckedChanged += new System.EventHandler(this.checkBox_newNodeSound_CheckedChanged);
            // 
            // groupBox_chatsettings
            // 
            this.groupBox_chatsettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_chatsettings.Controls.Add(this.checkBox_chat_nodeInfoMessagesInChat);
            this.groupBox_chatsettings.Controls.Add(this.checkBox_chatAutoSwitch);
            this.groupBox_chatsettings.Location = new System.Drawing.Point(6, 72);
            this.groupBox_chatsettings.Name = "groupBox_chatsettings";
            this.groupBox_chatsettings.Size = new System.Drawing.Size(326, 65);
            this.groupBox_chatsettings.TabIndex = 30;
            this.groupBox_chatsettings.TabStop = false;
            this.groupBox_chatsettings.Text = "chat settings";
            // 
            // checkBox_chat_nodeInfoMessagesInChat
            // 
            this.checkBox_chat_nodeInfoMessagesInChat.AutoSize = true;
            this.checkBox_chat_nodeInfoMessagesInChat.Checked = true;
            this.checkBox_chat_nodeInfoMessagesInChat.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_chat_nodeInfoMessagesInChat.Location = new System.Drawing.Point(6, 42);
            this.checkBox_chat_nodeInfoMessagesInChat.Name = "checkBox_chat_nodeInfoMessagesInChat";
            this.checkBox_chat_nodeInfoMessagesInChat.Size = new System.Drawing.Size(128, 17);
            this.checkBox_chat_nodeInfoMessagesInChat.TabIndex = 32;
            this.checkBox_chat_nodeInfoMessagesInChat.Text = "join/leave info in chat";
            this.checkBox_chat_nodeInfoMessagesInChat.UseVisualStyleBackColor = true;
            this.checkBox_chat_nodeInfoMessagesInChat.CheckedChanged += new System.EventHandler(this.checkBox_chat_nodeInfoMessagesInChat_CheckedChanged);
            // 
            // checkBox_chatAutoSwitch
            // 
            this.checkBox_chatAutoSwitch.AutoSize = true;
            this.checkBox_chatAutoSwitch.Checked = true;
            this.checkBox_chatAutoSwitch.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_chatAutoSwitch.Location = new System.Drawing.Point(6, 19);
            this.checkBox_chatAutoSwitch.Name = "checkBox_chatAutoSwitch";
            this.checkBox_chatAutoSwitch.Size = new System.Drawing.Size(134, 17);
            this.checkBox_chatAutoSwitch.TabIndex = 2;
            this.checkBox_chatAutoSwitch.Text = "auto switch to chat tab";
            this.toolTip1.SetToolTip(this.checkBox_chatAutoSwitch, "enable this option if you whish to automatically switch to the chat tab when mess" +
        "ages arrive");
            this.checkBox_chatAutoSwitch.UseVisualStyleBackColor = true;
            this.checkBox_chatAutoSwitch.CheckedChanged += new System.EventHandler(this.checkBox_chatAutoSwitch_CheckedChanged);
            // 
            // tabPageSettings_network
            // 
            this.tabPageSettings_network.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabPageSettings_network.Controls.Add(this.groupBox1);
            this.tabPageSettings_network.Controls.Add(this.groupBox_portoptions);
            this.tabPageSettings_network.Controls.Add(this.groupBox_packetfilter);
            this.tabPageSettings_network.Controls.Add(this.groupBox_MAClist);
            this.tabPageSettings_network.Location = new System.Drawing.Point(4, 25);
            this.tabPageSettings_network.Name = "tabPageSettings_network";
            this.tabPageSettings_network.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSettings_network.Size = new System.Drawing.Size(340, 423);
            this.tabPageSettings_network.TabIndex = 0;
            this.tabPageSettings_network.Text = "Network";
            this.tabPageSettings_network.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.comboBox_captureDevice);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(326, 61);
            this.groupBox1.TabIndex = 40;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "packet sniffer";
            // 
            // groupBox_portoptions
            // 
            this.groupBox_portoptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_portoptions.Controls.Add(this.checkbox_UPnP);
            this.groupBox_portoptions.Controls.Add(this.checkBox_useCloudServerForPortCheck);
            this.groupBox_portoptions.Controls.Add(this.label3);
            this.groupBox_portoptions.Controls.Add(this.label6);
            this.groupBox_portoptions.Controls.Add(this.label4);
            this.groupBox_portoptions.Controls.Add(this.comboBox_localIP);
            this.groupBox_portoptions.Controls.Add(this.textBox_local_Port);
            this.groupBox_portoptions.Location = new System.Drawing.Point(6, 73);
            this.groupBox_portoptions.Name = "groupBox_portoptions";
            this.groupBox_portoptions.Size = new System.Drawing.Size(326, 107);
            this.groupBox_portoptions.TabIndex = 39;
            this.groupBox_portoptions.TabStop = false;
            this.groupBox_portoptions.Text = "internet connection";
            // 
            // checkBox_useCloudServerForPortCheck
            // 
            this.checkBox_useCloudServerForPortCheck.AutoSize = true;
            this.checkBox_useCloudServerForPortCheck.Checked = true;
            this.checkBox_useCloudServerForPortCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_useCloudServerForPortCheck.Location = new System.Drawing.Point(6, 85);
            this.checkBox_useCloudServerForPortCheck.Name = "checkBox_useCloudServerForPortCheck";
            this.checkBox_useCloudServerForPortCheck.Size = new System.Drawing.Size(215, 17);
            this.checkBox_useCloudServerForPortCheck.TabIndex = 32;
            this.checkBox_useCloudServerForPortCheck.Text = "use cloud server to check incoming port";
            this.toolTip1.SetToolTip(this.checkBox_useCloudServerForPortCheck, "if selected, the cloud server wil be asked to send a test packet back");
            this.checkBox_useCloudServerForPortCheck.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(203, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "incoming port";
            // 
            // groupBox_packetfilter
            // 
            this.groupBox_packetfilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_packetfilter.Controls.Add(this.checkBox_forward_all_high_port_broadcast);
            this.groupBox_packetfilter.Controls.Add(this.checkBox_filter_wellknown_ports);
            this.groupBox_packetfilter.Controls.Add(this.checkBox_excludeGatewayIPs);
            this.groupBox_packetfilter.Location = new System.Drawing.Point(6, 186);
            this.groupBox_packetfilter.Name = "groupBox_packetfilter";
            this.groupBox_packetfilter.Size = new System.Drawing.Size(326, 91);
            this.groupBox_packetfilter.TabIndex = 38;
            this.groupBox_packetfilter.TabStop = false;
            this.groupBox_packetfilter.Text = "packet filter";
            // 
            // checkBox_forward_all_high_port_broadcast
            // 
            this.checkBox_forward_all_high_port_broadcast.AutoSize = true;
            this.checkBox_forward_all_high_port_broadcast.Checked = true;
            this.checkBox_forward_all_high_port_broadcast.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_forward_all_high_port_broadcast.Location = new System.Drawing.Point(6, 65);
            this.checkBox_forward_all_high_port_broadcast.Name = "checkBox_forward_all_high_port_broadcast";
            this.checkBox_forward_all_high_port_broadcast.Size = new System.Drawing.Size(254, 17);
            this.checkBox_forward_all_high_port_broadcast.TabIndex = 37;
            this.checkBox_forward_all_high_port_broadcast.Text = "forward all udp broadcast on high ports (p>1024)";
            this.toolTip1.SetToolTip(this.checkBox_forward_all_high_port_broadcast, "This option can be usefull if XBSlink can not see the traffic from your console.");
            this.checkBox_forward_all_high_port_broadcast.UseVisualStyleBackColor = true;
            this.checkBox_forward_all_high_port_broadcast.CheckedChanged += new System.EventHandler(this.checkBox_forward_all_high_port_broadcast_CheckedChanged);
            // 
            // checkBox_filter_wellknown_ports
            // 
            this.checkBox_filter_wellknown_ports.AutoSize = true;
            this.checkBox_filter_wellknown_ports.Checked = true;
            this.checkBox_filter_wellknown_ports.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_filter_wellknown_ports.Location = new System.Drawing.Point(6, 19);
            this.checkBox_filter_wellknown_ports.Name = "checkBox_filter_wellknown_ports";
            this.checkBox_filter_wellknown_ports.Size = new System.Drawing.Size(225, 17);
            this.checkBox_filter_wellknown_ports.TabIndex = 35;
            this.checkBox_filter_wellknown_ports.Text = "filter packets to well known ports (p<1024)";
            this.toolTip1.SetToolTip(this.checkBox_filter_wellknown_ports, "This security feature prevents XBSlink from accidently forwarding packets of loca" +
        "l services.");
            this.checkBox_filter_wellknown_ports.UseVisualStyleBackColor = true;
            this.checkBox_filter_wellknown_ports.CheckedChanged += new System.EventHandler(this.checkBox_filter_wellknown_ports_CheckedChanged);
            // 
            // checkBox_excludeGatewayIPs
            // 
            this.checkBox_excludeGatewayIPs.AutoSize = true;
            this.checkBox_excludeGatewayIPs.Checked = true;
            this.checkBox_excludeGatewayIPs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_excludeGatewayIPs.Location = new System.Drawing.Point(6, 42);
            this.checkBox_excludeGatewayIPs.Name = "checkBox_excludeGatewayIPs";
            this.checkBox_excludeGatewayIPs.Size = new System.Drawing.Size(202, 17);
            this.checkBox_excludeGatewayIPs.TabIndex = 36;
            this.checkBox_excludeGatewayIPs.Text = "exclude gateway IPs in packet sniffer";
            this.toolTip1.SetToolTip(this.checkBox_excludeGatewayIPs, "Protects your network from being disrupted by a faulty XBSlink node.");
            this.checkBox_excludeGatewayIPs.UseVisualStyleBackColor = true;
            this.checkBox_excludeGatewayIPs.CheckedChanged += new System.EventHandler(this.checkBox_excludeGatewayIPs_CheckedChanged);
            // 
            // groupBox_MAClist
            // 
            this.groupBox_MAClist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_MAClist.Controls.Add(this.checkBox_mac_restriction);
            this.groupBox_MAClist.Controls.Add(this.checkBox_enable_MAC_list);
            this.groupBox_MAClist.Controls.Add(this.button_del_MAC);
            this.groupBox_MAClist.Controls.Add(this.button_add_MAC);
            this.groupBox_MAClist.Controls.Add(this.textBox_add_MAC);
            this.groupBox_MAClist.Controls.Add(this.listBox_MAC_list);
            this.groupBox_MAClist.Location = new System.Drawing.Point(6, 283);
            this.groupBox_MAClist.Name = "groupBox_MAClist";
            this.groupBox_MAClist.Size = new System.Drawing.Size(326, 130);
            this.groupBox_MAClist.TabIndex = 21;
            this.groupBox_MAClist.TabStop = false;
            this.groupBox_MAClist.Text = "\"always forward\" MAC list";
            this.toolTip1.SetToolTip(this.groupBox_MAClist, "XBSlink will always forward packets from network devices with a MAC address liste" +
        "d here.");
            // 
            // checkBox_mac_restriction
            // 
            this.checkBox_mac_restriction.AutoSize = true;
            this.checkBox_mac_restriction.Checked = true;
            this.checkBox_mac_restriction.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_mac_restriction.Location = new System.Drawing.Point(6, 42);
            this.checkBox_mac_restriction.Name = "checkBox_mac_restriction";
            this.checkBox_mac_restriction.Size = new System.Drawing.Size(152, 17);
            this.checkBox_mac_restriction.TabIndex = 23;
            this.checkBox_mac_restriction.Text = "only forward these devices";
            this.toolTip1.SetToolTip(this.checkBox_mac_restriction, "only packets from the listed MAC addresses will be forwarded by XBSlink");
            this.checkBox_mac_restriction.UseVisualStyleBackColor = true;
            this.checkBox_mac_restriction.CheckedChanged += new System.EventHandler(this.checkBox_mac_restriction_CheckedChanged);
            // 
            // checkBox_enable_MAC_list
            // 
            this.checkBox_enable_MAC_list.AutoSize = true;
            this.checkBox_enable_MAC_list.Location = new System.Drawing.Point(6, 19);
            this.checkBox_enable_MAC_list.Name = "checkBox_enable_MAC_list";
            this.checkBox_enable_MAC_list.Size = new System.Drawing.Size(135, 17);
            this.checkBox_enable_MAC_list.TabIndex = 22;
            this.checkBox_enable_MAC_list.Text = "enable special MAC list";
            this.toolTip1.SetToolTip(this.checkBox_enable_MAC_list, "enable or disbale the \"always forward\" MAC list");
            this.checkBox_enable_MAC_list.UseVisualStyleBackColor = true;
            this.checkBox_enable_MAC_list.CheckedChanged += new System.EventHandler(this.checkBox_enable_MAC_list_CheckedChanged);
            // 
            // button_del_MAC
            // 
            this.button_del_MAC.Enabled = false;
            this.button_del_MAC.Location = new System.Drawing.Point(221, 100);
            this.button_del_MAC.Name = "button_del_MAC";
            this.button_del_MAC.Size = new System.Drawing.Size(39, 21);
            this.button_del_MAC.TabIndex = 21;
            this.button_del_MAC.Text = "Del";
            this.button_del_MAC.UseVisualStyleBackColor = true;
            this.button_del_MAC.Click += new System.EventHandler(this.button_del_MAC_Click);
            // 
            // button_add_MAC
            // 
            this.button_add_MAC.Enabled = false;
            this.button_add_MAC.Location = new System.Drawing.Point(168, 100);
            this.button_add_MAC.Name = "button_add_MAC";
            this.button_add_MAC.Size = new System.Drawing.Size(39, 21);
            this.button_add_MAC.TabIndex = 20;
            this.button_add_MAC.Text = "Add";
            this.button_add_MAC.UseVisualStyleBackColor = true;
            this.button_add_MAC.Click += new System.EventHandler(this.button_add_MAC_Click);
            // 
            // textBox_add_MAC
            // 
            this.textBox_add_MAC.AcceptsReturn = true;
            this.textBox_add_MAC.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBox_add_MAC.Location = new System.Drawing.Point(168, 65);
            this.textBox_add_MAC.MaxLength = 17;
            this.textBox_add_MAC.Name = "textBox_add_MAC";
            this.textBox_add_MAC.Size = new System.Drawing.Size(92, 20);
            this.textBox_add_MAC.TabIndex = 19;
            this.toolTip1.SetToolTip(this.textBox_add_MAC, "enter a MAC address in to format \"AABBCCDDEEFF\"");
            this.textBox_add_MAC.TextChanged += new System.EventHandler(this.textBox_add_MAC_TextChanged);
            // 
            // listBox_MAC_list
            // 
            this.listBox_MAC_list.FormattingEnabled = true;
            this.listBox_MAC_list.Location = new System.Drawing.Point(6, 65);
            this.listBox_MAC_list.Name = "listBox_MAC_list";
            this.listBox_MAC_list.Size = new System.Drawing.Size(156, 56);
            this.listBox_MAC_list.TabIndex = 17;
            this.listBox_MAC_list.SelectedIndexChanged += new System.EventHandler(this.listBox_MAC_list_SelectedIndexChanged);
            // 
            // button_reset_settings
            // 
            this.button_reset_settings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_reset_settings.Location = new System.Drawing.Point(117, 464);
            this.button_reset_settings.Name = "button_reset_settings";
            this.button_reset_settings.Size = new System.Drawing.Size(55, 23);
            this.button_reset_settings.TabIndex = 34;
            this.button_reset_settings.Text = "reset";
            this.button_reset_settings.UseVisualStyleBackColor = true;
            this.button_reset_settings.Click += new System.EventHandler(this.button_reset_settings_Click);
            // 
            // button_save_settings
            // 
            this.button_save_settings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_save_settings.Location = new System.Drawing.Point(6, 464);
            this.button_save_settings.Name = "button_save_settings";
            this.button_save_settings.Size = new System.Drawing.Size(105, 23);
            this.button_save_settings.TabIndex = 23;
            this.button_save_settings.Text = "save settings";
            this.button_save_settings.UseVisualStyleBackColor = true;
            this.button_save_settings.Click += new System.EventHandler(this.button_save_settings_Click);
            // 
            // tabPage_nat
            // 
            this.tabPage_nat.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_nat.Controls.Add(this.label18);
            this.tabPage_nat.Controls.Add(this.comboBox_nat_netmask);
            this.tabPage_nat.Controls.Add(this.button_nat_ippool_del);
            this.tabPage_nat.Controls.Add(this.button_nat_add_iprange);
            this.tabPage_nat.Controls.Add(this.textBox_nat_iprange_to);
            this.tabPage_nat.Controls.Add(this.textBox_nat_iprange_from);
            this.tabPage_nat.Controls.Add(this.label20);
            this.tabPage_nat.Controls.Add(this.label19);
            this.tabPage_nat.Controls.Add(this.listView_nat_IPpool);
            this.tabPage_nat.Controls.Add(this.groupBox3);
            this.tabPage_nat.Controls.Add(this.label17);
            this.tabPage_nat.Location = new System.Drawing.Point(4, 22);
            this.tabPage_nat.Name = "tabPage_nat";
            this.tabPage_nat.Size = new System.Drawing.Size(360, 493);
            this.tabPage_nat.TabIndex = 6;
            this.tabPage_nat.Text = "NAT";
            // 
            // label18
            // 
            this.label18.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(27, 469);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(68, 13);
            this.label18.TabIndex = 14;
            this.label18.Text = "IP Netmask :";
            // 
            // comboBox_nat_netmask
            // 
            this.comboBox_nat_netmask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBox_nat_netmask.FormattingEnabled = true;
            this.comboBox_nat_netmask.Items.AddRange(new object[] {
            "255.255.255.0",
            "255.255.0.0",
            "255.0.0.0"});
            this.comboBox_nat_netmask.Location = new System.Drawing.Point(101, 466);
            this.comboBox_nat_netmask.Name = "comboBox_nat_netmask";
            this.comboBox_nat_netmask.Size = new System.Drawing.Size(94, 21);
            this.comboBox_nat_netmask.TabIndex = 13;
            this.comboBox_nat_netmask.Text = "255.255.255.0";
            // 
            // button_nat_ippool_del
            // 
            this.button_nat_ippool_del.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_nat_ippool_del.Location = new System.Drawing.Point(246, 440);
            this.button_nat_ippool_del.Name = "button_nat_ippool_del";
            this.button_nat_ippool_del.Size = new System.Drawing.Size(99, 20);
            this.button_nat_ippool_del.TabIndex = 12;
            this.button_nat_ippool_del.Text = "Remove selected";
            this.button_nat_ippool_del.UseVisualStyleBackColor = true;
            this.button_nat_ippool_del.Click += new System.EventHandler(this.button_nat_ippool_del_Click);
            // 
            // button_nat_add_iprange
            // 
            this.button_nat_add_iprange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_nat_add_iprange.Location = new System.Drawing.Point(201, 440);
            this.button_nat_add_iprange.Name = "button_nat_add_iprange";
            this.button_nat_add_iprange.Size = new System.Drawing.Size(39, 20);
            this.button_nat_add_iprange.TabIndex = 11;
            this.button_nat_add_iprange.Text = "Add";
            this.button_nat_add_iprange.UseVisualStyleBackColor = true;
            this.button_nat_add_iprange.Click += new System.EventHandler(this.button_nat_add_iprange_Click);
            // 
            // textBox_nat_iprange_to
            // 
            this.textBox_nat_iprange_to.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox_nat_iprange_to.ForeColor = System.Drawing.Color.LightGray;
            this.textBox_nat_iprange_to.Location = new System.Drawing.Point(101, 440);
            this.textBox_nat_iprange_to.Name = "textBox_nat_iprange_to";
            this.textBox_nat_iprange_to.Size = new System.Drawing.Size(94, 20);
            this.textBox_nat_iprange_to.TabIndex = 10;
            this.textBox_nat_iprange_to.Text = "To";
            this.textBox_nat_iprange_to.TextChanged += new System.EventHandler(this.textBox_nat_iprange_to_TextChanged);
            this.textBox_nat_iprange_to.Enter += new System.EventHandler(this.textBox_nat_iprange_to_Enter);
            this.textBox_nat_iprange_to.Leave += new System.EventHandler(this.textBox_nat_iprange_to_Leave);
            // 
            // textBox_nat_iprange_from
            // 
            this.textBox_nat_iprange_from.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox_nat_iprange_from.ForeColor = System.Drawing.Color.LightGray;
            this.textBox_nat_iprange_from.Location = new System.Drawing.Point(6, 440);
            this.textBox_nat_iprange_from.Name = "textBox_nat_iprange_from";
            this.textBox_nat_iprange_from.Size = new System.Drawing.Size(94, 20);
            this.textBox_nat_iprange_from.TabIndex = 9;
            this.textBox_nat_iprange_from.Text = "From";
            this.textBox_nat_iprange_from.TextChanged += new System.EventHandler(this.textBox_nat_iprange_from_TextChanged);
            this.textBox_nat_iprange_from.Enter += new System.EventHandler(this.textBox_nat_iprange_from_Enter);
            this.textBox_nat_iprange_from.Leave += new System.EventHandler(this.textBox_nat_iprange_from_Leave);
            // 
            // label20
            // 
            this.label20.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(3, 424);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(107, 13);
            this.label20.TabIndex = 8;
            this.label20.Text = "Add IP range to pool:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(3, 118);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(105, 13);
            this.label19.TabIndex = 4;
            this.label19.Text = "NAT IP address pool";
            // 
            // listView_nat_IPpool
            // 
            this.listView_nat_IPpool.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView_nat_IPpool.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_nat_ippool_localIP,
            this.columnHeader_nat_ippool_device,
            this.columnHeader_nat_ippool__originalIP,
            this.columnHeader_nat_ippool_node});
            this.listView_nat_IPpool.GridLines = true;
            this.listView_nat_IPpool.Location = new System.Drawing.Point(0, 134);
            this.listView_nat_IPpool.Name = "listView_nat_IPpool";
            this.listView_nat_IPpool.Size = new System.Drawing.Size(348, 287);
            this.listView_nat_IPpool.TabIndex = 3;
            this.listView_nat_IPpool.UseCompatibleStateImageBehavior = false;
            this.listView_nat_IPpool.View = System.Windows.Forms.View.Details;
            this.listView_nat_IPpool.Resize += new System.EventHandler(this.listView_nat_IPpool_Resize);
            // 
            // columnHeader_nat_ippool_localIP
            // 
            this.columnHeader_nat_ippool_localIP.Text = "local IP";
            this.columnHeader_nat_ippool_localIP.Width = 94;
            // 
            // columnHeader_nat_ippool_device
            // 
            this.columnHeader_nat_ippool_device.Text = "Device";
            this.columnHeader_nat_ippool_device.Width = 91;
            // 
            // columnHeader_nat_ippool__originalIP
            // 
            this.columnHeader_nat_ippool__originalIP.Text = "original IP";
            this.columnHeader_nat_ippool__originalIP.Width = 95;
            // 
            // columnHeader_nat_ippool_node
            // 
            this.columnHeader_nat_ippool_node.Text = "Node";
            this.columnHeader_nat_ippool_node.Width = 143;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.checkBox_NAT_enablePS3mode);
            this.groupBox3.Controls.Add(this.checkBox_nat_useDHCP);
            this.groupBox3.Controls.Add(this.checkBox_nat_enable);
            this.groupBox3.Location = new System.Drawing.Point(6, 27);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(339, 88);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "NAT settings";
            // 
            // checkBox_NAT_enablePS3mode
            // 
            this.checkBox_NAT_enablePS3mode.AutoSize = true;
            this.checkBox_NAT_enablePS3mode.Enabled = false;
            this.checkBox_NAT_enablePS3mode.Location = new System.Drawing.Point(6, 42);
            this.checkBox_NAT_enablePS3mode.Name = "checkBox_NAT_enablePS3mode";
            this.checkBox_NAT_enablePS3mode.Size = new System.Drawing.Size(238, 17);
            this.checkBox_NAT_enablePS3mode.TabIndex = 4;
            this.checkBox_NAT_enablePS3mode.Text = "enable PS3 compatibilty mode (not ready yet)";
            this.checkBox_NAT_enablePS3mode.UseVisualStyleBackColor = true;
            this.checkBox_NAT_enablePS3mode.CheckedChanged += new System.EventHandler(this.checkBox_NAT_enablePS3mode_CheckedChanged);
            // 
            // checkBox_nat_useDHCP
            // 
            this.checkBox_nat_useDHCP.AutoSize = true;
            this.checkBox_nat_useDHCP.Enabled = false;
            this.checkBox_nat_useDHCP.Location = new System.Drawing.Point(6, 65);
            this.checkBox_nat_useDHCP.Name = "checkBox_nat_useDHCP";
            this.checkBox_nat_useDHCP.Size = new System.Drawing.Size(276, 17);
            this.checkBox_nat_useDHCP.TabIndex = 3;
            this.checkBox_nat_useDHCP.Text = "use local DHCP server to fill IP pool (planned feature)";
            this.toolTip1.SetToolTip(this.checkBox_nat_useDHCP, "not implemented yet, sorry");
            this.checkBox_nat_useDHCP.UseVisualStyleBackColor = true;
            this.checkBox_nat_useDHCP.CheckedChanged += new System.EventHandler(this.checkBox_nat_useDHCP_CheckedChanged);
            // 
            // checkBox_nat_enable
            // 
            this.checkBox_nat_enable.AutoSize = true;
            this.checkBox_nat_enable.Location = new System.Drawing.Point(6, 19);
            this.checkBox_nat_enable.Name = "checkBox_nat_enable";
            this.checkBox_nat_enable.Size = new System.Drawing.Size(202, 17);
            this.checkBox_nat_enable.TabIndex = 0;
            this.checkBox_nat_enable.Text = "enable source IP NAT (experimental!)";
            this.checkBox_nat_enable.UseVisualStyleBackColor = true;
            this.checkBox_nat_enable.CheckedChanged += new System.EventHandler(this.checkBox_nat_enable_CheckedChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(3, 9);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(276, 13);
            this.label17.TabIndex = 1;
            this.label17.Text = "NAT IP translation rewrites all remote packets to local IPs";
            // 
            // tabPage_about
            // 
            this.tabPage_about.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_about.Controls.Add(this.richTextBox_about);
            this.tabPage_about.Location = new System.Drawing.Point(4, 22);
            this.tabPage_about.Name = "tabPage_about";
            this.tabPage_about.Size = new System.Drawing.Size(360, 493);
            this.tabPage_about.TabIndex = 3;
            this.tabPage_about.Text = "About";
            // 
            // richTextBox_about
            // 
            this.richTextBox_about.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox_about.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox_about.Location = new System.Drawing.Point(0, 3);
            this.richTextBox_about.Name = "richTextBox_about";
            this.richTextBox_about.ReadOnly = true;
            this.richTextBox_about.Size = new System.Drawing.Size(345, 507);
            this.richTextBox_about.TabIndex = 0;
            this.richTextBox_about.Text = "";
            this.richTextBox_about.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBox_about_LinkClicked);
            // 
            // timer_messages
            // 
            this.timer_messages.Tick += new System.EventHandler(this.timer_messages_Tick);
            // 
            // comboBox_RemoteHost
            // 
            this.comboBox_RemoteHost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_RemoteHost.FormattingEnabled = true;
            this.comboBox_RemoteHost.Location = new System.Drawing.Point(86, 39);
            this.comboBox_RemoteHost.Name = "comboBox_RemoteHost";
            this.comboBox_RemoteHost.Size = new System.Drawing.Size(245, 21);
            this.comboBox_RemoteHost.TabIndex = 18;
            this.comboBox_RemoteHost.Text = "otherhost.dyndns.org";
            // 
            // timer_startEngine
            // 
            this.timer_startEngine.Tick += new System.EventHandler(this.timer_startEngine_Tick);
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            // 
            // toolTip2
            // 
            this.toolTip2.ShowAlways = true;
            // 
            // contextMenuStrip_systray
            // 
            this.contextMenuStrip_systray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_exit});
            this.contextMenuStrip_systray.Name = "contextMenuStrip_systray";
            this.contextMenuStrip_systray.Size = new System.Drawing.Size(135, 26);
            // 
            // toolStripMenuItem_exit
            // 
            this.toolStripMenuItem_exit.Name = "toolStripMenuItem_exit";
            this.toolStripMenuItem_exit.Size = new System.Drawing.Size(134, 22);
            this.toolStripMenuItem_exit.Text = "Exit XBSlink";
            this.toolStripMenuItem_exit.Click += new System.EventHandler(this.toolStripMenuItem_exit_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 610);
            this.Controls.Add(this.comboBox_RemoteHost);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button_announce);
            this.Controls.Add(this.textBox_remote_port);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_start_engine);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.label5);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(390, 220);
            this.Name = "FormMain";
            this.Text = "XBSlink";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage_newsFeed.ResumeLayout(false);
            this.tabPage_info.ResumeLayout(false);
            this.tabPage_info.PerformLayout();
            this.tabPage_clouds.ResumeLayout(false);
            this.tabPage_clouds.PerformLayout();
            this.tabPage_chat.ResumeLayout(false);
            this.tabPage_chat.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.tabPage_messages.ResumeLayout(false);
            this.tabPage_settings.ResumeLayout(false);
            this.tabControl_settings.ResumeLayout(false);
            this.tabPageSettings_general.ResumeLayout(false);
            this.groupBox_newsFeed.ResumeLayout(false);
            this.groupBox_newsFeed.PerformLayout();
            this.groupBox_usersettings.ResumeLayout(false);
            this.groupBox_usersettings.PerformLayout();
            this.groupBox_updates.ResumeLayout(false);
            this.groupBox_updates.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox_chatsettings.ResumeLayout(false);
            this.groupBox_chatsettings.PerformLayout();
            this.tabPageSettings_network.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox_portoptions.ResumeLayout(false);
            this.groupBox_portoptions.PerformLayout();
            this.groupBox_packetfilter.ResumeLayout(false);
            this.groupBox_packetfilter.PerformLayout();
            this.groupBox_MAClist.ResumeLayout(false);
            this.groupBox_MAClist.PerformLayout();
            this.tabPage_nat.ResumeLayout(false);
            this.tabPage_nat.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabPage_about.ResumeLayout(false);
            this.contextMenuStrip_systray.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ComboBox comboBox_captureDevice;
        private System.Windows.Forms.Button button_start_engine;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_local_Port;
        private System.Windows.Forms.TextBox textBox_remote_port;
        private System.Windows.Forms.Button button_announce;
        private System.Windows.Forms.ComboBox comboBox_localIP;
        private System.Windows.Forms.CheckBox checkbox_UPnP;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_sniffer_in;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_udp_in;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_udp_out;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage_info;
        private System.Windows.Forms.TabPage tabPage_settings;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox_MAClist;
        private System.Windows.Forms.Button button_add_MAC;
        private System.Windows.Forms.TextBox textBox_add_MAC;
        private System.Windows.Forms.ListBox listBox_MAC_list;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button_del_MAC;
        private System.Windows.Forms.Button button_save_settings;
        private System.Windows.Forms.CheckBox checkBox_enable_MAC_list;
        private System.Windows.Forms.TabPage tabPage_messages;
        private System.Windows.Forms.ListBox listBox_messages;
        private System.Windows.Forms.Button button_clearMessages;
        private System.Windows.Forms.TabPage tabPage_about;
        private System.Windows.Forms.RichTextBox richTextBox_about;
        private System.Windows.Forms.Timer timer_messages;
        private System.Windows.Forms.ComboBox comboBox_RemoteHost;
        private System.Windows.Forms.Timer timer_startEngine;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox_chatsettings;
        private System.Windows.Forms.CheckBox checkBox_chatAutoSwitch;
        private System.Windows.Forms.Label label_nickname;
        private System.Windows.Forms.TextBox textBox_chatNickname;
        private System.Windows.Forms.TabPage tabPage_chat;
        private System.Windows.Forms.TextBox textBox_chatEntry;
        private System.Windows.Forms.Button button_clearChat;
        private System.Windows.Forms.TextBox textBox_chatMessages;
        private System.Windows.Forms.TabPage tabPage_clouds;
        private System.Windows.Forms.ListView listView_clouds;
        private System.Windows.Forms.ColumnHeader columnHeader_cloudlistname;
        private System.Windows.Forms.ColumnHeader columnHeader_cloudlistnodecount;
        private System.Windows.Forms.ColumnHeader columnHeader_cloudlistmaxnodes;
        private System.Windows.Forms.Button buttonLoadCloudlist;
        private System.Windows.Forms.TextBox textBox_cloudlist;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox_CloudName;
        private System.Windows.Forms.Button button_CloudJoin;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBox_CloudPassword;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBox_CloudMaxNodes;
        private System.Windows.Forms.Button button_CloudLeave;
        private System.Windows.Forms.CheckBox checkBox_chat_notify;
        private System.Windows.Forms.CheckBox checkBox_newNodeSound;
        private System.Windows.Forms.Button button_messages_copy;
        private System.Windows.Forms.ToolTip toolTip2;
        private System.Windows.Forms.CheckBox checkBox_useCloudServerForPortCheck;
        private System.Windows.Forms.CheckBox checkBox_checkForUpdates;
        private System.Windows.Forms.CheckBox checkBox_mac_restriction;
        private System.Windows.Forms.ListView listView_nodes;
        private System.Windows.Forms.ColumnHeader columnHeader_nodeIP;
        private System.Windows.Forms.ColumnHeader columnHeader_nodePort;
        private System.Windows.Forms.ColumnHeader columnHeader_nodePing;
        private System.Windows.Forms.ColumnHeader columnHeader_nodeVersion;
        private System.Windows.Forms.ColumnHeader columnHeader_nodeNickname;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ListBox listBox_chatUserList;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label_num_persons_in_chat;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabPage tabPage_nat;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBox_nat_useDHCP;
        private System.Windows.Forms.CheckBox checkBox_nat_enable;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ListView listView_nat_IPpool;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Button button_nat_add_iprange;
        private System.Windows.Forms.TextBox textBox_nat_iprange_to;
        private System.Windows.Forms.TextBox textBox_nat_iprange_from;
        private System.Windows.Forms.ColumnHeader columnHeader_nat_ippool_localIP;
        private System.Windows.Forms.ColumnHeader columnHeader_nat_ippool_device;
        private System.Windows.Forms.ColumnHeader columnHeader_nat_ippool__originalIP;
        private System.Windows.Forms.ColumnHeader columnHeader_nat_ippool_node;
        private System.Windows.Forms.Button button_nat_ippool_del;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ComboBox comboBox_nat_netmask;
        private System.Windows.Forms.Button button_reset_settings;
        private System.Windows.Forms.CheckBox checkBox_filter_wellknown_ports;
        private System.Windows.Forms.CheckBox checkBox_NAT_enablePS3mode;
        private System.Windows.Forms.CheckBox checkBox_excludeGatewayIPs;
        private System.Windows.Forms.CheckBox checkBox_chat_nodeInfoMessagesInChat;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_icon;
        private System.Windows.Forms.CheckBox checkBox_forward_all_high_port_broadcast;
        private System.Windows.Forms.TabControl tabControl_settings;
        private System.Windows.Forms.TabPage tabPageSettings_network;
        private System.Windows.Forms.GroupBox groupBox_portoptions;
        private System.Windows.Forms.GroupBox groupBox_packetfilter;
        private System.Windows.Forms.TabPage tabPageSettings_general;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox_updates;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox_usersettings;
        private System.Windows.Forms.CheckBox checkBox_minimize2systray;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_systray;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_exit;
        private System.Windows.Forms.CheckBox checkBox_preventSystemStandby;
        private System.Windows.Forms.TabPage tabPage_newsFeed;
        private System.Windows.Forms.RichTextBox richTextBox_newsFeed;
        private System.Windows.Forms.GroupBox groupBox_newsFeed;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox checkBox_showNewsFeed;
        private System.Windows.Forms.TextBox textBox_newsFeedUri;
        private System.Windows.Forms.CheckBox checkBox_switchToNewsTab;
    }
}

