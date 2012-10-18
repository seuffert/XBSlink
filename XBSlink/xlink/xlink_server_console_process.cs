using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;


namespace XBSlink.XlinkKai
{
    public class xlink_server_console_process //: IServerConsoleProcess
    {
        
        public xlink_server _parent { get; set; }

        public xlink_server_console_process(xlink_server server)
        {
            _parent = server;
        }

        #region PROCESS MESSAGE

        public void ProcessReceivedMessage(xlink_msg udp_msg)
        {
            //Añadimos al listado de consolas MODIFICAR PARA QUE SOLO AÑADA CUANDO LOGUEE
            _parent.last_logged_console = udp_msg;

            xbs_messages.addInfoMessage(String.Format("({1}:{2}) R > {0}",udp_msg.data_msg,udp_msg.src_ip.ToString(),udp_msg.src_port.ToString()), xbs_message_sender.X360);

            //=============================   PRIMER PAQUETE DISCOVER  =====================================
            if (udp_msg.msg_type == xlink_msg.xbs_xlink_message_type.KAI_CLIENT_DISCOVER)
            {
                KaySendMessageActualConsole(udp_msg, "KAI_CLIENT_ENGINE_HERE;");
                //=============================   LOGIN DISCOVER =====================================
            }
            else if (udp_msg.msg_type == xlink_msg.xbs_xlink_message_type.KAI_CLIENT_ATTACH)
            {
                KaySendMessageActualConsole(udp_msg, "KAI_CLIENT_ENGINE_IN_USE;");
            }
            else if (udp_msg.msg_type == xlink_msg.xbs_xlink_message_type.KAI_CLIENT_TAKEOVER)
            {
                KaySendMessageActualConsole(udp_msg, "KAI_CLIENT_ATTACH;");
                //=============================   BOTON LOGIN =====================================
            }
            else if (udp_msg.msg_type == xlink_msg.xbs_xlink_message_type.KAI_CLIENT_GETSTATE)
            {
              
                KaySendMessageActualConsole(udp_msg,new string[] {
                      "KAI_CLIENT_LOGGED_IN;",
                      "KAI_CLIENT_CODEPAGE; 0;",
                      "KAI_CLIENT_CODEPAGE; 0;",
                      "KAI_CLIENT_SESSION_KEY;3njEXQPnEQAZE2U7wHHKAGeP6hQ=;",
                      "KAI_CLIENT_VECTOR;XBSLINK;;",
                      "KAI_CLIENT_STATUS;XBSLink is Online..;",
                      String.Format("KAI_CLIENT_USER_DATA;{0};", _parent.KAI_CLIENT_LOCAL_NAME),

                       xlink_client_messages_helper.KAI_SERVER_INFO(
                       _parent.KAI_SERVER_NAME ,
                       "ONLINE", 
                       _parent.KAI_CLIENT_LOCAL_NAME,
                       xbs_settings.settings.REG_CLOUDLIST_SERVER,
                       _parent.udp_kay_socket_port.ToString(),
                       _parent.KAI_SERVER_VERSION ),

                      "KAI_CLIENT_ARENA_STATUS;1;1;",
                      "KAI_CLIENT_CONNECTED_MESSENGER;",
                      "KAI_CLIENT_CHATMODE;;",
                      "KAI_CLIENT_ADMIN_PRIVILEGES;;",
                      "KAI_CLIENT_MODERATOR_PRIVILEGES;;;",

                       xlink_client_messages_helper.KAI_CLIENT_ADD_CONTACT("seuffert"),
                       xlink_client_messages_helper.KAI_CLIENT_ADD_CONTACT("magurin"),
                       xlink_client_messages_helper.KAI_CLIENT_ADD_CONTACT("tuxuser")
                  });

                KaySendMessageActualConsole(udp_msg, String.Format("KAI_CLIENT_LOCAL_DEVICE;{0};", _parent.KAI_CLIENT_LOCAL_DEVICE));
                _parent.ProcessLogin(udp_msg);

            }
            else if (udp_msg.msg_type == xlink_msg.xbs_xlink_message_type.KAI_CLIENT_LOGOUT)
            {
                KaySendMessageActualConsole(udp_msg, xlink_client_messages_helper.KAI_CLIENT_DETACH(_parent.KAI_CLIENT_LOCAL_DEVICE));
                //=============================   LOGOUT =====================================
                _parent.ProcessLogout(udp_msg);

            }
            else if (udp_msg.msg_type == xlink_msg.xbs_xlink_message_type.KAI_CLIENT_CHATMODE)
            {
                //=============================   FASE DE CHAT =====================================
                string[] parameters = udp_msg.GetParameters();
                if (parameters.Length > 1)
                {
                }
            }
            //================================= JOIN A CHANNEL ==========================
            else if (udp_msg.msg_type == xlink_msg.xbs_xlink_message_type.KAI_CLIENT_VECTOR)
            {
                string[] parameters = udp_msg.GetParameters();
                if (parameters.Length > 1)
                {
                    var command = parameters[0];
                    if (command != "" && command != "Arena")
                    {
                        _parent.ConsoleProcessJoinCloud(udp_msg,command, parameters[1]);
                    }
                }
            }

            else if (udp_msg.msg_type == xlink_msg.xbs_xlink_message_type.KAI_CLIENT_GET_VECTORS)
            {
                string[] parameters = udp_msg.GetParameters();
                if (parameters.Length > 1)
                {
                }
                 //Console.Write(String.Format(" * XBOX -> CHAT MODE -> DETECTED KAI_CLIENT_CHATMODE  {0}!", udp_msg.src_ip), xbs_message_sender.X360);
            }
            else if (udp_msg.msg_type == xlink_msg.xbs_xlink_message_type.KAI_CLIENT_CHAT)
            {
                string[] parameters = udp_msg.GetParameters();
                if (parameters.Length > 1)
                {
                    var command = parameters[0].Trim();
                    if (command != "")
                        _parent.ConsoleProcessChat(udp_msg,command);

                }

            }
            else if (udp_msg.msg_type == xlink_msg.xbs_xlink_message_type.KAI_CLIENT_PM)
            {
                UTF8Encoding oDecoding = new UTF8Encoding();
                var CommandMsg = oDecoding.GetString(udp_msg.Data);
                string[] parameters = udp_msg.GetParameters();
                if (parameters.Length > 1)
                    _parent.ConsoleProcessPM(udp_msg,parameters[0], parameters[1]);
            }

        }

        #endregion

        #region Process Methods


        public void KaySendUpdateCloud(xlink_msg node, string CloudName, int Players, bool isPrivate, int MaxPlayers)
        {
            KaySendMessageActualConsole(node, xlink_client_messages_helper.KAI_CLIENT_SUB_VECTOR_UPDATE(CloudName, Players, isPrivate, MaxPlayers));
        }

        public void KaySendMessageActualConsole(xlink_msg udp_msg, string message)
        {
           
            _parent.SendMessageToQueue(udp_msg, message );
            _parent.ConsoleProcessSendMessage(udp_msg,message);
        }


        public void KaySendMessageActualConsole(xlink_msg msg, string[] messages)
        {
            foreach (var tmp in messages)
            {
                KaySendMessageActualConsole(msg, tmp);
            }
        }

        #endregion


    }

}