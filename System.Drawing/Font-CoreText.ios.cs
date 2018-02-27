//
// System.Drawing.Font-CoreText.cs
//
// Author:
//   Kenneth J. Pouncey (kjpou@pt.lu)
//   Jiri Volejnik <aconcagua21@volny.cz>
//   Filip Navara <filip.navara@gmail.com>
//   Miguel de Icaza (miguel@microsoft.com)
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
using System.Drawing.Mac;
using CoreGraphics;
using CoreText;
using Foundation;
using UIKit;

namespace System.Drawing
{
	public partial class Font
	{
		internal Font (UIFont font)
		{
			var traits = font.FontDescriptor.SymbolicTraits;

			fontFamily = new FontFamily (font.FamilyName, true);
			fontStyle = (traits.HasFlag (UIFontDescriptorSymbolicTraits.Bold) ? FontStyle.Bold : 0) |
				(traits.HasFlag (UIFontDescriptorSymbolicTraits.Italic) ? FontStyle.Italic : 0);
			
			gdiVerticalFont = false;
			gdiCharSet = DefaultCharSet;
			sizeInPoints = (float)(font.PointSize * 72f / 96f);
			size = (float)font.PointSize;
			unit = GraphicsUnit.Pixel;

			nativeFont = font.ToCTFont ();
		}

		UIFont MakeFont (string name, float fsize, UIFontDescriptorSymbolicTraits traits)
		{
			var desc = UIFontDescriptor.FromName (name, fsize);
			desc = desc.CreateWithTraits (traits);
			return UIFont.FromDescriptor (desc, fsize);
		}

		void CreateNativeFont (FontFamily familyName, float emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet, bool gdiVerticalFont)
		{
			this.sizeInPoints = ConversionHelpers.GraphicsUnitConversion (unit, GraphicsUnit.Point, 96f, emSize);
			this.bold = FontStyle.Bold == (style & FontStyle.Bold);
			this.italic = FontStyle.Italic == (style & FontStyle.Italic);
			this.underLine = FontStyle.Underline == (style & FontStyle.Underline);
			this.size = emSize;
			this.unit = unit;

			var size = sizeInPoints * 96f / 72f;
			UIFontDescriptorSymbolicTraits traits = 0;
			if (bold)
				traits |= UIFontDescriptorSymbolicTraits.Bold;
			if (italic)
				traits |= UIFontDescriptorSymbolicTraits.Italic;

			UIFont font = null;
			if (NSThread.Current.IsMainThread)
				font = MakeFont (familyName.Name, size, traits);
			else
				NSThread.MainThread.InvokeOnMainThread (() => { font = MakeFont (familyName.Name, size, traits); });
			
			this.nativeFont = font.ToCTFont ();
		}
	}
}
