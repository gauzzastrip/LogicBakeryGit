package md564c4a2b9f24024bbfe8a5a73e9baebd9;


public class FFBitmapDrawable
	extends android.graphics.drawable.BitmapDrawable
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_draw:(Landroid/graphics/Canvas;)V:GetDraw_Landroid_graphics_Canvas_Handler\n" +
			"n_setAlpha:(I)V:GetSetAlpha_IHandler\n" +
			"n_setColorFilter:(ILandroid/graphics/PorterDuff$Mode;)V:GetSetColorFilter_ILandroid_graphics_PorterDuff_Mode_Handler\n" +
			"n_onBoundsChange:(Landroid/graphics/Rect;)V:GetOnBoundsChange_Landroid_graphics_Rect_Handler\n" +
			"";
		mono.android.Runtime.register ("FFImageLoading.Work.FFBitmapDrawable, FFImageLoading.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", FFBitmapDrawable.class, __md_methods);
	}


	public FFBitmapDrawable (android.content.res.Resources p0, android.graphics.Bitmap p1) throws java.lang.Throwable
	{
		super (p0, p1);
		if (getClass () == FFBitmapDrawable.class)
			mono.android.TypeManager.Activate ("FFImageLoading.Work.FFBitmapDrawable, FFImageLoading.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Res.Resources, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Graphics.Bitmap, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
	}


	public FFBitmapDrawable (android.graphics.Bitmap p0) throws java.lang.Throwable
	{
		super (p0);
		if (getClass () == FFBitmapDrawable.class)
			mono.android.TypeManager.Activate ("FFImageLoading.Work.FFBitmapDrawable, FFImageLoading.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Graphics.Bitmap, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public void draw (android.graphics.Canvas p0)
	{
		n_draw (p0);
	}

	private native void n_draw (android.graphics.Canvas p0);


	public void setAlpha (int p0)
	{
		n_setAlpha (p0);
	}

	private native void n_setAlpha (int p0);


	public void setColorFilter (int p0, android.graphics.PorterDuff.Mode p1)
	{
		n_setColorFilter (p0, p1);
	}

	private native void n_setColorFilter (int p0, android.graphics.PorterDuff.Mode p1);


	public void onBoundsChange (android.graphics.Rect p0)
	{
		n_onBoundsChange (p0);
	}

	private native void n_onBoundsChange (android.graphics.Rect p0);

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
