/**
 * Project: XBSlink: A XBox360 & PS3/2 System Link Proxy
 * File name: xbs_chat.cs
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
using System.Text;

namespace XBSlink
{
    class xbs_chat
    {
        private const String _STANDARD_NICKNAME = "Anonymous";
        public static String STANDARD_NICKNAME { get { return _STANDARD_NICKNAME + (new Random().Next(1000, 9999)).ToString(); } }
        public const String INCOMING_MESSAGE_SOUNG = "sounds/incoming_chat_msg.wav";
        public static bool notify_on_incoming_message = true;

        public static void addChatMessage( String sender, String msg)
        {
            addMessage(sender + " : " + msg);
            if (xbs_chat.notify_on_incoming_message)
            {
                System.Media.SoundPlayer sound_player = new System.Media.SoundPlayer();
                sound_player.SoundLocation = xbs_chat.INCOMING_MESSAGE_SOUNG;
                try
                {
                    sound_player.Play();
                }
                catch (Exception)
                {
                }
            }
        }

        public static void sendChatMessage(String message)
        {
            xbs_node_list.getInstance().sendChatMessageToAllNodes(message);
        }

        public static void addLocalMessage(String message)
        {
            addMessage("<" + xbs_node_list.getInstance().local_node.nickname + "> : " + message);
        }

        public static void addSystemMessage(String message)
        {
            xbs_messages.addChatMessage("* " + message + System.Environment.NewLine);
        }

        public static void addMessage(String message)
        {
            xbs_messages.addChatMessage(message + System.Environment.NewLine);
        }

    }
}
