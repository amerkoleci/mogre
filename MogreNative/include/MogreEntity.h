#pragma once

#include "OgreEntity.h"
#include "MogreMovableObject.h"

namespace Mogre
{
	ref class Entity;

	public ref class SubEntity // : public IRenderable
	{
	internal:
		Ogre::SubEntity* _native;
		bool _createdByCLR;

	public protected:
		SubEntity(intptr_t ptr) : _native((Ogre::SubEntity*)ptr)
		{

		}

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

	internal:
		property Ogre::SubEntity* UnmanagedPointer
		{
			Ogre::SubEntity* get();
		}
	};

	public ref class Entity : public MovableObject
	{
	public protected:
		Entity(intptr_t ptr) : MovableObject(ptr)
		{

		}

	public:
		Mogre::SubEntity^ GetSubEntity(size_t index);
		Mogre::SubEntity^ GetSubEntity(String^ name);

		Mogre::Entity^ Clone();

		void SetMaterialName(String^ name);

		void SetPolygonModeOverrideable(bool PolygonModeOverrideable);

	internal:
		property Ogre::Entity* UnmanagedPointer
		{
			Ogre::Entity* get();
		}
	};
}