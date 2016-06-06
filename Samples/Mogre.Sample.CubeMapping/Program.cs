// Alimer - Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Runtime.InteropServices;

namespace Mogre.Framework
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var app = new CubeMappingSample();
                app.Run();
            }
            catch (SEHException)
            {
                if (!OgreException.IsThrown)
                {
                    throw;
                }

                //Example.ShowOgreException();
            }
        }
    }
}
