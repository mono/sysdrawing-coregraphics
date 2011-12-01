//
// Brush.cs: The Brush setup code
//
// Authors:
//   Miguel de Icaza (miguel@xamarin.com)
//
// Copyright 2011 Xamarin Inc
//
using System;
#if MONOMAC
using MonoMac.CoreGraphics;
#else
using MonoTouch.CoreGraphics;
#endif

namespace System.Drawing {

	public abstract partial class Brush : MarshalByRefObject, IDisposable, ICloneable {
		bool changed = true;
		
		~Brush ()
		{
			Dispose (false);
		}

		public void Dispose ()
		{
			Dispose (true);
		}

		public virtual void Dispose (bool disposing)
		{
			if (disposing){
			}
		}

		abstract public object Clone ();

		internal abstract void Setup (Graphics graphics, bool fill);
	}
}