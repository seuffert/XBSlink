using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XBSlink.controls;


    public class TabPageChat: TabPage
    {

        public string CreateChatUser(string UserName)
        {

          //  TabPage tb = new TabPage(User);
                this.Name = UserName;
                this.Text = UserName;
                this.Container.Add(new ChatPrivateWindow()
                {

                });

            return "";
        }

    }

