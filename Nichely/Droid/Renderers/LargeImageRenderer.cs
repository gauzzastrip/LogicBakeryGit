using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.ComponentModel;
using Android.Graphics;
using NichelyPrototype;
using NichelyPrototype.Droid;
using System.Linq;


[assembly: ExportRenderer(typeof(LargeImage), typeof(LargeImageRenderer))]
namespace NichelyPrototype.Droid
{
	public class LargeImageRenderer : ImageRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged(e);
		}

		private bool _isDecoded;
		protected override async void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			var largeImage = (LargeImage)Element;

			if ((Element.Width > 0 && Element.Height > 0 && !_isDecoded) || (e.PropertyName == "ImageSource" && largeImage.ImageSource != null)) {
				BitmapFactory.Options options = new BitmapFactory.Options ();
				options.InJustDecodeBounds = true;

//				//Get the resource id for the image
//				var resourceId = largeImage.ImageSource.Split ('.').ToList ().First ();
//				var field = typeof(Resource.Drawable).GetField (resourceId);
//				var value = (int)field.GetRawConstantValue ();

				//await BitmapFactory.DecodeFileAsync (largeImage.ImageSource, options);

				//The with and height of the elements (LargeImage) will be used to decode the image
				var width = (int)Element.Width;
				var height = (int)Element.Height;
				options.OutWidth = width;
				options.OutHeight = height;
				options.InSampleSize = 3;// CalculateInSampleSize (options, width, height);

				options.InJustDecodeBounds = false;
				using (var bitmap = await BitmapFactory.DecodeFileAsync (largeImage.ImageSource, options)) {
					// (Context.Resources, value, options);
					//bitmap.Compress(Bitmap.CompressFormat.Jpeg,5,
					//Set the bitmap to the native control
					Control.SetImageBitmap (bitmap);
				}

				_isDecoded = true;
			}
		}

		public int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
		{
			// Raw height and width of image
			float height = options.OutHeight;
			float width = options.OutWidth;
			double inSampleSize = 1D;

			if (height > reqHeight || width > reqWidth)
			{
				int halfHeight = (int)(height / 2);
				int halfWidth = (int)(width / 2);

				// Calculate a inSampleSize that is a power of 2 - the decoder will use a value that is a power of two anyway.
				while ((halfHeight / inSampleSize) > reqHeight && (halfWidth / inSampleSize) > reqWidth)
				{
					inSampleSize *= 2;
				}
			}

			return (int)inSampleSize;
		}
	}
}

