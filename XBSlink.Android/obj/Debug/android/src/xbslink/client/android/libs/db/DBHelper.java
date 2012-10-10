package xbslink.client.android.libs.db;


public class DBHelper
	extends android.database.sqlite.SQLiteOpenHelper
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/database/sqlite/SQLiteDatabase;)V:GetOnCreate_Landroid_database_sqlite_SQLiteDatabase_Handler\n" +
			"n_onUpgrade:(Landroid/database/sqlite/SQLiteDatabase;II)V:GetOnUpgrade_Landroid_database_sqlite_SQLiteDatabase_IIHandler\n" +
			"";
		mono.android.Runtime.register ("XBSLink.Client.Android.Libs.DB.DBHelper, XBSlink.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", DBHelper.class, __md_methods);
	}


	public DBHelper (android.content.Context p0, java.lang.String p1, android.database.sqlite.SQLiteDatabase.CursorFactory p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == DBHelper.class)
			mono.android.TypeManager.Activate ("XBSLink.Client.Android.Libs.DB.DBHelper, XBSlink.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=c4c4237547e4b6cd:System.String, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:Android.Database.Sqlite.SQLiteDatabase/ICursorFactory, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=c4c4237547e4b6cd:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public void onCreate (android.database.sqlite.SQLiteDatabase p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.database.sqlite.SQLiteDatabase p0);


	public void onUpgrade (android.database.sqlite.SQLiteDatabase p0, int p1, int p2)
	{
		n_onUpgrade (p0, p1, p2);
	}

	private native void n_onUpgrade (android.database.sqlite.SQLiteDatabase p0, int p1, int p2);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
