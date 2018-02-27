using System;
using Foundation;
using UIKit;

namespace System.Drawing
{

	public sealed partial class SystemFonts {

		public static Font CaptionFont {
			get {
				//if (captionFont == null) 
				//	dispatcher.InvokeOnMainThread (() => { captionFont = new Font (NSFont.TitleBarFontOfSize (NSFont.SystemFontSize)); });
				return captionFont;
			}
		}

		public static Font DefaultFont {
			get {
				//if (defaultFont == null)
				//	dispatcher.InvokeOnMainThread (() => { defaultFont = new Font (NSFont.LabelFontOfSize (NSFont.SmallSystemFontSize)); });
				return defaultFont;
			}
		}

		public static Font DialogFont {
			get {
				//if (dialogFont == null)
				//	dispatcher.InvokeOnMainThread (() => { dialogFont = new Font (NSFont.LabelFontOfSize (NSFont.LabelFontSize)); });
				return dialogFont;
			}
		}

		public static Font IconTitleFont {
			get {
				//if (iconTitleFont == null)
				//	dispatcher.InvokeOnMainThread (() => { iconTitleFont = new Font (NSFont.LabelFontOfSize (NSFont.LabelFontSize)); });
				return iconTitleFont;
			}
		}

		public static Font MenuFont {
			get {
				//if (menuFont == null)
				//	dispatcher.InvokeOnMainThread (() => { menuFont = new Font (NSFont.MenuFontOfSize (NSFont.SystemFontSize)); });
				return menuFont;
			}
		}

		public static Font MessageBoxFont {
			get {
				//if (messageBoxFont == null)
				//	dispatcher.InvokeOnMainThread (() => { messageBoxFont = new Font (NSFont.SystemFontOfSize (NSFont.SmallSystemFontSize)); });
				return messageBoxFont;
			}
		}

		public static Font SmallCaptionFont {
			get {
				//if (smallCaptionFont == null)
				//	dispatcher.InvokeOnMainThread (() => { smallCaptionFont = new Font (NSFont.TitleBarFontOfSize (NSFont.SmallSystemFontSize)); });
				return smallCaptionFont;
			}
		}

		public static Font StatusFont {
			get {
				//if (statusFont == null)
				//	dispatcher.InvokeOnMainThread (() => { statusFont = new Font (NSFont.LabelFontOfSize (NSFont.LabelFontSize)); });
				return statusFont;
			}
		}
	}
}
