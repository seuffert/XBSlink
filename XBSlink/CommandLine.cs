﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using SharpPcap;
using SharpPcap.LibPcap;
using SharpPcap.WinPcap;
using NDesk.Options;
using System.Runtime.InteropServices;
using XBSlink.Properties;
/* test */
namespace XBSlink
{
    class xbs_console_app
    {
        xbs_settings xbs_settings = null;
        public static xbs_udp_listener udp_listener = null;
        public static xbs_sniffer sniffer = null;
        public static xbs_node_list node_list = null;
        public static xbs_nat NAT = null;
        private xbs_upnp upnp = null;
        public static xbs_cloudlist cloudlist = null;

        private Thread MessageThread = null;
        volatile bool exiting = false;
        private DateTime last_program_update_check = new DateTime(0);
        private DateTime app_start_time = DateTime.Now;

        LibPcapLiveDeviceList capture_devices = null;
        WinPcapDeviceList capture_devices_win = null;
        IPAddress external_ip = null;

        ConsoleColor default_color_text;
        ConsoleColor default_color_background;

        bool cmd_help = false;
        bool cmd_list_devices = false;
        bool cmd_list_clouds = false;
        String option_nickname = null;
        String option_cloudserver = null;
        String option_cloudname = null;
        String option_password = null;
        int option_maxnodes = 10;
        bool option_upnp = false;
        int option_local_port = 0;
        IPAddress option_local_ip = null;
        String option_capture_device = null;
        OptionSet command_line_option_set;
        bool option_check_for_update = false;

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        static extern bool FreeConsole();

        [System.Runtime.InteropServices.DllImport("kernel32", SetLastError = true)]
        static extern bool AttachConsole(uint dwProcessId);

        const uint ATTACH_PARENT_PROCESS = 0x0ffffffff;
        const int ERROR_ACCESS_DENIED = 5;
        private bool windows_console_attached = false;

        public xbs_console_app()
        {
        }

        public void run(xbs_settings settings, String[] args)
        {
            int exit_code = 0;

            if (System.Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                if (!AttachConsole(ATTACH_PARENT_PROCESS) && Marshal.GetLastWin32Error() == ERROR_ACCESS_DENIED)
                {
                    // A console was not allocated, so we need to make one.
                    if (!AllocConsole())
                    {
                        throw new Exception("Console Allocation Failed");
                    }
                }
                windows_console_attached = true;
            }
            Console.CancelKeyPress += delegate { handleCancelKeyPress(); };

            WebRequest.DefaultWebProxy = null;
            default_color_text = Console.ForegroundColor;
            default_color_background = Console.BackgroundColor;
            xbs_chat.notify_on_incoming_message = false;

            command_line_option_set = new OptionSet() {
                { "?|h|help", "show this help message", v => cmd_help = v!=null },
                { "l|list-devices", "list all available network packet capture devices", v => cmd_list_devices = v != null },
                { "d|update-check", "check online for program update", v => option_check_for_update = v!=null },
                { "n|nickname=", "set the nickname", v => option_nickname=v },
                { "s|cloudserver=", "set cloudserver URL", v => option_cloudserver=v },
                { "j|list-clouds", "list available clouds on cloudserver", v => cmd_list_clouds = v!=null },
                { "c|cloudname=", "connect to this cloud", v => option_cloudname=v },
                { "m|max-nodes=", "maximum clients in cloud. default is 10", (UInt16 v) => option_maxnodes=v },
                { "w|password=", "set password for cloud", v => option_password=v },
                { "u|upnp", "use UPnP to forward incoming port", v => option_upnp = v!=null },
                { "p|port=", "set the incoming port number. default is 31415", (UInt16 v) => option_local_port = v },
                { "o|source-ip=", "bind to this local ip address.", (String v) => option_local_ip=IPAddress.Parse(v) },
                { "i|capture-device=", "name of network device for capturing packets", v => option_capture_device = v },
            };

            xbs_settings = settings;
            cloudlist = new xbs_cloudlist();

            exit_code = parse_command_line(args);
            close_app(exit_code);
        }

