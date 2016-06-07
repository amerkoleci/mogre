#include "stdafx.h"
#include "MogreTextureManager.h"
#include "MogreHardwareBuffer.h"

using namespace Mogre;

// Image
Image::Image()
{
	_createdByCLR = true;
	_native = new Ogre::Image();
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

Image::Image(Mogre::Image^ img)
{
	_createdByCLR = true;
	_native = new Ogre::Image(*img->UnmanagedPointer);
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

Image::~Image()
{
	this->!Image();
}

Image::!Image()
{
	OnDisposing(this, nullptr);

	if (IsDisposed)
		return;

	if (_createdByCLR &&_native)
	{
		delete _native;
		_native = 0;
	}

	OnDisposed(this, nullptr);
}

bool Image::IsDisposed::get()
{
	return (_native == nullptr);
}

Mogre::uchar Image::BPP::get()
{
	return static_cast<const Ogre::Image*>(_native)->getBPP();
}

Mogre::uchar* Image::Data::get()
{
	return static_cast<Ogre::Image*>(_native)->getData();
}

size_t Image::Depth::get()
{
	return static_cast<const Ogre::Image*>(_native)->getDepth();
}

Mogre::PixelFormat Image::Format::get()
{
	return (Mogre::PixelFormat)static_cast<const Ogre::Image*>(_native)->getFormat();
}

bool Image::HasAlpha::get()
{
	return static_cast<const Ogre::Image*>(_native)->getHasAlpha();
}

size_t Image::Height::get()
{
	return static_cast<const Ogre::Image*>(_native)->getHeight();
}

size_t Image::NumFaces::get()
{
	return static_cast<const Ogre::Image*>(_native)->getNumFaces();
}

size_t Image::NumMipmaps::get()
{
	return static_cast<const Ogre::Image*>(_native)->getNumMipmaps();
}

size_t Image::RowSpan::get()
{
	return static_cast<const Ogre::Image*>(_native)->getRowSpan();
}

size_t Image::Size::get()
{
	return static_cast<const Ogre::Image*>(_native)->getSize();
}

size_t Image::Width::get()
{
	return static_cast<const Ogre::Image*>(_native)->getWidth();
}


Mogre::Image^ Image::FlipAroundY()
{
	Ogre::Image* ptr = &const_cast<Ogre::Image&>(_native->flipAroundY());
	return ObjectTable::GetOrCreateObject<Mogre::Image^>((intptr_t)ptr);
}

Mogre::Image^ Image::FlipAroundX()
{
	Ogre::Image* ptr = &const_cast<Ogre::Image&>(_native->flipAroundX());
	return ObjectTable::GetOrCreateObject<Mogre::Image^>((intptr_t)ptr);
}

Mogre::Image^ Image::LoadDynamicImage(Mogre::uchar* pData, size_t uWidth, size_t uHeight, size_t depth, Mogre::PixelFormat eFormat, bool autoDelete, size_t numFaces, size_t numMipMaps)
{
	Ogre::Image* ptr = &const_cast<Ogre::Image&>(_native->loadDynamicImage(pData, uWidth, uHeight, depth, (Ogre::PixelFormat)eFormat, autoDelete, numFaces, numMipMaps));
	return ObjectTable::GetOrCreateObject<Mogre::Image^>((intptr_t)ptr);
}

Mogre::Image^ Image::LoadDynamicImage(Mogre::uchar* pData, size_t uWidth, size_t uHeight, size_t depth, Mogre::PixelFormat eFormat, bool autoDelete, size_t numFaces)
{
	Ogre::Image* ptr = &const_cast<Ogre::Image&>(_native->loadDynamicImage(pData, uWidth, uHeight, depth, (Ogre::PixelFormat)eFormat, autoDelete, numFaces));
	return ObjectTable::GetOrCreateObject<Mogre::Image^>((intptr_t)ptr);
}

Mogre::Image^ Image::LoadDynamicImage(Mogre::uchar* pData, size_t uWidth, size_t uHeight, size_t depth, Mogre::PixelFormat eFormat, bool autoDelete)
{
	Ogre::Image* ptr = &const_cast<Ogre::Image&>(_native->loadDynamicImage(pData, uWidth, uHeight, depth, (Ogre::PixelFormat)eFormat, autoDelete));
	return ObjectTable::GetOrCreateObject<Mogre::Image^>((intptr_t)ptr);
}

Mogre::Image^ Image::LoadDynamicImage(Mogre::uchar* pData, size_t uWidth, size_t uHeight, size_t depth, Mogre::PixelFormat eFormat)
{
	Ogre::Image* ptr = &const_cast<Ogre::Image&>(_native->loadDynamicImage(pData, uWidth, uHeight, depth, (Ogre::PixelFormat)eFormat));
	return ObjectTable::GetOrCreateObject<Mogre::Image^>((intptr_t)ptr);
}

Mogre::Image^ Image::LoadDynamicImage(Mogre::uchar* pData, size_t uWidth, size_t uHeight, Mogre::PixelFormat eFormat)
{
	Ogre::Image* ptr = &const_cast<Ogre::Image&>(_native->loadDynamicImage(pData, uWidth, uHeight, (Ogre::PixelFormat)eFormat));
	return ObjectTable::GetOrCreateObject<Mogre::Image^>((intptr_t)ptr);
}

Mogre::Image^ Image::LoadRawData(Mogre::DataStreamPtr^ stream, size_t uWidth, size_t uHeight, size_t uDepth, Mogre::PixelFormat eFormat, size_t numFaces, size_t numMipMaps)
{
	Ogre::Image* ptr = &const_cast<Ogre::Image&>(_native->loadRawData((Ogre::DataStreamPtr&)stream, uWidth, uHeight, uDepth, (Ogre::PixelFormat)eFormat, numFaces, numMipMaps));
	return ObjectTable::GetOrCreateObject<Mogre::Image^>((intptr_t)ptr);
}

Mogre::Image^ Image::LoadRawData(Mogre::DataStreamPtr^ stream, size_t uWidth, size_t uHeight, size_t uDepth, Mogre::PixelFormat eFormat, size_t numFaces)
{
	Ogre::Image* ptr = &const_cast<Ogre::Image&>(_native->loadRawData((Ogre::DataStreamPtr&)stream, uWidth, uHeight, uDepth, (Ogre::PixelFormat)eFormat, numFaces));
	return ObjectTable::GetOrCreateObject<Mogre::Image^>((intptr_t)ptr);
}

Mogre::Image^ Image::LoadRawData(Mogre::DataStreamPtr^ stream, size_t uWidth, size_t uHeight, size_t uDepth, Mogre::PixelFormat eFormat)
{
	Ogre::Image* ptr = &const_cast<Ogre::Image&>(_native->loadRawData((Ogre::DataStreamPtr&)stream, uWidth, uHeight, uDepth, (Ogre::PixelFormat)eFormat));
	return ObjectTable::GetOrCreateObject<Mogre::Image^>((intptr_t)ptr);
}

Mogre::Image^ Image::LoadRawData(Mogre::DataStreamPtr^ stream, size_t uWidth, size_t uHeight, Mogre::PixelFormat eFormat)
{
	Ogre::Image* ptr = &const_cast<Ogre::Image&>(_native->loadRawData((Ogre::DataStreamPtr&)stream, uWidth, uHeight, (Ogre::PixelFormat)eFormat));
	return ObjectTable::GetOrCreateObject<Mogre::Image^>((intptr_t)ptr);
}

Mogre::Image^ Image::Load(String^ strFileName, String^ groupName)
{
	DECLARE_NATIVE_STRING(o_strFileName, strFileName);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	Ogre::Image* ptr = &const_cast<Ogre::Image&>(_native->load(o_strFileName, o_groupName));
	return ObjectTable::GetOrCreateObject<Mogre::Image^>((intptr_t)ptr);
}

Mogre::Image^ Image::Load(Mogre::DataStreamPtr^ stream, String^ type)
{
	DECLARE_NATIVE_STRING(o_type, type);

	Ogre::Image* ptr = &const_cast<Ogre::Image&>(_native->load((Ogre::DataStreamPtr&)stream, o_type));
	return ObjectTable::GetOrCreateObject<Mogre::Image^>((intptr_t)ptr);
}

void Image::Save(String^ filename)
{
	DECLARE_NATIVE_STRING(o_filename, filename);

	static_cast<Ogre::Image*>(_native)->save(o_filename);
}

bool Image::HasFlag(Mogre::ImageFlags imgFlag)
{
	return static_cast<const Ogre::Image*>(_native)->hasFlag((Ogre::ImageFlags)imgFlag);
}

Mogre::ColourValue Image::GetColourAt(int x, int y, int z)
{
	return ToColor4(static_cast<Ogre::Image*>(_native)->getColourAt(x, y, z));
}

Mogre::PixelBox Image::GetPixelBox(size_t face, size_t mipmap)
{
	return static_cast<const Ogre::Image*>(_native)->getPixelBox(face, mipmap);
}

Mogre::PixelBox Image::GetPixelBox(size_t face)
{
	return static_cast<const Ogre::Image*>(_native)->getPixelBox(face);
}

Mogre::PixelBox Image::GetPixelBox()
{
	return static_cast<const Ogre::Image*>(_native)->getPixelBox();
}

void Image::Resize(Mogre::ushort width, Mogre::ushort height, Mogre::Image::Filter filter)
{
	static_cast<Ogre::Image*>(_native)->resize(width, height, (Ogre::Image::Filter)filter);
}

void Image::Resize(Mogre::ushort width, Mogre::ushort height)
{
	static_cast<Ogre::Image*>(_native)->resize(width, height);
}

void Image::ApplyGamma([Out] Mogre::uchar% buffer, Mogre::Real gamma, size_t size, Mogre::uchar bpp)
{
	pin_ptr<Mogre::uchar> p_buffer = &buffer;

	Ogre::Image::applyGamma(p_buffer, gamma, size, bpp);
}

void Image::Scale(Mogre::PixelBox src, Mogre::PixelBox dst, Mogre::Image::Filter filter)
{
	Ogre::Image::scale(src, dst, (Ogre::Image::Filter)filter);
}

void Image::Scale(Mogre::PixelBox src, Mogre::PixelBox dst)
{
	Ogre::Image::scale(src, dst);
}

size_t Image::CalculateSize(size_t mipmaps, size_t faces, size_t width, size_t height, size_t depth, Mogre::PixelFormat format)
{
	return Ogre::Image::calculateSize(mipmaps, faces, width, height, depth, (Ogre::PixelFormat)format);
}

// Texture

size_t Texture::Depth::get()
{
	return static_cast<const Ogre::Texture*>(_native)->getDepth();
}

void Texture::Depth::set(size_t d)
{
	static_cast<Ogre::Texture*>(_native)->setDepth(d);
}

Mogre::ushort Texture::DesiredFloatBitDepth::get()
{
	return static_cast<const Ogre::Texture*>(_native)->getDesiredFloatBitDepth();
}
void Texture::DesiredFloatBitDepth::set(Mogre::ushort bits)
{
	static_cast<Ogre::Texture*>(_native)->setDesiredFloatBitDepth(bits);
}

Mogre::PixelFormat Texture::DesiredFormat::get()
{
	return (Mogre::PixelFormat)static_cast<const Ogre::Texture*>(_native)->getDesiredFormat();
}

Mogre::ushort Texture::DesiredIntegerBitDepth::get()
{
	return static_cast<const Ogre::Texture*>(_native)->getDesiredIntegerBitDepth();
}
void Texture::DesiredIntegerBitDepth::set(Mogre::ushort bits)
{
	static_cast<Ogre::Texture*>(_native)->setDesiredIntegerBitDepth(bits);
}

Mogre::PixelFormat Texture::Format::get()
{
	return (Mogre::PixelFormat)static_cast<const Ogre::Texture*>(_native)->getFormat();
}
void Texture::Format::set(Mogre::PixelFormat pf)
{
	static_cast<Ogre::Texture*>(_native)->setFormat((Ogre::PixelFormat)pf);
}

float Texture::Gamma::get()
{
	return static_cast<const Ogre::Texture*>(_native)->getGamma();
}
void Texture::Gamma::set(float g)
{
	static_cast<Ogre::Texture*>(_native)->setGamma(g);
}

bool Texture::HasAlpha::get()
{
	return static_cast<const Ogre::Texture*>(_native)->hasAlpha();
}

size_t Texture::Height::get()
{
	return static_cast<const Ogre::Texture*>(_native)->getHeight();
}
void Texture::Height::set(size_t h)
{
	static_cast<Ogre::Texture*>(_native)->setHeight(h);
}

bool Texture::MipmapsHardwareGenerated::get()
{
	return static_cast<const Ogre::Texture*>(_native)->getMipmapsHardwareGenerated();
}

size_t Texture::NumFaces::get()
{
	return static_cast<const Ogre::Texture*>(_native)->getNumFaces();
}

size_t Texture::NumMipmaps::get()
{
	return static_cast<const Ogre::Texture*>(_native)->getNumMipmaps();
}
void Texture::NumMipmaps::set(size_t num)
{
	static_cast<Ogre::Texture*>(_native)->setNumMipmaps(num);
}

size_t Texture::SrcDepth::get()
{
	return static_cast<const Ogre::Texture*>(_native)->getSrcDepth();
}

Mogre::PixelFormat Texture::SrcFormat::get()
{
	return (Mogre::PixelFormat)static_cast<const Ogre::Texture*>(_native)->getSrcFormat();
}

size_t Texture::SrcHeight::get()
{
	return static_cast<const Ogre::Texture*>(_native)->getSrcHeight();
}

size_t Texture::SrcWidth::get()
{
	return static_cast<const Ogre::Texture*>(_native)->getSrcWidth();
}

Mogre::TextureType Texture::TextureType::get()
{
	return (Mogre::TextureType)static_cast<const Ogre::Texture*>(_native)->getTextureType();
}
void Texture::TextureType::set(Mogre::TextureType ttype)
{
	static_cast<Ogre::Texture*>(_native)->setTextureType((Ogre::TextureType)ttype);
}

bool Texture::TreatLuminanceAsAlpha::get()
{
	return static_cast<const Ogre::Texture*>(_native)->getTreatLuminanceAsAlpha();
}
void Texture::TreatLuminanceAsAlpha::set(bool asAlpha)
{
	static_cast<Ogre::Texture*>(_native)->setTreatLuminanceAsAlpha(asAlpha);
}

int Texture::Usage::get()
{
	return static_cast<const Ogre::Texture*>(_native)->getUsage();
}
void Texture::Usage::set(int u)
{
	static_cast<Ogre::Texture*>(_native)->setUsage(u);
}

size_t Texture::Width::get()
{
	return static_cast<const Ogre::Texture*>(_native)->getWidth();
}
void Texture::Width::set(size_t w)
{
	static_cast<Ogre::Texture*>(_native)->setWidth(w);
}

void Texture::CreateInternalResources()
{
	static_cast<Ogre::Texture*>(_native)->createInternalResources();
}

void Texture::FreeInternalResources()
{
	static_cast<Ogre::Texture*>(_native)->freeInternalResources();
}

void Texture::CopyToTexture(Mogre::TexturePtr^ target)
{
	static_cast<Ogre::Texture*>(_native)->copyToTexture((Ogre::TexturePtr&)target);
}

void Texture::LoadImage(Mogre::Image^ img)
{
	auto imagePtr = GetPointerOrNull(img);
	static_cast<Ogre::Texture*>(_native)->loadImage(*imagePtr);
}

void Texture::LoadRawData(Mogre::DataStreamPtr^ stream, Mogre::ushort uWidth, Mogre::ushort uHeight, Mogre::PixelFormat eFormat)
{
	static_cast<Ogre::Texture*>(_native)->loadRawData((Ogre::DataStreamPtr&)stream, uWidth, uHeight, (Ogre::PixelFormat)eFormat);
}

void Texture::SetDesiredBitDepths(Mogre::ushort integerBits, Mogre::ushort floatBits)
{
	static_cast<Ogre::Texture*>(_native)->setDesiredBitDepths(integerBits, floatBits);
}

Mogre::HardwarePixelBufferSharedPtr^ Texture::GetBuffer(size_t face, size_t mipmap)
{
	return static_cast<Ogre::Texture*>(_native)->getBuffer(face, mipmap);
}

Mogre::HardwarePixelBufferSharedPtr^ Texture::GetBuffer(size_t face)
{
	return static_cast<Ogre::Texture*>(_native)->getBuffer(face);
}

Mogre::HardwarePixelBufferSharedPtr^ Texture::GetBuffer()
{
	return static_cast<Ogre::Texture*>(_native)->getBuffer();
}

// TextureManager

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

Mogre::TexturePtr^ TextureManager::Load(String^ name, String^ group, Mogre::TextureType texType, int numMipmaps, Mogre::Real gamma, bool isAlpha, Mogre::PixelFormat desiredFormat)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	return static_cast<Ogre::TextureManager*>(_native)->load(o_name, o_group, (Ogre::TextureType)texType, numMipmaps, gamma, isAlpha, (Ogre::PixelFormat)desiredFormat);
}

Mogre::TexturePtr^ TextureManager::Load(String^ name, String^ group, Mogre::TextureType texType, int numMipmaps, Mogre::Real gamma, bool isAlpha)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	return static_cast<Ogre::TextureManager*>(_native)->load(o_name, o_group, (Ogre::TextureType)texType, numMipmaps, gamma, isAlpha);
}

