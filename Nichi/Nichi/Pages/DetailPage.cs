using System;
using System.Linq;
using Xamarin.Forms;
using System.Collections.Generic;
using FFImageLoading.Forms;

namespace NichelyPrototype
{
    public class DetailPage : ContentPage
    {
		private StackLayout stackLayout;
		private Niche niche;
        public DetailPage(Niche _niche)
        {
			niche = _niche;
            var image = new LargeImage
            {

                Aspect = Aspect.AspectFit,
                HeightRequest = 300,
				ImageSource = niche.ImageUri
               // DownsampleHeight = 300
            };

            stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 10

            };
			var scrollViewAttributes = new ScrollView { Content = stackLayout };
            var addButton = new Button
            {
                Text = "Add",
                HorizontalOptions = LayoutOptions.Center
            };

            addButton.Clicked += (sender, e) => stackLayout.Children.Add(CreateDetailRow("", ""));

            var done = new ToolbarItem
            {   
                Text = "Save"
            };

            done.Clicked += async (sender, e) => 
            {
				int index =0;
				niche.Attributes.Clear();
                foreach(var child in stackLayout.Children)
                {
                    var title = ((Entry)((StackLayout)child).Children[0]).Text;
                    var detail = ((Entry)((StackLayout)child).Children[1]).Text;
				
					if(!string.IsNullOrEmpty(title) ){
						//var existingAttribute = niche.Attributes.Where(w=>w.Key.ToLower () == title.ToLower()).ToList();
						niche.Attributes.Add(new NicheAttribute() { Key = title,  Value = detail, NicheId = niche.NicheId , SortOrder = index } );
						index++;
					}
                }

                await DataService.SaveNicheAsync(niche);

                await Navigation.PopToRootAsync();
            };

            ToolbarItems.Add(done);

            Content = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 10,
                Padding = 10, 
                Children = 
                {
                    image,
					scrollViewAttributes,
                    addButton
                }
            };
        }
		protected override async void OnAppearing()
		{
			if (niche.Attributes.Count <= 0) {
				niche.Attributes = await DataService.GetDefaultAttributes (niche.Title); 
			} 
			foreach (var detail in niche.Attributes) {
				stackLayout.Children.Add (CreateDetailRow (detail.Key, detail.Value));
			}

			if(stackLayout.Children.Count <= 0)
				stackLayout.Children.Add(CreateDetailRow("", ""));
		}


		StackLayout CreateDetailRow(string title, object value)
        {
            var titleEntry = new Entry
            {
                Text = title,
                Placeholder = "Title",
                PlaceholderColor = Color.Gray,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				HorizontalTextAlignment = TextAlignment.End
            };

            var detailEntry = new Entry
            {
				Text = value.ToString(),
                Placeholder = "Detail",
                PlaceholderColor = Color.Gray,
				HorizontalOptions = LayoutOptions.FillAndExpand
            };

            return new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 10,
                Padding = 0,
                Children = 
                {
                    titleEntry,
                    detailEntry
                }
            };
        }
    }
}

