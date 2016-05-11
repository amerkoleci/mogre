#include "stdafx.h"
#include "MogreRenderSystem.h"

using namespace Mogre;

void RenderSystem_Listener_Director::eventOccurred(const Ogre::String& eventName, const Ogre::NameValuePairList* parameters)
{
	if (doCallForEventOccurred)
	{
		_receiver->EventOccurred(TO_CLR_STRING(eventName)/*, parameters*/);
	}
}

void RenderSystem::SetConfigOption(String^ name, String^ value)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_value, value);

	_native->setConfigOption(o_name, o_value);
}

Ogre::RenderSystem* RenderSystem::UnmanagedPointer::get()
{
	return _native;
}

//CPP_DECLARE_STLVECTOR(, RenderSystemList, Mogre::RenderSystem^, Ogre::RenderSystem*);