        private void close_app( int exit_code )
        {
            if (cloudlist != null)
                if (cloudlist.part_of_cloud)
                    cloudlist.LeaveCloud();
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
                if (upnp.isUPnPavailable())
                    upnp.upnp_deleteAllPortMappings();

            stop_threads();
#if DEBUG
            xbs_messages.addDebugMessage("exiting program.", xbs_message_sender.GENERAL);
#endif
            output_queued_messages();
#if DEBUG
            Console.ReadLine();
#endif
            exit_code = 0;
            if (windows_console_attached)
                FreeConsole();
            if (System.Windows.Forms.Application.MessageLoop)
                System.Windows.Forms.Application.Exit();
            else
                System.Environment.Exit(exit_code);
        }

        private int parse_command_line(String[] args)
        {
            try
            {
                command_line_option_set.Parse(args);
            }
            catch (OptionException e)
            {
                command_line_parser_error(command_line_option_set, e);
                return(-3);
            }
            catch (System.FormatException e)
            {
                command_line_parser_error(command_line_option_set, e);
                return(-4);
            }
            if (!cmd_help && !cmd_list_devices && !cmd_list_clouds)
            {
                if (option_capture_device == null)
                {
                    command_line_parser_error(command_line_option_set, new OptionException("you need to specify a capture device", "capture-device"));
                    return(-5);
                }
                else if (option_local_ip == null)
                {
                    command_line_parser_error(command_line_option_set, new OptionException("you need to specify a local ip addres to bind to", "local source ip address"));
                    return(-5);
                }
                else if (option_nickname == null)
                {
                    command_line_parser_error(command_line_option_set, new OptionException("you need to specify a nickname", "XBSlink nickname"));
                    return(-5);
                }
            }

            output_version_info();
            start_message_thread();

            if (cmd_help)
                ShowHelp();
            else if (cmd_list_clouds)
                show_cloudlist();
            else if (cmd_list_devices)
            {
                initCaptureDeviceList();
                list_Devices(args);
            }
            else
            {
                initCaptureDeviceList();
                start_engine();
            }
            return 0;
        }

