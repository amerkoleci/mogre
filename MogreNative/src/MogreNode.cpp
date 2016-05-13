#include "stdafx.h"
#include "MogreNode.h"
#include "Marshalling.h"
#include "ObjectTable.h"

using namespace Mogre;

bool Node::InheritOrientation::get()
{
	return _native->getInheritOrientation();
}
void Node::InheritOrientation::set(bool inherit)
{
	_native->setInheritOrientation(inherit);
}

bool Node::InheritScale::get()
{
	return _native->getInheritScale();
}

void Node::InheritScale::set(bool inherit)
{
	_native->setInheritScale(inherit);
}

/*Mogre::Matrix3^ Node::LocalAxes::get()
{
	return static_cast<const Ogre::Node*>(_native)->getLocalAxes();
}*/

String^ Node::Name::get()
{
	return TO_CLR_STRING(_native->getName());
}

void Node::Name::set(String^ value)
{
	DECLARE_NATIVE_STRING(o_value, value);

	_native->setName(o_value);
}

bool Node::IsStatic::get()
{
	return _native->isStatic();
}

void Node::IsStatic::set(bool value)
{
	_native->setStatic(value);
}

Mogre::Quaternion Node::Orientation::get()
{
	return ToQuaternion(_native->getOrientation());
}

void Node::Orientation::set(Mogre::Quaternion q)
{
	_native->setOrientation(FromQuaternion(q));
}

Mogre::Node^ Node::Parent::get()
{
	return ObjectTable::GetOrCreateObject<Mogre::Node^>((intptr_t)_native->getParent());
}

Ogre::uint16 Node::DepthLevel::get()
{
	return _native->getDepthLevel();
}

Mogre::Vector3 Node::Position::get()
{
	return ToVector3(_native->getPosition());
}

void Node::Position::set(Mogre::Vector3 pos)
{
	_native->setPosition(FromVector3(pos));
}

Mogre::Vector3 Node::Scale::get()
{
	return ToVector3(_native->getScale());
}

void Node::Scale::set(Mogre::Vector3 value)
{
	_native->setScale(FromVector3(value));
}

Mogre::Quaternion Node::DerivedOrientation::get()
{
	return ToQuaternion(_native->_getDerivedOrientation());
}

void Node::DerivedOrientation::set(Mogre::Quaternion value)
{
	_native->_setDerivedOrientation(FromQuaternion(value));
}

Mogre::Vector3 Node::DerivedPosition::get()
{
	return ToVector3(_native->_getDerivedPosition());
}

void Node::DerivedPosition::set(Mogre::Vector3 value)
{
	_native->_setDerivedPosition(FromVector3(value));
}

void Node::SetOrientation(Ogre::Real w, Ogre::Real x, Ogre::Real y, Ogre::Real z)
{
	_native->setOrientation(w, x, y, z);
}

void Node::ResetOrientation()
{
	_native->resetOrientation();
}

void Node::SetPosition(Ogre::Real x, Ogre::Real y, Ogre::Real z)
{
	static_cast<Ogre::Node*>(_native)->setPosition(x, y, z);
}

void Node::SetScale(Ogre::Real x, Ogre::Real y, Ogre::Real z)
{
	_native->setScale(x, y, z);
}

void Node::ScaleVector(Mogre::Vector3 scale)
{
	_native->scale(FromVector3(scale));
}

void Node::ScaleXYZ(Ogre::Real x, Ogre::Real y, Ogre::Real z)
{
	_native->scale(x, y, z);
}

void Node::Translate(Mogre::Vector3 d, Mogre::Node::TransformSpace relativeTo)
{
	_native->translate(FromVector3(d), (Ogre::Node::TransformSpace)relativeTo);
}

void Node::Translate(Mogre::Vector3 d)
{
	_native->translate(FromVector3(d));
}

void Node::Translate(Ogre::Real x, Ogre::Real y, Ogre::Real z, Mogre::Node::TransformSpace relativeTo)
{
	_native->translate(x, y, z, (Ogre::Node::TransformSpace)relativeTo);
}

void Node::Translate(Ogre::Real x, Ogre::Real y, Ogre::Real z)
{
	_native->translate(x, y, z);
}

