#include "stdafx.h"
#include "MogreLight.h"
#include "Marshalling.h"

using namespace Mogre;

Ogre::Real Light::tempSquareDist::get()
{
	return static_cast<Ogre::Light*>(_native)->tempSquareDist;
}

void Light::tempSquareDist::set(Ogre::Real value)
{
	static_cast<Ogre::Light*>(_native)->tempSquareDist = value;
}

Mogre::Vector4 Light::As4DVector::get()
{
	return ToVector4(static_cast<const Ogre::Light*>(_native)->getAs4DVector());
}

Ogre::Real Light::AttenuationConstant::get()
{
	return static_cast<const Ogre::Light*>(_native)->getAttenuationConstant();
}

Ogre::Real Light::AttenuationLinear::get()
{
	return static_cast<const Ogre::Light*>(_native)->getAttenuationLinear();
}

Ogre::Real Light::AttenuationQuadric::get()
{
	return static_cast<const Ogre::Light*>(_native)->getAttenuationQuadric();
}

Ogre::Real Light::AttenuationRange::get()
{
	return static_cast<const Ogre::Light*>(_native)->getAttenuationRange();
}

//Mogre::AxisAlignedBox^ Light::BoundingBox::get()
//{
//	return static_cast<const Ogre::Light*>(_native)->getBoundingBox();
//}

Mogre::Vector3 Light::DerivedDirection::get()
{
	return ToVector3(static_cast<const Ogre::Light*>(_native)->getDerivedDirection());
}

Mogre::ColourValue Light::DiffuseColour::get()
{
	return ToColor4(static_cast<const Ogre::Light*>(_native)->getDiffuseColour());
}

void Light::DiffuseColour::set(Mogre::ColourValue colour)
{
	static_cast<Ogre::Light*>(_native)->setDiffuseColour(FromColor4(colour));
}

Mogre::Vector3 Light::Direction::get()
{
	return ToVector3(static_cast<const Ogre::Light*>(_native)->getDirection());
}

void Light::Direction::set(Mogre::Vector3 vec)
{
	static_cast<Ogre::Light*>(_native)->setDirection(FromVector3(vec));
}

String^ Light::MovableType::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::Light*>(_native)->getMovableType());
}


Ogre::Real Light::PowerScale::get()
{
	return static_cast<const Ogre::Light*>(_native)->getPowerScale();
}
void Light::PowerScale::set(Ogre::Real power)
{
	static_cast<Ogre::Light*>(_native)->setPowerScale(power);
}

Mogre::ColourValue Light::SpecularColour::get()
{
	return ToColor4(static_cast<const Ogre::Light*>(_native)->getSpecularColour());
}

void Light::SpecularColour::set(Mogre::ColourValue colour)
{
	static_cast<Ogre::Light*>(_native)->setSpecularColour(FromColor4(colour));
}

Ogre::Real Light::SpotlightFalloff::get()
{
	return static_cast<const Ogre::Light*>(_native)->getSpotlightFalloff();
}

void Light::SpotlightFalloff::set(Ogre::Real val)
{
	static_cast<Ogre::Light*>(_native)->setSpotlightFalloff(val);
}

Mogre::Radian Light::SpotlightInnerAngle::get()
{
	return Mogre::Radian(static_cast<const Ogre::Light*>(_native)->getSpotlightInnerAngle().valueRadians());
}

void Light::SpotlightInnerAngle::set(Mogre::Radian val)
{
	static_cast<Ogre::Light*>(_native)->setSpotlightInnerAngle(Ogre::Radian(val.ValueRadians));
}

Mogre::Radian Light::SpotlightOuterAngle::get()
{
	return Mogre::Radian(static_cast<const Ogre::Light*>(_native)->getSpotlightOuterAngle().valueRadians());
}

void Light::SpotlightOuterAngle::set(Mogre::Radian val)
{
	static_cast<Ogre::Light*>(_native)->setSpotlightOuterAngle(Ogre::Radian(val.ValueRadians));
}

Mogre::Light::LightTypes Light::Type::get()
{
	return (Mogre::Light::LightTypes)static_cast<const Ogre::Light*>(_native)->getType();
}

void Light::Type::set(Mogre::Light::LightTypes type)
{
	static_cast<Ogre::Light*>(_native)->setType((Ogre::Light::LightTypes)type);
}

bool Light::Visible::get()
{
	return static_cast<const Ogre::Light*>(_native)->getVisible();
}

void Light::Visible::set(bool visible)
{
	static_cast<Ogre::Light*>(_native)->setVisible(visible);
}

void Light::_calcTempSquareDist(Mogre::Vector3 worldPos)
{
	static_cast<Ogre::Light*>(_native)->_calcTempSquareDist(FromVector3(worldPos));
}

void Light::SetDiffuseColour(Ogre::Real red, Ogre::Real green, Ogre::Real blue)
{
	static_cast<Ogre::Light*>(_native)->setDiffuseColour(red, green, blue);
}

void Light::SetSpecularColour(Ogre::Real red, Ogre::Real green, Ogre::Real blue)
{
	static_cast<Ogre::Light*>(_native)->setSpecularColour(red, green, blue);
}

void Light::SetAttenuation(Ogre::Real range, Ogre::Real constant, Ogre::Real linear, Ogre::Real quadratic)
{
	static_cast<Ogre::Light*>(_native)->setAttenuation(range, constant, linear, quadratic);
}

void Light::SetDirection(Ogre::Real x, Ogre::Real y, Ogre::Real z)
{
	static_cast<Ogre::Light*>(_native)->setDirection(Ogre::Vector3(x, y, z));
}

void Light::SetSpotlightRange(Mogre::Radian innerAngle, Mogre::Radian outerAngle, Ogre::Real falloff)
{
	static_cast<Ogre::Light*>(_native)->setSpotlightRange(Ogre::Radian(innerAngle.ValueRadians), Ogre::Radian(outerAngle.ValueRadians), falloff);
}

void Light::SetSpotlightRange(Mogre::Radian innerAngle, Mogre::Radian outerAngle)
{
	static_cast<Ogre::Light*>(_native)->setSpotlightRange(Ogre::Radian(innerAngle.ValueRadians), Ogre::Radian(outerAngle.ValueRadians));
}

Ogre::Real Light::ShadowNearClipDistance::get()
{
	return static_cast<const Ogre::Light*>(_native)->getShadowNearClipDistance();
}

void Light::ShadowNearClipDistance::set(Ogre::Real power)
{
	static_cast<Ogre::Light*>(_native)->setShadowNearClipDistance(power);
}

Ogre::Real Light::ShadowFarClipDistance::get()
{
	return static_cast<const Ogre::Light*>(_native)->getShadowFarClipDistance();
}

void Light::ShadowFarClipDistance::set(Ogre::Real power)
{
	static_cast<Ogre::Light*>(_native)->setShadowFarClipDistance(power);
}

Ogre::Light* Light::UnmanagedPointer::get()
{
	return static_cast<Ogre::Light*>(_native);
}