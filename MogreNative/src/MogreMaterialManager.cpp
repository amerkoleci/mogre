#include "stdafx.h"
#include "MogreMaterialManager.h"
#include "MogreTextureManager.h"
#include "MogreGpuProgramManager.h"
#include "MogreFrustum.h"

using namespace Mogre;
// LayerBlendModeEx_NativePtr
Mogre::LayerBlendType LayerBlendModeEx_NativePtr::blendType::get()
{
	return (Mogre::LayerBlendType)_native->blendType;
}
void LayerBlendModeEx_NativePtr::blendType::set(Mogre::LayerBlendType value)
{
	_native->blendType = (Ogre::LayerBlendType)value;
}

Mogre::LayerBlendOperationEx LayerBlendModeEx_NativePtr::operation::get()
{
	return (Mogre::LayerBlendOperationEx)_native->operation;
}
void LayerBlendModeEx_NativePtr::operation::set(Mogre::LayerBlendOperationEx value)
{
	_native->operation = (Ogre::LayerBlendOperationEx)value;
}

Mogre::LayerBlendSource LayerBlendModeEx_NativePtr::source1::get()
{
	return (Mogre::LayerBlendSource)_native->source1;
}
void LayerBlendModeEx_NativePtr::source1::set(Mogre::LayerBlendSource value)
{
	_native->source1 = (Ogre::LayerBlendSource)value;
}

Mogre::LayerBlendSource LayerBlendModeEx_NativePtr::source2::get()
{
	return (Mogre::LayerBlendSource)_native->source2;
}
void LayerBlendModeEx_NativePtr::source2::set(Mogre::LayerBlendSource value)
{
	_native->source2 = (Ogre::LayerBlendSource)value;
}

Mogre::ColourValue LayerBlendModeEx_NativePtr::colourArg1::get()
{
	return ToColor4(_native->colourArg1);
}

void LayerBlendModeEx_NativePtr::colourArg1::set(Mogre::ColourValue value)
{
	_native->colourArg1 = FromColor4(value);
}

Mogre::ColourValue LayerBlendModeEx_NativePtr::colourArg2::get()
{
	return ToColor4(_native->colourArg2);
}
void LayerBlendModeEx_NativePtr::colourArg2::set(Mogre::ColourValue value)
{
	_native->colourArg2 = FromColor4(value);
}

Mogre::Real LayerBlendModeEx_NativePtr::alphaArg1::get()
{
	return _native->alphaArg1;
}
void LayerBlendModeEx_NativePtr::alphaArg1::set(Mogre::Real value)
{
	_native->alphaArg1 = value;
}

Mogre::Real LayerBlendModeEx_NativePtr::alphaArg2::get()
{
	return _native->alphaArg2;
}
void LayerBlendModeEx_NativePtr::alphaArg2::set(Mogre::Real value)
{
	_native->alphaArg2 = value;
}

Mogre::Real LayerBlendModeEx_NativePtr::factor::get()
{
	return _native->factor;
}
void LayerBlendModeEx_NativePtr::factor::set(Mogre::Real value)
{
	_native->factor = value;
}

bool LayerBlendModeEx_NativePtr::Equals(Object^ obj)
{
	LayerBlendModeEx_NativePtr^ clr = dynamic_cast<LayerBlendModeEx_NativePtr^>(obj);
	if (clr == CLR_NULL)
	{
		return false;
	}

	if (_native == NULL) throw gcnew Exception("The underlying native object for the caller is null.");
	if (clr->_native == NULL) throw gcnew ArgumentException("The underlying native object for parameter 'obj' is null.");

	return *_native == *(static_cast<Ogre::LayerBlendModeEx*>(clr->_native));
}

bool LayerBlendModeEx_NativePtr::Equals(LayerBlendModeEx_NativePtr obj)
{
	if (_native == NULL) throw gcnew Exception("The underlying native object for the caller is null.");
	if (obj._native == NULL) throw gcnew ArgumentException("The underlying native object for parameter 'obj' is null.");

	return *_native == *obj._native;
}

bool LayerBlendModeEx_NativePtr::operator ==(LayerBlendModeEx_NativePtr obj1, LayerBlendModeEx_NativePtr obj2)
{
	return obj1.Equals(obj2);
}

bool LayerBlendModeEx_NativePtr::operator !=(LayerBlendModeEx_NativePtr obj1, LayerBlendModeEx_NativePtr obj2)
{
	return !obj1.Equals(obj2);
}

Mogre::LayerBlendModeEx_NativePtr LayerBlendModeEx_NativePtr::Create()
{
	LayerBlendModeEx_NativePtr ptr;
	ptr._native = new Ogre::LayerBlendModeEx();
	return ptr;
}

// --------------- TextureUnitState ---------------
Mogre::TextureUnitState::TextureEffectType TextureUnitState::TextureEffect_NativePtr::type::get()
{
	return (Mogre::TextureUnitState::TextureEffectType)_native->type;
}
void TextureUnitState::TextureEffect_NativePtr::type::set(Mogre::TextureUnitState::TextureEffectType value)
{
	_native->type = (Ogre::TextureUnitState::TextureEffectType)value;
}

int TextureUnitState::TextureEffect_NativePtr::subtype::get()
{
	return _native->subtype;
}
void TextureUnitState::TextureEffect_NativePtr::subtype::set(int value)
{
	_native->subtype = value;
}

Mogre::Real TextureUnitState::TextureEffect_NativePtr::arg1::get()
{
	return _native->arg1;
}
void TextureUnitState::TextureEffect_NativePtr::arg1::set(Mogre::Real value)
{
	_native->arg1 = value;
}

Mogre::Real TextureUnitState::TextureEffect_NativePtr::arg2::get()
{
	return _native->arg2;
}

void TextureUnitState::TextureEffect_NativePtr::arg2::set(Mogre::Real value)
{
	_native->arg2 = value;
}

Mogre::WaveformType TextureUnitState::TextureEffect_NativePtr::waveType::get()
{
	return (Mogre::WaveformType)_native->waveType;
}

void TextureUnitState::TextureEffect_NativePtr::waveType::set(Mogre::WaveformType value)
{
	_native->waveType = (Ogre::WaveformType)value;
}

Mogre::Real TextureUnitState::TextureEffect_NativePtr::base::get()
{
	return _native->base;
}

void TextureUnitState::TextureEffect_NativePtr::base::set(Mogre::Real value)
{
	_native->base = value;
}

Mogre::Real TextureUnitState::TextureEffect_NativePtr::frequency::get()
{
	return _native->frequency;
}

void TextureUnitState::TextureEffect_NativePtr::frequency::set(Mogre::Real value)
{
	_native->frequency = value;
}

Mogre::Real TextureUnitState::TextureEffect_NativePtr::phase::get()
{
	return _native->phase;
}

void TextureUnitState::TextureEffect_NativePtr::phase::set(Mogre::Real value)
{
	_native->phase = value;
}

Mogre::Real TextureUnitState::TextureEffect_NativePtr::amplitude::get()
{
	return _native->amplitude;
}

void TextureUnitState::TextureEffect_NativePtr::amplitude::set(Mogre::Real value)
{
	_native->amplitude = value;
}

Mogre::Frustum^ TextureUnitState::TextureEffect_NativePtr::frustum::get()
{
	return _native->frustum;
}

Mogre::TextureUnitState::TextureEffect_NativePtr TextureUnitState::TextureEffect_NativePtr::Create()
{
	TextureEffect_NativePtr ptr;
	ptr._native = new Ogre::TextureUnitState::TextureEffect();
	return ptr;
}

CPP_DECLARE_STLMULTIMAP(TextureUnitState::, EffectMap, Mogre::TextureUnitState::TextureEffectType, Mogre::TextureUnitState::TextureEffect_NativePtr, Ogre::TextureUnitState::TextureEffectType, Ogre::TextureUnitState::TextureEffect);

TextureUnitState::TextureUnitState(Mogre::Pass^ parent)
{
	_createdByCLR = true;
	_native = new Ogre::TextureUnitState(parent);
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

TextureUnitState::TextureUnitState(Mogre::Pass^ parent, Mogre::TextureUnitState^ oth)
{
	_createdByCLR = true;
	_native = new Ogre::TextureUnitState(parent, oth);
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

TextureUnitState::TextureUnitState(Mogre::Pass^ parent, String^ texName, unsigned int texCoordSet)
{
	_createdByCLR = true;
	DECLARE_NATIVE_STRING(o_texName, texName);

	_native = new Ogre::TextureUnitState(parent, o_texName, texCoordSet);
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

TextureUnitState::TextureUnitState(Mogre::Pass^ parent, String^ texName)
{
	_createdByCLR = true;
	DECLARE_NATIVE_STRING(o_texName, texName);

	_native = new Ogre::TextureUnitState(parent, o_texName);
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

TextureUnitState::~TextureUnitState()
{
	this->!TextureUnitState();
}

TextureUnitState::!TextureUnitState()
{
	OnDisposing(this, nullptr);

	if (IsDisposed)
		return;

	if (_createdByCLR && _native)
	{
		delete _native;
		_native = 0;
	}

	OnDisposed(this, nullptr);
}

Mogre::LayerBlendModeEx_NativePtr TextureUnitState::AlphaBlendMode::get()
{
	return static_cast<const Ogre::TextureUnitState*>(_native)->getAlphaBlendMode();
}

Mogre::Real TextureUnitState::AnimationDuration::get()
{
	return static_cast<const Ogre::TextureUnitState*>(_native)->getAnimationDuration();
}

Mogre::SceneBlendFactor TextureUnitState::ColourBlendFallbackDest::get()
{
	return (Mogre::SceneBlendFactor)static_cast<const Ogre::TextureUnitState*>(_native)->getColourBlendFallbackDest();
}

Mogre::SceneBlendFactor TextureUnitState::ColourBlendFallbackSrc::get()
{
	return (Mogre::SceneBlendFactor)static_cast<const Ogre::TextureUnitState*>(_native)->getColourBlendFallbackSrc();
}

Mogre::LayerBlendModeEx_NativePtr TextureUnitState::ColourBlendMode::get()
{
	return static_cast<const Ogre::TextureUnitState*>(_native)->getColourBlendMode();
}

unsigned int TextureUnitState::CurrentFrame::get()
{
	return static_cast<const Ogre::TextureUnitState*>(_native)->getCurrentFrame();
}
void TextureUnitState::CurrentFrame::set(unsigned int frameNumber)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setCurrentFrame(frameNumber);
}

Mogre::PixelFormat TextureUnitState::DesiredFormat::get()
{
	return (Mogre::PixelFormat)static_cast<const Ogre::TextureUnitState*>(_native)->getDesiredFormat();
}
void TextureUnitState::DesiredFormat::set(Mogre::PixelFormat desiredFormat)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setDesiredFormat((Ogre::PixelFormat)desiredFormat);
}

bool TextureUnitState::HasViewRelativeTextureCoordinateGeneration::get()
{
	return static_cast<const Ogre::TextureUnitState*>(_native)->hasViewRelativeTextureCoordinateGeneration();
}

bool TextureUnitState::IsAlpha::get()
{
	return static_cast<const Ogre::TextureUnitState*>(_native)->getIsAlpha();
}
void TextureUnitState::IsAlpha::set(bool isAlpha)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setIsAlpha(isAlpha);
}

bool TextureUnitState::IsBlank::get()
{
	return static_cast<const Ogre::TextureUnitState*>(_native)->isBlank();
}

bool TextureUnitState::IsCubic::get()
{
	return static_cast<const Ogre::TextureUnitState*>(_native)->isCubic();
}

bool TextureUnitState::IsLoaded::get()
{
	return static_cast<const Ogre::TextureUnitState*>(_native)->isLoaded();
}

bool TextureUnitState::IsTextureLoadFailing::get()
{
	return static_cast<const Ogre::TextureUnitState*>(_native)->isTextureLoadFailing();
}

String^ TextureUnitState::Name::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::TextureUnitState*>(_native)->getName());
}

