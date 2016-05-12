#pragma once
#include "OgreNode.h"

namespace Mogre
{
	public ref class Node 
	{
	internal:
		Ogre::Node* _native;

	private:
		Node(Ogre::Node* obj) : _native(obj)
		{
		}

	public protected:
		Node(intptr_t ptr) : _native((Ogre::Node*)ptr)
		{

		}

	internal:
		property Ogre::Node* UnmanagedPointer
		{
			Ogre::Node* get();
		}
	};
}