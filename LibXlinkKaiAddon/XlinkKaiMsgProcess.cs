using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;


    public class XlinkKaiMsgProcess
    {

      

        public enum eXlinkPhase
        {
            InitialStatus = 0, Logged = 1, PrevChatMode = 2, ChatMode = 3
        }

         eXlinkPhase _actual_phase = eXlinkPhase.InitialStatus;
         XlinkKaiAddonClient _parent;

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

        public XlinkKaiMsgProcess(XlinkKaiAddonClient Parent)
        {
            _parent = Parent;
        }


        void AddSystemMainUsers()
        {
            //en el principal
            JoinUserToVector(".:XBSLINK:.");
            JoinUserToVector("Select a channel to");
            JoinUserToVector("join the game. run");
            JoinUserToVector("the game and search");
            JoinUserToVector("LAN games.");
        }

        void DeleteAllSystemUsers()
        {
            //LeaveUserFromVector("Joined successfull.");
            //LeaveUserFromVector("Joining cloud..");
            LeaveUserFromVector(".:XBSLINK:.");
            LeaveUserFromVector("Select a channel to");
            LeaveUserFromVector("join the game. run");
            LeaveUserFromVector("the game and search");
            LeaveUserFromVector("LAN games.");
        }


        public void LeaveUserFromVector(string username)
     {
        // DeleteUserFromArray(username);

         SendMessageActualConsole("KAI_CLIENT_LEAVES_CHAT;General Chat;" + username + ";");
         SendMessageActualConsole("KAI_CLIENT_LEAVES_VECTOR;" + username + ";");
     }

        public void JoinUserToVector(string username)
        {
            //if (users != null)
            //{
                //AddUserToArray(username);
                SendMessageActualConsole("KAI_CLIENT_JOINS_VECTOR;" + username + ";");
                SendMessageActualConsole("KAI_CLIENT_JOINS_CHAT;General Chat;" + username + ";");
          //  }
        }

        //xbs_cloud[] elementos;

        void SendActualCloudList(List<XlinkKaiChannel> oChannels)
        {
            //Si hay los eliminamos
                foreach (var item in oChannels)
                    SendCreateCloud(item);

        }

        public IPAddress _sender_ip;
        public int _sender_port;

        public void ChangeSenderIPAddresPort(IPAddress console_ip_address, int console_port)
        {
            _sender_ip = console_ip_address;
            _sender_port = console_port;
        }

        public void ProcessReceivedMessage(XlinkKaiMsg udp_msg)
        {
            ChangeSenderIPAddresPort(udp_msg.src_ip, udp_msg.src_port);
           //_parent.ChangeIPAddresPort(udp_msg.src_ip, udp_msg.src_port);

            //=============================   PRIMER PAQUETE DISCOVER  =====================================
            if (udp_msg.msg_type == XlinkKaiMsg.xbs_xlink_message_type.KAI_CLIENT_DISCOVER)
            {
                //actual_phase = eXlinkPhase.InitialStatus;
                SendMessageActualConsole("KAI_CLIENT_ENGINE_HERE;");

                    _parent.ProcessDebugMessage (" * XBOX -> DETECTED KAI_CLIENT_DISCOVER FROM " + udp_msg.src_ip + "!!!!!! ADD CONFIG PARAM WITH CONSOLE IP !!!!!!!!", XlinkKaiMsg.xbs_message_sender.XBOX);

                //=============================   LOGIN DISCOVER =====================================
            }
            else if (udp_msg.msg_type == XlinkKaiMsg.xbs_xlink_message_type.KAI_CLIENT_ATTACH)
            {
                SendMessageActualConsole("KAI_CLIENT_ENGINE_IN_USE;");
            
                    _parent.ProcessDebugMessage(" * XBOX -> DETECTED KAI_CLIENT_TAKEOVER FROM " + udp_msg.src_ip + "!!!!!! ADD CONFIG PARAM WITH CONSOLE IP !!!!!!!!", XlinkKaiMsg.xbs_message_sender.XBOX);
            }
            else if (udp_msg.msg_type == XlinkKaiMsg.xbs_xlink_message_type.KAI_CLIENT_TAKEOVER)
            {
                SendMessageActualConsole("KAI_CLIENT_ATTACH;");

                _parent.ProcessDebugMessage(" * XBOX -> SISTEMA DETECTADO! -> DETECTED KAI_CLIENT_ENGINE_IN_USE  " + udp_msg.src_ip + "!!!!!! ADD CONFIG PARAM WITH CONSOLE IP !!!!!!!!", XlinkKaiMsg.xbs_message_sender.XBOX);

                //Lanzamos el evento de attach
                actual_phase = eXlinkPhase.Logged;
                //=============================   BOTON LOGIN =====================================
            }
            else if (udp_msg.msg_type == XlinkKaiMsg.xbs_xlink_message_type.KAI_CLIENT_GETSTATE)
            {


                SendMessageActualConsole(new string[] {
                      "KAI_CLIENT_LOGGED_IN;",
                      "KAI_CLIENT_CODEPAGE; 0;",
                      "KAI_CLIENT_CODEPAGE; 0;",
                      "KAI_CLIENT_SESSION_KEY;3njEXQPnEQAZE2U7wHHKAGeP6hQ=;",
                      "KAI_CLIENT_VECTOR;XBSLINK;;",
                      "KAI_CLIENT_STATUS;XBSLink is Online..;",
                      "KAI_CLIENT_USER_DATA;" + _parent.KAI_CLIENT_LOCAL_NAME + ";",
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
                //SendActualCloudList();

                //Obtenemos los usuarios
                //SendActualUserListFromCloud();
                SendMessageActualConsole("KAI_CLIENT_LOCAL_DEVICE;" + _parent.KAI_CLIENT_LOCAL_DEVICE + ";");

                //KAI_CLIENT_VECTOR;GRAN TURISMO;;

                //(-TU-)Kovert-xX/(JA)CAS/(japan)DNA/(JPN)bamboo_adm/-Gunslinger-/DyinTryin/GhOsTMaTa/Hawk_The_Slayer/l3laze/SgtLegend/shiningkiwi/[1up]Stickey/[3D]-GohitaN/[FLdS]_FAOS/

                //KAI_CLIENT_JOINS_CHAT

                SendMessageActualConsole("KAI_CLIENT_ATTACH;");

                    _parent.ProcessDebugMessage(" * XBOX -> CONECTADO - MENU PRINCIPAL! -> DETECTED KAI_CLIENT_GETSTATE  " + udp_msg.src_ip + "!!", XlinkKaiMsg.xbs_message_sender.XBOX);
                    _parent.ProcessLogin();

            }
            else if (udp_msg.msg_type == XlinkKaiMsg.xbs_xlink_message_type.KAI_CLIENT_LOGOUT)
            {
                //elementos = null;

                SendMessageActualConsole("KAI_CLIENT_DETACH;" + _parent.KAI_CLIENT_LOCAL_DEVICE + ";");

                //=============================   LOGOUT =====================================
         
                    _parent.ProcessLogout();

                
                    _parent.ProcessDebugMessage (" * XBOX -> CERRADO APLICACION! -> DETECTED KAI_CLIENT_LOGOUT  " + udp_msg.src_ip + "!", XlinkKaiMsg.xbs_message_sender.XBOX);
            }
            else if (udp_msg.msg_type == XlinkKaiMsg.xbs_xlink_message_type.KAI_CLIENT_CHATMODE)
            {
                //=============================   FASE DE CHAT =====================================
                UTF8Encoding oDecoding = new UTF8Encoding();
                var CommandMsg = oDecoding.GetString(udp_msg.GetData());
                string[] parameters = CommandMsg.Split(';');
                if (parameters.Length > 2)
                {
                    //var command = parameters[1].Trim();
                    //if (_actual_arena != "")
                    //{
                    //    //_actual_arena = command;
                    //    //RemoveUserList();
                    //    //AddUserList();
                    //    //Reenviamos el comando
                    //}
                    //else
                    //{
                    //    DeleteAllSystemUsers();
                    //    //JoinUserToVector("No users here.");
                    //}

                }

                //ENTRANDO EN MODO CHAT
               actual_phase = eXlinkPhase.ChatMode;
             
               _parent.ProcessDebugMessage(" * XBOX -> CHAT MODE -> DETECTED KAI_CLIENT_CHATMODE  " + udp_msg.src_ip + "!", XlinkKaiMsg.xbs_message_sender .XBOX);
            }
            //================================= JOIN A CHANNEL ==========================
            else if (udp_msg.msg_type == XlinkKaiMsg.xbs_xlink_message_type.KAI_CLIENT_VECTOR)
            {
                UTF8Encoding oDecoding = new UTF8Encoding();
                var CommandMsg = oDecoding.GetString(udp_msg.GetData());
                string[] parameters = CommandMsg.Split(';');
                if (parameters.Length > 2)
                {
                    var command = parameters[1].Trim();
                    if (command != "" && command != "Arena")
                    {
                        //RemoveUserList();
                        DeleteAllSystemUsers();
                        //_actual_arena = command;
                        //var encontrado = (from elemento in xbs_cloudlist.getInstance().getCloudlistArray() where elemento.name == command select elemento).FirstOrDefault();
                        //if (encontrado != null)
                        //{

                          //  _actual_arena = encontrado.name;

                            //DeleteAllSystemUsers();
                            //JoinUserToVector("Joining cloud..");
                             _parent.ProcessJoinCloud(command);

                            //SendActualUserListFromCloud()

                            //AddUserList();
                        }
                        else
                            AddSystemMainUsers();
                    }

                    //SendMessageActualConsole(new string[] { 
                    //        "KAI_CLIENT_JOINS_CHAT;" + _actual_arena + ";martinsxxrta;",
                    //        "KAI_CLIENT_JOINS_CHAT;" + _actual_arena + ";Jose;",
                    //    });
                }
          
            else if (udp_msg.msg_type == XlinkKaiMsg.xbs_xlink_message_type.KAI_CLIENT_GET_VECTORS)
            {

                UTF8Encoding oDecoding = new UTF8Encoding();
                var CommandMsg = oDecoding.GetString(udp_msg.GetData());
                string[] parameters = CommandMsg.Split(';');
                if (parameters.Length > 2)
                {
                    var command = parameters[1].Trim();
                    if (command != "" && command != "Arena" && command != "XBSLINK")
                    {
                        //Arena que deseamosobtener los vectores
                        //_actual_arena = command;
                        DeleteAllSystemUsers();
                        //RemoveUserList();
                        //SendActualCloudList();
                        //AddUserList();
                    }
                    else if (command == "XBSLINK")
                    {
                        //DeleteAllSystemUsers();
                        //RemoveUserList();
                        //SendActualCloudList();
                        AddSystemMainUsers();
                    }
                }

                //Solicitamos los vectores (usuarios de un canal)

                //Enviamos otra información del canal:
                //KAI_CLIENT_LEAVES_CHAT;Arena;magurin;KAI_CLIENT_JOINS_CHAT;;(Cpl)-DjLuK4z;KAI_CLIENT_JOINS_CHAT;;(MW3
                //KAI_CLIENT_CHAT;Arena;Kai Orbital Mesh;Welcome to XLink Kai's Arena Mode!

                //KAI_CLIENT_LEAVES_CHAT; Arena; magurin;
                //SendMessageActualConsole(new string[] { 
                //    "KAI_CLIENT_JOINS_CHAT;" + _actual_arena + ";martinsxxrta;",
                //    "KAI_CLIENT_JOINS_CHAT;" + _actual_arena + ";Jose;",
                //});

                //SendMessageActualConsole("KAI_CLIENT_ARENA_PING;Jose;136;0;1;;");
                //SendMessageActualConsole("KAI_CLIENT_CHAT;" + _actual_arena + ";Kai Orbital Mesh;BIENVENIDOOOO!Use the left panel to get yourself in the desired arena and start playing!;");
                //SendMessageActualConsole("KAI_CLIENT_DETACH;");

                //ENTRANDO EN MODO CHAT
                //actual_phase = eXlinkPhase.ChatMode;
               
                    _parent.ProcessDebugMessage(" * XBOX -> CHAT MODE -> DETECTED KAI_CLIENT_CHATMODE  " + udp_msg.src_ip + "!", XlinkKaiMsg.xbs_message_sender .XBOX);
            }
            else if (udp_msg.msg_type == XlinkKaiMsg.xbs_xlink_message_type.KAI_CLIENT_CHAT)
            {

                UTF8Encoding oDecoding = new UTF8Encoding();
                var CommandMsg = oDecoding.GetString(udp_msg.GetData());
                string[] parameters = CommandMsg.Split(';');
                if (parameters.Length > 2)
                {
                    var command = parameters[1].Trim();
                    if (command != "")
                    {
                       
                            _parent.ProcessChat(command);
                    }

                }

            }

            //KAI_CLIENT_CHATMODE;;
        }

        public void SendChatMessage(string username, string message)
        {
            SendMessageActualConsole("KAI_CLIENT_CHAT;XBSLINK;" + username + ";" + message.Replace(";", "") + ";");
        }

        public void SendUpdateCloud(string CloudName, int Players, bool isPrivate, int MaxPlayers)
        {
            //"KAI_CLIENT_SUB_VECTOR;FIFA;5;XBSLINK;-1;6;",
            //"KAI_CLIENT_USER_SUB_VECTOR;GRAN TURISMO;3;XBSLINK;-1;6;Tengo una vaca",
            SendMessageActualConsole("KAI_CLIENT_SUB_VECTOR_UPDATE;" + CloudName + ";" + Players + ";XBSLINK;");
        }

        public void SendCreateCloud(XlinkKaiChannel Canal)
        {
            SendCreateCloud(Canal.name, Canal.node_count, Canal.isPrivate, Canal.max_nodes);
        }

        public void SendCreateCloud(string CloudName, int Players, bool isPrivate, int MaxPlayers)
        {
            //"KAI_CLIENT_SUB_VECTOR;FIFA;5;XBSLINK;-1;6;",
            //"KAI_CLIENT_USER_SUB_VECTOR;GRAN TURISMO;3;XBSLINK;-1;6;Tengo una vaca",
            //SendMessageActualConsole("KAI_CLIENT_SUB_VECTOR;" + CloudName + ";" + Players + ";XBSLINK;" + ((isPrivate) ? "1" : "-1") + ";" + MaxPlayers.ToString() + ";");
            SendMessageActualConsole("KAI_CLIENT_USER_SUB_VECTOR;" + CloudName + ";" + Players + ";XBSLINK;" + ((isPrivate) ? "1" : "-1") + ";" + MaxPlayers.ToString() + ";" + ((isPrivate) ? "PASSWORD PROTECTED" : "Public Arena"));
        }

        //KAI_CLIENT_GETSTATE;
        void SendMessageActualConsole(string message)
        {
            _parent.SendMsgCola(new XlinkKaiMsg(_sender_ip, _sender_port, message));
            _parent.ProcessSendMessage(message, _sender_ip,  _sender_port);
            Thread.Sleep(500);
        }

        void SendMessageActualConsole(string[] messages)
        {
            foreach (var msg in messages)
            {
                SendMessageActualConsole(msg);
            }
        }

    }

