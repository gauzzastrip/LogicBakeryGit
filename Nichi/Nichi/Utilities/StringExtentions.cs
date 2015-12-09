using System;
using System.Globalization;

namespace NichelyPrototype
{
	public static class StringExtentions
	{
		
		public static string ToTitleCase(this string value)
		{

			string[] words = value.Split ('_');

			for (int i = 0; i <= words.Length - 1; i++) {
				if ((!object.ReferenceEquals (words [i], string.Empty))) {
					string firstLetter = words [i].Substring (0, 1);
					string rest = words [i].Substring (1);
					string result = firstLetter.ToUpper () + rest.ToLower ();
					words [i] = result;
				}
			}
			return String.Join ("", words);
		}
	}
}

