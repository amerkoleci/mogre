#pragma once

#include "OgreViewport.h"
#include "Marshalling.h"

namespace Mogre
{
	ref class RenderTarget;

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

		property Mogre::RenderTarget^ Target
		{
		public:
			Mogre::RenderTarget^ get();
		}

		property bool IsDisposed
		{
			virtual bool get()
			{
				return _native == nullptr;
			}
		}

		void _updateDimensions();
		void SetDimensions(Ogre::Real left, Ogre::Real top, Ogre::Real width, Ogre::Real height);
		void GetActualDimensions([Out] int% left, [Out] int% top, [Out] int% width, [Out] int% height);

		void Clear();
		void Clear(unsigned int buffers);
		void Clear(unsigned int buffers, Mogre::ColourValue colour);
		void Clear(unsigned int buffers, Mogre::ColourValue colour, Ogre::Real depth);
		void Clear(unsigned int buffers, Mogre::ColourValue colour, Ogre::Real depth, unsigned short stencil);

		DEFINE_MANAGED_NATIVE_CONVERSIONS(Viewport);

	internal:
		property Ogre::Viewport* UnmanagedPointer
		{
			Ogre::Viewport* get();
		}
	};
}