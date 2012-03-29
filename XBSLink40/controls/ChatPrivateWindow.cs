using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XBSlink.controls
{
    public partial class ChatPrivateWindow : UserControl
    {
        public ChatPrivateWindow()
        {
            InitializeComponent();
        }


        public delegate void AddMessageCallback(xbs_node_message_msgpm message, bool IsSended);


        public void AddMessage(xbs_node_message_msgpm message, bool IsSended)
        {

            if (textBox_chatMessages.InvokeRequired)
            {
                AddMessageCallback d = new AddMessageCallback(AddMessage);
                this.Invoke(d, new object[] { message });
            }
            else
            {
                if (message != null)
                {


                    textBox_chatMessages.Text += ((IsSended) ? xbs_settings.settings.REG_CHAT_NICKNAME : message.receiver.nickname) + " > " + message.message_string + Environment.NewLine;
                    textBox_chatMessages.SelectionStart = textBox_chatMessages.Text.Length;
                    textBox_chatMessages.ScrollToCaret();
                }
            }

        }

    }
}