void TextureUnitState::Name::set(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	static_cast<Ogre::TextureUnitState*>(_native)->setName(o_name);
}

unsigned int TextureUnitState::NumFrames::get()
{
	return static_cast<const Ogre::TextureUnitState*>(_native)->getNumFrames();
}

int TextureUnitState::NumMipmaps::get()
{
	return static_cast<const Ogre::TextureUnitState*>(_native)->getNumMipmaps();
}
void TextureUnitState::NumMipmaps::set(int numMipmaps)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setNumMipmaps(numMipmaps);
}

Mogre::Pass^ TextureUnitState::Parent::get()
{
	return static_cast<const Ogre::TextureUnitState*>(_native)->getParent();
}

unsigned int TextureUnitState::TextureAnisotropy::get()
{
	return static_cast<const Ogre::TextureUnitState*>(_native)->getTextureAnisotropy();
}
void TextureUnitState::TextureAnisotropy::set(unsigned int maxAniso)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setTextureAnisotropy(maxAniso);
}

Mogre::ColourValue TextureUnitState::TextureBorderColour::get()
{
	return ToColor4(static_cast<const Ogre::TextureUnitState*>(_native)->getTextureBorderColour());
}

void TextureUnitState::TextureBorderColour::set(Mogre::ColourValue colour)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setTextureBorderColour(FromColor4(colour));
}

unsigned int TextureUnitState::TextureCoordSet::get()
{
	return static_cast<const Ogre::TextureUnitState*>(_native)->getTextureCoordSet();
}
void TextureUnitState::TextureCoordSet::set(unsigned int set)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setTextureCoordSet(set);
}

float TextureUnitState::TextureMipmapBias::get()
{
	return static_cast<const Ogre::TextureUnitState*>(_native)->getTextureMipmapBias();
}
void TextureUnitState::TextureMipmapBias::set(float bias)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setTextureMipmapBias(bias);
}

String^ TextureUnitState::TextureName::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::TextureUnitState*>(_native)->getTextureName());
}

String^ TextureUnitState::TextureNameAlias::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::TextureUnitState*>(_native)->getTextureNameAlias());
}

void TextureUnitState::TextureNameAlias::set(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	static_cast<Ogre::TextureUnitState*>(_native)->setTextureNameAlias(o_name);
}

Mogre::Radian TextureUnitState::TextureRotate::get()
{
	return Mogre::Radian( static_cast<const Ogre::TextureUnitState*>(_native)->getTextureRotate().valueRadians() );
}

void TextureUnitState::TextureRotate::set(Mogre::Radian angle)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setTextureRotate(Ogre::Radian(angle.ValueRadians));
}

Mogre::Matrix4^ TextureUnitState::TextureTransform::get()
{
	return ToMatrix4(static_cast<const Ogre::TextureUnitState*>(_native)->getTextureTransform());
}

void TextureUnitState::TextureTransform::set(Mogre::Matrix4^ xform)
{
	pin_ptr<Ogre::Matrix4> p_xform = interior_ptr<Ogre::Matrix4>(&xform->m00);

	static_cast<Ogre::TextureUnitState*>(_native)->setTextureTransform(*p_xform);
}

Mogre::TextureType TextureUnitState::TextureType::get()
{
	return (Mogre::TextureType)static_cast<const Ogre::TextureUnitState*>(_native)->getTextureType();
}

Mogre::Real TextureUnitState::TextureUScale::get()
{
	return static_cast<const Ogre::TextureUnitState*>(_native)->getTextureUScale();
}
void TextureUnitState::TextureUScale::set(Mogre::Real value)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setTextureUScale(value);
}

Mogre::Real TextureUnitState::TextureUScroll::get()
{
	return static_cast<const Ogre::TextureUnitState*>(_native)->getTextureUScroll();
}
void TextureUnitState::TextureUScroll::set(Mogre::Real value)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setTextureUScroll(value);
}

Mogre::Real TextureUnitState::TextureVScale::get()
{
	return static_cast<const Ogre::TextureUnitState*>(_native)->getTextureVScale();
}
void TextureUnitState::TextureVScale::set(Mogre::Real value)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setTextureVScale(value);
}

Mogre::Real TextureUnitState::TextureVScroll::get()
{
	return static_cast<const Ogre::TextureUnitState*>(_native)->getTextureVScroll();
}
void TextureUnitState::TextureVScroll::set(Mogre::Real value)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setTextureVScroll(value);
}


void TextureUnitState::SetTextureName(String^ name, Mogre::TextureType ttype)
{
	DECLARE_NATIVE_STRING(o_name, name);

	static_cast<Ogre::TextureUnitState*>(_native)->setTextureName(o_name, (Ogre::TextureType)ttype);
}

void TextureUnitState::SetTextureName(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	static_cast<Ogre::TextureUnitState*>(_native)->setTextureName(o_name);
}

void TextureUnitState::SetCubicTextureName(String^ name, bool forUVW)
{
	DECLARE_NATIVE_STRING(o_name, name);

	static_cast<Ogre::TextureUnitState*>(_native)->setCubicTextureName(o_name, forUVW);
}

void TextureUnitState::SetCubicTextureName(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	static_cast<Ogre::TextureUnitState*>(_native)->setCubicTextureName(o_name);
}

void TextureUnitState::SetCubicTextureName(array<String^>^ names, bool forUVW)
{
	Ogre::String* arr_names = new Ogre::String[names->Length];
	for (int i = 0; i < names->Length; i++)
	{
		SET_NATIVE_STRING(arr_names[i], names[i])
	}

	static_cast<Ogre::TextureUnitState*>(_native)->setCubicTextureName(arr_names, forUVW);
	delete[] arr_names;

}
void TextureUnitState::SetCubicTextureName(array<String^>^ names)
{
	Ogre::String* arr_names = new Ogre::String[names->Length];
	for (int i = 0; i < names->Length; i++)
	{
		SET_NATIVE_STRING(arr_names[i], names[i])
	}

	static_cast<Ogre::TextureUnitState*>(_native)->setCubicTextureName(arr_names);
	delete[] arr_names;

}

void TextureUnitState::SetAnimatedTextureName(String^ name, unsigned int numFrames, Mogre::Real duration)
{
	DECLARE_NATIVE_STRING(o_name, name);

	static_cast<Ogre::TextureUnitState*>(_native)->setAnimatedTextureName(o_name, numFrames, duration);
}

void TextureUnitState::SetAnimatedTextureName(String^ name, unsigned int numFrames)
{
	DECLARE_NATIVE_STRING(o_name, name);

	static_cast<Ogre::TextureUnitState*>(_native)->setAnimatedTextureName(o_name, numFrames);
}

void TextureUnitState::SetAnimatedTextureName(array<String^>^ names, unsigned int numFrames, Mogre::Real duration)
{
	Ogre::String* arr_names = new Ogre::String[names->Length];
	for (int i = 0; i < names->Length; i++)
	{
		SET_NATIVE_STRING(arr_names[i], names[i])
	}

	static_cast<Ogre::TextureUnitState*>(_native)->setAnimatedTextureName(arr_names, numFrames, duration);
	delete[] arr_names;

}
void TextureUnitState::SetAnimatedTextureName(array<String^>^ names, unsigned int numFrames)
{
	Ogre::String* arr_names = new Ogre::String[names->Length];
	for (int i = 0; i < names->Length; i++)
	{
		SET_NATIVE_STRING(arr_names[i], names[i])
	}

	static_cast<Ogre::TextureUnitState*>(_native)->setAnimatedTextureName(arr_names, numFrames);
	delete[] arr_names;

}

Pair<size_t, size_t> TextureUnitState::GetTextureDimensions(unsigned int frame)
{
	auto pair = static_cast<const Ogre::TextureUnitState*>(_native)->getTextureDimensions(frame);
	return Pair<size_t, size_t>(pair.first, pair.second);
}

Pair<size_t, size_t> TextureUnitState::GetTextureDimensions()
{
	auto pair = static_cast<const Ogre::TextureUnitState*>(_native)->getTextureDimensions();
	return Pair<size_t, size_t>(pair.first, pair.second);
}

String^ TextureUnitState::GetFrameTextureName(unsigned int frameNumber)
{
	return TO_CLR_STRING(static_cast<const Ogre::TextureUnitState*>(_native)->getFrameTextureName(frameNumber));
}

void TextureUnitState::SetFrameTextureName(String^ name, unsigned int frameNumber)
{
	DECLARE_NATIVE_STRING(o_name, name);

	static_cast<Ogre::TextureUnitState*>(_native)->setFrameTextureName(o_name, frameNumber);
}

void TextureUnitState::AddFrameTextureName(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	static_cast<Ogre::TextureUnitState*>(_native)->addFrameTextureName(o_name);
}

void TextureUnitState::DeleteFrameTextureName(size_t frameNumber)
{
	static_cast<Ogre::TextureUnitState*>(_native)->deleteFrameTextureName(frameNumber);
}

void TextureUnitState::SetBindingType(Mogre::TextureUnitState::BindingType bt)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setBindingType((Ogre::TextureUnitState::BindingType)bt);
}

Mogre::TextureUnitState::BindingType TextureUnitState::GetBindingType()
{
	return (Mogre::TextureUnitState::BindingType)static_cast<const Ogre::TextureUnitState*>(_native)->getBindingType();
}

void TextureUnitState::SetContentType(Mogre::TextureUnitState::ContentType ct)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setContentType((Ogre::TextureUnitState::ContentType)ct);
}

Mogre::TextureUnitState::ContentType TextureUnitState::GetContentType()
{
	return (Mogre::TextureUnitState::ContentType)static_cast<const Ogre::TextureUnitState*>(_native)->getContentType();
}

bool TextureUnitState::Is3D()
{
	return static_cast<const Ogre::TextureUnitState*>(_native)->is3D();
}

void TextureUnitState::SetTextureScroll(Mogre::Real u, Mogre::Real v)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setTextureScroll(u, v);
}

void TextureUnitState::SetTextureScale(Mogre::Real uScale, Mogre::Real vScale)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setTextureScale(uScale, vScale);
}

Mogre::TextureUnitState::UVWAddressingMode TextureUnitState::GetTextureAddressingMode()
{
	return static_cast<const Ogre::TextureUnitState*>(_native)->getTextureAddressingMode();
}

void TextureUnitState::SetTextureAddressingMode(Mogre::TextureUnitState::TextureAddressingMode tam)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setTextureAddressingMode((Ogre::TextureUnitState::TextureAddressingMode)tam);
}

