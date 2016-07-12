#pragma once

#include "OgreRenderQueue.h"
#include "OgreRenderQueueInvocation.h"
#include "OgreRenderQueueSortingGrouping.h"
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

	public ref class RenderQueueGroup
	{
	internal:
		Ogre::RenderQueueGroup* _native;
		bool _createdByCLR;

	public protected:
		RenderQueueGroup(Ogre::RenderQueueGroup* obj) : _native(obj)
		{

		}

	public:
		~RenderQueueGroup();
	protected:
		!RenderQueueGroup();

	public:
		DEFINE_MANAGED_NATIVE_CONVERSIONS(RenderQueueGroup);
	};

	public ref class RenderQueue
	{
	internal:
		Ogre::RenderQueue* _native;
		bool _createdByCLR;

	public protected:
		RenderQueue(Ogre::RenderQueue* obj) : _native(obj)
		{

		}

	public:
		~RenderQueue();
	protected:
		!RenderQueue();

	public:
		property Ogre::uint8 DefaultQueueGroup
		{
		public:
			Ogre::uint8 get();
		public:
			void set(Ogre::uint8 grp);
		}

		property Ogre::ushort DefaultRenderablePriority
		{
		public:
			Ogre::ushort get();
		public:
			void set(Ogre::ushort priority);
		}

		Mogre::RenderQueueGroup^ GetQueueGroup(Ogre::uint8 qid);

		DEFINE_MANAGED_NATIVE_CONVERSIONS(RenderQueue);
	};
}