        private void start_engine()
        {
            if (option_upnp)
                discover_upnp();
            LibPcapLiveDevice pdev = loadCaptureDevice(option_capture_device);
            if (pdev == null)
            {
                xbs_messages.addInfoMessage("!! ERROR - could not load capture device with name \"" + option_capture_device + "\"", xbs_message_sender.GENERAL, xbs_message_type.FATAL_ERROR);
                close_app(-2);
            }

            NAT = new xbs_nat();
            node_list = new xbs_node_list();
            node_list.notify_on_new_node = false;
            if (option_local_port == 0)
                option_local_port = xbs_udp_listener.standard_port;
            GatewayIPAddressInformationCollection local_gateways = xbs_console_app.getGatewaysForBindIP(option_local_ip);
            try
            {
                udp_listener = new xbs_udp_listener(option_local_ip, option_local_port, node_list);
            }
            catch (Exception e)
            {
                xbs_messages.addInfoMessage("!! ERROR opening UDP port " + option_local_port, xbs_message_sender.GENERAL, xbs_message_type.FATAL_ERROR);
                xbs_messages.addInfoMessage(e.Message, xbs_message_sender.GENERAL);
                close_app(-7);
            }

            try
            {
                if (option_upnp && upnp.isUPnPavailable())
                {
                    external_ip = upnp.upnp_getPublicIP();
                    upnp.upnp_create_mapping(Mono.Nat.Protocol.Udp, udp_listener.udp_socket_port, udp_listener.udp_socket_port);
                }
            }
            catch (Exception)
            {
                xbs_messages.addInfoMessage("!! UPnP port mapping failed", xbs_message_sender.GENERAL, xbs_message_type.ERROR);
            }
            if (external_ip == null)
                external_ip = xbs_upnp.getExternalIPAddressFromWebsite();                        
            IPAddress local_node_ip = (external_ip == null) ? option_local_ip : external_ip;
            node_list.local_node = new xbs_node(local_node_ip, udp_listener.udp_socket_port);
            if (option_nickname!=null)
                node_list.local_node.nickname = option_nickname;

            sniffer = new xbs_sniffer(pdev, false, null, false, node_list, NAT, local_gateways, true);
            sniffer.start_capture();

            if (ExceptionMessage.ABORTING)
                close_app(-10);

            if (option_cloudserver == null)
                option_cloudserver = xbs_cloudlist.DEFAULT_CLOUDLIST_SERVER;

            if (option_cloudname != null)
            {
                if (option_password == null)
                    option_password = "";
                if (option_cloudname.Length >= xbs_cloudlist.MIN_CLOUDNAME_LENGTH)
                {
                    try
                    {
                        cloudlist.JoinOrCreateCloud(option_cloudserver, option_cloudname, option_maxnodes.ToString(), option_password, node_list.local_node.ip_public, node_list.local_node.port_public, node_list.local_node.nickname, xbs_upnp.isPortReachable, xbs_settings.xbslink_version);
                    }
                    catch (Exception e)
                    {
                        xbs_messages.addInfoMessage("!! ERROR connecting to cloud " + option_cloudname, xbs_message_sender.GENERAL, xbs_message_type.ERROR);
                        xbs_messages.addInfoMessage(e.Message, xbs_message_sender.GENERAL, xbs_message_type.ERROR);
                    }
                }
                else
                {
                    xbs_messages.addInfoMessage("!! ERROR - cloudname is too short. " + xbs_cloudlist.MIN_CLOUDNAME_LENGTH + " chars minimum", xbs_message_sender.GENERAL, xbs_message_type.ERROR);
                    close_app(-12);
                }
            }

            main_engine_loop();
        }

        private void main_engine_loop()
        {
            ConsoleKeyInfo keyinfo;

            while (!exiting)
            {
                Thread.Sleep(300);
                if (Console.KeyAvailable)
                {
                    keyinfo = Console.ReadKey(true);
                    switch (keyinfo.Key)
                    {
                        case ConsoleKey.Q:
                            exiting = true;
                            break;
                        case ConsoleKey.L:
                            show_cloudlist();
                            break;
                        case ConsoleKey.U:
                            showConnectedNodes();
                            break;
                        case ConsoleKey.H:
                            ShowHelp();
                            break;
                    }
                }

                if ((DateTime.Now - app_start_time).TotalSeconds >= 5)
                    if (option_check_for_update && ((DateTime.Now - last_program_update_check).TotalHours >= xbs_settings.PROGRAM_UPDATE_CHECK_HOURS_INTERVAL))
                        checkForProgramUpdates();
            }
            close_app(0);
        }

        private void command_line_parser_error(OptionSet command_line_option_set, Exception e)
        {
            WriteLine("!! Error parsing command line:");
            WriteLine("!! " + e.Message + Environment.NewLine);
            ShowHelp();
        }

        private void initialize()
        {
            node_list = new xbs_node_list();
            udp_listener = new xbs_udp_listener(node_list);
            upnp = new xbs_upnp();
        }

        private void list_Devices(String[] args)
        {
			WriteLine("List of available devices:");
            if (System.Environment.OSVersion.Platform == PlatformID.Win32NT)
                foreach (LibPcapLiveDevice dev in capture_devices_win)
                    WriteLine("  " + dev.Name + " (" + dev.Description + ")");
            else
                foreach (LibPcapLiveDevice dev in capture_devices)
                    WriteLine("  " + dev.Name + " (" + dev.Description+ ")");
        }

        private void output_version_info()
        {
            Console.WriteLine( Environment.NewLine+"XBSlink Version " + xbs_settings.xbslink_version);
        }

