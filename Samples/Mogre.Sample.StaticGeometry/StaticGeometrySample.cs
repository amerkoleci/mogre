// Alimer - Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;

namespace Mogre.Framework
{
	[SampleInfo("StaticGeometrySample", "thumb_skybox.png", "Shows how to use StaticGeometry.")]
	public class StaticGeometrySample : Sample
	{
		protected override void CreateScene()
		{
			// setup some basic lighting for our scene
			_sceneManager.AmbientLight = new ColourValue(0.3f, 0.3f, 0.3f);
			CreateGrassMesh();

			_camera.SetPosition(150, 50, 150);
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

			Entity grass = _sceneManager.CreateEntity("grass");
			var sg = _sceneManager.CreateStaticGeometry("Field");

			const int size = 140;

			sg.RegionDimensions = new Vector3(size, size, size);
			sg.Origin = new Vector3(70, 70, 70);

			for (int x = -280; x < 280; x += 20)
			{
				for (int z = -280; z < 280; z += 20)
				{
					Vector3 pos = new Vector3(x + Math.RangeRandom(-7, 7), 0, z + Math.RangeRandom(-7, 7));
					Quaternion ori = new Quaternion(new Degree(Math.RangeRandom(0, 359)), Vector3.UNIT_Y);
					Vector3 scale = new Vector3(1, Math.RangeRandom(0.85f, 1.15f), 1);
					sg.AddEntity(grass, pos, ori, scale);
				}
			}

			sg.Build();
		}

		protected override void DestroyScene()
		{
			MeshManager.Singleton.Remove("grass");
			//_sceneManager.DestroyStaticGeometry(sg);

			base.DestroyScene();
		}

		void CreateGrassMesh()
		{
			const float width = 25;
			const float height = 30;
			Vector3 vec = new Vector3(width / 2, 0, 0);
			ManualObject obj = _sceneManager.CreateManualObject("GrassObject");

			Quaternion quat = new Quaternion(new Degree(60), Vector3.UNIT_Y);

			obj.Begin("Examples/GrassBlades", RenderOperation.OperationTypes.OT_TRIANGLE_LIST);

			for (uint i = 0; i < 3; ++i)
			{
				obj.Position(-vec.x, height, -vec.z);
				obj.TextureCoord(0, 0);
				obj.Position(vec.x, height, vec.z);
				obj.TextureCoord(1, 0);
				obj.Position(-vec.x, 0, -vec.z);
				obj.TextureCoord(0, 1);
				obj.Position(vec.x, 0, vec.z);
				obj.TextureCoord(1, 1);

				uint offset = 4 * i;
				obj.Triangle(offset, offset + 3, offset + 1);
				obj.Triangle(offset, offset + 2, offset + 3);

				vec = quat * vec;
			}

			obj.End();
			obj.ConvertToMesh("grass");
			_sceneManager.DestroyManualObject(obj);
		}

		
		
	}
}
