// Alimer - Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Mogre.Framework
{
    [SampleInfo("Skybox Sample", "thumb_skybox.png", "Shows how to use skyboxes (fixed-distance cubes used for backgrounds).")]
    public class SkyBoxSample : Sample
    {
        protected override void CreateScene()
        {
            _sceneManager.SetSkyBox(true, "Examples/SpaceSkyBox", 5000);
        }
    }
}
