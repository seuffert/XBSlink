using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XBSlink
{
    class CommandLine
    {
        public static xbs_udp_listener udp_listener = null;
        public static xbs_sniffer sniffer = null;
        public static xbs_node_list node_list = null;
        private xbs_natstun natstun = null;

        public CommandLine()
        {
            node_list = new xbs_node_list();
            udp_listener = new xbs_udp_listener();
            natstun = new xbs_natstun();
        }


    }
}
