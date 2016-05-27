#pragma once

#include "OgreSceneManager.h"
#include "MogreCommon.h"
#include "MogreCamera.h"
#include "MogreLight.h"
#include "MogreBillboardSet.h"
#include "MogreBillboardChain.h"
#include "MogreRibbonTrail.h"
#include "MogreParticleSystem.h"

namespace Mogre
{
	ref class RenderSystem;
	ref class SceneNode;
	ref class Entity;
	ref class Animation;
	ref class AnimationState;

	public ref class SceneManager : IDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	public:
		enum class PrefabType
		{
			PT_PLANE = Ogre::SceneManager::PT_PLANE,
			PT_CUBE = Ogre::SceneManager::PT_CUBE,
			PT_SPHERE = Ogre::SceneManager::PT_SPHERE
		};

	private:
		Ogre::SceneManager* _native;
		bool _createdByCLR;

	private:
		SceneManager(Ogre::SceneManager* obj) : _native(obj)
		{
		}

	public protected:
		SceneManager(intptr_t ptr) : _native((Ogre::SceneManager*)ptr)
		{

		}

	public:
		~SceneManager();
	protected:
		!SceneManager();
	public:
		property bool IsDisposed
		{
			virtual bool get();
		}

		property Mogre::Color4 AmbientLight
		{
		public:
			Mogre::Color4 get();
		public:
			void set(Mogre::Color4 value);
		}

		property bool DisplaySceneNodes
		{
		public:
			bool get();
		public:
			void set(bool value);
		}

		property Mogre::SceneNode^ RootSceneNode
		{
		public:
			Mogre::SceneNode^ get();
		}

		Mogre::BillboardSet^ CreateBillboardSet(unsigned int poolSize);
		void DestroyBillboardSet(Mogre::BillboardSet^ set);
		void DestroyAllBillboardSets();

		Mogre::BillboardChain^ CreateBillboardChain();
		void DestroyBillboardChain(Mogre::BillboardChain^ obj);
		void DestroyAllBillboardChains();

		Mogre::RibbonTrail^ CreateRibbonTrail();
		void DestroyRibbonTrail(Mogre::RibbonTrail^ obj);
		void DestroyAllRibbonTrails();
		
		Mogre::ParticleSystem^ CreateParticleSystem(String^ templateName);
		Mogre::ParticleSystem^ CreateParticleSystem(size_t quota, String^ resourceGroup);
		Mogre::ParticleSystem^ CreateParticleSystem(size_t quota);

		void DestroyParticleSystem(Mogre::ParticleSystem^ obj);
		void DestroyAllParticleSystems();

		Mogre::Light^ CreateLight();
		void DestroyLight(Mogre::Light^ light);
		void DestroyAllLights();

		Mogre::Camera^ CreateCamera(String^ name);
		Mogre::Camera^ CreateCamera(String^ name, bool notShadowCaster);
		Mogre::Camera^ CreateCamera(String^ name, bool notShadowCaster, bool forCubemapping);
		Mogre::Camera^ FindCamera(String^ name);
		Mogre::Camera^ FindCameraNoThrow(String^ name);
		void DestroyCamera(Mogre::Camera^ camera);
		void DestroyAllCameras();

		Mogre::Entity^ CreateEntity(String^ meshName);
		Mogre::Entity^ CreateEntity(String^ meshName, String^ groupName);
		Mogre::Entity^ CreateEntity(String^ meshName, String^ groupName, SceneMemoryMgrTypes sceneType);
		Mogre::Entity^ CreateEntity(Mogre::SceneManager::PrefabType ptype);
		Mogre::Entity^ CreateEntity(Mogre::SceneManager::PrefabType ptype, SceneMemoryMgrTypes sceneType);

