#include "stdafx.h"
#include "MogreRenderSystem.h"
#include "MogreLog.h"
#include "MogreConfigFile.h"
#include "MogreTextureManager.h"
#include "MogreRenderTarget.h"
#include "MogreFrustum.h"
#include "MogreRenderOperation.h"

using namespace Mogre;

void RenderSystem_Listener_Director::eventOccurred(const Ogre::String& eventName, const Ogre::NameValuePairList* parameters)
{
	if (doCallForEventOccurred)
	{
		_receiver->EventOccurred(TO_CLR_STRING(eventName)/*, parameters*/);
	}
}

RenderSystemCapabilities::RenderSystemCapabilities()
{
	_createdByCLR = true;
	_native = new Ogre::RenderSystemCapabilities();
}

Ogre::ushort RenderSystemCapabilities::FragmentProgramConstantBoolCount::get()
{
	return static_cast<const Ogre::RenderSystemCapabilities*>(_native)->getFragmentProgramConstantBoolCount();
}
void RenderSystemCapabilities::FragmentProgramConstantBoolCount::set(Ogre::ushort c)
{
	static_cast<Ogre::RenderSystemCapabilities*>(_native)->setFragmentProgramConstantBoolCount(c);
}

Ogre::ushort RenderSystemCapabilities::FragmentProgramConstantFloatCount::get()
{
	return _native->getFragmentProgramConstantFloatCount();
}

void RenderSystemCapabilities::FragmentProgramConstantFloatCount::set(Ogre::ushort c)
{
	_native->setFragmentProgramConstantFloatCount(c);
}

Ogre::ushort RenderSystemCapabilities::FragmentProgramConstantIntCount::get()
{
	return static_cast<const Ogre::RenderSystemCapabilities*>(_native)->getFragmentProgramConstantIntCount();
}

void RenderSystemCapabilities::FragmentProgramConstantIntCount::set(Ogre::ushort c)
{
	static_cast<Ogre::RenderSystemCapabilities*>(_native)->setFragmentProgramConstantIntCount(c);
}

Ogre::Real RenderSystemCapabilities::MaxPointSize::get()
{
	return static_cast<const Ogre::RenderSystemCapabilities*>(_native)->getMaxPointSize();
}

void RenderSystemCapabilities::MaxPointSize::set(Ogre::Real s)
{
	static_cast<Ogre::RenderSystemCapabilities*>(_native)->setMaxPointSize(s);
}

bool RenderSystemCapabilities::NonPOW2TexturesLimited::get()
{
	return static_cast<const Ogre::RenderSystemCapabilities*>(_native)->getNonPOW2TexturesLimited();
}

void RenderSystemCapabilities::NonPOW2TexturesLimited::set(bool l)
{
	static_cast<Ogre::RenderSystemCapabilities*>(_native)->setNonPOW2TexturesLimited(l);
}

Ogre::ushort RenderSystemCapabilities::NumTextureUnits::get()
{
	return static_cast<const Ogre::RenderSystemCapabilities*>(_native)->getNumTextureUnits();
}

void RenderSystemCapabilities::NumTextureUnits::set(Ogre::ushort num)
{
	static_cast<Ogre::RenderSystemCapabilities*>(_native)->setNumTextureUnits(num);
}

Ogre::ushort RenderSystemCapabilities::NumVertexTextureUnits::get()
{
	return static_cast<const Ogre::RenderSystemCapabilities*>(_native)->getNumVertexTextureUnits();
}

void RenderSystemCapabilities::NumVertexTextureUnits::set(Ogre::ushort n)
{
	static_cast<Ogre::RenderSystemCapabilities*>(_native)->setNumVertexTextureUnits(n);
}

Ogre::ushort RenderSystemCapabilities::NumWorldMatricies::get()
{
	return static_cast<const Ogre::RenderSystemCapabilities*>(_native)->getNumWorldMatrices();
}

void RenderSystemCapabilities::NumWorldMatricies::set(Ogre::ushort num)
{
	static_cast<Ogre::RenderSystemCapabilities*>(_native)->setNumWorldMatrices(num);
}

