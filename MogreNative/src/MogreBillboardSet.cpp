#include "stdafx.h"
#include "MogreBillboardSet.h"
#include "Marshalling.h"

using namespace Mogre;

Ogre::BillboardSet* BillboardSet::UnmanagedPointer::get()
{
	return static_cast<Ogre::BillboardSet*>(_native);
}