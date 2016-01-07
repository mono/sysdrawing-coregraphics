using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using CoreGraphics;
using Foundation;
using UIKit;

namespace MTFontTest {
	public class DrawingView : UIView {

		public event PaintEventHandler Paint;

		public DrawingView (CGRect rect) : base (rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			AutoresizingMask = UIViewAutoresizing.All;
			BackgroundColor = new UIColor (0, 0, 0, 0);
			BackColor = Color.Wheat;

			var mainBundle = NSBundle.MainBundle;
		}

		#region Panel interface
		public Rectangle ClientRectangle {
			get {
				return new Rectangle ((int)Bounds.X, (int)Bounds.Y, (int)Bounds.Width, (int)Bounds.Height);
			}
		}

		Color backColor = Color.White;
		public Color BackColor {
			get {
				return backColor;
			}
			set {
				backColor = value;
			}
		}

		Font font;
		public Font Font {
			get {
				if (font == null)
					font = new Font ("Helvetica", 12f);
				return font;
			}
			set {
				font = value;
			}
		}

		public int Left  {
			get {
				return (int)Frame.Left; 
			}
			set {
				var location = new CGPoint (value, Frame.Y);
				Frame = new CGRect (location, Frame.Size);
			}
		}

		public int Right  {
			get {
				return (int)Frame.Right;
			}
			set {
				var size = Frame;
				size.Width = size.X - value;
				Frame = size;
			}
		}

		public int Top {
			get {
				return (int)Frame.Top;
			}
			set {
				var location = new CGPoint (Frame.X, value);
				Frame = new CGRect (location, Frame.Size);
			}
		}

		public int Bottom
		{
			get {
				return (int)Frame.Bottom;
			}
			set {
				var frame = Frame;
				frame.Height = frame.Y - value;
				Frame = frame;
			}
		}

		public int Width {
			get {
				return (int)Frame.Width;
			}
			set {
				var frame = Frame;
				frame.Width = value;
				Frame = frame;
			}
		}

		public int Height {
			get {
				return (int)Frame.Height;
			}
			set {
				var frame = Frame;
				frame.Height = value;
				Frame = frame;
			}
		}
#endregion

