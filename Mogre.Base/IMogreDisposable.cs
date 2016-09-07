// Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Mogre
{
	using System;

	public interface IMogreDisposable : System.IDisposable
	{
		event EventHandler OnDisposed;

		event EventHandler OnDisposing;

		bool IsDisposed
		{
			get;
		}
	}
}