Mogre::TexturePtr^ TextureManager::Load(String^ name, String^ group, Mogre::TextureType texType, int numMipmaps, Mogre::Real gamma)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	return static_cast<Ogre::TextureManager*>(_native)->load(o_name, o_group, (Ogre::TextureType)texType, numMipmaps, gamma);
}

Mogre::TexturePtr^ TextureManager::Load(String^ name, String^ group, Mogre::TextureType texType, int numMipmaps)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	return static_cast<Ogre::TextureManager*>(_native)->load(o_name, o_group, (Ogre::TextureType)texType, numMipmaps);
}

Mogre::TexturePtr^ TextureManager::Load(String^ name, String^ group, Mogre::TextureType texType)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	return static_cast<Ogre::TextureManager*>(_native)->load(o_name, o_group, (Ogre::TextureType)texType);
}

Mogre::TexturePtr^ TextureManager::Load(String^ name, String^ group)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	return static_cast<Ogre::TextureManager*>(_native)->load(o_name, o_group);
}

Mogre::TexturePtr^ TextureManager::LoadImage(String^ name, String^ group, Mogre::Image^ img, Mogre::TextureType texType, int iNumMipmaps, Mogre::Real gamma, bool isAlpha, Mogre::PixelFormat desiredFormat)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);
	auto imagePtr = GetPointerOrNull(img);

	return static_cast<Ogre::TextureManager*>(_native)->loadImage(o_name, o_group, imagePtr ? *imagePtr : Ogre::Image(), (Ogre::TextureType)texType, iNumMipmaps, gamma, isAlpha, (Ogre::PixelFormat)desiredFormat);
}

