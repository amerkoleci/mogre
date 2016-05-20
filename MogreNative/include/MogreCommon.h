#pragma once

#include "OgreCommon.h"
#include "STLContainerWrappers.h"

namespace Mogre
{
	typedef Ogre::ulong ulong;
	typedef Ogre::uint uint;
	typedef Ogre::ushort ushort;
	typedef Ogre::uchar uchar;
	typedef Ogre::Real Real;
	typedef Mogre::Color4 ColourValue;

	public enum class FrameBufferType
	{
		FBT_COLOUR = Ogre::FBT_COLOUR,
		FBT_DEPTH = Ogre::FBT_DEPTH,
		FBT_STENCIL = Ogre::FBT_STENCIL,
		Color = Ogre::FBT_COLOUR,
		Depth = Ogre::FBT_DEPTH,
		Stencil = Ogre::FBT_STENCIL,
	};

	public enum class SortMode
	{
		SM_DIRECTION = Ogre::SM_DIRECTION,
		SM_DISTANCE = Ogre::SM_DISTANCE,
		Direction = Ogre::SM_DIRECTION,
		Distance = Ogre::SM_DISTANCE
	};

	public enum class SceneMemoryMgrTypes
	{
		SCENE_DYNAMIC = Ogre::SCENE_DYNAMIC,
		SCENE_STATIC = Ogre::SCENE_STATIC,
		Dynamic = Ogre::SCENE_DYNAMIC,
		Static = Ogre::SCENE_STATIC,
	};

	[Flags]
	public enum class SceneType
	{
		ST_GENERIC = Ogre::ST_GENERIC,
		ST_EXTERIOR_CLOSE = Ogre::ST_EXTERIOR_CLOSE,
		ST_EXTERIOR_FAR = Ogre::ST_EXTERIOR_FAR,
		ST_EXTERIOR_REAL_FAR = Ogre::ST_EXTERIOR_REAL_FAR,
		ST_INTERIOR = Ogre::ST_INTERIOR,
		Generic = Ogre::ST_GENERIC,
		ExteriorClose = Ogre::ST_EXTERIOR_CLOSE,
		ExteriorFar = Ogre::ST_EXTERIOR_FAR,
		ExteriorRealFar = Ogre::ST_EXTERIOR_REAL_FAR,
		Interior = Ogre::ST_INTERIOR
	};


	public enum class TrackVertexColourEnum
	{
		TVC_NONE = Ogre::TVC_NONE,
		TVC_AMBIENT = Ogre::TVC_AMBIENT,
		TVC_DIFFUSE = Ogre::TVC_DIFFUSE,
		TVC_SPECULAR = Ogre::TVC_SPECULAR,
		TVC_EMISSIVE = Ogre::TVC_EMISSIVE
	};

	public enum class PolygonMode
	{
		PM_POINTS = Ogre::PM_POINTS,
		PM_WIREFRAME = Ogre::PM_WIREFRAME,
		PM_SOLID = Ogre::PM_SOLID
	};

	public enum class WaveformType
	{
		WFT_SINE = Ogre::WFT_SINE,
		WFT_TRIANGLE = Ogre::WFT_TRIANGLE,
		WFT_SQUARE = Ogre::WFT_SQUARE,
		WFT_SAWTOOTH = Ogre::WFT_SAWTOOTH,
		WFT_INVERSE_SAWTOOTH = Ogre::WFT_INVERSE_SAWTOOTH,
		WFT_PWM = Ogre::WFT_PWM
	};

	public enum class ManualCullingMode
	{
		MANUAL_CULL_NONE = Ogre::MANUAL_CULL_NONE,
		MANUAL_CULL_BACK = Ogre::MANUAL_CULL_BACK,
		MANUAL_CULL_FRONT = Ogre::MANUAL_CULL_FRONT
	};

	public enum class CullingMode
	{
		CULL_NONE = Ogre::CULL_NONE,
		CULL_CLOCKWISE = Ogre::CULL_CLOCKWISE,
		CULL_ANTICLOCKWISE = Ogre::CULL_ANTICLOCKWISE
	};

	public enum class FogMode
	{
		FOG_NONE = Ogre::FOG_NONE,
		FOG_EXP = Ogre::FOG_EXP,
		FOG_EXP2 = Ogre::FOG_EXP2,
		FOG_LINEAR = Ogre::FOG_LINEAR
	};

	public enum class ShadeOptions
	{
		SO_FLAT = Ogre::SO_FLAT,
		SO_GOURAUD = Ogre::SO_GOURAUD,
		SO_PHONG = Ogre::SO_PHONG
	};

	public enum class FilterOptions
	{
		FO_NONE = Ogre::FO_NONE,
		FO_POINT = Ogre::FO_POINT,
		FO_LINEAR = Ogre::FO_LINEAR,
		FO_ANISOTROPIC = Ogre::FO_ANISOTROPIC
	};

	public enum class FilterType
	{
		FT_MIN = Ogre::FT_MIN,
		FT_MAG = Ogre::FT_MAG,
		FT_MIP = Ogre::FT_MIP
	};

	public enum class CompareFunction
	{
		CMPF_ALWAYS_FAIL = Ogre::CMPF_ALWAYS_FAIL,
		CMPF_ALWAYS_PASS = Ogre::CMPF_ALWAYS_PASS,
		CMPF_LESS = Ogre::CMPF_LESS,
		CMPF_LESS_EQUAL = Ogre::CMPF_LESS_EQUAL,
		CMPF_EQUAL = Ogre::CMPF_EQUAL,
		CMPF_NOT_EQUAL = Ogre::CMPF_NOT_EQUAL,
		CMPF_GREATER_EQUAL = Ogre::CMPF_GREATER_EQUAL,
		CMPF_GREATER = Ogre::CMPF_GREATER
	};

	public enum class TextureFilterOptions
	{
		TFO_NONE = Ogre::TFO_NONE,
		TFO_BILINEAR = Ogre::TFO_BILINEAR,
		TFO_TRILINEAR = Ogre::TFO_TRILINEAR,
		TFO_ANISOTROPIC = Ogre::TFO_ANISOTROPIC
	};

	INC_DECLARE_STLMAP(UnaryOptionList, String^, bool, Ogre::String, bool, public, private);
	INC_DECLARE_STLMAP(BinaryOptionList, String^, String^, Ogre::String, Ogre::String, public, private);
	INC_DECLARE_STLMAP(NameValuePairList, String^, String^, Ogre::String, Ogre::String, public, private);
}