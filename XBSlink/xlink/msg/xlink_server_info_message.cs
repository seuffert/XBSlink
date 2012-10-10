using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;


namespace XBSlink.XlinkKai
{
    public class xlink_server_info_message:xlink_msg
    {

        public string _server_name;
        public string _server_version;
        public string _state;
        public string _nickname;

        public string _cloud_server_ip;
        public string _cloud_server_port;

        public void Assign(string parameters_msg)
        {
            var commands = parameters_msg.Split(';');
            _server_name = commands[0];
            _state = commands[1];
            _nickname = commands[2];
            _cloud_server_ip = commands[3];
            _cloud_server_port = commands[4];
            _server_version = commands[5];
        }

        public xlink_server_info_message(xlink_msg msg)
            : base(msg)
        {
            Assign(parameters_msg);
        }

        public xlink_server_info_message(string ipAddress, int port, string msgText)
            : base(ipAddress, port, msgText)
        {
            Assign(parameters_msg);
        }

        public xlink_server_info_message(IPAddress ipAddress, int port, string msgText)
            : base(ipAddress, port, msgText)
        {
            Assign(parameters_msg);
        }

        void RefreshContent()
        {
            Data = getUTF8BytesFromString(xlink_client_messages_helper.ServerInfoServer(
                _server_name,
                _state,
                _nickname,
                _cloud_server_ip,
                _cloud_server_port,
                _server_version));
        }

    }
}

