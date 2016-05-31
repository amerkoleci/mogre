#pragma once

#include "OgreViewport.h"

namespace Mogre
{
	public ref class Viewport : IMogreDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	internal:
		Ogre::Viewport* _native;
		bool _createdByCLR;

	private:
		Viewport(Ogre::Viewport* obj) : _native(obj)
		{
		}

	public protected:
		Viewport(intptr_t ptr) : _native((Ogre::Viewport*)ptr)
		{

		}

	public:
		~Viewport();
	protected:
		!Viewport();

	public:
		property bool IsDisposed
		{
			virtual bool get()
			{
				return _native == nullptr;
			}
		}

	internal:
		property Ogre::Viewport* UnmanagedPointer
		{
			Ogre::Viewport* get();
		}
	};
}