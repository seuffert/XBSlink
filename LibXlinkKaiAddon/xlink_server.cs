using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class xlink_server
{

    #region Handlers


    public xlink_server_console_process.eXlinkPhase _actual_phase { get { return xLinkMsgProcess.actual_phase; } }

    public delegate void XlinkDebugMessageHandler(string message_debug, xlink_msg.xbs_message_sender sender);
    public event XlinkDebugMessageHandler XlinkDebugMessage;

    //CONSOLE HANDLERS
    public delegate void XlinkConsoleSendMessageHandler(string message, IPAddress console_ip_address, int console_port);
    public event XlinkConsoleSendMessageHandler XlinkConsoleSendMessage;

    public delegate void XlinkConsoleJoinCloudHandler(string CloudName, string CloudPassword);
    public event XlinkConsoleJoinCloudHandler XlinkConsoleJoinCloud;

    public delegate void XlinkConsoleLogoutHandler();
    public event XlinkConsoleLogoutHandler XlinkConsoleLogout;

    public delegate void XlinkConsoleLoginHandler();
    public event XlinkConsoleLoginHandler XlinkConsoleLogin;

    public delegate void XlinkChatHandler(string message);
    public event XlinkChatHandler XlinkConsoleChat;

    public delegate void XlinkConsolePMHandler(string UserName, string Message, bool IsReceived);
    public event XlinkConsolePMHandler XlinkConsolePM;

    #endregion

    public string KAI_CLIENT_LOCAL_DEVICE = "00242BECE7A0";
    public string KAI_CLIENT_LOCAL_NAME = "magurin";

    public xlink_server_console_process xLinkMsgProcess;
    public IPAddress _console_ip_address;

    public const int standard_port = 31415;
    public const int standard_kay_port = 34522;

    public int udp_kay_socket_port;
    public EndPoint _local_endpoint;

    // public int udp_kay_socket_port;
    private Socket udp_kay_socket = null;
    private Thread receive_thread = null;

    private Thread sender_thread = null;

    public bool is_exiting = false;

    #region Constructor && INIT

    public xlink_server(string local_ip_address)
    {
        ChangeIPAddresPort(local_ip_address, standard_kay_port);
        ProcessSocket();
        xLinkMsgProcess = new xlink_server_console_process(this);

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

        ProcessDebugMessage(" * initialized CONSOLE udp listener on port " + udp_kay_socket_port, xlink_msg.xbs_message_sender.UDP_LISTENER);
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
            ProcessDebugMessage("Winsock error: " + Convert.ToString(System.Runtime.InteropServices.Marshal.GetLastWin32Error()), xlink_msg.xbs_message_sender.FATAL_ERROR);
        }

    }



    #endregion

    #region Events

    public void ProcessDebugMessage(string Message, xlink_msg.xbs_message_sender sender)
    {
        if (XlinkDebugMessage != null)
            XlinkDebugMessage(Message, sender);
    }

    public void ConsoleProcessSendMessage(string message, IPAddress console_ip_address, int console_port)
    {
        if (XlinkConsoleSendMessage != null)
            XlinkConsoleSendMessage(message, console_ip_address, console_port);
    }

    public void ConsoleProcessJoinCloud(string CloudName)
    {
        ConsoleProcessJoinCloud(CloudName, "");
    }

    public void ConsoleProcessJoinCloud(string CloudName, string CloudPassword)
    {
        if (XlinkConsoleJoinCloud != null)
            XlinkConsoleJoinCloud(CloudName, CloudPassword);
    }

    public void ConsoleProcessChat(string Message)
    {
        if (XlinkConsoleChat != null)
            XlinkConsoleChat(Message);
    }

    public void ConsoleProcessPM(string Username, string Message)
    {
        //Always send
        if (XlinkConsolePM != null)
            XlinkConsolePM(Username, Message, true);
    }


    public void ProcessLogout()
    {
        if (XlinkConsoleLogout != null)
            XlinkConsoleLogout();
    }

    public void ProcessLogin()
    {
        if (XlinkConsoleLogin != null)
            XlinkConsoleLogin();
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
        ProcessDebugMessage(" * initialized udp listener on port " + udp_kay_socket_port, xlink_msg.xbs_message_sender.UDP_LISTENER);
    }

    public void SendMsgThread(xlink_msg msg)
    {
        if (msg.src_ip != null)
        {
            byte[] bytes = msg.GetData();
            try
            {
                EndPoint ep = (EndPoint)new IPEndPoint(msg.src_ip, msg.src_port);
                udp_kay_socket.SendTo(bytes, bytes.Length, SocketFlags.None, ep);

                if (XlinkDebugMessage != null)
                    XlinkDebugMessage("!! Sending : " + msg.ToString(), xlink_msg.xbs_message_sender.XBOX);
            }
            catch (SocketException sock_ex)
            {
                if (XlinkDebugMessage != null)
                    XlinkDebugMessage("!! ERROR SENDING SOCKET UDP CONSOLE: " + sock_ex.Message, xlink_msg.xbs_message_sender.FATAL_ERROR);
            }
            catch (Exception ex)
            {
                if (XlinkDebugMessage != null)
                    XlinkDebugMessage("!! ERROR SENDING SOCKET UDP CONSOLE: " + ex.Message, xlink_msg.xbs_message_sender.FATAL_ERROR);
            }
        }

    }

    public void SendMsgCola(xlink_msg msg)
    {
        lock (sender_msg)
        {
            sender_msg.Add(msg);
        }
    }


    void while_receiver()
    {
        while (!is_exiting)
        {
            udp_receiver();
            // Thread.Sleep(500);
        }

    }

    void while_sender()
    {
        while (!is_exiting)
        {
            udp_sender();
            Thread.Sleep(500);
        }

    }

    List<xlink_msg> sender_msg = new List<xlink_msg>();

    void udp_sender()
    {
        if (sender_msg.Count > 0)
        {
            SendMsgThread(sender_msg[0]);
            sender_msg.Remove(sender_msg[0]);
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
            xlink_msg msg = new xlink_msg();
            msg.msg_type = xlink_msg.getMessageTypeFromUDPPacket(data);
            if (msg.msg_type != xlink_msg.xbs_xlink_message_type.NO_KAY_MSG)
            {
                msg.SetCleanArray(data);
            }

            if (msg != null)
            {
                remote_endpoint = (IPEndPoint)ep;
                msg.src_ip = remote_endpoint.Address;
                msg.src_port = remote_endpoint.Port;
                xLinkMsgProcess.ProcessReceivedMessage(msg);
            }

        }

    }

    #endregion

    #region IN COMMANDS

    public void XBS_SendChat(string user_name, string Text)
    {
        xLinkMsgProcess.SendChatMessage(user_name, Text);
    }

       public void XBS_SendPM(string user_name, string Text)
    {
        xLinkMsgProcess.SendPMMessage(user_name, Text);
    }

    public void XBS_ChannelCreate(string CloudName, int Players, bool isPrivate, int MaxPlayers)
    {
        xLinkMsgProcess.SendCreateCloud(CloudName, Players, isPrivate, MaxPlayers);
    }

    public void XBS_Detach()
    {

        //KAI_CLIENT_DETACH
        xLinkMsgProcess.SendDetach();
    }

        public void XBS_LeaveUser(string username)
     {
        // DeleteUserFromArray(username);

       xLinkMsgProcess.LeaveUserFromVector(username);
     }

        public void XBS_JoinUser(string username)
        {
            xLinkMsgProcess.JoinUserToVector(username);

        }

    #endregion

}

