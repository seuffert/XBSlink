using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using XBSlink.Android.Managers;


namespace XBSlink.Android
{
   public  class MasterTabContainer : TabActivity
    {
       IMenuItem menu_start_engine;

       public static TabHost.TabSpec tab_clouds;
       public static TabHost.TabSpec tab_chat;
       public static TabHost.TabSpec tab_pm;
       public static TabHost.TabSpec tab_users;
       public static TabHost.TabSpec tab_settings;
       public static TabHost.TabSpec tab_log;

       public MainCloudsActivity CloudsActivity;
       public MainChatActivity ChatActivity;
       public MainPMActivity PMActivity;
       public MainUsersActivity UsersActivity;
       public MainSettingsActivity SettingsActivity;
       public LogActivity LogActivity;

       public AudioManager audioManager;
       public LogManager logManager;

       public AudioManager GetAudioManagement()
       {
           return audioManager;
       }
   
       protected override void OnCreate(Bundle savedInstanceState)
       {
           Window.AddFlags(WindowManagerFlags.KeepScreenOn);
           //addFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);

           logManager = new LogManager();
           audioManager = new AudioManager(this);

           base.OnCreate(savedInstanceState);
         
       }


       public void SetMasterState(EnvironmentEx.ConnectionState NewState)
       {

           if (CloudsActivity!=null)
            CloudsActivity.SetState(NewState);
       
           EnvironmentEx.APP_ACTUALSTATE = NewState;
       }


       public void ShowMessage(string message)
       {
           AlertMessage.ShowAlert(this, message);
       }


       public void AddLog(string texto)
       {
           var activity = LocalActivityManager.GetActivity("log");
       }

       #region Tabs

       public TabHost.TabSpec AddTab(string id, string description, int resourceId, Type activity)
       {
           // Initialize a TabSpec for each tab and add it to the TabHost
           TabHost.TabSpec tmp_tab_spec = TabHost.NewTabSpec(id);
           tmp_tab_spec.SetIndicator(description, Resources.GetDrawable(resourceId));
           // Create an Intent to launch an Activity for the tab (to be reused)
           Intent intent = new Intent(this, activity);
           intent.AddFlags(ActivityFlags.NewTask);
           tmp_tab_spec.SetContent(intent);
           TabHost.AddTab(tmp_tab_spec);
           return tmp_tab_spec;
       }

       #endregion

       #region Activity

       public Activity GetActualActivity()
       {
           return GetActivityByTabTag(TabHost.CurrentTabTag);
       }

       public Activity GetActivityByTabTag(string TabTag)
       {
           return LocalActivityManager.GetActivity(TabTag);
       }

       #endregion

       #region Menu

       public override bool OnCreateOptionsMenu(IMenu menu)
       {

           base.OnCreateOptionsMenu(menu);
           // Unique menu item Identifier. Used for event handling.
           menu_start_engine = menu.Add(1, 1, 0, "Connect to Engine");
           IMenuItem menuItem2 = menu.Add(1, 2, 1, "Show Log");
           IMenuItem menuItem3 = menu.Add(1, 3, 2, "About");
           IMenuItem menuItem4 = menu.Add(1, 4, 3, "Exit");
           return true;
       }

       public void RefrescandoServidores()
       {
           TabHost.CurrentTab = 0;

           EnvironmentEx._clouds.Clear();

           AndroidClientEngineProcess clientEngineProcess = (CurrentActivity.Parent as MainActivity).GetClientEngineProcess();
           clientEngineProcess.ClientCloudGet();

           Toast.MakeText(this, "Refresh clouds", ToastLength.Short).Show();
       }

       public override bool OnOptionsItemSelected(IMenuItem item)
       {
           //Toast.MakeText(this, item.TitleFormatted + " - " + item.ItemId.ToString(), ToastLength.Short).Show();

           switch (item.ItemId)
           {
               case (1):

                   RefrescandoServidores();

                   return (true);
               case (2):

                   //Intent log = new Intent(this.BaseContext, (Java.Lang.Class)new LogActivity().Class);
                   //log.AddFlags(ActivityFlags.NewTask);
                   //StartActivity(log);
                   if (CurrentActivity != null && CurrentActivity.Parent != null)
                   {
                       (CurrentActivity.Parent as MainActivity).AddLog("Tu padre");
                   }

                   //menu id 1 was selected
                   return (true);
               // additional items can go here.
               case (3):
                   //menu id 1 was selected

                   Intent about = new Intent(this.BaseContext, (Java.Lang.Class)new AboutActivity().Class);
                   about.AddFlags(ActivityFlags.NewTask);
                   StartActivity(about);

                   return (true);
               case (4):
                   CloseAll();
                   //Android.OS.Process.killProcess(android.os.Process.myPid());
                   //closeAllBelowActivities(this);
                   //Finish();
                   //System.Environment.Exit(0);
                   //menu id 1 was selected
                   return (true);
           }

           return base.OnOptionsItemSelected(item);
       }

       public override void OnOptionsMenuClosed(IMenu menu)
       {
           base.OnOptionsMenuClosed(menu);
       }

       #endregion

       #region ClosingApp

       public void CloseAll()
       {
           closeAllBelowActivities(LocalActivityManager.CurrentActivity);
           Process.KillProcess(Process.MyPid());

       }

       public static void closeAllBelowActivities(Activity actual)
       {
           Boolean flag = true;
           Activity parent;
           while (flag && actual != null)
           {
               parent = actual.Parent;
               try
               {
                   actual.Finish();
                   actual = parent;
               }
               catch (Exception)
               {
                   flag = false;
               }
           }
       }

       #endregion

    }

    }