Ogre::ushort RenderSystemCapabilities::StencilBufferBitDepth::get()
{
	return static_cast<const Ogre::RenderSystemCapabilities*>(_native)->getStencilBufferBitDepth();
}

void RenderSystemCapabilities::StencilBufferBitDepth::set(Ogre::ushort num)
{
	static_cast<Ogre::RenderSystemCapabilities*>(_native)->setStencilBufferBitDepth(num);
}

Ogre::ushort RenderSystemCapabilities::VertexProgramConstantBoolCount::get()
{
	return static_cast<const Ogre::RenderSystemCapabilities*>(_native)->getVertexProgramConstantBoolCount();
}
void RenderSystemCapabilities::VertexProgramConstantBoolCount::set(Ogre::ushort c)
{
	static_cast<Ogre::RenderSystemCapabilities*>(_native)->setVertexProgramConstantBoolCount(c);
}

Ogre::ushort RenderSystemCapabilities::VertexProgramConstantFloatCount::get()
{
	return static_cast<const Ogre::RenderSystemCapabilities*>(_native)->getVertexProgramConstantFloatCount();
}
void RenderSystemCapabilities::VertexProgramConstantFloatCount::set(Ogre::ushort c)
{
	static_cast<Ogre::RenderSystemCapabilities*>(_native)->setVertexProgramConstantFloatCount(c);
}

Ogre::ushort RenderSystemCapabilities::VertexProgramConstantIntCount::get()
{
	return static_cast<const Ogre::RenderSystemCapabilities*>(_native)->getVertexProgramConstantIntCount();
}

void RenderSystemCapabilities::VertexProgramConstantIntCount::set(Ogre::ushort c)
{
	static_cast<Ogre::RenderSystemCapabilities*>(_native)->setVertexProgramConstantIntCount(c);
}

bool RenderSystemCapabilities::VertexTextureUnitsShared::get()
{
	return static_cast<const Ogre::RenderSystemCapabilities*>(_native)->getVertexTextureUnitsShared();
}

void RenderSystemCapabilities::VertexTextureUnitsShared::set(bool shared)
{
	static_cast<Ogre::RenderSystemCapabilities*>(_native)->setVertexTextureUnitsShared(shared);
}

void RenderSystemCapabilities::SetNumVertexBlendMatrices(Ogre::ushort num)
{
	static_cast<Ogre::RenderSystemCapabilities*>(_native)->setNumVertexBlendMatrices(num);
}

void RenderSystemCapabilities::SetNumMultiRenderTargets(Ogre::ushort num)
{
	static_cast<Ogre::RenderSystemCapabilities*>(_native)->setNumMultiRenderTargets(num);
}

Ogre::ushort RenderSystemCapabilities::NumVertexBlendMatrices()
{
	return static_cast<const Ogre::RenderSystemCapabilities*>(_native)->getNumVertexBlendMatrices();
}

Ogre::ushort RenderSystemCapabilities::NumMultiRenderTargets()
{
	return static_cast<const Ogre::RenderSystemCapabilities*>(_native)->getNumMultiRenderTargets();
}

void RenderSystemCapabilities::SetCapability(Mogre::Capabilities c)
{
	static_cast<Ogre::RenderSystemCapabilities*>(_native)->setCapability((Ogre::Capabilities)c);
}

bool RenderSystemCapabilities::HasCapability(Mogre::Capabilities c)
{
	return static_cast<const Ogre::RenderSystemCapabilities*>(_native)->hasCapability((Ogre::Capabilities)c);
}

void RenderSystemCapabilities::Log(Mogre::Log^ pLog)
{
	static_cast<Ogre::RenderSystemCapabilities*>(_native)->log(GetPointerOrNull(pLog));
}

Mogre::RenderSystemCapabilities^ RenderSystem::Capabilities::get()
{
	return ObjectTable::GetOrCreateObject<Mogre::RenderSystemCapabilities^>((intptr_t)_native->getCapabilities());
}

Mogre::VertexElementType RenderSystem::ColourVertexElementType::get()
{
	return (Mogre::VertexElementType)static_cast<const Ogre::RenderSystem*>(_native)->getColourVertexElementType();
}

