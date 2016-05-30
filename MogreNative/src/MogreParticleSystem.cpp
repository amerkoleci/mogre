#include "stdafx.h"
#include "MogreParticleSystem.h"
#include "Marshalling.h"

using namespace Mogre;

// ParticleEmitter
Mogre::Radian ParticleEmitter::Angle::get()
{
	return Mogre::Radian(_native->getAngle().valueRadians());
}

void ParticleEmitter::Angle::set(Mogre::Radian angle)
{
	_native->setAngle(Ogre::Radian(angle.ValueRadians));
}

Mogre::ColourValue ParticleEmitter::Colour::get()
{
	return ToColor4(_native->getColour());
}

void ParticleEmitter::Colour::set(Mogre::ColourValue colour)
{
	_native->setColour(FromColor4(colour));
}

Mogre::ColourValue ParticleEmitter::ColourRangeEnd::get()
{
	return ToColor4(_native->getColourRangeEnd());
}

void ParticleEmitter::ColourRangeEnd::set(Mogre::ColourValue colour)
{
	_native->setColourRangeEnd(FromColor4(colour));
}

Mogre::ColourValue ParticleEmitter::ColourRangeStart::get()
{
	return ToColor4(_native->getColourRangeStart());
}

void ParticleEmitter::ColourRangeStart::set(Mogre::ColourValue colour)
{
	_native->setColourRangeStart(FromColor4(colour));
}

Mogre::Vector3 ParticleEmitter::Direction::get()
{
	return ToVector3(_native->getDirection());
}

void ParticleEmitter::Direction::set(Mogre::Vector3 direction)
{
	_native->setDirection(FromVector3(direction));
}

Mogre::Real ParticleEmitter::Duration::get()
{
	return _native->getDuration();
}
void ParticleEmitter::Duration::set(Mogre::Real duration)
{
	_native->setDuration(duration);
}

Mogre::Real ParticleEmitter::EmissionRate::get()
{
	return _native->getEmissionRate();
}

void ParticleEmitter::EmissionRate::set(Mogre::Real particlesPerSecond)
{
	_native->setEmissionRate(particlesPerSecond);
}

String^ ParticleEmitter::EmittedEmitter::get()
{
	return TO_CLR_STRING(_native->getEmittedEmitter());
}

void ParticleEmitter::EmittedEmitter::set(String^ emittedEmitter)
{
	DECLARE_NATIVE_STRING(o_emittedEmitter, emittedEmitter);

	_native->setEmittedEmitter(o_emittedEmitter);
}

bool ParticleEmitter::Enabled::get()
{
	return _native->getEnabled();
}

void ParticleEmitter::Enabled::set(bool enabled)
{
	_native->setEnabled(enabled);
}

bool ParticleEmitter::IsEmitted::get()
{
	return _native->isEmitted();
}

Mogre::Real ParticleEmitter::MaxDuration::get()
{
	return _native->getMaxDuration();
}
void ParticleEmitter::MaxDuration::set(Mogre::Real max)
{
	_native->setMaxDuration(max);
}

Mogre::Real ParticleEmitter::MaxParticleVelocity::get()
{
	return _native->getMaxParticleVelocity();
}
void ParticleEmitter::MaxParticleVelocity::set(Mogre::Real max)
{
	_native->setMaxParticleVelocity(max);
}

Mogre::Real ParticleEmitter::MaxRepeatDelay::get()
{
	return _native->getMaxRepeatDelay();
}
void ParticleEmitter::MaxRepeatDelay::set(Mogre::Real max)
{
	_native->setMaxRepeatDelay(max);
}

Mogre::Real ParticleEmitter::MaxTimeToLive::get()
{
	return _native->getMaxTimeToLive();
}
void ParticleEmitter::MaxTimeToLive::set(Mogre::Real max)
{
	_native->setMaxTimeToLive(max);
}

Mogre::Real ParticleEmitter::MinDuration::get()
{
	return _native->getMinDuration();
}
void ParticleEmitter::MinDuration::set(Mogre::Real min)
{
	_native->setMinDuration(min);
}

Mogre::Real ParticleEmitter::MinParticleVelocity::get()
{
	return _native->getMinParticleVelocity();
}
void ParticleEmitter::MinParticleVelocity::set(Mogre::Real min)
{
	_native->setMinParticleVelocity(min);
}

Mogre::Real ParticleEmitter::MinRepeatDelay::get()
{
	return _native->getMinRepeatDelay();
}
void ParticleEmitter::MinRepeatDelay::set(Mogre::Real min)
{
	_native->setMinRepeatDelay(min);
}

