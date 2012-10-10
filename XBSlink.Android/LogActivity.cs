using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace XBSlink.Android
{
    [Activity(Label = "Log : XBSLink for Android")]
    public class LogActivity : MasterTab
    {
        TextView txtLogGeneral1;
        ScrollView chat_ScrollView;

        public void AppendLog(string texto)
        {
                                
            txtLogGeneral1.Append(texto + System.Environment.NewLine);
            chat_ScrollView.Post(new MyRunnable(chat_ScrollView));
        }

        Thread LogThread;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.Log);

            _mainActivity.LogActivity = this;

            chat_ScrollView = (ScrollView)FindViewById(Resource.Id.scrollLogGeneral);
            txtLogGeneral1 = (TextView)chat_ScrollView.FindViewById(Resource.Id.txtLogGeneral);

            LogThread = new Thread(() =>
            {
                while (!EnvironmentEx.SHUTDOWN)
                {
                    if (EnvironmentEx._log_cache.Count > 0)
                    {
                        lock (EnvironmentEx._log_cache)
                        {

                            List<string> tmp = new List<string>();
                            foreach (var item in EnvironmentEx._log_cache)
		                        tmp.Add(item);

                            RunOnUiThread(delegate
                            {
                                foreach (var item in tmp)
                                {
                                    AppendLog(item);
                                }
                            });

                            EnvironmentEx._log_cache.Clear();

                        }
                    }
                }
            });
            LogThread.Priority = System.Threading.ThreadPriority.Normal;
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

    }
}