This now integrates the sysdrawing-coregraphics changes from
https://github.com/filipnavara/mac-playground.git

It does so by keeping the iOS backend working, and uses
partial classes to split the platform specific capabilites.

This also removes some of the historical artifacts from
the upgrade, the move to support MonoMac, Xamarin.Mac and
so on and drops support for the Classic API which is no
longer around.