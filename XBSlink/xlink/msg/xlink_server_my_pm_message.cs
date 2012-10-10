using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace XBSlink.XlinkKai
{
    public class xlink_server_my_pm_message:xlink_msg
    {

        public string _message { get; set; }
        public string _tousername { get; set; }

        public void Assign(string parameters_msg)
        {
            _message = GetParameters()[0];
        }

        public xlink_server_my_pm_message(xlink_msg msg)
            : base(msg)
        {
            Assign(parameters_msg);
        }

        public xlink_server_my_pm_message(string ipAddress, int port, string msgText)
            : base(ipAddress, port, msgText)
        {
            Assign(parameters_msg);
        }

        public xlink_server_my_pm_message(IPAddress ipAddress, int port, string msgText)
            : base(ipAddress, port, msgText)
        {
            Assign(parameters_msg);
        }

        void RefreshContent()
        {
            Data = getUTF8BytesFromString(xlink_client_messages_helper.ServerMyPMMessage(_tousername, _message));
        }


    }
}
