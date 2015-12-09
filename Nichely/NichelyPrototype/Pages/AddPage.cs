using System;
using Xamarin.Forms;
using System.Collections;
using FFImageLoading.Forms;

namespace NichelyPrototype
{
    public class AddPage : ContentPage
    {
		private string fileUri;
		private ListView existingList;

        public AddPage(string _fileUri)
        {   

			fileUri = _fileUri;
            var titleLabel = new Label
            {
                Text = "This is...",
            };

            var somethingLabel = new Label
            {
                Text = "Something new?",
            };

            var categoryEntry = new Entry
            {
                Placeholder = "Beer, baseball card, etc.",
                PlaceholderColor = Color.Gray,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            existingList = new ListView
            {
				//ItemTemplate = new DataTemplate(typeof(CategoryListCell)),
                VerticalOptions = LayoutOptions.FillAndExpand
            };
			existingList.ItemSelected += (sender, e) => {
				var category= ((ListView)sender).SelectedItem;
				if (category != null){
					SaveNiche(category.ToString().ToTitleCase());
				}
			};

            var submitButton = new Button
            {
                Text = "Add",
                HorizontalOptions = LayoutOptions.Center
            };

            submitButton.Clicked += async (sender, e) => 
            {
				if (!string.IsNullOrEmpty(categoryEntry.Text)){
				SaveNiche( categoryEntry.Text);
				}
				else{
					DisplayAlert("Enter a custom niche","You have to enter a value if you want to enter a custom niche","Ok");
				}
            };

            var stack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 10,
                Padding = 10,
                Children = 
                {
                    titleLabel,
                    existingList,
                    somethingLabel,                           
                    categoryEntry,
                    submitButton
                }
            };

            Content = stack;
        }
		protected async void SaveNiche(string category)
		{
			var niche = new Niche
			{
				Title = category,
				ImageUri = fileUri
			};

			await Navigation.PushAsync(new DetailPage(niche));
		}
		protected override async void OnAppearing()
		{
			existingList.ItemsSource  = await DataService.GetCategoriesAsync () as IEnumerable;

		}
    }
	class CategoryListCell : ViewCell
	{
		public CategoryListCell()
		{
			var cateogry = new Label() {
				LineBreakMode = LineBreakMode.CharacterWrap,
				FontSize = 24
			};
			cateogry.SetBinding<string>(Label.TextProperty, v => v);

			var horizLayout = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				Spacing = 10,
				Padding = 10,
				Children = {
					cateogry
				}
			};

			View = horizLayout;    
		}
	}

}

