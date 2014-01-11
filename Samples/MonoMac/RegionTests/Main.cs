using System;
using System.DrawingNative;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;

namespace RegionTests
{
	class MainClass
	{
		static void Main (string[] args)
		{
			NSApplication.Init ();
			NSApplication.Main (args);
		}
	}
}	

