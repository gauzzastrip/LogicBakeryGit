package md52689f2df1f9d26957fa091e8dd83237d;


public class NicheOrientationListener
	extends android.view.OrientationEventListener
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onOrientationChanged:(I)V:GetOnOrientationChanged_IHandler\n" +
			"";
		mono.android.Runtime.register ("NichelyPrototype.Droid.NicheOrientationListener, NichelyPrototype.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", NicheOrientationListener.class, __md_methods);
	}


	public NicheOrientationListener (android.content.Context p0) throws java.lang.Throwable
	{
		super (p0);
		if (getClass () == NicheOrientationListener.class)
			mono.android.TypeManager.Activate ("NichelyPrototype.Droid.NicheOrientationListener, NichelyPrototype.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public NicheOrientationListener (android.content.Context p0, int p1) throws java.lang.Throwable
	{
		super (p0, p1);
		if (getClass () == NicheOrientationListener.class)
			mono.android.TypeManager.Activate ("NichelyPrototype.Droid.NicheOrientationListener, NichelyPrototype.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Hardware.SensorDelay, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
	}


	public void onOrientationChanged (int p0)
	{
		n_onOrientationChanged (p0);
	}

	private native void n_onOrientationChanged (int p0);

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
