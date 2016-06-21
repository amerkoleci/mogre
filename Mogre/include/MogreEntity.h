#pragma once

#include "OgreEntity.h"
#include "MogreMovableObject.h"
#include "MogreRenderOperation.h"
#include "Marshalling.h"

namespace Mogre
{
	ref class Entity;
	ref class VertexData;
	ref class Mesh;
	ref class MeshPtr;
	ref class SubMesh;
	ref class Technique;
	ref class MaterialPtr;
	ref class Camera;
	ref class AnimationState;
	ref class AnimationStateSet;

	public ref class SubEntity // : public IRenderable
	{
	internal:
		Ogre::SubEntity* _native;
		bool _createdByCLR;

	public protected:
		SubEntity(Ogre::SubEntity* ptr) : _native(ptr)
		{

		}

	public:
		property bool CastsShadows
		{
		public:
			virtual bool get();
		}

		property String^ MaterialName
		{
		public:
			String^ get();
		public:
			void set(String^ name);
		}

		property unsigned short NumWorldTransforms
		{
		public:
			virtual unsigned short get();
		}

		property Mogre::Entity^ Parent
		{
		public:
			Mogre::Entity^ get();
		}

		property Mogre::SubMesh^ SubMesh
		{
		public:
			Mogre::SubMesh^ get();
		}

		property Mogre::Technique^ Technique
		{
		public:
			virtual Mogre::Technique^ get();
		}

		property Mogre::VertexData^ VertexDataForBinding
		{
		public:
			Mogre::VertexData^ get();
		}

		virtual Mogre::MaterialPtr^ GetMaterial();

		virtual void GetRenderOperation(Mogre::RenderOperation^ op);

		virtual void GetWorldTransforms(Mogre::Matrix4* xform);

		virtual Ogre::Real GetSquaredViewDepth(Mogre::Camera^ cam);

		property bool PolygonModeOverrideable
		{
		public:
			virtual bool get();
		public:
			virtual void set(bool override);
		}

		property bool UseIdentityProjection
		{
		public:
			bool get();
		public:
			void set(bool useIdentityProjection);
		}

		property bool UseIdentityView
		{
		public:
			bool get();
		public:
			void set(bool useIdentityView);
		}

		void SetMaterialName(String^ name);
		void SetMaterialName(String^ name, String^ resGroup);

		void SetCustomParameter(size_t index, Mogre::Vector4 value);
		Mogre::Vector4 GetCustomParameter(size_t index);

		DEFINE_MANAGED_NATIVE_CONVERSIONS(SubEntity);

	internal:
		property Ogre::SubEntity* UnmanagedPointer
		{
			Ogre::SubEntity* get();
		}
	};

	public ref class Entity : public MovableObject
	{
		System::Collections::Generic::List<SubEntity^>^ _subEntities;

	public protected:
		Entity(Ogre::Entity* obj);

	public:
		property Mogre::AnimationStateSet^ AllAnimationStates
		{
		public:
			Mogre::AnimationStateSet^ get();
		}

		property bool DisplaySkeleton
		{
		public:
			bool get();
		public:
			void set(bool display);
		}

		/*property Mogre::EdgeData^ EdgeList
		{
		public:
			Mogre::EdgeData^ get();
		}*/

		property bool HasEdgeList
		{
		public:
			bool get();
		}

		property bool HasSkeleton
		{
		public:
			bool get();
		}

		property bool HasVertexAnimation
		{
		public:
			bool get();
		}

		property bool IsHardwareAnimationEnabled
		{
		public:
			bool get();
		}

		property bool IsInitialised
		{
		public:
			bool get();
		}

		property String^ MovableType
		{
		public:
			String^ get();
		}

		property size_t NumManualLodLevels
		{
		public:
			size_t get();
		}

		property unsigned int NumSubEntities
		{
		public:
			unsigned int get();
		}

		property Ogre::uint8 RenderQueueGroup
		{
		public:
			Ogre::uint8 get();
		public:
			void set(Ogre::uint8 queueID);
		}

		/*property Mogre::SkeletonInstance^ Skeleton
		{
		public:
			Mogre::SkeletonInstance^ get();
		}*/

		property int SoftwareAnimationNormalsRequests
		{
		public:
			int get();
		}

		property int SoftwareAnimationRequests
		{
		public:
			int get();
		}

		property Mogre::VertexData^ VertexDataForBinding
		{
		public:
			Mogre::VertexData^ get();
		}

		Mogre::MeshPtr^ GetMesh();

		Mogre::SubEntity^ GetSubEntity(size_t index);
		Mogre::SubEntity^ GetSubEntity(String^ name);

		Mogre::Entity^ Clone();
		Mogre::Entity^ Clone(String^ name);

		void SetMaterialName(String^ name);

		Mogre::AnimationState^ GetAnimationState(String^ name);
		Mogre::Entity^ GetManualLodLevel(size_t index);

		void SetPolygonModeOverrideable(bool PolygonModeOverrideable);

		DEFINE_MANAGED_NATIVE_CONVERSIONS_GET_MANAGED(Entity);
	};
}