#include "stdafx.h"
#include "MogreMovableObject.h"
#include "MogreNode.h"
#include "MogreSceneNode.h"
#include "Marshalling.h"

using namespace Mogre;

Ogre::Real MovableObject::WorldRadius::get()
{
	return static_cast<const Ogre::MovableObject*>(_native)->getWorldRadius();
}

bool MovableObject::CastShadows::get()
{
	return static_cast<const Ogre::MovableObject*>(_native)->getCastShadows();
}

void MovableObject::CastShadows::set(bool enabled)
{
	static_cast<Ogre::MovableObject*>(_native)->setCastShadows(enabled);
}

Ogre::uint32 MovableObject::DefaultQueryFlags::get()
{
	return Ogre::MovableObject::getDefaultQueryFlags();
}

void MovableObject::DefaultQueryFlags::set(Ogre::uint32 flags)
{
	Ogre::MovableObject::setDefaultQueryFlags(flags);
}

Ogre::uint32 MovableObject::DefaultVisibilityFlags::get()
{
	return Ogre::MovableObject::getDefaultVisibilityFlags();
}

void MovableObject::DefaultVisibilityFlags::set(Ogre::uint32 flags)
{
	Ogre::MovableObject::setDefaultVisibilityFlags(flags);
}

bool MovableObject::IsAttached::get()
{
	return static_cast<const Ogre::MovableObject*>(_native)->isAttached();
}

String^ MovableObject::MovableType::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::MovableObject*>(_native)->getMovableType());
}

String^ MovableObject::Name::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::MovableObject*>(_native)->getName());
}

void MovableObject::Name::set(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	static_cast<Ogre::MovableObject*>(_native)->setName(o_name);
}

Mogre::Node^ MovableObject::ParentNode::get()
{
	return ObjectTable::GetOrCreateObject<Mogre::Node^>((intptr_t)static_cast<const Ogre::MovableObject*>(_native)->getParentNode());
}

Mogre::SceneNode^ MovableObject::ParentSceneNode::get()
{
	return ObjectTable::GetOrCreateObject<Mogre::SceneNode^>((intptr_t)static_cast<const Ogre::MovableObject*>(_native)->getParentSceneNode());
}

Ogre::uint32 MovableObject::QueryFlags::get()
{
	return static_cast<const Ogre::MovableObject*>(_native)->getQueryFlags();
}

void MovableObject::QueryFlags::set(Ogre::uint32 flags)
{
	static_cast<Ogre::MovableObject*>(_native)->setQueryFlags(flags);
}

Ogre::Real MovableObject::RenderingDistance::get()
{
	return static_cast<const Ogre::MovableObject*>(_native)->getRenderingDistance();
}
void MovableObject::RenderingDistance::set(Ogre::Real dist)
{
	static_cast<Ogre::MovableObject*>(_native)->setRenderingDistance(dist);
}

Ogre::uint8 MovableObject::RenderQueueGroup::get()
{
	return static_cast<const Ogre::MovableObject*>(_native)->getRenderQueueGroup();
}

void MovableObject::RenderQueueGroup::set(Ogre::uint8 queueID)
{
	static_cast<Ogre::MovableObject*>(_native)->setRenderQueueGroup(queueID);
}

Ogre::uint32 MovableObject::VisibilityFlags::get()
{
	return static_cast<const Ogre::MovableObject*>(_native)->getVisibilityFlags();
}

void MovableObject::VisibilityFlags::set(Ogre::uint32 flags)
{
	static_cast<Ogre::MovableObject*>(_native)->setVisibilityFlags(flags);
}

bool MovableObject::Visible::get()
{
	return static_cast<const Ogre::MovableObject*>(_native)->getVisible();
}

void MovableObject::Visible::set(bool visible)
{
	static_cast<Ogre::MovableObject*>(_native)->setVisible(visible);
}

void MovableObject::AddQueryFlags(Ogre::uint32 flags)
{
	static_cast<Ogre::MovableObject*>(_native)->addQueryFlags(flags);
}

void MovableObject::RemoveQueryFlags(unsigned long flags)
{
	static_cast<Ogre::MovableObject*>(_native)->removeQueryFlags(flags);
}

void MovableObject::AddVisibilityFlags(Ogre::uint32 flags)
{
	static_cast<Ogre::MovableObject*>(_native)->addVisibilityFlags(flags);
}

void MovableObject::RemoveVisibilityFlags(Ogre::uint32 flags)
{
	static_cast<Ogre::MovableObject*>(_native)->removeVisibilityFlags(flags);
}

Ogre::MovableObject* MovableObject::UnmanagedPointer::get()
{
	return _native;
}