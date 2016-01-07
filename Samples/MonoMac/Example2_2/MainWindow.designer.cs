
namespace Example2_2
{
	
	// Should subclass MonoMac.AppKit.NSWindow
	[Foundation.Register("MainWindow")]
	public partial class MainWindow
	{
	}
	
	// Should subclass MonoMac.AppKit.NSWindowController
	[Foundation.Register("MainWindowController")]
	public partial class MainWindowController
	{
	}
}