Mogre::Real ParticleEmitter::MinTimeToLive::get()
{
	return _native->getMinTimeToLive();
}
void ParticleEmitter::MinTimeToLive::set(Mogre::Real min)
{
	_native->setMinTimeToLive(min);
}

String^ ParticleEmitter::Name::get()
{
	return TO_CLR_STRING(_native->getName());
}

void ParticleEmitter::Name::set(String^ newName)
{
	DECLARE_NATIVE_STRING(o_newName, newName);

	_native->setName(o_newName);
}

Mogre::Real ParticleEmitter::ParticleVelocity::get()
{
	return _native->getParticleVelocity();
}
void ParticleEmitter::ParticleVelocity::set(Mogre::Real speed)
{
	_native->setParticleVelocity(speed);
}

Mogre::Vector3 ParticleEmitter::Position::get()
{
	return ToVector3(_native->getPosition());
}

void ParticleEmitter::Position::set(Mogre::Vector3 pos)
{
	_native->setPosition(FromVector3(pos));
}

Mogre::Real ParticleEmitter::RepeatDelay::get()
{
	return _native->getRepeatDelay();
}
void ParticleEmitter::RepeatDelay::set(Mogre::Real duration)
{
	_native->setRepeatDelay(duration);
}

Mogre::Real ParticleEmitter::StartTime::get()
{
	return _native->getStartTime();
}
void ParticleEmitter::StartTime::set(Mogre::Real startTime)
{
	_native->setStartTime(startTime);
}

Mogre::Real ParticleEmitter::TimeToLive::get()
{
	return _native->getTimeToLive();
}
void ParticleEmitter::TimeToLive::set(Mogre::Real ttl)
{
	_native->setTimeToLive(ttl);
}

String^ ParticleEmitter::Type::get()
{
	return TO_CLR_STRING(_native->getType());
}

void ParticleEmitter::SetParticleVelocity(Mogre::Real min, Mogre::Real max)
{
	_native->setParticleVelocity(min, max);
}

void ParticleEmitter::SetTimeToLive(Mogre::Real minTtl, Mogre::Real maxTtl)
{
	_native->setTimeToLive(minTtl, maxTtl);
}

void ParticleEmitter::SetColour(Mogre::ColourValue colourStart, Mogre::ColourValue colourEnd)
{
	_native->setColour(FromColor4(colourStart), FromColor4(colourEnd));
}

//unsigned short ParticleEmitter::_getEmissionCount(Mogre::Real timeElapsed)
//{
//	return _native->_getEmissionCount(timeElapsed);
//}
//
//void ParticleEmitter::_initParticle(Mogre::Particle^ pParticle)
//{
//	_native->_initParticle(pParticle);
//}

void ParticleEmitter::SetDuration(Mogre::Real min, Mogre::Real max)
{
	_native->setDuration(min, max);
}

void ParticleEmitter::SetRepeatDelay(Mogre::Real min, Mogre::Real max)
{
	_native->setRepeatDelay(min, max);
}

void ParticleEmitter::SetEmitted(bool emitted)
{
	_native->setEmitted(emitted);
}

//Mogre::Const_ParameterList^ ParticleEmitter::GetParameters()
//{
//	return _native->getParameters();
//}

bool ParticleEmitter::SetParameter(String^ name, String^ value)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_value, value);

	return _native->setParameter(o_name, o_value);
}

void ParticleEmitter::SetParameterList(Mogre::Const_NameValuePairList^ paramList)
{
	_native->setParameterList(paramList);
}

String^ ParticleEmitter::GetParameter(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return TO_CLR_STRING(_native->getParameter(o_name));
}

Ogre::ParticleEmitter* ParticleEmitter::UnmanagedPointer::get()
{
	return _native;
}

// ParticleSystemRenderer
String^ ParticleSystemRenderer::Type::get()
{
	return TO_CLR_STRING(_native->getType());
}

bool ParticleSystemRenderer::SetParameter(String^ name, String^ value)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_value, value);

	return _native->setParameter(o_name, o_value);
}

String^ ParticleSystemRenderer::GetParameter(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return TO_CLR_STRING(_native->getParameter(o_name));
}

void ParticleSystemRenderer::SetParameterList(Mogre::Const_NameValuePairList^ paramList)
{
	_native->setParameterList(paramList);
}

Ogre::ParticleSystemRenderer* ParticleSystemRenderer::UnmanagedPointer::get()
{
	return _native;
}

// ParticleSystem

bool ParticleSystem::CullIndividually::get()
{
	return static_cast<const Ogre::ParticleSystem*>(_native)->getCullIndividually();
}

void ParticleSystem::CullIndividually::set(bool cullIndividual)
{
	static_cast<Ogre::ParticleSystem*>(_native)->setCullIndividually(cullIndividual);
}

