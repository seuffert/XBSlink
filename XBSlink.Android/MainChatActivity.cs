using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Text.Method;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace XBSlink.Android
{
    [Activity(Label = "Chat : XBSLink")]
    public class MainChatActivity : MasterTab
    {

        TextView txtChatGeneral;
        EditText txtChatMessage;
        ScrollView chat_ScrollView;

        string chatText ="";

        public bool isTime = false;

        System.Threading.Thread LogThread;

        public void AppendLineSystem(string text)
        {
            AppendLine(text, true, Color.Green);
        }

       public  void AppendLineUsers(string UserName, string text)
        {
            AppendLine(System.String.Format("[{0}] >> {1}", UserName, text), Color.Red);
        }

       public void AppendMyText(string text)
        {
            AppendLine(text,true, Color.Black);
        }


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MainTabChat);

            _mainActivity.ChatActivity = this;

            chatText = "";
            chat_ScrollView = (ScrollView)FindViewById(Resource.Id.chat_ScrollView);
            txtChatGeneral = (TextView)chat_ScrollView.FindViewById(Resource.Id.txtChatGeneral);
            txtChatMessage = (EditText)FindViewById(Resource.Id.txtChatMessage);

            AppendLineSystem(System.String.Format("Welcome to XBSLink chat.{0}=================={0}{0}{0}", System.Environment.NewLine));
            //AppendLineUsers("magurin", "Pues el otro dia me fui de guarrillas!");

            Button btnChatSend = FindViewById<Button>(Resource.Id.btnChatSend);
            btnChatSend.Click += delegate
            {
                AppendMyText(txtChatMessage.Text);
                chat_ScrollView.Post(new MyRunnable(chat_ScrollView));

                _mainActivity.ClientEngineProcessManager.ClientSendChatMessage(txtChatMessage.Text);

                txtChatMessage.Text = "";
            };

            LogThread = new System.Threading.Thread(() =>
            {
                while (!EnvironmentEx.SHUTDOWN)
                {
                  
                        lock (EnvironmentEx._chats_cache)
                        {
                            if (EnvironmentEx._chats_cache.Count > 0)
                            {
                            List<ChatItem> tmp = new List<ChatItem>();
                            foreach (var item in EnvironmentEx._chats_cache)
                                tmp.Add(item);

                         
                                foreach (var item in tmp)
                                {
                                    if (item._is_my_message)
                                        AppendMyText(item._message);
                                    else
                                        AppendLineUsers(item._username, item._message); //AppendLog(item);
                                }
                         
                            EnvironmentEx._chats_cache.Clear();

                        }
                    }
                }
            });
            LogThread.Start();

        }

        class MyRunnable : Java.Lang.Object, Java.Lang.IRunnable
        {

            ScrollView tmp_ScrollView;

            public MyRunnable(ScrollView ChatScrollView)
            {
                tmp_ScrollView = ChatScrollView;
            }

            public void Run() 
    {
        tmp_ScrollView.FullScroll(FocusSearchDirection.Down);
    }
        }

        public class UpdateUI : Java.Lang.Thread
    {
        TextView tv;
        string upd;
        public UpdateUI(TextView tv, string text)
        {
            this.tv = tv;
            this.upd = text;
        }
        public override void Run()
        {
            tv.Text = tv.Text + "\r\n" + upd;
            tv.Invalidate();

        }
}

        #region Append Basic


        void AppendLine(string text, bool IsBold)
        {
            AppendLine(text, IsBold, Color.Black);
        }

        public class SpanConfigs
        {
            public Java.Lang.Object what { get; set; }
            public int start { get; set; }
            public int end { get; set; }
            public SpanTypes flags { get; set; }
        }

        List<SpanConfigs> spanArray = new List<SpanConfigs>();

        void AppendLine(string text, bool IsBold, Color color)
        {
            string temporal_string = chatText + System.Environment.NewLine + text;
            SpannableString ss = new SpannableString(temporal_string);

            if (color != Color.Black)
            {
                spanArray.Add(new SpanConfigs()
                {
                    start = temporal_string.Length - text.Length,
                    end = temporal_string.Length,
                    flags = SpanTypes.ExclusiveExclusive,
                    what = new ForegroundColorSpan(color)
                });
            }

            if (IsBold)
            {
                spanArray.Add(new SpanConfigs()
                {
                    start = temporal_string.Length - text.Length,
                    end = temporal_string.Length,
                    flags = SpanTypes.ExclusiveExclusive,
                    what = new StyleSpan(TypefaceStyle.Bold)
                });
            }

            foreach (var item in spanArray)
            {
                ss.SetSpan(item.what, item.start, item.end, item.flags);
            }

            RunOnUiThread(delegate
            {
                txtChatGeneral.TextFormatted = ss;
                txtChatGeneral.MovementMethod = LinkMovementMethod.Instance;
            });

            chatText = temporal_string;
          
        }

        void AppendLine(string text, Color color)
        {
            AppendLine(text, false, color);
        }

        void AppendLine(string text)
        {
            AppendLine(text, false);
        }
      

        #endregion



    }
}