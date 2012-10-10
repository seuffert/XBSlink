using System;
using System.Collections.Generic;
using System.Text;

namespace XBSlink.Android
{
    public class ChatItem
    {
        public string _message { get; set; }
        public string _username { get; set; }
        public bool _is_my_message { get; set; }

        //public ChatItem(xlink_server_user_chat_message msg)
        //{
        //    _message = msg._message;
        //    _username = msg._username;
        //    _is_my_message = false;
        //}

        //public ChatItem(xlink_server_my_chat_message msg)
        //{
        //    _message = msg._message;
        //    _is_my_message = true;
        //}
    }
}
