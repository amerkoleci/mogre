#pragma once

#include "OgreMeshManager.h"
#include "MogreResource.h"
#include "MogreResourceManager.h"

namespace Mogre
{
	public ref class MeshManager : public ResourceManager
	{
	private protected:
		static MeshManager^ _singleton;

	public protected:
		MeshManager(Ogre::MeshManager* obj) : ResourceManager(obj)
		{
		}

	public:

		static property MeshManager^ Singleton
		{
			MeshManager^ get()
			{
				if (_singleton == CLR_NULL)
				{
					Ogre::MeshManager* ptr = Ogre::MeshManager::getSingletonPtr();
					if (ptr) _singleton = gcnew MeshManager(ptr);
				}
				return _singleton;
			}
		}

		property Ogre::Real BoundsPaddingFactor
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real paddingFactor);
		}

		property bool PrepareAllMeshesForShadowVolumes
		{
		public:
			bool get();
		public:
			void set(bool enable);
		}
	};
}