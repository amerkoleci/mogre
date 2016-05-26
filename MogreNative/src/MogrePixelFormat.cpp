#include "stdafx.h"
#include "MogrePixelFormat.h"
#include "MogrePixelBox.h"
#include "Marshalling.h"

using namespace Mogre;

PixelUtil::PixelUtil()
{
	_createdByCLR = true;
	_native = new Ogre::PixelUtil();
}

size_t PixelUtil::GetNumElemBytes(Mogre::PixelFormat format)
{
	return Ogre::PixelUtil::getNumElemBytes((Ogre::PixelFormat)format);
}

size_t PixelUtil::GetNumElemBits(Mogre::PixelFormat format)
{
	return Ogre::PixelUtil::getNumElemBits((Ogre::PixelFormat)format);
}

size_t PixelUtil::GetMemorySize(size_t width, size_t height, size_t depth, Mogre::PixelFormat format)
{
	return Ogre::PixelUtil::getMemorySize(width, height, depth, (Ogre::PixelFormat)format);
}

unsigned int PixelUtil::GetFlags(Mogre::PixelFormat format)
{
	return Ogre::PixelUtil::getFlags((Ogre::PixelFormat)format);
}

bool PixelUtil::HasAlpha(Mogre::PixelFormat format)
{
	return Ogre::PixelUtil::hasAlpha((Ogre::PixelFormat)format);
}

bool PixelUtil::IsFloatingPoint(Mogre::PixelFormat format)
{
	return Ogre::PixelUtil::isFloatingPoint((Ogre::PixelFormat)format);
}

bool PixelUtil::IsCompressed(Mogre::PixelFormat format)
{
	return Ogre::PixelUtil::isCompressed((Ogre::PixelFormat)format);
}

bool PixelUtil::IsDepth(Mogre::PixelFormat format)
{
	return Ogre::PixelUtil::isDepth((Ogre::PixelFormat)format);
}

bool PixelUtil::IsNativeEndian(Mogre::PixelFormat format)
{
	return Ogre::PixelUtil::isNativeEndian((Ogre::PixelFormat)format);
}

bool PixelUtil::IsLuminance(Mogre::PixelFormat format)
{
	return Ogre::PixelUtil::isLuminance((Ogre::PixelFormat)format);
}

bool PixelUtil::IsValidExtent(size_t width, size_t height, size_t depth, Mogre::PixelFormat format)
{
	return Ogre::PixelUtil::isValidExtent(width, height, depth, (Ogre::PixelFormat)format);
}


void PixelUtil::GetBitDepths(Mogre::PixelFormat format, array<int>^% rgba)
{
	rgba = gcnew array<int>(4);
	pin_ptr<int> ptr = &rgba[0];
	Ogre::PixelUtil::getBitDepths((Ogre::PixelFormat)format, ptr);
}


void PixelUtil::GetBitMasks(Mogre::PixelFormat format, array<Ogre::uint64>^% rgba)
{
	rgba = gcnew array<Ogre::uint64>(4);
	pin_ptr<Ogre::uint64> ptr = &rgba[0];
	Ogre::PixelUtil::getBitMasks((Ogre::PixelFormat)format, ptr);
}

String^ PixelUtil::GetFormatName(Mogre::PixelFormat srcformat)
{
	return TO_CLR_STRING(Ogre::PixelUtil::getFormatName((Ogre::PixelFormat)srcformat));
}

bool PixelUtil::IsAccessible(Mogre::PixelFormat srcformat)
{
	return Ogre::PixelUtil::isAccessible((Ogre::PixelFormat)srcformat);
}

Mogre::PixelComponentType PixelUtil::GetComponentType(Mogre::PixelFormat fmt)
{
	return (Mogre::PixelComponentType)Ogre::PixelUtil::getComponentType((Ogre::PixelFormat)fmt);
}

size_t PixelUtil::GetComponentCount(Mogre::PixelFormat fmt)
{
	return Ogre::PixelUtil::getComponentCount((Ogre::PixelFormat)fmt);
}

