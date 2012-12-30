using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using MonoTouch.UIKit;

namespace MTExample5_6
{
	public class PlotPanel : UIView {

		public event PaintEventHandler Paint;

		public PlotPanel (RectangleF rect) : base (rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			this.AutoresizingMask = UIViewAutoresizing.All;
			this.BackColor = Color.Wheat;

			// Set Form1 size:
//			this.Width = 350;
//			this.Height = 300;
			// Sub Chart parameters:
			// Subscribing to a paint eventhandler to drawingPanel: 
		}

		#region Panel interface
		public Rectangle ClientRectangle 
		{
			get {
				return new Rectangle((int)Bounds.X,
				                      (int)Bounds.Y,
				                      (int)Bounds.Width,
				                      (int)Bounds.Height);
			}
		}

		public Color BackColor 
		{
			get {
				float red;
				float green;
				float blue;
				float alpha;
				BackgroundColor.GetRGBA(out red, out green, out blue, out alpha);
				return Color.FromArgb((int)alpha, (int)red, (int)green, (int)blue);
			}

			set {
				var bgc = value;
				BackgroundColor = UIColor.FromRGBA(bgc.R,bgc.G,bgc.B, bgc.A);

			}
		}

		Font font;
		public Font Font
		{
			get {
				if (font == null)
					font = new Font("Helvetica",12);
				return font;
			}
			set 
			{
				font = value;
			}
		}

		public int Left 
		{
			get { return (int)Frame.Left; }

			set {
				var location = new PointF(value, Frame.Y);
				Frame = new RectangleF(location, Frame.Size);
			}

		}

		public int Right 
		{
			get { return (int)Frame.Right; }

			set { 
				var size = Frame;
				size.Width = size.X - value;
				Frame = size;
			}
			
		}

		public int Top
		{
			get { return (int)Frame.Top; }
			set { 
				var location = new PointF(Frame.X, value);
				Frame = new RectangleF(location, Frame.Size);

			}
		}

		public int Bottom
		{
			get { return (int)Frame.Bottom; }
			set { 
				var frame = Frame;
				frame.Height = frame.Y - value;
				Frame = frame;

			}
		}

		public int Width 
		{
			get { return (int)Frame.Width; }
			set { 
				var frame = Frame;
				frame.Width = value;
				Frame = frame;

			}
		}

		public int Height
		{
			get { return (int)Frame.Height; }
			set { 
				var frame = Frame;
				frame.Height = value;
				Frame = frame;

			}
		}
		#endregion


		public override void Draw (RectangleF dirtyRect)
		{
			if(Paint != null)
			{
				Graphics g = new Graphics();
				Rectangle clip = new Rectangle((int)dirtyRect.X,
				                               (int)dirtyRect.Y,
				                               (int)dirtyRect.Width,
				                               (int)dirtyRect.Height);
				
				var args = new PaintEventArgs(g, clip);

				Paint(this, args);
			}

			 
		}
	}

}

public delegate void PaintEventHandler(object sender, PaintEventArgs e);


public class PaintEventArgs : EventArgs, IDisposable
{
	private readonly Rectangle clipRect;
	private Graphics graphics;

	public PaintEventArgs(Graphics graphics, Rectangle clipRect)
	{
		if (graphics == null)
		{
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
		if ((disposing && (this.graphics != null)))
		{
			this.graphics.Dispose();
		}
	}
	
	~PaintEventArgs()
	{
		this.Dispose(false);
	}
	
	public Rectangle ClipRectangle
	{
		get
		{
			return this.clipRect;
		}
	}
	
	public Graphics Graphics
	{
		get
		{
			return this.graphics;
		}
	}
	
}
