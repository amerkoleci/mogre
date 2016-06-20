#pragma once

#include "OgreStaticGeometry.h"
#include "MogreMovableObject.h"
#include "MogreRenderOperation.h"
#include "MogreCommon.h"
#include "Marshalling.h"

namespace Mogre
{
	ref class Entity;
	ref class SceneNode;

	public ref class StaticGeometry : IMogreDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	internal:
		Ogre::StaticGeometry* _native;
		bool _createdByCLR;

	public protected:
		StaticGeometry(Ogre::StaticGeometry* obj) : _native(obj)
		{

		}

	public:
		~StaticGeometry();
	protected:
		!StaticGeometry();

	public:
		property bool IsDisposed
		{
			virtual bool get()
			{
				return _native == nullptr;
			}
		}

		property bool CastShadows
		{
		public:
			bool get();
		public:
			void set(bool castShadows);
		}

		property bool IsVisible
		{
		public:
			bool get();
		}

		property String^ Name
		{
		public:
			String^ get();
		}

		property Mogre::Vector3 Origin
		{
		public:
			Mogre::Vector3 get();
		public:
			void set(Mogre::Vector3 origin);
		}

		property Mogre::Vector3 RegionDimensions
		{
		public:
			Mogre::Vector3 get();
		public:
			void set(Mogre::Vector3 size);
		}

		property Mogre::Real RenderingDistance
		{
		public:
			Mogre::Real get();
		public:
			void set(Mogre::Real dist);
		}

		property Ogre::uint8 RenderQueueGroup
		{
		public:
			Ogre::uint8 get();
		public:
			void set(Ogre::uint8 queueID);
		}

		property Mogre::Real SquaredRenderingDistance
		{
		public:
			Mogre::Real get();
		}

		property Ogre::uint32 VisibilityFlags
		{
		public:
			Ogre::uint32 get();
		public:
			void set(Ogre::uint32 flags);
		}

		void AddEntity(Mogre::Entity^ ent, Mogre::Vector3 position, Mogre::Quaternion orientation, Mogre::Vector3 scale);
		void AddEntity(Mogre::Entity^ ent, Mogre::Vector3 position, Mogre::Quaternion orientation);
		void AddEntity(Mogre::Entity^ ent, Mogre::Vector3 position);

		void AddSceneNode(Mogre::SceneNode^ node);

		void Build();

		void Destroy();

		void Reset();

		void SetVisible(bool visible);

		//Mogre::StaticGeometry::RegionIterator^ GetRegionIterator();

		void Dump(String^ filename);

		DEFINE_MANAGED_NATIVE_CONVERSIONS(StaticGeometry);

	internal:
		property Ogre::StaticGeometry* UnmanagedPointer
		{
			Ogre::StaticGeometry* get() { return _native; }
		}
	};
}