using System;
using System.Drawing.Text;

#if MONOMAC
using MonoMac.AppKit;
using MonoMac.Foundation;
using MonoMac.CoreGraphics;
using MonoMac.CoreText;
#else
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreGraphics;
using MonoTouch.CoreText;
#endif

namespace System.Drawing
{
	public sealed partial class FontFamily
	{
		const string MONO_SPACE = "Courier";
		const string SANS_SERIF = "Helvetica";
		const string SERIF = "Times";

		CTFontDescriptor nativeFontDescriptor;

		internal CTFontDescriptor NativeDescriptor
		{
			get { return nativeFontDescriptor; }
		}

		private void CreateNativeFontFamily(string name, bool createDefaultIfNotExists)
		{
			CreateNativeFontFamily (name, null, createDefaultIfNotExists);
		}

		private void CreateNativeFontFamily(string name, FontCollection fontCollection, bool createDefaultIfNotExists)
		{
			if (fontCollection != null) 
			{
				if (fontCollection.nativeFontDescriptors.ContainsKey (name))
					nativeFontDescriptor = fontCollection.nativeFontDescriptors [name];

				if (nativeFontDescriptor == null && createDefaultIfNotExists) 
				{
					nativeFontDescriptor = new CTFontDescriptor (SANS_SERIF, 0);
				}
			} 
			else 
			{
				nativeFontDescriptor = new CTFontDescriptor (name, 0);

				if (nativeFontDescriptor == null && createDefaultIfNotExists) 
				{
					nativeFontDescriptor = new CTFontDescriptor (SANS_SERIF, 0);
				}
			}

			if (nativeFontDescriptor == null)
				throw new ArgumentException ("name specifies a font that is not installed on the computer running the application.");
			else 
			{
				var attrs = nativeFontDescriptor.GetAttributes ();
				var dic = attrs.Dictionary;
				familyName = attrs.FamilyName;
				// if the font description attributes do not contain a value for FamilyName then we
				// need to try and create the font to get the family name from the actual font.
				if (string.IsNullOrEmpty (familyName)) 
				{
					var font = new CTFont (nativeFontDescriptor, 0);
					familyName = font.FamilyName;
				}
			}

		}

	}
}

