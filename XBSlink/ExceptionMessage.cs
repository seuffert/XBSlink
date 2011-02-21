/**
 * Project: XBSlink: A XBox360 & PS3/2 System Link Proxy
 * File name: ExceptionMessage.cs
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
using System.Threading;

namespace XBSlink
{
    public partial class ExceptionMessage : Form
    {
        private static Thread t;
        private static String message;
        public static bool ABORTING = false;

        private String additional_messages;

        public ExceptionMessage( Form f, String messages )
        {
            InitializeComponent();
            additional_messages = messages;
            this.ShowDialog(f);
        }

        private void ExceptionMessage_Load(object sender, EventArgs e)
        {
            textBox_message.Text = ExceptionMessage.message + Environment.NewLine
                + " - - - - - - - - - - - - - - - -" + Environment.NewLine 
                + additional_messages;
            textBox_message.SelectionStart = textBox_message.Text.Length;
            textBox_message.SelectionLength = 0;
            textBox_header.Text = Properties.Resources.crash_message;
        }

        public static void ShowExceptionDialog( String ex_sender, Exception ex )
        {
            ExceptionMessage.ABORTING = true;
            String str = "FATAL Error in XBSlink "+ex_sender+" thread."+Environment.NewLine;
            str += Environment.NewLine + ex.ToString() + Environment.NewLine;
            ExceptionMessage.message = str;
            ExceptionMessage.t = new Thread(close_app_async);
            t.IsBackground = true;
            t.Start();
        }

        private static void close_app_async()
        {
            Program.main_form.Invoke(
                new MethodInvoker( 
                    delegate 
                    { 
                        new ExceptionMessage(Program.main_form, Program.main_form.getAllMessages()); 
                        Program.main_form.Close(); 
                    }
                )
            );
        }
    }
}
