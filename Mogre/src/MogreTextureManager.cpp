#include "stdafx.h"
#include "MogreTextureManager.h"
#include "MogreHardwareBuffer.h"
#include "OgreResourceGroupManager.h"

using namespace Mogre;

// Image
Image::Image()
{
	_createdByCLR = true;
	_native = new Ogre::Image();
}

Image::Image(Mogre::Image^ img)
{
	_createdByCLR = true;
	_native = new Ogre::Image(*img->UnmanagedPointer);
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
	return _native->getBPP();
}

Mogre::uchar* Image::Data::get()
{
	return _native->getData();
}

size_t Image::Depth::get()
{
	return _native->getDepth();
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
	return gcnew Mogre::Image(ptr);
}

Mogre::Image^ Image::FlipAroundX()
{
	Ogre::Image* ptr = &const_cast<Ogre::Image&>(_native->flipAroundX());
	return gcnew Mogre::Image(ptr);
}

Mogre::Image^ Image::LoadDynamicImage(Mogre::uchar* pData, size_t uWidth, size_t uHeight, size_t depth, Mogre::PixelFormat eFormat, bool autoDelete, size_t numFaces, size_t numMipMaps)
{
	Ogre::Image* ptr = &const_cast<Ogre::Image&>(_native->loadDynamicImage(pData, uWidth, uHeight, depth, (Ogre::PixelFormat)eFormat, autoDelete, numFaces, numMipMaps));
	return gcnew Mogre::Image(ptr);
}

Mogre::Image^ Image::LoadDynamicImage(Mogre::uchar* pData, size_t uWidth, size_t uHeight, size_t depth, Mogre::PixelFormat eFormat, bool autoDelete, size_t numFaces)
{
	Ogre::Image* ptr = &const_cast<Ogre::Image&>(_native->loadDynamicImage(pData, uWidth, uHeight, depth, (Ogre::PixelFormat)eFormat, autoDelete, numFaces));
	return gcnew Mogre::Image(ptr);
}

Mogre::Image^ Image::LoadDynamicImage(Mogre::uchar* pData, size_t uWidth, size_t uHeight, size_t depth, Mogre::PixelFormat eFormat, bool autoDelete)
{
	Ogre::Image* ptr = &const_cast<Ogre::Image&>(_native->loadDynamicImage(pData, uWidth, uHeight, depth, (Ogre::PixelFormat)eFormat, autoDelete));
	return gcnew Mogre::Image(ptr);
}

Mogre::Image^ Image::LoadDynamicImage(Mogre::uchar* pData, size_t uWidth, size_t uHeight, size_t depth, Mogre::PixelFormat eFormat)
{
	Ogre::Image* ptr = &const_cast<Ogre::Image&>(_native->loadDynamicImage(pData, uWidth, uHeight, depth, (Ogre::PixelFormat)eFormat));
	return gcnew Mogre::Image(ptr);
}

Mogre::Image^ Image::LoadDynamicImage(Mogre::uchar* pData, size_t uWidth, size_t uHeight, Mogre::PixelFormat eFormat)
{
	Ogre::Image* ptr = &const_cast<Ogre::Image&>(_native->loadDynamicImage(pData, uWidth, uHeight, (Ogre::PixelFormat)eFormat));
	return gcnew Mogre::Image(ptr);
}

Mogre::Image^ Image::LoadRawData(Mogre::DataStreamPtr^ stream, size_t uWidth, size_t uHeight, size_t uDepth, Mogre::PixelFormat eFormat, size_t numFaces, size_t numMipMaps)
{
	Ogre::Image* ptr = &const_cast<Ogre::Image&>(_native->loadRawData((Ogre::DataStreamPtr&)stream, uWidth, uHeight, uDepth, (Ogre::PixelFormat)eFormat, numFaces, numMipMaps));
	return gcnew Mogre::Image(ptr);
}

