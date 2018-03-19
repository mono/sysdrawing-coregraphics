//
// System.Drawing.Font-CoreText.cs
//
// Author:
//   Kenneth J. Pouncey (kjpou@pt.lu)
//
// Copyright 2011-2013 Xamarin Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;

using CoreGraphics;
using CoreText;
using Foundation;

namespace System.Drawing
{
	public partial class Font
	{
		internal CTFont nativeFont;

		internal static string PreferredLanguage {
			get {
				var languages = NSLocale.PreferredLanguages;
				return languages.Length > 0 ? languages [0] : "en-US";
			}
		}

		internal Font(CTFont font)
		{
			var traits = font.GetTraits().SymbolicTraits.GetValueOrDefault();
			fontFamily = new FontFamily(font.FamilyName, true);
			fontStyle |= traits.HasFlag(CTFontSymbolicTraits.Bold) ? FontStyle.Bold : 0;
			fontStyle |= traits.HasFlag(CTFontSymbolicTraits.Italic) ? FontStyle.Italic : 0;
			gdiVerticalFont = false;
			gdiCharSet = DefaultCharSet;
			sizeInPoints = (float)(font.Size * 72f / 96f);
			size = (float)font.Size;
			unit = GraphicsUnit.Pixel;
			nativeFont = font;
		}

		void CreateNativeFont(FontFamily family, float emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet, bool gdiVerticalFont)
		{
			this.sizeInPoints = ConversionHelpers.GraphicsUnitConversion(unit, GraphicsUnit.Point, 96f, emSize);
			this.size = emSize;
			this.unit = unit;

			var size = sizeInPoints * 96f / 72f;

			var traits = CTFontSymbolicTraits.None;
			traits |= Bold ? CTFontSymbolicTraits.Bold : 0;
			traits |= Italic ? CTFontSymbolicTraits.Italic : 0;

			this.nativeFont = CTFontWithFamily(family, traits, size);
		}

		static CTFont CTFontWithFamily(FontFamily family, CTFontSymbolicTraits traits, float size)
		{
			// Semibold font hack
			if (FontFamily.RemoveSemiboldSuffix(family.Name, out string familyName))
			    if (CTFontWithFamilyName(familyName, traits, size, CTFontWeight.Semibold) is CTFont semibold)
					return semibold;
			
			var	font = CTFontWithFamily(family, size);
			var mask = (CTFontSymbolicTraits)uint.MaxValue;
			var fontWithTraits = font.WithSymbolicTraits(size, traits, mask);
			return fontWithTraits ?? font;
		}

		static CTFont CTFontWithFamily(FontFamily family, float size)
		{
			return IsFontInstalled(family.NativeDescriptor)
				? new CTFont(family.NativeDescriptor, size)
				: new CTFont(CTFontUIFontType.System, size, PreferredLanguage);
		}
		
		static CTFont CTFontWithFamilyName(string family, CTFontSymbolicTraits straits, float size, float weight)
		{
			var bold = Math.Abs(weight - CTFontWeight.Bold) < 0.01f;
			var traits = new NSMutableDictionary();

			if (Math.Abs(weight) > 0.01 && !bold)
				traits[CTFontTraitKey.Weight] = NSNumber.FromFloat(weight);

			if (bold)
				straits |= CTFontSymbolicTraits.Bold;

			if (0 != (straits & (CTFontSymbolicTraits.Bold | CTFontSymbolicTraits.Italic)))
				traits[CTFontTraitKey.Symbolic] = NSNumber.FromUInt32((UInt32)straits);

			var attrs = new NSMutableDictionary();
			attrs[CTFontDescriptorAttributeKey.FamilyName] = (NSString)family;
			attrs[CTFontDescriptorAttributeKey.Traits] = traits;

			var desc = new CTFontDescriptor(new CTFontDescriptorAttributes(attrs));
			var font = new CTFont(desc, size);

			return font;
		}

		static bool IsFontInstalled(CTFontDescriptor descriptor)
		{
			var matching = descriptor.GetMatchingFontDescriptors((NSSet)null);
			return matching != null && matching.Length > 0;
		}

	

		/**
		 * 
		 * Returns: The line spacing, in pixels, of this font.
		 * 
		 * The line spacing of a Font is the vertical distance between the base lines of 
		 * two consecutive lines of text. Thus, the line spacing includes the blank space 
		 * between lines along with the height of the character itself.
		 * 
		 * If the Unit property of the font is set to anything other than GraphicsUnit.Pixel, 
		 * the height (in pixels) is calculated using the vertical resolution of the 
		 * screen display. For example, suppose the font unit is inches and the font size 
		 * is 0.3. Also suppose that for the corresponding font family, the em-height 
		 * is 2048 and the line spacing is 2355. For a screen display that has a vertical 
		 * resolution of 96 dots per inch, you can calculate the height as follows:
		 * 
		 * 2355*(0.3/2048)*96 = 33.11719
		 * 
		 **/
		float GetNativeheight ()
		{
			return (float)NMath.Ceiling(nativeFont.AscentMetric + nativeFont.DescentMetric + nativeFont.LeadingMetric + 1);
		}

		static class CTFontWeight
		{
			public const float UltraLight = -0.8f;
			public const float Thin = -0.6f;
			public const float Light = -0.4f;
			public const float Regular = 0.0f;
			public const float Medium = 0.23f;
			public const float Semibold = 0.3f;
			public const float Bold = 0.4f;
			public const float Heavy = 0.56f;
			public const float Black = 0.62f;
		}		
	}
}

