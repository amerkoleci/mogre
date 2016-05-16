#include "stdafx.h"
#include "MogreCamera.h"
#include "MogreSceneNode.h"
#include "MogreSceneManager.h"
#include "Marshalling.h"

using namespace Mogre;

bool Camera::AutoAspectRatio::get()
{
	return static_cast<const Ogre::Camera*>(_native)->getAutoAspectRatio();
}
void Camera::AutoAspectRatio::set(bool autoratio)
{
	static_cast<Ogre::Camera*>(_native)->setAutoAspectRatio(autoratio);
}

Mogre::Vector3 Camera::AutoTrackOffset::get()
{
	return ToVector3(static_cast<const Ogre::Camera*>(_native)->getAutoTrackOffset());
}

Mogre::SceneNode^ Camera::AutoTrackTarget::get()
{
	return ObjectTable::GetOrCreateObject<Mogre::SceneNode^>((intptr_t)static_cast<const Ogre::Camera*>(_native)->getAutoTrackTarget());
}

/*Mogre::Frustum^ Camera::CullingFrustum::get()
{
	return static_cast<Ogre::Camera*>(_native)->getCullingFrustum();
}

void Camera::CullingFrustum::set(Mogre::Frustum^ frustum)
{
	static_cast<Ogre::Camera*>(_native)->setCullingFrustum(frustum);
}*/

Mogre::Vector3 Camera::DerivedDirection::get()
{
	return ToVector3(static_cast<const Ogre::Camera*>(_native)->getDerivedDirection());
}

Mogre::Quaternion Camera::DerivedOrientation::get()
{
	return ToQuaternion(static_cast<const Ogre::Camera*>(_native)->getDerivedOrientation());
}

Mogre::Vector3 Camera::DerivedPosition::get()
{
	return ToVector3(static_cast<const Ogre::Camera*>(_native)->getDerivedPosition());
}

Mogre::Vector3 Camera::DerivedRight::get()
{
	return ToVector3(static_cast<const Ogre::Camera*>(_native)->getDerivedRight());
}

Mogre::Vector3 Camera::DerivedUp::get()
{
	return ToVector3(static_cast<const Ogre::Camera*>(_native)->getDerivedUp());
}

Mogre::Vector3 Camera::Direction::get()
{
	return ToVector3(static_cast<const Ogre::Camera*>(_native)->getDirection());
}

void Camera::Direction::set(Mogre::Vector3 vec)
{
	static_cast<Ogre::Camera*>(_native)->setDirection(FromVector3(vec));
}

Ogre::Real Camera::FarClipDistance::get()
{
	return static_cast<const Ogre::Camera*>(_native)->getFarClipDistance();
}

void Camera::FarClipDistance::set(Ogre::Real farDist)
{
	static_cast<Ogre::Camera*>(_native)->setFarClipDistance(farDist);
}

bool Camera::IsWindowSet::get()
{
	return static_cast<const Ogre::Camera*>(_native)->isWindowSet();
}

Ogre::Real Camera::LodBias::get()
{
	return static_cast<const Ogre::Camera*>(_native)->getLodBias();
}

void Camera::LodBias::set(Ogre::Real factor)
{
	static_cast<Ogre::Camera*>(_native)->setLodBias(factor);
}

String^ Camera::MovableType::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::Camera*>(_native)->getMovableType());
}

String^ Camera::Name::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::Camera*>(_native)->getName());
}

Ogre::Real Camera::NearClipDistance::get()
{
	return static_cast<const Ogre::Camera*>(_native)->getNearClipDistance();
}

void Camera::NearClipDistance::set(Ogre::Real nearDist)
{
	static_cast<Ogre::Camera*>(_native)->setNearClipDistance(nearDist);
}

Mogre::Quaternion Camera::Orientation::get()
{
	return ToQuaternion(static_cast<const Ogre::Camera*>(_native)->getOrientation());
}

void Camera::Orientation::set(Mogre::Quaternion q)
{
	static_cast<Ogre::Camera*>(_native)->setOrientation(FromQuaternion(q));
}

Mogre::PolygonMode Camera::PolygonMode::get()
{
	return (Mogre::PolygonMode)static_cast<const Ogre::Camera*>(_native)->getPolygonMode();
}

void Camera::PolygonMode::set(Mogre::PolygonMode sd)
{
	static_cast<Ogre::Camera*>(_native)->setPolygonMode((Ogre::PolygonMode)sd);
}

Mogre::Vector3 Camera::Position::get()
{
	return ToVector3(static_cast<const Ogre::Camera*>(_native)->getPosition());
}

void Camera::Position::set(Mogre::Vector3 vec)
{
	static_cast<Ogre::Camera*>(_native)->setPosition(FromVector3(vec));
}

Mogre::Vector3 Camera::RealDirection::get()
{
	return ToVector3(static_cast<const Ogre::Camera*>(_native)->getRealDirection());
}

