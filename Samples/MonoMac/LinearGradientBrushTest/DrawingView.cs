
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.CoreGraphics;
using System.Drawing;

namespace LinearGradientBrushTest
{
	public partial class DrawingView : MonoMac.AppKit.NSView
	{
		
		public event PaintEventHandler Paint;
		
		#region Constructors
		
		// Called when created from unmanaged code
		public DrawingView (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public DrawingView (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		// Shared initialization code
		void Initialize ()
		{
			this.AutoresizingMask = NSViewResizingMask.HeightSizable | NSViewResizingMask.WidthSizable;
			BackColor = Color.Wheat;
		}
		
		public DrawingView (RectangleF rect) : base (rect)
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
			Graphics g = new Graphics();
			g.Clear(backColor);
			
			Rectangle clip = new Rectangle((int)dirtyRect.X,
			                               (int)dirtyRect.Y,
			                               (int)dirtyRect.Width,
			                               (int)dirtyRect.Height);
			
			var args = new PaintEventArgs(g, clip);
			
			OnPaint(args);
			
			if(Paint != null)
			{
				Paint(this, args);
			}
			
		}

		public override bool AcceptsFirstResponder ()
		{
			return true;
		}

		int currentView = 0;
		int totalViews = 12;

		public override void KeyDown (NSEvent theEvent)
		{
			currentView++;
			currentView %= totalViews;
			//Console.WriteLine("Current View: {0}", currentView);
			this.NeedsDisplay = true;
		}


		protected void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;

			switch (currentView) 
			{
			case 0:
				PaintView0 (g);
				break;
			case 1:
				PaintView1 (g);
				break;
			case 2:
				PaintView2 (g);
				break;
			case 3:
				PaintView3 (g);
				break;
			case 4:
				PaintView4 (g);
				break;
			case 5:
				PaintView5 (g);
				break;
			case 6:
				PaintView6 (g);
				break;
			case 7:
				PaintView7 (g);
				break;
			case 8:
				PaintView8 (g);
				break;
			case 9:
				PaintView9 (g);
				break;
			case 10:
				PaintView10 (g);
				break;
			case 11:
				PaintView11 (g);
				break;
			};

			g.ResetTransform();
			Font viewFont = new Font("Helvetica", 24, FontStyle.Bold);
			Brush sBrush = Brushes.Black;
			PointF anyKeyPoint = PointF.Empty;
			var anyKey = "Press any key to continue.";
			SizeF anyKeySize = g.MeasureString(anyKey, viewFont);
			anyKeyPoint.X = (ClientRectangle.Width / 2) - (anyKeySize.Width / 2);
			anyKeyPoint.Y = ClientRectangle.Height - (anyKeySize.Height + 10);
			g.DrawString(anyKey, viewFont, sBrush, anyKeyPoint );
			g.Dispose();
			
		}
		