Mogre::Image^ Image::LoadRawData(Mogre::DataStreamPtr^ stream, size_t uWidth, size_t uHeight, size_t uDepth, Mogre::PixelFormat eFormat, size_t numFaces)
{
	Ogre::Image* ptr = &const_cast<Ogre::Image&>(_native->loadRawData((Ogre::DataStreamPtr&)stream, uWidth, uHeight, uDepth, (Ogre::PixelFormat)eFormat, numFaces));
	return gcnew Mogre::Image(ptr);
}

Mogre::Image^ Image::LoadRawData(Mogre::DataStreamPtr^ stream, size_t uWidth, size_t uHeight, size_t uDepth, Mogre::PixelFormat eFormat)
{
	Ogre::Image* ptr = &const_cast<Ogre::Image&>(_native->loadRawData((Ogre::DataStreamPtr&)stream, uWidth, uHeight, uDepth, (Ogre::PixelFormat)eFormat));
	return gcnew Mogre::Image(ptr);
}

Mogre::Image^ Image::LoadRawData(Mogre::DataStreamPtr^ stream, size_t uWidth, size_t uHeight, Mogre::PixelFormat eFormat)
{
	Ogre::Image* ptr = &const_cast<Ogre::Image&>(_native->loadRawData((Ogre::DataStreamPtr&)stream, uWidth, uHeight, (Ogre::PixelFormat)eFormat));
	return gcnew Mogre::Image(ptr);
}

Mogre::Image^ Image::Load(String^ strFileName, String^ groupName)
{
	DECLARE_NATIVE_STRING(o_strFileName, strFileName);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	Ogre::Image* ptr = &const_cast<Ogre::Image&>(_native->load(o_strFileName, o_groupName));
	return gcnew Mogre::Image(ptr);
}

