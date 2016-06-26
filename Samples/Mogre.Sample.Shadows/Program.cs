// Alimer - Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Runtime.InteropServices;

namespace Mogre.Framework
{
	class Program
	{
		static void Main()
		{
			try
			{
				var app = new ShadowsSample();
				app.Run();
			}
			catch (SEHException)
			{
				if (!OgreException.IsThrown)
				{
					throw;
				}

				var exception = OgreException.LastException;
				//Example.ShowOgreException();
			}
		}
	}
}
