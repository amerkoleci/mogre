#include "stdafx.h"
#include "MogreResource.h"
#include "MogreResourceManager.h"

using namespace Mogre;

void Resource_Listener_Director::loadingComplete(Ogre::Resource* param1)
{
	if (doCallForLoadingComplete)
	{
		_receiver->LoadingComplete(ObjectTable::GetOrCreateObject<Mogre::Resource^>((IntPtr)param1));
	}
}

void Resource_Listener_Director::preparingComplete(Ogre::Resource* param1)
{
	if (doCallForPreparingComplete)
	{
		_receiver->PreparingComplete(ObjectTable::GetOrCreateObject<Mogre::Resource^>((IntPtr)param1));
	}
}

void Resource_Listener_Director::unloadingComplete(Ogre::Resource* param1)
{
	if (doCallForUnloadingComplete)
	{
		_receiver->UnloadingComplete(ObjectTable::GetOrCreateObject<Mogre::Resource^>((IntPtr)param1));
	}
}

Mogre::ResourceManager^ Resource::Creator::get()
{
	return ObjectTable::GetOrCreateObject<Mogre::ResourceManager^>((IntPtr)static_cast<Ogre::Resource*>(_native)->getCreator());
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


//Mogre::Const_ParameterList^ Resource::GetParameters()
//{
//	return static_cast<const Ogre::Resource*>(_native)->getParameters();
//}

bool Resource::SetParameter(String^ name, String^ value)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_value, value);

	return static_cast<Ogre::Resource*>(_native)->setParameter(o_name, o_value);
}

void Resource::SetParameterList(Mogre::Const_NameValuePairList^ paramList)
{
	static_cast<Ogre::Resource*>(_native)->setParameterList(paramList);
}

String^ Resource::GetParameter(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return TO_CLR_STRING(static_cast<const Ogre::Resource*>(_native)->getParameter(o_name));
}