void TextureUnitState::SetTextureAddressingMode(Mogre::TextureUnitState::TextureAddressingMode u, Mogre::TextureUnitState::TextureAddressingMode v, Mogre::TextureUnitState::TextureAddressingMode w)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setTextureAddressingMode((Ogre::TextureUnitState::TextureAddressingMode)u, (Ogre::TextureUnitState::TextureAddressingMode)v, (Ogre::TextureUnitState::TextureAddressingMode)w);
}

void TextureUnitState::SetTextureAddressingMode(Mogre::TextureUnitState::UVWAddressingMode uvw)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setTextureAddressingMode(uvw);
}

void TextureUnitState::SetColourOperationEx(Mogre::LayerBlendOperationEx op, Mogre::LayerBlendSource source1, Mogre::LayerBlendSource source2, Mogre::ColourValue arg1, Mogre::ColourValue arg2, Mogre::Real manualBlend)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setColourOperationEx((Ogre::LayerBlendOperationEx)op, (Ogre::LayerBlendSource)source1, (Ogre::LayerBlendSource)source2, FromColor4(arg1), FromColor4(arg2), manualBlend);
}
void TextureUnitState::SetColourOperationEx(Mogre::LayerBlendOperationEx op, Mogre::LayerBlendSource source1, Mogre::LayerBlendSource source2, Mogre::ColourValue arg1, Mogre::ColourValue arg2)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setColourOperationEx((Ogre::LayerBlendOperationEx)op, (Ogre::LayerBlendSource)source1, (Ogre::LayerBlendSource)source2, FromColor4(arg1), FromColor4(arg2));
}
void TextureUnitState::SetColourOperationEx(Mogre::LayerBlendOperationEx op, Mogre::LayerBlendSource source1, Mogre::LayerBlendSource source2, Mogre::ColourValue arg1)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setColourOperationEx((Ogre::LayerBlendOperationEx)op, (Ogre::LayerBlendSource)source1, (Ogre::LayerBlendSource)source2, FromColor4(arg1));
}
void TextureUnitState::SetColourOperationEx(Mogre::LayerBlendOperationEx op, Mogre::LayerBlendSource source1, Mogre::LayerBlendSource source2)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setColourOperationEx((Ogre::LayerBlendOperationEx)op, (Ogre::LayerBlendSource)source1, (Ogre::LayerBlendSource)source2);
}
void TextureUnitState::SetColourOperationEx(Mogre::LayerBlendOperationEx op, Mogre::LayerBlendSource source1)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setColourOperationEx((Ogre::LayerBlendOperationEx)op, (Ogre::LayerBlendSource)source1);
}
void TextureUnitState::SetColourOperationEx(Mogre::LayerBlendOperationEx op)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setColourOperationEx((Ogre::LayerBlendOperationEx)op);
}

void TextureUnitState::SetColourOperation(Mogre::LayerBlendOperation op)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setColourOperation((Ogre::LayerBlendOperation)op);
}

void TextureUnitState::SetColourOpMultipassFallback(Mogre::SceneBlendFactor sourceFactor, Mogre::SceneBlendFactor destFactor)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setColourOpMultipassFallback((Ogre::SceneBlendFactor)sourceFactor, (Ogre::SceneBlendFactor)destFactor);
}

void TextureUnitState::SetAlphaOperation(Mogre::LayerBlendOperationEx op, Mogre::LayerBlendSource source1, Mogre::LayerBlendSource source2, Mogre::Real arg1, Mogre::Real arg2, Mogre::Real manualBlend)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setAlphaOperation((Ogre::LayerBlendOperationEx)op, (Ogre::LayerBlendSource)source1, (Ogre::LayerBlendSource)source2, arg1, arg2, manualBlend);
}
void TextureUnitState::SetAlphaOperation(Mogre::LayerBlendOperationEx op, Mogre::LayerBlendSource source1, Mogre::LayerBlendSource source2, Mogre::Real arg1, Mogre::Real arg2)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setAlphaOperation((Ogre::LayerBlendOperationEx)op, (Ogre::LayerBlendSource)source1, (Ogre::LayerBlendSource)source2, arg1, arg2);
}
void TextureUnitState::SetAlphaOperation(Mogre::LayerBlendOperationEx op, Mogre::LayerBlendSource source1, Mogre::LayerBlendSource source2, Mogre::Real arg1)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setAlphaOperation((Ogre::LayerBlendOperationEx)op, (Ogre::LayerBlendSource)source1, (Ogre::LayerBlendSource)source2, arg1);
}
void TextureUnitState::SetAlphaOperation(Mogre::LayerBlendOperationEx op, Mogre::LayerBlendSource source1, Mogre::LayerBlendSource source2)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setAlphaOperation((Ogre::LayerBlendOperationEx)op, (Ogre::LayerBlendSource)source1, (Ogre::LayerBlendSource)source2);
}
void TextureUnitState::SetAlphaOperation(Mogre::LayerBlendOperationEx op, Mogre::LayerBlendSource source1)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setAlphaOperation((Ogre::LayerBlendOperationEx)op, (Ogre::LayerBlendSource)source1);
}
void TextureUnitState::SetAlphaOperation(Mogre::LayerBlendOperationEx op)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setAlphaOperation((Ogre::LayerBlendOperationEx)op);
}

void TextureUnitState::AddEffect(Mogre::TextureUnitState::TextureEffect_NativePtr effect)
{
	static_cast<Ogre::TextureUnitState*>(_native)->addEffect(effect);
}

void TextureUnitState::SetEnvironmentMap(bool enable, Mogre::TextureUnitState::EnvMapType envMapType)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setEnvironmentMap(enable, (Ogre::TextureUnitState::EnvMapType)envMapType);
}
void TextureUnitState::SetEnvironmentMap(bool enable)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setEnvironmentMap(enable);
}

void TextureUnitState::SetScrollAnimation(Mogre::Real uSpeed, Mogre::Real vSpeed)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setScrollAnimation(uSpeed, vSpeed);
}

void TextureUnitState::SetRotateAnimation(Mogre::Real speed)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setRotateAnimation(speed);
}

void TextureUnitState::SetTransformAnimation(Mogre::TextureUnitState::TextureTransformType ttype, Mogre::WaveformType waveType, Mogre::Real base, Mogre::Real frequency, Mogre::Real phase, Mogre::Real amplitude)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setTransformAnimation((Ogre::TextureUnitState::TextureTransformType)ttype, (Ogre::WaveformType)waveType, base, frequency, phase, amplitude);
}
void TextureUnitState::SetTransformAnimation(Mogre::TextureUnitState::TextureTransformType ttype, Mogre::WaveformType waveType, Mogre::Real base, Mogre::Real frequency, Mogre::Real phase)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setTransformAnimation((Ogre::TextureUnitState::TextureTransformType)ttype, (Ogre::WaveformType)waveType, base, frequency, phase);
}
void TextureUnitState::SetTransformAnimation(Mogre::TextureUnitState::TextureTransformType ttype, Mogre::WaveformType waveType, Mogre::Real base, Mogre::Real frequency)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setTransformAnimation((Ogre::TextureUnitState::TextureTransformType)ttype, (Ogre::WaveformType)waveType, base, frequency);
}
void TextureUnitState::SetTransformAnimation(Mogre::TextureUnitState::TextureTransformType ttype, Mogre::WaveformType waveType, Mogre::Real base)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setTransformAnimation((Ogre::TextureUnitState::TextureTransformType)ttype, (Ogre::WaveformType)waveType, base);
}
void TextureUnitState::SetTransformAnimation(Mogre::TextureUnitState::TextureTransformType ttype, Mogre::WaveformType waveType)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setTransformAnimation((Ogre::TextureUnitState::TextureTransformType)ttype, (Ogre::WaveformType)waveType);
}

void TextureUnitState::SetProjectiveTexturing(bool enabled, Mogre::Frustum^ projectionSettings)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setProjectiveTexturing(enabled, projectionSettings);
}
void TextureUnitState::SetProjectiveTexturing(bool enabled)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setProjectiveTexturing(enabled);
}

void TextureUnitState::RemoveAllEffects()
{
	static_cast<Ogre::TextureUnitState*>(_native)->removeAllEffects();
}

void TextureUnitState::RemoveEffect(Mogre::TextureUnitState::TextureEffectType type)
{
	static_cast<Ogre::TextureUnitState*>(_native)->removeEffect((Ogre::TextureUnitState::TextureEffectType)type);
}

void TextureUnitState::SetBlank()
{
	static_cast<Ogre::TextureUnitState*>(_native)->setBlank();
}

void TextureUnitState::RetryTextureLoad()
{
	static_cast<Ogre::TextureUnitState*>(_native)->retryTextureLoad();
}

Mogre::TextureUnitState::Const_EffectMap^ TextureUnitState::GetEffects()
{
	return static_cast<const Ogre::TextureUnitState*>(_native)->getEffects();
}

void TextureUnitState::SetTextureFiltering(Mogre::TextureFilterOptions filterType)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setTextureFiltering((Ogre::TextureFilterOptions)filterType);
}

void TextureUnitState::SetTextureFiltering(Mogre::FilterType ftype, Mogre::FilterOptions opts)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setTextureFiltering((Ogre::FilterType)ftype, (Ogre::FilterOptions)opts);
}

void TextureUnitState::SetTextureFiltering(Mogre::FilterOptions minFilter, Mogre::FilterOptions magFilter, Mogre::FilterOptions mipFilter)
{
	static_cast<Ogre::TextureUnitState*>(_native)->setTextureFiltering((Ogre::FilterOptions)minFilter, (Ogre::FilterOptions)magFilter, (Ogre::FilterOptions)mipFilter);
}

Mogre::FilterOptions TextureUnitState::GetTextureFiltering(Mogre::FilterType ftpye)
{
	return (Mogre::FilterOptions)static_cast<const Ogre::TextureUnitState*>(_native)->getTextureFiltering((Ogre::FilterType)ftpye);
}

void TextureUnitState::_load()
{
	static_cast<Ogre::TextureUnitState*>(_native)->_load();
}

void TextureUnitState::_unload()
{
	static_cast<Ogre::TextureUnitState*>(_native)->_unload();
}

void TextureUnitState::_notifyNeedsRecompile()
{
	static_cast<Ogre::TextureUnitState*>(_native)->_notifyNeedsRecompile();
}

bool TextureUnitState::ApplyTextureAliases(Mogre::Const_AliasTextureNamePairList^ aliasList, bool apply)
{
	return static_cast<Ogre::TextureUnitState*>(_native)->applyTextureAliases(aliasList, apply);
}
bool TextureUnitState::ApplyTextureAliases(Mogre::Const_AliasTextureNamePairList^ aliasList)
{
	return static_cast<Ogre::TextureUnitState*>(_native)->applyTextureAliases(aliasList);
}

void TextureUnitState::_notifyParent(Mogre::Pass^ parent)
{
	static_cast<Ogre::TextureUnitState*>(_native)->_notifyParent(parent);
}

Mogre::TexturePtr^ TextureUnitState::_getTexturePtr()
{
	return _native->_getTexturePtr();
}

Mogre::TexturePtr^ TextureUnitState::_getTexturePtr(size_t frame)
{
	return _native->_getTexturePtr(frame);
}

