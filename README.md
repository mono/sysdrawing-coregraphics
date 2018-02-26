This module contains an implementation of the System.Drawing API using
Apple's CoreGraphics engine. With the exception of a couple of APIs
to integrate into the native platform, the API is identical to what
developers are used to.

This works with both iOS and OSX, using Xamarin.Mac Unified and Xamarin.iOS
Unified.

You will need a checkout of Mono as a peer module to this module.

Open the solution and build

Layout
------

The solution references code from mono and corefx, and expects Mono
with submodules to be checked out as a peer directory to this directory.

The Shared project contains most of the code that is used by both MacOS
and iOS, and also references the mono and corefx code.

The MacShared project contains the Mac specific bits, and is later consume
by the Mac target, but in the future all three variations of mac projects
that we support (.NET Desktop, Modern)


