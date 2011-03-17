using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

using SharpPcap;
using SharpPcap.LibPcap;
using SharpPcap.WinPcap;
using NDesk.Options;

using XBSlink.Properties;

namespace XBSlink
{
    class xbs_console_app
    {
        xbs_settings xbs_settings = null;
        public static xbs_udp_listener udp_listener = null;
        public static xbs_sniffer sniffer = null;
        public static xbs_node_list node_list = null;
        private xbs_natstun natstun = null;
        private xbs_cloudlist cloudlist = null;

        private Thread MessageThread = null;
        volatile bool exiting = false;

        LibPcapLiveDeviceList capture_devices = null;
        WinPcapDeviceList capture_devices_win = null;
        IPAddress external_ip = null;

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole(); 

        public xbs_console_app(xbs_settings settings, String[] args)
        {
            if (System.Environment.OSVersion.Platform == PlatformID.Win32NT)
                AllocConsole();

            if (!loadCaptureDeviceList())
            {
                String msg;
                if (System.Environment.OSVersion.Platform == PlatformID.Unix)
                    msg = Resources.message_no_capture_devices_unix;
                else if (System.Environment.OSVersion.Platform == PlatformID.Win32NT)
                    msg = Resources.message_no_capture_devices_startNPF;
                else
                    msg = Resources.message_no_capture_devices;
                xbs_messages.addInfoMessage("!! ERROR: " + msg);
                close_app(-1);
            }

            xbs_settings = settings;
            cloudlist = new xbs_cloudlist();

            parse_command_line(args);

            close_app(0);
        }

        private void close_app( int exit_code )
        {
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
            if (natstun != null)
                if (natstun.isUPnPavailable())
                    natstun.upnp_deleteAllPortMappings();

            stop_threads();
#if DEBUG
            xbs_messages.addDebugMessage("exiting program.");
#endif
            output_queued_messages();
#if DEBUG
            Console.ReadLine();
#endif
            if (System.Windows.Forms.Application.MessageLoop)
                System.Windows.Forms.Application.Exit();
            else
                System.Environment.Exit(exit_code);
            Environment.Exit(exit_code);
        }

        private void parse_command_line(String[] args)
        {
            Console.CancelKeyPress += delegate { handleCancelKeyPress(); };

            bool cmd_help = false;
            bool cmd_list_devices = false;
            bool cmd_list_clouds = false;
            String option_nickname = null;
            String option_cloudserver = null;
            String option_cloudname = null;
            bool option_upnp = false;
            bool option_advanced_broadcast = false;
            int option_local_port = 0;
            IPAddress option_local_ip = null;
            String option_capture_device = null;
            OptionSet command_line_option_set = new OptionSet() {
                { "?|h|help", "show this help message", v => cmd_help = v!=null },
                { "l|list-devices", "list all available network packet capture devices", v => cmd_list_devices = v != null },
                { "n|nickname=", "set the nickname", v => option_nickname=v },
                { "s|cloudserver=", "set cloudserver URL", v => option_cloudserver=v },
                { "j|list-clouds", "list available clouds on cloudserver", v => cmd_list_clouds = v!=null },
                { "c|cloudname=", "connect to this cloud", v => option_cloudname=v },
                { "u|upnp", "use UPnP to forward incoming port", v => option_upnp = v!=null },
                { "a|advanced-broadcast", "enable advanced forwarding of broadcasts", v => option_advanced_broadcast = v!=null },
                { "p|port=", "set the incoming port number. default is 31415", (UInt16 v) => option_local_port = v },
                { "o|source-ip=", "bind to this local ip address.", (String v) => option_local_ip=IPAddress.Parse(v) },
                { "i|capture-device=", "name of network device for capturing packets", v => option_capture_device = v },
            };
            try
            {
                command_line_option_set.Parse(args);
            }
            catch (OptionException e)
            {
                command_line_parser_error(command_line_option_set, e);
                close_app(-3);
            }
            catch (System.FormatException e)
            {
                command_line_parser_error(command_line_option_set, e);
                close_app(-4);
            }
            if (!cmd_help && !cmd_list_devices && !cmd_list_clouds)
            {
                if (option_capture_device == null)
                {
                    command_line_parser_error(command_line_option_set, new OptionException("you need to specify a capture device", "capture-device"));
                    close_app(-5);
                }
                else if (option_local_ip == null)
                {
                    command_line_parser_error(command_line_option_set, new OptionException("you need to specify a local ip addres to bind to", "local source ip address"));
                    close_app(-5);
                }
                else if (option_nickname == null)
                {
                    command_line_parser_error(command_line_option_set, new OptionException("you need to specify a nickname", "XBSlink nickname"));
                    close_app(-5);
                }
            }

            output_version_info();
            start_message_thread();

            if (cmd_help)
                ShowHelp(command_line_option_set);
            else if (cmd_list_devices)
                list_Devices(args);
            else if (cmd_list_clouds)
                show_cloudlist();
            else
                start_engine(option_upnp, option_capture_device, option_local_port, option_local_ip, option_nickname, option_advanced_broadcast);
        }

