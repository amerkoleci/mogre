#pragma once

#include "OgreResourceGroupManager.h"
#include "MogreCommon.h"
#include "MogreStringVector.h"

namespace Mogre
{
	ref class Resource;
	ref class ResourcePtr;
	ref class DataStreamPtr;
	ref class DataStreamListPtr;

	interface class IResourceGroupListener_Receiver
	{
		void ResourceGroupScriptingStarted(String^ groupName, size_t scriptCount);

		void ScriptParseStarted(String^ scriptName);

		void ScriptParseEnded(String^ scriptName);

		void ResourceGroupScriptingEnded(String^ groupName);

		void ResourceGroupLoadStarted(String^ groupName, size_t resourceCount);

		void ResourceLoadStarted(Mogre::ResourcePtr^ resource);

		void ResourceLoadEnded();

		void WorldGeometryStageStarted(String^ description);

		void WorldGeometryStageEnded();

		void ResourceGroupLoadEnded(String^ groupName);

	};

	public ref class ResourceGroupListener abstract sealed
	{
	public:
		delegate static void ResourceGroupScriptingStartedHandler(String^ groupName, size_t scriptCount);
		delegate static void ScriptParseStartedHandler(String^ scriptName);
		delegate static void ScriptParseEndedHandler(String^ scriptName);
		delegate static void ResourceGroupScriptingEndedHandler(String^ groupName);
		delegate static void ResourceGroupLoadStartedHandler(String^ groupName, size_t resourceCount);
		delegate static void ResourceLoadStartedHandler(Mogre::ResourcePtr^ resource);
		delegate static void ResourceLoadEndedHandler();
		delegate static void WorldGeometryStageStartedHandler(String^ description);
		delegate static void WorldGeometryStageEndedHandler();
		delegate static void ResourceGroupLoadEndedHandler(String^ groupName);
	};

	class ResourceGroupListener_Director : public Ogre::ResourceGroupListener
	{
	private:
		gcroot<IResourceGroupListener_Receiver^> _receiver;

	public:
		ResourceGroupListener_Director(IResourceGroupListener_Receiver^ recv)
			: _receiver(recv), doCallForResourceGroupScriptingStarted(false), doCallForScriptParseStarted(false), doCallForScriptParseEnded(false), doCallForResourceGroupScriptingEnded(false), doCallForResourceGroupLoadStarted(false), doCallForResourceLoadStarted(false), doCallForResourceLoadEnded(false), doCallForWorldGeometryStageStarted(false), doCallForWorldGeometryStageEnded(false), doCallForResourceGroupLoadEnded(false)
		{
		}

		bool doCallForResourceGroupScriptingStarted;
		bool doCallForScriptParseStarted;
		bool doCallForScriptParseEnded;
		bool doCallForResourceGroupScriptingEnded;
		bool doCallForResourceGroupLoadStarted;
		bool doCallForResourceLoadStarted;
		bool doCallForResourceLoadEnded;
		bool doCallForWorldGeometryStageStarted;
		bool doCallForWorldGeometryStageEnded;
		bool doCallForResourceGroupLoadEnded;

		virtual void resourceGroupScriptingStarted(const Ogre::String& groupName, size_t scriptCount) override;

		virtual void scriptParseStarted(const Ogre::String& scriptName, bool& skipThisScript) override;

		virtual void scriptParseEnded(const Ogre::String& scriptName, bool skipped) override;

		virtual void resourceGroupScriptingEnded(const Ogre::String& groupName) override;

		virtual void resourceGroupLoadStarted(const Ogre::String& groupName, size_t resourceCount) override;

		virtual void resourceLoadStarted(const Ogre::ResourcePtr& resource) override;

		virtual void resourceLoadEnded() override;

		virtual void worldGeometryStageStarted(const Ogre::String& description) override;

		virtual void worldGeometryStageEnded() override;

		virtual void resourceGroupLoadEnded(const Ogre::String& groupName) override;
	};

