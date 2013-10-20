//
// System.Drawing.Drawing2D.GraphicsPath-DrawString.cs
//
// Author:
//   Kenneth J. Pouncey (kjpou@pt.lu)
//
// Copyright 2011 Xamarin Inc.
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
using System.Drawing;

#if MONOMAC
using MonoMac.CoreGraphics;
using MonoMac.AppKit;
using MonoMac.Foundation;
using MonoMac.CoreText;
#else
using MonoTouch.CoreGraphics;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreText;
#endif

namespace System.Drawing.Drawing2D 
{

	public partial class GraphicsPath
	{
		public void AddString(string s,	FontFamily family,	int style, float emSize, Point origin, StringFormat format)
		{
			var font = new Font ("ChalkDuster", emSize, (FontStyle)style);
			var attString = buildAttributedString (s, font);

		}

		private NSMutableAttributedString buildAttributedString(string text, Font font, 
		                                                        Color? fontColor=null) 
		{

			// Create a new attributed string definition
			var ctAttributes = new CTStringAttributes ();

			// Font attribute
			ctAttributes.Font = font.nativeFont;
			// -- end font 

			if (fontColor.HasValue) {

				// Font color
				var fc = fontColor.Value;
				var cgColor = new CGColor(fc.R / 255f, 
				                          fc.G / 255f,
				                          fc.B / 255f,
				                          fc.A / 255f);

				ctAttributes.ForegroundColor = cgColor;
				ctAttributes.ForegroundColorFromContext = false;
				// -- end font Color
			}

			if (font.Underline) {
				// Underline
#if MONOMAC
				int single = (int)MonoMac.AppKit.NSUnderlineStyle.Single;
				int solid = (int)MonoMac.AppKit.NSUnderlinePattern.Solid;
				var attss = single | solid;
				ctAttributes.UnderlineStyleValue = attss;
#else
				ctAttributes.UnderlineStyleValue = 1;
#endif
				// --- end underline
			}


			if (font.Strikeout) {
				// StrikeThrough
				//				NSColor bcolor = NSColor.Blue;
				//				NSObject bcolorObject = new NSObject(bcolor.Handle);
				//				attsDic.Add(NSAttributedString.StrikethroughColorAttributeName, bcolorObject);
				//				#if MACOS
				//				int stsingle = (int)MonoMac.AppKit.NSUnderlineStyle.Single;
				//				int stsolid = (int)MonoMac.AppKit.NSUnderlinePattern.Solid;
				//				var stattss = stsingle | stsolid;
				//				var stunderlineObject = NSNumber.FromInt32(stattss);
				//				#else
				//				var stunderlineObject = NSNumber.FromInt32 (1);
				//				#endif
				//
				//				attsDic.Add(StrikethroughStyleAttributeName, stunderlineObject);
				// --- end underline
			}


			// Text alignment
			var alignment = CTTextAlignment.Left;
			var alignmentSettings = new CTParagraphStyleSettings();
			alignmentSettings.Alignment = alignment;
			var paragraphStyle = new CTParagraphStyle(alignmentSettings);

			ctAttributes.ParagraphStyle = paragraphStyle;
			// end text alignment

			NSMutableAttributedString atts = 
				new NSMutableAttributedString(text,ctAttributes.Dictionary);

			return atts;

		}
	}
}

