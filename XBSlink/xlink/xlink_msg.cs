using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace XBSlink.XlinkKai
{
   
    public class xlink_msg
    {

        public enum xbs_xlink_message_type : byte
        {
            KAI_CLIENT_DISCOVER = 0xD1,
            KAI_CLIENT_ATTACH = 0xD2,
            KAI_CLIENT_TAKEOVER = 0xD3,
            KAI_CLIENT_GETSTATE = 0xD4,
            KAI_CLIENT_CHATMODE = 0xD5,
            KAI_CLIENT_VECTOR = 0xD6,
            KAI_CLIENT_GET_VECTORS = 0xD7,
            KAI_CLIENT_LOGOUT = 0xD8,
            KAI_CLIENT_CHAT = 0xD9,
            KAI_CLIENT_PM = 0xE1,
            KAI_CLIENT_INVITE = 0xE2,
            KAI_CLIENT_CONTACT_OFFLINE = 0xE3,
            KAI_CLIENT_CONTACT_ONLINE = 0xE4,
            KAI_CLIENT_GET_PROFILE = 0xE5,

            KAI_CLIENT_SUB_VECTOR_UPDATE = 0xE6,
            KAI_CLIENT_USER_SUB_VECTOR = 0xE7,
            KAI_CLIENT_DETACH = 0xE8,
            KAI_CLIENT_LEAVES_CHAT = 0xE9,
            KAI_CLIENT_LEAVES_VECTOR = 0xF1,
            KAI_CLIENT_JOINS_VECTOR = 0xF2,
            KAI_CLIENT_JOINS_CHAT = 0xF3,
            KAI_CLIENT_ENGINE_HERE = 0xF4,
            KAI_CLIENT_ENGINE_IN_USE = 0xF5,
            KAI_CLIENT_ADD_CONTACT = 0xF6,
          
            
            NO_KAY_MSG = 0xC1
        }

        public IPAddress src_ip;
        public Int32 src_port;

        public xbs_xlink_message_type msg_type;

        //UInt16 data_len = 0;

        public string data_msg;
        public string parameters_msg;

        public string mac_address;

        private byte[] data;
        public byte[] Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
                mac_address = GetMacAddress(value);
                msg_type = xlink_msg.getMessageTypeFromUDPPacket(value);
                data_msg = getStringFromUTF8Bytes(value);
                parameters_msg = data_msg.Substring(getHeaderMessageFromType(msg_type).Length);
            }
        }

        public string GetMacAddress(byte[] data)
        {
            if (data.Length > 5)
            {
                string tmp = "";

                for (int i = 0; i < 5; i++)
                {
                    tmp += data[i];
                }
               // foreach (var item in data)
                   

                return tmp;
            }

            return "";
        }

        public string[] GetParameters(string message)
        {
            return xlink_client_messages_helper.GetParametersFromMessage(message);
        }

        public string[] GetParameters()
        {
            return GetParameters(parameters_msg);
            //return data_msg.Split(';');
        }

        public xlink_msg(byte[] datos)
        {
            Data = datos;
        }

        

        public xlink_msg(xlink_msg msg, string message)
        {
            SetMessage(msg.src_ip, msg.src_port, message);
        }

        public xlink_msg(xlink_msg msg)
        {
           SetMessage(msg.src_ip ,msg.src_port, msg.data_msg);
        }

        public xlink_msg(string ipAddress, int port, string msgText)
        {
           
            SetMessage(IPAddress.Parse(ipAddress),port, msgText);

        }

        public xlink_msg(IPAddress ipAddress, int port, string msgText)
        {
            SetMessage(ipAddress, port, msgText);
           
        }

        public xlink_msg(IPAddress ipAddress, int port)
        {
            SetMessage(ipAddress, port);
        }

        void SetMessage(IPAddress ipAddress, int port, string msgText)
        {
            SetMessage(ipAddress, port);
            SetDataFromText(msgText);
        }

        void SetMessage(IPAddress ipAddress, int port)
        {
            src_ip = ipAddress;
            src_port = port;
        }


        void SetDataFromText(string msgText)
        {
            Data = getUTF8BytesFromString(msgText);
        }



        /// <summary>
        /// OBTENER EL TIPO DE MENSAJE CON EL PAQUETE DATA
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static xbs_xlink_message_type getMessageTypeFromUDPPacket(byte[] bytes)
        {
            try
            {
                //TODO: ADICION DE PAQUETES 
                var elemento = getStringFromUTF8Bytes(bytes).Trim();
                if (elemento.StartsWith("KAI_CLIENT_DISCOVER;"))
                    return xbs_xlink_message_type.KAI_CLIENT_DISCOVER;
                else if (elemento.StartsWith("KAI_CLIENT_ATTACH;"))
                    return xbs_xlink_message_type.KAI_CLIENT_ATTACH;
                else if (elemento.StartsWith("KAI_CLIENT_TAKEOVER;"))
                    return xbs_xlink_message_type.KAI_CLIENT_TAKEOVER;
                else if (elemento.StartsWith("KAI_CLIENT_GETSTATE;"))
                    return xbs_xlink_message_type.KAI_CLIENT_GETSTATE;
                else if (elemento.StartsWith("KAI_CLIENT_CHATMODE;"))
                    return xbs_xlink_message_type.KAI_CLIENT_CHATMODE;
                else if (elemento.StartsWith("KAI_CLIENT_GET_VECTORS;"))
                    return xbs_xlink_message_type.KAI_CLIENT_GET_VECTORS;
                else if (elemento.StartsWith("KAI_CLIENT_VECTOR;"))
                    return xbs_xlink_message_type.KAI_CLIENT_VECTOR;
                else if (elemento.StartsWith("KAI_CLIENT_LOGOUT;"))
                    return xbs_xlink_message_type.KAI_CLIENT_LOGOUT;
                else if (elemento.StartsWith("KAI_CLIENT_PM;"))
                    return xbs_xlink_message_type.KAI_CLIENT_PM;
                else if (elemento.StartsWith("KAI_CLIENT_CHAT;"))
                    return xbs_xlink_message_type.KAI_CLIENT_CHAT;
                else if (elemento.StartsWith("KAI_CLIENT_INVITE;"))
                    return xbs_xlink_message_type.KAI_CLIENT_INVITE;
                else if (elemento.StartsWith("KAI_CLIENT_GETSTATE;"))
                    return xbs_xlink_message_type.KAI_CLIENT_GETSTATE;
                else if (elemento.StartsWith("KAI_CLIENT_CONTACT_ONLINE;"))
                    return xbs_xlink_message_type.KAI_CLIENT_CONTACT_ONLINE;
                else if (elemento.StartsWith("KAI_CLIENT_CONTACT_OFFLINE;"))
                    return xbs_xlink_message_type.KAI_CLIENT_CONTACT_OFFLINE;
                else if (elemento.StartsWith("KAI_CLIENT_GET_PROFILE;"))
                    return xbs_xlink_message_type.KAI_CLIENT_GET_PROFILE;

                 else if (elemento.StartsWith("KAI_CLIENT_SUB_VECTOR_UPDATE;"))
                    return xbs_xlink_message_type.KAI_CLIENT_SUB_VECTOR_UPDATE;
                 else if (elemento.StartsWith("KAI_CLIENT_USER_SUB_VECTOR;"))
                    return xbs_xlink_message_type.KAI_CLIENT_USER_SUB_VECTOR;
                 else if (elemento.StartsWith("KAI_CLIENT_DETACH;"))
                    return xbs_xlink_message_type.KAI_CLIENT_DETACH;
                 else if (elemento.StartsWith("KAI_CLIENT_LEAVES_CHAT;"))
                    return xbs_xlink_message_type.KAI_CLIENT_LEAVES_CHAT;
                 else if (elemento.StartsWith("KAI_CLIENT_LEAVES_VECTOR;"))
                    return xbs_xlink_message_type.KAI_CLIENT_LEAVES_VECTOR;
                 else if (elemento.StartsWith("KAI_CLIENT_JOINS_VECTOR;"))
                    return xbs_xlink_message_type.KAI_CLIENT_JOINS_VECTOR;
                 else if (elemento.StartsWith("KAI_CLIENT_JOINS_CHAT;"))
                    return xbs_xlink_message_type.KAI_CLIENT_JOINS_CHAT;
                 else if (elemento.StartsWith("KAI_CLIENT_ENGINE_HERE;"))
                    return xbs_xlink_message_type.KAI_CLIENT_ENGINE_HERE;
                 else if (elemento.StartsWith("KAI_CLIENT_ENGINE_IN_USE;"))
                    return xbs_xlink_message_type.KAI_CLIENT_ENGINE_IN_USE;
                 else if (elemento.StartsWith("KAI_CLIENT_ADD_CONTACT;"))
                    return xbs_xlink_message_type.KAI_CLIENT_ADD_CONTACT;

             

            }
            catch (Exception)
            {
            }
            return xbs_xlink_message_type.NO_KAY_MSG;
        }

        /// <summary>
        /// OBTENER CABECERA DE MENSAJE CON EL TIPO
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public static string getHeaderMessageFromType(xbs_xlink_message_type tipo)
        {

            switch (tipo)
            {

                    

                case xbs_xlink_message_type.KAI_CLIENT_DISCOVER:
                    return "KAI_CLIENT_DISCOVER;";
                case xbs_xlink_message_type.KAI_CLIENT_ATTACH:
                    return "KAI_CLIENT_ATTACH;";
                case xbs_xlink_message_type.KAI_CLIENT_TAKEOVER:
                    return "KAI_CLIENT_TAKEOVER;";
                case xbs_xlink_message_type.KAI_CLIENT_GETSTATE:
                    return "KAI_CLIENT_GETSTATE;";
                case xbs_xlink_message_type.KAI_CLIENT_CHATMODE:
                    return "KAI_CLIENT_CHATMODE;";
                case xbs_xlink_message_type.KAI_CLIENT_VECTOR:
                    return "KAI_CLIENT_VECTOR;";
                case xbs_xlink_message_type.KAI_CLIENT_GET_VECTORS:
                    return "KAI_CLIENT_GET_VECTORS;";
                case xbs_xlink_message_type.KAI_CLIENT_LOGOUT:
                    return "KAI_CLIENT_LOGOUT;";
                case xbs_xlink_message_type.KAI_CLIENT_CHAT:
                    return "KAI_CLIENT_CHAT;";
                case xbs_xlink_message_type.KAI_CLIENT_PM:
                    return "KAI_CLIENT_PM;";
                case xbs_xlink_message_type.KAI_CLIENT_INVITE:
                    return "KAI_CLIENT_INVITE;";
                case xbs_xlink_message_type.KAI_CLIENT_CONTACT_OFFLINE:
                    return "KAI_CLIENT_CONTACT_OFFLINE;";
                case xbs_xlink_message_type.KAI_CLIENT_CONTACT_ONLINE:
                    return "KAI_CLIENT_CONTACT_ONLINE;";
                case xbs_xlink_message_type.KAI_CLIENT_GET_PROFILE:
                    return "KAI_CLIENT_GET_PROFILE;";

                                    case xbs_xlink_message_type.KAI_CLIENT_SUB_VECTOR_UPDATE:
                    return "KAI_CLIENT_SUB_VECTOR_UPDATE;";
                                    case xbs_xlink_message_type.KAI_CLIENT_USER_SUB_VECTOR:
                    return "KAI_CLIENT_USER_SUB_VECTOR;";
                                    case xbs_xlink_message_type.KAI_CLIENT_DETACH:
                    return "KAI_CLIENT_DETACH;";
                                    case xbs_xlink_message_type.KAI_CLIENT_LEAVES_CHAT:
                    return "KAI_CLIENT_LEAVES_CHAT;";
                                    case xbs_xlink_message_type.KAI_CLIENT_LEAVES_VECTOR:
                    return "KAI_CLIENT_LEAVES_VECTOR;";
                                    case xbs_xlink_message_type.KAI_CLIENT_JOINS_VECTOR:
                    return "KAI_CLIENT_JOINS_VECTOR;";
                                    case xbs_xlink_message_type.KAI_CLIENT_JOINS_CHAT:
                    return "KAI_CLIENT_JOINS_CHAT;";
                                    case xbs_xlink_message_type.KAI_CLIENT_ENGINE_HERE:
                    return "KAI_CLIENT_ENGINE_HERE;";
                                    case xbs_xlink_message_type.KAI_CLIENT_ENGINE_IN_USE:
                    return "KAI_CLIENT_ENGINE_IN_USE;";
                                        case xbs_xlink_message_type.KAI_CLIENT_ADD_CONTACT:
                    return "KAI_CLIENT_ADD_CONTACT;";

            
            }

            return "NO_KAY_MSG;";
        }



        public bool IsKayPacket()
        {
            return (msg_type == xbs_xlink_message_type.KAI_CLIENT_ATTACH ||
               msg_type == xbs_xlink_message_type.KAI_CLIENT_CHAT ||
               msg_type == xbs_xlink_message_type.KAI_CLIENT_CHATMODE ||
               msg_type == xbs_xlink_message_type.KAI_CLIENT_CONTACT_OFFLINE ||
               msg_type == xbs_xlink_message_type.KAI_CLIENT_CONTACT_ONLINE ||
               msg_type == xbs_xlink_message_type.KAI_CLIENT_DISCOVER ||
               msg_type == xbs_xlink_message_type.KAI_CLIENT_GET_PROFILE ||
               msg_type == xbs_xlink_message_type.KAI_CLIENT_GET_VECTORS ||
               msg_type == xbs_xlink_message_type.KAI_CLIENT_GETSTATE ||
               msg_type == xbs_xlink_message_type.KAI_CLIENT_INVITE ||
               msg_type == xbs_xlink_message_type.KAI_CLIENT_LOGOUT ||
               msg_type == xbs_xlink_message_type.KAI_CLIENT_PM ||
               msg_type == xbs_xlink_message_type.KAI_CLIENT_TAKEOVER ||
                msg_type == xbs_xlink_message_type.KAI_CLIENT_SUB_VECTOR_UPDATE ||
                 msg_type == xbs_xlink_message_type.KAI_CLIENT_USER_SUB_VECTOR ||
                  msg_type == xbs_xlink_message_type.KAI_CLIENT_LEAVES_CHAT ||
                   msg_type == xbs_xlink_message_type.KAI_CLIENT_LEAVES_VECTOR ||
                    msg_type == xbs_xlink_message_type.KAI_CLIENT_JOINS_VECTOR ||
                     msg_type == xbs_xlink_message_type.KAI_CLIENT_JOINS_CHAT ||
                      msg_type == xbs_xlink_message_type.KAI_CLIENT_ENGINE_HERE ||
                       msg_type == xbs_xlink_message_type.KAI_CLIENT_ENGINE_IN_USE ||
                        msg_type == xbs_xlink_message_type.KAI_CLIENT_ADD_CONTACT ||
               msg_type == xbs_xlink_message_type.KAI_CLIENT_VECTOR);


        }

      


        public static byte[] getUTF8BytesFromString(String str)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] ret = Encoding.UTF8.GetBytes(str);
            return ret;
        }

        public static String getStringFromUTF8Bytes(byte[] bytes)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            String utf8String = encoder.GetString(bytes);
            return utf8String;
        }

        public void CleanArray()
        {
            SetCleanArray(Data);
        }

        public void SetCleanArray(byte[] _array)
        {

            int last_element = 0;
            //System.Collections.List<byte> eee = new System.Collections.List<byte>();

            for (int i = 2; i < _array.Length; i++)
            {
                if (_array[i] == 0)
                {
                    last_element = i;
                    i = _array.Length;
                }
            }

            byte[] dev = new byte[last_element];

            for (int i = 0; i < dev.Length; i++)
                dev[i] = _array[i];

            data = dev;
        }




    }



}