Mogre::Image^ Image::Load(Mogre::DataStreamPtr^ stream, String^ type)
{
	DECLARE_NATIVE_STRING(o_type, type);

	Ogre::Image* ptr = &const_cast<Ogre::Image&>(_native->load((Ogre::DataStreamPtr&)stream, o_type));
	return gcnew Mogre::Image(ptr);
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
Texture::~Texture()
{
	this->!Texture();
}
Texture::!Texture()
{
	//OnDisposing(this, nullptr);

	if (IsDisposed)
		return;

	if (_texturePtr)
	{
		TextureManager::Singleton->RemoveTextureInternal(this);
		delete _texturePtr;
		_texturePtr = nullptr;
		_isDisposed = true;
	}

	GC::SuppressFinalize(this);
	//OnDisposed(this, nullptr);
}


bool Texture::IsDisposed::get()
{
	return _isDisposed;
}

size_t Texture::Depth::get()
{
	return (*_texturePtr)->getDepth();
}

void Texture::Depth::set(size_t d)
{
	(*_texturePtr)->setDepth(d);
}

Mogre::ushort Texture::DesiredFloatBitDepth::get()
{
	return (*_texturePtr)->getDesiredFloatBitDepth();
}

void Texture::DesiredFloatBitDepth::set(Mogre::ushort bits)
{
	(*_texturePtr)->setDesiredFloatBitDepth(bits);
}

Mogre::PixelFormat Texture::DesiredFormat::get()
{
	return (Mogre::PixelFormat)(*_texturePtr)->getDesiredFormat();
}

Mogre::ushort Texture::DesiredIntegerBitDepth::get()
{
	return (*_texturePtr)->getDesiredIntegerBitDepth();
}
void Texture::DesiredIntegerBitDepth::set(Mogre::ushort bits)
{
	(*_texturePtr)->setDesiredIntegerBitDepth(bits);
}

Mogre::PixelFormat Texture::Format::get()
{
	return (Mogre::PixelFormat)(*_texturePtr)->getFormat();
}
void Texture::Format::set(Mogre::PixelFormat pf)
{
	(*_texturePtr)->setFormat((Ogre::PixelFormat)pf);
}

float Texture::Gamma::get()
{
	return (*_texturePtr)->getGamma();
}
void Texture::Gamma::set(float g)
{
	(*_texturePtr)->setGamma(g);
}

bool Texture::HasAlpha::get()
{
	return (*_texturePtr)->hasAlpha();
}

size_t Texture::Height::get()
{
	return (*_texturePtr)->getHeight();
}
void Texture::Height::set(size_t h)
{
	(*_texturePtr)->setHeight(h);
}

bool Texture::MipmapsHardwareGenerated::get()
{
	return (*_texturePtr)->getMipmapsHardwareGenerated();
}

size_t Texture::NumFaces::get()
{
	return (*_texturePtr)->getNumFaces();
}

size_t Texture::NumMipmaps::get()
{
	return (*_texturePtr)->getNumMipmaps();
}
void Texture::NumMipmaps::set(size_t num)
{
	(*_texturePtr)->setNumMipmaps(num);
}

size_t Texture::SrcDepth::get()
{
	return (*_texturePtr)->getSrcDepth();
}

Mogre::PixelFormat Texture::SrcFormat::get()
{
	return (Mogre::PixelFormat)(*_texturePtr)->getSrcFormat();
}

size_t Texture::SrcHeight::get()
{
	return (*_texturePtr)->getSrcHeight();
}

size_t Texture::SrcWidth::get()
{
	return (*_texturePtr)->getSrcWidth();
}

Mogre::TextureType Texture::TextureType::get()
{
	return (Mogre::TextureType)(*_texturePtr)->getTextureType();
}
void Texture::TextureType::set(Mogre::TextureType ttype)
{
	(*_texturePtr)->setTextureType((Ogre::TextureType)ttype);
}

bool Texture::TreatLuminanceAsAlpha::get()
{
	return (*_texturePtr)->getTreatLuminanceAsAlpha();
}
void Texture::TreatLuminanceAsAlpha::set(bool asAlpha)
{
	(*_texturePtr)->setTreatLuminanceAsAlpha(asAlpha);
}

int Texture::Usage::get()
{
	return (*_texturePtr)->getUsage();
}
void Texture::Usage::set(int u)
{
	(*_texturePtr)->setUsage(u);
}

size_t Texture::Width::get()
{
	return (*_texturePtr)->getWidth();
}
void Texture::Width::set(size_t w)
{
	(*_texturePtr)->setWidth(w);
}

void Texture::CreateInternalResources()
{
	(*_texturePtr)->createInternalResources();
}

void Texture::FreeInternalResources()
{
	(*_texturePtr)->freeInternalResources();
}

void Texture::CopyToTexture(Mogre::TexturePtr^ target)
{
	(*_texturePtr)->copyToTexture(*target->UnmanagedPointer);
}

void Texture::LoadImage(Mogre::Image^ img)
{
	auto imagePtr = GetPointerOrNull(img);
	(*_texturePtr)->loadImage(*imagePtr);
}

void Texture::LoadRawData(Mogre::DataStreamPtr^ stream, Mogre::ushort uWidth, Mogre::ushort uHeight, Mogre::PixelFormat eFormat)
{
	(*_texturePtr)->loadRawData((Ogre::DataStreamPtr&)stream, uWidth, uHeight, (Ogre::PixelFormat)eFormat);
}

void Texture::SetDesiredBitDepths(Mogre::ushort integerBits, Mogre::ushort floatBits)
{
	(*_texturePtr)->setDesiredBitDepths(integerBits, floatBits);
}

Mogre::HardwarePixelBufferSharedPtr^ Texture::GetBuffer(size_t face, size_t mipmap)
{
	return (*_texturePtr)->getBuffer(face, mipmap);
}

Mogre::HardwarePixelBufferSharedPtr^ Texture::GetBuffer(size_t face)
{
	return (*_texturePtr)->getBuffer(face);
}

Mogre::HardwarePixelBufferSharedPtr^ Texture::GetBuffer()
{
	return (*_texturePtr)->getBuffer();
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

Mogre::Texture^ TextureManager::GetByName(String^ name)
{
	return GetByName(name, TO_CLR_STRING(Ogre::ResourceGroupManager::AUTODETECT_RESOURCE_GROUP_NAME));
}

Mogre::Texture^ TextureManager::GetByName(String^ name, String^ groupName)
{
	DECLARE_NATIVE_STRING(o_name, name);

	auto textureCache = GetTextureCache(groupName);
	Texture^ texture;
	if (textureCache->TryGetValue(name, texture))
	{
		return texture;
	}

	auto texturePtr = static_cast<Ogre::TextureManager*>(_native)->getByName(o_name);
	if (texturePtr.isNull())
		return nullptr;

	texture = gcnew Texture(new Ogre::TexturePtr(texturePtr));
	textureCache->Add(name, texture);
	return texture;
}

Mogre::TexturePtr^ TextureManager::Load(String^ name, String^ group, Mogre::TextureType texType, int numMipmaps, Mogre::Real gamma, bool isAlpha, Mogre::PixelFormat desiredFormat)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	auto texturePtr = static_cast<Ogre::TextureManager*>(_native)->load(o_name, o_group, (Ogre::TextureType)texType, numMipmaps, gamma, isAlpha, (Ogre::PixelFormat)desiredFormat);
	if (texturePtr.isNull())
		return nullptr;

	auto texture = gcnew Texture(new Ogre::TexturePtr(texturePtr));
	auto textureCache = GetTextureCache(group);
	textureCache->Add(name, texture);
	return texture;
}

Mogre::TexturePtr^ TextureManager::Load(String^ name, String^ group, Mogre::TextureType texType, int numMipmaps, Mogre::Real gamma, bool isAlpha)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	auto texturePtr = static_cast<Ogre::TextureManager*>(_native)->load(o_name, o_group, (Ogre::TextureType)texType, numMipmaps, gamma, isAlpha);
	if (texturePtr.isNull())
		return nullptr;

	auto texture = gcnew Texture(new Ogre::TexturePtr(texturePtr));
	auto textureCache = GetTextureCache(group);
	textureCache->Add(name, texture);
	return texture;
}

