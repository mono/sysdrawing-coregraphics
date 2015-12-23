using UIKit;

namespace MTExample1_2 {
	public partial class MTExample1_2ViewController : UIViewController {
		static bool UserInterfaceIdiomIsPhone {
			get {
				return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone;
			}
		}

		public MTExample1_2ViewController ()
			: base (UserInterfaceIdiomIsPhone ? "MTExample1_2ViewController_iPhone" : "MTExample1_2ViewController_iPad", null)
		{
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return !UserInterfaceIdiomIsPhone || (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}
	}
}