        private void ShowHelp()
        {
            Console.WriteLine("Usage: XBSlink.exe [-h] [-l] [-s CLOUDSERVER] [-c CLOUDNAME] [-u] [-p PORT] -n NICKNAME -o IP -i CAPTURE_DEVICE_NAME");
            command_line_option_set.WriteOptionDescriptions(Console.Out);
        }

        private void show_cloudlist()
        {
            bool ret = cloudlist.loadCloudlistFromURL( xbs_cloudlist.DEFAULT_CLOUDLIST_SERVER );
            xbs_cloud[] clouds = cloudlist.getCloudlistArray();
            int count=0;
            WriteLine("Available clouds on cloudlist server:");
            foreach (xbs_cloud cloud in clouds)
            {
                count++;
                WriteLine(" " + count + ") " + cloud.name + " (" + cloud.node_count + "/" + cloud.max_nodes + ") " + (cloud.isPrivate ? " (password)" : ""));
            }
        }

        private void start_message_thread()
        {
            MessageThread = new Thread(new ThreadStart(message_thread_start));
            MessageThread.IsBackground = true;
            MessageThread.Priority = ThreadPriority.Normal;
            MessageThread.Start();
        }

        private void message_thread_start()
        {
            xbs_messages.addDebugMessage(" * message dispatcher thread starting...", xbs_message_sender.COMMANDLINE_MESSAGE_DISCPATCHER);
#if !DEBUG
            try
            {
#endif
            while (!exiting)
            {
                output_queued_messages();
                Thread.Sleep(300);
            }
#if !DEBUG
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("udp dispatcher service excpetion", ex);
            }
#endif
        }

        private void output_queued_messages()
        {
            String str;
            while (xbs_messages.getInfoMessageCount() > 0)
            {
                str = xbs_messages.DequeueInfoMessageString();
                if (str.StartsWith("!!"))
                    WriteError("I: " + str);
                else
                    WriteLine("I: " + str);
            }
            while (xbs_messages.getChatMessageCount() > 0)
                WriteChat("C: " + xbs_messages.DequeueChatMessageString().TrimEnd());
#if DEBUG
            while (xbs_messages.getDebugMessageCount() > 0)
                WriteDebug("D: " + xbs_messages.DequeueDebugMessageString());
#endif
        }

        private void stop_threads()
        {
            exiting = true;
            if (MessageThread!=null)
                if (MessageThread.ThreadState != ThreadState.Stopped)
                    MessageThread.Join();
        }

        private bool loadCaptureDeviceList()
        {
            try
            {
                capture_devices = LibPcapLiveDeviceList.Instance;
                    if (System.Environment.OSVersion.Platform == PlatformID.Win32NT)
                        capture_devices_win = WinPcapDeviceList.Instance;
            }
            catch (Exception)
            {
                xbs_messages.addInfoMessage("!! ERROR while getting Pcap capture device list.", xbs_message_sender.GENERAL, xbs_message_type.FATAL_ERROR);
            }
            return (capture_devices.Count > 0);
        }

        private void discover_upnp()
        {
            upnp = new xbs_upnp();
            upnp.upnp_startDiscovery();
            int count = 0;
            while (!upnp.isUPnPavailable() && count < (80))
            {
                Thread.Sleep(250);
                count++;
            }
            if (upnp.isUPnPavailable())
                external_ip = upnp.upnp_getPublicIP();
        }

        private void handleCancelKeyPress()
        {
            xbs_messages.addInfoMessage("!! cancel key pressed. closing threads.", xbs_message_sender.GENERAL, xbs_message_type.WARNING);
            close_app(0);
        }

        private LibPcapLiveDevice loadCaptureDevice(String capture_device_name)
        {
            LibPcapLiveDevice pdev = null;
            if (System.Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                foreach (WinPcapDevice dev in capture_devices_win)
                    if (dev.Name == capture_device_name)
                        pdev = dev;
            }
            else
                foreach (LibPcapLiveDevice dev in capture_devices)
                    if (dev.Name == capture_device_name)
                        pdev = dev;
            return pdev;
        }

