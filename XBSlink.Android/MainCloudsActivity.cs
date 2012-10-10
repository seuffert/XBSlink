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
    [Activity(Label = "Clouds : XBSLink")]
    public class MainCloudsActivity : MasterTab
    {

       
        ListView LsClouds;
        TextView txtCloudsCount;
        Button btnCloudsRefresh;

        EditText txtCloudsName;
        EditText txtCloudsMax;
        EditText txtCloudsPassword;
        Button   btnCloudsAdd;
        //Button   btnCloudsConnect;

        void InitializeObjects()
        {
            txtCloudsCount = FindViewById<TextView>(Resource.Id.txtCloudsCount);

            txtCloudsName = FindViewById<EditText>(Resource.Id.txtCloudsName);
            txtCloudsMax = FindViewById<EditText>(Resource.Id.txtCloudsMax);
            txtCloudsPassword = FindViewById<EditText>(Resource.Id.txtCloudsPassword);

            btnCloudsAdd = FindViewById<Button>(Resource.Id.btnCloudsAdd);
            btnCloudsAdd.Click += btnCloudsAdd_Click;
            //btnCloudsConnect = FindViewById<Button>(Resource.Id.btnCloudsConnect);
            //btnCloudsConnect.Click += btnCloudsConnect_Click;
            btnCloudsRefresh = FindViewById<Button>(Resource.Id.btnCloudsRefresh);
            btnCloudsRefresh.Click += btnCloudsRefresh_Click;
        }

        void InitializeGrid()
        {
            LsClouds = FindViewById<ListView>(Resource.Id.LstCloudsList);
            LsClouds.Adapter = new CloudsListAdapter(this, EnvironmentEx._clouds);
            LsClouds.ItemClick += LsClouds_ItemClick;
        }

        void SetCloudInfo(string CloudName, string Max, string Password)
        {
            txtCloudsName.Text = CloudName;
            txtCloudsMax.Text = Max;
            txtCloudsPassword.Text = Password;
        }

        public void SetState(EnvironmentEx.ConnectionState newState)
        {
            switch (newState)
            {
                case EnvironmentEx.ConnectionState.Disconnected:

                     LsClouds.Enabled = false;

                     ClearGrid();

                     btnCloudsAdd.Text = "Add/Join Cloud";
                     //btnCloudsConnect.Text = "Join";

                     txtCloudsName.Enabled = false;
                     txtCloudsMax.Enabled = false;
                     txtCloudsPassword.Enabled = false;

                     //btnCloudsConnect.Enabled = false;
                     btnCloudsAdd.Enabled = false;
                     btnCloudsRefresh.Enabled = false;

                    break;
                case EnvironmentEx.ConnectionState.CloudDisconnected:

                     LsClouds.Enabled = true;

                     txtCloudsName.Enabled = true;
                     txtCloudsMax.Enabled = true;
                     txtCloudsPassword.Enabled = true;

                     btnCloudsAdd.Text = "Add/Join Cloud";
                     //btnCloudsConnect.Text = "Join";

                     //btnCloudsConnect.Enabled = true;
                     btnCloudsAdd.Enabled = true;
                     btnCloudsRefresh.Enabled = true;

                    break;
                case EnvironmentEx.ConnectionState.CloudConnected:

                    LsClouds.Enabled = true;


                      txtCloudsName.Enabled = false;
                     txtCloudsMax.Enabled = false;
                     txtCloudsPassword.Enabled = false;

                     btnCloudsAdd.Text = "Leave Cloud";
                     //btnCloudsConnect.Text = "";

                       // btnCloudsConnect.Enabled = false;
                        btnCloudsAdd.Enabled = true;
                     btnCloudsRefresh.Enabled = false;

                    break;
             
                default:
                    break;
            }

        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MainTabClouds);

            _mainActivity.CloudsActivity = this;

            InitializeObjects();
            InitializeGrid();

            SetMasterState(EnvironmentEx.ConnectionState.CloudDisconnected);
        }

        #region Grid

        public void ClearGrid()
        {
            EnvironmentEx._clouds.Clear();
        }

        void LsClouds_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            var t = EnvironmentEx._clouds[e.Position];
            SetCloudInfo(t.Name,  t.MaxNodes , ""  );
        }


        #endregion

        void JoinSelectedCloud()
        {
            int MaxUsers = 100;
            if (txtCloudsMax.Text != "")
                MaxUsers = int.Parse(txtCloudsMax.Text);

            _mainActivity.ClientEngineProcessManager.ClientCloudCreateJoin(
                txtCloudsName.Text,
                MaxUsers,
                txtCloudsPassword.Text);

            ShowMessage("Creating cloud " + txtCloudsName.Text + "...");

            SetMasterState(EnvironmentEx.ConnectionState.CloudConnected);
        }

        void btnCloudsAdd_Click(object sender, EventArgs e)
        {
            if (EnvironmentEx.APP_ACTUALSTATE == EnvironmentEx.ConnectionState.CloudDisconnected)
            {
                JoinSelectedCloud();
            }
           
            else if (EnvironmentEx.APP_ACTUALSTATE == EnvironmentEx.ConnectionState.CloudConnected)
            {
                _mainActivity.ClientEngineProcessManager.ClientCloudLeave();
                ShowMessage("Leaving cloud...");
                SetMasterState(EnvironmentEx.ConnectionState.CloudDisconnected);
            }

        }

        //void btnCloudsConnect_Click(object sender, EventArgs e)
        //{
        //    if (EnvironmentEx.APP_ACTUALSTATE == EnvironmentEx.ConnectionState.CloudInserting)
        //    {
        //        SetMasterState(EnvironmentEx.ConnectionState.CloudDisconnected);
        //    }
        //    else if (EnvironmentEx.APP_ACTUALSTATE == EnvironmentEx.ConnectionState.CloudDisconnected)
        //    {
        //        JoinSelectedCloud();
        //    }

        //}

        void btnCloudsRefresh_Click(object sender, EventArgs e)
        {
          
            _mainActivity.RefrescandoServidores();
            //this.SetContentView(
        }

        public void RefreshCount()
        {

            try
            {
                LsClouds.RefreshDrawableState();
                txtCloudsCount.Text = String.Format("Clouds: {0}", EnvironmentEx._clouds.Count);
            }
            catch (Exception)
            {
                RunOnUiThread(delegate
                             {
                                 LsClouds.RefreshDrawableState();
                                 txtCloudsCount.Text = String.Format("Clouds: {0}", EnvironmentEx._clouds.Count);
                             });
            }

         

        }

        //public void GetTmpItems(List<CloudsItem> dev)
        //{
        //    dev.Add(new CloudsItem() { Nombre = "Vegetables", Asunto = "65 items", Image = Resource.Drawable.Icon });
        //    dev.Add(new CloudsItem() { Nombre = "Fruits", Asunto = "17 items", Image = Resource.Drawable.Icon });
        //    dev.Add(new CloudsItem() { Nombre = "Flower Buds", Asunto = "5 items", Image = Resource.Drawable.Icon });
        //    dev.Add(new CloudsItem() { Nombre = "Legumes", Asunto = "33 items", Image = Resource.Drawable.Icon });
        //    dev.Add(new CloudsItem() { Nombre = "Bulbs", Asunto = "18 items", Image = Resource.Drawable.Icon });
        //    dev.Add(new CloudsItem() { Nombre = "Tubers", Asunto = "43 items", Image = Resource.Drawable.Icon });
        //}
       
     
    }
}