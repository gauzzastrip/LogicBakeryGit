using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using XLabs.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Mvvm;
using XLabs.Platform.Services.Media;

namespace NichelyPrototype.Droid
{
    [Activity(Label = "NichelyPrototype.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity :XFormsApplicationDroid
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            FFImageLoading.Forms.Droid.CachedImageRenderer.Init();
            global::Xamarin.Forms.Forms.Init(this, bundle);

			var resolverContainer = new SimpleContainer();
			var app = new XFormsAppDroid();
			app.Init(this);
			resolverContainer.Register<IDevice>(t => AndroidDevice.CurrentDevice)
				.Register<IDisplay>(t => t.Resolve<IDevice>().Display)
				.Register<IDependencyContainer>(resolverContainer)
				.Register<IXFormsApp>(app)
				.Register<IMediaPicker>(t => t.Resolve<MediaPicker>());

			Resolver.SetResolver(resolverContainer.GetResolver());

			FFImageLoading.Forms.Droid.CachedImageRenderer.Init();

            LoadApplication(new App());

        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            NichelyPrototype.Droid.CameraService.OnResult(resultCode);

        }
    }
}

