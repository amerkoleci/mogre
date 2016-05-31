#pragma once

#include "OgreRenderQueue.h"
#include "OgreRenderQueueInvocation.h"
#include "MogreCommon.h"
#include "Marshalling.h"

namespace Mogre
{
	public enum class RenderQueueGroupID
	{
		RENDER_QUEUE_BACKGROUND = Ogre::RENDER_QUEUE_BACKGROUND,
		RENDER_QUEUE_SKIES_EARLY = Ogre::RENDER_QUEUE_SKIES_EARLY,
		RENDER_QUEUE_1 = Ogre::RENDER_QUEUE_1,
		RENDER_QUEUE_2 = Ogre::RENDER_QUEUE_2,
		RENDER_QUEUE_WORLD_GEOMETRY_1 = Ogre::RENDER_QUEUE_WORLD_GEOMETRY_1,
		RENDER_QUEUE_3 = Ogre::RENDER_QUEUE_3,
		RENDER_QUEUE_4 = Ogre::RENDER_QUEUE_4,
		RENDER_QUEUE_MAIN = Ogre::RENDER_QUEUE_MAIN,
		RENDER_QUEUE_6 = Ogre::RENDER_QUEUE_6,
		RENDER_QUEUE_7 = Ogre::RENDER_QUEUE_7,
		RENDER_QUEUE_WORLD_GEOMETRY_2 = Ogre::RENDER_QUEUE_WORLD_GEOMETRY_2,
		RENDER_QUEUE_8 = Ogre::RENDER_QUEUE_8,
		RENDER_QUEUE_9 = Ogre::RENDER_QUEUE_9,
		RENDER_QUEUE_SKIES_LATE = Ogre::RENDER_QUEUE_SKIES_LATE,
		RENDER_QUEUE_OVERLAY = Ogre::RENDER_QUEUE_OVERLAY,
		RENDER_QUEUE_MAX = Ogre::RENDER_QUEUE_MAX
	};

	public ref class RenderQueue : IMogreDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	public:

	internal:
		Ogre::RenderQueue* _native;
		bool _createdByCLR;

	public protected:
		RenderQueue(intptr_t ptr) : _native((Ogre::RenderQueue*)ptr)
		{

		}

		RenderQueue(Ogre::RenderQueue* obj) : _native(obj)
		{

		}

	public:
		~RenderQueue();
	protected:
		!RenderQueue();

	public:
		property bool IsDisposed
		{
			virtual bool get()
			{
				return _native == nullptr;
			}
		}


		DEFINE_MANAGED_NATIVE_CONVERSIONS(RenderQueue);

	internal:
		property Ogre::RenderQueue* UnmanagedPointer
		{
			Ogre::RenderQueue* get() { return _native; }
		}
	};
}