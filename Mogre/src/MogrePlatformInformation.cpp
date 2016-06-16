#include "stdafx.h"
#include "MogrePlatformInformation.h"
#include "Marshalling.h"

using namespace Mogre;

String^ PlatformInformation::GetCpuIdentifier()
{
	return TO_CLR_STRING(Ogre::PlatformInformation::getCpuIdentifier());
}

bool PlatformInformation::HasCpuFeature(Mogre::CpuFeatures feature)
{
	return Ogre::PlatformInformation::hasCpuFeature((Ogre::PlatformInformation::CpuFeatures)feature);
}