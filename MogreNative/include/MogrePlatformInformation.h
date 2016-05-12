#pragma once

#include "OgrePlatformInformation.h"
#include "MogreCommon.h"

namespace Mogre
{
	[Flags]
	public enum class CpuFeatures
	{
		None = Ogre::PlatformInformation::CPU_FEATURE_NONE,
		FeatureSSE = Ogre::PlatformInformation::CPU_FEATURE_SSE,
		FeatureSSE2 = Ogre::PlatformInformation::CPU_FEATURE_SSE2,
		FeatureSSE3 = Ogre::PlatformInformation::CPU_FEATURE_SSE3,
		FeatureMMX = 1 << 3,
		FeatureMMXEXT = 1 << 4,
		Feature3DNOW = 1 << 5,
		Feature3DNOWEXT = 1 << 6,
		FeatureCMOV = 1 << 7,
		FeatureTSC = 1 << 8,
		FeatureFPU = 1 << 9,
		FeaturePRO = 1 << 10,
		FeatureHTT = 1 << 11,
		FeatureVFP = 1 << 12,
		FeatureNEON = 1 << 13,
		FeatureMSA = 1 << 14
	};

	public ref class PlatformInformation abstract sealed
	{
	public:
		static String^ GetCpuIdentifier();
		static bool HasCpuFeature(CpuFeatures feature);

		static property uint32_t CpuFeatures
		{
			uint32_t get()
			{
				return Ogre::PlatformInformation::getCpuFeatures();
			}
		}

		static property uint32_t NumLogicalCores
		{
			uint32_t get()
			{
				return Ogre::PlatformInformation::getNumLogicalCores();
			}
		}
	};
}