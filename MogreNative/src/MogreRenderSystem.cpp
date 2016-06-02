#include "stdafx.h"
#include "MogreRenderSystem.h"
#include "MogreLog.h"
#include "MogreConfigFile.h"

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
	return ObjectTable::GetOrCreateObject<Mogre::RenderSystemCapabilities^>((intptr_t) _native->getCapabilities());
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
	return static_cast<Ogre::RenderSystem*>(_native)->getConfigOptions();
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
	static_cast<Ogre::RenderSystem*>(_native)->setCurrentPassIterationCount(count);
}

Ogre::RenderSystem* RenderSystem::UnmanagedPointer::get()
{
	return _native;
}

//CPP_DECLARE_STLVECTOR(, RenderSystemList, Mogre::RenderSystem^, Ogre::RenderSystem*);