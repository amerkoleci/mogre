#pragma once

#include "IDisposable.h"
#include "OgreRenderTarget.h"
//#include "MogreRenderTargetListener.h"

namespace Mogre
{
	public ref class RenderTarget : IDisposable//, public IRenderTargetListener_Receiver
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	internal:
		Ogre::RenderTarget* _native;

	private:
		RenderTarget(Ogre::RenderTarget* obj) : _native(obj)
		{
		}

	public protected:
		RenderTarget(intptr_t ptr) : _native((Ogre::RenderTarget*)ptr)
		{

		}

	protected:
		/// <summary>Creates a new instance of the Root class.</summary>
		RenderTarget();

	public:
		~RenderTarget();
	protected:
		!RenderTarget();
	public:
		property bool IsDisposed
		{
			virtual bool get();
		}

	internal:
		property Ogre::RenderTarget* UnmanagedPointer
		{
			Ogre::RenderTarget* get();
		}
	};
}