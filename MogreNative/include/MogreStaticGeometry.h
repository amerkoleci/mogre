#pragma once

#include "OgreStaticGeometry.h"
#include "MogreMovableObject.h"
#include "MogreRenderOperation.h"
#include "MogreCommon.h"
#include "Marshalling.h"

namespace Mogre
{
	public ref class StaticGeometry : IMogreDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	internal:
		Ogre::StaticGeometry* _native;
		bool _createdByCLR;

	public protected:
		StaticGeometry(intptr_t ptr) : _native((Ogre::StaticGeometry*)ptr)
		{

		}

		StaticGeometry(Ogre::StaticGeometry* obj) : _native(obj)
		{

		}

	public:
		~StaticGeometry();
	protected:
		!StaticGeometry();

	public:
		property bool IsDisposed
		{
			virtual bool get()
			{
				return _native == nullptr;
			}
		}

		DEFINE_MANAGED_NATIVE_CONVERSIONS(StaticGeometry);

	internal:
		property Ogre::StaticGeometry* UnmanagedPointer
		{
			Ogre::StaticGeometry* get() { return _native; }
		}
	};
}