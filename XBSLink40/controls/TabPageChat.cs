using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XBSlink.controls;
using XBSlink;
using System.Drawing;


    public class TabPageChat: TabPage
    {

        ChatPrivateWindow _chat;

        public void AddMessage(xbs_node_message_msgpm message, bool IsSended)
        {
            if (_chat != null)
            {
                SetNewMessage(true);
                _chat.AddMessage(message, IsSended);
            }

        }

       public void SetNewMessage(bool valor) {
           this.ForeColor = (valor) ?  Color.Red : Color.Black ;
        }

        public string CreateChatUser(string UserName)
        {
                this.Name = UserName;
                this.Text = UserName;
                _chat = new ChatPrivateWindow();

                this.Controls.Add(_chat);
            //this.listBox_messages.Dock = System.Windows.Forms.DockStyle.Fill;
                _chat.Dock = System.Windows.Forms.DockStyle.Fill;

            return "";
        }

    }