        private void WriteError(String text)
        {
            WriteLine(text, ConsoleColor.Black, ConsoleColor.Red);
        }
        private void WriteChat(String text)
        {
            WriteLine(text, ConsoleColor.Green);
        }
        private void WriteDebug(String text)
        {
            WriteLine(text, ConsoleColor.DarkYellow);
        }

        private void WriteLine(String text)
        {
            WriteLine(text, default_color_text, default_color_background);
        }
        private void WriteLine(String text, ConsoleColor color_text)
        {
            WriteLine(text, color_text, default_color_background);
        }
        private void WriteLine(String text, ConsoleColor color_text, ConsoleColor color_background)
        {
            Console.ForegroundColor = color_text;
            Console.BackgroundColor = color_background;
            Console.WriteLine(text);
        }

        private void showConnectedNodes()
        {
            List<xbs_node> nodes = node_list.getXBSNodeListCopy();
            WriteLine("Connected nodes: "+nodes.Count);
            int count = 0;
            String str;
            foreach (xbs_node node in nodes)
            {
                count++;
                String ping = (node.last_ping_delay_ms >= 0) ? node.last_ping_delay_ms + "ms" : "N/A";
                int port = (node.port_sendfrom == node.port_public) ? node.port_public : node.port_sendfrom;
                str = " " + count + ") \"" + node.nickname.PadRight(13) + "\" " + node.ip_public.ToString().PadLeft(15) + "/" + port.ToString().PadRight(5) + " Ping:" + ping.PadLeft(6) + " v" + node.client_version;
                if (node.get_xbox_count() > 0)
                    WriteLine(str, ConsoleColor.Yellow);
                else
                    WriteLine(str, ConsoleColor.White);
            }
        }

        private void initCaptureDeviceList()
        {
            if (!loadCaptureDeviceList())
            {
                String msg;
                if (System.Environment.OSVersion.Platform == PlatformID.Unix)
                    msg = Resources.message_no_capture_devices_unix;
                else if (System.Environment.OSVersion.Platform == PlatformID.Win32NT)
                    msg = Resources.message_no_capture_devices_startNPF;
                else
                    msg = Resources.message_no_capture_devices;
                xbs_messages.addInfoMessage("!! ERROR: " + msg, xbs_message_sender.GENERAL, xbs_message_type.FATAL_ERROR);
                close_app(-1);
            }
        }

        private void checkForProgramUpdates()
        {
            last_program_update_check = DateTime.Now;
            String result = xbs_settings.getOnlineProgramVersion();
            if (result != null)
            {
                int new_version_found = result.CompareTo(xbs_settings.xbslink_version);
                if (new_version_found > 0)
                    xbs_messages.addInfoMessage("A new version of XBSlink is available! (v" + result + ")", xbs_message_sender.GENERAL);
                else if (new_version_found < 0)
                    xbs_messages.addInfoMessage("Latest XBSlink version found: v" + result, xbs_message_sender.GENERAL);
                else
                    xbs_messages.addInfoMessage("You are using the latest XBSlink version.", xbs_message_sender.GENERAL);
            }
        }

        public static GatewayIPAddressInformationCollection getGatewaysForBindIP(IPAddress ip)
        {
            NetworkInterface[] network_interfaces = NetworkInterface.GetAllNetworkInterfaces();
            IPInterfaceProperties ip_properties;
            foreach (NetworkInterface ni in network_interfaces)
            {
                ip_properties = ni.GetIPProperties();
                foreach (UnicastIPAddressInformation uniCast in ip_properties.UnicastAddresses)
                {
                    if (uniCast.Address.AddressFamily==AddressFamily.InterNetwork && uniCast.Address.Equals(ip))
                    {
                        return ip_properties.GatewayAddresses;
                    }
                }
            }
            return null;
        }

    }
}
