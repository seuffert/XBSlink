/**
 * Project: XBSlink: A XBox360 & PS3/2 System Link Proxy
 * File name: Program.cs
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
using System.Windows.Forms;

namespace XBSlink
{
    static class Program
    {
        public static FormMain main_form = null;
        public static xbs_settings settings = null;
        private static CommandLine console_app = null;
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            settings = new xbs_settings();

            if (args.Length > 1)
            {
                console_app = new CommandLine(settings);
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                try
                {
                    main_form = new FormMain();
                }
                catch (ApplicationException)
                {
                    main_form = null;
                }

                if (main_form != null)
                    Application.Run(main_form);
            }
        }
    }
}
