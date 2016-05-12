#include "stdafx.h"
#include "MogreNode.h"
#include "Marshalling.h"

using namespace Mogre;

Ogre::Node* Node::UnmanagedPointer::get()
{
	return _native;
}