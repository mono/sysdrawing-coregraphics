
using System;
using System.Collections.Generic;
using System.Linq;
using System.DrawingNative;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace Example1_9
{
	public partial class MainWindow : MonoMac.AppKit.NSWindow
	{
		#region Constructors
		
		// Called when created from unmanaged code
		public MainWindow (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public MainWindow (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		// Shared initialization code
		void Initialize ()
		{
			BackgroundColor = NSColor.White;
//			var frame = Frame;
//			frame.Size = new SizeF(400,250);
//			SetFrame(frame,true);
		}
		
		#endregion
	}
}

