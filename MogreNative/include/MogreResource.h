#pragma once

#include "OgreResource.h"
#include "MogreStringInterface.h"
#include "MogreCommon.h"

namespace Mogre
{
	typedef Ogre::ResourceHandle ResourceHandle;
	ref class Resource;
	ref class ResourceManager;

	interface class IResource_Listener_Receiver
	{
		void LoadingComplete(Mogre::Resource^ param1);
		void PreparingComplete(Mogre::Resource^ param1);
		void UnloadingComplete(Mogre::Resource^ param1);
	};

	class Resource_Listener_Director : public Ogre::Resource::Listener
	{
	private:
		gcroot<IResource_Listener_Receiver^> _receiver;
	public:
		Resource_Listener_Director(IResource_Listener_Receiver^ recv)
			: _receiver(recv), doCallForLoadingComplete(false), doCallForPreparingComplete(false), doCallForUnloadingComplete(false)
		{
		}

		bool doCallForLoadingComplete;
		bool doCallForPreparingComplete;
		bool doCallForUnloadingComplete;

		virtual void loadingComplete(Ogre::Resource* param1) override;
		virtual void preparingComplete(Ogre::Resource* param1) override;
		virtual void unloadingComplete(Ogre::Resource* param1) override;
	};

	public ref class Resource : IMogreDisposable, /*public IStringInterface,*/ public IResource_Listener_Receiver
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	public:
		ref class Listener;

		enum class LoadingState
		{
			LOADSTATE_UNLOADED = Ogre::Resource::LOADSTATE_UNLOADED,
			LOADSTATE_LOADING = Ogre::Resource::LOADSTATE_LOADING,
			LOADSTATE_LOADED = Ogre::Resource::LOADSTATE_LOADED,
			LOADSTATE_UNLOADING = Ogre::Resource::LOADSTATE_UNLOADING,
			Unloaded = Ogre::Resource::LOADSTATE_UNLOADED,
			Loading = Ogre::Resource::LOADSTATE_LOADING,
			Loaded = Ogre::Resource::LOADSTATE_LOADED,
			Unloading = Ogre::Resource::LOADSTATE_UNLOADING
		};

		ref class Listener abstract sealed
		{
		public:
			delegate static void LoadingCompleteHandler(Mogre::Resource^ param1);
			delegate static void PreparingCompleteHandler(Mogre::Resource^ param1);
			delegate static void UnloadingCompleteHandler(Mogre::Resource^ param1);
		};

	private protected:
		Resource_Listener_Director* _listener;
		Mogre::Resource::Listener::LoadingCompleteHandler^ _loadingComplete;
		Mogre::Resource::Listener::PreparingCompleteHandler^ _preparingComplete;
		Mogre::Resource::Listener::UnloadingCompleteHandler^ _unloadingComplete;

	internal:
		Ogre::Resource* _native;
		bool _createdByCLR;

	public protected:
		Resource(intptr_t ptr) : _native((Ogre::Resource*)ptr)
		{
		}

		Resource(Ogre::Resource* ptr) : _native(ptr)
		{
		}

		~Resource()
		{
			this->!Resource();
		}

		!Resource()
		{
			OnDisposing(this, nullptr);

			if (IsDisposed)
				return;

			if (_listener != 0)
			{
				if (_native != 0)
					_native->removeListener(_listener);

				delete _listener;
				_listener = 0;
			}

			OnDisposed(this, nullptr);
		}

		//virtual Ogre::StringInterface* _IStringInterface_GetNativePtr() = IStringInterface::_GetNativePtr;

	public:
		property bool IsDisposed
		{
			virtual bool get() { return _native == nullptr; }
		}

		event Mogre::Resource::Listener::LoadingCompleteHandler^ LoadingComplete
		{
			void add(Mogre::Resource::Listener::LoadingCompleteHandler^ hnd)
			{
				if (_loadingComplete == CLR_NULL)
				{
					if (_listener == 0)
					{
						_listener = new Resource_Listener_Director(this);
						_native->addListener(_listener);
					}
					_listener->doCallForLoadingComplete = true;
				}
				_loadingComplete += hnd;
			}
			void remove(Mogre::Resource::Listener::LoadingCompleteHandler^ hnd)
			{
				_loadingComplete -= hnd;
				if (_loadingComplete == CLR_NULL) _listener->doCallForLoadingComplete = false;
			}
		private:
			void raise(Mogre::Resource^ param1)
			{
				if (_loadingComplete)
					_loadingComplete->Invoke(param1);
			}
		}

		event Mogre::Resource::Listener::PreparingCompleteHandler^ PreparingComplete
		{
			void add(Mogre::Resource::Listener::PreparingCompleteHandler^ hnd)
			{
				if (_preparingComplete == CLR_NULL)
				{
					if (_listener == 0)
					{
						_listener = new Resource_Listener_Director(this);
						_native->addListener(_listener);
					}
					_listener->doCallForPreparingComplete = true;
				}
				_preparingComplete += hnd;
			}
			void remove(Mogre::Resource::Listener::PreparingCompleteHandler^ hnd)
			{
				_preparingComplete -= hnd;
				if (_preparingComplete == CLR_NULL) _listener->doCallForPreparingComplete = false;
			}
		private:
			void raise(Mogre::Resource^ param1)
			{
				if (_preparingComplete)
					_preparingComplete->Invoke(param1);
			}
		}

