using System;
using Xamarin.Forms;
using FFImageLoading.Forms;
using XLabs.Platform.Services.Media;
using System.Threading.Tasks;
using System.Collections;
using System.Reflection;
using System.IO;

namespace NichelyPrototype
{
    public class HomePage : ContentPage
    {
        ListView images;
		private ImageSource imageSource;
		private IMediaPicker mediaPicker;
		private Image img;
		private string status;

		#if __IOS__
		public static string ResourcePrefix = "XamFormsImageResize.iOS.";
		#endif
		#if __ANDROID__
		public static string ResourcePrefix = "XamFormsImageResize.Android.";
		#endif 
		#if WINDOWS_PHONE
		public static string ResourcePrefix = "XamFormsImageResize.WinPhone.";
		#endif

        public HomePage()
        {
            Title = "My Things";

            var addButton = new Button
            {
                Text = "Add",
                HorizontalOptions = LayoutOptions.Center
            };

            addButton.Clicked += async (sender, e) => 
            {
                //var cameraProvider = DependencyService.Get<ICameraService>();  
				//var mediaPicker = DependencyService.Get<IMediaPicker> ();
               // var pictureResult = await cameraProvider.TakePictureAsync();

//                if (pictureResult != null)  
//                {
//                    await Navigation.PushAsync(new AddPage(pictureResult.FileUri));
//                }
				await TakePictureAsync();
            };

            images = new ListView()
            {
                ItemTemplate = new DataTemplate(typeof(ListExampleCell)),
                VerticalOptions = LayoutOptions.FillAndExpand,
                HasUnevenRows = true,
            };       
            images.ItemSelected += async (sender, e) => 
            {                
                var niche = e.SelectedItem as Niche;

                await Navigation.PushAsync(new DetailPage(niche));
            };
			images.ItemsSource  = DataService.GetImagesForHomeScreenAsync () as IEnumerable;

            var grid = new Grid();

            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto});

            grid.Children.Add(images, 0, 0);
            grid.Children.Add(addButton, 0, 1);

            Content = grid;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var existing = await DataService.GetAllNichesAsync();
            images.ItemsSource = existing;
        }

		private async Task TakePictureAsync ()
		{

			mediaPicker = DependencyService.Get<IMediaPicker>();
		
			imageSource = null;

			try
			{
				var mediaResult = await this.mediaPicker.TakePhotoAsync(new CameraMediaStorageOptions { DefaultCamera = CameraDevice.Front, MaxPixelDimension = 400 }).ContinueWith(t =>
					{
						if (t.IsFaulted)
						{
							var s = t.Exception.InnerException.ToString();
						}
						else if (t.IsCanceled)
						{
							var canceled = true;
						}
						else
						{
							var mediaFile = t.Result;
							mediaFile.Exif.Orientation = ExifLib.ExifOrientation.TopLeft;
							//var imgName = "NIMG_" + Guid.NewGuid() + ".jpg";
							//imageSource = ImageSource.FromStream(() => mediaFile.Source);
							//FileAccess.WriteStream(imgName, mediaFile.Source);
							//ImageSource imgSource = ImageSource.FromFile (FileAccess.FullPath (imgName)); 
							//mediaFile.Path = FileAccess.FullPath (imgName);

							return mediaFile;
						}

						return null;
					});
				if (mediaResult != null){
					await Navigation.PushAsync(new AddPage(mediaResult.Path));
				}
			}
			catch (System.Exception ex)
			{
				this.status = ex.Message;
			}
		}

    }

    class ListExampleCell : ViewCell
        {
            public ListExampleCell()
            {
                var image = new LargeImage() {
                    WidthRequest = 50,
                    HeightRequest = 50,
//                    DownsampleHeight = 50,
//                    TransparencyEnabled = false,
                    Aspect = Aspect.AspectFill
//                    CacheDuration = TimeSpan.FromDays(30),
//                    RetryCount = 3,
//                    RetryDelay = 500,
                };

				image.SetBinding<Niche>(LargeImage.ImageSourceProperty, v => v.ImageUri);

                var fileName = new Label() {
                    LineBreakMode = LineBreakMode.CharacterWrap,
                    YAlign = TextAlignment.Center,
                    XAlign = TextAlignment.Center,
                };
                fileName.SetBinding<Niche>(Label.TextProperty, v => v.Title);

                var root = new AbsoluteLayout() {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Padding = 5,
                };
                    
                root.Children.Add(image, new Rectangle(0f, 0f, 50, 50));
                root.Children.Add(fileName, new Rectangle(55, 0f, 150f, 50));

                View = root;    
            }
        }
}