Mogre::Quaternion Camera::RealOrientation::get()
{
	return ToQuaternion(static_cast<const Ogre::Camera*>(_native)->getRealOrientation());
}

Mogre::Vector3 Camera::RealPosition::get()
{
	return ToVector3(static_cast<const Ogre::Camera*>(_native)->getRealPosition());
}

Mogre::Vector3 Camera::RealRight::get()
{
	return ToVector3(static_cast<const Ogre::Camera*>(_native)->getRealRight());
}

Mogre::Vector3 Camera::RealUp::get()
{
	return ToVector3(static_cast<const Ogre::Camera*>(_native)->getRealUp());
}

Mogre::Vector3 Camera::Right::get()
{
	return ToVector3(static_cast<const Ogre::Camera*>(_native)->getRight());
}

Mogre::SceneManager^ Camera::SceneManager::get()
{
	return ObjectTable::GetOrCreateObject<Mogre::SceneManager^>((intptr_t)static_cast<const Ogre::Camera*>(_native)->getSceneManager());
}

Mogre::Vector3 Camera::Up::get()
{
	return ToVector3(static_cast<const Ogre::Camera*>(_native)->getUp());
}

bool Camera::UseRenderingDistance::get()
{
	return static_cast<const Ogre::Camera*>(_native)->getUseRenderingDistance();
}

void Camera::UseRenderingDistance::set(bool use)
{
	static_cast<Ogre::Camera*>(_native)->setUseRenderingDistance(use);
}

/*Mogre::Matrix4^ Camera::ViewMatrix::get()
{
	return static_cast<const Ogre::Camera*>(_native)->getViewMatrix();
}

Mogre::Viewport^ Camera::Viewport::get()
{
	return static_cast<const Ogre::Camera*>(_native)->getViewport();
}*/

void Camera::SetPosition(Ogre::Real x, Ogre::Real y, Ogre::Real z)
{
	static_cast<Ogre::Camera*>(_native)->setPosition(x, y, z);
}

void Camera::Move(Mogre::Vector3 vec)
{
	static_cast<Ogre::Camera*>(_native)->move(FromVector3(vec));
}

void Camera::MoveRelative(Mogre::Vector3 vec)
{
	static_cast<Ogre::Camera*>(_native)->moveRelative(FromVector3(vec));
}

void Camera::SetDirection(Ogre::Real x, Ogre::Real y, Ogre::Real z)
{
	static_cast<Ogre::Camera*>(_native)->setDirection(x, y, z);
}

void Camera::LookAt(Mogre::Vector3 targetPoint)
{
	static_cast<Ogre::Camera*>(_native)->lookAt(FromVector3(targetPoint));
}

void Camera::LookAt(Ogre::Real x, Ogre::Real y, Ogre::Real z)
{
	static_cast<Ogre::Camera*>(_native)->lookAt(x, y, z);
}

void Camera::Roll(Mogre::Radian angle)
{
	static_cast<Ogre::Camera*>(_native)->roll(Ogre::Radian(angle.ValueRadians));
}

void Camera::Yaw(Mogre::Radian angle)
{
	static_cast<Ogre::Camera*>(_native)->yaw(Ogre::Radian(angle.ValueRadians));
}

void Camera::Pitch(Mogre::Radian angle)
{
	static_cast<Ogre::Camera*>(_native)->pitch(Ogre::Radian(angle.ValueRadians));
}

void Camera::Rotate(Mogre::Vector3 axis, Mogre::Radian angle)
{
	static_cast<Ogre::Camera*>(_native)->rotate(FromVector3(axis), Ogre::Radian(angle.ValueRadians));
}

void Camera::Rotate(Mogre::Quaternion q)
{
	static_cast<Ogre::Camera*>(_native)->rotate(FromQuaternion(q));
}

void Camera::SetFixedYawAxis(bool useFixed, Mogre::Vector3 fixedAxis)
{
	static_cast<Ogre::Camera*>(_native)->setFixedYawAxis(useFixed, FromVector3(fixedAxis));
}

void Camera::SetFixedYawAxis(bool useFixed)
{
	static_cast<Ogre::Camera*>(_native)->setFixedYawAxis(useFixed);
}

void Camera::SetAutoTracking(bool enabled, Mogre::SceneNode^ target, Mogre::Vector3 offset)
{
	static_cast<Ogre::Camera*>(_native)->setAutoTracking(enabled, GetPointerOrNull(target), FromVector3(offset));
}

void Camera::SetAutoTracking(bool enabled, Mogre::SceneNode^ target)
{
	static_cast<Ogre::Camera*>(_native)->setAutoTracking(enabled, GetPointerOrNull(target));
}

void Camera::SetAutoTracking(bool enabled)
{
	static_cast<Ogre::Camera*>(_native)->setAutoTracking(enabled);
}


Ogre::Camera* Camera::UnmanagedPointer::get()
{
	return static_cast<Ogre::Camera*>(_native);
}