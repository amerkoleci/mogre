#include "stdafx.h"
#include "MogreRibbonTrail.h"
#include "Marshalling.h"

using namespace Mogre;

Ogre::RibbonTrail* RibbonTrail::UnmanagedPointer::get()
{
	return static_cast<Ogre::RibbonTrail*>(_native);
}