using System;
using Xamarin.Forms;
using NichelyPrototype.Droid;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Provider;
using Java.IO;
using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;

[assembly: Dependency(typeof(CameraService))]

namespace NichelyPrototype.Droid
{
    public class CameraService : ICameraService
    {
        static File file;
        static File pictureDirectory;

        static TaskCompletionSource<CameraResult> tcs; 

        public Task<CameraResult> TakePictureAsync()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);

            pictureDirectory = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "Nichely");

            if (!pictureDirectory.Exists())
            {
                pictureDirectory.Mkdirs();
            }

            file = new File(pictureDirectory, String.Format("photo_{0}.jpg", Guid.NewGuid()));

            intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(file));

            var activity = (Activity)Forms.Context;
            activity.StartActivityForResult(intent, 0);
            tcs = new TaskCompletionSource<CameraResult>();

            return tcs.Task;
        }

        public static void OnResult(Result resultCode)
        {
            if (resultCode == Result.Canceled)
            {
                tcs.TrySetResult(null);
                return;
            }

            if (resultCode != Result.Ok)
            {
                tcs.TrySetException(new Exception("Unexpected error"));
                return;
            }

            CameraResult res = new CameraResult();
            res.Image = ImageSource.FromFile(file.Path);
            res.FileUri = file.Path; 

            tcs.TrySetResult(res);
        }
    }
}

