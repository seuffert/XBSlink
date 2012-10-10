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
using Java.Net;

namespace XBSlink.Android
{
     [Activity(Label = "XBSLink for Android", Icon = "@drawable/icon")]
    public class TestActivity : Activity
    {

        string server_ip = "10.67.2.1";
        int server_port = 34522;
         DatagramSocket s;

         void SendMessage(string message)
         {
             
             AndroidClientEngine.SendMessage(s, server_ip, server_port,message);
         }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource

            SetContentView(Resource.Layout.TestButtons);
            
            // Create your application here
            s = new DatagramSocket(server_port);
            
            EditText txtChatMessage = FindViewById<EditText>(Resource.Id.txtChatMessage);
            EditText txtCloud = FindViewById<EditText>(Resource.Id.txtCloud);

            Button btnConnectCloud = FindViewById<Button>(Resource.Id.btnConnectCloud);
            btnConnectCloud.Click += delegate
            {
                SendMessage("CLIENT_CLOUD_JOIN;" + txtCloud.Text + ";" );
            };

            Button btnGetClouds = FindViewById<Button>(Resource.Id.btnGetClouds);
            btnGetClouds.Click += delegate
            {
                SendMessage("CLIENT_CLOUDS_GET;");
            };

            Button btnLeaveCloud = FindViewById<Button>(Resource.Id.btnLeaveCloud);
            btnLeaveCloud.Click += delegate
            {
                SendMessage("CLIENT_CLOUD_LEAVE;");
            };

            Button btnSendChat = FindViewById<Button>(Resource.Id.btnSendChat);
            btnSendChat.Click += delegate
            {
                SendMessage("CLIENT_SEND_CHAT_MESSAGE;" + txtChatMessage.Text);
            };

            Button btnSendPM = FindViewById<Button>(Resource.Id.btnSendPM);
            btnSendPM.Click += delegate
            {
                SendMessage("CLIENT_SEND_PM;magurin;" + txtChatMessage.Text);
            };

            Button btnStart = FindViewById<Button>(Resource.Id.btnStart);
            btnStart.Click += delegate
            {
                SendMessage("CLIENT_START;");
            };

            Button btnStop = FindViewById<Button>(Resource.Id.btnStop);
            btnStop.Click += delegate
            {
                SendMessage("CLIENT_STOP;");
            };

            Button btnExit = FindViewById<Button>(Resource.Id.btnExit);
            btnExit.Click += delegate
            {
                System.Environment.Exit(0);
            };

        }
    }
}