using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Java.Net;
using System.Text;
//using LumiSoft.Net.UDP;
using Android.Net.Wifi;
using Android.Net;
using Java.IO;
using Android.Graphics.Drawables;
using System.Threading;

namespace XBSlink.Android
{
    [Activity(Label = "XBSLink for Android", Icon = "@drawable/icon")]
    public class Test2Activity : MasterTabContainer
    {
        Thread oThread;
        public static bool Finished = false;
        
        string server_ip = "10.67.2.1";
        int server_port = 34522;

        DatagramSocket s;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);


            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Test1);

            s = new DatagramSocket(server_port);

            TextView textView1 = FindViewById<TextView>(Resource.Id.textView1);

            EditText txtLog = FindViewById<EditText>(Resource.Id.txtLog);

            Button btnExit = FindViewById<Button>(Resource.Id.btnExit);
            btnExit.Click += delegate
            {
                //Finish();
                FinishFromChild(this);
            };

            // Get our button from the layout resource,
            // and attach an event to it
            Button btnConnect = FindViewById<Button>(Resource.Id.btnConnect);
            btnConnect.Click += delegate
            {
                AndroidClientEngine.SendMessage(s,server_ip, server_port, "KAI_CLIENT_GET_CLOUDS;");
            };
               
          
            //MyService service = new MyService();
            //Console.WriteLine("First Thread id: " + Thread.CurrentThread.ManagedThreadId);
            //TestThread eee = new TestThread(server_port);
            //oThread = new Thread(new ThreadStart(eee.Start));
            //oThread.Start();

            oThread = new Thread(() =>
            {

                byte[] message = new byte[1500];
                
                DatagramPacket p = new DatagramPacket(message, message.Length);

                while (!Test2Activity.Finished)
                {
                    try
                    {
                        s.Receive(p);
                        String text = new String(ByteArrayToStr(message).ToCharArray(), 0, p.Length);

                        this.RunOnUiThread(() => txtLog.Append("Se ha recibido." + System.Environment.NewLine));
                    }
                    catch (Exception)
                    {

                    }
                }
                s.Close();
            });

            oThread.Start();
         
        }


        public static string ByteArrayToStr(byte[] str)
        {
            //var buf = Encoding.Convert(Encoding.GetEncoding("iso-8859-1"), Encoding.UTF8, str);
            return Encoding.UTF8.GetString(str);
        }
     

    }

    public class TestThread
    {
        int Port;

        public TestThread(int server_port)
        {
            Port = server_port;
        }

        public static string ByteArrayToStr(byte[] str)
        {
            //var buf = Encoding.Convert(Encoding.GetEncoding("iso-8859-1"), Encoding.UTF8, str);
            return Encoding.UTF8.GetString(str);
        }

        public void Start()
        {
            String text;

            byte[] message = new byte[256];

            DatagramSocket s = new DatagramSocket(Port);
            DatagramPacket p;

            while (!Test2Activity.Finished)
            {
                try
                {
                    p = new DatagramPacket(message, message.Length);
                    s.Receive(p);
                    text = new String(ByteArrayToStr(message).ToCharArray(), 0, p.Length);
                }
                catch (Exception)
                {

                }
            }
            s.Close();
        }

    }

    public class MyService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
        public void StartCommand()
        {
            Console.WriteLine("New Thread id: " + System.Threading.Thread.CurrentThread.ManagedThreadId);
        }
    }

}

