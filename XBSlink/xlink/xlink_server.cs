using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using XBSlink.Common;
using XBSlink.XlinkKai.Delegates.Clients;
using XBSlink.XlinkKai.Delegates.Consoles;




namespace XBSlink.XlinkKai
{

public class xlink_server //:IServer
{

    #region Xlink Events
    
    public event XlinkDebugMessageHandler XlinkDebugMessage;
    public event XlinkConsoleSendMessageHandler XlinkConsoleSendMessage;
    public event XlinkConsoleJoinCloudHandler XlinkConsoleJoinCloud;
    public event XlinkConsoleLogoutHandler XlinkConsoleLogout;
    public event XlinkConsoleLoginHandler XlinkConsoleLogin;
    public event XlinkChatHandler XlinkConsoleChat;
    public event XlinkConsolePMHandler XlinkConsolePM;

    #endregion


    public string _KAI_CLIENT_LOCAL_DEVICE = "00242BECE7A0";

    public string KAI_CLIENT_LOCAL_DEVICE { get {

        if (last_logged_console != null)
            return last_logged_console.src_ip.ToString();
        else
            return "00242BECE7A0";

    }
        set { _KAI_CLIENT_LOCAL_DEVICE = value; }
    }

    public string KAI_CLIENT_LOCAL_NAME = "magurin";
    public string KAI_SERVER_NAME = "MAGU HOME";
    public string KAI_SERVER_VERSION = "0.1b";
    

    //public IServerConsoleProcess xLinkMsgProcess { get; set; }
    public xlink_server_console_process xlink_process { get; set; }
   
    public IPAddress _console_ip_address;

    public const int standard_port = 31415;
    public const int standard_kay_port = 34522;
    
    public int udp_kay_socket_port;
    public EndPoint _local_endpoint;

    //List<xlink_msg> xClients;
    //List<xlink_msg> xConsoles;

    public xlink_msg last_logged_console;

    public bool IsConsoleLogged { get { return (last_logged_console != null); } }

    List<xlink_msg> sender_msg = new List<xlink_msg>();

    // public int udp_kay_socket_port;
    private Socket udp_kay_socket = null;
    private Thread receive_thread = null;

    private Thread sender_thread = null;

    //UdpServer m_pUdpServer;

    public bool is_exiting = false;

    #region Constructor && INIT

    public xlink_server()
    {
        xlink_process = new xlink_server_console_process(this);
    }

    public bool IsInArray(xlink_msg element, List<xlink_msg> elementArray)
    {
        foreach (var item in elementArray)
        {
            if (element.src_ip.ToString() == item.src_ip.ToString())
                return true;
        }
        return false;
    }

 
    public void Configure(string local_ip_address)
    {
        ChangeIPAddresPort(local_ip_address, standard_kay_port);
        ProcessSocket();
    }

    void while_receiver()
    {
        while (!is_exiting)
        {
            udp_receiver();
            // Thread.Sleep(500);
        }

    }


    void udp_receiver()
    {

        byte[] data = new byte[2048];
        IPEndPoint remote_endpoint = new IPEndPoint(IPAddress.Any, 0);
        EndPoint ep = (EndPoint)remote_endpoint;

        int bytes_received = 0;
        try
        {
            bytes_received = udp_kay_socket.ReceiveFrom(data, ref ep);
        }
        catch (Exception ex)
        {
            bytes_received = 0;
        }

        if (!is_exiting && bytes_received > 0)
        {

            xlink_msg msg = new xlink_msg(data);
            if (msg != null)
            {
                msg.src_ip = (ep as IPEndPoint).Address;
                msg.src_port = (ep as IPEndPoint).Port;

                xlink_process.ProcessReceivedMessage(msg);
            }
        }
    }


    public void Start()
    {

        is_exiting = false;

        receive_thread = new Thread(new ThreadStart(while_receiver));
        receive_thread.IsBackground = true;
        receive_thread.Priority = ThreadPriority.Normal;
        receive_thread.Start();

        sender_thread = new Thread(new ThreadStart(while_sender));
        sender_thread.IsBackground = true;
        sender_thread.Priority = ThreadPriority.Normal;
        sender_thread.Start();

        System.Console.WriteLine(" * initialized CONSOLE udp listener on port " + udp_kay_socket_port, xbs_message_sender.UDP_LISTENER);
        ProcessDebugMessage(null, " * initialized CONSOLE udp listener on port " + udp_kay_socket_port, xbs_message_sender.UDP_LISTENER);
    }

