// Alimer - Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mogre.Framework
{
	[SampleInfo("Shadows", "thumb_shadows.png", "A demonstration of ogre's various shadowing techniques.")]
	public class ShadowsSample : Sample
	{
		Entity _floorPlane;
		Light _mainLight;
		readonly List<Entity> _casters = new List<Entity>();
		bool _pssm = true;
		SceneNode _lightRootNode;
		static readonly Random _randomizer = new Random();

		protected override CompositorWorkspace SetupCompositor()
		{
			var compositorManager = _root.CompositorManager2;

			CompositorNodeDef nodeDef = compositorManager.GetNodeDefinition("ExampleShadows_Node");
			CompositorTargetDef targetDef = nodeDef.GetTargetPass(0);

			Debug.Assert(
				targetDef.RenderTargetName == IdString.FromString("rt_renderwindow"),
				"Media compositor file was modified. Make sure C++ code is in sync with those changes!");

			var passScene = targetDef.GetCompositorPass(1) as CompositorPassSceneDef;
			Debug.Assert(passScene != null,
				"Media compositor file was modified. Make sure C++ code is in sync with those changes!");

			passScene.ShadowNodeName = _pssm ? "ExampleShadows_PssmShadowNode" : "ExampleShadows_FocusedShadowNode";

			const string workspaceName = "ShadowsV2Workspace";
			if (!compositorManager.HasWorkspaceDefinition(workspaceName))
			{
				CompositorWorkspaceDef workspaceDef = compositorManager.AddWorkspaceDefinition(workspaceName);
				workspaceDef.ConnectOutput("ExampleShadows_Node", 0);
			}

			return compositorManager.AddWorkspace(_sceneManager, _window, _camera, workspaceName, true);
		}

		protected override void CreateScene()
		{
			_sceneManager.AmbientLight = new ColourValue(0.1f, 0.1f, 0.1f);

			SceneNode lightNode = _sceneManager.RootSceneNode.CreateChildSceneNode();
			_mainLight = _sceneManager.CreateLight();
			_mainLight.Name = "Sun";
			lightNode.AttachObject(_mainLight);
			_mainLight.Type = Light.LightTypes.LT_DIRECTIONAL;
			_mainLight.Direction = Vector3.Normalise(new Vector3(-0.1f, -1.0f, -1.25f));
			//_mainLight.ShadowFarDistance = 500;
			//_mainLight.Direction = Vector3.NEGATIVE_UNIT_Y;

			// Floor
			MeshManager.Singleton.CreatePlane(
			  "ground",
			  ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
			  new Plane(Vector3.UNIT_Y, -20),
			  1000.0f, 1000.0f,
			  1, 1, true, 1,
			  6, 6,
			  Vector3.UNIT_Z);

			_floorPlane = _sceneManager.CreateEntity("ground", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, SceneMemoryMgrTypes.SCENE_STATIC);
			_floorPlane.SetMaterialName(_pssm ? "Example_Shadows_Floor_pssm" : "Example_Shadows_Floor");
			_floorPlane.CastShadows = true;
			_sceneManager.GetRootSceneNode(SceneMemoryMgrTypes.SCENE_STATIC).AttachObject(_floorPlane);

			// Penguin
			Entity penguin = _sceneManager.CreateEntity("penguin.mesh");
			var penguinNode = _sceneManager.RootSceneNode.CreateChildSceneNode();
			penguinNode.AttachObject(penguin);
			penguin.Name = "Penguin";
			penguin.SetMaterialName(_pssm ? "Example_Shadows_Penguin_pssm" : "Example_Shadows_Penguin");
			_casters.Add(penguin);

			_camera.SetPosition(0, 20, 50);
			_camera.LookAt(0, 20, 0);

			//_sceneManager.ShadowDirectionalLightExtrusionDistance = 200.0f;
			_sceneManager.ShadowFarDistance = 1000;
			//_sceneManager.ShadowCasterRenderBackFaces = false;
			//_sceneManager.ShadowDirLightTextureOffset = 0.3f;
			_camera.NearClipDistance = 0.1f;
			_camera.FarClipDistance = 5000.0f;

			//CreateExtraLights();
			CreateDebugOverlay();
		}

		protected override bool OnFrameRenderingQueued(FrameEvent evt)
		{
			if (_lightRootNode != null)
			{
				_lightRootNode.Yaw(new Radian(evt.timeSinceLastFrame));
			}

			Node mainLightNode = _mainLight.ParentNode;
			mainLightNode.Yaw(new Radian(-evt.timeSinceLastFrame * 0.1f), Node.TransformSpace.TS_PARENT);

			return base.OnFrameRenderingQueued(evt);
		}

		void CreateExtraLights()
		{
			_lightRootNode = _sceneManager.RootSceneNode.CreateChildSceneNode();

			//Create 7 more lights
			for (var i = 0; i < 7; ++i)
			{
				SceneNode lightNode = _lightRootNode.CreateChildSceneNode();
				Light light = _sceneManager.CreateLight();
				light.Name = "Extra Point Light";
				lightNode.AttachObject(light);
				light.Type = Light.LightTypes.LT_POINT;

				light.SetAttenuation(1000.0f, 1.0f, 0.0f, 1.0f);

				light.SetDiffuseColour(
					_randomizer.Next(255) / 255.0f * 0.25f,
					_randomizer.Next(255) / 255.0f * 0.25f,
					_randomizer.Next(255) / 255.0f * 0.25f);

				lightNode.Position = new Vector3(
					((float)_randomizer.NextDouble() * 2.0f - 1.0f) * 60.0f,
					((float)_randomizer.NextDouble() * 2.0f - 1.0f) * 10.0f,
					((float)_randomizer.NextDouble() * 2.0f - 1.0f) * 60.0f);
			}
		}

		void CreateDebugOverlay()
		{
			//uint shadowNodeName = _pssm ? IdString.FromString("ExampleShadows_PssmShadowNode") : IdString.FromString("ExampleShadows_FocusedShadowNode");
			//var baseWhite = MaterialManager.Singleton.GetByName("Example_Shadows_DebugView");

			//var DepthShadowTexture = baseWhite.Clone("DepthShadowTexture0");
			//var textureUnit = DepthShadowTexture.GetTechnique(0).GetPass(0).GetTextureUnitState(0);
			//CompositorShadowNode* shadowNode = _workspace.FindShadowNode(shadowNodeName);
			//Ogre::TexturePtr tex = shadowNode->getLocalTextures()[0].textures[0];
			//textureUnit->setTextureName(tex->getName());
		}
	}
}
