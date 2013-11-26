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

		//internal Dictionary<string, CTFontDescriptor> nativeFontDescriptors = new Dictionary<string, CTFontDescriptor> ();

		void LoadFontFile (string fileName)
		{
			CTFont nativeFont;
			var dpiSize = 0;
			var ext = Path.GetExtension(fileName);

			//Try loading from Bundle first
			if (!String.IsNullOrEmpty(ext))
			{

				if (nativeFontDescriptors == null)
					nativeFontDescriptors = new Dictionary<string, CTFontDescriptor> ();

				var fontName = fileName.Substring (0, fileName.Length - ext.Length);
				var pathForResource = NSBundle.MainBundle.PathForResource (fontName, ext.Substring(1));

				try {
					var dataProvider = new CGDataProvider (pathForResource);
					var cgFont = CGFont.CreateFromProvider (dataProvider);

					try {
						nativeFont = new CTFont(cgFont, dpiSize, null);
						if (!nativeFontDescriptors.ContainsKey(nativeFont.FamilyName))
						{
							nativeFontDescriptors.Add(nativeFont.FamilyName, nativeFont.GetFontDescriptor());
						}
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
						if (!nativeFontDescriptors.ContainsKey(nativeFont.FamilyName))
						{
							nativeFontDescriptors.Add(nativeFont.FamilyName, nativeFont.GetFontDescriptor());
						}
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

