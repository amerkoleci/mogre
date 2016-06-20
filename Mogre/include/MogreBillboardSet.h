#pragma once

#include "OgreBillboardSet.h"
#include "MogreMovableObject.h"

namespace Mogre
{
	public enum class BillboardType
	{
		BBT_POINT = Ogre::BBT_POINT,
		BBT_ORIENTED_COMMON = Ogre::BBT_ORIENTED_COMMON,
		BBT_ORIENTED_SELF = Ogre::BBT_ORIENTED_SELF,
		BBT_PERPENDICULAR_COMMON = Ogre::BBT_PERPENDICULAR_COMMON,
		BBT_PERPENDICULAR_SELF = Ogre::BBT_PERPENDICULAR_SELF
	};

	public enum class BillboardRotationType
	{
		BBR_VERTEX = Ogre::BBR_VERTEX,
		BBR_TEXCOORD = Ogre::BBR_TEXCOORD
	};

	public enum class BillboardOrigin
	{
		BBO_TOP_LEFT = Ogre::BBO_TOP_LEFT,
		BBO_TOP_CENTER = Ogre::BBO_TOP_CENTER,
		BBO_TOP_RIGHT = Ogre::BBO_TOP_RIGHT,
		BBO_CENTER_LEFT = Ogre::BBO_CENTER_LEFT,
		BBO_CENTER = Ogre::BBO_CENTER,
		BBO_CENTER_RIGHT = Ogre::BBO_CENTER_RIGHT,
		BBO_BOTTOM_LEFT = Ogre::BBO_BOTTOM_LEFT,
		BBO_BOTTOM_CENTER = Ogre::BBO_BOTTOM_CENTER,
		BBO_BOTTOM_RIGHT = Ogre::BBO_BOTTOM_RIGHT
	};

	public ref class BillboardSet : public MovableObject
	{
	public protected:
		BillboardSet(Ogre::BillboardSet* obj) : MovableObject(obj)
		{

		}

	public:
		DEFINE_MANAGED_NATIVE_CONVERSIONS_GET_MANAGED(BillboardSet);
	};
}