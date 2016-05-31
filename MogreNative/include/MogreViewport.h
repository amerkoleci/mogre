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
		property int ActualHeight
		{
		public:
			int get();
		}

		property int ActualLeft
		{
		public:
			int get();
		}

		property int ActualTop
		{
		public:
			int get();
		}

		property int ActualWidth
		{
		public:
			int get();
		}

		property Ogre::Real Height
		{
		public:
			Ogre::Real get();
		}

		property Ogre::Real Left
		{
		public:
			Ogre::Real get();
		}

		property String^ MaterialScheme
		{
		public:
			String^ get();
		public:
			void set(String^ schemeName);
		}

		property bool OverlaysEnabled
		{
		public:
			bool get();
		public:
			void set(bool enabled);
		}

		property String^ RenderQueueInvocationSequenceName
		{
		public:
			String^ get();
		public:
			void set(String^ sequenceName);
		}

		property bool SkiesEnabled
		{
		public:
			bool get();
		public:
			void set(bool enabled);
		}

		property Ogre::Real Top
		{
		public:
			Ogre::Real get();
		}

		property Ogre::uint VisibilityMask
		{
		public:
			Ogre::uint get();
		}

		property Ogre::Real Width
		{
		public:
			Ogre::Real get();
		}

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