This repository contains the source code to build a System.Drawing.dll assembly built on top of the managed bindings (MonoTouch and MonoMac) for Apple's CoreGraphic.

Goal
----

The goal is to ease porting existing System.Drawing code bases to the Mac and iOS devices without bringing ay additional unmanaged dependencies (e.g. libgdiplus and everything it requires).

Unlike Mono's default System.Drawing.dll we are _not_ looking at being pixel perfect compared to Microsoft implementation. This should help, not eliminate, porting .NET code.


TODO
----

Common
======

* Complete the assembly :-)

MonoMac
=======

* Fix the circular dependency of the build.

Right now MonoMac.dll depends on System.Drawing.dll (e.g. Rectangle[F], Point[F] and Size[F]) but we want this System.Drawing.dll to depend on MonoMac.dll
