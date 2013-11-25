using System;
using System.Collections.Generic;

#if MONOMAC
using MonoMac.CoreText;
#else
using MonoTouch.CoreText;
#endif

namespace System.Drawing.Text 
{
	public abstract partial class FontCollection
	{
		internal CTFontCollection nativeFontCollection = null;

		List<string> NativeFontFamilies ()
		{
			if (nativeFontCollection == null) 
			{
				var collectionOptions = new CTFontCollectionOptions ();
				collectionOptions.RemoveDuplicates = true;
				nativeFontCollection = new CTFontCollection (collectionOptions);
			}

			var fontdescs = nativeFontCollection.GetMatchingFontDescriptors ();

			var fontFamilies = new List<string> ();
			foreach (var fontdesc in fontdescs) {

				var font = new CTFont (fontdesc, 0);

				// Just in case RemoveDuplicates collection option is not working
				if (!fontFamilies.Contains (font.FamilyName)) 
				{
					fontFamilies.Add (font.FamilyName);
				}

			}

			return fontFamilies;
		}

	}
}