    public void Stop()
    {
        is_exiting = true;
        if (receive_thread.IsAlive)
            receive_thread.Abort();
        receive_thread = null;

        if (sender_thread.IsAlive)
            sender_thread.Abort();
        sender_thread = null;

    }

    public void Shutdown()
    {
        is_exiting = true;
        Stop();

        udp_kay_socket.Shutdown(SocketShutdown.Both);
        udp_kay_socket.Close();

        if (udp_kay_socket.Connected)
        {
            ProcessDebugMessage( null, "Winsock error: " + Convert.ToString(System.Runtime.InteropServices.Marshal.GetLastWin32Error()),  xbs_message_sender.X360);
        }
    }

    #endregion

    #region OUT TO ENGINE EVENTS

    public void ProcessDebugMessage(xlink_msg udp_msg,  string Message, xbs_message_sender sender)
    {
        if (XlinkDebugMessage != null)
            XlinkDebugMessage(udp_msg,Message, sender);
    }

    public void ConsoleProcessSendMessage(xlink_msg udp_msg, string message)
    {
        if (XlinkConsoleSendMessage != null)
            XlinkConsoleSendMessage(udp_msg,message);
    }

    public void ConsoleProcessJoinCloud(xlink_msg udp_msg,string CloudName)
    {
        ConsoleProcessJoinCloud(udp_msg,CloudName, "");
    }

    public void ConsoleProcessJoinCloud(xlink_msg udp_msg,string CloudName, string CloudPassword)
    {
        //XB

        if (XlinkConsoleJoinCloud != null)
            XlinkConsoleJoinCloud(udp_msg,CloudName, CloudPassword);
    }

    public void ConsoleProcessChat(xlink_msg udp_msg,string Message)
    {
        if (XlinkConsoleChat != null)
            XlinkConsoleChat(udp_msg,Message);
    }

    public void ConsoleProcessPM(xlink_msg udp_msg,string Username, string Message)
    {
        //Always send
        if (XlinkConsolePM != null)
            XlinkConsolePM(Username, Message, true);
    }

     public void ProcessLogout(xlink_msg udp_msg)
    {
        if (XlinkConsoleLogout != null)
            XlinkConsoleLogout(udp_msg);
    }

    public void ProcessLogin(xlink_msg udp_msg)
    {
        if (XlinkConsoleLogin != null)
            XlinkConsoleLogin(udp_msg);
    }

    #endregion

    #region Socket

    public void ChangeIPAddresPort(string console_ip_address, int console_port)
    {
        _console_ip_address = IPAddress.Parse(console_ip_address);
        udp_kay_socket_port = console_port;
    }

    void ProcessSocket()
    {

        _local_endpoint = new IPEndPoint(_console_ip_address, udp_kay_socket_port);

        udp_kay_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        udp_kay_socket.EnableBroadcast = true;
        udp_kay_socket.MulticastLoopback = true;
        udp_kay_socket.ReceiveTimeout = 1000;

        try
        {
            udp_kay_socket.Bind(new IPEndPoint(IPAddress.Any, udp_kay_socket_port));
        }
        catch (SocketException)
        {
            throw new Exception("an error occured while initializing the UDP KAY socket.\r\nPlease see the messages tab.");
        }

        ProcessDebugMessage(null," * initialized udp listener on port " + udp_kay_socket_port, xbs_message_sender.UDP_LISTENER);
    }


    public void ProcessMessageQueue(xlink_msg msg)
    {
        if (msg != null)
        {
            if (msg.src_ip != null)
            {
                try
                {
                    EndPoint ep = (EndPoint)new IPEndPoint(msg.src_ip, msg.src_port);
                    udp_kay_socket.SendTo(msg.Data, msg.Data.Length, SocketFlags.None, ep);

                    if (XlinkDebugMessage != null)
                        XlinkDebugMessage(msg,"!! Sending : " + msg.ToString(), xbs_message_sender.X360);
                }
                catch (SocketException sock_ex)
                {
                    if (XlinkDebugMessage != null)
                        XlinkDebugMessage(msg, "!! ERROR SENDING SOCKET UDP CONSOLE: " + sock_ex.Message, xbs_message_sender.X360);
                }
                catch (Exception ex)
                {
                    if (XlinkDebugMessage != null)
                        XlinkDebugMessage(msg, "!! ERROR SENDING SOCKET UDP CONSOLE: " + ex.Message, xbs_message_sender.X360);
                }
            }
        }
    }

