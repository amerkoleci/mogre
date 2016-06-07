// Alimer - Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using Ogre;

namespace Mogre.Framework
{
	[SampleInfo("Cube Mapping Sample", "thumb_cubemap.png", "Demonstrates how to setup cube mapping with the compositor.")]
	public class CubeMappingSample : Sample
	{
		uint _previousVisibilityFlags;
		Camera _cubeCamera;
		Entity _head;
		SceneNode _pivot;
		AnimationState _fishSwim;
		CompositorWorkspace _cubemapWorkspace;

		protected override void TestCapabilities(RenderSystemCapabilities caps)
		{
			if (!caps.HasCapability(Capabilities.RSC_CUBEMAPPING))
			{
				throw new InvalidOperationException("Your graphics card does not support cube mapping, so you cannot run this sample. Sorry!");
			}
		}

		protected override bool OnFrameStarted(FrameEvent evt)
		{
			_pivot.Yaw(new Radian(evt.timeSinceLastFrame));      // spin the fishy around the cube mapped one
			_fishSwim.AddTime(evt.timeSinceLastFrame * 3);   // make the fishy swim

			return base.OnFrameStarted(evt);
		}

		protected override void CreateScene()
		{
			_previousVisibilityFlags = MovableObject.DefaultVisibilityFlags;
			//MovableObject.DefaultVisibilityFlags = RegularSurfaces;
			_workspace.SetListener(new CubeMapCompositorWorkspaceListener(this));

			_sceneManager.SetSkyDome(true, "Examples/CloudySky");

			// setup some basic lighting for our scene
			_sceneManager.AmbientLight = new ColourValue(0.3f, 0.3f, 0.3f);
			SceneNode lightNode = _sceneManager.RootSceneNode.CreateChildSceneNode();
			lightNode.SetPosition(20, 80, 50);
			lightNode.AttachObject(_sceneManager.CreateLight());

			CreateCubeMap();

			// create an ogre head, give it the dynamic cube map material, and place it at the origin
			_head = _sceneManager.CreateEntity("ogrehead.mesh",
											 ResourceGroupManager.AUTODETECT_RESOURCE_GROUP_NAME,
											 SceneMemoryMgrTypes.SCENE_STATIC);
			_head.Name = "CubeMappedHead";
			_head.SetMaterialName("Examples/DynamicCubeMap");
			//_head.VisibilityFlags = NonRefractiveSurfaces;
			_sceneManager.GetRootSceneNode(SceneMemoryMgrTypes.SCENE_STATIC).AttachObject(_head);

			_pivot = _sceneManager.RootSceneNode.CreateChildSceneNode();  // create a pivot node

			Entity fish = _sceneManager.CreateEntity("fish.mesh");
			fish.Name = "Fish";
			_fishSwim = fish.GetAnimationState("swim");
			_fishSwim.Enabled = true;

			// create a child node at an offset and attach a regular ogre head and a nimbus to it
			SceneNode node = _pivot.CreateChildSceneNode();
			node.SetPosition(-60, 10, 0);
			node.SetScale(7, 7, 7);
			node.Yaw(new Degree(90));
			node.AttachObject(fish);

			// create a floor mesh resource
			var mesh = MeshManager.Singleton.CreatePlane("floor",
				ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
				new Plane(Vector3.UNIT_Y, -30), 1000, 1000, 10, 10, true, 1, 8, 8, Vector3.UNIT_Z);

			// create a floor entity, give it a material, and place it at the origin
			Entity floor = _sceneManager.CreateEntity(mesh, SceneMemoryMgrTypes.SCENE_STATIC);
			floor.Name = "Floor";
			floor.SetMaterialName("Examples/BumpyMetal");
			_sceneManager.GetRootSceneNode(SceneMemoryMgrTypes.SCENE_STATIC).AttachObject(floor);
		}

		protected override void DestroyScene()
		{
			_sceneManager.DestroyCamera(_cubeCamera);
			MeshManager.Singleton.Remove("floor");
			TextureManager.Singleton.Remove("dyncubemap");

			//Restore global settings
			MovableObject.DefaultVisibilityFlags = _previousVisibilityFlags;
		}

		void CreateCubeMap()
		{
			// create the camera used to render to our cubemap
			_cubeCamera = _sceneManager.CreateCamera("CubeMapCamera", true, true);
			_cubeCamera.FOVy = new Degree(90);
			_cubeCamera.AspectRatio = 1.0f;
			_cubeCamera.SetFixedYawAxis(false);
			_cubeCamera.NearClipDistance = 5.0f;
			_cubeCamera.FarClipDistance = /*100*/10000;

			TexturePtr tex = TextureManager.Singleton.CreateManual("dyncubemap", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, TextureType.TEX_TYPE_CUBE_MAP, 512, 512, 0, PixelFormat.PF_R8G8B8, (int)TextureUsage.TU_RENDERTARGET);

			CompositorManager2 compositorManager = _root.CompositorManager2;
			const string workspaceName = "CompositorSampleCubemap_cubemap";
			if (!compositorManager.HasWorkspaceDefinition(workspaceName))
			{
				CompositorWorkspaceDef workspaceDef = compositorManager.AddWorkspaceDefinition(workspaceName);
				//"CubemapRendererNode" has been defined in scripts.
				//Very handy (as it 99% the same for everything)
				workspaceDef.ConnectOutput("CubemapRendererNode", 0);
			}

			CompositorChannel channel = new CompositorChannel
			{
				target = tex.GetBuffer(0).GetRenderTarget()
			};
			channel.textures.Add(tex);
			_cubemapWorkspace = compositorManager.AddWorkspace(_sceneManager, channel, _cubeCamera, workspaceName, false);
		}

		class CubeMapCompositorWorkspaceListener : CompositorWorkspaceListener
		{
			readonly CubeMappingSample _sample;

			public CubeMapCompositorWorkspaceListener(CubeMappingSample sample)
			{
				_sample = sample;
			}

			public override void WorkspacePreUpdate(CompositorWorkspace workspace)
			{
				/** CompositorWorkspaceListener::workspacePreUpdate is the best place to update other (manual)
					Workspaces for multiple reasons:
						1. It happens after Ogre issued D3D9's beginScene. If you want to update a workspace
							outside beginScene/endScene pair, you will have to call Workspace::_beginUpdate(true)
						   and _endUpdate(true) yourself. This will add synchronization overhead in the API,
						   lowering performance.
						2. It happens before the whole scene is rendered, thus you can ensure your RTTs are
						   up to date.

					One alternative that allows you to forget about this listener is to use auto-updated
					workspaces, but you will have to ensure this workspace is created before your main
					workspace (the one that outputs to the RenderWindow).

					Another alternative is the one presented in the Fresnel demo: The rendering is fully
					handled inside one single workspace, and the textures are created by the Compositor
					instead of being manually created.
				*/
				//_sample._cubemapWorkspace._beginUpdate( forceFrameBeginEnd );
				_sample._cubemapWorkspace._update();
				//_sample._cubemapWorkspace._endUpdate( forceFrameBeginEnd );
			}
		}
	}
}
