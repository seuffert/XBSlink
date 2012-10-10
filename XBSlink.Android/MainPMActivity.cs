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
using XBSlink.Android.Grid;

namespace XBSlink.Android
{
    [Activity(Label = "PM : XBSLink")]
    public class MainPMActivity : MasterTab
    {
               
        ListView ListViewItems;

      

        void SetUsersCount()
        {
            txtPMCount1.Text = String.Format("PMs: {0}", EnvironmentEx._pms.Count);
        }


        void InitializeGrid()
        {
            ListViewItems = FindViewById<ListView>(Resource.Id.LstPMList);
            //GetTmpItems(tableItems);
            ListViewItems.Adapter = new PMListAdapter(this, EnvironmentEx._pms);
            ListViewItems.ItemClick += LsPM_ItemClick;
        }

        void LsPM_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            var t = EnvironmentEx._pms[e.Position];
            Toast.MakeText(this, t.Nombre, ToastLength.Short).Show();
            Console.WriteLine("Clicked on " + t.Nombre);
            _mainActivity.audioManager.PlayNewNode(this);
        }

        TextView txtPMCount1;

        void InitializeObjects()
        {
            txtPMCount1 = FindViewById<TextView>(Resource.Id.txtPMCount);
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.MainTabPM);

            _mainActivity.PMActivity = this;

            InitializeObjects();

            InitializeGrid();

            SetUsersCount();

           
        }


    }
}