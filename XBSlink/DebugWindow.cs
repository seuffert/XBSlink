/**
 * Project: XBSlink: A XBox360 & PS3/2 System Link Proxy
 * File name: DebugWindow.cs
 *   
 * @author Oliver Seuffert, Copyright (C) 2011.
 */
/* 
 * XBSlink is free software; you can redistribute it and/or modify 
 * it under the terms of the GNU General Public License as published by 
 * the Free Software Foundation; either version 2 of the License, or 
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful, but 
 * WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
 * or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License 
 * for more details.
 * 
 * You should have received a copy of the GNU General Public License along 
 * with this program; If not, see <http://www.gnu.org/licenses/>
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace XBSlink
{
    public partial class DebugWindow : Form
    {
        private static DebugWindow form = null;
        private DateTime last_resize = DateTime.Now;

        public DebugWindow()
        {
            InitializeComponent();
            form = this;
        }

        private void DebugWindows_Load(object sender, EventArgs e)
        {
            listView_messages.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            timer1.Start();
        }

        private void add_text(String text)
        {
            ListViewItem lv_item = null;
            String text_extended = null;
            if (text.Length > 259)
            {
                lv_item = new ListViewItem(text.Substring(0,259));
                text_extended = text.Substring(259);
            }
            else
                lv_item = new ListViewItem(text);
            String message = text.Substring(11).Trim();
            if (message.StartsWith("!!"))
                lv_item.BackColor = Color.Firebrick;
            else if (message.StartsWith("%"))
                lv_item.BackColor = Color.YellowGreen;
            else if (message.StartsWith("*"))
                lv_item.BackColor = Color.WhiteSmoke;
            else if (message.StartsWith("x"))
                lv_item.BackColor = Color.SkyBlue;
            else if (message.StartsWith("-"))
                lv_item.BackColor = Color.Orange;
            else if (message.StartsWith("@"))
                lv_item.BackColor = Color.Khaki;
            else if (message.StartsWith("+"))
                lv_item.BackColor = Color.Aquamarine;
            else if (message.StartsWith("~"))
                lv_item.BackColor = Color.Teal;
            listView_messages.Items.Add(lv_item);
            if (text_extended != null)
                lv_item.SubItems.Add(text_extended);
            listView_messages.EnsureVisible(listView_messages.Items.Count - 1);
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            lock (DebugWindow.form)
                listView_messages.Items.Clear();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            String msg;
            if (xbs_messages.getDebugMessageCount() > 0)
            {
                listView_messages.BeginUpdate();
                while (xbs_messages.getDebugMessageCount() > 0)
                {
                    msg = xbs_messages.DequeueDebugMessageString();
                    add_text(msg);
                }
                resizeMessagesListViewHeaders();
                listView_messages.EndUpdate();
            }
        }

        private void DebugWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (timer1!=null)
                if (timer1.Enabled)
                    timer1.Stop();
            DebugWindow.form = null;
        }

        private void DebugWindow_SizeChanged(object sender, EventArgs e)
        {
            
        }

        private void resizeMessagesListViewHeaders()
        {
            try
            {
                listView_messages.BeginUpdate();
                //columnHeader_message.Width = listView_messages.ClientSize.Width - 2;
                columnHeader_message.Width = -1;
                columnHeader_message2.Width = -1;
                listView_messages.Refresh();
                listView_messages.EndUpdate();
            }
            catch (Exception)
            {
            }
        }

        private void listView_Resize(object sender, EventArgs e)
        {
            if ((DateTime.Now - last_resize).TotalMilliseconds < 100)
                return;
            last_resize = DateTime.Now;
            resizeMessagesListViewHeaders();
        }
    }
}
