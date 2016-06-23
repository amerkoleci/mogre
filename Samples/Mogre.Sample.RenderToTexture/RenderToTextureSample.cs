// Alimer - Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;

namespace Mogre.Framework
{
	[SampleInfo("RenderToTextureSample", "thumb_skybox.png", "Shows how to use RenderToTexturex.")]
	public class RenderToTextureSample : Sample
	{
		const uint NonRefractiveSurfaces = 0x00000001;
		const uint RefractiveSurfaces = 0x00000002;
		const uint ReflectedSurfaces = 0x00000004;
		const uint RegularSurfaces = NonRefractiveSurfaces | ReflectedSurfaces;

		uint _previousVisibilityFlags;

		protected override void CreateScene()
		{
			_previousVisibilityFlags = MovableObject.DefaultVisibilityFlags;
			MovableObject.DefaultVisibilityFlags = RegularSurfaces;

			_camera.SetPosition(-50, 125, 760);
			_camera.LookAt(0, 0, 0);

			_sceneManager.AmbientLight = new ColourValue(0.5f, 0.5f, 0.5f);
			_sceneManager.SetSkyBox(true, "Examples/CloudyNoonSkyBox");  // set a skybox
			Light light = _sceneManager.CreateLight();
			_sceneManager.CreateSceneNode().AttachObject(light);
			light.Type = Light.LightTypes.LT_DIRECTIONAL;
			light.Direction = Vector3.NEGATIVE_UNIT_Y;
			light.Name = "MainLight";

			Entity robot = _sceneManager.CreateEntity("robot", "robot.mesh");
			var robotNode = _sceneManager.RootSceneNode.CreateChildSceneNode();
			robotNode.AttachObject(robot);

			Plane plane = new Plane(Vector3.UNIT_Y, 0);

			MeshManager.Singleton.CreatePlane(
			  "water",
			  ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
			  plane,
			  700.0f, 1300.0f,
			  10, 10, true, 1,
			  1.0f, 1.0f,
			  Vector3.UNIT_Z);

			MaterialPtr renderMat = MaterialManager.Singleton.Create("RttMat", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
			var pass = renderMat.GetTechnique(0).GetPass(0);
			pass.LightingEnabled = false;
			var textureUnit = pass.CreateTextureUnitState();
			textureUnit.SetContentType(TextureUnitState.ContentType.CONTENT_COMPOSITOR);
			textureUnit.SetTextureName("reflection", TextureType.TEX_TYPE_2D);
			textureUnit.SetTextureAddressingMode(TextureUnitState.TextureAddressingMode.TAM_WRAP);

			Entity water = _sceneManager.CreateEntity("water");
			water.SetMaterialName("RttMat" /*"Examples/GrassFloor"*/);
			water.CastShadows = false;
			water.VisibilityFlags = RefractiveSurfaces;
			water.RenderQueueGroup = 95;
			_sceneManager.RootSceneNode.CreateChildSceneNode().AttachObject(water);
		}

		protected override void DestroyScene()
		{
			//Restore global settings
			MovableObject.DefaultVisibilityFlags = _previousVisibilityFlags;

			base.DestroyScene();
		}

		protected override CompositorWorkspace SetupCompositor()
		{
			// The compositor scripts are also part of this sample. Go to Fresnel.compositor
			// to see the sample scripts on how to setup the rendering pipeline.
			CompositorManager2 compositorManager = _root.CompositorManager2;

			const string workspaceName = "RttSampleWorkspace";
			CompositorWorkspace workspace = compositorManager.AddWorkspace(
				_sceneManager, _window,
				_camera, workspaceName, true);
			workspace.SetListener(new RttWorkspaceListener(this));

			return workspace;
		}

		class RttWorkspaceListener : CompositorWorkspaceListener
		{
			readonly RenderToTextureSample _sample;

			public RttWorkspaceListener(RenderToTextureSample sample)
			{
				_sample = sample;
			}

			public override void PassPreExecute(CompositorPass pass)
			{
				var camera = _sample.Camera;
				if (pass.Definition.Identifier == 59645)
				{
					Plane plane = new Plane(Vector3.UNIT_Y, 0);

					camera.AutoAspectRatio = false;
					camera.EnableReflection(plane);
					camera.EnableCustomNearClipPlane(plane);
				}
				else
				{

					camera.AutoAspectRatio = true;
					camera.DisableReflection();
					camera.DisableCustomNearClipPlane();
				}
			}
		}
	}
}
