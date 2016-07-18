// Alimer - Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Windows.Forms;

namespace Mogre.Framework
{
    [SampleInfo("Camera Tracking Sample", "thumb_camtrack.png", "An example of using AnimationTracks to make a node smoothly follow a predefined path.")]
    public class CameraTrackSample : Sample
    {
		AnimationState _animState;

		protected override void CreateScene()
        {
            // setup some basic lighting for our scene
            _sceneManager.AmbientLight = new ColourValue(0.3f, 0.3f, 0.3f);
            var lightNode = _sceneManager.RootSceneNode.CreateChildSceneNode();
            lightNode.SetPosition(20, 80, 50);
            lightNode.AttachObject(_sceneManager.CreateLight());

            _sceneManager.SetSkyBox(true, "Examples/SpaceSkyBox");

			// create an ogre head entity and attach it to a node
			Entity head = _sceneManager.CreateEntity("ogrehead.mesh");
			head.Name = "Head";
			SceneNode headNode = _sceneManager.GetRootSceneNode().CreateChildSceneNode();
			headNode.AttachObject(head);

			// create a camera node and attach camera to it
			SceneNode camNode = _sceneManager.GetRootSceneNode().CreateChildSceneNode();
			_camera.DetachFromParent();
			camNode.AttachObject(_camera);

			// set up a 10 second animation for our camera, using spline interpolation for nice curves
			Animation anim = _sceneManager.CreateAnimation("CameraTrack", 10);
			anim.SetInterpolationMode(Animation.InterpolationMode.IM_SPLINE);

			// create a track to animate the camera's node
			NodeAnimationTrack track = anim.CreateNodeTrack(camNode);

			// create keyframes for our track
			track.CreateNodeKeyFrame(0).Translate = new Vector3(200, 0, 0);
			track.CreateNodeKeyFrame(2.5f).Translate = new Vector3(0, -50, 100);
			track.CreateNodeKeyFrame(5).Translate = new Vector3(-500, 100, 0);
			track.CreateNodeKeyFrame(7.5f).Translate = new Vector3(0, 200, -300);
			track.CreateNodeKeyFrame(10).Translate = new Vector3(200, 0, 0);

			// create a new animation state to track this
			_animState = _sceneManager.CreateAnimationState("CameraTrack");
			_animState.Enabled = true;

			var rttCamera = _sceneManager.CreateCamera("RttCamera");
			rttCamera.Position = new Vector3(0f, 0f, 500f);
			rttCamera.LookAt(new Vector3(0f, 0f, -300f));
			rttCamera.AutoAspectRatio = true;
			rttCamera.NearClipDistance = 5.0f;

			screenTexture0 = TextureManager.Singleton.CreateManual(
				"screenTexture0",
				ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
				TextureType.TEX_TYPE_2D,
				640,
				480,
				0,
				PixelFormat.PF_R8G8B8,
				(int)TextureUsage.TU_RENDERTARGET
			);

			CompositorManager2 compositorManager = _root.CompositorManager2;
			string workspaceName = GetType().Name + "RttWorkspace";
			if (!compositorManager.HasWorkspaceDefinition(workspaceName))
			{
				compositorManager.CreateBasicWorkspaceDef(workspaceName, new ColourValue(0.0f, 1.0f, 0.0f, 1.0f));
			}

			_root.CompositorManager2.AddWorkspace(_sceneManager, screenTexture0.GetBuffer().GetRenderTarget(), rttCamera, workspaceName, true);
		}

		protected override bool OnFrameStarted(FrameEvent evt)
		{
			_animState.AddTime(evt.timeSinceLastFrame);   // increment animation time
			return base.OnFrameStarted(evt);
		}

		protected override DefaultInputHandler CreateInputHandler()
		{
			return new MyTestInputHandler(this);
		}

		class MyTestInputHandler : DefaultInputHandler
		{
			public MyTestInputHandler(CameraTrackSample sample) : base(sample)
			{

			}

			protected override void HandleKeyDown(object sender, KeyEventArgs e)
			{
				switch(e.KeyCode)
				{
					case Keys.Space:
						((CameraTrackSample)_sample).TakeRttScreen();
						break;
				}
				base.HandleKeyDown(sender, e);
			}
		}

		TexturePtr screenTexture0;

		void TakeRttScreen()
		{
			string[] temp = System.IO.Directory.GetFiles(Environment.CurrentDirectory, "Rttscreenshot*.jpg");
			string fileName = string.Format("Rttscreenshot{0}.jpg", temp.Length + 1);
			screenTexture0.GetBuffer().GetRenderTarget().WriteContentsToFile(fileName);
		}

	}
}
