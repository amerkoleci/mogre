#pragma once

#include "OgreResource.h"
#include "MogreStringInterface.h"
#include "MogreCommon.h"

namespace Mogre
{
	typedef Ogre::ResourceHandle ResourceHandle;
	ref class Resource;

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

	public ref class Resource : /*public IStringInterface,*/ public IResource_Listener_Receiver
	{
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

	public protected:
		Resource(intptr_t ptr) : _native((Ogre::Resource*)ptr)
		{
		}

		~Resource()
		{
			if (_listener != 0)
			{
				if (_native != 0)
					_native->removeListener(_listener);

				delete _listener;
				_listener = 0;
			}
		}

		//virtual Ogre::StringInterface* _IStringInterface_GetNativePtr() = IStringInterface::_GetNativePtr;

	public:
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
}