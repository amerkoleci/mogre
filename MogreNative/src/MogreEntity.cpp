#include "stdafx.h"
#include "MogreEntity.h"
#include "Marshalling.h"

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
	return ObjectTable::GetOrCreateObject<Mogre::Entity^>((intptr_t)
		static_cast<const Ogre::SubEntity*>(_native)->getParent()
		);
}

Ogre::SubEntity* SubEntity::UnmanagedPointer::get()
{
	return static_cast<Ogre::SubEntity*>(_native);
}

Mogre::SubEntity^ Entity::GetSubEntity(size_t index)
{
	return ObjectTable::GetOrCreateObject<Mogre::SubEntity^>((intptr_t)
		static_cast<const Ogre::Entity*>(_native)->getSubEntity(index)
		);
}

Mogre::SubEntity^ Entity::GetSubEntity(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return ObjectTable::GetOrCreateObject<Mogre::SubEntity^>((intptr_t)
		static_cast<const Ogre::Entity*>(_native)->getSubEntity(o_name)
		);
}

Mogre::Entity^ Entity::Clone()
{
	return ObjectTable::GetOrCreateObject<Mogre::Entity^>((intptr_t) static_cast<const Ogre::Entity*>(_native)->clone());
}

void Entity::SetMaterialName(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	static_cast<Ogre::Entity*>(_native)->setMaterialName(o_name);
}

void Entity::SetPolygonModeOverrideable(bool PolygonModeOverrideable)
{
	static_cast<Ogre::Entity*>(_native)->setPolygonModeOverrideable(PolygonModeOverrideable);
}


Ogre::Entity* Entity::UnmanagedPointer::get()
{
	return static_cast<Ogre::Entity*>(_native);
}