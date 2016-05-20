#pragma once

#include "OgreRenderTarget.h"
#include "Marshalling.h"

namespace Mogre
{
	ref class Viewport;
	ref class RenderTarget;

	public value class RenderTargetViewportEvent_NativePtr
	{
	private protected:
		Ogre::RenderTargetViewportEvent* _native;

	public:

		property Mogre::Viewport^ source
		{
		public:
			Mogre::Viewport^ get();
		}

		DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_NATIVEPTRVALUECLASS(RenderTargetViewportEvent_NativePtr, Ogre::RenderTargetViewportEvent);


		property IntPtr NativePtr
		{
			IntPtr get() { return (IntPtr)_native; }
		}

		property bool IsNull
		{
			bool get() { return (_native == 0); }
		}
	};

	public value class RenderTargetEvent_NativePtr
	{
	private protected:
		Ogre::RenderTargetEvent* _native;

	public:
		property Mogre::RenderTarget^ source
		{
		public:
			Mogre::RenderTarget^ get();
		}

		DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_NATIVEPTRVALUECLASS(RenderTargetEvent_NativePtr, Ogre::RenderTargetEvent);


		property IntPtr NativePtr
		{
			IntPtr get() { return (IntPtr)_native; }
		}

		property bool IsNull
		{
			bool get() { return (_native == 0); }
		}
	};

	interface class IRenderTargetListener_Receiver
	{
		void PreRenderTargetUpdate(Mogre::RenderTargetEvent_NativePtr evt);

		void PostRenderTargetUpdate(Mogre::RenderTargetEvent_NativePtr evt);

		void PreViewportUpdate(Mogre::RenderTargetViewportEvent_NativePtr evt);

		void PostViewportUpdate(Mogre::RenderTargetViewportEvent_NativePtr evt);

		void ViewportAdded(Mogre::RenderTargetViewportEvent_NativePtr evt);

		void ViewportRemoved(Mogre::RenderTargetViewportEvent_NativePtr evt);

	};

	public ref class RenderTargetListener abstract sealed
	{
	public:
		delegate static void PreRenderTargetUpdateHandler(Mogre::RenderTargetEvent_NativePtr evt);
		delegate static void PostRenderTargetUpdateHandler(Mogre::RenderTargetEvent_NativePtr evt);
		delegate static void PreViewportUpdateHandler(Mogre::RenderTargetViewportEvent_NativePtr evt);
		delegate static void PostViewportUpdateHandler(Mogre::RenderTargetViewportEvent_NativePtr evt);
		delegate static void ViewportAddedHandler(Mogre::RenderTargetViewportEvent_NativePtr evt);
		delegate static void ViewportRemovedHandler(Mogre::RenderTargetViewportEvent_NativePtr evt);
	};

	class RenderTargetListener_Director : public Ogre::RenderTargetListener
	{
	private:
		gcroot<IRenderTargetListener_Receiver^> _receiver;

	public:
		RenderTargetListener_Director(IRenderTargetListener_Receiver^ recv)
			: _receiver(recv), doCallForPreRenderTargetUpdate(false), doCallForPostRenderTargetUpdate(false), doCallForPreViewportUpdate(false), doCallForPostViewportUpdate(false), doCallForViewportAdded(false), doCallForViewportRemoved(false)
		{
		}

		bool doCallForPreRenderTargetUpdate;
		bool doCallForPostRenderTargetUpdate;
		bool doCallForPreViewportUpdate;
		bool doCallForPostViewportUpdate;
		bool doCallForViewportAdded;
		bool doCallForViewportRemoved;

		virtual void preRenderTargetUpdate(const Ogre::RenderTargetEvent& evt) override;

		virtual void postRenderTargetUpdate(const Ogre::RenderTargetEvent& evt) override;

		virtual void preViewportUpdate(const Ogre::RenderTargetViewportEvent& evt) override;

		virtual void postViewportUpdate(const Ogre::RenderTargetViewportEvent& evt) override;

