#include "stdafx.h"

extern "C"
{
	MOGRE_EXPORTS_API uint32_t PlatformInformation_getNumLogicalCores()
	{
		return Ogre::PlatformInformation::getNumLogicalCores();
	}

	MOGRE_EXPORTS_API uint32_t PlatformInformation_getCpuFeatures()
	{
		return Ogre::PlatformInformation::getCpuFeatures();
	}

	MOGRE_EXPORTS_API bool PlatformInformation_hasCpuFeature(Ogre::PlatformInformation::CpuFeatures features)
	{
		return Ogre::PlatformInformation::hasCpuFeature(features);
	}
}