using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.Views;
using Android.Widget;

namespace XBSlink.Android.Grid
{
    public class UsersListAdapter : BaseAdapter
    {
        Activity context;

        public List<UsersItem> items;

        public UsersListAdapter(Activity context, List<UsersItem> elements) //We need a context to inflate our row view from
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
                view = context.LayoutInflater.Inflate(Resource.Layout.UsersView, null);
             view.FindViewById<TextView>(Resource.Id.UsersViewTittle).Text = item._nickname;
             view.FindViewById<TextView>(Resource.Id.UsersViewDescription).Text = item._description;
             view.FindViewById<ImageView>(Resource.Id.UsersViewImage).SetImageResource(item._image);

            //Finally return the view
            return view;
        }

        public UsersItem GetItemAtPosition(int position)
        {
            return items[position];
        }
    }
}
