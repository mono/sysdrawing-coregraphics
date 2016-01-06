using UIKit;

namespace MTExample4_1 {
	public partial class MTExample4_1ViewController : UIViewController {
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public MTExample4_1ViewController ()
			: base (UserInterfaceIdiomIsPhone ? "MTExample4_1ViewController_iPhone" : "MTExample4_1ViewController_iPad", null)
		{
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return !UserInterfaceIdiomIsPhone || toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown;
		}
	}
}

