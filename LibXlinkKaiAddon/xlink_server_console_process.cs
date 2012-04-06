using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;


    public class xlink_server_console_process
    {

        public enum eXlinkPhase
        {
            InitialStatus = 0, Logged = 1, PrevChatMode = 2, ChatMode = 3, Disconnected = 4
        }

         eXlinkPhase _actual_phase = eXlinkPhase.InitialStatus;
         xlink_server _parent;

        public eXlinkPhase actual_phase
        {
            get
            {
                return _actual_phase;
            }
            set
            {
                _actual_phase = value;
            }
        }

        public xlink_server_console_process(xlink_server Parent)
        {
            _parent = Parent;
        }
   
        public IPAddress _sender_ip;
        public int _sender_port;

        public void ChangeSenderIPAddresPort(IPAddress console_ip_address, int console_port)
        {
            _sender_ip = console_ip_address;
            _sender_port = console_port;
        }

        #region Main Process

        public void ProcessReceivedMessage(xlink_msg udp_msg)
        {
            ChangeSenderIPAddresPort(udp_msg.src_ip, udp_msg.src_port);

            //=============================   PRIMER PAQUETE DISCOVER  =====================================
            if (udp_msg.msg_type == xlink_msg.xbs_xlink_message_type.KAI_CLIENT_DISCOVER)
            {
                //actual_phase = eXlinkPhase.InitialStatus;
                SendMessageActualConsole("KAI_CLIENT_ENGINE_HERE;");
                _parent.ProcessDebugMessage(String.Format(" * XBOX -> DETECTED KAI_CLIENT_DISCOVER FROM {0}!!!!!! ADD CONFIG PARAM WITH CONSOLE IP !!!!!!!!", udp_msg.src_ip), xlink_msg.xbs_message_sender.XBOX);

                //=============================   LOGIN DISCOVER =====================================
            }
            else if (udp_msg.msg_type == xlink_msg.xbs_xlink_message_type.KAI_CLIENT_ATTACH)
            {
                SendMessageActualConsole("KAI_CLIENT_ENGINE_IN_USE;");

                _parent.ProcessDebugMessage(String.Format(" * XBOX -> DETECTED KAI_CLIENT_TAKEOVER FROM {0}!!!!!! ADD CONFIG PARAM WITH CONSOLE IP !!!!!!!!", udp_msg.src_ip), xlink_msg.xbs_message_sender.XBOX);
            }
            else if (udp_msg.msg_type == xlink_msg.xbs_xlink_message_type.KAI_CLIENT_TAKEOVER)
            {
                SendMessageActualConsole("KAI_CLIENT_ATTACH;");
                _parent.ProcessDebugMessage(String.Format(" * XBOX -> SISTEMA DETECTADO! -> DETECTED KAI_CLIENT_ENGINE_IN_USE  {0}!!!!!! ADD CONFIG PARAM WITH CONSOLE IP !!!!!!!!", udp_msg.src_ip), xlink_msg.xbs_message_sender.XBOX);

                //Lanzamos el evento de attach
                actual_phase = eXlinkPhase.Logged;

                //=============================   BOTON LOGIN =====================================
            }
            else if (udp_msg.msg_type == xlink_msg.xbs_xlink_message_type.KAI_CLIENT_GETSTATE)
            {
                SendMessageActualConsole(new string[] {
                      "KAI_CLIENT_LOGGED_IN;",
                      "KAI_CLIENT_CODEPAGE; 0;",
                      "KAI_CLIENT_CODEPAGE; 0;",
                      "KAI_CLIENT_SESSION_KEY;3njEXQPnEQAZE2U7wHHKAGeP6hQ=;",
                      "KAI_CLIENT_VECTOR;XBSLINK;;",
                      "KAI_CLIENT_STATUS;XBSLink is Online..;",
                      String.Format("KAI_CLIENT_USER_DATA;{0};", _parent.KAI_CLIENT_LOCAL_NAME),
                      "KAI_CLIENT_ARENA_STATUS;1;1;",
                      "KAI_CLIENT_CONNECTED_MESSENGER;",
                      "KAI_CLIENT_CHATMODE;;",
                      "KAI_CLIENT_ADMIN_PRIVILEGES;;",
                      "KAI_CLIENT_MODERATOR_PRIVILEGES;;;",
                      "KAI_CLIENT_ADD_CONTACT;ON PROGRESS...;",
                      "KAI_CLIENT_ADD_CONTACT;Oli;",
                      "KAI_CLIENT_ADD_CONTACT;magurin;"
                  });

                //Creamos los clouds
                SendMessageActualConsole(String.Format("KAI_CLIENT_LOCAL_DEVICE;{0};", _parent.KAI_CLIENT_LOCAL_DEVICE));
                SendMessageActualConsole("KAI_CLIENT_ATTACH;");
                _parent.ProcessDebugMessage(String.Format(" * XBOX -> CONECTADO - MAIN MENU! -> DETECTED KAI_CLIENT_GETSTATE  {0}!!", udp_msg.src_ip), xlink_msg.xbs_message_sender.XBOX);
                _parent.ProcessLogin();

            }
            else if (udp_msg.msg_type == xlink_msg.xbs_xlink_message_type.KAI_CLIENT_LOGOUT)
            {

                SendMessageActualConsole(String.Format("KAI_CLIENT_DETACH;{0};", _parent.KAI_CLIENT_LOCAL_DEVICE));
                //=============================   LOGOUT =====================================

                actual_phase = eXlinkPhase.Disconnected;
                _parent.ProcessLogout();
                _parent.ProcessDebugMessage(String.Format(" * XBOX -> CLOSSING APLICACION! -> DETECTED KAI_CLIENT_LOGOUT  {0}!", udp_msg.src_ip), xlink_msg.xbs_message_sender.XBOX);
            }
            else if (udp_msg.msg_type == xlink_msg.xbs_xlink_message_type.KAI_CLIENT_CHATMODE)
            {
                //=============================   FASE DE CHAT =====================================
                UTF8Encoding oDecoding = new UTF8Encoding();
                var CommandMsg = oDecoding.GetString(udp_msg.GetData());
                string[] parameters = CommandMsg.Split(';');
                if (parameters.Length > 2)
                {
                    //NOT USED 
                }

                //ENTRANDO EN MODO CHAT
                actual_phase = eXlinkPhase.ChatMode;

                _parent.ProcessDebugMessage(String.Format(" * XBOX -> CHAT MODE -> DETECTED KAI_CLIENT_CHATMODE  {0}!", udp_msg.src_ip), xlink_msg.xbs_message_sender.XBOX);
            }
            //================================= JOIN A CHANNEL ==========================
            else if (udp_msg.msg_type == xlink_msg.xbs_xlink_message_type.KAI_CLIENT_VECTOR)
            {
                UTF8Encoding oDecoding = new UTF8Encoding();
                var CommandMsg = oDecoding.GetString(udp_msg.GetData());
                string[] parameters = CommandMsg.Split(';');
                if (parameters.Length > 2)
                {
                    var command = parameters[1].Trim();
                    if (command != "" && command != "Arena")
                    {
                        DeleteAllSystemUsers();
                        _parent.ConsoleProcessJoinCloud(command, parameters[2]);
                    }
                    else
                        AddSystemMainUsers();
                }


            }

            else if (udp_msg.msg_type == xlink_msg.xbs_xlink_message_type.KAI_CLIENT_GET_VECTORS)
            {

                UTF8Encoding oDecoding = new UTF8Encoding();
                var CommandMsg = oDecoding.GetString(udp_msg.GetData());
                string[] parameters = CommandMsg.Split(';');
                if (parameters.Length > 2)
                {
                    var command = parameters[1].Trim();
                    if (command != "" && command != "Arena" && command != "XBSLINK")
                        DeleteAllSystemUsers();
                    else if (command == "XBSLINK")
                        AddSystemMainUsers();
                }

                _parent.ProcessDebugMessage(String.Format(" * XBOX -> CHAT MODE -> DETECTED KAI_CLIENT_CHATMODE  {0}!", udp_msg.src_ip), xlink_msg.xbs_message_sender.XBOX);
            }
            else if (udp_msg.msg_type == xlink_msg.xbs_xlink_message_type.KAI_CLIENT_CHAT)
            {

                UTF8Encoding oDecoding = new UTF8Encoding();
                var CommandMsg = oDecoding.GetString(udp_msg.GetData());
                string[] parameters = CommandMsg.Split(';');
                if (parameters.Length > 2)
                {
                    var command = parameters[1].Trim();
                    if (command != "")
                    {
                        _parent.ConsoleProcessChat(command);
                    }

                }

            }
            else if (udp_msg.msg_type == xlink_msg.xbs_xlink_message_type.KAI_CLIENT_PM)
            {
                UTF8Encoding oDecoding = new UTF8Encoding();
                var CommandMsg = oDecoding.GetString(udp_msg.GetData());
                string[] parameters = CommandMsg.Split(';');
                if (parameters.Length > 2)
                {
                    var command = parameters[1].Trim();
                    if (command != "")
                    {
                        _parent.ConsoleProcessPM(parameters[1], parameters[2]);
                    }
                }

            }

        }

        #endregion

        #region Process Methods


        public void SendPMMessage(string username, string message)
        {
            SendMessageActualConsole(String.Format("KAI_CLIENT_PM;{0};{1};", username, message.Replace(";", "")));
        }

        public void SendChatMessage(string username, string message)
        {
            SendMessageActualConsole(String.Format("KAI_CLIENT_CHAT;XBSLINK;{0};{1};", username, message.Replace(";", "")));
        }

        public void SendUpdateCloud(string CloudName, int Players, bool isPrivate, int MaxPlayers)
        {
            //"KAI_CLIENT_SUB_VECTOR;FIFA;5;XBSLINK;-1;6;",
            //"KAI_CLIENT_USER_SUB_VECTOR;GRAN TURISMO;3;XBSLINK;-1;6;Tengo una vaca",
            SendMessageActualConsole(String.Format("KAI_CLIENT_SUB_VECTOR_UPDATE;{0};{1};XBSLINK;", CloudName, Players));
        }

        public void SendCreateCloud(xlink_channel Canal)
        {
            SendCreateCloud(Canal.name, Canal.node_count, Canal.isPrivate, Canal.max_nodes);
        }

        public void SendCreateCloud(string CloudName, int Players, bool isPrivate, int MaxPlayers)
        {
            SendMessageActualConsole(String.Format("KAI_CLIENT_USER_SUB_VECTOR;{0};{1};XBSLINK;{2};{3};{4}", CloudName, Players, ((isPrivate) ? "1" : "-1"), MaxPlayers, ((isPrivate) ? "PASSWORD PROTECTED" : "Public Arena")));
        }

        public void SendDetach()
        {
            SendMessageActualConsole(String.Format("KAI_CLIENT_DETACH;{0};", _parent.KAI_CLIENT_LOCAL_DEVICE));
            SendMessageActualConsole("KAI_CLIENT_ATTACH;");
        }

        void SendMessageActualConsole(string message)
        {
            _parent.SendMsgCola(new xlink_msg(_sender_ip, _sender_port, message));
            _parent.ConsoleProcessSendMessage(message, _sender_ip, _sender_port);
            //Thread.Sleep(400);
        }

        void SendMessageActualConsole(string[] messages)
        {
            foreach (var msg in messages)
            {
                SendMessageActualConsole(msg);
            }
        }

        void AddSystemMainUsers()
        {
            JoinUserToVector(".:XBSLINK:.");
            JoinUserToVector("Select a channel");
        }

        void DeleteAllSystemUsers()
        {
            LeaveUserFromVector(".:XBSLINK:.");
            LeaveUserFromVector("Select a channel");
        }

        public void LeaveUserFromVector(string username)
        {
            SendMessageActualConsole(String.Format("KAI_CLIENT_LEAVES_CHAT;General Chat;{0};", username));
            SendMessageActualConsole(String.Format("KAI_CLIENT_LEAVES_VECTOR;{0};", username));
        }

        public void JoinUserToVector(string username)
        {
            SendMessageActualConsole(String.Format("KAI_CLIENT_JOINS_VECTOR;{0};", username));
            SendMessageActualConsole(String.Format("KAI_CLIENT_JOINS_CHAT;General Chat;{0};", username));
        }

        void SendActualCloudList(List<xlink_channel> oChannels)
        {
            //Si hay los eliminamos
            foreach (var item in oChannels)
                SendCreateCloud(item);
        }


        #endregion


    }