void TextureUnitState::_setTexturePtr(Mogre::TexturePtr^ texptr)
{
	_native->_setTexturePtr((const Ogre::TexturePtr&)texptr);
}

void TextureUnitState::_setTexturePtr(Mogre::TexturePtr^ texptr, size_t frame)
{
	_native->_setTexturePtr((const Ogre::TexturePtr&)texptr, frame);
}

// --------------- IlluminationPass_NativePtr ---------------

Mogre::IlluminationStage IlluminationPass_NativePtr::stage::get()
{
	return (Mogre::IlluminationStage)_native->stage;
}

void IlluminationPass_NativePtr::stage::set(Mogre::IlluminationStage value)
{
	_native->stage = (Ogre::IlluminationStage)value;
}

Mogre::Pass^ IlluminationPass_NativePtr::pass::get()
{
	return _native->pass;
}
void IlluminationPass_NativePtr::pass::set(Mogre::Pass^ value)
{
	_native->pass = value;
}

bool IlluminationPass_NativePtr::destroyOnShutdown::get()
{
	return _native->destroyOnShutdown;
}
void IlluminationPass_NativePtr::destroyOnShutdown::set(bool value)
{
	_native->destroyOnShutdown = value;
}

Mogre::Pass^ IlluminationPass_NativePtr::originalPass::get()
{
	return _native->originalPass;
}

void IlluminationPass_NativePtr::originalPass::set(Mogre::Pass^ value)
{
	_native->originalPass = value;
}


Mogre::IlluminationPass_NativePtr IlluminationPass_NativePtr::Create()
{
	IlluminationPass_NativePtr ptr;
	ptr._native = new Ogre::IlluminationPass();
	return ptr;
}

// --------------- Pass ---------------
CPP_DECLARE_STLSET(Pass::, PassSet, Mogre::Pass^, Ogre::Pass*);
CPP_DECLARE_ITERATOR_NOCONSTRUCTOR(Pass::, TextureUnitStateIterator, Ogre::Pass::TextureUnitStateIterator, Mogre::Pass::TextureUnitStates, Mogre::TextureUnitState^, Ogre::TextureUnitState*);

Pass::Pass(Mogre::Technique^ parent, unsigned short index)
{
	_createdByCLR = true;
	_native = new Ogre::Pass(parent, index);
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

Pass::Pass(Mogre::Technique^ parent, unsigned short index, Mogre::Pass^ oth)
{
	_createdByCLR = true;
	_native = new Ogre::Pass(parent, index, oth);
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

Pass::~Pass()
{
	this->!Pass();
}

Pass::!Pass()
{
	OnDisposing(this, nullptr);

	if (IsDisposed)
		return;

	if (_createdByCLR && _native)
	{
		delete _native;
		_native = 0;
	}

	OnDisposed(this, nullptr);
}

Mogre::CompareFunction Pass::AlphaRejectFunction::get()
{
	return (Mogre::CompareFunction)static_cast<const Ogre::Pass*>(_native)->getAlphaRejectFunction();
}
void Pass::AlphaRejectFunction::set(Mogre::CompareFunction func)
{
	static_cast<Ogre::Pass*>(_native)->setAlphaRejectFunction((Ogre::CompareFunction)func);
}

unsigned char Pass::AlphaRejectValue::get()
{
	return static_cast<const Ogre::Pass*>(_native)->getAlphaRejectValue();
}
void Pass::AlphaRejectValue::set(unsigned char val)
{
	static_cast<Ogre::Pass*>(_native)->setAlphaRejectValue(val);
}

Mogre::ColourValue Pass::Ambient::get()
{
	return ToColor4(static_cast<const Ogre::Pass*>(_native)->getAmbient());
}

void Pass::Ambient::set(Mogre::ColourValue ambient)
{
	static_cast<Ogre::Pass*>(_native)->setAmbient(FromColor4(ambient));
}

bool Pass::ColourWriteEnabled::get()
{
	return static_cast<const Ogre::Pass*>(_native)->getColourWriteEnabled();
}
void Pass::ColourWriteEnabled::set(bool enabled)
{
	static_cast<Ogre::Pass*>(_native)->setColourWriteEnabled(enabled);
}

Mogre::CullingMode Pass::CullingMode::get()
{
	return (Mogre::CullingMode)static_cast<const Ogre::Pass*>(_native)->getCullingMode();
}
void Pass::CullingMode::set(Mogre::CullingMode mode)
{
	static_cast<Ogre::Pass*>(_native)->setCullingMode((Ogre::CullingMode)mode);
}

float Pass::DepthBiasConstant::get()
{
	return static_cast<const Ogre::Pass*>(_native)->getDepthBiasConstant();
}

float Pass::DepthBiasSlopeScale::get()
{
	return static_cast<const Ogre::Pass*>(_native)->getDepthBiasSlopeScale();
}

bool Pass::DepthCheckEnabled::get()
{
	return static_cast<const Ogre::Pass*>(_native)->getDepthCheckEnabled();
}
void Pass::DepthCheckEnabled::set(bool enabled)
{
	static_cast<Ogre::Pass*>(_native)->setDepthCheckEnabled(enabled);
}

Mogre::CompareFunction Pass::DepthFunction::get()
{
	return (Mogre::CompareFunction)static_cast<const Ogre::Pass*>(_native)->getDepthFunction();
}
void Pass::DepthFunction::set(Mogre::CompareFunction func)
{
	static_cast<Ogre::Pass*>(_native)->setDepthFunction((Ogre::CompareFunction)func);
}

bool Pass::DepthWriteEnabled::get()
{
	return static_cast<const Ogre::Pass*>(_native)->getDepthWriteEnabled();
}
void Pass::DepthWriteEnabled::set(bool enabled)
{
	static_cast<Ogre::Pass*>(_native)->setDepthWriteEnabled(enabled);
}

Mogre::SceneBlendFactor Pass::DestBlendFactor::get()
{
	return (Mogre::SceneBlendFactor)static_cast<const Ogre::Pass*>(_native)->getDestBlendFactor();
}

Mogre::ColourValue Pass::Diffuse::get()
{
	return ToColor4(static_cast<const Ogre::Pass*>(_native)->getDiffuse());
}

void Pass::Diffuse::set(Mogre::ColourValue diffuse)
{
	static_cast<Ogre::Pass*>(_native)->setDiffuse(FromColor4(diffuse));
}

Mogre::ColourValue Pass::FogColour::get()
{
	return ToColor4(static_cast<const Ogre::Pass*>(_native)->getFogColour());
}

Mogre::Real Pass::FogDensity::get()
{
	return static_cast<const Ogre::Pass*>(_native)->getFogDensity();
}

Mogre::Real Pass::FogEnd::get()
{
	return static_cast<const Ogre::Pass*>(_native)->getFogEnd();
}

Mogre::FogMode Pass::FogMode::get()
{
	return (Mogre::FogMode)static_cast<const Ogre::Pass*>(_native)->getFogMode();
}

bool Pass::FogOverride::get()
{
	return static_cast<const Ogre::Pass*>(_native)->getFogOverride();
}

Mogre::Real Pass::FogStart::get()
{
	return static_cast<const Ogre::Pass*>(_native)->getFogStart();
}

String^ Pass::FragmentProgramName::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::Pass*>(_native)->getFragmentProgramName());
}

bool Pass::HasFragmentProgram::get()
{
	return static_cast<const Ogre::Pass*>(_native)->hasFragmentProgram();
}

Ogre::uint32 Pass::Hash::get()
{
	return static_cast<const Ogre::Pass*>(_native)->getHash();
}

bool Pass::HasShadowCasterVertexProgram::get()
{
	return static_cast<const Ogre::Pass*>(_native)->hasShadowCasterVertexProgram();
}

bool Pass::HasVertexProgram::get()
{
	return static_cast<const Ogre::Pass*>(_native)->hasVertexProgram();
}

unsigned short Pass::Index::get()
{
	return static_cast<const Ogre::Pass*>(_native)->getIndex();
}

bool Pass::IsAmbientOnly::get()
{
	return static_cast<const Ogre::Pass*>(_native)->isAmbientOnly();
}

bool Pass::IsLoaded::get()
{
	return static_cast<const Ogre::Pass*>(_native)->isLoaded();
}

bool Pass::IsPointAttenuationEnabled::get()
{
	return static_cast<const Ogre::Pass*>(_native)->isPointAttenuationEnabled();
}

bool Pass::IsProgrammable::get()
{
	return static_cast<const Ogre::Pass*>(_native)->isProgrammable();
}

bool Pass::IsTransparent::get()
{
	return static_cast<const Ogre::Pass*>(_native)->isTransparent();
}

bool Pass::IteratePerLight::get()
{
	return static_cast<const Ogre::Pass*>(_native)->getIteratePerLight();
}

unsigned short Pass::LightCountPerIteration::get()
{
	return static_cast<const Ogre::Pass*>(_native)->getLightCountPerIteration();
}
void Pass::LightCountPerIteration::set(unsigned short c)
{
	static_cast<Ogre::Pass*>(_native)->setLightCountPerIteration(c);
}

bool Pass::LightingEnabled::get()
{
	return static_cast<const Ogre::Pass*>(_native)->getLightingEnabled();
}
void Pass::LightingEnabled::set(bool enabled)
{
	static_cast<Ogre::Pass*>(_native)->setLightingEnabled(enabled);
}

Mogre::ManualCullingMode Pass::ManualCullingMode::get()
{
	return (Mogre::ManualCullingMode)static_cast<const Ogre::Pass*>(_native)->getManualCullingMode();
}

void Pass::ManualCullingMode::set(Mogre::ManualCullingMode mode)
{
	static_cast<Ogre::Pass*>(_native)->setManualCullingMode((Ogre::ManualCullingMode)mode);
}

unsigned short Pass::MaxSimultaneousLights::get()
{
	return static_cast<const Ogre::Pass*>(_native)->getMaxSimultaneousLights();
}

void Pass::MaxSimultaneousLights::set(unsigned short maxLights)
{
	static_cast<Ogre::Pass*>(_native)->setMaxSimultaneousLights(maxLights);
}

String^ Pass::Name::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::Pass*>(_native)->getName());
}

void Pass::Name::set(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	static_cast<Ogre::Pass*>(_native)->setName(o_name);
}

unsigned short Pass::NumTextureUnitStates::get()
{
	return static_cast<const Ogre::Pass*>(_native)->getNumTextureUnitStates();
}

Mogre::Light::LightTypes Pass::OnlyLightType::get()
{
	return (Mogre::Light::LightTypes)static_cast<const Ogre::Pass*>(_native)->getOnlyLightType();
}

Mogre::Technique^ Pass::Parent::get()
{
	return static_cast<const Ogre::Pass*>(_native)->getParent();
}

size_t Pass::PassIterationCount::get()
{
	return static_cast<const Ogre::Pass*>(_native)->getPassIterationCount();
}

Mogre::Real Pass::PointAttenuationConstant::get()
{
	return static_cast<const Ogre::Pass*>(_native)->getPointAttenuationConstant();
}

Mogre::Real Pass::PointAttenuationLinear::get()
{
	return static_cast<const Ogre::Pass*>(_native)->getPointAttenuationLinear();
}

Mogre::Real Pass::PointAttenuationQuadratic::get()
{
	return static_cast<const Ogre::Pass*>(_native)->getPointAttenuationQuadratic();
}