Ogre::Real ParticleSystem::DefaultHeight::get()
{
	return static_cast<const Ogre::ParticleSystem*>(_native)->getDefaultHeight();
}

void ParticleSystem::DefaultHeight::set(Ogre::Real height)
{
	static_cast<Ogre::ParticleSystem*>(_native)->setDefaultHeight(height);
}

Ogre::Real ParticleSystem::DefaultIterationInterval::get()
{
	return Ogre::ParticleSystem::getDefaultIterationInterval();
}

void ParticleSystem::DefaultIterationInterval::set(Ogre::Real iterationInterval)
{
	Ogre::ParticleSystem::setDefaultIterationInterval(iterationInterval);
}

Ogre::Real ParticleSystem::DefaultNonVisibleUpdateTimeout::get()
{
	return Ogre::ParticleSystem::getDefaultNonVisibleUpdateTimeout();
}

void ParticleSystem::DefaultNonVisibleUpdateTimeout::set(Ogre::Real timeout)
{
	Ogre::ParticleSystem::setDefaultNonVisibleUpdateTimeout(timeout);
}

Ogre::Real ParticleSystem::DefaultWidth::get()
{
	return static_cast<const Ogre::ParticleSystem*>(_native)->getDefaultWidth();
}

void ParticleSystem::DefaultWidth::set(Ogre::Real width)
{
	static_cast<Ogre::ParticleSystem*>(_native)->setDefaultWidth(width);
}

size_t ParticleSystem::EmittedEmitterQuota::get()
{
	return static_cast<const Ogre::ParticleSystem*>(_native)->getEmittedEmitterQuota();
}
void ParticleSystem::EmittedEmitterQuota::set(size_t quota)
{
	static_cast<Ogre::ParticleSystem*>(_native)->setEmittedEmitterQuota(quota);
}

Ogre::Real ParticleSystem::IterationInterval::get()
{
	return static_cast<const Ogre::ParticleSystem*>(_native)->getIterationInterval();
}

void ParticleSystem::IterationInterval::set(Ogre::Real iterationInterval)
{
	static_cast<Ogre::ParticleSystem*>(_native)->setIterationInterval(iterationInterval);
}

bool ParticleSystem::KeepParticlesInLocalSpace::get()
{
	return static_cast<const Ogre::ParticleSystem*>(_native)->getKeepParticlesInLocalSpace();
}

void ParticleSystem::KeepParticlesInLocalSpace::set(bool keepLocal)
{
	static_cast<Ogre::ParticleSystem*>(_native)->setKeepParticlesInLocalSpace(keepLocal);
}

String^ ParticleSystem::MaterialName::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::ParticleSystem*>(_native)->getMaterialName());
}

void ParticleSystem::MaterialName::set(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	static_cast<Ogre::ParticleSystem*>(_native)->setMaterialName(o_name);
}

String^ ParticleSystem::MovableType::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::ParticleSystem*>(_native)->getMovableType());
}

Ogre::Real ParticleSystem::NonVisibleUpdateTimeout::get()
{
	return static_cast<const Ogre::ParticleSystem*>(_native)->getNonVisibleUpdateTimeout();
}

void ParticleSystem::NonVisibleUpdateTimeout::set(Ogre::Real timeout)
{
	static_cast<Ogre::ParticleSystem*>(_native)->setNonVisibleUpdateTimeout(timeout);
}

unsigned short ParticleSystem::NumAffectors::get()
{
	return static_cast<const Ogre::ParticleSystem*>(_native)->getNumAffectors();
}

unsigned short ParticleSystem::NumEmitters::get()
{
	return static_cast<const Ogre::ParticleSystem*>(_native)->getNumEmitters();
}

size_t ParticleSystem::NumParticles::get()
{
	return static_cast<const Ogre::ParticleSystem*>(_native)->getNumParticles();
}

String^ ParticleSystem::Origin::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::ParticleSystem*>(_native)->getOrigin());
}

size_t ParticleSystem::ParticleQuota::get()
{
	return static_cast<const Ogre::ParticleSystem*>(_native)->getParticleQuota();
}
void ParticleSystem::ParticleQuota::set(size_t quota)
{
	static_cast<Ogre::ParticleSystem*>(_native)->setParticleQuota(quota);
}

Mogre::ParticleSystemRenderer^ ParticleSystem::Renderer::get()
{
	return ObjectTable::GetOrCreateObject<Mogre::ParticleSystemRenderer^>((intptr_t)
		static_cast<const Ogre::ParticleSystem*>(_native)->getRenderer()
		);
}

String^ ParticleSystem::RendererName::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::ParticleSystem*>(_native)->getRendererName());
}

