#include "stdafx.h"
#include "MogreStaticGeometry.h"
#include "MogreEntity.h"
#include "MogreSceneNode.h"

using namespace Mogre;

StaticGeometry::~StaticGeometry()
{
	this->!StaticGeometry();
}

StaticGeometry::!StaticGeometry()
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

bool StaticGeometry::CastShadows::get()
{
	return static_cast<Ogre::StaticGeometry*>(_native)->getCastShadows();
}
void StaticGeometry::CastShadows::set(bool castShadows)
{
	static_cast<Ogre::StaticGeometry*>(_native)->setCastShadows(castShadows);
}

bool StaticGeometry::IsVisible::get()
{
	return static_cast<const Ogre::StaticGeometry*>(_native)->isVisible();
}

String^ StaticGeometry::Name::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::StaticGeometry*>(_native)->getName());
}

Mogre::Vector3 StaticGeometry::Origin::get()
{
	return ToVector3(static_cast<const Ogre::StaticGeometry*>(_native)->getOrigin());
}
void StaticGeometry::Origin::set(Mogre::Vector3 origin)
{
	static_cast<Ogre::StaticGeometry*>(_native)->setOrigin(FromVector3(origin));
}

Mogre::Vector3 StaticGeometry::RegionDimensions::get()
{
	return ToVector3(static_cast<const Ogre::StaticGeometry*>(_native)->getRegionDimensions());
}

void StaticGeometry::RegionDimensions::set(Mogre::Vector3 size)
{
	static_cast<Ogre::StaticGeometry*>(_native)->setRegionDimensions(FromVector3(size));
}

Mogre::Real StaticGeometry::RenderingDistance::get()
{
	return static_cast<const Ogre::StaticGeometry*>(_native)->getRenderingDistance();
}
void StaticGeometry::RenderingDistance::set(Mogre::Real dist)
{
	static_cast<Ogre::StaticGeometry*>(_native)->setRenderingDistance(dist);
}

Ogre::uint8 StaticGeometry::RenderQueueGroup::get()
{
	return static_cast<const Ogre::StaticGeometry*>(_native)->getRenderQueueGroup();
}

void StaticGeometry::RenderQueueGroup::set(Ogre::uint8 queueID)
{
	static_cast<Ogre::StaticGeometry*>(_native)->setRenderQueueGroup(queueID);
}

Mogre::Real StaticGeometry::SquaredRenderingDistance::get()
{
	return static_cast<const Ogre::StaticGeometry*>(_native)->getSquaredRenderingDistance();
}

void StaticGeometry::AddEntity(Mogre::Entity^ ent, Mogre::Vector3 position, Mogre::Quaternion orientation, Mogre::Vector3 scale)
{
	static_cast<Ogre::StaticGeometry*>(_native)->addEntity(ent, FromVector3(position), FromQuaternion(orientation), FromVector3(scale));
}

void StaticGeometry::AddEntity(Mogre::Entity^ ent, Mogre::Vector3 position, Mogre::Quaternion orientation)
{
	static_cast<Ogre::StaticGeometry*>(_native)->addEntity(ent, FromVector3(position), FromQuaternion(orientation));
}

void StaticGeometry::AddEntity(Mogre::Entity^ ent, Mogre::Vector3 position)
{
	static_cast<Ogre::StaticGeometry*>(_native)->addEntity(ent, FromVector3(position));
}

void StaticGeometry::AddSceneNode(Mogre::SceneNode^ node)
{
	static_cast<Ogre::StaticGeometry*>(_native)->addSceneNode(node);
}

void StaticGeometry::Build()
{
	static_cast<Ogre::StaticGeometry*>(_native)->build();
}

void StaticGeometry::Destroy()
{
	static_cast<Ogre::StaticGeometry*>(_native)->destroy();
}

void StaticGeometry::Reset()
{
	static_cast<Ogre::StaticGeometry*>(_native)->reset();
}

void StaticGeometry::SetVisible(bool visible)
{
	static_cast<Ogre::StaticGeometry*>(_native)->setVisible(visible);
}

//Mogre::StaticGeometry::RegionIterator^ StaticGeometry::GetRegionIterator()
//{
//	return static_cast<Ogre::StaticGeometry*>(_native)->getRegionIterator();
//}

void StaticGeometry::Dump(String^ filename)
{
	DECLARE_NATIVE_STRING(o_filename, filename);

	static_cast<const Ogre::StaticGeometry*>(_native)->dump(o_filename);
}