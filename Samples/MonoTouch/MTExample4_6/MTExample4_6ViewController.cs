using UIKit;

namespace MTExample4_6 {
	public partial class MTExample4_6ViewController : UIViewController {
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public MTExample4_6ViewController ()
			: base (UserInterfaceIdiomIsPhone ? "MTExample4_6ViewController_iPhone" : "MTExample4_6ViewController_iPad", null)
		{
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return !UserInterfaceIdiomIsPhone || (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}
	}
}

