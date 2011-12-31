using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;


    public class XlinkKaiMsg
    {

        public XlinkKaiMsg()
        {
        }
       
        public XlinkKaiMsg(string ipAddress, int port , string msgText)
        {
            src_ip = IPAddress.Parse(ipAddress);
            src_port = port;
            SetMessage(msgText);

        }

        public override string ToString()
        {
            return data_msg;
        }

        void SetMessage(string msgText)
        {
            data_msg = msgText;
            data = getUTF8BytesFromString(msgText);
        }

        public XlinkKaiMsg(IPAddress ipAddress, int port, string msgText)
        {
            src_ip = ipAddress;
            src_port = port;
            SetMessage(msgText);
        }

        public enum xbs_message_sender : byte
        {
            GENERAL = 0x00,
            SNIFFER = 0x01,
            UDP_LISTENER = 0x02,
            UPNP = 0x03,
            COMMANDLINE_MESSAGE_DISCPATCHER = 0x04,
            CLOUDLIST = 0x05,
            NAT = 0x06,
            NODE = 0x07,
            NODELIST = 0x08,
            XBOX = 0x09,
            FATAL_ERROR = 0x1B
        }


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
            NO_KAY_MSG = 0xC1
        }


        public xbs_xlink_message_type msg_type;

        //UInt16 data_len = 0;
        byte[] data;
        string data_msg;

        public IPAddress src_ip;
        public Int32 src_port;

        public byte[] GetData()
        {
            return data;
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


        void SetData(byte[] _array)
        {
            data_msg = getStringFromUTF8Bytes(_array);
            data = _array;
        }

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
                else if (elemento.StartsWith("KAI_CLIENT_CHAT;"))
                    return xbs_xlink_message_type.KAI_CLIENT_CHAT;
            }
            catch (Exception)
            {
            }
            return xbs_xlink_message_type.NO_KAY_MSG;
        }




    }

