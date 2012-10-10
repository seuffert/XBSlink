using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using Java.Net;
using XBSlink.XlinkKai;

namespace XBSlink.Android
{
   public  class AndroidClientEngine
    {

        public const int standard_port = 31415;
        public const int standard_kay_port = 34522;
        public const int standard_kay_client_port = 34523;

        public int udp_kay_socket_port;
        public EndPoint _local_endpoint;

     
        public string KAI_CLIENT_LOCAL_DEVICE = "00242BECE7A0";
        public string KAI_CLIENT_LOCAL_NAME = "magurin";

        public bool is_exiting;
        public IPAddress _xbs_link_ip;

        private Thread sender_thread = null;


        //public delegate void XlinkDebugMessageHandler(string message_debug, xlink_msg.xbs_message_sender sender);
        //public event XlinkDebugMessageHandler XlinkDebugMessage;

        public delegate void ProcessReceivedMessageHandler(xlink_msg msg);
        public event ProcessReceivedMessageHandler ProcessReceivedMessage;


        List<xlink_msg> sender_msg = new List<xlink_msg>();

        public AndroidClientEngine()
        {
            ChangeIPAddresPort( standard_kay_port);
           // InitializeSocket();
        }

        public DatagramSocket s;

        Thread receive_thread;

     
        public void Start()
        {
            is_exiting = false;
            sender_thread = new Thread(new ThreadStart(while_sender));
            sender_thread.IsBackground = true;
            sender_thread.Priority = ThreadPriority.Normal;
            sender_thread.Start();

            try
            {
                s = new DatagramSocket(standard_kay_client_port);
            }
            catch (Exception)
            {
            }
            
            System.Console.WriteLine(" * initialized CONSOLE udp listener on port " + udp_kay_socket_port);
            //ProcessDebugMessage(" * initialized CONSOLE udp listener on port " + udp_kay_socket_port, xlink_msg.xbs_message_sender.UDP_LISTENER);

            receive_thread = new Thread(new ThreadStart(Receive));
            receive_thread.IsBackground = true;
            receive_thread.Priority = ThreadPriority.Normal;
            receive_thread.Start();
              

        }


        void while_sender()
        {
            while (!is_exiting)
            {
                udp_sender();
                Thread.Sleep(400);
            }

        }

        void udp_sender()
        {
            if (sender_msg.Count > 0)
            {
                SendMessage(sender_msg[0]);
                sender_msg.Remove(sender_msg[0]);
            }
        }

        public void SendMessage(xlink_msg msg)
        {
            if (msg.src_ip != null)
            {
                SendMessage(s, msg.src_ip, msg.src_port, msg.Data);
            }

        }



        public static void SendMessage(DatagramSocket s, IPAddress Ip, int port, string msgText)
        {
            var Data = xlink_msg.getUTF8BytesFromString(msgText);
            SendMessage(s, Ip, port, Data);
        }


        public static void SendMessage(DatagramSocket s, string Ip, int port, string msgText)
        {
            SendMessage(s, IPAddress.Parse(Ip), port, msgText);
        }

        public static void SendMessage(DatagramSocket s, IPAddress Ip, int port, byte[] message)
        {
            InetAddress local = InetAddress.GetByName(Ip.ToString());
            //int msg_length = message.Length;
            //byte[] message = StrToByteArray(messageStr);
            DatagramPacket p = new DatagramPacket(message, message.Length, local, port);
            s.Send(p);
        }


        public static void SendMessage(DatagramSocket s, InetAddress Ip, int port, byte[] message)
        {
            DatagramPacket p = new DatagramPacket(message, message.Length, Ip, port);
            s.Send(p);
        }

        int ByteArray = 4096;


        void Receive()
        {

            is_exiting = false;

            string text;

            byte[] message;
            int Port=-1;
            string HostAddress = "";

            DatagramPacket p;

            while (!is_exiting)
            {
                message = new byte[ByteArray];
                p = new DatagramPacket(message, message.Length);

                try
                {
                    s.Receive(p);
                    text = ByteArrayToStr(p.GetData()).Replace("\0", "").Replace("\0", string.Empty); //new String().Trim().ToCharArray(), 0, p.Length);
                    message = StrToByteArray(text);
                    HostAddress = p.Address.HostAddress;
                    Port = p.Port;
                    //s.Close();
                }
                catch (Exception)
                {
                }

                if (message.Length > 0)
                    PacketReceived(HostAddress, Port, message);

            }
            //Log.d("Udp tutorial", "message:" + text);
            //s.Close();

        }


        public void PacketReceived(string IpAdress, int Port, byte[] message)
        {
            xlink_msg msg = new xlink_msg(message);
            if (msg != null)
            {
                //remote_endpoint = (IPEndPoint)ep;
                msg.src_ip = IPAddress.Parse(IpAdress);
                msg.src_port = Port;

                //ProcesamosMSG
                if (ProcessReceivedMessage != null)
                    ProcessReceivedMessage(msg);
            }
        }
       

        public void Close()
        {

        }

        public void ChangeIPAddresPort( int console_port)
        {
           // _xbs_link_ip = IPAddress.Parse(xbs_link_ip);
            udp_kay_socket_port = console_port;
        }

        public void Listen(int port)
        {
          
        }

          public static string ByteArrayToStr(byte[] str)
          {
            //var buf = Encoding.Convert(Encoding.GetEncoding("iso-8859-1"), Encoding.UTF8, str);
            return Encoding.UTF8.GetString(str);
          }

        public static byte[] StrToByteArray(string str)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(str);
        }

      

    }
}
