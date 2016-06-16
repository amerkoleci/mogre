#pragma once

#include "OgreSceneManager.h"
#include "OgreRenderQueueListener.h"
#include "OgreSceneManagerEnumerator.h"
#include "MogreCommon.h"
#include "MogreCamera.h"
#include "MogreLight.h"
#include "MogreBillboardSet.h"
#include "MogreBillboardChain.h"
#include "MogreRibbonTrail.h"
#include "MogreParticleSystem.h"
#include "MogreManualObject.h"
#include "MogreStaticGeometry.h"
#include "STLContainerWrappers.h"
#include "IteratorWrapper.h"
#include "Marshalling.h"

namespace Mogre
{
	ref class RenderSystem;
	ref class SceneNode;
	ref class Entity;
	ref class Animation;
	ref class AnimationState;
	ref class MeshPtr;
	ref class MovableObject;
	ref class SceneQuery;
	ref class AxisAlignedBoxSceneQuery;
	ref class SphereSceneQuery;
	ref class PlaneBoundedVolumeListSceneQuery;
	ref class RaySceneQuery;
	ref class IntersectionSceneQuery;
	ref class Pass;
	ref class Viewport;

	interface class IRenderQueueListener_Receiver
	{
		void RenderQueueStarted(Ogre::uint8 queueGroupId, String^ invocation, [Out] bool% skipThisInvocation);
		void RenderQueueEnded(Ogre::uint8 queueGroupId, String^ invocation, [Out] bool% repeatThisInvocation);
	};

	public ref class RenderQueueListener abstract sealed
	{
	public:
		delegate static void RenderQueueStartedHandler(Ogre::uint8 queueGroupId, String^ invocation, [Out] bool% skipThisInvocation);
		delegate static void RenderQueueEndedHandler(Ogre::uint8 queueGroupId, String^ invocation, [Out] bool% repeatThisInvocation);
	};

	//################################################################
	//RenderQueueListener
	//################################################################

	class RenderQueueListener_Director : public Ogre::RenderQueueListener
	{
	private:
		gcroot<IRenderQueueListener_Receiver^> _receiver;

	public:
		RenderQueueListener_Director(IRenderQueueListener_Receiver^ recv)
			: _receiver(recv), doCallForRenderQueueStarted(false), doCallForRenderQueueEnded(false)
		{
		}

		bool doCallForRenderQueueStarted;
		bool doCallForRenderQueueEnded;

		virtual void preRenderQueues() override;
		virtual void postRenderQueues() override;
		virtual void renderQueueStarted(Ogre::RenderQueue *rq, Ogre::uint8 queueGroupId, const Ogre::String& invocation, bool& skipThisInvocation) override;
		virtual void renderQueueEnded(Ogre::uint8 queueGroupId, const Ogre::String& invocation, bool& repeatThisInvocation) override;
	};

