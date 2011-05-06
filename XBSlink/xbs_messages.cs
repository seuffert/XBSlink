using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace XBSlink
{
    /*
    enum xbs_message_type : byte
    {
        GENERAL = 0x00,
        SNIFFER = 0x01,
        UDP_LISTENER = 0x02
    }
    */

    class xbs_message
    {
        public DateTime time_added;
        public String text;
        public String ThreadName = Thread.CurrentThread.Name;
        public int ThreadID = Thread.CurrentThread.ManagedThreadId;

        public xbs_message(String txt, DateTime time)
        {
            time_added = time;
            text = txt;
        }

        public xbs_message(String txt)
        {
            time_added = DateTime.Now;
            text = txt;
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

        private static void addMessage(String msg, Queue<xbs_message> queue)
        {
            lock (queue)
                queue.Enqueue(new xbs_message(msg));
        }
        public static void addInfoMessage(String msg)
        {
            addMessage(msg, messages);
#if DEBUG
            addDebugMessage(msg);
#endif
        }
        public static void addChatMessage(String msg)
        {
            addMessage(msg, chat_messages);
        }
        public static void addDebugMessage(String msg)
        {
#if DEBUG
            addMessage(msg, debug_messages);
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