Ogre::Real RenderSystem::HorizontalTexelOffset::get()
{
	return static_cast<Ogre::RenderSystem*>(_native)->getHorizontalTexelOffset();
}

Ogre::Real RenderSystem::MaximumDepthInputValue::get()
{
	return static_cast<Ogre::RenderSystem*>(_native)->getMaximumDepthInputValue();
}

Ogre::Real RenderSystem::MinimumDepthInputValue::get()
{
	return static_cast<Ogre::RenderSystem*>(_native)->getMinimumDepthInputValue();
}

String^ RenderSystem::Name::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::RenderSystem*>(_native)->getName());
}

Ogre::Real RenderSystem::VerticalTexelOffset::get()
{
	return static_cast<Ogre::RenderSystem*>(_native)->getVerticalTexelOffset();
}

Mogre::ConfigOptionMap^ RenderSystem::GetConfigOptions()
{
	return _native->getConfigOptions();
}

void RenderSystem::SetConfigOption(String^ name, String^ value)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_value, value);

	_native->setConfigOption(o_name, o_value);
}

void RenderSystem::SetStencilCheckEnabled(bool enabled)
{
	static_cast<Ogre::RenderSystem*>(_native)->setStencilCheckEnabled(enabled);
}

void RenderSystem::SetStencilBufferParams(Mogre::CompareFunction func, Ogre::uint32 refValue, Ogre::uint32 compareMask, Ogre::uint32 writeMask, Mogre::StencilOperation stencilFailOp, Mogre::StencilOperation depthFailOp, Mogre::StencilOperation passOp, bool twoSidedOperation)
{
	static_cast<Ogre::RenderSystem*>(_native)->setStencilBufferParams((Ogre::CompareFunction)func, refValue, compareMask, writeMask, (Ogre::StencilOperation)stencilFailOp, (Ogre::StencilOperation)depthFailOp, (Ogre::StencilOperation)passOp, twoSidedOperation);
}

void RenderSystem::SetStencilBufferParams(Mogre::CompareFunction func, Ogre::uint32 refValue, Ogre::uint32 compareMask, Ogre::uint32 writeMask, Mogre::StencilOperation stencilFailOp, Mogre::StencilOperation depthFailOp, Mogre::StencilOperation passOp)
{
	static_cast<Ogre::RenderSystem*>(_native)->setStencilBufferParams((Ogre::CompareFunction)func, refValue, compareMask, writeMask, (Ogre::StencilOperation)stencilFailOp, (Ogre::StencilOperation)depthFailOp, (Ogre::StencilOperation)passOp);
}

void RenderSystem::SetStencilBufferParams(Mogre::CompareFunction func, Ogre::uint32 refValue, Ogre::uint32 compareMask, Ogre::uint32 writeMask, Mogre::StencilOperation stencilFailOp, Mogre::StencilOperation depthFailOp)
{
	static_cast<Ogre::RenderSystem*>(_native)->setStencilBufferParams((Ogre::CompareFunction)func, refValue, compareMask, writeMask, (Ogre::StencilOperation)stencilFailOp, (Ogre::StencilOperation)depthFailOp);
}

void RenderSystem::SetStencilBufferParams(Mogre::CompareFunction func, Ogre::uint32 refValue, Ogre::uint32 compareMask, Ogre::uint32 writeMask, Mogre::StencilOperation stencilFailOp)
{
	static_cast<Ogre::RenderSystem*>(_native)->setStencilBufferParams((Ogre::CompareFunction)func, refValue, compareMask, writeMask, (Ogre::StencilOperation)stencilFailOp);
}

void RenderSystem::SetStencilBufferParams(Mogre::CompareFunction func, Ogre::uint32 refValue, Ogre::uint32 compareMask, Ogre::uint32 writeMask)
{
	static_cast<Ogre::RenderSystem*>(_native)->setStencilBufferParams((Ogre::CompareFunction)func, refValue, compareMask, writeMask);
}

