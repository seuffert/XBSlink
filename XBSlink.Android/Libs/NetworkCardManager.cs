using System;
using System.Collections.Generic;
using System.Text;
using Android.Content;
using Android.Net;
using Android.Net.Wifi;
using Java.Net;

namespace XBSlink.Android.Managers
{
   public class NetworkCardManager
    {

        WifiInfo GetWifiInfo(Context actualContext)
        {
            WifiManager wifiManager = (WifiManager)actualContext.GetSystemService(Context.WifiService);
            return wifiManager.ConnectionInfo;
        }

       public static bool IsWifi(Context actualContext)
        {
            ConnectivityManager connectivityManager = (ConnectivityManager)actualContext.GetSystemService(Context.ConnectivityService);
            var mobileState = connectivityManager.GetNetworkInfo(ConnectivityType.Wifi).GetState();
            return (mobileState == NetworkInfo.State.Connected);
        }

       public static bool Is3g(Context actualContext)
        {
            ConnectivityManager connectivityManager = (ConnectivityManager)actualContext.GetSystemService(Context.ConnectivityService);
            var activeConnection = connectivityManager.ActiveNetworkInfo;
            return ((activeConnection != null) && activeConnection.IsConnected);
        }

       public string GetExternalIpUsingTcpClient()
       {
           var ipBytes = Get3GIPEndPoint().Address.GetAddressBytes();
           return GetIpFromIpArray(ipBytes);
       }

       public string GetExternalIpUsingSocketClient()
       {
           try
           {
               //create a new client socket ...
               System.Net.Sockets.Socket vb_Socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork,
                   System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
               System.Net.IPAddress remoteIPAddress = System.Net.Dns.GetHostAddresses("www.google.com")[0];
               System.Net.IPEndPoint remoteEndPoint = new System.Net.IPEndPoint(remoteIPAddress, 80);
               vb_Socket.Connect(remoteEndPoint);
               var ipBytes = ((System.Net.IPEndPoint)vb_Socket.LocalEndPoint).Address.GetAddressBytes();
               return GetIpFromIpArray(ipBytes);
           }
           catch (System.Net.Sockets.SocketException)
           {
           }
           return "";
       }

       public bool Ping(string Ip)
       {
           try
           {
               InetAddress element = InetAddress.GetByName(Ip);

               if (element != null)
                   return (element.IsReachable(5000));
           }
           catch (Exception)
           {
           }

           return false;
       }

       #region IpAddress Operations

       public static string ConvertToIpAddress(int ip)
       {
           return string.Format("{0:D}.{1:D}.{2:D}.{3:D}", (ip & 0xff), ((ip >> 8) & 0xff), ((ip >> 16) & 0xff), ((ip >> 24) & 0xff));
       }

       string ConvertToIpAddress(long ip)
       {
           return ConvertToIpAddress(Convert.ToInt32(ip));
       }

       public System.Net.IPEndPoint Get3GIPEndPoint()
       {
           System.Net.Sockets.TcpClient sock = new System.Net.Sockets.TcpClient();
           sock.Connect(System.Net.Dns.GetHostAddresses("www.google.com"), 80);
           System.Net.IPEndPoint dev = ((System.Net.IPEndPoint)sock.Client.LocalEndPoint);
           sock.Close();
           return dev;
       }

       public string GetIpFromIpArray(byte[] ipBytes)
       {
           var ip = (uint)ipBytes[3] << 24;
           ip += (uint)ipBytes[2] << 16;
           ip += (uint)ipBytes[1] << 8;
           ip += (uint)ipBytes[0];
           return ConvertToIpAddress((int)ip);
       }

      

       #endregion

        /*
        *   public void Select()
        {
            if (IsWifi())
            {
                var info = GetWifiInfo();
                string ip = ConvertToIpAddress(info.IpAddress);
                txt.Text = String.Format("WIFI: {0} SSID: {1} IP:{2}", info.MacAddress, info.SSID, ip);
            }
            else if (Is3g())
            {
                System.Net.IPEndPoint ip = Get3GIPEndPoint();
                txt.Text = String.Format("3G: External IP:{0}", GetIpFromIpArray(ip.Address.GetAddressBytes()));
            }
            else
            {
                txt.Text = "No dispone de conexión a la red";
            }
        }
        * */

    }
}