	public ref class SceneManager : IMogreDisposable, public IRenderQueueListener_Receiver
	{
		Mogre::SceneNode^ _sceneRootDynamic;
		Mogre::SceneNode^ _sceneRootStatic;

	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	public:
		INC_DECLARE_STLVECTOR(MovableObjectVec, Mogre::MovableObject^, Ogre::MovableObject*, public:, private:);
		INC_DECLARE_ITERATOR(MovableObjectIterator, Ogre::SceneManager::MovableObjectIterator, Mogre::SceneManager::MovableObjectVec, Mogre::MovableObject^, Ogre::MovableObject*);

		INC_DECLARE_STLVECTOR(CameraList, Mogre::Camera^, Ogre::Camera*, public:, private:);
		INC_DECLARE_ITERATOR_NOCONSTRUCTOR(CameraIterator, Ogre::SceneManager::CameraIterator, Mogre::SceneManager::CameraList, Mogre::Camera^, Ogre::Camera*);

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

		//Event and Listener fields
		RenderQueueListener_Director* _renderQueueListener;
		Mogre::RenderQueueListener::RenderQueueStartedHandler^ _renderQueueStarted;
		Mogre::RenderQueueListener::RenderQueueEndedHandler^ _renderQueueEnded;

		System::Collections::Generic::Dictionary<String^, SceneNode^>^ _sceneNodes;
		System::Collections::Generic::Dictionary<String^, Entity^>^ _entities;
		System::Collections::Generic::Dictionary<String^, ManualObject^>^ _manualObjects;

	private:
		SceneManager(Ogre::SceneManager* obj) : _native(obj)
		{
			_sceneNodes = gcnew System::Collections::Generic::Dictionary<String^, SceneNode^>();
			_entities = gcnew System::Collections::Generic::Dictionary<String^, Entity^>();
			_manualObjects = gcnew System::Collections::Generic::Dictionary<String^, ManualObject^>();
		}

	public protected:
		SceneManager(IntPtr ptr) : _native((Ogre::SceneManager*)ptr.ToPointer())
		{
			_sceneNodes = gcnew System::Collections::Generic::Dictionary<String^, SceneNode^>();
			_entities = gcnew System::Collections::Generic::Dictionary<String^, Entity^>();
			_manualObjects = gcnew System::Collections::Generic::Dictionary<String^, ManualObject^>();
		}

	public:
		~SceneManager();
	protected:
		!SceneManager();

	public:
		event Mogre::RenderQueueListener::RenderQueueStartedHandler^ RenderQueueStarted
		{
			void add(Mogre::RenderQueueListener::RenderQueueStartedHandler^ hnd)
			{
				if (_renderQueueStarted == CLR_NULL)
				{
					if (_renderQueueListener == 0)
					{
						_renderQueueListener = new RenderQueueListener_Director(this);
						_native->addRenderQueueListener(_renderQueueListener);
					}
					_renderQueueListener->doCallForRenderQueueStarted = true;
				}
				_renderQueueStarted += hnd;
			}
			void remove(Mogre::RenderQueueListener::RenderQueueStartedHandler^ hnd)
			{
				_renderQueueStarted -= hnd;
				if (_renderQueueStarted == CLR_NULL) _renderQueueListener->doCallForRenderQueueStarted = false;
			}
		private:
			void raise(Ogre::uint8 queueGroupId, String^ invocation, [Out] bool% skipThisInvocation)
			{
				if (_renderQueueStarted)
					_renderQueueStarted->Invoke(queueGroupId, invocation, skipThisInvocation);
			}
		}

		event Mogre::RenderQueueListener::RenderQueueEndedHandler^ RenderQueueEnded
		{
			void add(Mogre::RenderQueueListener::RenderQueueEndedHandler^ hnd)
			{
				if (_renderQueueEnded == CLR_NULL)
				{
					if (_renderQueueListener == 0)
					{
						_renderQueueListener = new RenderQueueListener_Director(this);
						_native->addRenderQueueListener(_renderQueueListener);
					}
					_renderQueueListener->doCallForRenderQueueEnded = true;
				}
				_renderQueueEnded += hnd;
			}
			void remove(Mogre::RenderQueueListener::RenderQueueEndedHandler^ hnd)
			{
				_renderQueueEnded -= hnd;
				if (_renderQueueEnded == CLR_NULL) _renderQueueListener->doCallForRenderQueueEnded = false;
			}
		private:
			void raise(Ogre::uint8 queueGroupId, String^ invocation, [Out] bool% repeatThisInvocation)
			{
				if (_renderQueueEnded)
					_renderQueueEnded->Invoke(queueGroupId, invocation, repeatThisInvocation);
			}
		}

		property bool IsDisposed
		{
			virtual bool get();
		}

		property String^ Name
		{
		public:
			String^ get();
		}

		property String^ TypeName
		{
		public:
			String^ get();
		}

		property Mogre::ColourValue AmbientLight
		{
		public:
			Mogre::ColourValue get();
		public:
			void set(Mogre::ColourValue value);
		}

		property Mogre::Viewport^ CurrentViewport
		{
		public:
			Mogre::Viewport^ get();
		}

		property Mogre::Camera^ CameraInProgress
		{
		public:
			Mogre::Camera^ get();
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

		property Ogre::Real ShadowFarDistance
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real distance);
		}

		property bool ShadowCasterRenderBackFaces
		{
		public:
			bool get();
		public:
			void set(bool bf);
		}

		property Mogre::ColourValue ShadowColour
		{
		public:
			Mogre::ColourValue get();
		public:
			void set(Mogre::ColourValue colour);
		}

		property bool FindVisibleObjects
		{
		public:
			bool get();
		public:
			void set(bool find);
		}

		property Mogre::ColourValue FogColour
		{
		public:
			Mogre::ColourValue get();
		}

		property Mogre::Real FogDensity
		{
		public:
			Mogre::Real get();
		}

		property Mogre::Real FogEnd
		{
		public:
			Mogre::Real get();
		}

		property Mogre::FogMode FogMode
		{
		public:
			Mogre::FogMode get();
		}

		property Mogre::Real FogStart
		{
		public:
			Mogre::Real get();
		}

