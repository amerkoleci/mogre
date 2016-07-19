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

		SceneManager _rttSceneManager;
		Camera _rttCamera;
		CompositorWorkspace _rttWorkspace;
		SceneNode _penguinNode;

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
			headNode.Translate(0, 40, 0);
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

			// Now create another scene manager with only
			_rttSceneManager = CreateSceneManager();

			// Create the RenderTargetTexture
			_rttCamera = _rttSceneManager.CreateCamera("RttCamera");
			_rttCamera.Position = new Vector3(0f, 10f, 500f);
			_rttCamera.LookAt(new Vector3(0f, 0f, -300f));
			_rttCamera.AutoAspectRatio = true;
			_rttCamera.NearClipDistance = 5.0f;

			// Penguin
			Entity penguin = _rttSceneManager.CreateEntity("penguin.mesh");
			_penguinNode = _rttSceneManager.RootSceneNode.CreateChildSceneNode();
			_penguinNode.AttachObject(penguin);
			penguin.Name = "Penguin";

			MaterialPtr penguinMaterial = MaterialManager.Singleton.Create("PenguinMaterial", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
			penguinMaterial.GetTechnique(0).GetPass(0).LightingEnabled = false;
			penguinMaterial.GetTechnique(0).GetPass(0).CreateTextureUnitState("penguin.jpg");
			penguin.SetMaterialName(penguinMaterial.Name);

			var screenTexture0 = TextureManager.Singleton.CreateManual(
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
				compositorManager.CreateBasicWorkspaceDef(workspaceName, new ColourValue(1.0f, 0.0f, 0.0f, 1.0f));
			}

			_rttWorkspace = _root.CompositorManager2.AddWorkspace(
				_rttSceneManager, 
				screenTexture0.GetBuffer().GetRenderTarget(),
				_rttCamera, 
				workspaceName,
				true);

			MaterialPtr renderMat = MaterialManager.Singleton.Create("RttMat", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
			var pass = renderMat.GetTechnique(0).GetPass(0);
			pass.LightingEnabled = false;
			var textureUnit = pass.CreateTextureUnitState();
			textureUnit.SetContentType(TextureUnitState.ContentType.CONTENT_COMPOSITOR);
			textureUnit.SetTextureName("screenTexture0", TextureType.TEX_TYPE_2D);
			textureUnit.SetTextureAddressingMode(TextureUnitState.TextureAddressingMode.TAM_WRAP);

			Plane plane = new Plane(Vector3.UNIT_Y, 0);
			MeshManager.Singleton.CreatePlane(
			  "planeMesh",
			  ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
			  plane,
			  700.0f, 1300.0f,
			  10, 10, true, 1,
			  4.0f, 4.0f,
			  Vector3.UNIT_Z);

			Entity planeEntity = _sceneManager.CreateEntity("planeMesh");
			planeEntity.SetMaterialName("RttMat");
			planeEntity.CastShadows = false;
			_sceneManager.RootSceneNode.CreateChildSceneNode().AttachObject(planeEntity);
		}

		protected override void DestroyScene()
		{
			_root.DestroySceneManager(_rttSceneManager);
			Utilities.Dispose(ref _rttSceneManager);
		}

		protected override bool OnFrameStarted(FrameEvent evt)
		{
			_animState.AddTime(evt.timeSinceLastFrame);   // increment animation time
			_penguinNode.Yaw(evt.timeSinceLastFrame * 30);
			return base.OnFrameStarted(evt);
		}
	}
}
