using System;
using System.Threading.Tasks;
using Akavache;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Linq;

namespace NichelyPrototype
{
    public static class DataService
    {
        public static async Task SaveNicheAsync(Niche niche)
        {
            // Make sure you set the application name before doing any inserts or gets
            BlobCache.ApplicationName = "NichelyPrototype";
			if (!(string.IsNullOrEmpty (niche.Title))) {
				await BlobCache.UserAccount.InsertObject (niche.NicheId.ToString (), niche);
			}
        }

		public static async Task<List<Niche>> GetAllNichesAsync()
        {
            // Make sure you set the application name before doing any inserts or gets
            BlobCache.ApplicationName = "NichelyPrototype";

			var allNiches = await BlobCache.UserAccount.GetAllObjects<Niche>();

			return allNiches.ToList();
        }

		public static async Task<List<NicheAttribute>> GetDefaultAttributes(string category)
		{
			var attributes = new List<NicheAttribute> ();
			var niches = await GetNichesByCategoryAsync (category);
			attributes = (from niche in niches 
			              //where niche.Attributes.Any()
					      select niche into n
			              from nicheAttribute in n.Attributes
			              select new NicheAttribute () { 
				Key = nicheAttribute.Key.ToTitleCase (),
				SortOrder = nicheAttribute.SortOrder,
				Value = ""
			}).ToList ();
			var presetAttributeStrings = new List<string> ();
			if (PresetCategories.TryGetValue (category, out presetAttributeStrings)) {
				//if (!attributes.Any(attribute => attribute.Key.ToLower() == 
				var presetAttributes = presetAttributeStrings
					.Except (attributes.Select (s => s.Key.ToLower ()))
				                       //.Where (w => !(attributes.Any (attribute => attribute.Key.ToLower () == w.ToLower ())))
					.Select ((a, i) => new NicheAttribute {
					Key = a,
					Value = "",
					SortOrder = i
				});
				attributes.AddRange (presetAttributes);
			}
			attributes = attributes
				.GroupBy (g => g.Key.ToLower ())
				.Select (niche => niche.FirstOrDefault ())
				.OrderBy (o => o.SortOrder)
				.ToList ();
			
			return attributes;
		}

		public static async Task<List<Niche>> FilterNichesAsync(string searchText)
		{
			var allNiches = await GetAllNichesAsync();
			var filteredNiches = allNiches.Where (w => !(string.IsNullOrEmpty(w.Title)) && (w.Title.ToLower ().Contains (searchText.ToLower()) ||
				(w.Attributes != null ? w.Attributes.Any (attritube => attritube.Key.ToLower ().Contains (searchText.ToLower())
					|| (attritube.Value != null ? attritube.Value.ToString ().ToLower ().Contains (searchText.ToLower()) : false))
					: false)));
				
			return filteredNiches.ToList();
		}

		public static async Task<List<Niche>> GetNichesByCategoryAsync(string searchText)
		{
			var allNiches = await GetAllNichesAsync();
			var filteredNiches = allNiches.Where (w => !(string.IsNullOrEmpty (w.Title)) && (w.Title.ToLower () == (searchText.ToLower ())));

			return filteredNiches.ToList();
		}

		public static async Task<List<string>> GetCategoriesAsync(int take = 6)
		{
			var allNiches = await GetAllNichesAsync ();

			var nicheCategories = allNiches
				.Where (w => !((string.IsNullOrEmpty (w.Title))))
				.GroupBy (g => g.Title.ToLower ())
				.Select (niche => new { Category = niche.FirstOrDefault ().Title, Count = niche.Count ()})
				.OrderBy (o => o.Count)
				.Select (niche => niche.Category)
				.ToList ();
	
			if (nicheCategories.Count () < take) {
				var presetCats = PresetCategories.Take (take - nicheCategories.Count ()).ToDictionary(x => x.Key,x => x.Value);

				nicheCategories.AddRange(presetCats.Keys);
			}
			nicheCategories = nicheCategories
				.GroupBy (g => g.ToLower ())
				.Select(s=> s.FirstOrDefault().ToTitleCase())
				.Take (take)
				.ToList ();
			return nicheCategories;
		}

		public static async Task<List<Niche>> GetImagesForHomeScreenAsync()
		{
			var allNiches = await GetAllNichesAsync ();
			var nicheImages = allNiches
				.OrderByDescending (o => o.CreatedOn)
				.Take (20);

			return nicheImages.ToList ();
		}
		
		public static Dictionary<string,List<string>> PresetCategories = new Dictionary<string,List<string>>(){
			{ "Beer", new List<string> { "Brewery" ,"Name", "Stye", "ABV","IBU","SRM" , "OG","FG", "Notes"}},
			{"Wine", new List<string> { "Winery" ,"Name", "Stye", "ABV", "Notes"}},
			{"Liquor", new List<string> { "Distillery" ,"Name", "Stye", "ABV", "Notes"}},
			{"Food", new List<string> { "Restaurant" ,"Name", "Address", "Notes"}},
			{"Craft", new List<string> { "Name" ,"For What", "Notes"}},
			{"Idea", new List<string> { "Name" ,"Notes"}},
			{"Baseball Card", new List<string> { "Name" ,"Price", "Notes"}},
			{"Yuh Gi Oh Card", new List<string> { "Name" ,"Price", "Notes"}}
		};
    }
}

