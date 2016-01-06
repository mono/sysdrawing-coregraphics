using UIKit;

namespace MTExample1_9 {
	public partial class MTExample1_9ViewController : UIViewController {
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public MTExample1_9ViewController ()
			: base (UserInterfaceIdiomIsPhone ? "MTExample1_9ViewController_iPhone" : "MTExample1_9ViewController_iPad", null)
		{
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return !UserInterfaceIdiomIsPhone || (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}
	}
}

