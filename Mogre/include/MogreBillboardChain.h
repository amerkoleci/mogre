#pragma once

#include "OgreBillboardChain.h"
#include "MogreMovableObject.h"

namespace Mogre
{
	public ref class BillboardChain : public MovableObject
	{
	public:
		ref class ElementList;

		enum class TexCoordDirection
		{
			TCD_U = Ogre::BillboardChain::TCD_U,
			TCD_V = Ogre::BillboardChain::TCD_V,
			U = Ogre::BillboardChain::TCD_U,
			V = Ogre::BillboardChain::TCD_V,
		};


	public protected:
		BillboardChain(Ogre::BillboardChain* obj) : MovableObject(obj)
		{

		}

	public:
		DEFINE_MANAGED_NATIVE_CONVERSIONS_GET_MANAGED(BillboardChain);
	};
}