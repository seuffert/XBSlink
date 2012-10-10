using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace XBSlink.XlinkKai
{
    public class xlink_server_create_cloud_message:xlink_msg
    {
        public string _name { get; set; }
        public string _node_count { get; set; }
        public string _max_nodes  { get; set; }
        public bool _isPrivate { get; set; }
      
        public void Assign(string parameters_msg)
        {
            var tmp_params = GetParameters();
            _name = tmp_params[0];
            _node_count = tmp_params[1];
            _max_nodes = tmp_params[2];
            _isPrivate = Convert.ToBoolean(tmp_params[3]);
           
        }

        public xlink_server_create_cloud_message(xlink_msg msg)
            : base(msg)
        {
            Assign(parameters_msg);
        }

        public xlink_server_create_cloud_message(string ipAddress, int port, string msgText)
            : base(ipAddress, port, msgText)
        {
            Assign(parameters_msg);
        }

        public xlink_server_create_cloud_message(IPAddress ipAddress, int port, string msgText)
            : base(ipAddress, port, msgText)
        {
            Assign(parameters_msg);
        }

        void RefreshContent()
        {
            Data = getUTF8BytesFromString(xlink_client_messages_helper.ServerAddCloud(_name, _node_count, _max_nodes, _isPrivate));
        }

    }
}
