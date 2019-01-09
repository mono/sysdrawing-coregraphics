using System;
using Foundation;
using CoreText;

namespace System.Drawing {

	public sealed partial class SystemFonts {
		static SystemFonts ()
		{
			CaptionFont = new Font(new CTFont(CTFontUIFontType.WindowTitle, 0, null));
			DefaultFont = new Font(new CTFont(CTFontUIFontType.ControlContent, 0, null));
			DialogFont = new Font(new CTFont(CTFontUIFontType.Label, 0, null));
			IconTitleFont = new Font(new CTFont(CTFontUIFontType.Label, 0, null));
			MenuFont = new Font(new CTFont(CTFontUIFontType.MenuItem, 0, null));
			MessageBoxFont = new Font(new CTFont(CTFontUIFontType.System, 0, null));
			SmallCaptionFont = new Font(new CTFont(CTFontUIFontType.UtilityWindowTitle, 0, null));
			StatusFont = new Font(new CTFont(CTFontUIFontType.Label, 0, null));
		}

		public static Font GetFontByName (string systemFontName)
		{
			if (systemFontName == "CaptionFont")
				return CaptionFont;
			if (systemFontName == "DefaultFont")
				return DefaultFont;
			if (systemFontName == "DialogFont")
				return DialogFont;
			if (systemFontName == "IconTitleFont")
				return IconTitleFont;
			if (systemFontName == "MenuFont")
				return MenuFont;
			if (systemFontName == "MessageBoxFont")
				return MessageBoxFont;
			if (systemFontName == "SmallCaptionFont")
				return SmallCaptionFont;
			if (systemFontName == "StatusFont")
				return StatusFont;
			return null;
		}

		public static Font CaptionFont { get; private set; }
		public static Font DefaultFont { get; private set; }
		public static Font DialogFont { get; private set; }
		public static Font IconTitleFont { get; private set; }
		public static Font MenuFont { get; private set; }
		public static Font MessageBoxFont { get; private set; }
		public static Font SmallCaptionFont { get; private set; }
		public static Font StatusFont { get; private set; }
	}
}
