#include "stdafx.h"
#include "MogreEntity.h"
#include "MogreVertexIndexData.h"
#include "MogreMeshManager.h"
#include "MogreMaterialManager.h"
#include "MogreMeshManager.h"
#include "MogreCamera.h"
#include "MogreAnimation.h"

using namespace Mogre;

bool SubEntity::CastsShadows::get()
{
	return static_cast<const Ogre::SubEntity*>(_native)->getCastsShadows();
}

String^ SubEntity::MaterialName::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::SubEntity*>(_native)->getMaterialName());
}

void SubEntity::MaterialName::set(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	static_cast<Ogre::SubEntity*>(_native)->setMaterialName(o_name);
}

unsigned short SubEntity::NumWorldTransforms::get()
{
	return static_cast<const Ogre::SubEntity*>(_native)->getNumWorldTransforms();
}

Mogre::Entity^ SubEntity::Parent::get()
{
	return ObjectTable::GetOrCreateObject<Mogre::Entity^>((IntPtr)
		static_cast<const Ogre::SubEntity*>(_native)->getParent()
		);
}

Ogre::SubEntity* SubEntity::UnmanagedPointer::get()
{
	return static_cast<Ogre::SubEntity*>(_native);
}

Mogre::SubEntity^ Entity::GetSubEntity(size_t index)
{
	return static_cast<const Ogre::Entity*>(_native)->getSubEntity(index);
}

Mogre::SubEntity^ Entity::GetSubEntity(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return static_cast<const Ogre::Entity*>(_native)->getSubEntity(o_name);
}

Mogre::SubMesh^ SubEntity::SubMesh::get()
{
	return static_cast<Ogre::SubEntity*>(_native)->getSubMesh();
}

Mogre::Technique^ SubEntity::Technique::get()
{
	return _native->getTechnique();
}

Mogre::VertexData^ SubEntity::VertexDataForBinding::get()
{
	return _native->getVertexDataForBinding();
}

Mogre::MaterialPtr^ SubEntity::GetMaterial()
{
	return _native->getMaterial();
}

void SubEntity::GetRenderOperation(Mogre::RenderOperation^ op)
{
	static_cast<Ogre::SubEntity*>(_native)->getRenderOperation(op);
}

void SubEntity::GetWorldTransforms(Mogre::Matrix4* xform)
{
	Ogre::Matrix4* o_xform = reinterpret_cast<Ogre::Matrix4*>(xform);

	_native->getWorldTransforms(o_xform);
}

Mogre::Real SubEntity::GetSquaredViewDepth(Mogre::Camera^ cam)
{
	return _native->getSquaredViewDepth(cam);
}


bool SubEntity::PolygonModeOverrideable::get()
{
	return static_cast<const Ogre::SubEntity*>(_native)->getPolygonModeOverrideable();
}
void SubEntity::PolygonModeOverrideable::set(bool override)
{
	static_cast<Ogre::SubEntity*>(_native)->setPolygonModeOverrideable(override);
}

bool SubEntity::UseIdentityProjection::get()
{
	return static_cast<const Ogre::SubEntity*>(_native)->getUseIdentityProjection();
}
void SubEntity::UseIdentityProjection::set(bool useIdentityProjection)
{
	static_cast<Ogre::SubEntity*>(_native)->setUseIdentityProjection(useIdentityProjection);
}

bool SubEntity::UseIdentityView::get()
{
	return static_cast<const Ogre::SubEntity*>(_native)->getUseIdentityView();
}
void SubEntity::UseIdentityView::set(bool useIdentityView)
{
	static_cast<Ogre::SubEntity*>(_native)->setUseIdentityView(useIdentityView);
}

void SubEntity::SetMaterialName(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);
	_native->setMaterialName(o_name);
}

void SubEntity::SetMaterialName(String^ name, String^ resGroup)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_resGroup, resGroup);

	_native->setMaterialName(o_name, o_resGroup);
}

