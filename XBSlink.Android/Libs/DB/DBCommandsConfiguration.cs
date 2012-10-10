using System;
using System.Collections.Generic;
using System.Text;
using Android.Content;
using Android.Database;

namespace XBSLink.Client.Android.Libs.DB
{

    public class configuration
    {
        public string parameter { get; set; }
        public string value { get; set; }
    }

    class DBCommandsConfiguration
{

    //string server_ip = "10.67.2.1";
    //int server_port = 34522;

    public static string table_name = "configuration";

    private DBHelper dbHelp;
    public DBCommandsConfiguration(Context context)
    {
        dbHelp = new DBHelper(context);
        dbHelp.OnCreate(dbHelp.WritableDatabase);
    }

    public IList<configuration> GetAllParameters()
    {
                                              
        ICursor golfCursor = dbHelp.ReadableDatabase.Query(table_name, null, null, null, null, null, null, null);
        var scores = new List<configuration>();
        while (golfCursor.MoveToNext())
        {
            configuration scr = MapCursorToEntity(golfCursor);
            scores.Add(scr);
        }
        return scores;
    }

    public configuration GetByParameter(string Parameter)
    {
        ICursor finded = dbHelp.ReadableDatabase.Query(table_name, new string[] { "parameter","value" }, "parameter='" + Parameter + "'", null, null, null, null, null);
        var scores = new List<configuration>();
        while (finded.MoveToNext())
        {
            return MapCursorToEntity(finded);
        }
        return null;
    }

    public int Update(string Parameter, string Value)
    {
        if (Parameter != "")
        {
            var values = new ContentValues();
            values.Put("value", Value);
            return dbHelp.WritableDatabase.Update(table_name, values, "parameter='" + Parameter + "'", null);
        }
        return -1;
    }

    public long Insert(string Parameter, string Value)
    {
        if (Parameter != "")
        {
            var encontrada = GetByParameter(Parameter);
            if (encontrada == null)
            {
                var values = new ContentValues();
                values.Put("parameter", Parameter);
                values.Put("value", Value);
                return dbHelp.WritableDatabase.Insert(table_name, null, values);
            }
            else
                return (long)Update(Parameter, Value);
        }
        return -1;
    }
    public int Delete(string Parameter)
    {
        if (Parameter != "")
            return dbHelp.WritableDatabase.Delete(table_name, "parameter=?", new string[] { Parameter.ToString() });
        return -1;
    }

    public void DeleteAll()
    {
        dbHelp.WritableDatabase.Delete(table_name, "", null);
    }

    private configuration MapCursorToEntity(ICursor cursor)
    {
        configuration scr = new configuration();
        scr.parameter = cursor.GetString(0);
        scr.value = cursor.GetString(1);
        return (scr);
    }


    public void SetConfiguration(string xbslink_internal_ip, bool play_sound_new_message, bool play_sound_join_leave)
    {
        Insert("play_sound_new_message", play_sound_new_message.ToString());
        Insert("play_sound_join_leave", play_sound_join_leave.ToString());
        Insert("xbslink_internal_ip", xbslink_internal_ip);
    }

}
}
