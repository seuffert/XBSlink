using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace XBSlink.XlinkKai
{
    public class xlink_create_cloud_create_join_message : xlink_msg
    {

        public string _cloudname { get; set; }
        public string _maxusers { get; set; }
        public string _password { get; set; }


        public void Assign(string parameters_msg)
        {
            var commands = parameters_msg.Split(';');
            _cloudname = commands[0];
            _maxusers = commands[1];
            _password = commands[2];
        }

        public xlink_create_cloud_create_join_message(xlink_msg msg)
            : base(msg)
        {
            Assign(parameters_msg);
        }

        public xlink_create_cloud_create_join_message(string ipAddress, int port, string msgText)
            : base(ipAddress, port, msgText)
        {
            Assign(parameters_msg);
        }

        public xlink_create_cloud_create_join_message(IPAddress ipAddress, int port, string msgText)
            : base(ipAddress, port, msgText)
        {
            Assign(parameters_msg);
        }

        void RefreshContent()
        {
            Data = getUTF8BytesFromString(xlink_client_messages_helper.ClientCloudCreateJoin(_cloudname, int.Parse(_maxusers), _password));
        }


    }
}
