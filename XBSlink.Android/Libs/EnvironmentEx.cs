using System;
using System.Collections.Generic;
using System.Text;
using XBSlink.Android.Grid;
using XBSlink.XlinkKai;

namespace XBSlink.Android
{
    public class EnvironmentEx
    {

        public static string XBSLINK_SERVER_NAME = "NOT CONNECTED";
        public static string XBSLINK_SERVER_VERSION = "";
        public static string CLOUD_SERVER_IP = "";
        public static string CLOUD_SERVER_PORT = "";
        public static string NICKNAME ="";

        public static bool SHUTDOWN = false;

        public static ConnectionState APP_ACTUALSTATE { get; set; }

        public static bool IsConnected()
        {
            return (APP_ACTUALSTATE != ConnectionState.Disconnected);
        }
       
        public enum ConnectionState
        {
            Disconnected = 0,
            CloudDisconnected = 1,
            CloudConnected = 2
          
        }

        public static List<CloudsItem> _clouds = new List<CloudsItem>();
        public static List<PMItem> _pms = new List<PMItem>();
        public static List<UsersItem> _users = new List<UsersItem>();
        public static List<string> _log_cache = new List<string>();
        public static List<ChatItem> _chats_cache = new List<ChatItem>();


        public static void InsertLog(string message)
        {
            _log_cache.Add(message);
        }

        public static void InsertChat(ChatItem chat)
        {
            _chats_cache.Add(chat);
        }
     

        public static CloudsItem CloudFind(string cloudname)
        {
            lock (_clouds)
            {
                foreach (var item in _clouds)
                {
                    if (cloudname == item.Name)
                        return item;
                }
                return null;
            }
        }


        public static void CloudAdd(CloudsItem cloud_added)
        {
            lock (_clouds)
            {
                var encontrado = CloudFind(cloud_added.Name);
                if (encontrado != null)
                    cloud_added.SetData(encontrado);
                else
                    _clouds.Add(cloud_added);
            }
        }


        public static void CloudDelete(CloudsItem item)
        {
            lock (_clouds)
            {
                var encontrada = CloudFind(item.Name);
                if (encontrada != null)
                    _clouds.Remove(item);
            }
           
        }

        public static UsersItem UserFind(string nickname)
        {
            lock (_users)
            {
                foreach (var item in _users)
                {
                    if (nickname == item._nickname)
                        return item;
                }
                return null;
            }
        }

        public static void UserAdd(xlink_server_user_online_message nickname)
        {
            lock (_users)
            {
                var encontrado = UserFind(nickname._username);
                if (encontrado != null)
                    encontrado.SetData(nickname);
                else
                    _users.Add(new UsersItem(nickname));
            }
        }

        public static void UserDelete(string nickname)
        {
            lock (_users)
            {
                var encontrado = UserFind(nickname);
                if (encontrado != null)
                    _users.Remove(encontrado);
            }
        }

        public static PMItem PMFind(string nickname)
        {

            lock (_pms)
            {

                foreach (var item in EnvironmentEx._pms)
                {
                    if (nickname == item.Nombre)
                        return item;
                }
                return null;
            }
        }

        public static void PMAdd(string nickname)
        {
            lock (_pms)
            {
                if (PMFind(nickname) == null)
                    EnvironmentEx._pms.Add(new PMItem()
                    {
                        Nombre = nickname,
                        Asunto = "",
                        Image = Resource.Drawable.Icon
                    });
            }
        }

        public static void PMDelete(string nickname)
        {
            lock (_pms)
            {
                var encontrado = PMFind(nickname);
                if (encontrado != null)
                    EnvironmentEx._pms.Remove(encontrado);
            }
        }


    }
}
