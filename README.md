This module contains an implementation of the System.Drawing API using
Apple's CoreGraphics engine. With the exception of a couple of APIs
to integrate into the native platform, the API is identical to what
developers are used to.

This works with both iOS and OSX, using Xamarin.Mac Unified and Xamarin.iOS
Unified.

The Makefile assumes you have a copy of mono checked out side by side. 
You can update the MONO_SYSD variable to point to a checkout in another location if necessary.
