
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing.Drawing2D;

using Foundation;
using AppKit;
using CoreGraphics;
using System.Drawing;

namespace Example1_7
{
	public partial class DrawingView : AppKit.NSView
	{

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
		}

		public DrawingView (CGRect rect) : base (rect)
		{
			Initialize();
		}
		
#endregion

		public override void DrawRect (CGRect dirtyRect)
		{
			Graphics g = Graphics.FromCurrentContext();

			g.Clear(Color.LightGreen);

		}
		
		//		public override bool IsFlipped {
		//			get {
		//				//return base.IsFlipped;
		//				return false;
		//			}
		//		}

	}
}

