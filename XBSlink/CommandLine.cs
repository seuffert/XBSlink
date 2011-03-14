using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpPcap;
using SharpPcap.LibPcap;
using SharpPcap.WinPcap;
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

        public xbs_console_app(xbs_settings settings, String[] args)
        {
            xbs_settings = settings;

            output_version_info();

            switch (args[0])
            {
                case "--list-devices":
                    list_Devices(args);
                    break;
            }
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

    }
}