	public ref class ResourceGroupManager : public IResourceGroupListener_Receiver
	{
	private protected:
		//Event and Listener fields
		ResourceGroupListener_Director* _resourceGroupListener;
		Mogre::ResourceGroupListener::ResourceGroupScriptingStartedHandler^ _resourceGroupScriptingStarted;
		Mogre::ResourceGroupListener::ScriptParseStartedHandler^ _scriptParseStarted;
		Mogre::ResourceGroupListener::ScriptParseEndedHandler^ _scriptParseEnded;
		Mogre::ResourceGroupListener::ResourceGroupScriptingEndedHandler^ _resourceGroupScriptingEnded;
		Mogre::ResourceGroupListener::ResourceGroupLoadStartedHandler^ _resourceGroupLoadStarted;
		Mogre::ResourceGroupListener::ResourceLoadStartedHandler^ _resourceLoadStarted;
		Mogre::ResourceGroupListener::ResourceLoadEndedHandler^ _resourceLoadEnded;
		Mogre::ResourceGroupListener::WorldGeometryStageStartedHandler^ _worldGeometryStageStarted;
		Mogre::ResourceGroupListener::WorldGeometryStageEndedHandler^ _worldGeometryStageEnded;
		Mogre::ResourceGroupListener::ResourceGroupLoadEndedHandler^ _resourceGroupLoadEnded;

		static ResourceGroupManager^ _singleton;
		Ogre::ResourceGroupManager* _native;
		bool _createdByCLR;

	public protected:
		ResourceGroupManager(Ogre::ResourceGroupManager* obj) : _native(obj)
		{
		}

		~ResourceGroupManager()
		{
			this->!ResourceGroupManager();
		}
		!ResourceGroupManager()
		{
			_native = Ogre::ResourceGroupManager::getSingletonPtr();
			if (_resourceGroupListener != 0)
			{
				if (_native != 0)
					_native->removeResourceGroupListener(_resourceGroupListener);

				delete _resourceGroupListener;
				_resourceGroupListener = 0;
			}

			if (_createdByCLR && _native)
			{
				delete _native; _native = 0;
			}
			_singleton = nullptr;
		}

	public:
		ResourceGroupManager();

		static property ResourceGroupManager^ Singleton
		{
			ResourceGroupManager^ get()
			{
				if (_singleton == CLR_NULL)
				{
					Ogre::ResourceGroupManager* ptr = Ogre::ResourceGroupManager::getSingletonPtr();
					if (ptr) _singleton = gcnew ResourceGroupManager(ptr);
				}
				return _singleton;
			}
		}

		static property String^ DEFAULT_RESOURCE_GROUP_NAME
		{
		public:
			String^ get();
		public:
			void set(String^ value);
		}

		static property String^ INTERNAL_RESOURCE_GROUP_NAME
		{
		public:
			String^ get();
		public:
			void set(String^ value);
		}

		static property String^ AUTODETECT_RESOURCE_GROUP_NAME
		{
		public:
			String^ get();
		public:
			void set(String^ value);
		}

		static property size_t RESOURCE_SYSTEM_NUM_REFERENCE_COUNTS
		{
		public:
			size_t get();
		public:
			void set(size_t value);
		}

		property String^ WorldResourceGroupName
		{
		public:
			String^ get();
		public:
			void set(String^ groupName);
		}

		event Mogre::ResourceGroupListener::ResourceGroupScriptingStartedHandler^ ResourceGroupScriptingStarted
		{
			void add(Mogre::ResourceGroupListener::ResourceGroupScriptingStartedHandler^ hnd)
			{
				if (_resourceGroupScriptingStarted == CLR_NULL)
				{
					if (_resourceGroupListener == 0)
					{
						_resourceGroupListener = new ResourceGroupListener_Director(this);
						static_cast<Ogre::ResourceGroupManager*>(_native)->addResourceGroupListener(_resourceGroupListener);
					}
					_resourceGroupListener->doCallForResourceGroupScriptingStarted = true;
				}
				_resourceGroupScriptingStarted += hnd;
			}
			void remove(Mogre::ResourceGroupListener::ResourceGroupScriptingStartedHandler^ hnd)
			{
				_resourceGroupScriptingStarted -= hnd;
				if (_resourceGroupScriptingStarted == CLR_NULL) _resourceGroupListener->doCallForResourceGroupScriptingStarted = false;
			}
		private:
			void raise(String^ groupName, size_t scriptCount)
			{
				if (_resourceGroupScriptingStarted)
					_resourceGroupScriptingStarted->Invoke(groupName, scriptCount);
			}
		}

		event Mogre::ResourceGroupListener::ScriptParseStartedHandler^ ScriptParseStarted
		{
			void add(Mogre::ResourceGroupListener::ScriptParseStartedHandler^ hnd)
			{
				if (_scriptParseStarted == CLR_NULL)
				{
					if (_resourceGroupListener == 0)
					{
						_resourceGroupListener = new ResourceGroupListener_Director(this);
						static_cast<Ogre::ResourceGroupManager*>(_native)->addResourceGroupListener(_resourceGroupListener);
					}
					_resourceGroupListener->doCallForScriptParseStarted = true;
				}
				_scriptParseStarted += hnd;
			}
			void remove(Mogre::ResourceGroupListener::ScriptParseStartedHandler^ hnd)
			{
				_scriptParseStarted -= hnd;
				if (_scriptParseStarted == CLR_NULL) _resourceGroupListener->doCallForScriptParseStarted = false;
			}
		private:
			void raise(String^ scriptName)
			{
				if (_scriptParseStarted)
					_scriptParseStarted->Invoke(scriptName);
			}
		}

		event Mogre::ResourceGroupListener::ScriptParseEndedHandler^ ScriptParseEnded
		{
			void add(Mogre::ResourceGroupListener::ScriptParseEndedHandler^ hnd)
			{
				if (_scriptParseEnded == CLR_NULL)
				{
					if (_resourceGroupListener == 0)
					{
						_resourceGroupListener = new ResourceGroupListener_Director(this);
						static_cast<Ogre::ResourceGroupManager*>(_native)->addResourceGroupListener(_resourceGroupListener);
					}
					_resourceGroupListener->doCallForScriptParseEnded = true;
				}
				_scriptParseEnded += hnd;
			}
			void remove(Mogre::ResourceGroupListener::ScriptParseEndedHandler^ hnd)
			{
				_scriptParseEnded -= hnd;
				if (_scriptParseEnded == CLR_NULL) _resourceGroupListener->doCallForScriptParseEnded = false;
			}
		private:
			void raise(String^ scriptName)
			{
				if (_scriptParseEnded)
					_scriptParseEnded->Invoke(scriptName);
			}
		}

		event Mogre::ResourceGroupListener::ResourceGroupScriptingEndedHandler^ ResourceGroupScriptingEnded
		{
			void add(Mogre::ResourceGroupListener::ResourceGroupScriptingEndedHandler^ hnd)
			{
				if (_resourceGroupScriptingEnded == CLR_NULL)
				{
					if (_resourceGroupListener == 0)
					{
						_resourceGroupListener = new ResourceGroupListener_Director(this);
						static_cast<Ogre::ResourceGroupManager*>(_native)->addResourceGroupListener(_resourceGroupListener);
					}
					_resourceGroupListener->doCallForResourceGroupScriptingEnded = true;
				}
				_resourceGroupScriptingEnded += hnd;
			}
			void remove(Mogre::ResourceGroupListener::ResourceGroupScriptingEndedHandler^ hnd)
			{
				_resourceGroupScriptingEnded -= hnd;
				if (_resourceGroupScriptingEnded == CLR_NULL) _resourceGroupListener->doCallForResourceGroupScriptingEnded = false;
			}
		private:
			void raise(String^ groupName)
			{
				if (_resourceGroupScriptingEnded)
					_resourceGroupScriptingEnded->Invoke(groupName);
			}
		}

		event Mogre::ResourceGroupListener::ResourceGroupLoadStartedHandler^ ResourceGroupLoadStarted
		{
			void add(Mogre::ResourceGroupListener::ResourceGroupLoadStartedHandler^ hnd)
			{
				if (_resourceGroupLoadStarted == CLR_NULL)
				{
					if (_resourceGroupListener == 0)
					{
						_resourceGroupListener = new ResourceGroupListener_Director(this);
						static_cast<Ogre::ResourceGroupManager*>(_native)->addResourceGroupListener(_resourceGroupListener);
					}
					_resourceGroupListener->doCallForResourceGroupLoadStarted = true;
				}
				_resourceGroupLoadStarted += hnd;
			}
			void remove(Mogre::ResourceGroupListener::ResourceGroupLoadStartedHandler^ hnd)
			{
				_resourceGroupLoadStarted -= hnd;
				if (_resourceGroupLoadStarted == CLR_NULL) _resourceGroupListener->doCallForResourceGroupLoadStarted = false;
			}
		private:
			void raise(String^ groupName, size_t resourceCount)
			{
				if (_resourceGroupLoadStarted)
					_resourceGroupLoadStarted->Invoke(groupName, resourceCount);
			}
		}

		event Mogre::ResourceGroupListener::ResourceLoadStartedHandler^ ResourceLoadStarted
		{
			void add(Mogre::ResourceGroupListener::ResourceLoadStartedHandler^ hnd)
			{
				if (_resourceLoadStarted == CLR_NULL)
				{
					if (_resourceGroupListener == 0)
					{
						_resourceGroupListener = new ResourceGroupListener_Director(this);
						static_cast<Ogre::ResourceGroupManager*>(_native)->addResourceGroupListener(_resourceGroupListener);
					}
					_resourceGroupListener->doCallForResourceLoadStarted = true;
				}
				_resourceLoadStarted += hnd;
			}
			void remove(Mogre::ResourceGroupListener::ResourceLoadStartedHandler^ hnd)
			{
				_resourceLoadStarted -= hnd;
				if (_resourceLoadStarted == CLR_NULL) _resourceGroupListener->doCallForResourceLoadStarted = false;
			}
		private:
			void raise(Mogre::ResourcePtr^ resource)
			{
				if (_resourceLoadStarted)
					_resourceLoadStarted->Invoke(resource);
			}
		}

		event Mogre::ResourceGroupListener::ResourceLoadEndedHandler^ ResourceLoadEnded
		{
			void add(Mogre::ResourceGroupListener::ResourceLoadEndedHandler^ hnd)
			{
				if (_resourceLoadEnded == CLR_NULL)
				{
					if (_resourceGroupListener == 0)
					{
						_resourceGroupListener = new ResourceGroupListener_Director(this);
						static_cast<Ogre::ResourceGroupManager*>(_native)->addResourceGroupListener(_resourceGroupListener);
					}
					_resourceGroupListener->doCallForResourceLoadEnded = true;
				}
				_resourceLoadEnded += hnd;
			}
			void remove(Mogre::ResourceGroupListener::ResourceLoadEndedHandler^ hnd)
			{
				_resourceLoadEnded -= hnd;
				if (_resourceLoadEnded == CLR_NULL) _resourceGroupListener->doCallForResourceLoadEnded = false;
			}
		private:
			void raise()
			{
				if (_resourceLoadEnded)
					_resourceLoadEnded->Invoke();
			}
		}

		event Mogre::ResourceGroupListener::WorldGeometryStageStartedHandler^ WorldGeometryStageStarted
		{
			void add(Mogre::ResourceGroupListener::WorldGeometryStageStartedHandler^ hnd)
			{
				if (_worldGeometryStageStarted == CLR_NULL)
				{
					if (_resourceGroupListener == 0)
					{
						_resourceGroupListener = new ResourceGroupListener_Director(this);
						static_cast<Ogre::ResourceGroupManager*>(_native)->addResourceGroupListener(_resourceGroupListener);
					}
					_resourceGroupListener->doCallForWorldGeometryStageStarted = true;
				}
				_worldGeometryStageStarted += hnd;
			}
			void remove(Mogre::ResourceGroupListener::WorldGeometryStageStartedHandler^ hnd)
			{
				_worldGeometryStageStarted -= hnd;
				if (_worldGeometryStageStarted == CLR_NULL) _resourceGroupListener->doCallForWorldGeometryStageStarted = false;
			}
		private:
			void raise(String^ description)
			{
				if (_worldGeometryStageStarted)
					_worldGeometryStageStarted->Invoke(description);
			}
		}

		event Mogre::ResourceGroupListener::WorldGeometryStageEndedHandler^ WorldGeometryStageEnded
		{
			void add(Mogre::ResourceGroupListener::WorldGeometryStageEndedHandler^ hnd)
			{
				if (_worldGeometryStageEnded == CLR_NULL)
				{
					if (_resourceGroupListener == 0)
					{
						_resourceGroupListener = new ResourceGroupListener_Director(this);
						static_cast<Ogre::ResourceGroupManager*>(_native)->addResourceGroupListener(_resourceGroupListener);
					}
					_resourceGroupListener->doCallForWorldGeometryStageEnded = true;
				}
				_worldGeometryStageEnded += hnd;
			}
			void remove(Mogre::ResourceGroupListener::WorldGeometryStageEndedHandler^ hnd)
			{
				_worldGeometryStageEnded -= hnd;
				if (_worldGeometryStageEnded == CLR_NULL) _resourceGroupListener->doCallForWorldGeometryStageEnded = false;
			}
		private:
			void raise()
			{
				if (_worldGeometryStageEnded)
					_worldGeometryStageEnded->Invoke();
			}
		}

		event Mogre::ResourceGroupListener::ResourceGroupLoadEndedHandler^ ResourceGroupLoadEnded
		{
			void add(Mogre::ResourceGroupListener::ResourceGroupLoadEndedHandler^ hnd)
			{
				if (_resourceGroupLoadEnded == CLR_NULL)
				{
					if (_resourceGroupListener == 0)
					{
						_resourceGroupListener = new ResourceGroupListener_Director(this);
						static_cast<Ogre::ResourceGroupManager*>(_native)->addResourceGroupListener(_resourceGroupListener);
					}
					_resourceGroupListener->doCallForResourceGroupLoadEnded = true;
				}
				_resourceGroupLoadEnded += hnd;
			}
			void remove(Mogre::ResourceGroupListener::ResourceGroupLoadEndedHandler^ hnd)
			{
				_resourceGroupLoadEnded -= hnd;
				if (_resourceGroupLoadEnded == CLR_NULL) _resourceGroupListener->doCallForResourceGroupLoadEnded = false;
			}
		private:
			void raise(String^ groupName)
			{
				if (_resourceGroupLoadEnded)
					_resourceGroupLoadEnded->Invoke(groupName);
			}
		}

		void CreateResourceGroup(String^ name);
		void InitialiseResourceGroup(String^ name);
		void InitialiseAllResourceGroups();

		void LoadResourceGroup(String^ name, bool loadMainResources, bool loadWorldGeom);
		void LoadResourceGroup(String^ name, bool loadMainResources);
		void LoadResourceGroup(String^ name);

		void UnloadResourceGroup(String^ name, bool reloadableOnly);
		void UnloadResourceGroup(String^ name);

		void UnloadUnreferencedResourcesInGroup(String^ name, bool reloadableOnly);
		void UnloadUnreferencedResourcesInGroup(String^ name);

		void ClearResourceGroup(String^ name);

		void DestroyResourceGroup(String^ name);
		bool IsResourceGroupInitialised(String^ name);
		bool IsResourceGroupLoaded(String^name);
		bool ResourceGroupExists(String^ name);

		void AddResourceLocation(String^ name, String^ locType, String^ resGroup, bool recursive);
		void AddResourceLocation(String^ name, String^ locType, String^ resGroup);
		void AddResourceLocation(String^ name, String^ locType);

		void RemoveResourceLocation(String^ name, String^ resGroup);
		void RemoveResourceLocation(String^ name);

		//void DeclareResource(String^ name, String^ resourceType, String^ groupName, Mogre::Const_NameValuePairList^ loadParameters);
		void DeclareResource(String^ name, String^ resourceType, String^ groupName);
		void DeclareResource(String^ name, String^ resourceType);

		//void DeclareResource(String^ name, String^ resourceType, String^ groupName, Mogre::IManualResourceLoader^ loader, Mogre::Const_NameValuePairList^ loadParameters);
		//void DeclareResource(String^ name, String^ resourceType, String^ groupName, Mogre::IManualResourceLoader^ loader);

		void UndeclareResource(String^ name, String^ groupName);

		Mogre::DataStreamPtr^ OpenResource(String^ resourceName, String^ groupName, bool searchGroupsIfNotFound, Mogre::Resource^ resourceBeingLoaded);
		Mogre::DataStreamPtr^ OpenResource(String^ resourceName, String^ groupName, bool searchGroupsIfNotFound);
		Mogre::DataStreamPtr^ OpenResource(String^ resourceName, String^ groupName);
		Mogre::DataStreamPtr^ OpenResource(String^ resourceName);

		Mogre::DataStreamListPtr^ OpenResources(String^ pattern, String^ groupName);
		Mogre::DataStreamListPtr^ OpenResources(String^ pattern);

		Mogre::StringVectorPtr^ ListResourceNames(String^ groupName, bool dirs);
		Mogre::StringVectorPtr^ ListResourceNames(String^ groupName);

		//Mogre::FileInfoListPtr^ ListResourceFileInfo(String^ groupName, bool dirs);
		//Mogre::FileInfoListPtr^ ListResourceFileInfo(String^ groupName);

		Mogre::StringVectorPtr^ FindResourceNames(String^ groupName, String^ pattern, bool dirs);
		Mogre::StringVectorPtr^ FindResourceNames(String^ groupName, String^ pattern);

		bool ResourceExists(String^ group, String^ filename);

		String^ FindGroupContainingResource(String^ filename);

		bool ResourceLocationExists(String^ name);
		bool ResourceLocationExists(String^ name, String^ resGroup);

		//Mogre::FileInfoListPtr^ FindResourceFileInfo(String^ group, String^ pattern, bool dirs);
		//Mogre::FileInfoListPtr^ FindResourceFileInfo(String^ group, String^ pattern);

		//void LinkWorldGeometryToResourceGroup(String^ group, String^ worldGeometry, Mogre::SceneManager^ sceneManager);
		//void UnlinkWorldGeometryFromResourceGroup(String^ group);

		void ShutdownAll();

		Mogre::StringVector^ GetResourceGroups();

		void AddBuiltinLocations();

	protected public:
		virtual void OnResourceGroupScriptingStarted(String^ groupName, size_t scriptCount) = IResourceGroupListener_Receiver::ResourceGroupScriptingStarted
		{
			ResourceGroupScriptingStarted(groupName, scriptCount);
		}

			virtual void OnScriptParseStarted(String^ scriptName) = IResourceGroupListener_Receiver::ScriptParseStarted
		{
			ScriptParseStarted(scriptName);
		}

			virtual void OnScriptParseEnded(String^ scriptName) = IResourceGroupListener_Receiver::ScriptParseEnded
		{
			ScriptParseEnded(scriptName);
		}

			virtual void OnResourceGroupScriptingEnded(String^ groupName) = IResourceGroupListener_Receiver::ResourceGroupScriptingEnded
		{
			ResourceGroupScriptingEnded(groupName);
		}

			virtual void OnResourceGroupLoadStarted(String^ groupName, size_t resourceCount) = IResourceGroupListener_Receiver::ResourceGroupLoadStarted
		{
			ResourceGroupLoadStarted(groupName, resourceCount);
		}

			virtual void OnResourceLoadStarted(Mogre::ResourcePtr^ resource) = IResourceGroupListener_Receiver::ResourceLoadStarted
		{
			ResourceLoadStarted(resource);
		}

			virtual void OnResourceLoadEnded() = IResourceGroupListener_Receiver::ResourceLoadEnded
		{
			ResourceLoadEnded();
		}

			virtual void OnWorldGeometryStageStarted(String^ description) = IResourceGroupListener_Receiver::WorldGeometryStageStarted
		{
			WorldGeometryStageStarted(description);
		}

			virtual void OnWorldGeometryStageEnded() = IResourceGroupListener_Receiver::WorldGeometryStageEnded
		{
			WorldGeometryStageEnded();
		}

			virtual void OnResourceGroupLoadEnded(String^ groupName) = IResourceGroupListener_Receiver::ResourceGroupLoadEnded
		{
			ResourceGroupLoadEnded(groupName);
		}
	};
}