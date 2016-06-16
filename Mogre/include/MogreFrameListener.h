#pragma once

#include "Marshalling.h"
#include "OgreFrameListener.h"

namespace Mogre
{
	//################################################################
	//FrameEvent
	//################################################################

	public value class FrameEvent
	{
	public:
		Ogre::Real timeSinceLastEvent;
		Ogre::Real timeSinceLastFrame;

		DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_VALUECLASS(FrameEvent);
	};

	interface class IFrameListener_Receiver
	{
		bool FrameStarted(Mogre::FrameEvent evt);
		bool FrameRenderingQueued(Mogre::FrameEvent evt);
		bool FrameEnded(Mogre::FrameEvent evt);
	};

	public ref class FrameListener abstract sealed
	{
	public:
		delegate static bool FrameStartedHandler(Mogre::FrameEvent evt);
		delegate static bool FrameRenderingQueuedHandler(Mogre::FrameEvent evt);
		delegate static bool FrameEndedHandler(Mogre::FrameEvent evt);
	};

	class FrameListener_Director : public Ogre::FrameListener
	{
	private:
		gcroot<IFrameListener_Receiver^> _receiver;

	public:
		FrameListener_Director(IFrameListener_Receiver^ recv)
			: _receiver(recv), doCallForFrameStarted(false), doCallForFrameRenderingQueued(false), doCallForFrameEnded(false)
		{
		}

		bool doCallForFrameStarted;
		bool doCallForFrameRenderingQueued;
		bool doCallForFrameEnded;

		virtual bool frameStarted(const Ogre::FrameEvent& evt) override;
		virtual bool frameRenderingQueued(const Ogre::FrameEvent& evt) override;
		virtual bool frameEnded(const Ogre::FrameEvent& evt) override;
	};
}