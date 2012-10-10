package xbslink.android;


public class BroadcastService
	extends mono.android.app.IntentService
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onHandleIntent:(Landroid/content/Intent;)V:GetOnHandleIntent_Landroid_content_Intent_Handler\n" +
			"";
		mono.android.Runtime.register ("XBSlink.Android.BroadcastService, XBSlink.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", BroadcastService.class, __md_methods);
	}


	public BroadcastService (java.lang.String p0)
	{
		super (p0);
		if (getClass () == BroadcastService.class)
			mono.android.TypeManager.Activate ("XBSlink.Android.BroadcastService, XBSlink.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "System.String, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0 });
	}


	public BroadcastService ()
	{
		super ();
		if (getClass () == BroadcastService.class)
			mono.android.TypeManager.Activate ("XBSlink.Android.BroadcastService, XBSlink.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onHandleIntent (android.content.Intent p0)
	{
		n_onHandleIntent (p0);
	}

	private native void n_onHandleIntent (android.content.Intent p0);

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
