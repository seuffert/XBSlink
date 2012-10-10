using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;


namespace XBSlink.XlinkKai.Delegates.Consoles
{
    public delegate void XlinkDebugMessageHandler(xlink_msg udp_msg, string message_debug, xbs_message_sender sender);
    public delegate void XlinkConsoleSendMessageHandler(xlink_msg udp_msg,string message);
    public delegate void XlinkConsoleJoinCloudHandler(xlink_msg udp_msg,string CloudName, string CloudPassword);
    public delegate void XlinkConsoleLogoutHandler(xlink_msg udp_msg);
    public delegate void XlinkConsoleLoginHandler(xlink_msg udp_msg);
    public delegate void XlinkChatHandler(xlink_msg udp_msg,string message);
    public delegate void XlinkConsolePMHandler(xlink_msg udp_msg,string UserName, string Message, bool IsReceived);
   
}

namespace XBSlink.XlinkKai.Delegates.Clients
{
    public delegate void ClientGetCloudsHandler(xlink_msg udp_msg);
    public delegate void ClientCloudJoinHandler(xlink_msg udp_msg);
    public delegate void ClientCloudCreateJoinHandler(xlink_msg udp_msg);
    public delegate void ClientCloudLeaveHandler(xlink_msg udp_msg);
    public delegate void ClientChatMessageHandler(xlink_msg udp_msg);
    public delegate void ClientSendPMHandler(xlink_msg udp_msg);
    public delegate void ClientStartHandler(xlink_msg udp_msg);
    public delegate void ClientStopHandler(xlink_msg udp_msg);

    public delegate void ClientConnnectHandler(xlink_msg udp_msg);
    public delegate void ClientDisconnectHandler(xlink_msg udp_msg);
    public delegate void ClientDiscoverHandler(xlink_msg udp_msg);


    public delegate void ClientFavoriteAddHandler(xlink_msg udp_msg);
    public delegate void ClientFavoriteDelHandler(xlink_msg udp_msg);

}
