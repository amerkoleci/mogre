#include "stdafx.h"
#include "MogreResource.h"
#include "MogreResourceManager.h"

using namespace Mogre;

void Resource_Listener_Director::loadingComplete(Ogre::Resource* param1)
{
	if (doCallForLoadingComplete)
	{
		_receiver->LoadingComplete(ObjectTable::GetOrCreateObject<Mogre::Resource^>((intptr_t)param1));
	}
}

void Resource_Listener_Director::preparingComplete(Ogre::Resource* param1)
{
	if (doCallForPreparingComplete)
	{
		_receiver->PreparingComplete(ObjectTable::GetOrCreateObject<Mogre::Resource^>((intptr_t)param1));
	}
}

void Resource_Listener_Director::unloadingComplete(Ogre::Resource* param1)
{
	if (doCallForUnloadingComplete)
	{
		_receiver->UnloadingComplete(ObjectTable::GetOrCreateObject<Mogre::Resource^>((intptr_t)param1));
	}
}

Mogre::ResourceManager^ Resource::Creator::get()
{
	return ObjectTable::GetOrCreateObject<Mogre::ResourceManager^>((intptr_t)static_cast<Ogre::Resource*>(_native)->getCreator());
}

String^ Resource::Group::get()
{
	return TO_CLR_STRING(static_cast<Ogre::Resource*>(_native)->getGroup());
}

Mogre::ResourceHandle Resource::Handle::get()
{
	return static_cast<const Ogre::Resource*>(_native)->getHandle();
}

bool Resource::IsBackgroundLoaded::get()
{
	return static_cast<const Ogre::Resource*>(_native)->isBackgroundLoaded();
}

bool Resource::IsLoaded::get()
{
	return static_cast<const Ogre::Resource*>(_native)->isLoaded();
}

bool Resource::IsManuallyLoaded::get()
{
	return static_cast<const Ogre::Resource*>(_native)->isManuallyLoaded();
}

bool Resource::IsReloadable::get()
{
	return static_cast<const Ogre::Resource*>(_native)->isReloadable();
}

String^ Resource::Name::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::Resource*>(_native)->getName());
}

String^ Resource::Origin::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::Resource*>(_native)->getOrigin());
}

size_t Resource::Size::get()
{
	return static_cast<const Ogre::Resource*>(_native)->getSize();
}

void Resource::Load(bool backgroundThread)
{
	static_cast<Ogre::Resource*>(_native)->load(backgroundThread);
}

void Resource::Load()
{
	static_cast<Ogre::Resource*>(_native)->load();
}

void Resource::Reload()
{
	static_cast<Ogre::Resource*>(_native)->reload();
}

void Resource::Unload()
{
	static_cast<Ogre::Resource*>(_native)->unload();
}

void Resource::Touch()
{
	static_cast<Ogre::Resource*>(_native)->touch();
}

Mogre::Resource::LoadingState Resource::IsLoading()
{
	return (Mogre::Resource::LoadingState)static_cast<const Ogre::Resource*>(_native)->isLoading();
}

Mogre::Resource::LoadingState Resource::GetLoadingState()
{
	return (Mogre::Resource::LoadingState)static_cast<const Ogre::Resource*>(_native)->getLoadingState();
}

void Resource::SetBackgroundLoaded(bool bl)
{
	static_cast<Ogre::Resource*>(_native)->setBackgroundLoaded(bl);
}

void Resource::EscalateLoading()
{
	static_cast<Ogre::Resource*>(_native)->escalateLoading();
}

void Resource::ChangeGroupOwnership(String^ newGroup)
{
	DECLARE_NATIVE_STRING(o_newGroup, newGroup);

	static_cast<Ogre::Resource*>(_native)->changeGroupOwnership(o_newGroup);
}
