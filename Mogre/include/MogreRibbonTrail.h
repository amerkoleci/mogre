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

	public:
		DEFINE_MANAGED_NATIVE_CONVERSIONS_GET_MANAGED(RibbonTrail);
	};
}