void RenderSystem::SetStencilBufferParams(Mogre::CompareFunction func, Ogre::uint32 refValue, Ogre::uint32 compareMask)
{
	static_cast<Ogre::RenderSystem*>(_native)->setStencilBufferParams((Ogre::CompareFunction)func, refValue, compareMask);
}

void RenderSystem::SetStencilBufferParams(Mogre::CompareFunction func, Ogre::uint32 refValue)
{
	static_cast<Ogre::RenderSystem*>(_native)->setStencilBufferParams((Ogre::CompareFunction)func, refValue);
}

void RenderSystem::SetStencilBufferParams(Mogre::CompareFunction func)
{
	static_cast<Ogre::RenderSystem*>(_native)->setStencilBufferParams((Ogre::CompareFunction)func);
}

void RenderSystem::SetStencilBufferParams()
{
	static_cast<Ogre::RenderSystem*>(_native)->setStencilBufferParams();
}

void RenderSystem::SetInvertVertexWinding(bool invert)
{
	static_cast<Ogre::RenderSystem*>(_native)->setInvertVertexWinding(invert);
}

void RenderSystem::SetScissorTest(bool enabled, size_t left, size_t top, size_t right, size_t bottom)
{
	static_cast<Ogre::RenderSystem*>(_native)->setScissorTest(enabled, left, top, right, bottom);
}
void RenderSystem::SetScissorTest(bool enabled, size_t left, size_t top, size_t right)
{
	static_cast<Ogre::RenderSystem*>(_native)->setScissorTest(enabled, left, top, right);
}
void RenderSystem::SetScissorTest(bool enabled, size_t left, size_t top)
{
	static_cast<Ogre::RenderSystem*>(_native)->setScissorTest(enabled, left, top);
}
void RenderSystem::SetScissorTest(bool enabled, size_t left)
{
	static_cast<Ogre::RenderSystem*>(_native)->setScissorTest(enabled, left);
}
void RenderSystem::SetScissorTest(bool enabled)
{
	static_cast<Ogre::RenderSystem*>(_native)->setScissorTest(enabled);
}

void RenderSystem::ClearFrameBuffer(unsigned int buffers, Mogre::ColourValue colour, Mogre::Real depth, unsigned short stencil)
{
	static_cast<Ogre::RenderSystem*>(_native)->clearFrameBuffer(buffers, FromColor4(colour), depth, stencil);
}
void RenderSystem::ClearFrameBuffer(unsigned int buffers, Mogre::ColourValue colour, Mogre::Real depth)
{
	static_cast<Ogre::RenderSystem*>(_native)->clearFrameBuffer(buffers, FromColor4(colour), depth);
}
void RenderSystem::ClearFrameBuffer(unsigned int buffers, Mogre::ColourValue colour)
{
	static_cast<Ogre::RenderSystem*>(_native)->clearFrameBuffer(buffers, FromColor4(colour));
}
void RenderSystem::ClearFrameBuffer(unsigned int buffers)
{
	static_cast<Ogre::RenderSystem*>(_native)->clearFrameBuffer(buffers);
}

void RenderSystem::SetCurrentPassIterationCount(size_t count)
{
	_native->setCurrentPassIterationCount(count);
}

void RenderSystem::ConvertColourValue(Mogre::ColourValue colour, [Out] Ogre::uint32% pDest)
{
	pin_ptr<Ogre::uint32> p_pDest = &pDest;

	_native->convertColourValue(FromColor4(colour), p_pDest);
}

void RenderSystem::BindGpuProgram(Mogre::GpuProgram^ prg)
{
	static_cast<Ogre::RenderSystem*>(_native)->bindGpuProgram(prg);
}

void RenderSystem::BindGpuProgramParameters(Mogre::GpuProgramType gptype, Mogre::GpuProgramParametersSharedPtr^ params, Ogre::uint16 variabilityMask)
{
	static_cast<Ogre::RenderSystem*>(_native)->bindGpuProgramParameters((Ogre::GpuProgramType)gptype, (Ogre::GpuProgramParametersSharedPtr)params, variabilityMask);
}

