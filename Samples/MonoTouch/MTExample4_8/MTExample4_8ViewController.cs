using UIKit;

namespace MTExample4_8 {
	public partial class MTExample4_8ViewController : UIViewController {
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public MTExample4_8ViewController ()
			: base (UserInterfaceIdiomIsPhone ? "MTExample4_8ViewController_iPhone" : "MTExample4_8ViewController_iPad", null)
		{
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return !UserInterfaceIdiomIsPhone || toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown;
		}
	}
}

