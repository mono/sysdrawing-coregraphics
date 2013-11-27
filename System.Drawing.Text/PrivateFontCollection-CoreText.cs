using System;
using System.Collections.Generic;
using System.IO;

#if MONOMAC
using MonoMac.CoreText;
using MonoMac.Foundation;
using MonoMac.CoreGraphics;
#else
using MonoTouch.CoreText;
using MonoTouch.Foundation;
using MonoTouch.CoreGraphics;
#endif

namespace System.Drawing.Text
{
	public sealed partial class PrivateFontCollection 
	{

		void LoadFontFile (string fileName)
		{
			CTFont nativeFont;
			var dpiSize = 0;
			var ext = Path.GetExtension(fileName);

			if (!String.IsNullOrEmpty(ext))
			{

				if (nativeFontDescriptors == null)
					nativeFontDescriptors = new Dictionary<string, CTFontDescriptor> ();

				//Try loading from Bundle first
				var fontName = fileName.Substring (0, fileName.Length - ext.Length);
				var pathForResource = NSBundle.MainBundle.PathForResource (fontName, ext.Substring(1));

				NSUrl url;

				if (!string.IsNullOrEmpty(pathForResource))
					url = NSUrl.FromFilename (pathForResource);
				else
					url = NSUrl.FromFilename (fileName);

				// We will not use CTFontManager.RegisterFontsForUrl (url, CTFontManagerScope.Process);
				// here.  The reason is that there is no way we can be sure that the font can be created to
				// to identify the family name afterwards.  So instead we will create a CGFont from a data provider.
				// create CTFont to obtain the CTFontDescriptor, store family name and font descriptor to be accessed
				// later.
				try {
					var dataProvider = new CGDataProvider (url.Path);
					var cgFont = CGFont.CreateFromProvider (dataProvider);

					try {
						nativeFont = new CTFont(cgFont, dpiSize, null);
						if (!nativeFontDescriptors.ContainsKey(nativeFont.FamilyName))
						{
							nativeFontDescriptors.Add(nativeFont.FamilyName, nativeFont.GetFontDescriptor());
							NSError error;
							var registered = CTFontManager.RegisterGraphicsFont(cgFont, out error);
							if (!registered)
								throw new ArgumentException("Error registering: " + Path.GetFileName(fileName));
						}
					}
					catch
					{
						// note: MS throw the same exception FileNotFoundException if the file exists but isn't a valid font file
						throw new System.IO.FileNotFoundException (fileName);
					}
				}
				catch (Exception)
				{
					// note: MS throw the same exception FileNotFoundException if the file exists but isn't a valid font file
					throw new System.IO.FileNotFoundException (fileName);
				}
			}
		}
	}
}

