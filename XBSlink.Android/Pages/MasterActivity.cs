using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.Views;

namespace XBSLink.Client.Android
{
   public class MasterActivity : Activity
    {

       public override bool OnCreateOptionsMenu(IMenu menu)
{
    base.OnCreateOptionsMenu(menu);
    int groupId = 0;
    // Unique menu item Identifier. Used for event handling.
    int menuItemId =Menu.First;
    // The order position of the item
    int menuItemOrder = Menu.None;
    // Text to be displayed for this menu item.
    int menuItemText = 1;
    // Create the menu item and keep a reference to it.
    IMenuItem menuItem1 = menu.Add(groupId, menuItemId, menuItemOrder, 
        menuItemText);
    menuItem1.SetShortcut('1', 'a');
    Int32 MenuGroup = 10;
    IMenuItem menuItem2 =
        menu.Add(MenuGroup, menuItemId + 10, menuItemOrder + 1,
        new Java.Lang.String("Menu Item 2"));
    IMenuItem menuItem3 =
        menu.Add(MenuGroup, menuItemId + 20, menuItemOrder + 2,
        new Java.Lang.String("Menu Item 3"));
    ISubMenu sub = menu.AddSubMenu(0, menuItemOrder + 30,
        menuItemOrder + 3, new Java.Lang.String("Submenu 1"));
    sub.SetHeaderIcon(Resource.Drawable.ic_tab_artists_grey);
    sub.SetIcon(Resource.Drawable.ic_tab_artists_grey);
    IMenuItem submenuItem = sub.Add(0, menuItemId + 40, menuItemOrder + 4,
        new Java.Lang.String("Submenu Item"));
    IMenuItem submenuItem2 =
        sub.Add(MenuGroup, menuItemId + 50, menuItemOrder + 5,
        new Java.Lang.String("sub-1")).SetCheckable(true);
    IMenuItem submenuItem3 =
        sub.Add(MenuGroup, menuItemId + 60, menuItemOrder + 6,
        new Java.Lang.String("sub-2")).SetCheckable(true);
     return true;
       }

    }
}
