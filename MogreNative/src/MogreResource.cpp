#include "stdafx.h"
#include "MogreResource.h"

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