void RenderSystem::BindGpuProgramPassIterationParameters(Mogre::GpuProgramType gptype)
{
	static_cast<Ogre::RenderSystem*>(_native)->bindGpuProgramPassIterationParameters((Ogre::GpuProgramType)gptype);
}

void RenderSystem::UnbindGpuProgram(Mogre::GpuProgramType gptype)
{
	static_cast<Ogre::RenderSystem*>(_native)->unbindGpuProgram((Ogre::GpuProgramType)gptype);
}

bool RenderSystem::IsGpuProgramBound(Mogre::GpuProgramType gptype)
{
	return static_cast<Ogre::RenderSystem*>(_native)->isGpuProgramBound((Ogre::GpuProgramType)gptype);
}

void RenderSystem::_convertProjectionMatrix(Mogre::Matrix4^ matrix, Mogre::Matrix4^ dest, bool forGpuProgram)
{
	pin_ptr<Ogre::Matrix4> p_matrix = interior_ptr<Ogre::Matrix4>(&matrix->m00);
	pin_ptr<Ogre::Matrix4> p_dest = interior_ptr<Ogre::Matrix4>(&dest->m00);

	static_cast<Ogre::RenderSystem*>(_native)->_convertProjectionMatrix(*p_matrix, *p_dest, forGpuProgram);
}
void RenderSystem::_convertProjectionMatrix(Mogre::Matrix4^ matrix, Mogre::Matrix4^ dest)
{
	pin_ptr<Ogre::Matrix4> p_matrix = interior_ptr<Ogre::Matrix4>(&matrix->m00);
	pin_ptr<Ogre::Matrix4> p_dest = interior_ptr<Ogre::Matrix4>(&dest->m00);

	static_cast<Ogre::RenderSystem*>(_native)->_convertProjectionMatrix(*p_matrix, *p_dest);
}

void RenderSystem::_makeProjectionMatrix(Mogre::Radian fovy, Mogre::Real aspect, Mogre::Real nearPlane, Mogre::Real farPlane, Mogre::Matrix4^ dest, bool forGpuProgram)
{
	pin_ptr<Ogre::Matrix4> p_dest = interior_ptr<Ogre::Matrix4>(&dest->m00);

	static_cast<Ogre::RenderSystem*>(_native)->_makeProjectionMatrix(Ogre::Radian(fovy.ValueRadians), aspect, nearPlane, farPlane, *p_dest, forGpuProgram);
}

void RenderSystem::_makeProjectionMatrix(Mogre::Radian fovy, Mogre::Real aspect, Mogre::Real nearPlane, Mogre::Real farPlane, Mogre::Matrix4^ dest)
{
	pin_ptr<Ogre::Matrix4> p_dest = interior_ptr<Ogre::Matrix4>(&dest->m00);

	static_cast<Ogre::RenderSystem*>(_native)->_makeProjectionMatrix(Ogre::Radian(fovy.ValueRadians), aspect, nearPlane, farPlane, *p_dest);
}

void RenderSystem::_makeProjectionMatrix(Mogre::Real left, Mogre::Real right, Mogre::Real bottom, Mogre::Real top, Mogre::Real nearPlane, Mogre::Real farPlane, Mogre::Matrix4^ dest, bool forGpuProgram)
{
	pin_ptr<Ogre::Matrix4> p_dest = interior_ptr<Ogre::Matrix4>(&dest->m00);

	static_cast<Ogre::RenderSystem*>(_native)->_makeProjectionMatrix(left, right, bottom, top, nearPlane, farPlane, *p_dest, forGpuProgram);
}
void RenderSystem::_makeProjectionMatrix(Mogre::Real left, Mogre::Real right, Mogre::Real bottom, Mogre::Real top, Mogre::Real nearPlane, Mogre::Real farPlane, Mogre::Matrix4^ dest)
{
	pin_ptr<Ogre::Matrix4> p_dest = interior_ptr<Ogre::Matrix4>(&dest->m00);

	static_cast<Ogre::RenderSystem*>(_native)->_makeProjectionMatrix(left, right, bottom, top, nearPlane, farPlane, *p_dest);
}

