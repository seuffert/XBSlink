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


        public delegate void AddMessageCallback(xbs_message message);


        public void AddMessage(xbs_message message)
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
                    textBox_chatMessages.Text += message.ToString();
                    textBox_chatMessages.SelectionStart = textBox_chatMessages.Text.Length;
                    textBox_chatMessages.ScrollToCaret();
                }
            }

        }

    }
}
