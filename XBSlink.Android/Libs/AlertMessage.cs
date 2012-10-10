using System;
using System.Collections.Generic;
using System.Text;
using Android.Content;
using Android.Widget;

namespace XBSlink.Android
{
    class AlertMessage
    {

        public static void ShowAlert(Context context, string message)
        {
            Toast.MakeText(context, message, ToastLength.Short).Show();
        }

    }
}
