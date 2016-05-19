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
            // setup some basic lighting for our scene
            _sceneManager.AmbientLight = new Color4(0.3f, 0.3f, 0.3f);
            var lightNode = _sceneManager.RootSceneNode.CreateChildSceneNode();
            lightNode.SetPosition(20, 80, 50);
            lightNode.AttachObject(_sceneManager.CreateLight());

            _sceneManager.SetSkyBox(true, "Examples/SpaceSkyBox", 5000);

            // create a spaceship model, and place it at the origin
            _sceneManager.RootSceneNode.AttachObject(_sceneManager.CreateEntity("razor.mesh"));

            // create a particle system with 200 quota, then set its material and dimensions
            //ParticleSystem* thrusters = _sceneManager.CreateParticleSystem(25);
            //thrusters->setMaterialName("Examples/Flare");
            //thrusters->setDefaultDimensions(25, 25);

            //// create two emitters for our thruster particle system
            //for (int i = 0; i < 2; i++)
            //{
            //    ParticleEmitter* emitter = thrusters->addEmitter("Point");  // add a point emitter

            //    // set the emitter properties
            //    emitter->setAngle(Degree(3));
            //    emitter->setTimeToLive(0.5);
            //    emitter->setEmissionRate(25);
            //    emitter->setParticleVelocity(25);
            //    emitter->setDirection(Vector3::NEGATIVE_UNIT_Z);
            //    emitter->setColour(ColourValue::White, ColourValue::Red);
            //    emitter->setPosition(Vector3(i == 0 ? 5.7 : -18, 0, 0));
            //}

            // attach our thruster particles to the rear of the ship
            //_sceneManager.RootSceneNode.CreateChildSceneNode(SceneMemoryMgrTypes.Dynamic, new Vector3(0, 6.5f, -67)).AttachObject(thrusters);

            // set the camera's initial position and orientation
            _camera.SetPosition(0, 0, 150);
            _camera.Yaw(new Degree(5));
        }
    }
}
