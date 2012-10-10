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
    [Activity(Label = "Users : XBSLink")]
    public class MainUsersActivity : MasterTab
    {

        //Users Online: 0
        
        ListView ListViewItems;
        TextView txtUsersCount;

        //public void GetTmpItems(List<UsersItem> dev)
        //{
        //    dev.Add(new UsersItem() { Nombre = "Magurin", Asunto = "65 items", Image = Resource.Drawable.Icon });
        //    dev.Add(new UsersItem() { Nombre = "Cosme", Asunto = "17 items", Image = Resource.Drawable.Icon });
        //    dev.Add(new UsersItem() { Nombre = "Leisur", Asunto = "5 items", Image = Resource.Drawable.Icon });
        //    dev.Add(new UsersItem() { Nombre = "Develop", Asunto = "33 items", Image = Resource.Drawable.Icon });
        //    dev.Add(new UsersItem() { Nombre = "Studios", Asunto = "18 items", Image = Resource.Drawable.Icon });
        //    dev.Add(new UsersItem() { Nombre = "Games", Asunto = "43 items", Image = Resource.Drawable.Icon });
        //    dev.Add(new UsersItem() { Nombre = "Finally", Asunto = "43 items", Image = Resource.Drawable.Icon });
        //    dev.Add(new UsersItem() { Nombre = "Magurin", Asunto = "65 items", Image = Resource.Drawable.Icon });
        //    dev.Add(new UsersItem() { Nombre = "Cosme", Asunto = "17 items", Image = Resource.Drawable.Icon });
        //    dev.Add(new UsersItem() { Nombre = "Leisur", Asunto = "5 items", Image = Resource.Drawable.Icon });
        //    dev.Add(new UsersItem() { Nombre = "Develop", Asunto = "33 items", Image = Resource.Drawable.Icon });
        //    dev.Add(new UsersItem() { Nombre = "Studios", Asunto = "18 items", Image = Resource.Drawable.Icon });
        //    dev.Add(new UsersItem() { Nombre = "Games", Asunto = "43 items", Image = Resource.Drawable.Icon });
        //    dev.Add(new UsersItem() { Nombre = "Finally", Asunto = "43 items", Image = Resource.Drawable.Icon });
        //}

        void InitializeGrid()
        {
            ListViewItems = FindViewById<ListView>(Resource.Id.LstUsersList);
            //GetTmpItems(tableItems);
            ListViewItems.Adapter = new UsersListAdapter(this, EnvironmentEx._users);
            ListViewItems.ItemClick += LsClouds_ItemClick;
            SetUsersCount();
        }

        public void Refresh()
        {
            try
            {
                ListViewItems.RefreshDrawableState();
                SetUsersCount();
            }
            catch (Exception)
            {
             RunOnUiThread(delegate
                             {
                                 ListViewItems.RefreshDrawableState();
                                 SetUsersCount();
                             });
            }

            
        }

        void LsClouds_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            var t = EnvironmentEx._users[e.Position];
            Toast.MakeText(this, t._nickname, ToastLength.Short).Show();
            Console.WriteLine("Clicked on " + t._nickname);
        }


        void SetUsersCount()
        {
            txtUsersCount.Text = String.Format("Users Online: {0}", EnvironmentEx._users.Count);
        }


        void InitializeObjects()
        {
            txtUsersCount = FindViewById<TextView>(Resource.Id.txtUsersCount);
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.MainTabUsers);

            InitializeObjects();

            InitializeGrid();


           
        }
    }
}