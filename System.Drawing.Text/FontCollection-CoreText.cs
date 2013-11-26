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
		internal Dictionary<string, CTFontDescriptor> nativeFontDescriptors = null;

		List<string> NativeFontFamilies ()
		{
			if (nativeFontCollection == null) 
			{
				var collectionOptions = new CTFontCollectionOptions ();
				collectionOptions.RemoveDuplicates = true;
				nativeFontCollection = new CTFontCollection (collectionOptions);
				nativeFontDescriptors = new Dictionary<string, CTFontDescriptor> ();
			}

			var fontdescs = nativeFontCollection.GetMatchingFontDescriptors ();

			foreach (var fontdesc in fontdescs) 
			{
				var attribs = fontdesc.GetAttributes ();
				var font = new CTFont (fontdesc, 0);

				// Just in case RemoveDuplicates collection option is not working
				if (!nativeFontDescriptors.ContainsKey (font.FamilyName)) 
				{
					nativeFontDescriptors.Add (font.FamilyName, fontdesc);
				}

			}
			var fontFamilies = new List<string> (nativeFontDescriptors.Keys);

			return fontFamilies;
		}

	}
}

