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
using System.Reflection;

namespace XBSlink
{
    public partial class DebugWindow : Form
    {
        private static DebugWindow form = null;
        private DateTime last_resize = DateTime.Now;
        Font font_listbox;
        Brush text_brush = new SolidBrush(Color.Black);
        List<Color> background_color_list = new List<Color>(200);

        public DebugWindow()
        {
            InitializeComponent();
            form = this;
            font_listbox = new Font(FontFamily.GenericSansSerif, 8, FontStyle.Regular);
        }

        private void DebugWindows_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void add_text(String[] messages)
        {
            for (int i = 0; i < messages.Length; i++)
                background_color_list.Add(getBackColor(messages[i]));
            listBox_messages.Items.AddRange(messages);
            int top_index = listBox_messages.Items.Count - (int)(listBox_messages.Height / listBox_messages.ItemHeight) + 2;
            listBox_messages.TopIndex = (top_index>0 && top_index<listBox_messages.Items.Count) ? top_index : 0;
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            lock (DebugWindow.form)
            {
                background_color_list.Clear();
                listBox_messages.Items.Clear();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (xbs_messages.getDebugMessageCount() > 0)
            {
                listBox_messages.BeginUpdate();
                add_text( xbs_messages.DequeueDebugMessageStringArray() );
                listBox_messages.EndUpdate();
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

        private void listBox_messages_DrawItem(object sender, DrawItemEventArgs e)
        {
            String msg = (String)listBox_messages.Items[e.Index];
            e.Graphics.FillRectangle(new SolidBrush(background_color_list[e.Index]), e.Bounds);
            TextRenderer.DrawText(e.Graphics, msg, font_listbox, e.Bounds, Color.Black, TextFormatFlags.Left);
            if (e.Index == listBox_messages.Items.Count - 1)
            {
                int items_height = listBox_messages.ItemHeight * listBox_messages.Items.Count;
                e.Graphics.FillRectangle(new SolidBrush(e.BackColor), 0, e.Bounds.Bottom, listBox_messages.ClientRectangle.Width, listBox_messages.ClientRectangle.Height);
            }
        }

        private void listBox_messages_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            Size s = TextRenderer.MeasureText((String)listBox_messages.Items[e.Index], font_listbox);
            e.ItemHeight = (int)s.Height;
            e.ItemWidth = (int)s.Width;
            if (listBox_messages.HorizontalExtent < e.ItemWidth)
                listBox_messages.HorizontalExtent = e.ItemWidth;
        }

        private Color getBackColor( string str )
        {
            String message = str.Substring(str.IndexOf(" : ")+2).Trim();
            Color c = listBox_messages.BackColor;
            if (message.StartsWith("!!"))
                c = Color.Firebrick;
            else if (message.StartsWith("%"))
                c = Color.YellowGreen;
            else if (message.StartsWith("*"))
                c = Color.WhiteSmoke;
            else if (message.StartsWith("x"))
                c = Color.SkyBlue;
            else if (message.StartsWith("-"))
                c = Color.Orange;
            else if (message.StartsWith("@"))
                c = Color.Khaki;
            else if (message.StartsWith("+"))
                c = Color.Aquamarine;
            else if (message.StartsWith("~"))
                c = Color.Teal;
            else if (message.StartsWith("i>"))
                c = Color.SandyBrown;
            else if (message.StartsWith("s>"))
                c = Color.Olive;
            return c;
        }
    }

    public class DebugWindowListBox : ListBox
    {
        public DebugWindowListBox()
            : base()
        {
            this.SetStyle( ControlStyles.OptimizedDoubleBuffer, true);
            this.DoubleBuffered = true;
            //this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle( ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle( ControlStyles.ResizeRedraw, true);
        }

        private const int WM_ERASEBKGND = 0x0014;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg != WM_ERASEBKGND || Items.Count == 0)
                base.WndProc(ref m);
        }
    }

}