Mogre::TexturePtr^ TextureManager::Load(String^ name, String^ group, Mogre::TextureType texType, int numMipmaps, Mogre::Real gamma)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	auto texturePtr = static_cast<Ogre::TextureManager*>(_native)->load(o_name, o_group, (Ogre::TextureType)texType, numMipmaps, gamma);
	if (texturePtr.isNull())
		return nullptr;

	auto texture = gcnew Texture(new Ogre::TexturePtr(texturePtr));
	auto textureCache = GetTextureCache(group);
	textureCache->Add(name, texture);
	return texture;
}

Mogre::TexturePtr^ TextureManager::Load(String^ name, String^ group, Mogre::TextureType texType, int numMipmaps)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	auto texturePtr = static_cast<Ogre::TextureManager*>(_native)->load(o_name, o_group, (Ogre::TextureType)texType, numMipmaps);
	if (texturePtr.isNull())
		return nullptr;

	auto texture = gcnew Texture(new Ogre::TexturePtr(texturePtr));
	auto textureCache = GetTextureCache(group);
	textureCache->Add(name, texture);
	return texture;
}

Mogre::TexturePtr^ TextureManager::Load(String^ name, String^ group, Mogre::TextureType texType)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	auto texturePtr = static_cast<Ogre::TextureManager*>(_native)->load(o_name, o_group, (Ogre::TextureType)texType);
	if (texturePtr.isNull())
		return nullptr;

	auto texture = gcnew Texture(new Ogre::TexturePtr(texturePtr));
	auto textureCache = GetTextureCache(group);
	textureCache->Add(name, texture);
	return texture;
}