Mogre::Real Pass::PointMaxSize::get()
{
	return static_cast<const Ogre::Pass*>(_native)->getPointMaxSize();
}
void Pass::PointMaxSize::set(Mogre::Real max)
{
	static_cast<Ogre::Pass*>(_native)->setPointMaxSize(max);
}

Mogre::Real Pass::PointMinSize::get()
{
	return static_cast<const Ogre::Pass*>(_native)->getPointMinSize();
}
void Pass::PointMinSize::set(Mogre::Real min)
{
	static_cast<Ogre::Pass*>(_native)->setPointMinSize(min);
}

Mogre::Real Pass::PointSize::get()
{
	return static_cast<const Ogre::Pass*>(_native)->getPointSize();
}
void Pass::PointSize::set(Mogre::Real ps)
{
	static_cast<Ogre::Pass*>(_native)->setPointSize(ps);
}

bool Pass::PointSpritesEnabled::get()
{
	return static_cast<const Ogre::Pass*>(_native)->getPointSpritesEnabled();
}
void Pass::PointSpritesEnabled::set(bool enabled)
{
	static_cast<Ogre::Pass*>(_native)->setPointSpritesEnabled(enabled);
}

Mogre::PolygonMode Pass::PolygonMode::get()
{
	return (Mogre::PolygonMode)static_cast<const Ogre::Pass*>(_native)->getPolygonMode();
}
void Pass::PolygonMode::set(Mogre::PolygonMode mode)
{
	static_cast<Ogre::Pass*>(_native)->setPolygonMode((Ogre::PolygonMode)mode);
}

String^ Pass::ResourceGroup::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::Pass*>(_native)->getResourceGroup());
}

bool Pass::RunOnlyForOneLightType::get()
{
	return static_cast<const Ogre::Pass*>(_native)->getRunOnlyForOneLightType();
}

Mogre::ColourValue Pass::SelfIllumination::get()
{
	return ToColor4(static_cast<const Ogre::Pass*>(_native)->getSelfIllumination());
}

void Pass::SelfIllumination::set(Mogre::ColourValue selfIllum)
{
	static_cast<Ogre::Pass*>(_native)->setSelfIllumination(FromColor4(selfIllum));
}

Mogre::ShadeOptions Pass::ShadingMode::get()
{
	return (Mogre::ShadeOptions)static_cast<const Ogre::Pass*>(_native)->getShadingMode();
}
void Pass::ShadingMode::set(Mogre::ShadeOptions mode)
{
	static_cast<Ogre::Pass*>(_native)->setShadingMode((Ogre::ShadeOptions)mode);
}

String^ Pass::ShadowCasterVertexProgramName::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::Pass*>(_native)->getShadowCasterVertexProgramName());
}

Mogre::Real Pass::Shininess::get()
{
	return static_cast<const Ogre::Pass*>(_native)->getShininess();
}

void Pass::Shininess::set(Mogre::Real val)
{
	static_cast<Ogre::Pass*>(_native)->setShininess(val);
}

Mogre::SceneBlendFactor Pass::SourceBlendFactor::get()
{
	return (Mogre::SceneBlendFactor)static_cast<const Ogre::Pass*>(_native)->getSourceBlendFactor();
}

Mogre::ColourValue Pass::Specular::get()
{
	return ToColor4(static_cast<const Ogre::Pass*>(_native)->getSpecular());
}

void Pass::Specular::set(Mogre::ColourValue specular)
{
	static_cast<Ogre::Pass*>(_native)->setSpecular(FromColor4(specular));
}

unsigned short Pass::StartLight::get()
{
	return static_cast<const Ogre::Pass*>(_native)->getStartLight();
}
void Pass::StartLight::set(unsigned short startLight)
{
	static_cast<Ogre::Pass*>(_native)->setStartLight(startLight);
}

Mogre::TrackVertexColourType Pass::VertexColourTracking::get()
{
	return static_cast<const Ogre::Pass*>(_native)->getVertexColourTracking();
}

void Pass::VertexColourTracking::set(Mogre::TrackVertexColourType tracking)
{
	static_cast<Ogre::Pass*>(_native)->setVertexColourTracking(tracking);
}

String^ Pass::VertexProgramName::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::Pass*>(_native)->getVertexProgramName());
}

void Pass::SetAmbient(Mogre::Real red, Mogre::Real green, Mogre::Real blue)
{
	static_cast<Ogre::Pass*>(_native)->setAmbient(red, green, blue);
}

void Pass::SetDiffuse(Mogre::Real red, Mogre::Real green, Mogre::Real blue, Mogre::Real alpha)
{
	static_cast<Ogre::Pass*>(_native)->setDiffuse(red, green, blue, alpha);
}

void Pass::SetSpecular(Mogre::Real red, Mogre::Real green, Mogre::Real blue, Mogre::Real alpha)
{
	static_cast<Ogre::Pass*>(_native)->setSpecular(red, green, blue, alpha);
}

void Pass::SetSelfIllumination(Mogre::Real red, Mogre::Real green, Mogre::Real blue)
{
	static_cast<Ogre::Pass*>(_native)->setSelfIllumination(red, green, blue);
}

void Pass::SetPointAttenuation(bool enabled, Mogre::Real constant, Mogre::Real linear, Mogre::Real quadratic)
{
	static_cast<Ogre::Pass*>(_native)->setPointAttenuation(enabled, constant, linear, quadratic);
}
void Pass::SetPointAttenuation(bool enabled, Mogre::Real constant, Mogre::Real linear)
{
	static_cast<Ogre::Pass*>(_native)->setPointAttenuation(enabled, constant, linear);
}
void Pass::SetPointAttenuation(bool enabled, Mogre::Real constant)
{
	static_cast<Ogre::Pass*>(_native)->setPointAttenuation(enabled, constant);
}
void Pass::SetPointAttenuation(bool enabled)
{
	static_cast<Ogre::Pass*>(_native)->setPointAttenuation(enabled);
}

Mogre::TextureUnitState^ Pass::CreateTextureUnitState()
{
	return static_cast<Ogre::Pass*>(_native)->createTextureUnitState();
}

Mogre::TextureUnitState^ Pass::CreateTextureUnitState(String^ textureName, unsigned short texCoordSet)
{
	DECLARE_NATIVE_STRING(o_textureName, textureName);

	return static_cast<Ogre::Pass*>(_native)->createTextureUnitState(o_textureName, texCoordSet);
}

Mogre::TextureUnitState^ Pass::CreateTextureUnitState(String^ textureName)
{
	DECLARE_NATIVE_STRING(o_textureName, textureName);

	return static_cast<Ogre::Pass*>(_native)->createTextureUnitState(o_textureName);
}

void Pass::AddTextureUnitState(Mogre::TextureUnitState^ state)
{
	static_cast<Ogre::Pass*>(_native)->addTextureUnitState(state);
}

Mogre::TextureUnitState^ Pass::GetTextureUnitState(unsigned short index)
{
	return static_cast<Ogre::Pass*>(_native)->getTextureUnitState(index);
}

Mogre::TextureUnitState^ Pass::GetTextureUnitState(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return static_cast<Ogre::Pass*>(_native)->getTextureUnitState(o_name);
}

unsigned short Pass::GetTextureUnitStateIndex(Mogre::TextureUnitState^ state)
{
	return static_cast<const Ogre::Pass*>(_native)->getTextureUnitStateIndex(state);
}

Mogre::Pass::TextureUnitStateIterator^ Pass::GetTextureUnitStateIterator()
{
	return static_cast<Ogre::Pass*>(_native)->getTextureUnitStateIterator();
}

void Pass::RemoveTextureUnitState(unsigned short index)
{
	static_cast<Ogre::Pass*>(_native)->removeTextureUnitState(index);
}

void Pass::RemoveAllTextureUnitStates()
{
	static_cast<Ogre::Pass*>(_native)->removeAllTextureUnitStates();
}

void Pass::SetSceneBlending(Mogre::SceneBlendType sbt)
{
	static_cast<Ogre::Pass*>(_native)->setSceneBlending((Ogre::SceneBlendType)sbt);
}

void Pass::SetSceneBlending(Mogre::SceneBlendFactor sourceFactor, Mogre::SceneBlendFactor destFactor)
{
	static_cast<Ogre::Pass*>(_native)->setSceneBlending((Ogre::SceneBlendFactor)sourceFactor, (Ogre::SceneBlendFactor)destFactor);
}

void Pass::SetFog(bool overrideScene, Mogre::FogMode mode, Mogre::ColourValue colour, Mogre::Real expDensity, Mogre::Real linearStart, Mogre::Real linearEnd)
{
	static_cast<Ogre::Pass*>(_native)->setFog(overrideScene, (Ogre::FogMode)mode, FromColor4(colour), expDensity, linearStart, linearEnd);
}
void Pass::SetFog(bool overrideScene, Mogre::FogMode mode, Mogre::ColourValue colour, Mogre::Real expDensity, Mogre::Real linearStart)
{
	static_cast<Ogre::Pass*>(_native)->setFog(overrideScene, (Ogre::FogMode)mode, FromColor4(colour), expDensity, linearStart);
}

void Pass::SetFog(bool overrideScene, Mogre::FogMode mode, Mogre::ColourValue colour, Mogre::Real expDensity)
{
	static_cast<Ogre::Pass*>(_native)->setFog(overrideScene, (Ogre::FogMode)mode, FromColor4(colour), expDensity);
}

void Pass::SetFog(bool overrideScene, Mogre::FogMode mode, Mogre::ColourValue colour)
{
	static_cast<Ogre::Pass*>(_native)->setFog(overrideScene, (Ogre::FogMode)mode, FromColor4(colour));
}

void Pass::SetFog(bool overrideScene, Mogre::FogMode mode)
{
	static_cast<Ogre::Pass*>(_native)->setFog(overrideScene, (Ogre::FogMode)mode);
}

void Pass::SetFog(bool overrideScene)
{
	static_cast<Ogre::Pass*>(_native)->setFog(overrideScene);
}

void Pass::SetDepthBias(float constantBias, float slopeScaleBias)
{
	static_cast<Ogre::Pass*>(_native)->setDepthBias(constantBias, slopeScaleBias);
}
void Pass::SetDepthBias(float constantBias)
{
	static_cast<Ogre::Pass*>(_native)->setDepthBias(constantBias);
}

void Pass::SetAlphaRejectSettings(Mogre::CompareFunction func, unsigned char value)
{
	static_cast<Ogre::Pass*>(_native)->setAlphaRejectSettings((Ogre::CompareFunction)func, value);
}

void Pass::SetIteratePerLight(bool enabled, bool onlyForOneLightType, Mogre::Light::LightTypes lightType)
{
	static_cast<Ogre::Pass*>(_native)->setIteratePerLight(enabled, onlyForOneLightType, (Ogre::Light::LightTypes)lightType);
}
void Pass::SetIteratePerLight(bool enabled, bool onlyForOneLightType)
{
	static_cast<Ogre::Pass*>(_native)->setIteratePerLight(enabled, onlyForOneLightType);
}
void Pass::SetIteratePerLight(bool enabled)
{
	static_cast<Ogre::Pass*>(_native)->setIteratePerLight(enabled);
}

