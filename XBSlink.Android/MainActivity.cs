using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Net;
using XBSLink.Client.Android.Libs;
using XBSLink.Client.Android.Libs.DB;
using XBSlink.XlinkKai;
using XBSlink.Android.Grid;


namespace XBSlink.Android
{
     [Activity(Label = "XBSLink for Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : MasterTabContainer
     {

         public AndroidClientEngineProcess ClientEngineProcessManager { get; set; }
         public AndroidClientEngine ClientEngineManager { get; set; }

         public static bool Finished = false;
         //int server_port = 34522;

         ActivityBroadcastReceiver broadcastReceiver;

         public EnvironmentEx.ConnectionState GetAppActualState()
         {
             return EnvironmentEx.APP_ACTUALSTATE;
         }

         protected override void OnCreate(Bundle bundle)
         {
             base.OnCreate(bundle);

             // Create your application here
             SetContentView(Resource.Layout.MainTab);
             
             InitializeTabs();
             InitializeClientEngine();

             RefrescandoServidores();
         }

         void InitializeTabs()
         {
             tab_clouds = AddTab("clouds", "Clouds", Resource.Drawable.ic_tab_artists_grey, typeof(MainCloudsActivity));
             tab_chat = AddTab("chat", "Chat", Resource.Drawable.ic_tab_artists_grey, typeof(MainChatActivity));
             tab_pm = AddTab("pm", "PM", Resource.Drawable.ic_tab_artists_grey, typeof(MainPMActivity));
             tab_users = AddTab("users", "Users", Resource.Drawable.ic_tab_artists_grey, typeof(MainUsersActivity));
             tab_log = AddTab("log", "Log", Resource.Drawable.ic_tab_artists_grey, typeof(LogActivity));
             tab_settings = AddTab("settings", "Settings", Resource.Drawable.ic_tab_artists_grey, typeof(MainSettingsActivity));

             TabHost.CurrentTab = 1;
             TabHost.TabChanged += TabHost_TabChanged;
             //TabWidget
         }

         public void InitializeClientEngine()
         {
             ClientEngineManager = new AndroidClientEngine();
             ClientEngineManager.ProcessReceivedMessage += ClientEngine_ProcessReceivedMessage;
             //ClientEngine.XlinkDebugMessage += ClientEngine_XlinkDebugMessage;
             ClientEngineManager.Start();

             //Lanzamos los canales
             ClientEngineProcessManager = new AndroidClientEngineProcess(ClientEngineManager);
             ClientEngineProcessManager.ClientDiscover();

         }

         void TabHost_TabChanged(object sender, TabHost.TabChangeEventArgs e)
         {
             if (e.TabId == "clouds")
                 if (CloudsActivity != null)
                     CloudsActivity.RefreshCount();
                 else if (e.TabId == "users")
                 {
                     if (UsersActivity != null)
                         UsersActivity.Refresh();
                 }
                 else if (e.TabId == "log")
                 {
                     //if (LogActivity != null)
                     //    LogActivity.ExecuteLogCache();
                 }
             //AlertMessage.ShowAlert(this,e.TabId);
         }

         #region ClientEngine

         public AndroidClientEngine GetClientEngine()
         {
             return ClientEngineManager;
         }

         public AndroidClientEngineProcess GetClientEngineProcess()
         {
             return ClientEngineProcessManager;
         }

      

         void ClientEngine_ProcessReceivedMessage(xlink_msg msg)
         {
             if (msg != null)
             {

                 switch (msg.msg_type)
                 {

                     case xlink_msg.xbs_xlink_message_type.SERVER_ACCEPT:

                         SetMasterState(EnvironmentEx.ConnectionState.CloudDisconnected);

                         EnvironmentEx.InsertLog("xlink_msg.xbs_xlink_message_type.SERVER_ACCEPT");

                         break;
                     case xlink_msg.xbs_xlink_message_type.SERVER_CHAT_MY_MESSAGE:

                         //xlink_server_my_chat_message chat_my_message = new xlink_server_my_chat_message(msg);
                         //  ChatActivity.AppendMyText(chat_my_message._message);
                         //  EnvironmentEx.InsertChat(new ChatItem(chat_my_message));


                         break;
                     case xlink_msg.xbs_xlink_message_type.SERVER_CHAT_USER_MESSAGE:

                            //xlink_server_user_chat_message chat_message = new xlink_server_user_chat_message(msg);
                            //ChatActivity.AppendLineUsers(chat_message._username, chat_message._message);

                            //EnvironmentEx.InsertChat(new ChatItem(chat_message));

                            //EnvironmentEx.InsertLog(chat_message.data_msg);
                         
                            break;
                   
                     case xlink_msg.xbs_xlink_message_type.SERVER_CLOUD_CONNECT:

                         break;
                     case xlink_msg.xbs_xlink_message_type.SERVER_CLOUD_REFRESH:
                         //Refrescamos la lista de servidores

                         break;
                     case xlink_msg.xbs_xlink_message_type.SERVER_DENIED:
                         SetMasterState(EnvironmentEx.ConnectionState.Disconnected);
                         break;
                     case xlink_msg.xbs_xlink_message_type.SERVER_GET_FAVORITES:
                         break;
                     case xlink_msg.xbs_xlink_message_type.SERVER_INFO:

                         xlink_server_info_message server_info_message = new xlink_server_info_message(msg);
                         EnvironmentEx.XBSLINK_SERVER_NAME = server_info_message._server_version;
                         EnvironmentEx.CLOUD_SERVER_IP = server_info_message._cloud_server_ip;
                         EnvironmentEx.CLOUD_SERVER_PORT = server_info_message._cloud_server_port;
                         EnvironmentEx.NICKNAME = server_info_message._nickname;
                         EnvironmentEx.XBSLINK_SERVER_VERSION = server_info_message._server_version;

                         if (SettingsActivity!=null)
                            SettingsActivity.RefreshServerInfoConfiguration();

                         EnvironmentEx.InsertLog(server_info_message.data_msg);

                         break;
                     case xlink_msg.xbs_xlink_message_type.SERVER_VOICE_CHAT_CREATE:
                         break;
                     case xlink_msg.xbs_xlink_message_type.SERVER_VOICE_CHAT_JOIN:
                         break;
                     case xlink_msg.xbs_xlink_message_type.SERVER_START:

                         SetMasterState(EnvironmentEx.ConnectionState.CloudConnected);

                         EnvironmentEx.InsertLog("SERVER_START");

                         break;
                     case xlink_msg.xbs_xlink_message_type.SERVER_STOP:

                         SetMasterState(EnvironmentEx.ConnectionState.Disconnected);

                         EnvironmentEx.InsertLog("SERVER_STOP");

                         break;

                     case xlink_msg.xbs_xlink_message_type.SERVER_ADD_CLOUD:

                         xlink_server_create_cloud_message cloud_added = new xlink_server_create_cloud_message(msg);
                         EnvironmentEx.CloudAdd(new CloudsItem(cloud_added._name,
                             cloud_added._node_count, 
                             cloud_added._max_nodes,
                             cloud_added._isPrivate));

                         if (CloudsActivity != null)
                             CloudsActivity.RefreshCount();

                         EnvironmentEx.InsertLog(cloud_added.data_msg);

                         //SetMasterState(EnvironmentEx.ConnectionState.Disconnected);

                         break;
                     case xlink_msg.xbs_xlink_message_type.SERVER_PM_MY:
                         xlink_server_my_pm_message my_pm = new xlink_server_my_pm_message(msg);
                         EnvironmentEx.PMAdd(my_pm._tousername);

                         EnvironmentEx.InsertLog(my_pm.data_msg);

                         break;
                     case xlink_msg.xbs_xlink_message_type.SERVER_PM_USER:
                         xlink_server_my_pm_message user_pm = new xlink_server_my_pm_message(msg);
                         EnvironmentEx.PMAdd(user_pm._tousername);
                         EnvironmentEx.InsertLog(user_pm.data_msg);

                         break;

                     case xlink_msg.xbs_xlink_message_type.SERVER_USER_ONLINE:
                         xlink_server_user_online_message user_online = new xlink_server_user_online_message(msg);

                         EnvironmentEx.UserAdd (user_online);

                         if (UsersActivity != null)
                             UsersActivity.Refresh();

                         EnvironmentEx.InsertLog(user_online.data_msg);

                         break;

                     case xlink_msg.xbs_xlink_message_type.SERVER_USER_OFFLINE:

                         xlink_server_user_offline_message user_offline = new xlink_server_user_offline_message(msg);
                         EnvironmentEx.UserDelete(user_offline._username);

                         if (UsersActivity != null)
                             UsersActivity.Refresh();

                         EnvironmentEx.InsertLog(user_offline.data_msg);

                         break;

                     default:
                         break;
                 }

             }
         }

         #endregion

         #region ClientEngineActions

         void CloudListViewFill(xbs_cloud[] data)
         {
             if (data.Length > 0)
             {
                 SetDefaultTab(0);
                 MainCloudsActivity mn = (MainCloudsActivity) GetActivityByTabTag ("clouds");
                 mn.ClearGrid();
                 foreach (var item in data)
                    EnvironmentEx.CloudAdd(new CloudsItem(item));
             }

         }

         #endregion

         protected override void OnResume()
         {
             base.OnResume();
             //RegisterReceiver(broadcastReceiver,
             //    new IntentFilter(ACTION_NEW_TWEETS));
         }
         protected override void OnPause()
         {
             UnregisterReceiver(broadcastReceiver);
             base.OnPause();
         }


         public class ActivityBroadcastReceiver : BroadcastReceiver
         {
             public event Action<Context, Intent> Receive;
             public override void OnReceive(Context context, Intent intent)
             {
                 if (this.Receive != null)
                     this.Receive(context, intent);
             }
         }


     }
}