Mogre::TexturePtr^ TextureManager::Load(String^ name, String^ group)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	auto texturePtr = static_cast<Ogre::TextureManager*>(_native)->load(o_name, o_group);
	if (texturePtr.isNull())
		return nullptr;

	auto texture = gcnew Texture(new Ogre::TexturePtr(texturePtr));
	auto textureCache = GetTextureCache(group);
	textureCache->Add(name, texture);
	return texture;
}

Mogre::TexturePtr^ TextureManager::LoadImage(String^ name, String^ group, Mogre::Image^ img, Mogre::TextureType texType, int iNumMipmaps, Mogre::Real gamma, bool isAlpha, Mogre::PixelFormat desiredFormat)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);
	auto imagePtr = GetPointerOrNull(img);

	auto texturePtr = static_cast<Ogre::TextureManager*>(_native)->loadImage(o_name, o_group, imagePtr ? *imagePtr : Ogre::Image(), (Ogre::TextureType)texType, iNumMipmaps, gamma, isAlpha, (Ogre::PixelFormat)desiredFormat);
	if (texturePtr.isNull())
		return nullptr;

	auto texture = gcnew Texture(new Ogre::TexturePtr(texturePtr));
	auto textureCache = GetTextureCache(group);
	textureCache->Add(name, texture);
	return texture;
}

Mogre::TexturePtr^ TextureManager::LoadImage(String^ name, String^ group, Mogre::Image^ img, Mogre::TextureType texType, int iNumMipmaps, Mogre::Real gamma, bool isAlpha)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);
	auto imagePtr = GetPointerOrNull(img);

	auto texturePtr = static_cast<Ogre::TextureManager*>(_native)->loadImage(o_name, o_group, imagePtr ? *imagePtr : Ogre::Image(), (Ogre::TextureType)texType, iNumMipmaps, gamma, isAlpha);
	if (texturePtr.isNull())
		return nullptr;

	auto texture = gcnew Texture(new Ogre::TexturePtr(texturePtr));
	auto textureCache = GetTextureCache(group);
	textureCache->Add(name, texture);
	return texture;
}

Mogre::TexturePtr^ TextureManager::LoadImage(String^ name, String^ group, Mogre::Image^ img, Mogre::TextureType texType, int iNumMipmaps, Mogre::Real gamma)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);
	auto imagePtr = GetPointerOrNull(img);

	auto texturePtr = static_cast<Ogre::TextureManager*>(_native)->loadImage(o_name, o_group, imagePtr ? *imagePtr : Ogre::Image(), (Ogre::TextureType)texType, iNumMipmaps, gamma);
	if (texturePtr.isNull())
		return nullptr;

	auto texture = gcnew Texture(new Ogre::TexturePtr(texturePtr));
	auto textureCache = GetTextureCache(group);
	textureCache->Add(name, texture);
	return texture;
}

Mogre::TexturePtr^ TextureManager::LoadImage(String^ name, String^ group, Mogre::Image^ img, Mogre::TextureType texType, int iNumMipmaps)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);
	auto imagePtr = GetPointerOrNull(img);

	auto texturePtr = static_cast<Ogre::TextureManager*>(_native)->loadImage(o_name, o_group, imagePtr ? *imagePtr : Ogre::Image(), (Ogre::TextureType)texType, iNumMipmaps);
	if (texturePtr.isNull())
		return nullptr;

	auto texture = gcnew Texture(new Ogre::TexturePtr(texturePtr));
	auto textureCache = GetTextureCache(group);
	textureCache->Add(name, texture);
	return texture;
}

Mogre::TexturePtr^ TextureManager::LoadImage(String^ name, String^ group, Mogre::Image^ img, Mogre::TextureType texType)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);
	auto imagePtr = GetPointerOrNull(img);

	auto texturePtr = static_cast<Ogre::TextureManager*>(_native)->loadImage(o_name, o_group, imagePtr ? *imagePtr : Ogre::Image(), (Ogre::TextureType)texType);
	if (texturePtr.isNull())
		return nullptr;

	auto texture = gcnew Texture(new Ogre::TexturePtr(texturePtr));
	auto textureCache = GetTextureCache(group);
	textureCache->Add(name, texture);
	return texture;
}