		void PaintView0 (Graphics g)
		{
			// Create a pen object:
			Pen aPen = new Pen(Color.Blue, 2);
			var smallRect = new Rectangle(500,20,50,20);
			
			
			Pen gPen = new Pen(Color.Green, 2);
			var largeRect = new Rectangle(260, 100, 117, 117);
			
			var smallGradient = new LinearGradientBrush(
				smallRect,
				Color.Blue,
				Color.Red,
				0.0f);

			smallGradient.WrapMode = WrapMode.TileFlipXY;

			g.FillRectangle(smallGradient, largeRect);
			
			g.DrawRectangle(gPen, largeRect);
			g.DrawRectangle(aPen, smallRect);

			Point startPoint2 = new Point(10, 110);
			Point endPoint2 = new Point(140, 110);
			Rectangle ellipseRect2 = new Rectangle(20, 100, 200, 200);
			
			var startPoint = new PointF(ellipseRect2.X + 50, ellipseRect2.Y + ellipseRect2.Height /2);
			var endPoint = new PointF(ellipseRect2.X + (ellipseRect2.Width / 2) + 50, ellipseRect2.Bottom - 50);
			
			float[] myFactors = { .2f, .4f, .8f, .8f, .4f, .2f };
			float[] myPositions = { 0.0f, .2f, .4f, .6f, .8f, 1.0f };
			//float[] myFactors = { .2f, .4f, .8f };//, .8f, .4f, .2f };
			//float[] myPositions = { 0.0f, .2f, 1.0f };//, .6f, .8f, 1.0f };
			
			//float[] myFactors = { .2f };
			//float[] myPositions = { 1 }; 
			Blend myBlend = new Blend();
			myBlend.Factors = myFactors;
			myBlend.Positions = myPositions;
			LinearGradientBrush lgBrush2 = new LinearGradientBrush(
				startPoint,
				endPoint,
				Color.Blue,
				Color.Red);
			
			//			LinearGradientBrush lgBrush2 = new LinearGradientBrush(
			//			    ellipseRect2,
			//			    Color.Blue,
			//			    Color.Red, 45, true);
			
			lgBrush2.WrapMode = WrapMode.TileFlipXY;
			lgBrush2.Blend = myBlend;
			g.FillRectangle(lgBrush2, ellipseRect2);
			g.FillEllipse(lgBrush2, ellipseRect2);
			
			//RectangleF boundingRec = lgBrush2.Rectangle;
			//g.DrawRectangle(Pens.White, boundingRec.X,boundingRec.Y,boundingRec.Width,boundingRec.Height);
			
			RectangleF boundingRec = lgBrush2.Rectangle;
			var pennn = new Pen(Color.Green, 1);
			g.DrawRectangle(pennn, boundingRec.X, boundingRec.Y, boundingRec.Width, boundingRec.Height);
			
			var penl = new Pen(Color.Magenta, 1);
			g.DrawLine(penl, startPoint, endPoint);
			
			
			Blend blend1 = new Blend(9);
			
			// Set the values in the Factors array to be all green,  
			// go to all blue, and then go back to green.
			blend1.Factors = new float[]{0.0F, 0.2F, 0.5F, 0.7F, 1.0F, 
				0.7F, 0.5F, 0.2F, 0.0F};
			
			// Set the positions.
			blend1.Positions =
			new float[]{0.0F, 0.1F, 0.3F, 0.4F, 0.5F, 0.6F, 
				0.7F, 0.8F, 1.0F};
			
			// Declare a rectangle to draw the Blend in.
			Rectangle rectangle1 = new Rectangle(10, 10, 120, 100);
			
			// Create a new LinearGradientBrush using the rectangle,  
			// green and blue. and 90-degree angle.
			//			LinearGradientBrush brush1 =
			//				new LinearGradientBrush(rectangle1, Color.LightGreen,
			//				                        Color.Blue, 90, true);
			
			LinearGradientBrush brush1 =
				new LinearGradientBrush(rectangle1, Color.LightGreen,
				                        Color.Blue, LinearGradientMode.BackwardDiagonal);
			
			// Set the Blend property on the brush to the custom blend.
			brush1.Blend = blend1;
			
			// Fill in an ellipse with the brush.
			g.FillEllipse(brush1, rectangle1);

		}

		void PaintView1 (Graphics g)
		{
			Rectangle r = new Rectangle(10, 10, 100, 100);
			
			LinearGradientBrush theBrush = null;
			int yOffSet = 10;
			
			Array obj = Enum.GetValues(typeof(LinearGradientMode));

			for (int x = 0; x < obj.Length; x++)
			{
				LinearGradientMode temp = (LinearGradientMode)obj.GetValue(x);
				theBrush = new LinearGradientBrush(r, Color.Red,
				                                   Color.Blue, temp);
				
				theBrush.LinearColors = new Color[] {Color.Red, Color.Blue};
					g.DrawString(temp.ToString(), new Font("Times New Roman", 15),
				             new SolidBrush(Color.Black), 0, yOffSet);

				g.FillRectangle(theBrush, 120, yOffSet, 200, 50);

				RectangleF boundingRec4 = theBrush.Rectangle;
				var pennn4 = new Pen(Color.Green, 1);
				g.DrawRectangle(pennn4, boundingRec4.X, boundingRec4.Y, boundingRec4.Width, boundingRec4.Height);
				
				yOffSet += 80;
			}


		}

		void PaintView2 (Graphics g)
		{
			// Create a LinearGradientBrush.
			Rectangle myRect = new Rectangle(20, 20, 200, 100);
			LinearGradientBrush myLGBrush = new LinearGradientBrush(
				myRect, Color.Blue, Color.Red,  0.0f, true);
			
			// Draw an ellipse to the screen using the LinearGradientBrush.
			g.FillEllipse(myLGBrush, myRect);
			
			// Rotate the LinearGradientBrush.
			myLGBrush.RotateTransform(45.0f, MatrixOrder.Prepend);
			
			// Rejustify the brush to start at the left edge of the ellipse.
			myLGBrush.TranslateTransform(-100.0f, 0.0f);
			
			// Draw a second ellipse to the screen using 
			// the transformed brush.
			g.FillEllipse(myLGBrush, 20, 150, 200, 100);
			
		}

