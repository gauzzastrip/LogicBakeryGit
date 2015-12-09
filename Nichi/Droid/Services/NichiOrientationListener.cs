using System;
using Android.Content;
using Android.Views;
using Android.Hardware;
using Plugin.Media;

namespace NichelyPrototype.Droid
{
	public class NichiOrientationListener : OrientationEventListener
	{
		private readonly Action<int> onOrientationChanged;

		private int currentOrientation;
		public NichiOrientationListener(Context context, Action<int> onOrientationChanged) : base(context)
		{
			//parent = (MainActivity)context;

			currentOrientation = -1;
			SetCameraOrientation (90);

			this.onOrientationChanged = onOrientationChanged;
		}

		public override void OnOrientationChanged(int orientation)
		{
			this.onOrientationChanged(orientation);
			SetCameraOrientation (orientation);	
		}
	
		public void SetCameraOrientation(int orientation){
			if (orientation != currentOrientation) {
				
				currentOrientation = orientation;
				var mNumberOfCameras = Android.Hardware.Camera.NumberOfCameras;
				var cameraInfo = new Android.Hardware.Camera.CameraInfo ();
				int rotation = 0;
				orientation = (orientation + 45) / 90 * 90;

				for (int i = 0; i < mNumberOfCameras; i++) {
					Android.Hardware.Camera.GetCameraInfo (i, cameraInfo);
				
					if (cameraInfo.Facing == Android.Hardware.Camera.CameraInfo.CameraFacingBack) {
						rotation = (cameraInfo.Orientation - orientation + 360) % 360;
//						var camera = Android.Hardware.Camera.Open (i);
//						var currentParams = camera.GetParameters ();
//						currentParams.Set ("orientation", "portrait");
//						currentParams.Set ("rotation", rotation);
//						camera.SetParameters (currentParams);
//
//						try
//						{
//							var downPolymorphic = camera.GetType().GetMethod("SetDisplayOrientation", new Type[] { typeof( int) });
//							if (downPolymorphic != null)
//								downPolymorphic.Invoke(camera, new Object[] { rotation });
//						}
//						catch (Exception e1)
//						{
//							
//						}
//						camera.Release ();

					} else {  // back-facing camera
						rotation = (cameraInfo.Orientation + orientation) % 360;
					}
					CrossMedia.Current.Orientation = rotation;

				}
			}
		}

	}



}

