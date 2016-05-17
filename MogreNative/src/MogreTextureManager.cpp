#include "stdafx.h"
#include "MogreTextureManager.h"

using namespace Mogre;

size_t TextureManager::DefaultNumMipmaps::get()
{
	return static_cast<Ogre::TextureManager*>(_native)->getDefaultNumMipmaps();
}

void TextureManager::DefaultNumMipmaps::set(size_t num)
{
	static_cast<Ogre::TextureManager*>(_native)->setDefaultNumMipmaps(num);
}

Ogre::ushort TextureManager::PreferredFloatBitDepth::get()
{
	return static_cast<const Ogre::TextureManager*>(_native)->getPreferredFloatBitDepth();
}

Ogre::ushort TextureManager::PreferredIntegerBitDepth::get()
{
	return static_cast<const Ogre::TextureManager*>(_native)->getPreferredIntegerBitDepth();
}

//Mogre::TexturePtr^ TextureManager::Load(String^ name, String^ group, Mogre::TextureType texType, int numMipmaps, Mogre::Real gamma, bool isAlpha, Mogre::PixelFormat desiredFormat)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_group, group);
//
//	return static_cast<Ogre::TextureManager*>(_native)->load(o_name, o_group, (Ogre::TextureType)texType, numMipmaps, gamma, isAlpha, (Ogre::PixelFormat)desiredFormat);
//}
//
//Mogre::TexturePtr^ TextureManager::Load(String^ name, String^ group, Mogre::TextureType texType, int numMipmaps, Mogre::Real gamma, bool isAlpha)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_group, group);
//
//	return static_cast<Ogre::TextureManager*>(_native)->load(o_name, o_group, (Ogre::TextureType)texType, numMipmaps, gamma, isAlpha);
//}
//
//Mogre::TexturePtr^ TextureManager::Load(String^ name, String^ group, Mogre::TextureType texType, int numMipmaps, Mogre::Real gamma)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_group, group);
//
//	return static_cast<Ogre::TextureManager*>(_native)->load(o_name, o_group, (Ogre::TextureType)texType, numMipmaps, gamma);
//}
//Mogre::TexturePtr^ TextureManager::Load(String^ name, String^ group, Mogre::TextureType texType, int numMipmaps)
//{
//	DECLARE_NATIVE_STRING(o_name, name)
//	DECLARE_NATIVE_STRING(o_group, group)
//
//	return static_cast<Ogre::TextureManager*>(_native)->load(o_name, o_group, (Ogre::TextureType)texType, numMipmaps);
//}
//
//Mogre::TexturePtr^ TextureManager::Load(String^ name, String^ group, Mogre::TextureType texType)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_group, group);
//
//	return static_cast<Ogre::TextureManager*>(_native)->load(o_name, o_group, (Ogre::TextureType)texType);
//}
//
//Mogre::TexturePtr^ TextureManager::Load(String^ name, String^ group)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_group, group);
//
//	return static_cast<Ogre::TextureManager*>(_native)->load(o_name, o_group);
//}
//
//Mogre::TexturePtr^ TextureManager::LoadImage(String^ name, String^ group, Mogre::Image^ img, Mogre::TextureType texType, int iNumMipmaps, Mogre::Real gamma, bool isAlpha, Mogre::PixelFormat desiredFormat)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_group, group);
//
//	return static_cast<Ogre::TextureManager*>(_native)->loadImage(o_name, o_group, img, (Ogre::TextureType)texType, iNumMipmaps, gamma, isAlpha, (Ogre::PixelFormat)desiredFormat);
//}
//Mogre::TexturePtr^ TextureManager::LoadImage(String^ name, String^ group, Mogre::Image^ img, Mogre::TextureType texType, int iNumMipmaps, Mogre::Real gamma, bool isAlpha)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_group, group);
//
//	return static_cast<Ogre::TextureManager*>(_native)->loadImage(o_name, o_group, img, (Ogre::TextureType)texType, iNumMipmaps, gamma, isAlpha);
//}
//Mogre::TexturePtr^ TextureManager::LoadImage(String^ name, String^ group, Mogre::Image^ img, Mogre::TextureType texType, int iNumMipmaps, Mogre::Real gamma)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_group, group);
//
//	return static_cast<Ogre::TextureManager*>(_native)->loadImage(o_name, o_group, img, (Ogre::TextureType)texType, iNumMipmaps, gamma);
//}
//Mogre::TexturePtr^ TextureManager::LoadImage(String^ name, String^ group, Mogre::Image^ img, Mogre::TextureType texType, int iNumMipmaps)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_group, group);
//
//	return static_cast<Ogre::TextureManager*>(_native)->loadImage(o_name, o_group, img, (Ogre::TextureType)texType, iNumMipmaps);
//}
//Mogre::TexturePtr^ TextureManager::LoadImage(String^ name, String^ group, Mogre::Image^ img, Mogre::TextureType texType)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_group, group);
//
//	return static_cast<Ogre::TextureManager*>(_native)->loadImage(o_name, o_group, img, (Ogre::TextureType)texType);
//}
//Mogre::TexturePtr^ TextureManager::LoadImage(String^ name, String^ group, Mogre::Image^ img)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_group, group);
//
//	return static_cast<Ogre::TextureManager*>(_native)->loadImage(o_name, o_group, img);
//}
//
//Mogre::TexturePtr^ TextureManager::LoadRawData(String^ name, String^ group, Mogre::DataStreamPtr^ stream, Mogre::ushort uWidth, Mogre::ushort uHeight, Mogre::PixelFormat format, Mogre::TextureType texType, int iNumMipmaps, Mogre::Real gamma)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_group, group);
//
//	return static_cast<Ogre::TextureManager*>(_native)->loadRawData(o_name, o_group, (Ogre::DataStreamPtr&)stream, uWidth, uHeight, (Ogre::PixelFormat)format, (Ogre::TextureType)texType, iNumMipmaps, gamma);
//}
//
//Mogre::TexturePtr^ TextureManager::LoadRawData(String^ name, String^ group, Mogre::DataStreamPtr^ stream, Mogre::ushort uWidth, Mogre::ushort uHeight, Mogre::PixelFormat format, Mogre::TextureType texType, int iNumMipmaps)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_group, group);
//
//	return static_cast<Ogre::TextureManager*>(_native)->loadRawData(o_name, o_group, (Ogre::DataStreamPtr&)stream, uWidth, uHeight, (Ogre::PixelFormat)format, (Ogre::TextureType)texType, iNumMipmaps);
//}
//
//Mogre::TexturePtr^ TextureManager::LoadRawData(String^ name, String^ group, Mogre::DataStreamPtr^ stream, Mogre::ushort uWidth, Mogre::ushort uHeight, Mogre::PixelFormat format, Mogre::TextureType texType)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_group, group);
//
//	return static_cast<Ogre::TextureManager*>(_native)->loadRawData(o_name, o_group, (Ogre::DataStreamPtr&)stream, uWidth, uHeight, (Ogre::PixelFormat)format, (Ogre::TextureType)texType);
//}
//
//Mogre::TexturePtr^ TextureManager::LoadRawData(String^ name, String^ group, Mogre::DataStreamPtr^ stream, Mogre::ushort uWidth, Mogre::ushort uHeight, Mogre::PixelFormat format)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_group, group);
//
//	return static_cast<Ogre::TextureManager*>(_native)->loadRawData(o_name, o_group, (Ogre::DataStreamPtr&)stream, uWidth, uHeight, (Ogre::PixelFormat)format);
//}
//
//Mogre::TexturePtr^ TextureManager::CreateManual(String^ name, String^ group, Mogre::TextureType texType, Mogre::uint width, Mogre::uint height, Mogre::uint depth, int num_mips, Mogre::PixelFormat format, int usage, Mogre::IManualResourceLoader^ loader)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_group, group);
//
//	return static_cast<Ogre::TextureManager*>(_native)->createManual(o_name, o_group, (Ogre::TextureType)texType, width, height, depth, num_mips, (Ogre::PixelFormat)format, usage, loader);
//}
//
//Mogre::TexturePtr^ TextureManager::CreateManual(String^ name, String^ group, Mogre::TextureType texType, Mogre::uint width, Mogre::uint height, Mogre::uint depth, int num_mips, Mogre::PixelFormat format, int usage)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_group, group);
//
//	return static_cast<Ogre::TextureManager*>(_native)->createManual(o_name, o_group, (Ogre::TextureType)texType, width, height, depth, num_mips, (Ogre::PixelFormat)format, usage);
//}
//
//Mogre::TexturePtr^ TextureManager::CreateManual(String^ name, String^ group, Mogre::TextureType texType, Mogre::uint width, Mogre::uint height, Mogre::uint depth, int num_mips, Mogre::PixelFormat format)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_group, group);
//
//	return static_cast<Ogre::TextureManager*>(_native)->createManual(o_name, o_group, (Ogre::TextureType)texType, width, height, depth, num_mips, (Ogre::PixelFormat)format);
//}
//
//Mogre::TexturePtr^ TextureManager::CreateManual(String^ name, String^ group, Mogre::TextureType texType, Mogre::uint width, Mogre::uint height, int num_mips, Mogre::PixelFormat format, int usage, Mogre::IManualResourceLoader^ loader)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_group, group);
//
//	return static_cast<Ogre::TextureManager*>(_native)->createManual(o_name, o_group, (Ogre::TextureType)texType, width, height, num_mips, (Ogre::PixelFormat)format, usage, loader);
//}
//Mogre::TexturePtr^ TextureManager::CreateManual(String^ name, String^ group, Mogre::TextureType texType, Mogre::uint width, Mogre::uint height, int num_mips, Mogre::PixelFormat format, int usage)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_group, group);
//
//	return static_cast<Ogre::TextureManager*>(_native)->createManual(o_name, o_group, (Ogre::TextureType)texType, width, height, num_mips, (Ogre::PixelFormat)format, usage);
//}
//Mogre::TexturePtr^ TextureManager::CreateManual(String^ name, String^ group, Mogre::TextureType texType, Mogre::uint width, Mogre::uint height, int num_mips, Mogre::PixelFormat format)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_group, group);
//
//	return static_cast<Ogre::TextureManager*>(_native)->createManual(o_name, o_group, (Ogre::TextureType)texType, width, height, num_mips, (Ogre::PixelFormat)format);
//}

