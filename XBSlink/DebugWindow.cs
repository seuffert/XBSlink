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
        public Queue<String> messages = new Queue<String>();

        public DebugWindow()
        {
            InitializeComponent();
            form = this;
        }

        private void DebugWindows_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void add_text(String text)
        {
            listBox_messages.Items.Add(text);
            listBox_messages.TopIndex = listBox_messages.Items.Count - (listBox_messages.Height / listBox_messages.ItemHeight);
        }

        public static void addMessage(String str)
        {
            if (form == null)
                return;
            DateTime dt = DateTime.Now;
            str =  String.Format("{0:00}", dt.Hour)+":"+String.Format("{0:00}", dt.Minute)+":"+String.Format("{0:00}", dt.Second) + "." + String.Format("{0:000}", dt.Millisecond) + " : " + str;
            lock (DebugWindow.form)
            {
                if (DebugWindow.form != null)
                {
                    lock (DebugWindow.form.messages)
                        DebugWindow.form.messages.Enqueue(str);
                }
            }       

        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            lock (DebugWindow.form)
                lock (DebugWindow.form.messages)
                    listBox_messages.Items.Clear();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            String msg;
            lock (messages)
            {
                while (messages.Count > 0)
                {
                    msg = messages.Dequeue();
                    add_text(msg);
                }
            }
        }

        private void DebugWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (timer1!=null)
                if (timer1.Enabled)
                    timer1.Stop();
            DebugWindow.form = null;
        }
    }
}