Mogre::TexturePtr^ TextureManager::LoadImage(String^ name, String^ group, Mogre::Image^ img, Mogre::TextureType texType, int iNumMipmaps, Mogre::Real gamma, bool isAlpha)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);
	auto imagePtr = GetPointerOrNull(img);

	return static_cast<Ogre::TextureManager*>(_native)->loadImage(o_name, o_group, imagePtr ? *imagePtr : Ogre::Image(), (Ogre::TextureType)texType, iNumMipmaps, gamma, isAlpha);
}

Mogre::TexturePtr^ TextureManager::LoadImage(String^ name, String^ group, Mogre::Image^ img, Mogre::TextureType texType, int iNumMipmaps, Mogre::Real gamma)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);
	auto imagePtr = GetPointerOrNull(img);

	return static_cast<Ogre::TextureManager*>(_native)->loadImage(o_name, o_group, imagePtr ? *imagePtr : Ogre::Image(), (Ogre::TextureType)texType, iNumMipmaps, gamma);
}

Mogre::TexturePtr^ TextureManager::LoadImage(String^ name, String^ group, Mogre::Image^ img, Mogre::TextureType texType, int iNumMipmaps)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);
	auto imagePtr = GetPointerOrNull(img);

	return static_cast<Ogre::TextureManager*>(_native)->loadImage(o_name, o_group, imagePtr ? *imagePtr : Ogre::Image(), (Ogre::TextureType)texType, iNumMipmaps);
}