		void SetSkyPlane(bool enable, Mogre::Plane plane, String^ materialName, Ogre::Real scale, Ogre::Real tiling, bool drawFirst, Ogre::Real bow, int xsegments, int ysegments, String^ groupName);
		void SetSkyPlane(bool enable, Mogre::Plane plane, String^ materialName, Ogre::Real scale, Ogre::Real tiling, bool drawFirst, Ogre::Real bow, int xsegments, int ysegments);
		void SetSkyPlane(bool enable, Mogre::Plane plane, String^ materialName, Ogre::Real scale, Ogre::Real tiling, bool drawFirst, Ogre::Real bow, int xsegments);
		void SetSkyPlane(bool enable, Mogre::Plane plane, String^ materialName, Ogre::Real scale, Ogre::Real tiling, bool drawFirst, Ogre::Real bow);
		void SetSkyPlane(bool enable, Mogre::Plane plane, String^ materialName, Ogre::Real scale, Ogre::Real tiling, bool drawFirst);
		void SetSkyPlane(bool enable, Mogre::Plane plane, String^ materialName, Ogre::Real scale, Ogre::Real tiling);
		void SetSkyPlane(bool enable, Mogre::Plane plane, String^ materialName, Ogre::Real scale);
		void SetSkyPlane(bool enable, Mogre::Plane plane, String^ materialName);

		void SetSkyBox(bool enable, String^ materialName, Ogre::Real distance, bool drawFirst, Mogre::Quaternion orientation, String^ groupName);
		void SetSkyBox(bool enable, String^ materialName, Ogre::Real distance, bool drawFirst, Mogre::Quaternion orientation);
		void SetSkyBox(bool enable, String^ materialName, Ogre::Real distance, bool drawFirst);
		void SetSkyBox(bool enable, String^ materialName, Ogre::Real distance);
		void SetSkyBox(bool enable, String^ materialName);

		void SetSkyDome(bool enable, String^ materialName, Ogre::Real curvature, Ogre::Real tiling, Ogre::Real distance, bool drawFirst, Mogre::Quaternion orientation, int xsegments, int ysegments, int ysegments_keep, String^ groupName);
		void SetSkyDome(bool enable, String^ materialName, Ogre::Real curvature, Ogre::Real tiling, Ogre::Real distance, bool drawFirst, Mogre::Quaternion orientation, int xsegments, int ysegments, int ysegments_keep);
		void SetSkyDome(bool enable, String^ materialName, Ogre::Real curvature, Ogre::Real tiling, Ogre::Real distance, bool drawFirst, Mogre::Quaternion orientation, int xsegments, int ysegments);
		void SetSkyDome(bool enable, String^ materialName, Ogre::Real curvature, Ogre::Real tiling, Ogre::Real distance, bool drawFirst, Mogre::Quaternion orientation, int xsegments);
		void SetSkyDome(bool enable, String^ materialName, Ogre::Real curvature, Ogre::Real tiling, Ogre::Real distance, bool drawFirst, Mogre::Quaternion orientation);
		void SetSkyDome(bool enable, String^ materialName, Ogre::Real curvature, Ogre::Real tiling, Ogre::Real distance, bool drawFirst);
		void SetSkyDome(bool enable, String^ materialName, Ogre::Real curvature, Ogre::Real tiling, Ogre::Real distance);
		void SetSkyDome(bool enable, String^ materialName, Ogre::Real curvature, Ogre::Real tiling);
		void SetSkyDome(bool enable, String^ materialName, Ogre::Real curvature);
		void SetSkyDome(bool enable, String^ materialName);

		void SetFog(Mogre::FogMode mode, Mogre::Color4 colour, Ogre::Real expDensity, Ogre::Real linearStart, Ogre::Real linearEnd);
		void SetFog(Mogre::FogMode mode, Mogre::Color4 colour, Ogre::Real expDensity, Ogre::Real linearStart);
		void SetFog(Mogre::FogMode mode, Mogre::Color4 colour, Ogre::Real expDensity);
		void SetFog(Mogre::FogMode mode, Mogre::Color4 colour);
		void SetFog(Mogre::FogMode mode);
		void SetFog();

		Mogre::Animation^ CreateAnimation(String^ name, Mogre::Real length);

		Mogre::Animation^ GetAnimation(String^ name);

		bool HasAnimation(String^ name);

		void DestroyAnimation(String^ name);

		void DestroyAllAnimations();

		Mogre::AnimationState^ CreateAnimationState(String^ animName);

		Mogre::AnimationState^ GetAnimationState(String^ animName);

		bool HasAnimationState(String^ name);

		void DestroyAnimationState(String^ name);

		void DestroyAllAnimationStates();

		void ClearScene();

	internal:
		property Ogre::SceneManager* UnmanagedPointer
		{
			Ogre::SceneManager* get();
		}
	};
}