using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.Views;
using Android.Widget;

namespace XBSlink.Android.Grid
{
    public class PMListAdapter : BaseAdapter
    {
        Activity context;

        public List<PMItem> items;

        public PMListAdapter(Activity context, List<PMItem> elements) //We need a context to inflate our row view from
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
                view = context.LayoutInflater.Inflate(Resource.Layout.PMView, null);
             view.FindViewById<TextView>(Resource.Id.PMViewTittle).Text = item.Nombre;
             view.FindViewById<TextView>(Resource.Id.PMViewDescription).Text = item.Asunto;
             view.FindViewById<ImageView>(Resource.Id.PMViewImage).SetImageResource(item.Image);

            //Finally return the view
            return view;
        }

        public PMItem GetItemAtPosition(int position)
        {
            return items[position];
        }
    }
}
