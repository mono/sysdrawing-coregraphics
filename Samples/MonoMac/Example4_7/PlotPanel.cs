
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing.Drawing2D;

using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.CoreGraphics;
using MonoMac.CoreText;
using System.Drawing;

namespace Example4_7
{
	public partial class PlotPanel : MonoMac.AppKit.NSView
	{
		public event PaintEventHandler Paint;

		#region Constructors
		
		// Called when created from unmanaged code
		public PlotPanel (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public PlotPanel (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		// Shared initialization code
		void Initialize ()
		{
			this.AutoresizingMask = NSViewResizingMask.HeightSizable | NSViewResizingMask.WidthSizable;
		}

		public PlotPanel (RectangleF rect) : base (rect)
		{
			Initialize();
		}
		
#endregion
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
		
		Color backColor = Color.White;
		public Color BackColor 
		{
			get {
				return backColor;
			}
			
			set {
				backColor = value;
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
			get { 

				return (int)Frame.Left; 
			}
			
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
				this.NeedsDisplay = true;

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

		public override void DrawRect (System.Drawing.RectangleF dirtyRect)
		{
			if(Paint != null)
			{
				Graphics g = new Graphics();
				g.Clear(backColor);

				Rectangle clip = new Rectangle((int)dirtyRect.X,
				                               (int)dirtyRect.Y,
				                               (int)dirtyRect.Width,
				                               (int)dirtyRect.Height);

				var args = new PaintEventArgs(g, clip);
				
				Paint(this, args);
			}
			
		}


		// We make sure we are flipped here so our Frame object calculations are set up
		// correctly when this sub view is resized and repositioned.
		public override bool IsFlipped {
			get {
				//return base.IsFlipped;
				return true;
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
