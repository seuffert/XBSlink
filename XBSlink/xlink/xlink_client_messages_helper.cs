using System;
using System.Collections.Generic;
using System.Text;

namespace XBSlink.XlinkKai
{
  public  class xlink_client_messages_helper
  {

      #region Clouds


      public static string GetClouds(xbs_cloud[] clouds)
      {
          return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.SERVER_GET_CLOUDS) + GetStrFromArrayClouds(clouds);
      }

      public static string GetClouds(List<xbs_cloud> clouds)
      {
          return GetClouds(clouds.ToArray());
      }

      public static xbs_cloud[] GetArrayCloudsFromStr(string clouds)
      {
          var clouds_splitted = clouds.Split('|');
          xbs_cloud[] dev = new xbs_cloud[clouds_splitted.Length];
          for (int i = 0; i < clouds_splitted.Length; i++)
          {
              var parameters = clouds_splitted[i].Split(';');
              dev[i] = new xbs_cloud(parameters[0], int.Parse(parameters[1]), int.Parse(parameters[2]), bool.Parse(parameters[3]));
          }
          return dev;
      }

      public static string SERVER_ADD_CLOUD(xbs_cloud cloud)
      {
          return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.SERVER_ADD_CLOUD) + GetStrCloudLine(cloud);
      }

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

      #region SEND MESSAGES

      public static string ClientCloudCreateJoin(string Cloud, int MaxUsers, string Password)
      {
          return String.Format("{0}{1};{2};{3};",

              xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.CLIENT_CLOUD_CREATE_JOIN),
              Cloud,
              MaxUsers.ToString(),
              Password);
          
      }

      public static string ClientCloudJoin(string Cloud)
      {
          return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.CLIENT_CLOUD_JOIN) + Cloud + ";";
      }

      public static string ClientCloudGet()
      {
          return (xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.CLIENT_CLOUDS_GET));
      }

      public static string ClientCloudLeave()
      {
          return (xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.CLIENT_CLOUD_LEAVE));
      }

      public static string ClientSendChatMessage(string Message)
      {
          return (
              xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.CLIENT_SEND_CHAT_MESSAGE)
              + Message + ";"
              );
      }

      public static string ClientSendPM(string UserName, string Message)
      {
          return (xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.CLIENT_SEND_PM) + UserName + ";" + Message + ";");
      }

      public static string ClientStart()
      {
          return (xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.CLIENT_START));
      }

      public static string ClientStop()
      {
          return (xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.CLIENT_STOP));
      }

      public static string ClientFavoriteAdd(string nickname)
      {
          return (xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.CLIENT_FAVORITE_ADD));
      }

      public static string ClientFavoriteDel(string nickname)
      {
          return (xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.CLIENT_FAVORITE_DEL));
      }

      public static string ClientConnect()
      {
          return (xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.CLIENT_CONNECT));
      }

      public static string ClientDisconnect()
      {
          return (xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.CLIENT_DISCONNECT));
      }

      public static string ClientDiscover()
      {
          return (xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.CLIENT_DISCOVER));
      }

      #endregion


        #region SERVER SEND MESSAGES

        public static string SERVER_STOP()
        {
            //"KAI_CLIENT_SUB_VECTOR;FIFA;5;XBSLINK;-1;6;",
            //"KAI_CLIENT_USER_SUB_VECTOR;GRAN TURISMO;3;XBSLINK;-1;6;Tengo una vaca",
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.SERVER_STOP);
        }

        public static string SERVER_CLOUD_CREATE_JOIN(string CloudName, int Players, bool isPrivate, int MaxPlayers)
        {
            //"KAI_CLIENT_SUB_VECTOR;FIFA;5;XBSLINK;-1;6;",
            //"KAI_CLIENT_USER_SUB_VECTOR;GRAN TURISMO;3;XBSLINK;-1;6;Tengo una vaca",
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.SERVER_CLOUD_CREATE_JOIN) + String.Format("{0};{1};XBSLINK;", CloudName, Players);
        }

        public static string ServerUserOnline(string username,string client_version, string last_ping_delay_ms)
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.SERVER_USER_ONLINE) + username + ";"+ client_version + ";"+ last_ping_delay_ms + ";";
        }

        public static string ServerUserOffline(string username)
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.SERVER_USER_OFFLINE) + username + ";";
        }

        public static string ServerMyPMMessage(string tousername, string message)
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.SERVER_PM_MY) + tousername + ";" + message + ";";
        }

        public static string ServerAddCloud(string name, string node_count, string max_nodes, bool isPrivate)
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.SERVER_ADD_CLOUD) + String.Format("{0};{1};{2};{3};", name, node_count, max_nodes, isPrivate.ToString());
        }

        public static string ServerUserPMMessage(string fromusername, string message)
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.SERVER_CHAT_USER_MESSAGE) + fromusername + ";" + message + ";";
        }

        public static string ServerMyChatMessage(string message)
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.SERVER_CHAT_MY_MESSAGE) + message + ";";
        }

        public static string ServerUserChatMessage(string username,string message)
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.SERVER_CHAT_USER_MESSAGE) + username + ";" + message + ";" ;
        }

        public static string ServerInfoServer(string server_name, string state, string nickname, string cloud_server_ip,string cloud_server_port, string server_version)
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.SERVER_INFO) + server_name + ";" + state + ";" + nickname + ";" + cloud_server_ip + ";" + cloud_server_port + ";" + server_version + ";";
        }

        public static string GetClouds(string CloudLines)
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.SERVER_GET_CLOUDS) + CloudLines;
        }

        public static string GetCloudsLine(string name, int node_count, int max_nodes, bool isPrivate)
        {
            return String.Format("{0};{1};{2};{3}", name, node_count.ToString(), max_nodes.ToString(), isPrivate);
        }


        #endregion

        #region LINKKAY MESSAGES


        public static string[] KAY_GET_USER_JOIN_TO_VECTOR(string username)
        {

            return new string[] {
                 KAI_CLIENT_JOINS_VECTOR(username),KAI_CLIENT_JOINS_CHAT(username)
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

        public static string KAI_CLIENT_JOINS_VECTOR(string username)
        {
            return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.KAI_CLIENT_JOINS_VECTOR) + String.Format("{0};", username);
        }

        public static string KAI_CLIENT_JOINS_CHAT(string username)
        {
           return xlink_msg.getHeaderMessageFromType(xlink_msg.xbs_xlink_message_type.KAI_CLIENT_JOINS_CHAT) + String.Format("General Chat;{0};", username);
        }

        #endregion

        #region General

        public static string[] GetParametersFromMessage(string message) {
          return message.Split(';');
        }

      #endregion

  }
}
