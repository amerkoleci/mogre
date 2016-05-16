#pragma once

#include "OgreRibbonTrail.h"
#include "MogreBillboardChain.h"

namespace Mogre
{
	public ref class RibbonTrail : public BillboardChain
	{
	public protected:
		RibbonTrail(intptr_t ptr) : BillboardChain(ptr)
		{

		}

	internal:
		property Ogre::RibbonTrail* UnmanagedPointer
		{
			Ogre::RibbonTrail* get();
		}
	};
}