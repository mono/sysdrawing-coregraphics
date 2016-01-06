using UIKit;

namespace MTExample1_5 {
	public partial class MTExample1_5ViewController : UIViewController {
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public MTExample1_5ViewController ()
			: base (UserInterfaceIdiomIsPhone ? "MTExample1_5ViewController_iPhone" : "MTExample1_5ViewController_iPad", null)
		{
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return !UserInterfaceIdiomIsPhone || (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}
	}
}