void SubEntity::SetCustomParameter(size_t index, Mogre::Vector4 value)
{
	_native->setCustomParameter(index, FromVector4(value));
}

Mogre::Vector4 SubEntity::GetCustomParameter(size_t index)
{
	return ToVector4(_native->getCustomParameter(index));
}

// -------------- Entity --------------
Mogre::AnimationStateSet^ Entity::AllAnimationStates::get()
{
	return static_cast<const Ogre::Entity*>(_native)->getAllAnimationStates();
}

bool Entity::DisplaySkeleton::get()
{
	return static_cast<const Ogre::Entity*>(_native)->getDisplaySkeleton();
}

void Entity::DisplaySkeleton::set(bool display)
{
	static_cast<Ogre::Entity*>(_native)->setDisplaySkeleton(display);
}

//Mogre::EdgeData^ Entity::EdgeList::get()
//{
//	return static_cast<Ogre::Entity*>(_native)->getEdgeList();
//}

bool Entity::HasEdgeList::get()
{
	return static_cast<Ogre::Entity*>(_native)->hasEdgeList();
}

bool Entity::HasSkeleton::get()
{
	return static_cast<const Ogre::Entity*>(_native)->hasSkeleton();
}

bool Entity::HasVertexAnimation::get()
{
	return static_cast<const Ogre::Entity*>(_native)->hasVertexAnimation();
}

bool Entity::IsHardwareAnimationEnabled::get()
{
	return static_cast<Ogre::Entity*>(_native)->isHardwareAnimationEnabled();
}

bool Entity::IsInitialised::get()
{
	return static_cast<const Ogre::Entity*>(_native)->isInitialised();
}

String^ Entity::MovableType::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::Entity*>(_native)->getMovableType());
}

size_t Entity::NumManualLodLevels::get()
{
	return static_cast<const Ogre::Entity*>(_native)->getNumManualLodLevels();
}

unsigned int Entity::NumSubEntities::get()
{
	return static_cast<const Ogre::Entity*>(_native)->getNumSubEntities();
}

Ogre::uint8 Entity::RenderQueueGroup::get()
{
	return static_cast<const Ogre::Entity*>(_native)->getRenderQueueGroup();
}

void Entity::RenderQueueGroup::set(Ogre::uint8 queueID)
{
	static_cast<Ogre::Entity*>(_native)->setRenderQueueGroup(queueID);
}

//Mogre::SkeletonInstance^ Entity::Skeleton::get()
//{
//	return static_cast<const Ogre::Entity*>(_native)->getSkeleton();
//}

int Entity::SoftwareAnimationNormalsRequests::get()
{
	return static_cast<const Ogre::Entity*>(_native)->getSoftwareAnimationNormalsRequests();
}

int Entity::SoftwareAnimationRequests::get()
{
	return static_cast<const Ogre::Entity*>(_native)->getSoftwareAnimationRequests();
}

Mogre::VertexData^ Entity::VertexDataForBinding::get()
{
	return static_cast<Ogre::Entity*>(_native)->getVertexDataForBinding();
}

Mogre::MeshPtr^ Entity::GetMesh()
{
	return static_cast<const Ogre::Entity*>(_native)->getMesh();
}

Mogre::Entity^ Entity::Clone()
{
	return ObjectTable::GetOrCreateObject<Mogre::Entity^>((IntPtr) static_cast<const Ogre::Entity*>(_native)->clone());
}

void Entity::SetMaterialName(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	static_cast<Ogre::Entity*>(_native)->setMaterialName(o_name);
}

Mogre::AnimationState^ Entity::GetAnimationState(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return static_cast<const Ogre::Entity*>(_native)->getAnimationState(o_name);
}

Mogre::Entity^ Entity::GetManualLodLevel(size_t index)
{
	return static_cast<const Ogre::Entity*>(_native)->getManualLodLevel(index);
}

void Entity::SetPolygonModeOverrideable(bool PolygonModeOverrideable)
{
	static_cast<Ogre::Entity*>(_native)->setPolygonModeOverrideable(PolygonModeOverrideable);
}