void TextureManager::SetPreferredIntegerBitDepth(Ogre::ushort bits, bool reloadTextures)
{
	static_cast<Ogre::TextureManager*>(_native)->setPreferredIntegerBitDepth(bits, reloadTextures);
}
void TextureManager::SetPreferredIntegerBitDepth(Ogre::ushort bits)
{
	static_cast<Ogre::TextureManager*>(_native)->setPreferredIntegerBitDepth(bits);
}

void TextureManager::SetPreferredFloatBitDepth(Ogre::ushort bits, bool reloadTextures)
{
	static_cast<Ogre::TextureManager*>(_native)->setPreferredFloatBitDepth(bits, reloadTextures);
}
void TextureManager::SetPreferredFloatBitDepth(Ogre::ushort bits)
{
	static_cast<Ogre::TextureManager*>(_native)->setPreferredFloatBitDepth(bits);
}

void TextureManager::SetPreferredBitDepths(Ogre::ushort integerBits, Ogre::ushort floatBits, bool reloadTextures)
{
	static_cast<Ogre::TextureManager*>(_native)->setPreferredBitDepths(integerBits, floatBits, reloadTextures);
}
void TextureManager::SetPreferredBitDepths(Ogre::ushort integerBits, Ogre::ushort floatBits)
{
	static_cast<Ogre::TextureManager*>(_native)->setPreferredBitDepths(integerBits, floatBits);
}