    public void SendMessageToQueue(xlink_msg udp_msg, string[] msg)
    {
        foreach (var item in msg)
            SendMessageToQueue(udp_msg, item);
    }

    public void SendMessageToQueue(string[] msg)
    {
        foreach (var item in msg)
            SendMessageToQueue(null, item);
    }


    public void SendMessageToQueue( string msg)
    {
        SendMessageToQueue(null, msg);
    }
    
    /// <summary>
    /// If udp_msg is null there are a system msg
    /// </summary>
    /// <param name="udp_msg"></param>
    /// <param name="msg"></param>
    /// <param name="SendToAll"></param>
    public void SendMessageToQueue(xlink_msg udp_msg, string msg)
    {
    
        if (udp_msg == null)
            udp_msg = last_logged_console;

        if (udp_msg != null)
        {
            xbs_messages.addInfoMessage(String.Format("({1}:{2}) S > {0}", msg, udp_msg.src_ip.ToString(), udp_msg.src_port.ToString()), xbs_message_sender.X360);
            xlink_msg temp_msg = new xlink_msg(udp_msg, msg);
            lock (sender_msg)
                sender_msg.Add(temp_msg);
        }
        else
            xbs_messages.addInfoMessage(String.Format("NOT CONSOLE LOGGED. IGNORED: {0}",msg), xbs_message_sender.X360);
    }

 
    void while_sender()
    {
        while (!is_exiting)
        {
            if (sender_msg.Count > 0)
            {
                ProcessMessageQueue(sender_msg[0]);
                sender_msg.Remove(sender_msg[0]);
            }
            Thread.Sleep(400);
        }

    }

    #endregion

    #region USER SENDING COMMANDS

    public void XBS_SendMyChat(xlink_msg udp_msg, string message)
    {
        SendMessageToQueue(udp_msg, xlink_client_messages_helper.KAI_CLIENT_CHAT(KAI_CLIENT_LOCAL_NAME , message)); //NO ESTÁ CLARO
    }

    /// <summary>
    /// MESSAGE FROM ENGINE (USER)
    /// </summary>
    /// <param name="udp_msg"></param>
    /// <param name="user_name"></param>
    /// <param name="Text"></param>
    public void XBS_SendUserChat(string user_name, string Text)
    {
        SendMessageToQueue(xlink_client_messages_helper.KAI_CLIENT_CHAT(user_name, Text));
    }

    public void XBS_SendPM(xlink_msg udp_msg, string user_name, string Text)
    {
        SendMessageToQueue(udp_msg, xlink_client_messages_helper.KAI_CLIENT_PM(user_name, Text));
    }

    public void XBS_ChannelCreate(xlink_msg udp_msg, string CloudName, int Players, bool isPrivate, int MaxPlayers)
    {
        SendMessageToQueue(udp_msg, xlink_client_messages_helper.KAI_CLIENT_USER_SUB_VECTOR(CloudName, Players, isPrivate, MaxPlayers));
    }

    public void XBS_Detach(xlink_msg udp_msg)
    {
        SendMessageToQueue(udp_msg, xlink_client_messages_helper.KAY_GET_DETACH(KAI_CLIENT_LOCAL_DEVICE));
    }

        public void XBS_LeaveUser(xlink_msg udp_msg,string username)
     {
         SendMessageToQueue(udp_msg, xlink_client_messages_helper.KAY_GET_LEAVE_USER_FROM_VECTOR(username));
     }

        public void XBS_JoinUser(xlink_msg udp_msg,string username, string client_version, string last_ping_delay_ms)
        {
            SendMessageToQueue(udp_msg, xlink_client_messages_helper.KAY_GET_USER_JOIN_TO_VECTOR(username, client_version, last_ping_delay_ms));
        }

    #endregion

   
}

}