Mogre::TexturePtr^ TextureManager::LoadImage(String^ name, String^ group, Mogre::Image^ img, Mogre::TextureType texType)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);
	auto imagePtr = GetPointerOrNull(img);

	return static_cast<Ogre::TextureManager*>(_native)->loadImage(o_name, o_group, imagePtr ? *imagePtr : Ogre::Image(), (Ogre::TextureType)texType);
}

Mogre::TexturePtr^ TextureManager::LoadImage(String^ name, String^ group, Mogre::Image^ img)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);
	auto imagePtr = GetPointerOrNull(img);

	return static_cast<Ogre::TextureManager*>(_native)->loadImage(o_name, o_group, imagePtr ? *imagePtr : Ogre::Image());
}

Mogre::TexturePtr^ TextureManager::LoadRawData(String^ name, String^ group, Mogre::DataStreamPtr^ stream, Mogre::ushort uWidth, Mogre::ushort uHeight, Mogre::PixelFormat format, Mogre::TextureType texType, int iNumMipmaps, Mogre::Real gamma)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	return static_cast<Ogre::TextureManager*>(_native)->loadRawData(o_name, o_group, (Ogre::DataStreamPtr&)stream, uWidth, uHeight, (Ogre::PixelFormat)format, (Ogre::TextureType)texType, iNumMipmaps, gamma);
}