		event Mogre::Resource::Listener::UnloadingCompleteHandler^ UnloadingComplete
		{
			void add(Mogre::Resource::Listener::UnloadingCompleteHandler^ hnd)
			{
				if (_unloadingComplete == CLR_NULL)
				{
					if (_listener == 0)
					{
						_listener = new Resource_Listener_Director(this);
						_native->addListener(_listener);
					}
					_listener->doCallForUnloadingComplete = true;
				}
				_unloadingComplete += hnd;
			}
			void remove(Mogre::Resource::Listener::UnloadingCompleteHandler^ hnd)
			{
				_unloadingComplete -= hnd;
				if (_unloadingComplete == CLR_NULL) _listener->doCallForUnloadingComplete = false;
			}
		private:
			void raise(Mogre::Resource^ param1)
			{
				if (_unloadingComplete)
					_unloadingComplete->Invoke(param1);
			}
		}

		property Mogre::ResourceManager^ Creator
		{
		public:
			Mogre::ResourceManager^ get();
		}

		property String^ Group
		{
		public:
			String^ get();
		}

		property Mogre::ResourceHandle Handle
		{
		public:
			Mogre::ResourceHandle get();
		}

		property bool IsBackgroundLoaded
		{
		public:
			bool get();
		}

		property bool IsLoaded
		{
		public:
			bool get();
		}

		property bool IsManuallyLoaded
		{
		public:
			bool get();
		}

		property bool IsReloadable
		{
		public:
			bool get();
		}

		property String^ Name
		{
		public:
			String^ get();
		}

		property String^ Origin
		{
		public:
			String^ get();
		}

		property size_t Size
		{
		public:
			size_t get();
		}

		void Load(bool backgroundThread);
		void Load();

		void Reload();

		void Unload();

		void Touch();

		Mogre::Resource::LoadingState IsLoading();

		Mogre::Resource::LoadingState GetLoadingState();

		void SetBackgroundLoaded(bool bl);

		void EscalateLoading();

		void ChangeGroupOwnership(String^ newGroup);

		DEFINE_MANAGED_NATIVE_CONVERSIONS(Resource);

	internal:
		property Ogre::Resource* UnmanagedPointer
		{
			Ogre::Resource* get()
			{
				return _native;
			}
		}

	protected public:
		virtual void OnLoadingComplete(Mogre::Resource^ param1) = IResource_Listener_Receiver::LoadingComplete
		{
			LoadingComplete(param1);
		}

			virtual void OnPreparingComplete(Mogre::Resource^ param1) = IResource_Listener_Receiver::PreparingComplete
		{
			PreparingComplete(param1);
		}

			virtual void OnUnloadingComplete(Mogre::Resource^ param1) = IResource_Listener_Receiver::UnloadingComplete
		{
			UnloadingComplete(param1);
		}
	};

	public ref class ResourcePtr : public Resource
	{
	public protected:
		Ogre::ResourcePtr* _sharedPtr;

		ResourcePtr(Ogre::ResourcePtr& sharedPtr) : Resource(sharedPtr.getPointer())
		{
			_sharedPtr = new Ogre::ResourcePtr(sharedPtr);
		}

		!ResourcePtr()
		{
			if (_sharedPtr != 0)
			{
				delete _sharedPtr;
				_sharedPtr = 0;
			}
		}

		~ResourcePtr()
		{
			this->!ResourcePtr();
		}

	public:
		DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_SHAREDPTR(ResourcePtr);

		ResourcePtr(Resource^ obj) : Resource(obj->_native)
		{
			_sharedPtr = new Ogre::ResourcePtr(static_cast<Ogre::Resource*>(obj->_native));
		}

		virtual bool Equals(Object^ obj) override
		{
			ResourcePtr^ clr = dynamic_cast<ResourcePtr^>(obj);
			if (clr == CLR_NULL)
			{
				return false;
			}

			return (_native == clr->_native);
		}
		bool Equals(ResourcePtr^ obj)
		{
			if (obj == CLR_NULL)
			{
				return false;
			}

			return (_native == obj->_native);
		}

		static bool operator == (ResourcePtr^ val1, ResourcePtr^ val2)
		{
			if ((Object^)val1 == (Object^)val2) return true;
			if ((Object^)val1 == nullptr || (Object^)val2 == nullptr) return false;
			return (val1->_native == val2->_native);
		}

		static bool operator != (ResourcePtr^ val1, ResourcePtr^ val2)
		{
			return !(val1 == val2);
		}

		virtual int GetHashCode() override
		{
			return reinterpret_cast<int>(_native);
		}

		property IntPtr NativePtr
		{
			IntPtr get() { return (IntPtr)_sharedPtr; }
		}

		property bool Unique
		{
			bool get()
			{
				return (*_sharedPtr).unique();
			}
		}

		property int UseCount
		{
			int get()
			{
				return (*_sharedPtr).useCount();
			}
		}

		property Resource^ Target
		{
			Resource^ get()
			{
				return ObjectTable::GetOrCreateObject<Mogre::Resource^>((intptr_t)static_cast<Ogre::Resource*>(_native));
			}
		}
	};
}