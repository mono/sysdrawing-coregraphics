using System;
using System.Drawing;
using CoreGraphics;
using UIKit;

namespace MTExample5_5 {
	public class PlotPanel : UIView {
		public event PaintEventHandler Paint;

		public PlotPanel(CGRect rect) : base(rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			BackColor = Color.Wheat;
		}

		#region Panel interface
		public CGRect ClientRectangle {
			get {
				return new CGRect((int)Bounds.X, (int)Bounds.Y, (int)Bounds.Width, (int)Bounds.Height);
			}
		}

		public Color BackColor {
			get {
				nfloat red;
				nfloat green;
				nfloat blue;
				nfloat alpha;
				BackgroundColor.GetRGBA (out red, out green, out blue, out alpha);
				return Color.FromArgb ((int)alpha, (int)red, (int)green, (int)blue);
			}
			set {
				var bgc = value;
				BackgroundColor = UIColor.FromRGBA (bgc.R, bgc.G, bgc.B, bgc.A);
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

		public int Left {
			get {
				return (int)Frame.Left;
			}
			set {
				var location = new CGPoint (value, Frame.Y);
				Frame = new CGRect(location, Frame.Size);
			}
		}

		public int Right {
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

		public int Bottom {
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
			if (Paint != null) {
				Graphics g = Graphics.FromCurrentContext();
				var clip = new CGRect ((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
				var args = new PaintEventArgs (g, clip);
				Paint(this, args);
			}
		}
	}

}

public delegate void PaintEventHandler(object sender, PaintEventArgs e);


public class PaintEventArgs : EventArgs, IDisposable {
	readonly CGRect clipRect;
	Graphics graphics;

	public PaintEventArgs (Graphics graphics, CGRect clipRect)
	{
		if (graphics == null)
			throw new ArgumentNullException ("graphics");
		this.graphics = graphics;
		this.clipRect = clipRect;
	}

	public void Dispose ()
	{
		this.Dispose (true);
		GC.SuppressFinalize (this);
	}

	protected virtual void Dispose (bool disposing)
	{
		if ((disposing && (this.graphics != null))) {
			this.graphics.Dispose ();
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