/*void Node::Translate(Mogre::Matrix3^ axes, Mogre::Vector3 move, Mogre::Node::TransformSpace relativeTo)
{
	pin_ptr<Ogre::Matrix3> p_axes = interior_ptr<Ogre::Matrix3>(&axes->m00);

	static_cast<Ogre::Node*>(_native)->translate(*p_axes, move, (Ogre::Node::TransformSpace)relativeTo);
}

void Node::Translate(Mogre::Matrix3^ axes, Mogre::Vector3 move)
{
	pin_ptr<Ogre::Matrix3> p_axes = interior_ptr<Ogre::Matrix3>(&axes->m00);

	static_cast<Ogre::Node*>(_native)->translate(*p_axes, move);
}

void Node::Translate(Mogre::Matrix3^ axes, Mogre::Real x, Mogre::Real y, Mogre::Real z, Mogre::Node::TransformSpace relativeTo)
{
	pin_ptr<Ogre::Matrix3> p_axes = interior_ptr<Ogre::Matrix3>(&axes->m00);

	static_cast<Ogre::Node*>(_native)->translate(*p_axes, x, y, z, (Ogre::Node::TransformSpace)relativeTo);
}
void Node::Translate(Mogre::Matrix3^ axes, Mogre::Real x, Mogre::Real y, Mogre::Real z)
{
	pin_ptr<Ogre::Matrix3> p_axes = interior_ptr<Ogre::Matrix3>(&axes->m00);

	static_cast<Ogre::Node*>(_native)->translate(*p_axes, x, y, z);
}

void Node::Roll(Mogre::Radian angle, Mogre::Node::TransformSpace relativeTo)
{
	static_cast<Ogre::Node*>(_native)->roll(angle, (Ogre::Node::TransformSpace)relativeTo);
}
void Node::Roll(Mogre::Radian angle)
{
	static_cast<Ogre::Node*>(_native)->roll(angle);
}

void Node::Pitch(Mogre::Radian angle, Mogre::Node::TransformSpace relativeTo)
{
	static_cast<Ogre::Node*>(_native)->pitch(angle, (Ogre::Node::TransformSpace)relativeTo);
}
void Node::Pitch(Mogre::Radian angle)
{
	static_cast<Ogre::Node*>(_native)->pitch(angle);
}

void Node::Yaw(Mogre::Radian angle, Mogre::Node::TransformSpace relativeTo)
{
	static_cast<Ogre::Node*>(_native)->yaw(angle, (Ogre::Node::TransformSpace)relativeTo);
}
void Node::Yaw(Mogre::Radian angle)
{
	static_cast<Ogre::Node*>(_native)->yaw(angle);
}

void Node::Rotate(Mogre::Vector3 axis, Mogre::Radian angle, Mogre::Node::TransformSpace relativeTo)
{
	static_cast<Ogre::Node*>(_native)->rotate(axis, angle, (Ogre::Node::TransformSpace)relativeTo);
}
void Node::Rotate(Mogre::Vector3 axis, Mogre::Radian angle)
{
	static_cast<Ogre::Node*>(_native)->rotate(axis, angle);
}
*/

void Node::Rotate(Mogre::Quaternion q, Mogre::Node::TransformSpace relativeTo)
{
	_native->rotate(FromQuaternion(q), (Ogre::Node::TransformSpace)relativeTo);
}

void Node::Rotate(Mogre::Quaternion q)
{
	_native->rotate(FromQuaternion(q));
}

Mogre::Node^ Node::CreateChild(SceneMemoryMgrTypes sceneType, Mogre::Vector3 translate, Mogre::Quaternion rotate)
{
	auto native = (intptr_t)_native->createChild((Ogre::SceneMemoryMgrTypes)sceneType, FromVector3(translate), FromQuaternion(rotate));
	return ObjectTable::GetOrCreateObject<Mogre::Node^>(native);
}

Mogre::Node^ Node::CreateChild(SceneMemoryMgrTypes sceneType, Mogre::Vector3 translate)
{
	auto native = (intptr_t)_native->createChild((Ogre::SceneMemoryMgrTypes)sceneType, FromVector3(translate));
	return ObjectTable::GetOrCreateObject<Mogre::Node^>(native);
}

Mogre::Node^ Node::CreateChild(SceneMemoryMgrTypes sceneType)
{
	return ObjectTable::GetOrCreateObject<Mogre::Node^>((intptr_t)_native->createChild((Ogre::SceneMemoryMgrTypes)sceneType));
}

Mogre::Node^ Node::CreateChild()
{
	return ObjectTable::GetOrCreateObject<Mogre::Node^>((intptr_t)_native->createChild());
}

Ogre::Node* Node::UnmanagedPointer::get()
{
	return _native;
}