void RenderSystem::_makeOrthoMatrix(Mogre::Radian fovy, Mogre::Real aspect, Mogre::Real nearPlane, Mogre::Real farPlane, Mogre::Matrix4^ dest, bool forGpuProgram)
{
	pin_ptr<Ogre::Matrix4> p_dest = interior_ptr<Ogre::Matrix4>(&dest->m00);

	static_cast<Ogre::RenderSystem*>(_native)->_makeOrthoMatrix(Ogre::Radian(fovy.ValueRadians), aspect, nearPlane, farPlane, *p_dest, forGpuProgram);
}
void RenderSystem::_makeOrthoMatrix(Mogre::Radian fovy, Mogre::Real aspect, Mogre::Real nearPlane, Mogre::Real farPlane, Mogre::Matrix4^ dest)
{
	pin_ptr<Ogre::Matrix4> p_dest = interior_ptr<Ogre::Matrix4>(&dest->m00);

	static_cast<Ogre::RenderSystem*>(_native)->_makeOrthoMatrix(Ogre::Radian(fovy.ValueRadians), aspect, nearPlane, farPlane, *p_dest);
}

void RenderSystem::_setWorldMatrix(Mogre::Matrix4^ m)
{
	pin_ptr<Ogre::Matrix4> p_m = interior_ptr<Ogre::Matrix4>(&m->m00);

	static_cast<Ogre::RenderSystem*>(_native)->_setWorldMatrix(*p_m);
}

void RenderSystem::_setWorldMatrices(const Mogre::Matrix4* m, unsigned short count)
{
	const Ogre::Matrix4* o_m = reinterpret_cast<const Ogre::Matrix4*>(m);

	_native->_setWorldMatrices(o_m, count);
}

void RenderSystem::_setViewMatrix(Mogre::Matrix4^ m)
{
	pin_ptr<Ogre::Matrix4> p_m = interior_ptr<Ogre::Matrix4>(&m->m00);

	_native->_setViewMatrix(*p_m);
}

void RenderSystem::_setProjectionMatrix(Mogre::Matrix4^ m)
{
	pin_ptr<Ogre::Matrix4> p_m = interior_ptr<Ogre::Matrix4>(&m->m00);

	_native->_setProjectionMatrix(*p_m);
}

void RenderSystem::_setTextureUnitSettings(size_t texUnit, Mogre::TextureUnitState^ tl)
{
	_native->_setTextureUnitSettings(texUnit, tl);
}

void RenderSystem::_setBindingType(Mogre::TextureUnitState::BindingType bindigType)
{
	_native->_setBindingType((Ogre::TextureUnitState::BindingType)bindigType);
}

void RenderSystem::_disableTextureUnit(size_t texUnit)
{
	_native->_disableTextureUnit(texUnit);
}

void RenderSystem::_disableTextureUnitsFrom(size_t texUnit)
{
	_native->_disableTextureUnitsFrom(texUnit);
}

void RenderSystem::_setTexture(size_t unit, bool enabled, TexturePtr^ texPtr)
{
	_native->_setTexture(unit, enabled, texPtr);
}
void RenderSystem::_setTexture(size_t unit, bool enabled, String^ texName)
{
	DECLARE_NATIVE_STRING(o_texName, texName);
	_native->_setTexture(unit, enabled, o_texName);
}

void RenderSystem::_setVertexTexture(size_t unit, TexturePtr^ tex)
{
	_native->_setVertexTexture(unit, tex);
}
void RenderSystem::_setGeometryTexture(size_t unit, TexturePtr^ tex)
{
	_native->_setGeometryTexture(unit, tex);
}
void RenderSystem::_setComputeTexture(size_t unit, TexturePtr^ tex)
{
	_native->_setComputeTexture(unit, tex);
}

void RenderSystem::_setTessellationHullTexture(size_t unit, TexturePtr^ tex)
{
	_native->_setTessellationHullTexture(unit, tex);
}

void RenderSystem::_setTessellationDomainTexture(size_t unit, TexturePtr^ tex)
{
	_native->_setTessellationDomainTexture(unit, tex);
}

