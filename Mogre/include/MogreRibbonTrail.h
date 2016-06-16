#pragma once

#include "OgreRibbonTrail.h"
#include "MogreBillboardChain.h"

namespace Mogre
{
	public ref class RibbonTrail : public BillboardChain
	{
	public protected:
		RibbonTrail(Ogre::RibbonTrail* obj) : BillboardChain(obj)
		{

		}

		RibbonTrail(IntPtr ptr) : BillboardChain(ptr)
		{

		}

	public:
		DEFINE_MANAGED_NATIVE_CONVERSIONS(RibbonTrail);
	};
}