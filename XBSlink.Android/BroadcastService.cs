using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.Content;

namespace XBSlink.Android
{
        [Service]
    public class BroadcastService : IntentService
    {
        public const string ACTION_NEW_TWEETS = "action.NEW_TWEETS";
        public long LastSinceId { get; set; }
        public BroadcastService() : base()
        {
            this.LastSinceId = 0;
        }
        protected override void OnHandleIntent(Intent intent)
        {
            var lastSinceId = this.LastSinceId;
            //var tweets = Search.SearchTweets(lastSinceId, “#MonoDroid”);
            //this.LastSinceId = tweets.Max(t => t.Id);
            //if (tweets.Exists(t => t.Id > lastSinceId))
            //{
            //    var newTweetsIntent = new Intent(ACTION_NEW_TWEETS);
            //    newTweetsIntent.PutExtra("oldSinceId", lastSinceId);
            //    SendBroadcast(newTweetsIntent);
            //}
        }
    }
}
