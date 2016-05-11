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

	public ref class ResourceGroupManager //: public IResourceGroupListener_Receiver
	{
	private protected:
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
			/*if (_resourceGroupListener != 0)
			{
				if (_native != 0) 
					_native->removeResourceGroupListener(_resourceGroupListener);
				delete _resourceGroupListener; _resourceGroupListener = 0;
			}*/
			if (_createdByCLR && _native) { delete _native; _native = 0; }
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

		//Mogre::DataStreamPtr^ OpenResource(String^ resourceName, String^ groupName, bool searchGroupsIfNotFound, Mogre::Resource^ resourceBeingLoaded);
		//Mogre::DataStreamPtr^ OpenResource(String^ resourceName, String^ groupName, bool searchGroupsIfNotFound);
		//Mogre::DataStreamPtr^ OpenResource(String^ resourceName, String^ groupName);
		//Mogre::DataStreamPtr^ OpenResource(String^ resourceName);

		//Mogre::DataStreamListPtr^ OpenResources(String^ pattern, String^ groupName);
		//Mogre::DataStreamListPtr^ OpenResources(String^ pattern);

		//Mogre::StringVectorPtr^ ListResourceNames(String^ groupName, bool dirs);
		//Mogre::StringVectorPtr^ ListResourceNames(String^ groupName);

		//Mogre::FileInfoListPtr^ ListResourceFileInfo(String^ groupName, bool dirs);
		//Mogre::FileInfoListPtr^ ListResourceFileInfo(String^ groupName);

		//Mogre::StringVectorPtr^ FindResourceNames(String^ groupName, String^ pattern, bool dirs);
		//Mogre::StringVectorPtr^ FindResourceNames(String^ groupName, String^ pattern);

		//bool ResourceExists(String^ group, String^ filename);

		//String^ FindGroupContainingResource(String^ filename);

		//Mogre::FileInfoListPtr^ FindResourceFileInfo(String^ group, String^ pattern, bool dirs);
		//Mogre::FileInfoListPtr^ FindResourceFileInfo(String^ group, String^ pattern);

		//void LinkWorldGeometryToResourceGroup(String^ group, String^ worldGeometry, Mogre::SceneManager^ sceneManager);
		//void UnlinkWorldGeometryFromResourceGroup(String^ group);

		void ShutdownAll();
	};
}