using System;
using Android.Content;
using Android.Views;
using Android.Hardware;

namespace NichelyPrototype.Droid
{
	public class NicheOrientationListener : OrientationEventListener
	{
		private readonly Action<int> onOrientationChanged;

		private int currentOrientation;
		public NicheOrientationListener(Context context, Action<int> onOrientationChanged) : base(context)
		{
			//parent = (MainActivity)context;

			currentOrientation = -1;
			SetCameraOrientation (0);

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
					var camera = Android.Hardware.Camera.Open (i);

					if (cameraInfo.Facing == Android.Hardware.Camera.CameraInfo.CameraFacingBack) {

						rotation = (cameraInfo.Orientation - orientation + 360) % 360;
						//	var 
						//	camera.SetParameters(
						//camera.SetDisplayOrientation (rotation);
						var currentParams = camera.GetParameters ();
						currentParams.SetRotation (180);
						camera.SetParameters (currentParams);
						camera.SetDisplayOrientation (180);
						//p.SetRotation(rotation);

						//mParameters.setRotation(rotation);


					} else {  // back-facing camera
						rotation = (cameraInfo.Orientation + orientation) % 360;
					}
					camera.Release ();
				}
			}
		}

	}



}

