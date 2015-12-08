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
	[Activity (Label = "NichelyPrototype.Droid", Icon = "@drawable/icon", MainLauncher = true)]
	public class MainActivity  : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		/// <summary>
		/// Indicated if the application has been initialized
		/// </summary>
		private static bool _initialized;
		private static int currentOrientation;
		private static NicheOrientationListener orientationListener;
		public int GlobalOrientation;

		public override void OnConfigurationChanged (Android.Content.Res.Configuration newConfig)
		{
			base.OnConfigurationChanged (newConfig);
		}



		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			//FFImageLoading.Forms.Droid.CachedImageRenderer.Init();
			//global::Xamarin.Forms.Forms.Init(this, bundle);

//			if (!Resolver.IsSet)
//			{
//				this.SetIoc();
//			}
//			else
//			{
//				var app = Resolver.Resolve<IXFormsApp>() as IXFormsApp<XFormsApplicationDroid>;
//				if (app != null) app.AppContext = this;
//			}

			Forms.Init (this, bundle);

			Forms.ViewInitialized += (sender, e) => {
				if (!string.IsNullOrWhiteSpace (e.View.StyleId)) {
					e.NativeView.ContentDescription = e.View.StyleId;
				}
			};

			orientationListener = new NicheOrientationListener (this.BaseContext, o => {
			});

			//SetCameraOrientation ();
//			Android.Views.OrientationListener orientationListener = new Android.Views.OrientationListener (BaseContext);
//			orientationListener.OnOrientationChanged += (orientation) => {
//				if (orientation == null && (orientation > 360 | orientation < -360)) return;
//
//
//			};
////			Android.Views.OrientationEventListener orientationEventListener = new Android.Views.OrientationEventListener (this.BaseContext);
//
//			orientationEventListener.OnOrientationChanged += (orientation) =>{
//				//var orientation = (Android.Media.Orientation)e;
//				if (orientation == null && (orientation > 360 | orientation < -360)) return;
//
//				var mNumberOfCameras  = Android.Hardware.Camera.NumberOfCameras;
//
//				Android.Hardware.Camera.CameraInfo cameraInfo =
//					new Android.Hardware.Camera.CameraInfo();
//
//				int rotation = 0;
//				orientation = (orientation + 45) / 90 * 90;
//				for (int i = 0; i < mNumberOfCameras; i++) {
//					Android.Hardware.Camera.GetCameraInfo (i, cameraInfo);
//
//					if (cameraInfo.Facing == Android.Hardware.Camera.CameraInfo.CameraFacingBack) {
//
//						rotation = (cameraInfo.Orientation - orientation + 360) % 360;
//						Android.Hardware.Camera.Parameters p = new Android.Hardware.Camera.Parameters();
//						p.SetRotation(rotation);
//						//mParameters.setRotation(rotation);
//
//
//					}
//					else {  // back-facing camera
//						rotation = (cameraInfo.Orientation + orientation) % 360;
//					}
//				}

			//};
			orientationListener.Enable();
			LoadApplication (new App ());

		}
		//		public override void OnConfigurationChanged (Android.Content.Res.Configuration newConfig)
		//		{
		//
		//			base.OnConfigurationChanged (newConfig);
		//			int mRotation = GetCameraDisplayOrientation();
		//			Android.Hardware.Camera.Parameters parameters = camera.GetParameters();
		//
		//			parameters.SetRotation(mRotation); //set rotation to save the picture
		//
		//			camera.SetDisplayOrientation(result); //set the rotation for preview camera
		//
		//			camera.SetParameters(parameters);
		//
		//		}
		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);
			NichelyPrototype.Droid.CameraService.OnResult (resultCode);

		}
		public override bool DispatchTouchEvent (Android.Views.MotionEvent ev)
		{
			return base.DispatchTouchEvent (ev);
		}
		public override void OnContentChanged ()
		{
			base.OnContentChanged ();
		}
		private void SetIoc ()
		{
//			var resolverContainer = new SimpleContainer();
//
//			var app = new XFormsAppDroid();
//
//			app.Init(this);
//
//			var documents = app.AppDataDirectory;
//			var pathToDatabase = Path.Combine(documents, "xforms.db");
//
//			resolverContainer.Register<IDevice>(t => AndroidDevice.CurrentDevice)
//				.Register<IDisplay>(t => t.Resolve<IDevice>().Display)
//				.Register<IFontManager>(t => new FontManager(t.Resolve<IDisplay>()))
//			//	.Register<IJsonSerializer, Services.Serialization.ServiceStackV3.JsonSerializer>()
//
//				//.Register<IJsonSerializer, Newtonsoft.Json.JsonSerializer>()
//				.Register<IEmailService, EmailService>()
//				//.Register<IMediaPicker, MediaPicker>()
//				.Register<ITextToSpeechService, TextToSpeechService>()
//				.Register<IDependencyContainer>(resolverContainer)
//				.Register<IXFormsApp>(app)
//				.Register<ISecureStorage>(t => new KeyVaultStorage(t.Resolve<IDevice>().Id.ToCharArray()));
//
//
//			Resolver.SetResolver(resolverContainer.GetResolver());
		}
	
	}
}
	


