using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using XBSLink.Client.Android.Libs.DB;

namespace XBSlink.Android
{
    [Activity(Label = "Configuration : XBSLink")]
    public class MainSettingsActivity : MasterTab
    {
        DBCommandsConfiguration commands = null;

        CheckBox chkSettingsJoinLeave1;
        CheckBox chkSettingsPlayNewMessage1;
        EditText txtSettingsIPInternalServer1;

        EditText txtSettingsNickName;
        EditText txtSettingsCloudServerHost;
        EditText txtSettingsCloudServerPort;

        Button btnSettingsSave1;
        Button btnSettingsReconnect1;

        void InitializeConfiguration()
        {

            if (commands != null)
            {
                configuration tmp = commands.GetByParameter("play_sound_new_message");
                if (tmp != null)
                {
                    chkSettingsPlayNewMessage1.Checked = Boolean.Parse(tmp.value);
                }

                tmp = commands.GetByParameter("play_sound_join_leave");
                if (tmp != null)
                {
                    chkSettingsJoinLeave1.Checked = Boolean.Parse(tmp.value);
                }

                tmp = commands.GetByParameter("xbslink_internal_ip");
                if (tmp != null)
                {
                    txtSettingsIPInternalServer1.Text = tmp.value;
                }
            }
        }


        void SaveActualConfig()
        {
            commands.SetConfiguration(txtSettingsIPInternalServer1.Text, chkSettingsPlayNewMessage1.Checked, chkSettingsJoinLeave1.Checked);
        }


        public void RefreshServerInfoConfiguration()
        {
            try
            {
                txtSettingsNickName.Text = EnvironmentEx.NICKNAME;
                txtSettingsCloudServerHost.Text = EnvironmentEx.CLOUD_SERVER_IP;
                txtSettingsCloudServerPort.Text = EnvironmentEx.CLOUD_SERVER_PORT;
            }
            catch (Exception)
            {
                 RunOnUiThread(delegate
                             {
                                 txtSettingsNickName.Text = EnvironmentEx.NICKNAME;
                                 txtSettingsCloudServerHost.Text = EnvironmentEx.CLOUD_SERVER_IP;
                                 txtSettingsCloudServerPort.Text = EnvironmentEx.CLOUD_SERVER_PORT;
                             });
            }

        
        }

        void InitializeObjects()
        {

            txtSettingsNickName = FindViewById<EditText>(Resource.Id.txtSettingsNickName);
            txtSettingsCloudServerHost = FindViewById<EditText>(Resource.Id.txtSettingsCloudServerHost);
            txtSettingsCloudServerPort = FindViewById<EditText>(Resource.Id.txtSettingsCloudServerPort);

            txtSettingsIPInternalServer1 = FindViewById<EditText>(Resource.Id.txtSettingsIPInternalServer);
            chkSettingsJoinLeave1 = FindViewById<CheckBox>(Resource.Id.chkSettingsJoinLeave);
            chkSettingsPlayNewMessage1 = FindViewById<CheckBox>(Resource.Id.chkSettingsPlayNewMessage);

            btnSettingsSave1 = FindViewById<Button>(Resource.Id.btnSettingsSave);
            btnSettingsSave1.Click += delegate
            {
                SaveActualConfig();
                Toast.MakeText(this, "Settings saved", ToastLength.Short).Show();
            };

            btnSettingsReconnect1 = FindViewById<Button>(Resource.Id.btnSettingsReconnect);
            btnSettingsReconnect1.Click += delegate
            {
                //InitializeConfiguration();

                _mainActivity.ClientEngineProcessManager.ClientDiscover();

            };

            var delete = FindViewById<Button>(Resource.Id.btnSettingsDelete);
            btnSettingsReconnect1.Click += delegate
            {
                //InitializeConfiguration();
                //commands.DeleteAll();
            };


        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.MainTabSettings);

            _mainActivity.SettingsActivity = this;

            commands = new DBCommandsConfiguration(this);

            InitializeObjects();
            InitializeConfiguration();

            _mainActivity.ClientEngineProcessManager.ClientDiscover();
         
        }
    }
}