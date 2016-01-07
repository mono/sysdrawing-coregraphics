using UIKit;

namespace MTExample3_2 {
	public partial class MTExample3_2iPadViewController : UIViewController {
		public MTExample3_2iPadViewController () : base ("MTExample3_2iPadViewController", null)
		{
		}
		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			ReleaseDesignerOutlets ();
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}
	}
}