void Pass::SetVertexProgram(String^ name, bool resetParams)
{
	DECLARE_NATIVE_STRING(o_name, name);

	static_cast<Ogre::Pass*>(_native)->setVertexProgram(o_name, resetParams);
}

void Pass::SetVertexProgram(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	static_cast<Ogre::Pass*>(_native)->setVertexProgram(o_name);
}

void Pass::SetVertexProgramParameters(Mogre::GpuProgramParametersSharedPtr^ params)
{
	static_cast<Ogre::Pass*>(_native)->setVertexProgramParameters((Ogre::GpuProgramParametersSharedPtr)params);
}

Mogre::GpuProgramParametersSharedPtr^ Pass::GetVertexProgramParameters()
{
	return static_cast<const Ogre::Pass*>(_native)->getVertexProgramParameters();
}

Mogre::GpuProgramPtr^ Pass::GetVertexProgram()
{
	return static_cast<const Ogre::Pass*>(_native)->getVertexProgram();
}

void Pass::SetShadowCasterVertexProgram(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	static_cast<Ogre::Pass*>(_native)->setShadowCasterVertexProgram(o_name);
}

void Pass::SetShadowCasterVertexProgramParameters(Mogre::GpuProgramParametersSharedPtr^ params)
{
	static_cast<Ogre::Pass*>(_native)->setShadowCasterVertexProgramParameters((Ogre::GpuProgramParametersSharedPtr)params);
}

Mogre::GpuProgramParametersSharedPtr^ Pass::GetShadowCasterVertexProgramParameters()
{
	return static_cast<const Ogre::Pass*>(_native)->getShadowCasterVertexProgramParameters();
}

Mogre::GpuProgramPtr^ Pass::GetShadowCasterVertexProgram()
{
	return static_cast<const Ogre::Pass*>(_native)->getShadowCasterVertexProgram();
}

void Pass::SetFragmentProgram(String^ name, bool resetParams)
{
	DECLARE_NATIVE_STRING(o_name, name);

	static_cast<Ogre::Pass*>(_native)->setFragmentProgram(o_name, resetParams);
}

void Pass::SetFragmentProgram(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	static_cast<Ogre::Pass*>(_native)->setFragmentProgram(o_name);
}

void Pass::SetFragmentProgramParameters(Mogre::GpuProgramParametersSharedPtr^ params)
{
	static_cast<Ogre::Pass*>(_native)->setFragmentProgramParameters((Ogre::GpuProgramParametersSharedPtr)params);
}

Mogre::GpuProgramParametersSharedPtr^ Pass::GetFragmentProgramParameters()
{
	return static_cast<const Ogre::Pass*>(_native)->getFragmentProgramParameters();
}

Mogre::GpuProgramPtr^ Pass::GetFragmentProgram()
{
	return static_cast<const Ogre::Pass*>(_native)->getFragmentProgram();
}

Mogre::Pass^ Pass::_split(unsigned short numUnits)
{
	return static_cast<Ogre::Pass*>(_native)->_split(numUnits);
}

void Pass::_notifyIndex(unsigned short index)
{
	static_cast<Ogre::Pass*>(_native)->_notifyIndex(index);
}

void Pass::_load()
{
	static_cast<Ogre::Pass*>(_native)->_load();
}

void Pass::_unload()
{
	static_cast<Ogre::Pass*>(_native)->_unload();
}

void Pass::_dirtyHash()
{
	static_cast<Ogre::Pass*>(_native)->_dirtyHash();
}

void Pass::_recalculateHash()
{
	static_cast<Ogre::Pass*>(_native)->_recalculateHash();
}

void Pass::_notifyNeedsRecompile()
{
	static_cast<Ogre::Pass*>(_native)->_notifyNeedsRecompile();
}

unsigned short Pass::_getTextureUnitWithContentTypeIndex(Mogre::TextureUnitState::ContentType contentType, unsigned short index)
{
	return static_cast<const Ogre::Pass*>(_native)->_getTextureUnitWithContentTypeIndex((Ogre::TextureUnitState::ContentType)contentType, index);
}

void Pass::SetTextureFiltering(Mogre::TextureFilterOptions filterType)
{
	static_cast<Ogre::Pass*>(_native)->setTextureFiltering((Ogre::TextureFilterOptions)filterType);
}

void Pass::SetTextureAnisotropy(unsigned int maxAniso)
{
	static_cast<Ogre::Pass*>(_native)->setTextureAnisotropy(maxAniso);
}

void Pass::QueueForDeletion()
{
	static_cast<Ogre::Pass*>(_native)->queueForDeletion();
}

void Pass::SetPassIterationCount(size_t count)
{
	static_cast<Ogre::Pass*>(_native)->setPassIterationCount(count);
}

bool Pass::ApplyTextureAliases(Mogre::Const_AliasTextureNamePairList^ aliasList, bool apply)
{
	return static_cast<const Ogre::Pass*>(_native)->applyTextureAliases(aliasList, apply);
}
bool Pass::ApplyTextureAliases(Mogre::Const_AliasTextureNamePairList^ aliasList)
{
	return static_cast<const Ogre::Pass*>(_native)->applyTextureAliases(aliasList);
}

Mogre::Pass::Const_PassSet^ Pass::GetDirtyHashList()
{
	return Ogre::Pass::getDirtyHashList();
}

Mogre::Pass::Const_PassSet^ Pass::GetPassGraveyard()
{
	return Ogre::Pass::getPassGraveyard();
}

void Pass::ClearDirtyHashList()
{
	Ogre::Pass::clearDirtyHashList();
}

void Pass::ProcessPendingPassUpdates()
{
	Ogre::Pass::processPendingPassUpdates();
}

void Pass::SetHashFunction(Mogre::Pass::BuiltinHashFunction builtin)
{
	Ogre::Pass::setHashFunction((Ogre::Pass::BuiltinHashFunction)builtin);
}


CPP_DECLARE_STLVECTOR(, IlluminationPassList, Mogre::IlluminationPass_NativePtr, Ogre::IlluminationPass*);

// --------------- Technique ---------------
CPP_DECLARE_ITERATOR_NOCONSTRUCTOR(Technique::, PassIterator, Ogre::Technique::PassIterator, Mogre::Technique::Passes, Mogre::Pass^, Ogre::Pass*);
CPP_DECLARE_ITERATOR(Technique::, IlluminationPassIterator, Ogre::Technique::IlluminationPassIterator, Mogre::IlluminationPassList, Mogre::IlluminationPass_NativePtr, Ogre::IlluminationPass*, );

Technique::Technique(Mogre::Material^ parent)
{
	_createdByCLR = true;
	_native = new Ogre::Technique(parent);
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

Technique::Technique(Mogre::Material^ parent, Mogre::Technique^ oth)
{
	_createdByCLR = true;
	_native = new Ogre::Technique(parent, oth);
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

Technique::~Technique()
{
	this->!Technique();
}

Technique::!Technique()
{
	OnDisposing(this, nullptr);

	if (IsDisposed)
		return;

	if (_createdByCLR && _native)
	{
		delete _native;
		_native = 0;
	}

	OnDisposed(this, nullptr);
}

bool Technique::HasColourWriteDisabled::get()
{
	return static_cast<const Ogre::Technique*>(_native)->hasColourWriteDisabled();
}

bool Technique::IsDepthCheckEnabled::get()
{
	return static_cast<const Ogre::Technique*>(_native)->isDepthCheckEnabled();
}

bool Technique::IsDepthWriteEnabled::get()
{
	return static_cast<const Ogre::Technique*>(_native)->isDepthWriteEnabled();
}

bool Technique::IsLoaded::get()
{
	return static_cast<const Ogre::Technique*>(_native)->isLoaded();
}

bool Technique::IsSupported::get()
{
	return static_cast<const Ogre::Technique*>(_native)->isSupported();
}

bool Technique::IsTransparent::get()
{
	return static_cast<const Ogre::Technique*>(_native)->isTransparent();
}

unsigned short Technique::LodIndex::get()
{
	return static_cast<const Ogre::Technique*>(_native)->getLodIndex();
}
void Technique::LodIndex::set(unsigned short index)
{
	static_cast<Ogre::Technique*>(_native)->setLodIndex(index);
}

String^ Technique::Name::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::Technique*>(_native)->getName());
}

void Technique::Name::set(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	static_cast<Ogre::Technique*>(_native)->setName(o_name);
}

unsigned short Technique::NumPasses::get()
{
	return static_cast<const Ogre::Technique*>(_native)->getNumPasses();
}

Mogre::Material^ Technique::Parent::get()
{
	return static_cast<const Ogre::Technique*>(_native)->getParent();
}

String^ Technique::ResourceGroup::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::Technique*>(_native)->getResourceGroup());
}

String^ Technique::SchemeName::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::Technique*>(_native)->getSchemeName());
}

void Technique::SchemeName::set(String^ schemeName)
{
	DECLARE_NATIVE_STRING(o_schemeName, schemeName);

	static_cast<Ogre::Technique*>(_native)->setSchemeName(o_schemeName);
}

String^ Technique::_compile(bool autoManageTextureUnits)
{
	return TO_CLR_STRING(static_cast<Ogre::Technique*>(_native)->_compile(autoManageTextureUnits));
}

void Technique::_compileIlluminationPasses()
{
	static_cast<Ogre::Technique*>(_native)->_compileIlluminationPasses();
}

Mogre::Pass^ Technique::CreatePass()
{
	return static_cast<Ogre::Technique*>(_native)->createPass();
}

Mogre::Pass^ Technique::GetPass(unsigned short index)
{
	return static_cast<Ogre::Technique*>(_native)->getPass(index);
}

Mogre::Pass^ Technique::GetPass(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return static_cast<Ogre::Technique*>(_native)->getPass(o_name);
}

void Technique::RemovePass(unsigned short index)
{
	static_cast<Ogre::Technique*>(_native)->removePass(index);
}

void Technique::RemoveAllPasses()
{
	static_cast<Ogre::Technique*>(_native)->removeAllPasses();
}

bool Technique::MovePass(unsigned short sourceIndex, unsigned short destinationIndex)
{
	return static_cast<Ogre::Technique*>(_native)->movePass(sourceIndex, destinationIndex);
}

Mogre::Technique::PassIterator^ Technique::GetPassIterator()
{
	return static_cast<Ogre::Technique*>(_native)->getPassIterator();
}

Mogre::Technique::IlluminationPassIterator^ Technique::GetIlluminationPassIterator()
{
	return static_cast<Ogre::Technique*>(_native)->getIlluminationPassIterator();
}


void Technique::_load()
{
	static_cast<Ogre::Technique*>(_native)->_load();
}

void Technique::_unload()
{
	static_cast<Ogre::Technique*>(_native)->_unload();
}

void Technique::_notifyNeedsRecompile()
{
	static_cast<Ogre::Technique*>(_native)->_notifyNeedsRecompile();
}

void Technique::SetPointSize(Mogre::Real ps)
{
	static_cast<Ogre::Technique*>(_native)->setPointSize(ps);
}

void Technique::SetAmbient(Mogre::Real red, Mogre::Real green, Mogre::Real blue)
{
	static_cast<Ogre::Technique*>(_native)->setAmbient(red, green, blue);
}