		void PaintView3 (Graphics g)
		{
			// Create a LinearGradientBrush.
			Rectangle myRect = new Rectangle(20, 20, 200, 100);
			LinearGradientBrush myLGBrush = new LinearGradientBrush(
				myRect, Color.Blue, Color.Red,  0.0f, true);
			
			// Draw an ellipse to the screen using the LinearGradientBrush.
			g.FillEllipse(myLGBrush, myRect);
			
			// Transform the LinearGradientBrush.
			Point[] transformArray = { new Point(20, 150),
				new Point(400,150), new Point(20, 200) };
			
			Matrix myMatrix = new Matrix(myRect, transformArray);
			myLGBrush.MultiplyTransform(
				myMatrix,
				MatrixOrder.Prepend);

			RectangleF boundingRec = myLGBrush.Rectangle;
			var pennn = new Pen(Color.Green, 1);
			g.DrawRectangle(pennn, boundingRec.X, boundingRec.Y, boundingRec.Width, boundingRec.Height);
			// Draw a second ellipse to the screen using 
			// the transformed brush.
			g.FillEllipse(myLGBrush, 20, 150, 380, 50);		
		}

		void PaintView4 (Graphics g)
		{
			// Create a blue pen with width of 2
			Pen bluePen = new Pen(Color.Blue, 2);
			Point pt1 = new Point(10, 10);
			Point pt2 = new Point(20, 20);
			Color[] lnColors = { Color.Black, Color.Red };
			Rectangle rect1 = new Rectangle(10, 10, 15, 15);
			// Create two linear gradient brushes
			LinearGradientBrush lgBrush1 = new LinearGradientBrush
				(rect1, Color.Blue, Color.Green,
				 LinearGradientMode.BackwardDiagonal);
			LinearGradientBrush lgBrush = new LinearGradientBrush
				(pt1, pt2, Color.Red, Color.Green);
			// Set linear colors
			lgBrush.LinearColors = lnColors;
			// Set gamma correction
			lgBrush.GammaCorrection = true;
			// Fill and draw rectangle and ellipses
			g.FillRectangle(lgBrush, 150, 0, 50, 100);
			g.DrawEllipse(bluePen, 0, 0, 100, 50);
			g.FillEllipse(lgBrush1, 300, 0, 100, 100);
			// Apply scale transformation
			g.ScaleTransform(1, 0.5f);
			// Apply translate transformation
			g.TranslateTransform(50, 0, MatrixOrder.Append);
			// Apply rotate transformation
			g.RotateTransform(30.0f, MatrixOrder.Append);
			// Fill ellipse
			g.FillEllipse(lgBrush1, 300, 0, 100, 100);
			// Rotate again
			g.RotateTransform(15.0f, MatrixOrder.Append);
			// Fill rectangle
			g.FillRectangle(lgBrush, 150, 0, 50, 100);
			// Rotate again
			g.RotateTransform(15.0f, MatrixOrder.Append);
			// Draw ellipse
			g.DrawEllipse(bluePen, 0, 0, 100, 50);
			// Dispose of objects
			lgBrush1.Dispose();
			lgBrush.Dispose();
			bluePen.Dispose();

		}

		void PaintView5 (Graphics g)
		{
			LinearGradientBrush br = new LinearGradientBrush(ClientRectangle, Color.Black, Color.Black, 0 , false);
			ColorBlend cb = new ColorBlend();
			cb.Positions = new[] {0, 1/6f, 2/6f, 3/6f, 4/6f, 5/6f, 1};
			cb.Colors = new[] {Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Indigo, Color.Violet};
			br.InterpolationColors= cb;
			// rotate
			br.RotateTransform(45);
			// paint
			g.FillRectangle(br, ClientRectangle);

			Pen myPen = new Pen(Color.Blue, 1);
			Pen myPen2 = new Pen(Color.Red, 1);
			
			// Create an array of points.
			Point[] myArray =
			{
				new Point(20, 20),
				new Point(120, 20),
				new Point(120, 120),
				new Point(20, 120),
				new Point(20,20)
			};
			
			// Draw the Points to the screen before applying the 
			// transform.
			g.DrawLines(myPen, myArray);
			
			// Create a matrix and scale it.
			Matrix myMatrix = new Matrix();
			myMatrix.Scale(3, 2, MatrixOrder.Append);
			myMatrix.TransformPoints(myArray);
			
			// Draw the Points to the screen again after applying the 
			// transform.
			g.DrawLines(myPen2, myArray);
		}

		void PaintView6 (Graphics g)
		{

			// http://msdn.microsoft.com/en-gb/library/5chww2y4(v=vs.71).aspx

			LinearGradientBrush linGrBrush = new LinearGradientBrush(
				new Point(0, 10),
				new Point(200, 10),
				Color.Red,
				Color.Blue);
			
			g.FillRectangle(linGrBrush, 0, 0, 200, 50);
			linGrBrush.GammaCorrection = true;
			g.FillRectangle(linGrBrush, 0, 60, 200, 50);


		}