Mogre::TexturePtr^ TextureManager::LoadRawData(String^ name, String^ group, Mogre::DataStreamPtr^ stream, Mogre::ushort uWidth, Mogre::ushort uHeight, Mogre::PixelFormat format, Mogre::TextureType texType, int iNumMipmaps)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	return static_cast<Ogre::TextureManager*>(_native)->loadRawData(o_name, o_group, (Ogre::DataStreamPtr&)stream, uWidth, uHeight, (Ogre::PixelFormat)format, (Ogre::TextureType)texType, iNumMipmaps);
}

Mogre::TexturePtr^ TextureManager::LoadRawData(String^ name, String^ group, Mogre::DataStreamPtr^ stream, Mogre::ushort uWidth, Mogre::ushort uHeight, Mogre::PixelFormat format, Mogre::TextureType texType)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	return static_cast<Ogre::TextureManager*>(_native)->loadRawData(o_name, o_group, (Ogre::DataStreamPtr&)stream, uWidth, uHeight, (Ogre::PixelFormat)format, (Ogre::TextureType)texType);
}

Mogre::TexturePtr^ TextureManager::LoadRawData(String^ name, String^ group, Mogre::DataStreamPtr^ stream, Mogre::ushort uWidth, Mogre::ushort uHeight, Mogre::PixelFormat format)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	return static_cast<Ogre::TextureManager*>(_native)->loadRawData(o_name, o_group, (Ogre::DataStreamPtr&)stream, uWidth, uHeight, (Ogre::PixelFormat)format);
}