Ogre::uint8 ParticleSystem::RenderQueueGroup::get()
{
	return static_cast<const Ogre::ParticleSystem*>(_native)->getRenderQueueGroup();
}

void ParticleSystem::RenderQueueGroup::set(Ogre::uint8 queueID)
{
	static_cast<Ogre::ParticleSystem*>(_native)->setRenderQueueGroup(queueID);
}

String^ ParticleSystem::ResourceGroupName::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::ParticleSystem*>(_native)->getResourceGroupName());
}

bool ParticleSystem::SortingEnabled::get()
{
	return static_cast<const Ogre::ParticleSystem*>(_native)->getSortingEnabled();
}
void ParticleSystem::SortingEnabled::set(bool enabled)
{
	static_cast<Ogre::ParticleSystem*>(_native)->setSortingEnabled(enabled);
}

Ogre::Real ParticleSystem::SpeedFactor::get()
{
	return static_cast<const Ogre::ParticleSystem*>(_native)->getSpeedFactor();
}

void ParticleSystem::SpeedFactor::set(Ogre::Real speedFactor)
{
	static_cast<Ogre::ParticleSystem*>(_native)->setSpeedFactor(speedFactor);
}

void ParticleSystem::SetRenderer(String^ typeName)
{
	DECLARE_NATIVE_STRING(o_typeName, typeName);

	static_cast<Ogre::ParticleSystem*>(_native)->setRenderer(o_typeName);
}

Mogre::ParticleEmitter^ ParticleSystem::AddEmitter(String^ emitterType)
{
	DECLARE_NATIVE_STRING(o_emitterType, emitterType);

	return ObjectTable::GetOrCreateObject<Mogre::ParticleEmitter^>((intptr_t)
		static_cast<Ogre::ParticleSystem*>(_native)->addEmitter(o_emitterType)
		);
}

Mogre::ParticleEmitter^ ParticleSystem::GetEmitter(unsigned short index)
{
	return ObjectTable::GetOrCreateObject<Mogre::ParticleEmitter^>((intptr_t)
		static_cast<Ogre::ParticleSystem*>(_native)->getEmitter(index)
		);
}

void ParticleSystem::RemoveEmitter(unsigned short index)
{
	static_cast<Ogre::ParticleSystem*>(_native)->removeEmitter(index);
}

void ParticleSystem::RemoveEmitter(Mogre::ParticleEmitter^ emitter)
{
	static_cast<Ogre::ParticleSystem*>(_native)->removeEmitter(GetPointerOrNull(emitter));
}

void ParticleSystem::RemoveAllEmitters()
{
	static_cast<Ogre::ParticleSystem*>(_native)->removeAllEmitters();
}

void ParticleSystem::Clear()
{
	static_cast<Ogre::ParticleSystem*>(_native)->clear();
}

void ParticleSystem::FastForward(Mogre::Real time, Mogre::Real interval)
{
	static_cast<Ogre::ParticleSystem*>(_native)->fastForward(time, interval);
}

void ParticleSystem::FastForward(Mogre::Real time)
{
	static_cast<Ogre::ParticleSystem*>(_native)->fastForward(time);
}

void ParticleSystem::SetDefaultDimensions(Mogre::Real width, Mogre::Real height)
{
	static_cast<Ogre::ParticleSystem*>(_native)->setDefaultDimensions(width, height);
}

void ParticleSystem::SetBoundsAutoUpdated(bool autoUpdate, Mogre::Real stopIn)
{
	static_cast<Ogre::ParticleSystem*>(_native)->setBoundsAutoUpdated(autoUpdate, stopIn);
}

void ParticleSystem::SetBoundsAutoUpdated(bool autoUpdate)
{
	static_cast<Ogre::ParticleSystem*>(_native)->setBoundsAutoUpdated(autoUpdate);
}

//Mogre::Const_ParameterList^ ParticleSystem::GetParameters()
//{
//	return static_cast<const Ogre::ParticleSystem*>(_native)->getParameters();
//}

bool ParticleSystem::SetParameter(String^ name, String^ value)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_value, value);

	return static_cast<Ogre::ParticleSystem*>(_native)->setParameter(o_name, o_value);
}

void ParticleSystem::SetParameterList(Mogre::Const_NameValuePairList^ paramList)
{
	static_cast<Ogre::ParticleSystem*>(_native)->setParameterList(paramList);
}

String^ ParticleSystem::GetParameter(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return TO_CLR_STRING(static_cast<const Ogre::ParticleSystem*>(_native)->getParameter(o_name));
}

Ogre::ParticleSystem* ParticleSystem::UnmanagedPointer::get()
{
	return static_cast<Ogre::ParticleSystem*>(_native);
}