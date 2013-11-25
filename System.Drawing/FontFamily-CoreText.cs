using System;

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

		private void CreateNativeFontFamily(string familyName, bool createDefaultIfNotExists)
		{
			nativeFontDescriptor = new CTFontDescriptor (familyName, 0);

			if (nativeFontDescriptor == null && createDefaultIfNotExists) 
			{
				nativeFontDescriptor = new CTFontDescriptor (SANS_SERIF, 0);
			}
		}
	}
}

