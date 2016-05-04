#include "MogreStableHeaders.h"

#include "MogreHardwareVertexBuffer.h"
#include "MogreRoot.h"
#include "MogreRenderSystem.h"

namespace Mogre
{
    //-----------------------------------------------------------------------------
    VertexElement::VertexElement(unsigned short source, size_t offset, 
        VertexElementType theType, VertexElementSemantic semantic, unsigned short index)
    {
		data.mSource = source;
		data.mOffset = offset;
		data.mType = theType;
		data.mSemantic = semantic;
		data.mIndex = index;
    }
    //-----------------------------------------------------------------------------
	size_t VertexElement::Size::get()
	{
		return GetTypeSize(data.mType);
	}
	//-----------------------------------------------------------------------------
	size_t VertexElement::GetTypeSize(VertexElementType etype)
	{
		switch(etype)
		{
		case VertexElementType::VET_COLOUR:
		case VertexElementType::VET_COLOUR_ABGR:
		case VertexElementType::VET_COLOUR_ARGB:
			return sizeof(RGBA);
		case VertexElementType::VET_FLOAT1:
			return sizeof(float);
		case VertexElementType::VET_FLOAT2:
			return sizeof(float)*2;
		case VertexElementType::VET_FLOAT3:
			return sizeof(float)*3;
		case VertexElementType::VET_FLOAT4:
			return sizeof(float)*4;
		case VertexElementType::VET_SHORT1:
			return sizeof(short);
		case VertexElementType::VET_SHORT2:
			return sizeof(short)*2;
		case VertexElementType::VET_SHORT3:
			return sizeof(short)*3;
		case VertexElementType::VET_SHORT4:
			return sizeof(short)*4;
        case VertexElementType::VET_UBYTE4:
            return sizeof(unsigned char)*4;
		}
		return 0;
	}
	//-----------------------------------------------------------------------------
	unsigned short VertexElement::GetTypeCount(VertexElementType etype)
	{
		switch (etype)
		{
		case VertexElementType::VET_COLOUR:
		case VertexElementType::VET_COLOUR_ABGR:
		case VertexElementType::VET_COLOUR_ARGB:
			return 1;
		case VertexElementType::VET_FLOAT1:
			return 1;
		case VertexElementType::VET_FLOAT2:
			return 2;
		case VertexElementType::VET_FLOAT3:
			return 3;
		case VertexElementType::VET_FLOAT4:
			return 4;
		case VertexElementType::VET_SHORT1:
			return 1;
		case VertexElementType::VET_SHORT2:
			return 2;
		case VertexElementType::VET_SHORT3:
			return 3;
		case VertexElementType::VET_SHORT4:
			return 4;
        case VertexElementType::VET_UBYTE4:
            return 4;
		}

		throw gcnew ArgumentException("Invalid type", "etype");
	}
	//-----------------------------------------------------------------------------
	VertexElementType VertexElement::MultiplyTypeCount(VertexElementType baseType, 
		unsigned short count)
	{
		switch (baseType)
		{
		case VertexElementType::VET_FLOAT1:
			switch(count)
			{
			case 1:
				return VertexElementType::VET_FLOAT1;
			case 2:
				return VertexElementType::VET_FLOAT2;
			case 3:
				return VertexElementType::VET_FLOAT3;
			case 4:
				return VertexElementType::VET_FLOAT4;
            default:
                break;
			}
			break;
		case VertexElementType::VET_SHORT1:
			switch(count)
			{
			case 1:
				return VertexElementType::VET_SHORT1;
			case 2:
				return VertexElementType::VET_SHORT2;
			case 3:
				return VertexElementType::VET_SHORT3;
			case 4:
				return VertexElementType::VET_SHORT4;
            default:
                break;
			}
			break;
        default:
            break;
		}

		throw gcnew ArgumentException("Invalid base type", "baseType");
	}
	//--------------------------------------------------------------------------
	VertexElementType VertexElement::BestColourVertexElementType::get()
	{
		// Use the current render system to determine if possible
		if (Root::Singleton && Root::Singleton->RenderSystem)
		{
			return Root::Singleton->RenderSystem->ColourVertexElementType;
		}
		else
		{
			// We can't know the specific type right now, so pick a type
			// based on platform
#if OGRE_PLATFORM == OGRE_PLATFORM_WIN32
			return VertexElementType::VET_COLOUR_ARGB; // prefer D3D format on windows
#else
			return VertexElementType::VET_COLOUR_ABGR; // prefer GL format on everything else
#endif

		}
	}
	//--------------------------------------------------------------------------
	void VertexElement::ConvertColourValue(VertexElementType srcType, 
		VertexElementType dstType, uint32% ptr)
	{
		if (srcType == dstType)
			return;

		// Conversion between ARGB and ABGR is always a case of flipping R/B
		ptr = 
		   ((ptr&0x00FF0000)>>16)|((ptr&0x000000FF)<<16)|(ptr&0xFF00FF00);				
	}
	//--------------------------------------------------------------------------
	uint32 VertexElement::ConvertColourValue(ColourValue src, 
		VertexElementType dst)
	{
		switch(dst)
		{
#if OGRE_PLATFORM == OGRE_PLATFORM_WIN32
        default:
#endif
		case VertexElementType::VET_COLOUR_ARGB:
			return src.GetAsARGB();
#if OGRE_PLATFORM != OGRE_PLATFORM_WIN32
        default:
#endif
		case VertexElementType::VET_COLOUR_ABGR: 
			return src.GetAsABGR();
		};

	}
	//-----------------------------------------------------------------------------
	VertexElementType VertexElement::GetBaseType(VertexElementType multiType)
	{
		switch (multiType)
		{
			case VertexElementType::VET_FLOAT1:
			case VertexElementType::VET_FLOAT2:
			case VertexElementType::VET_FLOAT3:
			case VertexElementType::VET_FLOAT4:
				return VertexElementType::VET_FLOAT1;
			case VertexElementType::VET_COLOUR:
				return VertexElementType::VET_COLOUR;
			case VertexElementType::VET_COLOUR_ABGR:
				return VertexElementType::VET_COLOUR_ABGR;
			case VertexElementType::VET_COLOUR_ARGB:
				return VertexElementType::VET_COLOUR_ARGB;
			case VertexElementType::VET_SHORT1:
			case VertexElementType::VET_SHORT2:
			case VertexElementType::VET_SHORT3:
			case VertexElementType::VET_SHORT4:
				return VertexElementType::VET_SHORT1;
			case VertexElementType::VET_UBYTE4:
				return VertexElementType::VET_UBYTE4;
		};
        // To keep compiler happy
        return VertexElementType::VET_FLOAT1;
	}
}