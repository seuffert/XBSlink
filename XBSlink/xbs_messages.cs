using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace XBSlink
{
   public enum xbs_message_sender : byte
    {
        GENERAL                         = 0x00,
        SNIFFER                         = 0x01,
        UDP_LISTENER                    = 0x02,
        UPNP                            = 0x03,
        COMMANDLINE_MESSAGE_DISCPATCHER = 0x04,
        CLOUDLIST                       = 0x05,
        NAT                             = 0x06,
        NODE                            = 0x07,
        NODELIST                        = 0x08,
        X360 = 0x09,
        TRANSLATOR = 0x10
    }

    enum xbs_message_type : byte
    {
        GENERAL         = 0x00,
        WARNING         = 0x01,
        ERROR           = 0x02,
        FATAL_ERROR     = 0x03,
    }

    class xbs_message
    {
        public DateTime time_added;
        public String text;
        public String ThreadName = Thread.CurrentThread.Name;
        public int ThreadID = Thread.CurrentThread.ManagedThreadId;
        public xbs_message_type type;
        public xbs_message_sender sender;

        public xbs_message(String txt, DateTime time, xbs_message_sender msg_sender, xbs_message_type msg_type)
        {
            time_added = time;
            text = txt;
            type = msg_type;
            sender = msg_sender;
        }

        public xbs_message( String txt, xbs_message_sender sender, xbs_message_type type)
            : this( txt, DateTime.Now, sender, type )
        {
        }

        public override string ToString()
        {
            //return String.Format("{0:00}", msg.time_added.Hour) + ":" + String.Format("{0:00}", msg.time_added.Minute) + ":" + String.Format("{0:00}", msg.time_added.Second) + " : " + msg.text;
#if DEBUG
            return String.Format("[{4}] {0:00}:{1:00}:{2:00} : {3}", this.time_added.Hour, this.time_added.Minute, this.time_added.Second, this.text, this.ThreadID);
#else
            return String.Format("{0:00}:{1:00}:{2:00} : {3}", this.time_added.Hour, this.time_added.Minute, this.time_added.Second, this.text);
#endif
        }

    }

    class xbs_messages
    {
        private static Queue<xbs_message> messages = new Queue<xbs_message>();
        private static Queue<xbs_message> chat_messages = new Queue<xbs_message>();
        private static Queue<xbs_message> debug_messages = new Queue<xbs_message>();

        private static void addMessage(String msg, Queue<xbs_message> queue, xbs_message_sender sender, xbs_message_type type)
        {
            lock (queue)
                queue.Enqueue(new xbs_message(msg, sender, type));
        }
        public static void addInfoMessage(String msg, xbs_message_sender sender)
        {
            addInfoMessage(msg, sender, xbs_message_type.GENERAL);
        }
        public static void addInfoMessage(String msg, xbs_message_sender sender, xbs_message_type type)
        {
            addMessage(msg, messages, sender, type);
#if DEBUG
            addDebugMessage(msg, sender, type);
#endif
        }
        public static void addChatMessage(String msg)
        {
            addMessage(msg, chat_messages, xbs_message_sender.GENERAL, xbs_message_type.GENERAL);
        }
        public static void addDebugMessage(String msg, xbs_message_sender sender)
        {
            addDebugMessage(msg, sender, xbs_message_type.GENERAL);
        }
        public static void addDebugMessage(String msg, xbs_message_sender sender, xbs_message_type type)
        {
#if DEBUG
            addMessage(msg, debug_messages, sender, type);
#endif
        }

        private static int getMessageCount(Queue<xbs_message> queue)
        {
            lock (queue)
                return queue.Count;
        }
        public static int getInfoMessageCount()
        {
            return getMessageCount(messages);
        }
        public static int getChatMessageCount()
        {
            return getMessageCount(chat_messages);
        }
        public static int getDebugMessageCount()
        {
            return getMessageCount(debug_messages);
        }

        private static String DequeueMessageString(Queue<xbs_message> queue)
        {
            String str = null;
            xbs_message msg = DequeueMessage(queue);
            if (msg != null)
                str = msg.ToString();
            return str;
        }
        private static String[] DequeueMessageStringArray(Queue<xbs_message> queue)
        {
            xbs_message[] messages = DequeueMessageArray(queue);
            if (messages == null) 
                return null; 
            String[] str_array = new String[messages.Length];
            for (int i = 0; i < messages.Length; i++)
                str_array[i] = messages[i].ToString();
            return str_array;
        }

        public static String DequeueInfoMessageString()
        {
            return DequeueMessageString(messages);
        }
        public static String DequeueChatMessageString()
        {
            return DequeueMessageString(chat_messages);
        }
        public static String DequeueDebugMessageString()
        {
            return DequeueMessageString(debug_messages);
        }
        public static String[] DequeueDebugMessageStringArray()
        {
            return DequeueMessageStringArray(debug_messages);
        }

        private static xbs_message DequeueMessage(Queue<xbs_message> queue)
        {
            xbs_message msg = null;
            lock (queue)
                if (queue.Count > 0)
                    msg = queue.Dequeue();
            return msg;
        }
        private static xbs_message[] DequeueMessageArray(Queue<xbs_message> queue)
        {
            xbs_message[] messages = null;
            lock (queue)
                if (queue.Count > 0)
                {
                    messages = queue.ToArray();
                    queue.Clear();
                }
            return messages;
        }

        public static xbs_message DequeueInfoMessage()
        {
            return DequeueMessage(messages);
        }
        public static xbs_message DequeueChatMessage()
        {
            return DequeueMessage(chat_messages);
        }
        public static xbs_message DequeueDebugMessage()
        {
            return DequeueMessage(debug_messages);
        }

        public static xbs_message[] DequeueDebugMessageArray()
        {
            return DequeueMessageArray(debug_messages);
        }
    }
}