		public override void Draw (CGRect rect)
		{
			Graphics g = Graphics.FromCurrentContext();
			g.Clear(backColor);

			var clip = new CGRect ((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
			var args = new PaintEventArgs (g, clip);

			OnPaint (args);

			if(Paint != null)
				Paint (this, args);
		}

		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			currentView++;
			currentView %= totalViews;
			MarkDirty ();
			SetNeedsDisplay ();
		}

		Font anyKeyFont = new Font ("Chalkduster", 18f, FontStyle.Bold);
		Font clipFont = new Font ("Helvetica",12f, FontStyle.Bold);

		int currentView = 0;
		int totalViews = 4;

		string title = string.Empty;

		protected void OnPaint (PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.InterpolationMode = InterpolationMode.NearestNeighbor;

			g.Clear(Color.White);
			switch (currentView) {
			case 0:
				AvailableFonts (g);
				break;
			case 1:
				PrivateFonts (g);
				break;
			case 2:
				CreatePrivateFontCollection (g);
				break;
			case 3:
				ObtainFontMetrics (g);
				break;
			}

			Brush sBrush = Brushes.Black;

			g.ResetTransform ();

			if (!g.IsClipEmpty)  {
				var clipPoint = PointF.Empty;
				var clipString = string.Format ("Clip-{0}", g.ClipBounds);
				g.ResetClip ();
				var clipSize = g.MeasureString(clipString, clipFont);
				clipPoint.X = (ClientRectangle.Width / 2) - (clipSize.Width / 2);
				clipPoint.Y = 5f;
				g.DrawString (clipString, clipFont, sBrush, clipPoint );

			}

			var anyKeyPoint = PointF.Empty;
			var anyKey = "Tap screen to continue.";
			var anyKeySize = g.MeasureString(anyKey, anyKeyFont);
			anyKeyPoint.X = (ClientRectangle.Width / 2) - (anyKeySize.Width / 2);
			anyKeyPoint.Y = ClientRectangle.Height - (anyKeySize.Height + 10);
			g.DrawString(anyKey, anyKeyFont, sBrush, anyKeyPoint );

			anyKeySize = g.MeasureString(title, anyKeyFont);
			anyKeyPoint.X = (ClientRectangle.Width / 2) - (anyKeySize.Width / 2);
			anyKeyPoint.Y -= anyKeySize.Height;
			g.DrawString(title, anyKeyFont, sBrush, anyKeyPoint );

			g.Dispose();
		}


		void ObtainFontMetrics(Graphics g)
		{
			string infoString = "";	// enough space for one line of output 
			int ascent;				// font family ascent in design units 
			float ascentPixel;		// ascent converted to pixels 
			int descent;			// font family descent in design units 
			float descentPixel;		// descent converted to pixels 
			int lineSpacing;		// font family line spacing in design units 
			float lineSpacingPixel;	// line spacing converted to pixels

			var fontStyle = FontStyle.Regular;
			var fontFamily = new FontFamily ("arial");

			var font = new Font (fontFamily, 16, fontStyle, GraphicsUnit.Pixel);
			var pointF = new PointF (10, 10);
			var solidBrush = new SolidBrush(Color.Black);

			// Display the font size in pixels.
			infoString = "font family : " + font.FontFamily.Name + " " + fontStyle + ".";
			g.DrawString(infoString, font, solidBrush, pointF);

			// Move down one line.
			pointF.Y += font.Height;

			// Display the font size in pixels.
			infoString = "font.Size returns " + font.Size + ".";
			g.DrawString (infoString, font, solidBrush, pointF);

			// Move down one line.
			pointF.Y += font.Height;

			// Display the font family em height in design units.
			infoString = "fontFamily.GetEmHeight() returns " + fontFamily.GetEmHeight(fontStyle) + ".";
			g.DrawString (infoString, font, solidBrush, pointF);

			// Move down two lines.
			pointF.Y += 2 * font.Height;

			// Display the ascent in design units and pixels.
			ascent = fontFamily.GetCellAscent (fontStyle);

			// 14.484375 = 16.0 * 1854 / 2048
			ascentPixel = font.Size * ascent / fontFamily.GetEmHeight(fontStyle);
			infoString = "The ascent is " + ascent + " design units, " + ascentPixel + " pixels.";
			g.DrawString(infoString, font, solidBrush, pointF);

			// Move down one line.
			pointF.Y += font.Height;

			// Display the descent in design units and pixels.
			descent = fontFamily.GetCellDescent(fontStyle);

			// 3.390625 = 16.0 * 434 / 2048
			descentPixel = font.Size * descent / fontFamily.GetEmHeight(fontStyle);
			infoString = "The descent is " + descent + " design units, " + descentPixel + " pixels.";
			g.DrawString (infoString, font, solidBrush, pointF);

			// Move down one line.
			pointF.Y += font.Height;

			// Display the line spacing in design units and pixels.
			lineSpacing = fontFamily.GetLineSpacing(fontStyle);

			// 18.398438 = 16.0 * 2355 / 2048
			lineSpacingPixel = font.Size * lineSpacing / fontFamily.GetEmHeight(fontStyle);
			infoString = "The line spacing is " + lineSpacing + " design units, " + lineSpacingPixel + " pixels.";
			g.DrawString (infoString, font, solidBrush, pointF);
			title = "ObtainFontMetrics";
		}

		void AvailableFonts (Graphics g)
		{
			var installedFonts = new InstalledFontCollection ();

			foreach ( FontFamily ff in installedFonts.Families ) {
				Console.WriteLine (ff.ToString());
				foreach (var style in Enum.GetValues(typeof(FontStyle)) ) {
					if (ff.IsStyleAvailable ((FontStyle)style))
						Console.WriteLine (ff.ToString() + " - " + (FontStyle)style);
				}
			}

			var format = StringFormat.GenericDefault;
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;
			g.DrawString ("Please see console.", anyKeyFont, Brushes.Blue, ClientRectangle,format);
			title = "AvailableFonts";
		}

		void PrivateFonts (Graphics g)
		{
			var privateFonts = new PrivateFontCollection ();

			privateFonts.AddFontFile ("A Damn Mess.ttf");
			privateFonts.AddFontFile ("Abberancy.ttf");
			privateFonts.AddFontFile ("Abduction.ttf");
			privateFonts.AddFontFile ("American Typewriter.ttf");
			privateFonts.AddFontFile ("Paint Boy.ttf");

			foreach ( FontFamily ff in privateFonts.Families ) {
				Console.WriteLine (ff.ToString ());
				foreach (var style in Enum.GetValues (typeof(FontStyle)) ) {
					if (ff.IsStyleAvailable ((FontStyle)style))
						Console.WriteLine (ff.ToString() + " - " + (FontStyle)style);
				}
			}

			var format = StringFormat.GenericDefault;
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;
			g.DrawString ("Please see console.", anyKeyFont, Brushes.Blue, ClientRectangle,format);
			title = "PrivateFontCollection";
		}

		void CreatePrivateFontCollection (Graphics g)
		{
			var pointF = new PointF (10, 20);
			var solidBrush = new SolidBrush (Color.Black);

			int count = 0;
			string familyName = string.Empty;
			string familyNameAndStyle;
			FontFamily[] fontFamilies;
			var privateFontCollection = new PrivateFontCollection ();
			privateFontCollection.AddFontFile ("A Damn Mess.ttf");
			privateFontCollection.AddFontFile ("Abberancy.ttf");
			privateFontCollection.AddFontFile ("Abduction.ttf");
			privateFontCollection.AddFontFile ("American Typewriter.ttf");
			privateFontCollection.AddFontFile ("Paint Boy.ttf");

			// Get the array of FontFamily objects.
			fontFamilies = privateFontCollection.Families;

			// How many objects in the fontFamilies array?
			count = fontFamilies.Length;
			var fontSize = 20;

			// Display the name of each font family in the private collection 
			// along with the available styles for that font family. 
			for (int j = 0; j < count; ++j)
			{
				// Get the font family name.
				familyName = fontFamilies[j].Name;
				// Is the regular style available? 
				if (fontFamilies[j].IsStyleAvailable(FontStyle.Regular))
				{
					familyNameAndStyle = string.Empty;
					familyNameAndStyle = familyNameAndStyle + familyName;
					familyNameAndStyle = familyNameAndStyle + " Regular";
					var regFont = new Font(
						familyName,
						fontSize,
						FontStyle.Regular,
						GraphicsUnit.Pixel);

					g.DrawString(
						familyNameAndStyle,
						regFont,
						solidBrush,
						pointF);

					pointF.Y += regFont.Height;
				}

				// Is the bold style available? 
				if (fontFamilies[j].IsStyleAvailable(FontStyle.Bold))
				{
					familyNameAndStyle = string.Empty;
					familyNameAndStyle = familyNameAndStyle + familyName;
					familyNameAndStyle = familyNameAndStyle + " Bold";

					var boldFont = new Font(
						familyName,
						fontSize,
						FontStyle.Bold,
						GraphicsUnit.Pixel);

					g.DrawString(familyNameAndStyle, boldFont, solidBrush, pointF);
					pointF.Y += boldFont.Height;
				}

				// Is the italic style available? 
				if (fontFamilies[j].IsStyleAvailable(FontStyle.Italic))
				{
					familyNameAndStyle = string.Empty;
					familyNameAndStyle = familyNameAndStyle + familyName;
					familyNameAndStyle = familyNameAndStyle + " Italic";

					var italicFont = new Font(
						familyName,
						fontSize,
						FontStyle.Italic,
						GraphicsUnit.Pixel);

					g.DrawString(
						familyNameAndStyle,
						italicFont,
						solidBrush,
						pointF);

					pointF.Y += italicFont.Height;
				}

				// Is the bold italic style available? 
				if (fontFamilies[j].IsStyleAvailable(FontStyle.Italic) && fontFamilies[j].IsStyleAvailable(FontStyle.Bold))
				{
					familyNameAndStyle = string.Empty;
					familyNameAndStyle = familyNameAndStyle + familyName;
					familyNameAndStyle = familyNameAndStyle + "BoldItalic";

					var italicFont = new Font(
						familyName,
						26,
						FontStyle.Italic | FontStyle.Bold,
						GraphicsUnit.Pixel);

					g.DrawString(
						familyNameAndStyle,
						italicFont,
						solidBrush,
						pointF);

					pointF.Y += italicFont.Height;
				}
				// Is the underline style available? 
				if (fontFamilies[j].IsStyleAvailable(FontStyle.Underline))
				{
					familyNameAndStyle = string.Empty;
					familyNameAndStyle = familyNameAndStyle + familyName;
					familyNameAndStyle = familyNameAndStyle + " Underline";

					var underlineFont = new Font(
						familyName,
						fontSize,
						FontStyle.Underline,
						GraphicsUnit.Pixel);

					g.DrawString(
						familyNameAndStyle,
						underlineFont,
						solidBrush,
						pointF);

					pointF.Y += underlineFont.Height;
				}

				// Is the strikeout style available? 
				if (fontFamilies[j].IsStyleAvailable(FontStyle.Strikeout)) {
					familyNameAndStyle = string.Empty;
					familyNameAndStyle = familyNameAndStyle + familyName;
					familyNameAndStyle = familyNameAndStyle + " Strikeout";

					var strikeFont = new Font(
						familyName,
						fontSize,
						FontStyle.Strikeout,
						GraphicsUnit.Pixel);

					g.DrawString (
						familyNameAndStyle,
						strikeFont,
						solidBrush,
						pointF);

					pointF.Y += strikeFont.Height;
				}

				pointF.Y += 10;
			}

			title = "CreatePrivateFontCollection";
		}

	}


}

public delegate void PaintEventHandler(object sender, PaintEventArgs e);


public class PaintEventArgs : EventArgs, IDisposable {
	readonly CGRect clipRect;
	Graphics graphics;

	public PaintEventArgs(Graphics graphics, CGRect clipRect)
	{
		if (graphics == null) {
			throw new ArgumentNullException("graphics");
		}
		this.graphics = graphics;
		this.clipRect = clipRect;
	}

	public void Dispose()
	{
		this.Dispose(true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if ((disposing && (this.graphics != null))) {
			this.graphics.Dispose();
		}
	}

	~PaintEventArgs()
	{
		this.Dispose(false);
	}

	public CGRect ClipRectangle {
		get {
			return this.clipRect;
		}
	}

	public Graphics Graphics {
		get {
			return this.graphics;
		}
	}

}