        private void start_engine(bool option_upnp, String option_capture_device, int option_local_port, IPAddress option_local_ip, String option_nickname, bool option_advanced_broadcast)
        {
            if (option_upnp)
                discover_upnp();
            LibPcapLiveDevice pdev = loadCaptureDevice(option_capture_device);
            if (pdev == null)
            {
                xbs_messages.addInfoMessage("!! ERROR - could not load capture device with name \"" + option_capture_device + "\"");
                close_app(-2);
            }

            node_list = new xbs_node_list();
            if (option_local_port == 0)
                option_local_port = xbs_udp_listener.standard_port;
            try
            {
                udp_listener = new xbs_udp_listener(option_local_ip, option_local_port);
            }
            catch (Exception e)
            {
                xbs_messages.addInfoMessage("!! ERROR opening UDP port " + option_local_port);
                xbs_messages.addInfoMessage(e.Message);
                close_app(-7);
            }

            try
            {
                if (option_upnp && natstun.isUPnPavailable())
                {
                    external_ip = natstun.upnp_getPublicIP();
                    natstun.upnp_create_mapping(Mono.Nat.Protocol.Udp, udp_listener.udp_socket_port, udp_listener.udp_socket_port);
                }
            }
            catch (Exception)
            {
                xbs_messages.addInfoMessage("!! UPnP port mapping failed");
            }
            if (external_ip == null)
                external_ip = xbs_natstun.getExternalIPAddressFromWebsite();                        
            IPAddress local_node_ip = (external_ip == null) ? option_local_ip : external_ip;
            node_list.local_node = new xbs_node(local_node_ip, udp_listener.udp_socket_port);
            if (option_nickname!=null)
                node_list.local_node.nickname = option_nickname;

            sniffer = new xbs_sniffer(pdev, option_advanced_broadcast, false, false, node_list);
            //setSnifferMacList();
            sniffer.start_capture();

            if (ExceptionMessage.ABORTING)
                close_app(-10);

            main_engine_loop();
        }

        private void main_engine_loop()
        {
            while (true)
            {
                Thread.Sleep(300);
            }
        }

        private void command_line_parser_error(OptionSet command_line_option_set, Exception e)
        {
            Console.WriteLine("!! Error parsing command line:");
            Console.WriteLine("!! " + e.Message + Environment.NewLine);
            ShowHelp(command_line_option_set);
            Console.ReadLine();
        }

        private void initialize()
        {
            node_list = new xbs_node_list();
            udp_listener = new xbs_udp_listener();
            natstun = new xbs_natstun();
        }

        private void list_Devices(String[] args)
        {
			Console.WriteLine("List of available devices:");
            if (System.Environment.OSVersion.Platform == PlatformID.Win32NT)
                foreach (LibPcapLiveDevice dev in capture_devices_win)
                    Console.WriteLine("  " + dev.Name + " (" + dev.Description + ")");
            else
                foreach (LibPcapLiveDevice dev in capture_devices)
                    Console.WriteLine("  " + dev.Name + " (" + dev.Description+ ")");
        }

        private void output_version_info()
        {
            Console.WriteLine( Environment.NewLine+"XBSlink Version " + xbs_settings.xbslink_version);
        }

        private void ShowHelp(OptionSet command_line_option_set)
        {
            Console.WriteLine("Usage: XBSlink.exe [-h] [-l] [-s CLOUDSERVER] [-c CLOUDNAME] [-u] [-p PORT] -n NICKNAME -o IP -i CAPTURE_DEVICE_NAME");
            command_line_option_set.WriteOptionDescriptions(Console.Out);
        }

        private void show_cloudlist()
        {
            bool ret = cloudlist.loadCloudlistFromURL( xbs_cloudlist.DEFAULT_CLOUDLIST_SERVER );
            xbs_cloud[] clouds = cloudlist.getCloudlistArray();
            int count=0;
            Console.WriteLine("Available clouds on cloudlist server:");
            foreach (xbs_cloud cloud in clouds)
            {
                count++;
                Console.WriteLine(" " + count + ") " + cloud.name + " (" + cloud.node_count + "/" + cloud.max_nodes + ") " + (cloud.isPrivate ? " (password)" : ""));
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
            xbs_messages.addDebugMessage(" * message dispatcher thread starting...");
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
            while (xbs_messages.getInfoMessageCount() > 0)
                Console.WriteLine("I: " + xbs_messages.DequeueInfoMessageString());
            while (xbs_messages.getChatMessageCount() > 0)
                Console.WriteLine("C: " + xbs_messages.DequeueChatMessage());
#if DEBUG
            while (xbs_messages.getDebugMessageCount() > 0)
                Console.WriteLine("D: " + xbs_messages.DequeueDebugMessageString());
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
                xbs_messages.addInfoMessage("!! ERRROR getting Pcap capture device list.");
            }
            return (capture_devices.Count > 0);
        }

        private void discover_upnp()
        {
            natstun = new xbs_natstun();
            natstun.upnp_startDiscovery();
            int count = 0;
            while (!natstun.isUPnPavailable() && count < (80))
            {
                Thread.Sleep(250);
                count++;
            }
            if (natstun.isUPnPavailable())
                external_ip = natstun.upnp_getPublicIP();
        }

        private void handleCancelKeyPress()
        {
            xbs_messages.addInfoMessage("!! cancel key pressed. closing threads.");
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


    }
}