		virtual void viewportAdded(const Ogre::RenderTargetViewportEvent& evt) override;

		virtual void viewportRemoved(const Ogre::RenderTargetViewportEvent& evt) override;
	};


	public ref class RenderTarget : IDisposable, public IRenderTargetListener_Receiver
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	public:
		enum class StatFlags
		{
			SF_NONE = Ogre::RenderTarget::SF_NONE,
			SF_FPS = Ogre::RenderTarget::SF_FPS,
			SF_AVG_FPS = Ogre::RenderTarget::SF_AVG_FPS,
			SF_BEST_FPS = Ogre::RenderTarget::SF_BEST_FPS,
			SF_WORST_FPS = Ogre::RenderTarget::SF_WORST_FPS,
			SF_TRIANGLE_COUNT = Ogre::RenderTarget::SF_TRIANGLE_COUNT,
			SF_ALL = Ogre::RenderTarget::SF_ALL
		};

		ref class FrameStats
		{
		public protected:
			FrameStats()
			{
			}

			size_t _batchCount;
			size_t _triangleCount;
			int _vBlankMissCount;

			static operator FrameStats ^ (const Ogre::RenderTarget::FrameStats& obj)
			{
				FrameStats^ clr = gcnew FrameStats;
				clr->_batchCount = obj.batchCount;
				clr->_triangleCount = obj.triangleCount;
				clr->_vBlankMissCount = obj.vBlankMissCount;

				return clr;
			}

			static operator FrameStats ^ (const Ogre::RenderTarget::FrameStats* pObj)
			{
				return *pObj;
			}
		public:
			property size_t BatchCount
			{
				size_t get()
				{
					return _batchCount;
				}
			}

			property size_t TriangleCount
			{
				size_t get()
				{
					return _triangleCount;
				}
			}

			property int VBlankMissCount
			{
				int get()
				{
					return _vBlankMissCount;
				}
			}
		};

	private protected:

		//Event and Listener fields
		RenderTargetListener_Director* _renderTargetListener;
		Mogre::RenderTargetListener::PreRenderTargetUpdateHandler^ _preRenderTargetUpdate;
		Mogre::RenderTargetListener::PostRenderTargetUpdateHandler^ _postRenderTargetUpdate;
		Mogre::RenderTargetListener::PreViewportUpdateHandler^ _preViewportUpdate;
		Mogre::RenderTargetListener::PostViewportUpdateHandler^ _postViewportUpdate;
		Mogre::RenderTargetListener::ViewportAddedHandler^ _viewportAdded;
		Mogre::RenderTargetListener::ViewportRemovedHandler^ _viewportRemoved;

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
		event Mogre::RenderTargetListener::PreRenderTargetUpdateHandler^ PreRenderTargetUpdate
		{
			void add(Mogre::RenderTargetListener::PreRenderTargetUpdateHandler^ hnd)
			{
				if (_preRenderTargetUpdate == CLR_NULL)
				{
					if (_renderTargetListener == 0)
					{
						_renderTargetListener = new RenderTargetListener_Director(this);
						static_cast<Ogre::RenderTarget*>(_native)->addListener(_renderTargetListener);
					}
					_renderTargetListener->doCallForPreRenderTargetUpdate = true;
				}
				_preRenderTargetUpdate += hnd;
			}
			void remove(Mogre::RenderTargetListener::PreRenderTargetUpdateHandler^ hnd)
			{
				_preRenderTargetUpdate -= hnd;
				if (_preRenderTargetUpdate == CLR_NULL) _renderTargetListener->doCallForPreRenderTargetUpdate = false;
			}
		private:
			void raise(Mogre::RenderTargetEvent_NativePtr evt)
			{
				if (_preRenderTargetUpdate)
					_preRenderTargetUpdate->Invoke(evt);
			}
		}

