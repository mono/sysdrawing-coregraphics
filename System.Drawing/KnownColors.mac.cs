//
// System.Drawing.KnownColors
//
// Authors:
//	Gonzalo Paniagua Javier (gonzalo@ximian.com)
//	Peter Dennis Bartok (pbartok@novell.com)
//	Sebastien Pouliot  <sebastien@ximian.com>
//      Jiri Volejnik <aconcagua21@volny.cz>
//      Filip Navara <filip.navara@gmail.com>
//
// Copyright (C) 2007 Novell, Inc (http://www.novell.com)
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

using Foundation;
using System.Drawing.Mac;
using System.Collections.Generic;
using AppKit;

namespace System.Drawing
{

	internal static partial class KnownColors
	{
		static void Update ()
		{
			// note: Mono's SWF Theme class will call the static Update method to apply
			// correct system colors outside Windows

			ArgbValues [(int)KnownColor.ActiveBorder] = NSColor.WindowFrame.ToUArgb ();
			// KnownColor.ActiveCaption
			// KnownColor.ActiveCaptionText
			// KnownColor.AppWorkspace
			ArgbValues [(int)KnownColor.Control] = NSColor.Control.ToUArgb ();
			ArgbValues [(int)KnownColor.ControlText] = NSColor.ControlText.ToUArgb ();
			ArgbValues [(int)KnownColor.ControlDark] = NSColor.ControlShadow.ToUArgb ();
			ArgbValues [(int)KnownColor.ControlDarkDark] = NSColor.ControlDarkShadow.ToUArgb ();
			ArgbValues [(int)KnownColor.ControlLight] = NSColor.ControlHighlight.ToUArgb ();
			ArgbValues [(int)KnownColor.ControlLightLight] = NSColor.ControlLightHighlight.ToUArgb ();
			// KnownColor.Desktop
			ArgbValues [(int)KnownColor.GrayText] = NSColor.DisabledControlText.ToUArgb ();
			//ArgbValues[(int)KnownColor.Highlight] = NSColor.Highlight.ToUArgb();
			ArgbValues [(int)KnownColor.Highlight] = NSColor.SelectedTextBackground.ToUArgb ();
			ArgbValues [(int)KnownColor.HighlightText] = NSColor.SelectedText.ToUArgb ();
			// KnownColor.HighlightText
			// KnownColor.HotTrack
			// KnownColor.InactiveBorder
			// KnownColor.InactiveCaption
			// KnownColor.InactiveCaptionText
			// KnownColor.Info
			// KnownColor.InfoText
			// KnownColor.Menu
			// KnownColor.MenuText
			ArgbValues [(int)KnownColor.ScrollBar] = NSColor.ScrollBar.ToUArgb ();
			//ArgbValues[(int)KnownColor.Window] = NSColor.WindowBackground.ToUArgb();
			//ArgbValues[(int)KnownColor.WindowFrame] = NSColor.WindowFrame.ToUArgb();
			//ArgbValues[(int)KnownColor.WindowText] = NSColor.WindowFrameText.ToUArgb();
			ArgbValues [(int)KnownColor.ButtonFace] = NSColor.Control.ToUArgb ();
			ArgbValues [(int)KnownColor.ButtonHighlight] = NSColor.ControlHighlight.ToUArgb ();
			ArgbValues [(int)KnownColor.ButtonShadow] = NSColor.ControlShadow.ToUArgb ();
			// KnownColor.GradientActiveCaption
			// KnownColor.GradientInactiveCaption
			// KnownColor.MenuBar
			// KnownColor.MenuHighlight
		}

		static void WatchColorChanges ()
		{
			NSNotificationCenter.DefaultCenter.AddObserver (new NSString ("NSSystemColorsDidChangeNotification"), (obj) => { Update (); });
		}
	}
}
