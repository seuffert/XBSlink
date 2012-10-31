using System;
using System.Collections.Generic;
using System.Text;

namespace XBSlink.Common
{
  public  class xlink_client_messages_helper
  {

      #region Clouds

      
      public static string GetStrFromArrayClouds(List<xbs_cloud> clouds)
      {
          return GetStrFromArrayClouds(clouds.ToArray());
      }

      private static string GetStrCloudLine(xbs_cloud cloud)
      {
          return String.Format("{0};{1};{2};{3}", cloud.name, cloud.node_count.ToString(), cloud.max_nodes.ToString(), cloud.isPrivate);
      }

      private static string GetStrFromArrayClouds(xbs_cloud[] clouds)
      {
          string dev = "";
          //var clouds = cloudlist.getCloudlistArray();
          for (int i = 0; i < clouds.Length; i++)
          {
              if (i > 0) dev += "|";
              dev += GetStrCloudLine(clouds[i]);
          }
          return dev;
      }


      #endregion

        #region LINKKAY MESSAGES


        public static string[] KAY_GET_USER_JOIN_TO_VECTOR(string username)
        {
            return new string[] {
                 KAI_CLIENT_JOINS_VECTOR(username),KAI_CLIENT_JOINS_CHAT(username)
             };
        }


        public static string[] KAY_GET_USER_JOIN_TO_VECTOR(string username, string client_version, string last_ping_delay_ms)
        {
            return new string[] {
                 KAI_CLIENT_JOINS_VECTOR(username,client_version,last_ping_delay_ms),KAI_CLIENT_JOINS_CHAT(username,client_version,last_ping_delay_ms)
             };

        }

        public static string[] KAY_GET_LEAVE_USER_FROM_VECTOR(string username)
        {
            return new string[] {
                KAI_CLIENT_LEAVES_CHAT(username), KAI_CLIENT_LEAVES_VECTOR(username)}
                ;
        }

        public static string[] KAY_GET_DETACH(string KAI_CLIENT_LOCAL_DEVICE)
        {
            return new string[] {
                KAI_CLIENT_DETACH(KAI_CLIENT_LOCAL_DEVICE), KAI_CLIENT_ATTACH()
            };
        }

        public static string KAI_CLIENT_PM(string username, string message)
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.KAI_CLIENT_PM) + String.Format("{0};{1};",username, message.Replace(";", ""));
        }

        public static string KAI_CLIENT_CHAT(string username, string message)
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.KAI_CLIENT_CHAT) + String.Format("XBSLINK;{0};{1};", username, message.Replace(";", ""));
        }

        public static string KAI_CLIENT_SUB_VECTOR_UPDATE(string username, string message)
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.KAI_CLIENT_SUB_VECTOR_UPDATE) + String.Format("XBSLINK;{0};{1};", username, message.Replace(";", ""));
        }

        public static string KAI_CLIENT_SUB_VECTOR_UPDATE(string CloudName, int Players, bool isPrivate, int MaxPlayers)
        {
            //"KAI_CLIENT_SUB_VECTOR;FIFA;5;XBSLINK;-1;6;",
            //"KAI_CLIENT_USER_SUB_VECTOR;GRAN TURISMO;3;XBSLINK;-1;6;Tengo una vaca",
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.KAI_CLIENT_SUB_VECTOR_UPDATE) + String.Format("{0};{1};XBSLINK;", CloudName, Players);
        }

        public static string KAI_CLIENT_USER_SUB_VECTOR(string CloudName, int Players, bool isPrivate, int MaxPlayers)
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.KAI_CLIENT_USER_SUB_VECTOR) + String.Format("{0};{1};XBSLINK;{2};{3};{4};", CloudName, Players, ((isPrivate) ? "1" : "-1"), MaxPlayers, ((isPrivate) ? "PASSWORD PROTECTED" : "Public Arena"));
        }

        public static string KAI_CLIENT_DETACH()
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.KAI_CLIENT_DETACH);
        }

        public static string KAI_CLIENT_DETACH(string KaiClienLocalDevice)
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.KAI_CLIENT_DETACH) + KaiClienLocalDevice + ";";
        }

        public static string KAI_CLIENT_ATTACH()
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.KAI_CLIENT_ATTACH) ;
        }

        public static string KAI_CLIENT_LEAVES_CHAT(string username)
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.KAI_CLIENT_LEAVES_CHAT) + String.Format("General Chat;{0};", username);
        }

        public static string KAI_CLIENT_LEAVES_VECTOR(string username)
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.KAI_CLIENT_LEAVES_VECTOR) + String.Format("{0};", username);
        }

        public static string KAI_CLIENT_ADD_CONTACT(string username)
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.KAI_CLIENT_ADD_CONTACT) + String.Format("{0};", username);
        }

        public static string KAI_CLIENT_JOINS_VECTOR(string username, string client_version, string last_ping_delay_ms)
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.KAI_CLIENT_JOINS_VECTOR) + String.Format("{0};{1};{2};", username, client_version, last_ping_delay_ms);
        }

        public static string KAI_CLIENT_JOINS_VECTOR(string username)
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.KAI_CLIENT_JOINS_VECTOR) + String.Format("{0};", username);
        }

        public static string KAI_CLIENT_JOINS_CHAT(string username)
        {
           return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.KAI_CLIENT_JOINS_CHAT) + String.Format("General Chat;{0};", username);
        }

        public static string KAI_CLIENT_JOINS_CHAT(string username, string client_version, string last_ping_delay_ms)
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.KAI_CLIENT_JOINS_CHAT) + String.Format("General Chat;{0};{1};{2};", username, client_version,last_ping_delay_ms);
        }


        #endregion

        #region KAYCLIENT


        public static string KAI_GET_SERVER_INFO()
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.KAI_GET_SERVER_INFO);
        }

        public static string KAI_SERVER_INFO(string server_name, string state, string nickname, string cloud_server_ip, string cloud_server_port, string server_version)
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.KAI_SERVER_INFO) + server_name + ";" + state + ";" + nickname + ";" + cloud_server_ip + ";" + cloud_server_port + ";" + server_version + ";";
        }

        public static string KAI_CLIENT_DISCOVER()
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.KAI_CLIENT_DISCOVER);
        }

        public static string KAI_CLIENT_TAKEOVER()
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.KAI_CLIENT_TAKEOVER);
        }

        public static string KAI_CLIENT_GETSTATE()
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.KAI_CLIENT_GETSTATE);
        }

        public static string KAI_CLIENT_LOGOUT()
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.KAI_CLIENT_LOGOUT);
        }

        public static string KAI_CLIENT_VECTOR(string cloud, string password)
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.KAI_CLIENT_VECTOR) + String.Format("{0};{1};", cloud, password);
        }

        public static string KAI_CLIENT_GET_VECTORS()
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.KAI_CLIENT_GET_VECTORS);
        }

        public static string KAI_CLIENT_CHATMODE()
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.KAI_CLIENT_CHATMODE);
        }

        public static string KAI_CLIENT_CHAT(string message)
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.KAI_CLIENT_CHAT) + String.Format("{0};", message);
        }

        //public static string KAI_CLIENT_PM(string username,string message)
        //{
        //    return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.KAI_CLIENT_PM) +String.Format("{0};{1}", username, message);
        //}

        #endregion

        #region General

        public static string[] GetParametersFromMessage(string message) {
          return message.Split(';');
        }

      #endregion

  }
}
