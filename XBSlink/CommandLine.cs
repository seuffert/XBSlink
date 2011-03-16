using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;

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

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole(); 

        public xbs_console_app(xbs_settings settings, String[] args)
        {
            if (System.Environment.OSVersion.Platform == PlatformID.Win32NT)
                AllocConsole(); 
            
            xbs_settings = settings;
            cloudlist = new xbs_cloudlist();

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
                return;
            }
            catch (System.FormatException e)
            {
                command_line_parser_error(command_line_option_set, e);
                return;
            }
            if (!cmd_help && !cmd_list_devices && !cmd_list_clouds && option_capture_device == null)
            {
                command_line_parser_error(command_line_option_set, new OptionException("you need to specify a capture device", "capture-device"));
                return;
            }

            output_version_info();
            if (cmd_help)
                ShowHelp(command_line_option_set);
            else if (cmd_list_devices)
                list_Devices(args);
            else if (cmd_list_clouds)
                show_cloudlist();
            else
            {
                node_list = new xbs_node_list();
                natstun = new xbs_natstun();
                
            }

            Console.ReadLine();
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
            LibPcapLiveDeviceList devices = LibPcapLiveDeviceList.Instance;
            foreach (LibPcapLiveDevice dev in devices)
                Console.WriteLine("  " + dev.Name + " (" + dev.Description+ ")");
        }

        private void output_version_info()
        {
            Console.WriteLine( Environment.NewLine+"XBSlink Version " + xbs_settings.xbslink_version);
        }

        private void ShowHelp(OptionSet command_line_option_set)
        {
            Console.WriteLine("Usage: XBSlink.exe [-h] [-l] [-n NICKNAME] [-s CLOUDSERVER] [-c CLOUDNAME] [-u] [-p PORT] [-o IP] -i CAPTURE_DEVICE_NAME");
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
    }
}
