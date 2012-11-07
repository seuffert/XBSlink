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

  
        public string _username { get; set; }

        public ChatPrivateWindow(string username)
        {
            InitializeComponent();
            _username = username;
        }


        public void AddMessage(xbs_node_message_msgpm message, bool IsSended)
        {
             if (message != null)
                {
            Invoke((MethodInvoker)delegate
            {
               
                    textBox_chatMessages.Text += ((IsSended) ? xbs_settings.settings.REG_CHAT_NICKNAME : message.receiver.nickname) + " > " + message.message_string + Environment.NewLine;
                    textBox_chatMessages.SelectionStart = textBox_chatMessages.Text.Length;
                    textBox_chatMessages.ScrollToCaret();
               
            });
                }
        }

       public void UserNotInChannelSystemMessage()
        {

            Invoke((MethodInvoker)delegate
            {
                    textBox_chatMessages.Text += "The user " + _username + " is not in your channel." + Environment.NewLine;
                    textBox_chatMessages.SelectionStart = textBox_chatMessages.Text.Length;
                    textBox_chatMessages.ScrollToCaret();
            });

        }


    }
}
