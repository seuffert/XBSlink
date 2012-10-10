package xbslink.android;


public class MainChatActivity_MyRunnable
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		java.lang.Runnable
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_run:()V:GetRunHandler:Java.Lang.IRunnableInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("XBSlink.Android.MainChatActivity/MyRunnable, XBSlink.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", MainChatActivity_MyRunnable.class, __md_methods);
	}


	public MainChatActivity_MyRunnable ()
	{
		super ();
		if (getClass () == MainChatActivity_MyRunnable.class)
			mono.android.TypeManager.Activate ("XBSlink.Android.MainChatActivity/MyRunnable, XBSlink.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public MainChatActivity_MyRunnable (android.widget.ScrollView p0)
	{
		super ();
		if (getClass () == MainChatActivity_MyRunnable.class)
			mono.android.TypeManager.Activate ("XBSlink.Android.MainChatActivity/MyRunnable, XBSlink.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Widget.ScrollView, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=c4c4237547e4b6cd", this, new java.lang.Object[] { p0 });
	}


	public void run ()
	{
		n_run ();
	}

	private native void n_run ();

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