		event Mogre::RenderTargetListener::PostRenderTargetUpdateHandler^ PostRenderTargetUpdate
		{
			void add(Mogre::RenderTargetListener::PostRenderTargetUpdateHandler^ hnd)
			{
				if (_postRenderTargetUpdate == CLR_NULL)
				{
					if (_renderTargetListener == 0)
					{
						_renderTargetListener = new RenderTargetListener_Director(this);
						static_cast<Ogre::RenderTarget*>(_native)->addListener(_renderTargetListener);
					}
					_renderTargetListener->doCallForPostRenderTargetUpdate = true;
				}
				_postRenderTargetUpdate += hnd;
			}
			void remove(Mogre::RenderTargetListener::PostRenderTargetUpdateHandler^ hnd)
			{
				_postRenderTargetUpdate -= hnd;
				if (_postRenderTargetUpdate == CLR_NULL) _renderTargetListener->doCallForPostRenderTargetUpdate = false;
			}
		private:
			void raise(Mogre::RenderTargetEvent_NativePtr evt)
			{
				if (_postRenderTargetUpdate)
					_postRenderTargetUpdate->Invoke(evt);
			}
		}

		event Mogre::RenderTargetListener::PreViewportUpdateHandler^ PreViewportUpdate
		{
			void add(Mogre::RenderTargetListener::PreViewportUpdateHandler^ hnd)
			{
				if (_preViewportUpdate == CLR_NULL)
				{
					if (_renderTargetListener == 0)
					{
						_renderTargetListener = new RenderTargetListener_Director(this);
						static_cast<Ogre::RenderTarget*>(_native)->addListener(_renderTargetListener);
					}
					_renderTargetListener->doCallForPreViewportUpdate = true;
				}
				_preViewportUpdate += hnd;
			}
			void remove(Mogre::RenderTargetListener::PreViewportUpdateHandler^ hnd)
			{
				_preViewportUpdate -= hnd;
				if (_preViewportUpdate == CLR_NULL) _renderTargetListener->doCallForPreViewportUpdate = false;
			}
		private:
			void raise(Mogre::RenderTargetViewportEvent_NativePtr evt)
			{
				if (_preViewportUpdate)
					_preViewportUpdate->Invoke(evt);
			}
		}

		event Mogre::RenderTargetListener::PostViewportUpdateHandler^ PostViewportUpdate
		{
			void add(Mogre::RenderTargetListener::PostViewportUpdateHandler^ hnd)
			{
				if (_postViewportUpdate == CLR_NULL)
				{
					if (_renderTargetListener == 0)
					{
						_renderTargetListener = new RenderTargetListener_Director(this);
						static_cast<Ogre::RenderTarget*>(_native)->addListener(_renderTargetListener);
					}
					_renderTargetListener->doCallForPostViewportUpdate = true;
				}
				_postViewportUpdate += hnd;
			}
			void remove(Mogre::RenderTargetListener::PostViewportUpdateHandler^ hnd)
			{
				_postViewportUpdate -= hnd;
				if (_postViewportUpdate == CLR_NULL) _renderTargetListener->doCallForPostViewportUpdate = false;
			}
		private:
			void raise(Mogre::RenderTargetViewportEvent_NativePtr evt)
			{
				if (_postViewportUpdate)
					_postViewportUpdate->Invoke(evt);
			}
		}

		event Mogre::RenderTargetListener::ViewportAddedHandler^ ViewportAdded
		{
			void add(Mogre::RenderTargetListener::ViewportAddedHandler^ hnd)
			{
				if (_viewportAdded == CLR_NULL)
				{
					if (_renderTargetListener == 0)
					{
						_renderTargetListener = new RenderTargetListener_Director(this);
						static_cast<Ogre::RenderTarget*>(_native)->addListener(_renderTargetListener);
					}
					_renderTargetListener->doCallForViewportAdded = true;
				}
				_viewportAdded += hnd;
			}
			void remove(Mogre::RenderTargetListener::ViewportAddedHandler^ hnd)
			{
				_viewportAdded -= hnd;
				if (_viewportAdded == CLR_NULL) _renderTargetListener->doCallForViewportAdded = false;
			}
		private:
			void raise(Mogre::RenderTargetViewportEvent_NativePtr evt)
			{
				if (_viewportAdded)
					_viewportAdded->Invoke(evt);
			}
		}