void Technique::SetAmbient(Mogre::ColourValue ambient)
{
	static_cast<Ogre::Technique*>(_native)->setAmbient(FromColor4(ambient));
}

void Technique::SetDiffuse(Mogre::Real red, Mogre::Real green, Mogre::Real blue, Mogre::Real alpha)
{
	static_cast<Ogre::Technique*>(_native)->setDiffuse(red, green, blue, alpha);
}

void Technique::SetDiffuse(Mogre::ColourValue diffuse)
{
	static_cast<Ogre::Technique*>(_native)->setDiffuse(FromColor4(diffuse));
}

void Technique::SetSpecular(Mogre::Real red, Mogre::Real green, Mogre::Real blue, Mogre::Real alpha)
{
	static_cast<Ogre::Technique*>(_native)->setSpecular(red, green, blue, alpha);
}

void Technique::SetSpecular(Mogre::ColourValue specular)
{
	static_cast<Ogre::Technique*>(_native)->setSpecular(FromColor4(specular));
}

void Technique::SetShininess(Mogre::Real val)
{
	static_cast<Ogre::Technique*>(_native)->setShininess(val);
}

void Technique::SetSelfIllumination(Mogre::Real red, Mogre::Real green, Mogre::Real blue)
{
	static_cast<Ogre::Technique*>(_native)->setSelfIllumination(red, green, blue);
}

void Technique::SetSelfIllumination(Mogre::ColourValue selfIllum)
{
	static_cast<Ogre::Technique*>(_native)->setSelfIllumination(FromColor4(selfIllum));
}

void Technique::SetDepthCheckEnabled(bool enabled)
{
	static_cast<Ogre::Technique*>(_native)->setDepthCheckEnabled(enabled);
}

void Technique::SetDepthWriteEnabled(bool enabled)
{
	static_cast<Ogre::Technique*>(_native)->setDepthWriteEnabled(enabled);
}

void Technique::SetDepthFunction(Mogre::CompareFunction func)
{
	static_cast<Ogre::Technique*>(_native)->setDepthFunction((Ogre::CompareFunction)func);
}

void Technique::SetColourWriteEnabled(bool enabled)
{
	static_cast<Ogre::Technique*>(_native)->setColourWriteEnabled(enabled);
}

void Technique::SetCullingMode(Mogre::CullingMode mode)
{
	static_cast<Ogre::Technique*>(_native)->setCullingMode((Ogre::CullingMode)mode);
}

void Technique::SetManualCullingMode(Mogre::ManualCullingMode mode)
{
	static_cast<Ogre::Technique*>(_native)->setManualCullingMode((Ogre::ManualCullingMode)mode);
}

void Technique::SetLightingEnabled(bool enabled)
{
	static_cast<Ogre::Technique*>(_native)->setLightingEnabled(enabled);
}

void Technique::SetShadingMode(Mogre::ShadeOptions mode)
{
	static_cast<Ogre::Technique*>(_native)->setShadingMode((Ogre::ShadeOptions)mode);
}

void Technique::SetFog(bool overrideScene, Mogre::FogMode mode, Mogre::ColourValue colour, Mogre::Real expDensity, Mogre::Real linearStart, Mogre::Real linearEnd)
{
	static_cast<Ogre::Technique*>(_native)->setFog(overrideScene, (Ogre::FogMode)mode, FromColor4(colour), expDensity, linearStart, linearEnd);
}

void Technique::SetFog(bool overrideScene, Mogre::FogMode mode, Mogre::ColourValue colour, Mogre::Real expDensity, Mogre::Real linearStart)
{
	static_cast<Ogre::Technique*>(_native)->setFog(overrideScene, (Ogre::FogMode)mode, FromColor4(colour), expDensity, linearStart);
}

void Technique::SetFog(bool overrideScene, Mogre::FogMode mode, Mogre::ColourValue colour, Mogre::Real expDensity)
{
	static_cast<Ogre::Technique*>(_native)->setFog(overrideScene, (Ogre::FogMode)mode, FromColor4(colour), expDensity);
}

void Technique::SetFog(bool overrideScene, Mogre::FogMode mode, Mogre::ColourValue colour)
{
	static_cast<Ogre::Technique*>(_native)->setFog(overrideScene, (Ogre::FogMode)mode, FromColor4(colour));
}

void Technique::SetFog(bool overrideScene, Mogre::FogMode mode)
{
	static_cast<Ogre::Technique*>(_native)->setFog(overrideScene, (Ogre::FogMode)mode);
}

void Technique::SetFog(bool overrideScene)
{
	static_cast<Ogre::Technique*>(_native)->setFog(overrideScene);
}

void Technique::SetDepthBias(float constantBias, float slopeScaleBias)
{
	static_cast<Ogre::Technique*>(_native)->setDepthBias(constantBias, slopeScaleBias);
}

void Technique::SetTextureFiltering(Mogre::TextureFilterOptions filterType)
{
	static_cast<Ogre::Technique*>(_native)->setTextureFiltering((Ogre::TextureFilterOptions)filterType);
}

void Technique::SetTextureAnisotropy(unsigned int maxAniso)
{
	static_cast<Ogre::Technique*>(_native)->setTextureAnisotropy(maxAniso);
}

void Technique::SetSceneBlending(Mogre::SceneBlendType sbt)
{
	static_cast<Ogre::Technique*>(_native)->setSceneBlending((Ogre::SceneBlendType)sbt);
}

void Technique::SetSceneBlending(Mogre::SceneBlendFactor sourceFactor, Mogre::SceneBlendFactor destFactor)
{
	static_cast<Ogre::Technique*>(_native)->setSceneBlending((Ogre::SceneBlendFactor)sourceFactor, (Ogre::SceneBlendFactor)destFactor);
}

unsigned short Technique::_getSchemeIndex()
{
	return static_cast<const Ogre::Technique*>(_native)->_getSchemeIndex();
}

bool Technique::ApplyTextureAliases(Mogre::Const_AliasTextureNamePairList^ aliasList, bool apply)
{
	return static_cast<const Ogre::Technique*>(_native)->applyTextureAliases(aliasList, apply);
}

bool Technique::ApplyTextureAliases(Mogre::Const_AliasTextureNamePairList^ aliasList)
{
	return static_cast<const Ogre::Technique*>(_native)->applyTextureAliases(aliasList);
}

// --------------- Material ---------------

//
//Material::Material(Mogre::ResourceManager^ creator, String^ name, Mogre::ResourceHandle handle, String^ group, bool isManual, Mogre::IManualResourceLoader^ loader) : Resource((CLRObject*)0)
//{
//	_createdByCLR = true;
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_group, group);
//
//	_native = new Ogre::Material(creator, o_name, handle, o_group, isManual, loader);
//	ObjectTable::Add((intptr_t)_native, this, nullptr);
//}

Material::Material(Mogre::ResourceManager^ creator, String^ name, Mogre::ResourceHandle handle, String^ group, bool isManual) : Resource((Ogre::Resource*)0)
{
	_createdByCLR = true;
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	_native = new Ogre::Material(creator, o_name, handle, o_group, isManual);
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

Material::Material(Mogre::ResourceManager^ creator, String^ name, Mogre::ResourceHandle handle, String^ group) : Resource((Ogre::Resource*)0)
{
	_createdByCLR = true;
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	_native = new Ogre::Material(creator, o_name, handle, o_group);
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

bool Material::CompilationRequired::get()
{
	return static_cast<const Ogre::Material*>(_native)->getCompilationRequired();
}

bool Material::IsTransparent::get()
{
	return static_cast<const Ogre::Material*>(_native)->isTransparent();
}

unsigned short Material::NumSupportedTechniques::get()
{
	return static_cast<const Ogre::Material*>(_native)->getNumSupportedTechniques();
}

unsigned short Material::NumTechniques::get()
{
	return static_cast<const Ogre::Material*>(_native)->getNumTechniques();
}

bool Material::ReceiveShadows::get()
{
	return static_cast<const Ogre::Material*>(_native)->getReceiveShadows();
}

void Material::ReceiveShadows::set(bool enabled)
{
	static_cast<Ogre::Material*>(_native)->setReceiveShadows(enabled);
}

bool Material::TransparencyCastsShadows::get()
{
	return static_cast<const Ogre::Material*>(_native)->getTransparencyCastsShadows();
}
void Material::TransparencyCastsShadows::set(bool enabled)
{
	static_cast<Ogre::Material*>(_native)->setTransparencyCastsShadows(enabled);
}

String^ Material::UnsupportedTechniquesExplanation::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::Material*>(_native)->getUnsupportedTechniquesExplanation());
}

Mogre::Technique^ Material::CreateTechnique()
{
	return static_cast<Ogre::Material*>(_native)->createTechnique();
}

Mogre::Technique^ Material::GetTechnique(unsigned short index)
{
	return static_cast<Ogre::Material*>(_native)->getTechnique(index);
}

Mogre::Technique^ Material::GetTechnique(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return static_cast<Ogre::Material*>(_native)->getTechnique(o_name);
}

void Material::RemoveTechnique(unsigned short index)
{
	static_cast<Ogre::Material*>(_native)->removeTechnique(index);
}

void Material::RemoveAllTechniques()
{
	static_cast<Ogre::Material*>(_native)->removeAllTechniques();
}

Mogre::Material::TechniqueIterator^ Material::GetTechniqueIterator()
{
	return static_cast<Ogre::Material*>(_native)->getTechniqueIterator();
}

Mogre::Material::TechniqueIterator^ Material::GetSupportedTechniqueIterator()
{
	return static_cast<Ogre::Material*>(_native)->getSupportedTechniqueIterator();
}

Mogre::Technique^ Material::GetSupportedTechnique(unsigned short index)
{
	return static_cast<Ogre::Material*>(_native)->getSupportedTechnique(index);
}

unsigned short Material::GetNumLodLevels(unsigned short schemeIndex)
{
	return static_cast<const Ogre::Material*>(_native)->getNumLodLevels(schemeIndex);
}

unsigned short Material::GetNumLodLevels(String^ schemeName)
{
	DECLARE_NATIVE_STRING(o_schemeName, schemeName);

	return static_cast<const Ogre::Material*>(_native)->getNumLodLevels(o_schemeName);
}

Mogre::Technique^ Material::GetBestTechnique(unsigned short lodIndex)
{
	return static_cast<Ogre::Material*>(_native)->getBestTechnique(lodIndex);
}
Mogre::Technique^ Material::GetBestTechnique()
{
	return static_cast<Ogre::Material*>(_native)->getBestTechnique();
}

Mogre::MaterialPtr^ Material::Clone(String^ newName, bool changeGroup, String^ newGroup)
{
	DECLARE_NATIVE_STRING(o_newName, newName);
	DECLARE_NATIVE_STRING(o_newGroup, newGroup);

	return static_cast<const Ogre::Material*>(_native)->clone(o_newName, changeGroup, o_newGroup);
}

Mogre::MaterialPtr^ Material::Clone(String^ newName, bool changeGroup)
{
	DECLARE_NATIVE_STRING(o_newName, newName);

	return static_cast<const Ogre::Material*>(_native)->clone(o_newName, changeGroup);
}

Mogre::MaterialPtr^ Material::Clone(String^ newName)
{
	DECLARE_NATIVE_STRING(o_newName, newName);

	return static_cast<const Ogre::Material*>(_native)->clone(o_newName);
}

