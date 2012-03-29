/**
 * Project: XBSlink: A XBox360 & PS3/2 System Link Proxy
 * File name: xbs_system_functions.cs
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
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace XBSlink
{
    static class xbs_system_functions
    {
        private static volatile WindowsNativeMethods.EXECUTION_STATE fPreviousExecutionState;

        public static void PreventSystemFromSleeping()
        {
            try
            {
                fPreviousExecutionState = WindowsNativeMethods.SetThreadExecutionState(WindowsNativeMethods.EXECUTION_STATE.ES_CONTINUOUS | WindowsNativeMethods.EXECUTION_STATE.ES_AWAYMODE_REQUIRED | WindowsNativeMethods.EXECUTION_STATE.ES_SYSTEM_REQUIRED);
            }
            catch (DllNotFoundException dnte)
            {
               
            }
            catch (EntryPointNotFoundException enfe)
            {
                 

            }
        }

        public static void restoreSystemSleepState()
        {
            try
            {
                WindowsNativeMethods.SetThreadExecutionState(WindowsNativeMethods.EXECUTION_STATE.ES_CONTINUOUS);
            }
            catch (DllNotFoundException dnte)
            {
            }
            catch (EntryPointNotFoundException enfe)
            {
            }
        }
    }

    internal static class WindowsNativeMethods
    {
        [FlagsAttribute]
        public enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_SYSTEM_REQUIRED = 0x00000001
            // Legacy flag, should not be used.
            // ES_USER_PRESENT = 0x00000004
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);
    }
}