Mogre::TexturePtr^ TextureManager::LoadImage(String^ name, String^ group, Mogre::Image^ img)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);
	auto imagePtr = GetPointerOrNull(img);

	auto texturePtr = static_cast<Ogre::TextureManager*>(_native)->loadImage(o_name, o_group, imagePtr ? *imagePtr : Ogre::Image());
	if (texturePtr.isNull())
		return nullptr;

	auto texture = gcnew Texture(new Ogre::TexturePtr(texturePtr));
	auto textureCache = GetTextureCache(group);
	textureCache->Add(name, texture);
	return texture;
}

Mogre::TexturePtr^ TextureManager::LoadRawData(String^ name, String^ group, Mogre::DataStreamPtr^ stream, Mogre::ushort uWidth, Mogre::ushort uHeight, Mogre::PixelFormat format, Mogre::TextureType texType, int iNumMipmaps, Mogre::Real gamma)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	auto texturePtr = static_cast<Ogre::TextureManager*>(_native)->loadRawData(o_name, o_group, (Ogre::DataStreamPtr&)stream, uWidth, uHeight, (Ogre::PixelFormat)format, (Ogre::TextureType)texType, iNumMipmaps, gamma);
	if (texturePtr.isNull())
		return nullptr;

	auto texture = gcnew Texture(new Ogre::TexturePtr(texturePtr));
	auto textureCache = GetTextureCache(group);
	textureCache->Add(name, texture);
	return texture;
}

Mogre::TexturePtr^ TextureManager::LoadRawData(String^ name, String^ group, Mogre::DataStreamPtr^ stream, Mogre::ushort uWidth, Mogre::ushort uHeight, Mogre::PixelFormat format, Mogre::TextureType texType, int iNumMipmaps)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	auto texturePtr = static_cast<Ogre::TextureManager*>(_native)->loadRawData(o_name, o_group, (Ogre::DataStreamPtr&)stream, uWidth, uHeight, (Ogre::PixelFormat)format, (Ogre::TextureType)texType, iNumMipmaps);
	if (texturePtr.isNull())
		return nullptr;

	auto texture = gcnew Texture(new Ogre::TexturePtr(texturePtr));
	auto textureCache = GetTextureCache(group);
	textureCache->Add(name, texture);
	return texture;
}

Mogre::TexturePtr^ TextureManager::LoadRawData(String^ name, String^ group, Mogre::DataStreamPtr^ stream, Mogre::ushort uWidth, Mogre::ushort uHeight, Mogre::PixelFormat format, Mogre::TextureType texType)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	auto texturePtr = static_cast<Ogre::TextureManager*>(_native)->loadRawData(o_name, o_group, (Ogre::DataStreamPtr&)stream, uWidth, uHeight, (Ogre::PixelFormat)format, (Ogre::TextureType)texType);
	if (texturePtr.isNull())
		return nullptr;

	auto texture = gcnew Texture(new Ogre::TexturePtr(texturePtr));
	auto textureCache = GetTextureCache(group);
	textureCache->Add(name, texture);
	return texture;
}

Mogre::TexturePtr^ TextureManager::LoadRawData(String^ name, String^ group, Mogre::DataStreamPtr^ stream, Mogre::ushort uWidth, Mogre::ushort uHeight, Mogre::PixelFormat format)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	auto texturePtr = static_cast<Ogre::TextureManager*>(_native)->loadRawData(o_name, o_group, (Ogre::DataStreamPtr&)stream, uWidth, uHeight, (Ogre::PixelFormat)format);
	if (texturePtr.isNull())
		return nullptr;

	auto texture = gcnew Texture(new Ogre::TexturePtr(texturePtr));
	auto textureCache = GetTextureCache(group);
	textureCache->Add(name, texture);
	return texture;
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

	auto texturePtr = static_cast<Ogre::TextureManager*>(_native)->createManual(o_name, o_group, (Ogre::TextureType)texType, width, height, depth, num_mips, (Ogre::PixelFormat)format, usage);
	if (texturePtr.isNull())
		return nullptr;

	auto texture = gcnew Texture(new Ogre::TexturePtr(texturePtr));
	auto textureCache = GetTextureCache(group);
	textureCache->Add(name, texture);
	return texture;
}

