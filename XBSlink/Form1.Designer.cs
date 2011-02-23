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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_sniffer_in = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_udp_in = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_udp_out = new System.Windows.Forms.ToolStripStatusLabel();
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
            this.checkBox_all_broadcasts = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_info = new System.Windows.Forms.TabPage();
            this.label15 = new System.Windows.Forms.Label();
            this.listView_nodes = new System.Windows.Forms.ListView();
            this.columnHeader_nodeIP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_nodePort = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_ping = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_Version = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_nickname = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
            this.columnHeader_name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_nodecount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_maxnodes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage_chat = new System.Windows.Forms.TabPage();
            this.textBox_chatMessages = new System.Windows.Forms.TextBox();
            this.button_clearChat = new System.Windows.Forms.Button();
            this.textBox_chatEntry = new System.Windows.Forms.TextBox();
            this.tabPage_messages = new System.Windows.Forms.TabPage();
            this.button_messages_copy = new System.Windows.Forms.Button();
            this.listBox_messages = new System.Windows.Forms.ListBox();
            this.button_clearMessages = new System.Windows.Forms.Button();
            this.tabPage_settings = new System.Windows.Forms.TabPage();
            this.checkBox_checkForUpdates = new System.Windows.Forms.CheckBox();
            this.checkBox_useCloudServerForPortCheck = new System.Windows.Forms.CheckBox();
            this.checkBox_newNodeSound = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox_chat_notify = new System.Windows.Forms.CheckBox();
            this.checkBox_chatAutoSwitch = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox_chatNickname = new System.Windows.Forms.TextBox();
            this.checkBox_useStunServer = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_stunServerPort = new System.Windows.Forms.TextBox();
            this.textBox_stunServerHostname = new System.Windows.Forms.TextBox();
            this.button_save_settings = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox_mac_restriction = new System.Windows.Forms.CheckBox();
            this.checkBox_enable_MAC_list = new System.Windows.Forms.CheckBox();
            this.button_del_MAC = new System.Windows.Forms.Button();
            this.button_add_MAC = new System.Windows.Forms.Button();
            this.textBox_add_MAC = new System.Windows.Forms.TextBox();
            this.listBox_MAC_list = new System.Windows.Forms.ListBox();
            this.tabPage_about = new System.Windows.Forms.TabPage();
            this.richTextBox_about = new System.Windows.Forms.RichTextBox();
            this.timer_messages = new System.Windows.Forms.Timer(this.components);
            this.comboBox_RemoteHost = new System.Windows.Forms.ComboBox();
            this.timer_startEngine = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage_info.SuspendLayout();
            this.tabPage_clouds.SuspendLayout();
            this.tabPage_chat.SuspendLayout();
            this.tabPage_messages.SuspendLayout();
            this.tabPage_settings.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage_about.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel_sniffer_in,
            this.toolStripStatusLabel4,
            this.toolStripStatusLabel_udp_in,
            this.toolStripStatusLabel_udp_out});
            this.statusStrip1.Location = new System.Drawing.Point(0, 499);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(374, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.statusStrip1_MouseDoubleClick);
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
            // comboBox_captureDevice
            // 
            this.comboBox_captureDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_captureDevice.FormattingEnabled = true;
            this.comboBox_captureDevice.Location = new System.Drawing.Point(9, 29);
            this.comboBox_captureDevice.Name = "comboBox_captureDevice";
            this.comboBox_captureDevice.Size = new System.Drawing.Size(336, 21);
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
            this.textBox_local_Port.Location = new System.Drawing.Point(257, 59);
            this.textBox_local_Port.MaxLength = 5;
            this.textBox_local_Port.Name = "textBox_local_Port";
            this.textBox_local_Port.Size = new System.Drawing.Size(36, 20);
            this.textBox_local_Port.TabIndex = 7;
            this.textBox_local_Port.Text = "31415";
            this.textBox_local_Port.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_local_Port_KeyPress);
            this.textBox_local_Port.Leave += new System.EventHandler(this.textBox_local_Port_Leave);
            // 
            // textBox_remote_port
            // 
            this.textBox_remote_port.Location = new System.Drawing.Point(325, 39);
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
            this.button_announce.Size = new System.Drawing.Size(145, 23);
            this.button_announce.TabIndex = 9;
            this.button_announce.Text = "Announce to Remote Host";
            this.button_announce.UseVisualStyleBackColor = true;
            this.button_announce.Click += new System.EventHandler(this.button_announce_Click);
            // 
            // comboBox_localIP
            // 
            this.comboBox_localIP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_localIP.FormattingEnabled = true;
            this.comboBox_localIP.Location = new System.Drawing.Point(58, 59);
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
            this.checkbox_UPnP.Location = new System.Drawing.Point(9, 140);
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
            this.label2.Location = new System.Drawing.Point(3, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(261, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Capture Device (connected to same network as xbox)";
            this.toolTip1.SetToolTip(this.label2, "select the network device thats connected to the same network as your Xbox360");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Local IP :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(245, 62);
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
            // checkBox_all_broadcasts
            // 
            this.checkBox_all_broadcasts.AutoSize = true;
            this.checkBox_all_broadcasts.Location = new System.Drawing.Point(9, 186);
            this.checkBox_all_broadcasts.Name = "checkBox_all_broadcasts";
            this.checkBox_all_broadcasts.Size = new System.Drawing.Size(193, 17);
            this.checkBox_all_broadcasts.TabIndex = 16;
            this.checkBox_all_broadcasts.Text = "advanced forwarding of broadcasts";
            this.toolTip1.SetToolTip(this.checkBox_all_broadcasts, "this helps with some games. might also forward some unrelated broadcast packets.");
            this.checkBox_all_broadcasts.UseVisualStyleBackColor = true;
            this.checkBox_all_broadcasts.CheckedChanged += new System.EventHandler(this.checkBox_all_broadcasts_CheckedChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage_info);
            this.tabControl1.Controls.Add(this.tabPage_clouds);
            this.tabControl1.Controls.Add(this.tabPage_chat);
            this.tabControl1.Controls.Add(this.tabPage_messages);
            this.tabControl1.Controls.Add(this.tabPage_settings);
            this.tabControl1.Controls.Add(this.tabPage_about);
            this.tabControl1.Location = new System.Drawing.Point(10, 65);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(356, 430);
            this.tabControl1.TabIndex = 17;
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
            this.tabPage_info.Size = new System.Drawing.Size(348, 404);
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
            this.columnHeader_ping,
            this.columnHeader_Version,
            this.columnHeader_nickname});
            this.listView_nodes.GridLines = true;
            this.listView_nodes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView_nodes.LabelWrap = false;
            this.listView_nodes.Location = new System.Drawing.Point(0, 22);
            this.listView_nodes.MultiSelect = false;
            this.listView_nodes.Name = "listView_nodes";
            this.listView_nodes.ShowGroups = false;
            this.listView_nodes.Size = new System.Drawing.Size(348, 290);
            this.listView_nodes.TabIndex = 2;
            this.listView_nodes.UseCompatibleStateImageBehavior = false;
            this.listView_nodes.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader_nodeIP
            // 
            this.columnHeader_nodeIP.Text = "Address";
            this.columnHeader_nodeIP.Width = 86;
            // 
            // columnHeader_nodePort
            // 
            this.columnHeader_nodePort.Text = "Port";
            this.columnHeader_nodePort.Width = 47;
            // 
            // columnHeader_ping
            // 
            this.columnHeader_ping.Text = "Ping";
            this.columnHeader_ping.Width = 42;
            // 
            // columnHeader_Version
            // 
            this.columnHeader_Version.Text = "Version";
            this.columnHeader_Version.Width = 54;
            // 
            // columnHeader_nickname
            // 
            this.columnHeader_nickname.Text = "Nickname";
            this.columnHeader_nickname.Width = 114;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(0, 312);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(348, 92);
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
            this.tabPage_clouds.Size = new System.Drawing.Size(348, 404);
            this.tabPage_clouds.TabIndex = 5;
            this.tabPage_clouds.Text = "Clouds";
            // 
            // button_CloudLeave
            // 
            this.button_CloudLeave.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.button_CloudLeave.Enabled = false;
            this.button_CloudLeave.Location = new System.Drawing.Point(270, 363);
            this.button_CloudLeave.Name = "button_CloudLeave";
            this.button_CloudLeave.Size = new System.Drawing.Size(75, 21);
            this.button_CloudLeave.TabIndex = 11;
            this.button_CloudLeave.Text = "leave";
            this.button_CloudLeave.UseVisualStyleBackColor = true;
            this.button_CloudLeave.Click += new System.EventHandler(this.button_CloudLeave_Click);
            // 
            // button_CloudJoin
            // 
            this.button_CloudJoin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_CloudJoin.Enabled = false;
            this.button_CloudJoin.Location = new System.Drawing.Point(270, 384);
            this.button_CloudJoin.Name = "button_CloudJoin";
            this.button_CloudJoin.Size = new System.Drawing.Size(75, 21);
            this.button_CloudJoin.TabIndex = 10;
            this.button_CloudJoin.Text = "join/create";
            this.button_CloudJoin.UseVisualStyleBackColor = true;
            this.button_CloudJoin.Click += new System.EventHandler(this.button_CloudJoin_Click);
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(185, 365);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 13);
            this.label14.TabIndex = 9;
            this.label14.Text = "Password";
            // 
            // textBox_CloudPassword
            // 
            this.textBox_CloudPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox_CloudPassword.Enabled = false;
            this.textBox_CloudPassword.Location = new System.Drawing.Point(188, 381);
            this.textBox_CloudPassword.Name = "textBox_CloudPassword";
            this.textBox_CloudPassword.PasswordChar = '*';
            this.textBox_CloudPassword.Size = new System.Drawing.Size(76, 20);
            this.textBox_CloudPassword.TabIndex = 8;
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(146, 365);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(27, 13);
            this.label13.TabIndex = 7;
            this.label13.Text = "Max";
            // 
            // textBox_CloudMaxNodes
            // 
            this.textBox_CloudMaxNodes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox_CloudMaxNodes.Enabled = false;
            this.textBox_CloudMaxNodes.Location = new System.Drawing.Point(149, 381);
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
            this.label12.Location = new System.Drawing.Point(3, 365);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(63, 13);
            this.label12.TabIndex = 5;
            this.label12.Text = "Cloud name";
            // 
            // textBox_CloudName
            // 
            this.textBox_CloudName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox_CloudName.Enabled = false;
            this.textBox_CloudName.Location = new System.Drawing.Point(6, 381);
            this.textBox_CloudName.Name = "textBox_CloudName";
            this.textBox_CloudName.Size = new System.Drawing.Size(137, 20);
            this.textBox_CloudName.TabIndex = 4;
            // 
            // buttonLoadCloudlist
            // 
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
            this.textBox_cloudlist.Location = new System.Drawing.Point(61, 2);
            this.textBox_cloudlist.Name = "textBox_cloudlist";
            this.textBox_cloudlist.Size = new System.Drawing.Size(203, 20);
            this.textBox_cloudlist.TabIndex = 2;
            this.textBox_cloudlist.Text = "http://www.secudb.de/~seuffert/xbslink/cloudlist/";
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
            this.listView_clouds.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_name,
            this.columnHeader_nodecount,
            this.columnHeader_maxnodes});
            this.listView_clouds.FullRowSelect = true;
            this.listView_clouds.GridLines = true;
            this.listView_clouds.Location = new System.Drawing.Point(-2, 28);
            this.listView_clouds.MultiSelect = false;
            this.listView_clouds.Name = "listView_clouds";
            this.listView_clouds.ShowGroups = false;
            this.listView_clouds.Size = new System.Drawing.Size(350, 334);
            this.listView_clouds.TabIndex = 0;
            this.listView_clouds.UseCompatibleStateImageBehavior = false;
            this.listView_clouds.View = System.Windows.Forms.View.Details;
            this.listView_clouds.SelectedIndexChanged += new System.EventHandler(this.listView_clouds_SelectedIndexChanged);
            // 
            // columnHeader_name
            // 
            this.columnHeader_name.Text = "Name";
            this.columnHeader_name.Width = 204;
            // 
            // columnHeader_nodecount
            // 
            this.columnHeader_nodecount.Text = "Nodes";
            // 
            // columnHeader_maxnodes
            // 
            this.columnHeader_maxnodes.Text = "Max";
            // 
            // tabPage_chat
            // 
            this.tabPage_chat.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_chat.Controls.Add(this.textBox_chatMessages);
            this.tabPage_chat.Controls.Add(this.button_clearChat);
            this.tabPage_chat.Controls.Add(this.textBox_chatEntry);
            this.tabPage_chat.Location = new System.Drawing.Point(4, 22);
            this.tabPage_chat.Name = "tabPage_chat";
            this.tabPage_chat.Size = new System.Drawing.Size(348, 404);
            this.tabPage_chat.TabIndex = 4;
            this.tabPage_chat.Text = "Chat";
            // 
            // textBox_chatMessages
            // 
            this.textBox_chatMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_chatMessages.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textBox_chatMessages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_chatMessages.Location = new System.Drawing.Point(0, 0);
            this.textBox_chatMessages.Multiline = true;
            this.textBox_chatMessages.Name = "textBox_chatMessages";
            this.textBox_chatMessages.ReadOnly = true;
            this.textBox_chatMessages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_chatMessages.Size = new System.Drawing.Size(348, 383);
            this.textBox_chatMessages.TabIndex = 3;
            // 
            // button_clearChat
            // 
            this.button_clearChat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.button_clearChat.Location = new System.Drawing.Point(309, 383);
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
            this.textBox_chatEntry.Location = new System.Drawing.Point(0, 384);
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
            this.tabPage_messages.Size = new System.Drawing.Size(348, 404);
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
            this.listBox_messages.Location = new System.Drawing.Point(3, 36);
            this.listBox_messages.Name = "listBox_messages";
            this.listBox_messages.ScrollAlwaysVisible = true;
            this.listBox_messages.Size = new System.Drawing.Size(342, 366);
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
            this.tabPage_settings.Controls.Add(this.checkBox_checkForUpdates);
            this.tabPage_settings.Controls.Add(this.checkBox_useCloudServerForPortCheck);
            this.tabPage_settings.Controls.Add(this.checkBox_newNodeSound);
            this.tabPage_settings.Controls.Add(this.groupBox2);
            this.tabPage_settings.Controls.Add(this.checkBox_useStunServer);
            this.tabPage_settings.Controls.Add(this.label9);
            this.tabPage_settings.Controls.Add(this.label8);
            this.tabPage_settings.Controls.Add(this.label7);
            this.tabPage_settings.Controls.Add(this.textBox_stunServerPort);
            this.tabPage_settings.Controls.Add(this.textBox_stunServerHostname);
            this.tabPage_settings.Controls.Add(this.button_save_settings);
            this.tabPage_settings.Controls.Add(this.label6);
            this.tabPage_settings.Controls.Add(this.groupBox1);
            this.tabPage_settings.Controls.Add(this.comboBox_localIP);
            this.tabPage_settings.Controls.Add(this.checkBox_all_broadcasts);
            this.tabPage_settings.Controls.Add(this.textBox_local_Port);
            this.tabPage_settings.Controls.Add(this.comboBox_captureDevice);
            this.tabPage_settings.Controls.Add(this.checkbox_UPnP);
            this.tabPage_settings.Controls.Add(this.label2);
            this.tabPage_settings.Controls.Add(this.label4);
            this.tabPage_settings.Controls.Add(this.label3);
            this.tabPage_settings.Location = new System.Drawing.Point(4, 22);
            this.tabPage_settings.Name = "tabPage_settings";
            this.tabPage_settings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_settings.Size = new System.Drawing.Size(348, 404);
            this.tabPage_settings.TabIndex = 1;
            this.tabPage_settings.Text = "Settings";
            // 
            // checkBox_checkForUpdates
            // 
            this.checkBox_checkForUpdates.AutoSize = true;
            this.checkBox_checkForUpdates.Checked = true;
            this.checkBox_checkForUpdates.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_checkForUpdates.Location = new System.Drawing.Point(196, 379);
            this.checkBox_checkForUpdates.Name = "checkBox_checkForUpdates";
            this.checkBox_checkForUpdates.Size = new System.Drawing.Size(112, 17);
            this.checkBox_checkForUpdates.TabIndex = 33;
            this.checkBox_checkForUpdates.Text = "check for updates";
            this.toolTip1.SetToolTip(this.checkBox_checkForUpdates, "Periodically check the XBSlink website for program updates.");
            this.checkBox_checkForUpdates.UseVisualStyleBackColor = true;
            // 
            // checkBox_useCloudServerForPortCheck
            // 
            this.checkBox_useCloudServerForPortCheck.AutoSize = true;
            this.checkBox_useCloudServerForPortCheck.Checked = true;
            this.checkBox_useCloudServerForPortCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_useCloudServerForPortCheck.Location = new System.Drawing.Point(9, 163);
            this.checkBox_useCloudServerForPortCheck.Name = "checkBox_useCloudServerForPortCheck";
            this.checkBox_useCloudServerForPortCheck.Size = new System.Drawing.Size(215, 17);
            this.checkBox_useCloudServerForPortCheck.TabIndex = 32;
            this.checkBox_useCloudServerForPortCheck.Text = "use cloud server to check incoming port";
            this.toolTip1.SetToolTip(this.checkBox_useCloudServerForPortCheck, "if selected, the cloud server wil be asked to send a test packet back");
            this.checkBox_useCloudServerForPortCheck.UseVisualStyleBackColor = true;
            // 
            // checkBox_newNodeSound
            // 
            this.checkBox_newNodeSound.AutoSize = true;
            this.checkBox_newNodeSound.Checked = true;
            this.checkBox_newNodeSound.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_newNodeSound.Location = new System.Drawing.Point(9, 209);
            this.checkBox_newNodeSound.Name = "checkBox_newNodeSound";
            this.checkBox_newNodeSound.Size = new System.Drawing.Size(189, 17);
            this.checkBox_newNodeSound.TabIndex = 31;
            this.checkBox_newNodeSound.Text = "sound notification when node joins";
            this.checkBox_newNodeSound.UseVisualStyleBackColor = true;
            this.checkBox_newNodeSound.CheckedChanged += new System.EventHandler(this.checkBox_newNodeSound_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox_chat_notify);
            this.groupBox2.Controls.Add(this.checkBox_chatAutoSwitch);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.textBox_chatNickname);
            this.groupBox2.Location = new System.Drawing.Point(187, 232);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(155, 137);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "chat settings";
            // 
            // checkBox_chat_notify
            // 
            this.checkBox_chat_notify.AutoSize = true;
            this.checkBox_chat_notify.Checked = true;
            this.checkBox_chat_notify.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_chat_notify.Location = new System.Drawing.Point(9, 85);
            this.checkBox_chat_notify.Name = "checkBox_chat_notify";
            this.checkBox_chat_notify.Size = new System.Drawing.Size(109, 17);
            this.checkBox_chat_notify.TabIndex = 3;
            this.checkBox_chat_notify.Text = "sound notification";
            this.checkBox_chat_notify.UseVisualStyleBackColor = true;
            this.checkBox_chat_notify.CheckedChanged += new System.EventHandler(this.checkBox_chat_notify_CheckedChanged);
            // 
            // checkBox_chatAutoSwitch
            // 
            this.checkBox_chatAutoSwitch.AutoSize = true;
            this.checkBox_chatAutoSwitch.Checked = true;
            this.checkBox_chatAutoSwitch.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_chatAutoSwitch.Location = new System.Drawing.Point(9, 62);
            this.checkBox_chatAutoSwitch.Name = "checkBox_chatAutoSwitch";
            this.checkBox_chatAutoSwitch.Size = new System.Drawing.Size(134, 17);
            this.checkBox_chatAutoSwitch.TabIndex = 2;
            this.checkBox_chatAutoSwitch.Text = "auto switch to chat tab";
            this.toolTip1.SetToolTip(this.checkBox_chatAutoSwitch, "enable this option if you whish to automatically switch to the chat tab when mess" +
                    "ages arrive");
            this.checkBox_chatAutoSwitch.UseVisualStyleBackColor = true;
            this.checkBox_chatAutoSwitch.CheckedChanged += new System.EventHandler(this.checkBox_chatAutoSwitch_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "Nickname";
            // 
            // textBox_chatNickname
            // 
            this.textBox_chatNickname.Location = new System.Drawing.Point(9, 36);
            this.textBox_chatNickname.Name = "textBox_chatNickname";
            this.textBox_chatNickname.Size = new System.Drawing.Size(140, 20);
            this.textBox_chatNickname.TabIndex = 0;
            this.textBox_chatNickname.Text = "Anonymous";
            this.toolTip1.SetToolTip(this.textBox_chatNickname, "the nickname will appear in the chat as well as in the node list");
            this.textBox_chatNickname.Leave += new System.EventHandler(this.textBox_chatNickname_Leave);
            // 
            // checkBox_useStunServer
            // 
            this.checkBox_useStunServer.AutoSize = true;
            this.checkBox_useStunServer.Location = new System.Drawing.Point(9, 117);
            this.checkBox_useStunServer.Name = "checkBox_useStunServer";
            this.checkBox_useStunServer.Size = new System.Drawing.Size(213, 17);
            this.checkBox_useStunServer.TabIndex = 29;
            this.checkBox_useStunServer.Text = "use STUN Server to discover NAT type";
            this.checkBox_useStunServer.UseVisualStyleBackColor = true;
            this.checkBox_useStunServer.CheckedChanged += new System.EventHandler(this.checkBox_useStunServer_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(299, 89);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(26, 13);
            this.label9.TabIndex = 28;
            this.label9.Text = "Port";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(245, 89);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(10, 13);
            this.label8.TabIndex = 27;
            this.label8.Text = ":";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 89);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 13);
            this.label7.TabIndex = 26;
            this.label7.Text = "STUN :";
            // 
            // textBox_stunServerPort
            // 
            this.textBox_stunServerPort.Location = new System.Drawing.Point(257, 86);
            this.textBox_stunServerPort.MaxLength = 5;
            this.textBox_stunServerPort.Name = "textBox_stunServerPort";
            this.textBox_stunServerPort.Size = new System.Drawing.Size(36, 20);
            this.textBox_stunServerPort.TabIndex = 25;
            this.textBox_stunServerPort.Text = "3478";
            this.textBox_stunServerPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_stunServerPort_KeyPress);
            this.textBox_stunServerPort.Leave += new System.EventHandler(this.textBox_stunServerPort_Leave);
            // 
            // textBox_stunServerHostname
            // 
            this.textBox_stunServerHostname.Location = new System.Drawing.Point(58, 86);
            this.textBox_stunServerHostname.Name = "textBox_stunServerHostname";
            this.textBox_stunServerHostname.Size = new System.Drawing.Size(184, 20);
            this.textBox_stunServerHostname.TabIndex = 24;
            this.textBox_stunServerHostname.Text = "stun.sipgate.net";
            this.textBox_stunServerHostname.Leave += new System.EventHandler(this.textBox_stunServerHostname_Leave);
            // 
            // button_save_settings
            // 
            this.button_save_settings.Location = new System.Drawing.Point(9, 375);
            this.button_save_settings.Name = "button_save_settings";
            this.button_save_settings.Size = new System.Drawing.Size(105, 23);
            this.button_save_settings.TabIndex = 23;
            this.button_save_settings.Text = "save settings";
            this.button_save_settings.UseVisualStyleBackColor = true;
            this.button_save_settings.Click += new System.EventHandler(this.button_save_settings_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(299, 62);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(26, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "Port";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox_mac_restriction);
            this.groupBox1.Controls.Add(this.checkBox_enable_MAC_list);
            this.groupBox1.Controls.Add(this.button_del_MAC);
            this.groupBox1.Controls.Add(this.button_add_MAC);
            this.groupBox1.Controls.Add(this.textBox_add_MAC);
            this.groupBox1.Controls.Add(this.listBox_MAC_list);
            this.groupBox1.Location = new System.Drawing.Point(13, 232);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(168, 137);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "\"always forward\" MAC list";
            this.toolTip1.SetToolTip(this.groupBox1, "XBSlink will always forward packets from network devices with a MAC address liste" +
                    "d here.");
            // 
            // checkBox_mac_restriction
            // 
            this.checkBox_mac_restriction.AutoSize = true;
            this.checkBox_mac_restriction.Checked = true;
            this.checkBox_mac_restriction.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_mac_restriction.Enabled = false;
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
            this.button_del_MAC.Location = new System.Drawing.Point(129, 114);
            this.button_del_MAC.Name = "button_del_MAC";
            this.button_del_MAC.Size = new System.Drawing.Size(33, 21);
            this.button_del_MAC.TabIndex = 21;
            this.button_del_MAC.Text = "Del";
            this.button_del_MAC.UseVisualStyleBackColor = true;
            this.button_del_MAC.Click += new System.EventHandler(this.button_del_MAC_Click);
            // 
            // button_add_MAC
            // 
            this.button_add_MAC.Enabled = false;
            this.button_add_MAC.Location = new System.Drawing.Point(91, 114);
            this.button_add_MAC.Name = "button_add_MAC";
            this.button_add_MAC.Size = new System.Drawing.Size(35, 21);
            this.button_add_MAC.TabIndex = 20;
            this.button_add_MAC.Text = "Add";
            this.button_add_MAC.UseVisualStyleBackColor = true;
            this.button_add_MAC.Click += new System.EventHandler(this.button_add_MAC_Click);
            // 
            // textBox_add_MAC
            // 
            this.textBox_add_MAC.AcceptsReturn = true;
            this.textBox_add_MAC.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBox_add_MAC.Location = new System.Drawing.Point(6, 114);
            this.textBox_add_MAC.MaxLength = 17;
            this.textBox_add_MAC.Name = "textBox_add_MAC";
            this.textBox_add_MAC.Size = new System.Drawing.Size(79, 20);
            this.textBox_add_MAC.TabIndex = 19;
            this.toolTip1.SetToolTip(this.textBox_add_MAC, "enter a MAC address in to format \"AABBCCDDEEFF\"");
            this.textBox_add_MAC.TextChanged += new System.EventHandler(this.textBox_add_MAC_TextChanged);
            // 
            // listBox_MAC_list
            // 
            this.listBox_MAC_list.FormattingEnabled = true;
            this.listBox_MAC_list.Location = new System.Drawing.Point(6, 65);
            this.listBox_MAC_list.Name = "listBox_MAC_list";
            this.listBox_MAC_list.Size = new System.Drawing.Size(156, 43);
            this.listBox_MAC_list.TabIndex = 17;
            this.listBox_MAC_list.SelectedIndexChanged += new System.EventHandler(this.listBox_MAC_list_SelectedIndexChanged);
            // 
            // tabPage_about
            // 
            this.tabPage_about.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_about.Controls.Add(this.richTextBox_about);
            this.tabPage_about.Location = new System.Drawing.Point(4, 22);
            this.tabPage_about.Name = "tabPage_about";
            this.tabPage_about.Size = new System.Drawing.Size(348, 404);
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
            this.richTextBox_about.Size = new System.Drawing.Size(345, 401);
            this.richTextBox_about.TabIndex = 0;
            this.richTextBox_about.Text = "";
            // 
            // timer_messages
            // 
            this.timer_messages.Tick += new System.EventHandler(this.timer_messages_Tick);
            // 
            // comboBox_RemoteHost
            // 
            this.comboBox_RemoteHost.FormattingEnabled = true;
            this.comboBox_RemoteHost.Location = new System.Drawing.Point(86, 39);
            this.comboBox_RemoteHost.Name = "comboBox_RemoteHost";
            this.comboBox_RemoteHost.Size = new System.Drawing.Size(233, 21);
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
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 521);
            this.Controls.Add(this.comboBox_RemoteHost);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button_announce);
            this.Controls.Add(this.textBox_remote_port);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_start_engine);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label5);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(390, 5000);
            this.MinimumSize = new System.Drawing.Size(390, 220);
            this.Name = "FormMain";
            this.Text = "XBSlink";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage_info.ResumeLayout(false);
            this.tabPage_info.PerformLayout();
            this.tabPage_clouds.ResumeLayout(false);
            this.tabPage_clouds.PerformLayout();
            this.tabPage_chat.ResumeLayout(false);
            this.tabPage_chat.PerformLayout();
            this.tabPage_messages.ResumeLayout(false);
            this.tabPage_settings.ResumeLayout(false);
            this.tabPage_settings.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage_about.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
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
        private System.Windows.Forms.CheckBox checkBox_all_broadcasts;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage_info;
        private System.Windows.Forms.TabPage tabPage_settings;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox1;
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
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_stunServerPort;
        private System.Windows.Forms.TextBox textBox_stunServerHostname;
        private System.Windows.Forms.CheckBox checkBox_useStunServer;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Timer timer_startEngine;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox_chatAutoSwitch;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox_chatNickname;
        private System.Windows.Forms.TabPage tabPage_chat;
        private System.Windows.Forms.TextBox textBox_chatEntry;
        private System.Windows.Forms.Button button_clearChat;
        private System.Windows.Forms.TextBox textBox_chatMessages;
        private System.Windows.Forms.TabPage tabPage_clouds;
        private System.Windows.Forms.ListView listView_clouds;
        private System.Windows.Forms.ColumnHeader columnHeader_name;
        private System.Windows.Forms.ColumnHeader columnHeader_nodecount;
        private System.Windows.Forms.ColumnHeader columnHeader_maxnodes;
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
        private System.Windows.Forms.ColumnHeader columnHeader_ping;
        private System.Windows.Forms.ColumnHeader columnHeader_Version;
        private System.Windows.Forms.ColumnHeader columnHeader_nickname;
        private System.Windows.Forms.Label label15;
    }
}

