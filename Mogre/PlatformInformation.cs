using System;
using System.Runtime.InteropServices;

namespace Mogre
{
	public static class PlatformInformation
	{
		public static int NumLogicalCores
		{
			get
			{
				return (int)PlatformInformation_getNumLogicalCores();
			}
		}

		public static CpuFeatures CpuFeatures
		{
			get
			{
				return (CpuFeatures)PlatformInformation_getCpuFeatures();
			}
		}

		public static bool HasCpuFeature(CpuFeatures feature)
		{
			return PlatformInformation_hasCpuFeature((uint)feature);
		}

		#region PInvoke

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern uint PlatformInformation_getNumLogicalCores();

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern uint PlatformInformation_getCpuFeatures();

		[return: MarshalAs(UnmanagedType.U1)]
		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern bool PlatformInformation_hasCpuFeature(uint features);

		#endregion PInvoke

	}

	/// <summary>
	/// Enum describing the different CPU features we want to check for, platform-dependent
	/// </summary>
	[Flags]
	public enum CpuFeatures
	{
		None = 0,
		FeatureSSE = 1 << 0,
		FeatureSSE2 = 1 << 1,
		FeatureSSE3 = 1 << 2,
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
	}
}
