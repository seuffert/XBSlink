using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.Views;
using Android.Widget;

namespace XBSlink.Android.Grid
{
    public class CloudsListAdapter : BaseAdapter
    {
        Activity context;

        public List<CloudsItem> items;

        public CloudsListAdapter(Activity context, List<CloudsItem> elements) //We need a context to inflate our row view from
            : base()
        {
            this.context = context;
            items = elements;
        }

        public override int Count
        {
            get { return items.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            var item = items[position];

            View view = convertView;
            if (view == null) // no view to re-use, create new
                view = context.LayoutInflater.Inflate(Resource.Layout.CloudView, null);

            view.FindViewById<TextView>(Resource.Id.CCloudViewTittle).Text = item.Name;
            view.FindViewById<TextView>(Resource.Id.CCloudViewDescription).Text = item.Description;
            view.FindViewById<ImageView>(Resource.Id.CCloudViewImage).SetImageResource(item.Image);

            //Finally return the view
            return view;
        }

        public CloudsItem GetItemAtPosition(int position)
        {
            return items[position];
        }
    }
}