Mogre::TexturePtr^ TextureManager::CreateManual(String^ name, String^ group, Mogre::TextureType texType, Mogre::uint width, Mogre::uint height, Mogre::uint depth, int num_mips, Mogre::PixelFormat format)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	auto texturePtr = static_cast<Ogre::TextureManager*>(_native)->createManual(o_name, o_group, (Ogre::TextureType)texType, width, height, depth, num_mips, (Ogre::PixelFormat)format);
	if (texturePtr.isNull())
		return nullptr;

	auto texture = gcnew Texture(new Ogre::TexturePtr(texturePtr));
	auto textureCache = GetTextureCache(group);
	textureCache->Add(name, texture);
	return texture;
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

	auto texturePtr = static_cast<Ogre::TextureManager*>(_native)->createManual(
		o_name,
		o_group,
		(Ogre::TextureType)texType,
		width,
		height,
		num_mips,
		(Ogre::PixelFormat)format,
		usage);
	if (texturePtr.isNull())
		return nullptr;

	auto texture = gcnew Texture(new Ogre::TexturePtr(texturePtr));
	auto textureCache = GetTextureCache(group);
	textureCache->Add(name, texture);
	return texture;
}

Mogre::TexturePtr^ TextureManager::CreateManual(String^ name, String^ group, Mogre::TextureType texType, Mogre::uint width, Mogre::uint height, int num_mips, Mogre::PixelFormat format)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	auto texturePtr = static_cast<Ogre::TextureManager*>(_native)->createManual(o_name, o_group, (Ogre::TextureType)texType, width, height, num_mips, (Ogre::PixelFormat)format);
	if (texturePtr.isNull())
		return nullptr;

	auto texture = gcnew Texture(new Ogre::TexturePtr(texturePtr));
	auto textureCache = GetTextureCache(group);
	textureCache->Add(name, texture);
	return texture;
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

void TextureManager::Shutdown()
{
restart:
	for each (auto textureCache in _textures)
	{
		auto copy = gcnew System::Collections::Generic::Dictionary<String^, Texture^>(textureCache.Value);
		for each(auto texture in copy->Values)
		{
			delete texture;
		}
		_textures->Remove(textureCache.Key);
		goto restart;
	}

	_textures->Clear();
}

void TextureManager::RemoveTextureInternal(Mogre::Texture^ texture)
{
	auto textureCache = GetTextureCache(texture->Group);
	textureCache->Remove(texture->Name);
	Unload(texture->Handle);
	Remove(texture->Handle);
}

Mogre::Texture^ TextureManager::GetOrCreateTexture(Ogre::TexturePtr* nativePtr)
{
	auto clrName = TO_CLR_STRING((*nativePtr)->getName());
	auto textureCache = GetTextureCache(TO_CLR_STRING((*nativePtr)->getGroup()));
	Texture^ texture;
	if (textureCache->TryGetValue(clrName, texture))
	{
		return texture;
	}

	texture = gcnew Texture(nativePtr);
	textureCache->Add(clrName, texture);
	return texture;
}

System::Collections::Generic::Dictionary<String^, Texture^>^ TextureManager::GetTextureCache(String^ groupName)
{
	System::Collections::Generic::Dictionary<String^, Texture^>^ result;
	if (!_textures->TryGetValue(groupName, result))
	{
		result = gcnew System::Collections::Generic::Dictionary<String^, Texture^>();
		_textures->Add(groupName, result);
	}

	return result;
}