Mogre::PixelFormat PixelUtil::GetFormatFromName(String^ name, bool accessibleOnly, bool caseSensitive)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return (Mogre::PixelFormat)Ogre::PixelUtil::getFormatFromName(o_name, accessibleOnly, caseSensitive);
}

Mogre::PixelFormat PixelUtil::GetFormatFromName(String^ name, bool accessibleOnly)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return (Mogre::PixelFormat)Ogre::PixelUtil::getFormatFromName(o_name, accessibleOnly);
}

Mogre::PixelFormat PixelUtil::GetFormatFromName(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return (Mogre::PixelFormat)Ogre::PixelUtil::getFormatFromName(o_name);
}

String^ PixelUtil::GetBNFExpressionOfPixelFormats(bool accessibleOnly)
{
	return TO_CLR_STRING(Ogre::PixelUtil::getBNFExpressionOfPixelFormats(accessibleOnly));
}
String^ PixelUtil::GetBNFExpressionOfPixelFormats()
{
	return TO_CLR_STRING(Ogre::PixelUtil::getBNFExpressionOfPixelFormats());
}

Mogre::PixelFormat PixelUtil::GetFormatForBitDepths(Mogre::PixelFormat fmt, Ogre::ushort integerBits, Ogre::ushort floatBits)
{
	return (Mogre::PixelFormat)Ogre::PixelUtil::getFormatForBitDepths((Ogre::PixelFormat)fmt, integerBits, floatBits);
}

void PixelUtil::PackColour(Color4 colour, Mogre::PixelFormat pf, void* dest)
{
	Ogre::PixelUtil::packColour(FromColor4(colour), (Ogre::PixelFormat)pf, dest);
}

void PixelUtil::PackColour(Ogre::uint8 r, Ogre::uint8 g, Ogre::uint8 b, Ogre::uint8 a, Mogre::PixelFormat pf, void* dest)
{
	Ogre::PixelUtil::packColour(r, g, b, a, (Ogre::PixelFormat)pf, dest);
}

void PixelUtil::PackColour(float r, float g, float b, float a, Mogre::PixelFormat pf, void* dest)
{
	Ogre::PixelUtil::packColour(r, g, b, a, (Ogre::PixelFormat)pf, dest);
}

void PixelUtil::UnpackColour(Color4* colour, Mogre::PixelFormat pf, const void* src)
{
	Ogre::ColourValue* o_colour = reinterpret_cast<Ogre::ColourValue*>(colour);

	Ogre::PixelUtil::unpackColour(o_colour, (Ogre::PixelFormat)pf, src);
}

void PixelUtil::UnpackColour([Out] Ogre::uint8% r, [Out] Ogre::uint8% g, [Out] Ogre::uint8% b, [Out] Ogre::uint8% a, Mogre::PixelFormat pf, const void* src)
{
	pin_ptr<Ogre::uint8> p_r = &r;
	pin_ptr<Ogre::uint8> p_g = &g;
	pin_ptr<Ogre::uint8> p_b = &b;
	pin_ptr<Ogre::uint8> p_a = &a;

	Ogre::PixelUtil::unpackColour(p_r, p_g, p_b, p_a, (Ogre::PixelFormat)pf, src);
}

void PixelUtil::UnpackColour([Out] float% r, [Out] float% g, [Out] float% b, [Out] float% a, Mogre::PixelFormat pf, const void* src)
{
	pin_ptr<float> p_r = &r;
	pin_ptr<float> p_g = &g;
	pin_ptr<float> p_b = &b;
	pin_ptr<float> p_a = &a;

	Ogre::PixelUtil::unpackColour(p_r, p_g, p_b, p_a, (Ogre::PixelFormat)pf, src);
}

void PixelUtil::BulkPixelConversion(void* src, Mogre::PixelFormat srcFormat, void* dest, Mogre::PixelFormat dstFormat, unsigned int count)
{
	Ogre::PixelUtil::bulkPixelConversion(src, (Ogre::PixelFormat)srcFormat, dest, (Ogre::PixelFormat)dstFormat, count);
}

void PixelUtil::BulkPixelConversion(Mogre::PixelBox src, Mogre::PixelBox dst)
{
	Ogre::PixelUtil::bulkPixelConversion(src, dst);
}