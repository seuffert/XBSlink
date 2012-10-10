using System;
using System.Collections.Generic;
using System.Text;
using Android.Content;
using Android.Database.Sqlite;

namespace XBSLink.Client.Android.Libs.DB
{
   class DBHelper : SQLiteOpenHelper
{
    private const string DbName = "XBSLink";
    private const int DbVersion = 1;

    public DBHelper(Context context) : base(context, DbName, null, DbVersion)
    {   
    }

    public override void OnCreate(SQLiteDatabase db)
    {
        db.ExecSQL(@"CREATE TABLE IF NOT EXISTS configuration (parameter TEXT, value TEXT)");
    }

    public override void OnUpgrade(SQLiteDatabase db,
       int oldVersion, int newVersion)
    {
        db.ExecSQL("DROP TABLE IF EXISTS configuration");
        OnCreate(db);
    }
}
}