bool TextureManager::IsFormatSupported(Mogre::TextureType ttype, Mogre::PixelFormat format, int usage)
{
	return static_cast<Ogre::TextureManager*>(_native)->isFormatSupported((Ogre::TextureType)ttype, (Ogre::PixelFormat)format, usage);
}

bool TextureManager::IsEquivalentFormatSupported(Mogre::TextureType ttype, Mogre::PixelFormat format, int usage)
{
	return static_cast<Ogre::TextureManager*>(_native)->isEquivalentFormatSupported((Ogre::TextureType)ttype, (Ogre::PixelFormat)format, usage);
}

Mogre::PixelFormat TextureManager::GetNativeFormat(Mogre::TextureType ttype, Mogre::PixelFormat format, int usage)
{
	return (Mogre::PixelFormat)static_cast<Ogre::TextureManager*>(_native)->getNativeFormat((Ogre::TextureType)ttype, (Ogre::PixelFormat)format, usage);
}

bool TextureManager::IsHardwareFilteringSupported(Mogre::TextureType ttype, Mogre::PixelFormat format, int usage, bool preciseFormatOnly)
{
	return static_cast<Ogre::TextureManager*>(_native)->isHardwareFilteringSupported((Ogre::TextureType)ttype, (Ogre::PixelFormat)format, usage, preciseFormatOnly);
}

bool TextureManager::IsHardwareFilteringSupported(Mogre::TextureType ttype, Mogre::PixelFormat format, int usage)
{
	return static_cast<Ogre::TextureManager*>(_native)->isHardwareFilteringSupported((Ogre::TextureType)ttype, (Ogre::PixelFormat)format, usage);
}