		void PaintView7 (Graphics g)
		{
			// Create a LinearGradientBrush.
			Rectangle myRect = new Rectangle(20, 20, 200, 100);
			LinearGradientBrush myLGBrush = new LinearGradientBrush(
				myRect, Color.Blue, Color.Red, 0.0f, true);
			
			LinearGradientBrush myLGBrush2 = new LinearGradientBrush(
				myRect, Color.Blue, Color.Red, 0.0f, true);
			
			// Draw an ellipse to the screen using the LinearGradientBrush.
			g.FillEllipse(myLGBrush, myRect);
			
			// Create a bell-shaped brush with the peak at the 
			// center of the drawing area.
			myLGBrush.SetSigmaBellShape(.5f, 1.0f);
			
			// Use the bell- shaped brush to draw a second 
			// ellipse.
			myRect.Y = 150;
			g.FillEllipse(myLGBrush, myRect);


		}

		void PaintView8 (Graphics g)
		{
			// Create a LinearGradientBrush.
			Rectangle myRect = new Rectangle(20, 20, 200, 100);
			LinearGradientBrush myLGBrush = new LinearGradientBrush(
				myRect, Color.Blue, Color.Red, 0.0f, true);
			
			// Draw an ellipse to the screen using the LinearGradientBrush.
			g.FillEllipse(myLGBrush, myRect);
			
			// Create a triangular shaped brush with the peak at the center 
			// of the drawing area.
			myLGBrush.SetBlendTriangularShape(.50f, 1.0f);
			
			// Use the triangular brush to draw a second ellipse.
			myRect.Y = 150;
			g.FillEllipse(myLGBrush, myRect);

		}

		void PaintView9 (Graphics g)
		{
			// Create a LinearGradientBrush.
			LinearGradientBrush linGrBrush = new LinearGradientBrush(
				new Point(0, 10),
				new Point(200, 10),
				Color.FromArgb(255, 255, 0, 0),   // Opaque red
				Color.FromArgb(255, 0, 0, 255));  // Opaque blue
			
			Pen pen = new Pen(linGrBrush,4);
			Pen pena = new Pen(Brushes.Blue,4);
			
			g.DrawLine(pen, 0, 10, 200, 10);
			g.FillEllipse(linGrBrush, 0, 30, 200, 100);
			g.FillRectangle(linGrBrush, 0, 155, 500, 30);
			g.DrawEllipse(pen, 250, 30, 200, 100);
			g.DrawRectangle(pen, 0, 200, 500, 30);
		}

		void PaintView10 (Graphics g)
		{
			// Create a LinearGradientBrush.
			Rectangle myRect = new Rectangle(20, 20, 200, 100);
			LinearGradientBrush myLGBrush = new LinearGradientBrush(
				myRect, Color.Blue, Color.Red,  0.0f, true);

			// Draw an ellipse to the screen using the LinearGradientBrush.
			g.FillEllipse(myLGBrush, myRect);

			// Transform the LinearGradientBrush.
			Point[] transformArray = { new Point(20, 150),
				new Point(400,150), new Point(20, 200) };
			
			Matrix myMatrix = new Matrix(myRect, transformArray);
			myLGBrush.MultiplyTransform(
				myMatrix,
				MatrixOrder.Prepend);

			RectangleF boundingRec = myLGBrush.Rectangle;
			var pennn = new Pen(Color.Green, 1);
			g.DrawRectangle(pennn, boundingRec.X, boundingRec.Y, boundingRec.Width, boundingRec.Height);
			// Draw a second ellipse to the screen using 
			// the transformed brush.
			g.FillEllipse(myLGBrush, 20, 150, 380, 50);
			Font myFont = new Font("Helvetica", 24, FontStyle.Bold);

			g.DrawString("Now is the time for all good men", myFont, myLGBrush, 20,125);
		}

		void PaintView11 (Graphics g)
		{

			// The emsize is calculated here until it can be fixed.
			float emsize = 96f / 72f;
			emsize *= 24;
			var myRect = new RectangleF(10, 10, 200, 200);
			LinearGradientBrush myBrush = new LinearGradientBrush(myRect, Color.Black, Color.Black, 0 , false);
			ColorBlend cb = new ColorBlend();
			cb.Positions = new[] {0, 1/6f, 2/6f, 3/6f, 4/6f, 5/6f, 1};
			cb.Colors = new[] {Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Indigo, Color.Violet};
			myBrush.InterpolationColors= cb;
			
			// rotate
			myBrush.RotateTransform(45);
			
			//Font myFont = new Font("Times New Roman", emsize, FontStyle.Bold);
			Font myFont = new Font("Helvetica", emsize, FontStyle.Bold);
			
			
			g.DrawString("Look at this text!  It is Gradiented!!", myFont, myBrush, myRect);
			
			// reset and rotate again
			myBrush.ResetTransform();
			myBrush.RotateTransform(-45);
			
			g.DrawString("Look at this text!  It is Gradiented!!", myFont, myBrush, 10, 250);
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
