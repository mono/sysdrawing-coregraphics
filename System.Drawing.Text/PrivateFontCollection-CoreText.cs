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
		List<string> familyNames = new List<string> ();
		List<CTFont> nativeFonts = new List<CTFont> ();

		void LoadFontFile (string fileName)
		{
			CTFont nativeFont;
			var dpiSize = 0;
			var ext = Path.GetExtension(fileName);

			//Try loading from Bundle first
			if (!String.IsNullOrEmpty(ext))
			{
				var fontName = fileName.Substring (0, fileName.Length - ext.Length);
				var pathForResource = NSBundle.MainBundle.PathForResource (fontName, ext.Substring(1));

				try {
					var dataProvider = new CGDataProvider (pathForResource);
					var cgFont = CGFont.CreateFromProvider (dataProvider);

					try {
						nativeFont = new CTFont(cgFont, dpiSize, null);
						familyNames.Add(nativeFont.FamilyName);
						nativeFonts.Add(nativeFont);
					}
					catch
					{
						throw new System.IO.FileNotFoundException (fileName);
					}
				}
				catch (Exception)
				{
					try {
						nativeFont = new CTFont(Path.GetFileNameWithoutExtension(fileName),dpiSize);
						familyNames.Add(nativeFont.FamilyName);
						nativeFonts.Add(nativeFont);
					}
					catch
					{
						throw new System.IO.FileNotFoundException (fileName);
					}	


				}
			}
		}
	}
}

