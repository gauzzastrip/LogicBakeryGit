using System.IO;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Content;

using Xamarin.Forms;

using Android.Content.Res;
using Plugin.Media;


namespace NichelyPrototype.Droid
{
	[Activity (Label = "Nichi.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity  : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		/// <summary>
		/// Indicated if the application has been initialized
		/// </summary>
	
		private static NichiOrientationListener orientationListener;
	
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			Forms.Init (this, bundle);

			Forms.ViewInitialized += (sender, e) => {
				if (!string.IsNullOrWhiteSpace (e.View.StyleId)) {
					e.NativeView.ContentDescription = e.View.StyleId;
				}
			};

			orientationListener = new NichiOrientationListener (this.BaseContext, o => {});

			orientationListener.Enable ();
			LoadApplication (new App ());
		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);
			NichelyPrototype.Droid.CameraService.OnResult (resultCode);
		}
	}
}
	