void RenderSystem::_setTextureCoordSet(size_t unit, size_t index)
{
	static_cast<Ogre::RenderSystem*>(_native)->_setTextureCoordSet(unit, index);
}

void RenderSystem::_setTextureCoordCalculation(size_t unit, Mogre::TexCoordCalcMethod m, Mogre::Frustum^ frustum)
{
	static_cast<Ogre::RenderSystem*>(_native)->_setTextureCoordCalculation(unit, (Ogre::TexCoordCalcMethod)m, frustum);
}
void RenderSystem::_setTextureCoordCalculation(size_t unit, Mogre::TexCoordCalcMethod m)
{
	static_cast<Ogre::RenderSystem*>(_native)->_setTextureCoordCalculation(unit, (Ogre::TexCoordCalcMethod)m);
}

void RenderSystem::_setTextureBlendMode(size_t unit, Mogre::LayerBlendModeEx_NativePtr bm)
{
	static_cast<Ogre::RenderSystem*>(_native)->_setTextureBlendMode(unit, bm);
}

void RenderSystem::_setTextureUnitFiltering(size_t unit, Mogre::FilterOptions minFilter, Mogre::FilterOptions magFilter, Mogre::FilterOptions mipFilter)
{
	static_cast<Ogre::RenderSystem*>(_native)->_setTextureUnitFiltering(unit, (Ogre::FilterOptions)minFilter, (Ogre::FilterOptions)magFilter, (Ogre::FilterOptions)mipFilter);
}

void RenderSystem::_setTextureUnitFiltering(size_t unit, Mogre::FilterType ftype, Mogre::FilterOptions filter)
{
	static_cast<Ogre::RenderSystem*>(_native)->_setTextureUnitFiltering(unit, (Ogre::FilterType)ftype, (Ogre::FilterOptions)filter);
}

void RenderSystem::_setTextureLayerAnisotropy(size_t unit, unsigned int maxAnisotropy)
{
	static_cast<Ogre::RenderSystem*>(_native)->_setTextureLayerAnisotropy(unit, maxAnisotropy);
}

void RenderSystem::_setTextureAddressingMode(size_t unit, Mogre::TextureUnitState::UVWAddressingMode uvw)
{
	static_cast<Ogre::RenderSystem*>(_native)->_setTextureAddressingMode(unit, uvw);
}

void RenderSystem::_setTextureBorderColour(size_t unit, Mogre::ColourValue colour)
{
	static_cast<Ogre::RenderSystem*>(_native)->_setTextureBorderColour(unit, FromColor4(colour));
}

void RenderSystem::_setTextureMipmapBias(size_t unit, float bias)
{
	static_cast<Ogre::RenderSystem*>(_native)->_setTextureMipmapBias(unit, bias);
}

void RenderSystem::_setTextureMatrix(size_t unit, Mogre::Matrix4^ xform)
{
	pin_ptr<Ogre::Matrix4> p_xform = interior_ptr<Ogre::Matrix4>(&xform->m00);

	static_cast<Ogre::RenderSystem*>(_native)->_setTextureMatrix(unit, *p_xform);
}

void RenderSystem::_setSceneBlending(Mogre::SceneBlendFactor sourceFactor, Mogre::SceneBlendFactor destFactor)
{
	static_cast<Ogre::RenderSystem*>(_native)->_setSceneBlending((Ogre::SceneBlendFactor)sourceFactor, (Ogre::SceneBlendFactor)destFactor);
}

void RenderSystem::_setAlphaRejectSettings(Mogre::CompareFunction func, unsigned char value, bool alphaToCoverage)
{
	static_cast<Ogre::RenderSystem*>(_native)->_setAlphaRejectSettings((Ogre::CompareFunction)func, value, alphaToCoverage);
}

void RenderSystem::_setRenderTarget(RenderTarget^ target)
{
	_native->_setRenderTarget(target);
}

void RenderSystem::_render(Mogre::RenderOperation^ op)
{
	_native->_render(op);
}

Ogre::RenderSystem* RenderSystem::UnmanagedPointer::get()
{
	return _native;
}