		property Ogre::uint32 VisibilityMask
		{
		public:
			Ogre::uint32 get();
		public:
			void set(Ogre::uint32 vmask);
		}

		Mogre::SceneNode^ GetRootSceneNode();
		Mogre::SceneNode^ GetRootSceneNode(SceneMemoryMgrTypes sceneType);

		Mogre::SceneNode^ CreateSceneNode();
		Mogre::SceneNode^ CreateSceneNode(SceneMemoryMgrTypes sceneType);
		Mogre::SceneNode^ CreateSceneNode(String^ name);
		Mogre::SceneNode^ CreateSceneNode(String^ name, SceneMemoryMgrTypes sceneType);

		void DestroySceneNode(Mogre::SceneNode^ node);
		Mogre::SceneNode^ GetSceneNode(Ogre::IdType id);
		Mogre::SceneNode^ GetSceneNode(String^ name);
		bool HasSceneNode(String^ name);
		void DestroySceneNode(String^ name);

		void RegisterSceneNodeListener(Mogre::SceneNode^ node);
		void UnregisterSceneNodeListener(Mogre::SceneNode^ node);

		Mogre::BillboardSet^ CreateBillboardSet(unsigned int poolSize);
		void DestroyBillboardSet(Mogre::BillboardSet^ set);
		void DestroyAllBillboardSets();

		Mogre::BillboardChain^ CreateBillboardChain();
		void DestroyBillboardChain(Mogre::BillboardChain^ obj);
		void DestroyAllBillboardChains();

		Mogre::ManualObject^ CreateManualObject(SceneMemoryMgrTypes sceneType);
		Mogre::ManualObject^ CreateManualObject();
		void DestroyManualObject(Mogre::ManualObject^ obj);
		void DestroyAllManualObjects();

		Mogre::ManualObject^ CreateManualObject(String^ name);
		Mogre::ManualObject^ CreateManualObject(String^ name, SceneMemoryMgrTypes sceneType);
		Mogre::ManualObject^ GetManualObject(String^ name);
		bool HasManualObject(String^ name);
		void DestroyManualObject(String^ name);

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
		Mogre::Camera^ GetCamera(String^ name);
		bool HasCamera(String^ name);
		void DestroyCamera(Mogre::Camera^ camera);
		void DestroyCamera(String^ name);
		void DestroyAllCameras();

		Mogre::Entity^ CreateEntity(String^ meshName);
		Mogre::Entity^ CreateEntity(String^ meshName, String^ groupName, SceneMemoryMgrTypes sceneType);
		Mogre::Entity^ CreateEntity(Mogre::SceneManager::PrefabType ptype);
		Mogre::Entity^ CreateEntity(Mogre::SceneManager::PrefabType ptype, SceneMemoryMgrTypes sceneType);
		Mogre::Entity^ CreateEntity(MeshPtr^ mesh, SceneMemoryMgrTypes sceneType);
		Mogre::Entity^ CreateEntity(MeshPtr^ mesh);

		Mogre::Entity^ CreateEntity(String^ name, String^ meshName);
		Mogre::Entity^ CreateEntity(String^ name, String^ meshName, String^ groupName);
		Mogre::Entity^ CreateEntity(String^ name, String^ meshName, String^ groupName, SceneMemoryMgrTypes sceneType);
		Mogre::Entity^ CreateEntity(String^ name, Mogre::SceneManager::PrefabType ptype);
		Mogre::Entity^ CreateEntity(String^ name, Mogre::SceneManager::PrefabType ptype, SceneMemoryMgrTypes sceneType);
		Mogre::Entity^ CreateEntity(String^ name, MeshPtr^ mesh, SceneMemoryMgrTypes sceneType);
		Mogre::Entity^ CreateEntity(String^ name, MeshPtr^ mesh);

		void DestroyEntity(Mogre::Entity^ ent);
		Mogre::Entity^ GetEntity(String^ name);
		bool HasEntity(String^ name);
		void DestroyEntity(String^ name);
		void DestroyAllEntities();

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

		void SetFog(Mogre::FogMode mode, Mogre::ColourValue colour, Ogre::Real expDensity, Ogre::Real linearStart, Ogre::Real linearEnd);
		void SetFog(Mogre::FogMode mode, Mogre::ColourValue colour, Ogre::Real expDensity, Ogre::Real linearStart);
		void SetFog(Mogre::FogMode mode, Mogre::ColourValue colour, Ogre::Real expDensity);
		void SetFog(Mogre::FogMode mode, Mogre::ColourValue colour);
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

