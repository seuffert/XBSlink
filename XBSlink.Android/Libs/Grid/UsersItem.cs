using System;
using System.Collections.Generic;
using System.Text;
using XBSlink.XlinkKai;

namespace XBSlink.Android.Grid
{

    public class UsersItem
        {

            public string _ip_announced { get; set; }
            public int _port_announced { get; set; }
            public string _ip_sendfrom { get; set; }
            public int _port_sendfrom { get; set; }

            
        
            public DateTime _addedTime { get; set; }
            public DateTime _lastChangeTime { get; set; }

            public string _nickname { get; set; }
            public string _client_version { get; set; }
            private string _last_ping_delay_ms { get; set; }
            

            public string _description
            {
                get;
                set;
            }

            public int _image
            {
                get;
                set;
            }

            public bool _is_friend { get; set; }


            public UsersItem(xlink_server_user_online_message message)
                : this(message._username, message._client_version, message._last_ping_delay_ms, false)
            {

            }

            public UsersItem(string nickname, string client_version, string last_ping_delay_ms, bool is_friend)
            {
                _nickname = nickname;
                SetData(client_version,last_ping_delay_ms,is_friend);
            }

            public void SetData(UsersItem item)
            {
                SetData(item._client_version,item._last_ping_delay_ms,item._is_friend);
               
            }

            public void SetData(xlink_server_user_online_message user)
            {
                SetData(user._client_version, user._last_ping_delay_ms, _is_friend);
            }


            public void SetData(string client_version, string last_ping_delay_ms, bool is_friend)
            {

                _client_version = client_version; _last_ping_delay_ms = last_ping_delay_ms; _is_friend = is_friend;

                _image = (is_friend) ? Resource.Drawable.icon_key : Resource.Drawable.Icon;
                SetDescription();
            }

            public void SetDescription() {
                _description = String.Format("{0} ({1})", _client_version , _last_ping_delay_ms, _is_friend );
            }

        }
    }