//Mogre::TexturePtr^ TextureManager::CreateManual(String^ name, String^ group, Mogre::TextureType texType, Mogre::uint width, Mogre::uint height, Mogre::uint depth, int num_mips, Mogre::PixelFormat format, int usage, Mogre::IManualResourceLoader^ loader)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_group, group);
//
//	return static_cast<Ogre::TextureManager*>(_native)->createManual(o_name, o_group, (Ogre::TextureType)texType, width, height, depth, num_mips, (Ogre::PixelFormat)format, usage, loader);
//}

Mogre::TexturePtr^ TextureManager::CreateManual(String^ name, String^ group, Mogre::TextureType texType, Mogre::uint width, Mogre::uint height, Mogre::uint depth, int num_mips, Mogre::PixelFormat format, int usage)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	return static_cast<Ogre::TextureManager*>(_native)->createManual(o_name, o_group, (Ogre::TextureType)texType, width, height, depth, num_mips, (Ogre::PixelFormat)format, usage);
}

Mogre::TexturePtr^ TextureManager::CreateManual(String^ name, String^ group, Mogre::TextureType texType, Mogre::uint width, Mogre::uint height, Mogre::uint depth, int num_mips, Mogre::PixelFormat format)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	return static_cast<Ogre::TextureManager*>(_native)->createManual(o_name, o_group, (Ogre::TextureType)texType, width, height, depth, num_mips, (Ogre::PixelFormat)format);
}

//Mogre::TexturePtr^ TextureManager::CreateManual(String^ name, String^ group, Mogre::TextureType texType, Mogre::uint width, Mogre::uint height, int num_mips, Mogre::PixelFormat format, int usage, Mogre::IManualResourceLoader^ loader)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_group, group);
//
//	return static_cast<Ogre::TextureManager*>(_native)->createManual(o_name, o_group, (Ogre::TextureType)texType, width, height, num_mips, (Ogre::PixelFormat)format, usage, loader);
//}

Mogre::TexturePtr^ TextureManager::CreateManual(String^ name, String^ group, Mogre::TextureType texType, Mogre::uint width, Mogre::uint height, int num_mips, Mogre::PixelFormat format, int usage)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	Ogre::TexturePtr nativeTexture = static_cast<Ogre::TextureManager*>(_native)->createManual(
		o_name,
		o_group,
		(Ogre::TextureType)texType,
		width,
		height,
		num_mips,
		(Ogre::PixelFormat)format,
		usage);

	return nativeTexture;
}

Mogre::TexturePtr^ TextureManager::CreateManual(String^ name, String^ group, Mogre::TextureType texType, Mogre::uint width, Mogre::uint height, int num_mips, Mogre::PixelFormat format)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	return static_cast<Ogre::TextureManager*>(_native)->createManual(o_name, o_group, (Ogre::TextureType)texType, width, height, num_mips, (Ogre::PixelFormat)format);
}

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