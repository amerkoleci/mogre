// Alimer - Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Mogre.Framework
{
    [SampleInfo("Sky Plane", "thumb_skyplane.png", "Shows how to use skyplanes (fixed-distance planes used for backgrounds).")]
    public class SkyPlaneSample : Sample
    {
        protected override void CreateScene()
        {
            // setup some basic lighting for our scene
            _sceneManager.AmbientLight = new ColourValue(0.3f, 0.3f, 0.3f);
            var lightNode = _sceneManager.RootSceneNode.CreateChildSceneNode();
            lightNode.SetPosition(20, 80, 50);
            lightNode.AttachObject(_sceneManager.CreateLight());

            _sceneManager.SetSkyPlane(true, new Plane(0, -1, 0, 5000), "Examples/SpaceSkyPlane", 10000, 3);

            // and finally... omg it's a DRAGON!
            _sceneManager.RootSceneNode.AttachObject(_sceneManager.CreateEntity("dragon.mesh"));

            // turn around and look at the DRAGON!
            _camera.Yaw(new Degree(210));
            _camera.Pitch(new Degree(-10));
        }
    }
}