		Mogre::StaticGeometry^ CreateStaticGeometry(String^ name);
		Mogre::StaticGeometry^ GetStaticGeometry(String^ name);
		bool HasStaticGeometry(String^ name);
		void DestroyStaticGeometry(Mogre::StaticGeometry^ geom);
		void DestroyStaticGeometry(String^ name);
		void DestroyAllStaticGeometry();

		Mogre::AxisAlignedBoxSceneQuery^ CreateAABBQuery(Mogre::AxisAlignedBox^ box, unsigned long mask);
		Mogre::AxisAlignedBoxSceneQuery^ CreateAABBQuery(Mogre::AxisAlignedBox^ box);

		Mogre::SphereSceneQuery^ CreateSphereQuery(Mogre::Sphere sphere, unsigned long mask);
		Mogre::SphereSceneQuery^ CreateSphereQuery(Mogre::Sphere sphere);

		//Mogre::PlaneBoundedVolumeListSceneQuery^ CreatePlaneBoundedVolumeQuery(Mogre::Const_PlaneBoundedVolumeList^ volumes, unsigned long mask);
		//Mogre::PlaneBoundedVolumeListSceneQuery^ CreatePlaneBoundedVolumeQuery(Mogre::Const_PlaneBoundedVolumeList^ volumes);

		Mogre::RaySceneQuery^ CreateRayQuery(Mogre::Ray ray, unsigned long mask);
		Mogre::RaySceneQuery^ CreateRayQuery(Mogre::Ray ray);

		Mogre::IntersectionSceneQuery^ CreateIntersectionQuery(unsigned long mask);
		Mogre::IntersectionSceneQuery^ CreateIntersectionQuery();
		void DestroyQuery(Mogre::SceneQuery^ query);

		Mogre::SceneManager::CameraIterator^ GetCameraIterator();

		void ClearScene();

		bool HasMovableObject(MovableObject^ movable);
		void DestroyMovableObject(MovableObject^ movable, String^ typeName);
		void DestroyMovableObject(MovableObject^ movable);
		void DestroyAllMovableObjectsByType(String^ typeName);
		void DestroyAllMovableObjects();

		Mogre::SceneManager::MovableObjectIterator^ GetMovableObjectIterator(String^ typeName);

		void InjectMovableObject(Mogre::MovableObject^ m);
		void ExtractMovableObject(Mogre::MovableObject^ m);
		void ExtractAllMovableObjectsByType(String^ typeName);

		Mogre::Pass^ _setPass(Mogre::Pass^ pass, bool evenIfSuppressed, bool shadowDerivation);
		Mogre::Pass^ _setPass(Mogre::Pass^ pass, bool evenIfSuppressed);
		Mogre::Pass^ _setPass(Mogre::Pass^ pass);

		void NotifyStaticAabbDirty(Mogre::MovableObject^ movableObject);
		void NotifyStaticDirty(Mogre::Node^ node);
		void UpdateAllAnimations();
		void UpdateAllTransforms();

		void UpdateAllLods(Mogre::Camera^ lodCamera, Ogre::Real lodBias, Ogre::uint8 firstRq, Ogre::uint8 lastRq);
		void UpdateSceneGraph();

		DEFINE_MANAGED_NATIVE_CONVERSIONS(SceneManager);

	internal:
		property Ogre::SceneManager* UnmanagedPointer
		{
			Ogre::SceneManager* get();
		}

	protected public:
		virtual void OnRenderQueueStarted(Ogre::uint8 queueGroupId, String^ invocation, [Out] bool% skipThisInvocation) = IRenderQueueListener_Receiver::RenderQueueStarted
		{
			RenderQueueStarted(queueGroupId, invocation, skipThisInvocation);
		}

			virtual void OnRenderQueueEnded(Ogre::uint8 queueGroupId, String^ invocation, [Out] bool% repeatThisInvocation) = IRenderQueueListener_Receiver::RenderQueueEnded
		{
			RenderQueueEnded(queueGroupId, invocation, repeatThisInvocation);
		}
	};

	public ref class SceneManagerEnumerator
	{
	public:
		INC_DECLARE_STLMAP(Instances, String^, Mogre::SceneManager^, Ogre::String, Ogre::SceneManager*, public:, private:);
		INC_DECLARE_MAP_ITERATOR(SceneManagerIterator, Ogre::SceneManagerEnumerator::SceneManagerIterator, Mogre::SceneManagerEnumerator::Instances, Mogre::SceneManager^, Ogre::SceneManager*, String^, Ogre::String);
	};
}