using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.Content;

namespace XBSlink.Android
{
    public class MasterTab : Activity
    {


        public void SetMasterState(EnvironmentEx.ConnectionState NewState)
        {
            _mainActivity.SetMasterState(NewState);
        }


        public void ShowMessage(string message)
        {
           // AlertMessage.ShowAlert(this, message);
            _mainActivity = (MainActivity)getParentActivity(this);
        }


        public Context getParentActivity(Activity activity)
        {
            return activity.Parent;
        }


        //public MasterTabContainer _masterTabContainerActivity;
        public MainActivity _mainActivity;

        protected override void OnCreate(global::Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _mainActivity = (MainActivity)getParentActivity(this);
            //_masterTabContainerActivity = (MasterTabContainer)_mainActivity.BaseContext;
        }




        

    }
}
