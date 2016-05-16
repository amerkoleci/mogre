#include "stdafx.h"
#include "MogreBillboardChain.h"
#include "Marshalling.h"

using namespace Mogre;

Ogre::BillboardChain* BillboardChain::UnmanagedPointer::get()
{
	return static_cast<Ogre::BillboardChain*>(_native);
}