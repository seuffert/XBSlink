using System;
using System.Collections.Generic;
using System.Text;
using Java.Net;
using XBSlink.XlinkKai;

namespace XBSlink.Android
{
    public class AndroidClientEngineProcess 
    {

        string server_ip = "10.67.2.1";
        public int REMOTE_SERVER_PORT = 34522;
        public int LOCAL_CLIENT_PORT = 34523;


         public DatagramSocket s;

        public AndroidClientEngineProcess(AndroidClientEngine ClientEngine)
        {
            s = ClientEngine.s;
        }

        public AndroidClientEngineProcess(DatagramSocket socket, string ServerIp, int ServerPort)
        {
            s = socket;
            server_ip = ServerIp;
            REMOTE_SERVER_PORT = ServerPort;
        }


        public void SendMessage( string message)
        {
            AndroidClientEngine.SendMessage(s, server_ip, REMOTE_SERVER_PORT, message);
        }

        public void ClientCloudCreateJoin(string Cloud, int MaxUsers, string Password)
        {
            SendMessage(xlink_client_messages_helper.ClientCloudCreateJoin(Cloud, MaxUsers, Password));
        }

        public void ClientCloudJoin( string Cloud)
        {
            SendMessage(xlink_client_messages_helper.ClientCloudJoin(Cloud));
        }

          public void ClientCloudGet() {
              SendMessage(xlink_client_messages_helper.ClientCloudGet());
        }

          public void ClientCloudLeave() {
              SendMessage(xlink_client_messages_helper.ClientCloudLeave());
        }

          public void ClientSendChatMessage( string Message)
          {
              SendMessage(xlink_client_messages_helper.ClientSendChatMessage(Message));
            
        }

          public void ClientConnect()
          {
              SendMessage(xlink_client_messages_helper.ClientConnect());
          }

          public void ClientDisconnect()
          {
              SendMessage(xlink_client_messages_helper.ClientDisconnect());
          }

          public void ClientDiscover()
          {
              SendMessage(xlink_client_messages_helper.ClientDiscover());
          }

          public void ClientSendPM( string UserName, string Message)
          {
              SendMessage( xlink_client_messages_helper.ClientSendPM(UserName, Message));
        }

          public void ClientStart() {
              SendMessage( xlink_client_messages_helper.ClientStart());
        }

            public void ClientStop() {
                SendMessage( xlink_client_messages_helper.ClientStop());
        }


    }
}
