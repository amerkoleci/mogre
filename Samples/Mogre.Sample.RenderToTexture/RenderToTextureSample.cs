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
		private CompositorWorkspace _rttWorkspace;

		protected override CompositorWorkspace SetupCompositor()
		{
			CompositorManager2 compositorManager = _root.CompositorManager2;
			const string workspaceName = "RenderToTextureSample";
			var workspace = compositorManager.AddWorkspace(_sceneManager, _window, _camera, workspaceName, true);
			return workspace;
		}

		protected override void CreateScene()
		{
			// setup some basic lighting for our scene
			_sceneManager.AmbientLight = new ColourValue(0.2f, 0.2f, 0.2f);
			Light light = _sceneManager.CreateLight();
			light.Name = "MainLight";

			_camera.SetPosition(60, 200, 70);
			_camera.LookAt(0, 0, 0);

			Entity robot = _sceneManager.CreateEntity("robot", "robot.mesh");
			var robotNode = _sceneManager.RootSceneNode.CreateChildSceneNode();
			robotNode.AttachObject(robot);

			Plane plane = default(Plane);
			plane.normal = Vector3.UNIT_Y;
			plane.d = 0;

			MeshManager.Singleton.CreatePlane(
			  "floor",
			  ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
			  plane,
			  450.0f, 450.0f,
			  10, 10, true, 1,
			  50.0f, 50.0f,
			  Vector3.UNIT_Z);

			Entity planeEntity = _sceneManager.CreateEntity("floor");
			planeEntity.SetMaterialName("Examples/GrassFloor");
			planeEntity.CastShadows = false;
			_sceneManager.RootSceneNode.CreateChildSceneNode().AttachObject(planeEntity);
		}

		protected override void DestroyScene()
		{
			base.DestroyScene();
		}
	}
}