void Material::CopyDetailsTo(Mogre::MaterialPtr^ mat)
{
	static_cast<const Ogre::Material*>(_native)->copyDetailsTo((Ogre::MaterialPtr&)mat);
}

void Material::Compile(bool autoManageTextureUnits)
{
	static_cast<Ogre::Material*>(_native)->compile(autoManageTextureUnits);
}

void Material::Compile()
{
	static_cast<Ogre::Material*>(_native)->compile();
}

void Material::SetPointSize(Mogre::Real ps)
{
	static_cast<Ogre::Material*>(_native)->setPointSize(ps);
}

void Material::SetAmbient(Mogre::Real red, Mogre::Real green, Mogre::Real blue)
{
	static_cast<Ogre::Material*>(_native)->setAmbient(red, green, blue);
}

void Material::SetAmbient(Mogre::ColourValue ambient)
{
	static_cast<Ogre::Material*>(_native)->setAmbient(FromColor4(ambient));
}

void Material::SetDiffuse(Mogre::Real red, Mogre::Real green, Mogre::Real blue, Mogre::Real alpha)
{
	static_cast<Ogre::Material*>(_native)->setDiffuse(red, green, blue, alpha);
}

void Material::SetDiffuse(Mogre::ColourValue diffuse)
{
	static_cast<Ogre::Material*>(_native)->setDiffuse(FromColor4(diffuse));
}

void Material::SetSpecular(Mogre::Real red, Mogre::Real green, Mogre::Real blue, Mogre::Real alpha)
{
	static_cast<Ogre::Material*>(_native)->setSpecular(red, green, blue, alpha);
}

void Material::SetSpecular(Mogre::ColourValue specular)
{
	static_cast<Ogre::Material*>(_native)->setSpecular(FromColor4(specular));
}

void Material::SetShininess(Mogre::Real val)
{
	static_cast<Ogre::Material*>(_native)->setShininess(val);
}

void Material::SetSelfIllumination(Mogre::Real red, Mogre::Real green, Mogre::Real blue)
{
	static_cast<Ogre::Material*>(_native)->setSelfIllumination(red, green, blue);
}

void Material::SetSelfIllumination(Mogre::ColourValue selfIllum)
{
	static_cast<Ogre::Material*>(_native)->setSelfIllumination(FromColor4(selfIllum));
}

void Material::SetDepthCheckEnabled(bool enabled)
{
	static_cast<Ogre::Material*>(_native)->setDepthCheckEnabled(enabled);
}

void Material::SetDepthWriteEnabled(bool enabled)
{
	static_cast<Ogre::Material*>(_native)->setDepthWriteEnabled(enabled);
}

void Material::SetDepthFunction(Mogre::CompareFunction func)
{
	static_cast<Ogre::Material*>(_native)->setDepthFunction((Ogre::CompareFunction)func);
}

void Material::SetColourWriteEnabled(bool enabled)
{
	static_cast<Ogre::Material*>(_native)->setColourWriteEnabled(enabled);
}

void Material::SetCullingMode(Mogre::CullingMode mode)
{
	static_cast<Ogre::Material*>(_native)->setCullingMode((Ogre::CullingMode)mode);
}

void Material::SetManualCullingMode(Mogre::ManualCullingMode mode)
{
	static_cast<Ogre::Material*>(_native)->setManualCullingMode((Ogre::ManualCullingMode)mode);
}

void Material::SetLightingEnabled(bool enabled)
{
	static_cast<Ogre::Material*>(_native)->setLightingEnabled(enabled);
}

void Material::SetShadingMode(Mogre::ShadeOptions mode)
{
	static_cast<Ogre::Material*>(_native)->setShadingMode((Ogre::ShadeOptions)mode);
}

void Material::SetFog(bool overrideScene, Mogre::FogMode mode, Mogre::ColourValue colour, Mogre::Real expDensity, Mogre::Real linearStart, Mogre::Real linearEnd)
{
	static_cast<Ogre::Material*>(_native)->setFog(overrideScene, (Ogre::FogMode)mode, FromColor4(colour), expDensity, linearStart, linearEnd);
}

void Material::SetFog(bool overrideScene, Mogre::FogMode mode, Mogre::ColourValue colour, Mogre::Real expDensity, Mogre::Real linearStart)
{
	static_cast<Ogre::Material*>(_native)->setFog(overrideScene, (Ogre::FogMode)mode, FromColor4(colour), expDensity, linearStart);
}

void Material::SetFog(bool overrideScene, Mogre::FogMode mode, Mogre::ColourValue colour, Mogre::Real expDensity)
{
	static_cast<Ogre::Material*>(_native)->setFog(overrideScene, (Ogre::FogMode)mode, FromColor4(colour), expDensity);
}

void Material::SetFog(bool overrideScene, Mogre::FogMode mode, Mogre::ColourValue colour)
{
	static_cast<Ogre::Material*>(_native)->setFog(overrideScene, (Ogre::FogMode)mode, FromColor4(colour));
}

void Material::SetFog(bool overrideScene, Mogre::FogMode mode)
{
	static_cast<Ogre::Material*>(_native)->setFog(overrideScene, (Ogre::FogMode)mode);
}

void Material::SetFog(bool overrideScene)
{
	static_cast<Ogre::Material*>(_native)->setFog(overrideScene);
}

void Material::SetDepthBias(float constantBias, float slopeScaleBias)
{
	static_cast<Ogre::Material*>(_native)->setDepthBias(constantBias, slopeScaleBias);
}

void Material::SetTextureFiltering(Mogre::TextureFilterOptions filterType)
{
	static_cast<Ogre::Material*>(_native)->setTextureFiltering((Ogre::TextureFilterOptions)filterType);
}

void Material::SetTextureAnisotropy(int maxAniso)
{
	static_cast<Ogre::Material*>(_native)->setTextureAnisotropy(maxAniso);
}

void Material::SetSceneBlending(Mogre::SceneBlendType sbt)
{
	static_cast<Ogre::Material*>(_native)->setSceneBlending((Ogre::SceneBlendType)sbt);
}

void Material::SetSceneBlending(Mogre::SceneBlendFactor sourceFactor, Mogre::SceneBlendFactor destFactor)
{
	static_cast<Ogre::Material*>(_native)->setSceneBlending((Ogre::SceneBlendFactor)sourceFactor, (Ogre::SceneBlendFactor)destFactor);
}

void Material::_notifyNeedsRecompile()
{
	static_cast<Ogre::Material*>(_native)->_notifyNeedsRecompile();
}

/*void Material::SetLodLevels(Mogre::Material::Const_LodDistanceList^ lodDistances)
{
	static_cast<Ogre::Material*>(_native)->setLodLevels(lodDistances);
}*/

void Material::Touch()
{
	static_cast<Ogre::Material*>(_native)->touch();
}

bool Material::ApplyTextureAliases(Mogre::Const_AliasTextureNamePairList^ aliasList, bool apply)
{
	return static_cast<const Ogre::Material*>(_native)->applyTextureAliases(aliasList, apply);
}
bool Material::ApplyTextureAliases(Mogre::Const_AliasTextureNamePairList^ aliasList)
{
	return static_cast<const Ogre::Material*>(_native)->applyTextureAliases(aliasList);
}

CPP_DECLARE_STLVECTOR(Material::, LodDistanceList, Mogre::Real, Ogre::Real);
CPP_DECLARE_ITERATOR_NOCONSTRUCTOR(Material::, TechniqueIterator, Ogre::Material::TechniqueIterator, Mogre::Material::Techniques, Mogre::Technique^, Ogre::Technique*);

// ----------------- MaterialManager -----------

MaterialManager::MaterialManager() : ResourceManager((Ogre::ResourceManager*) 0)
{
	_createdByCLR = true;
	_native = new Ogre::MaterialManager();
}

String^ MaterialManager::DEFAULT_SCHEME_NAME::get()
{
	return TO_CLR_STRING(Ogre::MaterialManager::DEFAULT_SCHEME_NAME);
}
void MaterialManager::DEFAULT_SCHEME_NAME::set(String^ value)
{
	DECLARE_NATIVE_STRING(o_value, value);

	Ogre::MaterialManager::DEFAULT_SCHEME_NAME = o_value;
}

String^ MaterialManager::ActiveScheme::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::MaterialManager*>(_native)->getActiveScheme());
}
void MaterialManager::ActiveScheme::set(String^ schemeName)
{
	DECLARE_NATIVE_STRING(o_schemeName, schemeName);

	static_cast<Ogre::MaterialManager*>(_native)->setActiveScheme(o_schemeName);
}

unsigned int MaterialManager::DefaultAnisotropy::get()
{
	return static_cast<const Ogre::MaterialManager*>(_native)->getDefaultAnisotropy();
}
void MaterialManager::DefaultAnisotropy::set(unsigned int maxAniso)
{
	static_cast<Ogre::MaterialManager*>(_native)->setDefaultAnisotropy(maxAniso);
}

void MaterialManager::Initialise()
{
	static_cast<Ogre::MaterialManager*>(_native)->initialise();
}

void MaterialManager::ParseScript(Mogre::DataStreamPtr^ stream, String^ groupName)
{
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	static_cast<Ogre::MaterialManager*>(_native)->parseScript((Ogre::DataStreamPtr&)stream, o_groupName);
}

void MaterialManager::SetDefaultTextureFiltering(Mogre::TextureFilterOptions fo)
{
	static_cast<Ogre::MaterialManager*>(_native)->setDefaultTextureFiltering((Ogre::TextureFilterOptions)fo);
}

void MaterialManager::SetDefaultTextureFiltering(Mogre::FilterType ftype, Mogre::FilterOptions opts)
{
	static_cast<Ogre::MaterialManager*>(_native)->setDefaultTextureFiltering((Ogre::FilterType)ftype, (Ogre::FilterOptions)opts);
}

void MaterialManager::SetDefaultTextureFiltering(Mogre::FilterOptions minFilter, Mogre::FilterOptions magFilter, Mogre::FilterOptions mipFilter)
{
	static_cast<Ogre::MaterialManager*>(_native)->setDefaultTextureFiltering((Ogre::FilterOptions)minFilter, (Ogre::FilterOptions)magFilter, (Ogre::FilterOptions)mipFilter);
}

Mogre::FilterOptions MaterialManager::GetDefaultTextureFiltering(Mogre::FilterType ftype)
{
	return (Mogre::FilterOptions)static_cast<const Ogre::MaterialManager*>(_native)->getDefaultTextureFiltering((Ogre::FilterType)ftype);
}

Mogre::MaterialPtr^ MaterialManager::GetDefaultSettings()
{
	return static_cast<const Ogre::MaterialManager*>(_native)->getDefaultSettings();
}

unsigned short MaterialManager::_getSchemeIndex(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return static_cast<Ogre::MaterialManager*>(_native)->_getSchemeIndex(o_name);
}

String^ MaterialManager::_getSchemeName(unsigned short index)
{
	return TO_CLR_STRING(static_cast<Ogre::MaterialManager*>(_native)->_getSchemeName(index));
}

unsigned short MaterialManager::_getActiveSchemeIndex()
{
	return static_cast<const Ogre::MaterialManager*>(_native)->_getActiveSchemeIndex();
}