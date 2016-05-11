#include "stdafx.h"
#include "MogreFrameListener.h"

using namespace Mogre;

bool FrameListener_Director::frameStarted(const Ogre::FrameEvent& evt)
{
	if (doCallForFrameStarted)
	{
		bool mp_return = _receiver->FrameStarted(evt);
		return mp_return;
	}
	
	return true;
}

bool FrameListener_Director::frameEnded(const Ogre::FrameEvent& evt)
{
	if (doCallForFrameEnded)
	{
		bool mp_return = _receiver->FrameEnded(evt);
		return mp_return;
	}
	
	return true;
}