		event Mogre::RenderTargetListener::ViewportRemovedHandler^ ViewportRemoved
		{
			void add(Mogre::RenderTargetListener::ViewportRemovedHandler^ hnd)
			{
				if (_viewportRemoved == CLR_NULL)
				{
					if (_renderTargetListener == 0)
					{
						_renderTargetListener = new RenderTargetListener_Director(this);
						static_cast<Ogre::RenderTarget*>(_native)->addListener(_renderTargetListener);
					}
					_renderTargetListener->doCallForViewportRemoved = true;
				}
				_viewportRemoved += hnd;
			}
			void remove(Mogre::RenderTargetListener::ViewportRemovedHandler^ hnd)
			{
				_viewportRemoved -= hnd;
				if (_viewportRemoved == CLR_NULL) _renderTargetListener->doCallForViewportRemoved = false;
			}
		private:
			void raise(Mogre::RenderTargetViewportEvent_NativePtr evt)
			{
				if (_viewportRemoved)
					_viewportRemoved->Invoke(evt);
			}
		}

		property bool IsDisposed
		{
			virtual bool get();
		}

		property unsigned short NumViewports
		{
		public:
			unsigned short get();
		}

		property Ogre::uchar Priority
		{
		public:
			Ogre::uchar get();
		public:
			void set(Ogre::uchar priority);
		}

		property size_t TriangleCount
		{
		public:
			size_t get();
		}

		property size_t BatchCount
		{
		public:
			size_t get();
		}

		Mogre::Viewport^ AddViewport(float left, float top, float width, float height);
		Mogre::Viewport^ AddViewport(float left, float top, float width);
		Mogre::Viewport^ AddViewport(float left, float top);
		Mogre::Viewport^ AddViewport(float left);
		Mogre::Viewport^ AddViewport();

		Mogre::Viewport^ GetViewport(unsigned short index);

		void RemoveViewport(Mogre::Viewport^ viewport);

		void RemoveAllViewports();

		Mogre::RenderTarget::FrameStats^ GetStatistics();
		void ResetStatistics();

	internal:
		property Ogre::RenderTarget* UnmanagedPointer
		{
			Ogre::RenderTarget* get();
		}

	protected public:
		virtual void OnPreRenderTargetUpdate(Mogre::RenderTargetEvent_NativePtr evt) = IRenderTargetListener_Receiver::PreRenderTargetUpdate
		{
			PreRenderTargetUpdate(evt);
		}

			virtual void OnPostRenderTargetUpdate(Mogre::RenderTargetEvent_NativePtr evt) = IRenderTargetListener_Receiver::PostRenderTargetUpdate
		{
			PostRenderTargetUpdate(evt);
		}

			virtual void OnPreViewportUpdate(Mogre::RenderTargetViewportEvent_NativePtr evt) = IRenderTargetListener_Receiver::PreViewportUpdate
		{
			PreViewportUpdate(evt);
		}

			virtual void OnPostViewportUpdate(Mogre::RenderTargetViewportEvent_NativePtr evt) = IRenderTargetListener_Receiver::PostViewportUpdate
		{
			PostViewportUpdate(evt);
		}

			virtual void OnViewportAdded(Mogre::RenderTargetViewportEvent_NativePtr evt) = IRenderTargetListener_Receiver::ViewportAdded
		{
			ViewportAdded(evt);
		}

			virtual void OnViewportRemoved(Mogre::RenderTargetViewportEvent_NativePtr evt) = IRenderTargetListener_Receiver::ViewportRemoved
		{
			ViewportRemoved(evt);
		}
	};
}