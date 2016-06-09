#include "stdafx.h"
#include "MogreResourceGroupManager.h"
#include "MogreResource.h"
#include "MogreDataStream.h"

using namespace Mogre;

void ResourceGroupListener_Director::resourceGroupScriptingStarted(const Ogre::String& groupName, size_t scriptCount)
{
	if (doCallForResourceGroupScriptingStarted)
	{
		_receiver->ResourceGroupScriptingStarted(TO_CLR_STRING(groupName), scriptCount);
	}
}

void ResourceGroupListener_Director::scriptParseStarted(const Ogre::String& scriptName, bool& skipThisScript)
{
	if (doCallForScriptParseStarted)
	{
		_receiver->ScriptParseStarted(TO_CLR_STRING(scriptName));
	}
}

void ResourceGroupListener_Director::scriptParseEnded(const Ogre::String& scriptName, bool skipped)
{
	if (doCallForScriptParseEnded)
	{
		_receiver->ScriptParseEnded(TO_CLR_STRING(scriptName));
	}
}

void ResourceGroupListener_Director::resourceGroupScriptingEnded(const Ogre::String& groupName)
{
	if (doCallForResourceGroupScriptingEnded)
	{
		_receiver->ResourceGroupScriptingEnded(TO_CLR_STRING(groupName));
	}
}

void ResourceGroupListener_Director::resourceGroupLoadStarted(const Ogre::String& groupName, size_t resourceCount)
{
	if (doCallForResourceGroupLoadStarted)
	{
		_receiver->ResourceGroupLoadStarted(TO_CLR_STRING(groupName), resourceCount);
	}
}

void ResourceGroupListener_Director::resourceLoadStarted(const Ogre::ResourcePtr& resource)
{
	if (doCallForResourceLoadStarted)
	{
		_receiver->ResourceLoadStarted(resource);
	}
}

void ResourceGroupListener_Director::resourceLoadEnded()
{
	if (doCallForResourceLoadEnded)
	{
		_receiver->ResourceLoadEnded();
	}
}

void ResourceGroupListener_Director::worldGeometryStageStarted(const Ogre::String& description)
{
	if (doCallForWorldGeometryStageStarted)
	{
		_receiver->WorldGeometryStageStarted(TO_CLR_STRING(description));
	}
}

void ResourceGroupListener_Director::worldGeometryStageEnded()
{
	if (doCallForWorldGeometryStageEnded)
	{
		_receiver->WorldGeometryStageEnded();
	}
}

void ResourceGroupListener_Director::resourceGroupLoadEnded(const Ogre::String& groupName)
{
	if (doCallForResourceGroupLoadEnded)
	{
		_receiver->ResourceGroupLoadEnded(TO_CLR_STRING(groupName));
	}
}

ResourceGroupManager::ResourceGroupManager()
{
	_createdByCLR = true;
	_native = new Ogre::ResourceGroupManager();
}

String^ ResourceGroupManager::DEFAULT_RESOURCE_GROUP_NAME::get()
{
	return TO_CLR_STRING(Ogre::ResourceGroupManager::DEFAULT_RESOURCE_GROUP_NAME);
}

void ResourceGroupManager::DEFAULT_RESOURCE_GROUP_NAME::set(String^ value)
{
	DECLARE_NATIVE_STRING(o_value, value);

	Ogre::ResourceGroupManager::DEFAULT_RESOURCE_GROUP_NAME = o_value;
}

String^ ResourceGroupManager::INTERNAL_RESOURCE_GROUP_NAME::get()
{
	return TO_CLR_STRING(Ogre::ResourceGroupManager::INTERNAL_RESOURCE_GROUP_NAME);
}

void ResourceGroupManager::INTERNAL_RESOURCE_GROUP_NAME::set(String^ value)
{
	DECLARE_NATIVE_STRING(o_value, value);

	Ogre::ResourceGroupManager::INTERNAL_RESOURCE_GROUP_NAME = o_value;
}

String^ ResourceGroupManager::AUTODETECT_RESOURCE_GROUP_NAME::get()
{
	return TO_CLR_STRING(Ogre::ResourceGroupManager::AUTODETECT_RESOURCE_GROUP_NAME);
}

void ResourceGroupManager::AUTODETECT_RESOURCE_GROUP_NAME::set(String^ value)
{
	DECLARE_NATIVE_STRING(o_value, value);

	Ogre::ResourceGroupManager::AUTODETECT_RESOURCE_GROUP_NAME = o_value;
}

size_t ResourceGroupManager::RESOURCE_SYSTEM_NUM_REFERENCE_COUNTS::get()
{
	return Ogre::ResourceGroupManager::RESOURCE_SYSTEM_NUM_REFERENCE_COUNTS;
}

void ResourceGroupManager::RESOURCE_SYSTEM_NUM_REFERENCE_COUNTS::set(size_t value)
{
	Ogre::ResourceGroupManager::RESOURCE_SYSTEM_NUM_REFERENCE_COUNTS = value;
}

String^ ResourceGroupManager::WorldResourceGroupName::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::ResourceGroupManager*>(_native)->getWorldResourceGroupName());
}

void ResourceGroupManager::WorldResourceGroupName::set(String^ groupName)
{
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	_native->setWorldResourceGroupName(o_groupName);
}

void ResourceGroupManager::CreateResourceGroup(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	_native->createResourceGroup(o_name);
}

void ResourceGroupManager::InitialiseResourceGroup(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	_native->initialiseResourceGroup(o_name);
}

void ResourceGroupManager::InitialiseAllResourceGroups()
{
	_native->initialiseAllResourceGroups();
}

void ResourceGroupManager::LoadResourceGroup(String^ name, bool loadMainResources, bool loadWorldGeom)
{
	DECLARE_NATIVE_STRING(o_name, name);

	_native->loadResourceGroup(o_name, loadMainResources, loadWorldGeom);
}
void ResourceGroupManager::LoadResourceGroup(String^ name, bool loadMainResources)
{
	DECLARE_NATIVE_STRING(o_name, name);

	_native->loadResourceGroup(o_name, loadMainResources);
}

void ResourceGroupManager::LoadResourceGroup(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	_native->loadResourceGroup(o_name);
}

void ResourceGroupManager::UnloadResourceGroup(String^ name, bool reloadableOnly)
{
	DECLARE_NATIVE_STRING(o_name, name);

	_native->unloadResourceGroup(o_name, reloadableOnly);
}
void ResourceGroupManager::UnloadResourceGroup(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	_native->unloadResourceGroup(o_name);
}

void ResourceGroupManager::UnloadUnreferencedResourcesInGroup(String^ name, bool reloadableOnly)
{
	DECLARE_NATIVE_STRING(o_name, name);

	_native->unloadUnreferencedResourcesInGroup(o_name, reloadableOnly);
}

void ResourceGroupManager::UnloadUnreferencedResourcesInGroup(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	_native->unloadUnreferencedResourcesInGroup(o_name);
}

void ResourceGroupManager::ClearResourceGroup(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	_native->clearResourceGroup(o_name);
}

void ResourceGroupManager::DestroyResourceGroup(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	_native->destroyResourceGroup(o_name);
}

bool ResourceGroupManager::IsResourceGroupInitialised(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return _native->isResourceGroupInitialised(o_name);
}

bool ResourceGroupManager::IsResourceGroupLoaded(String^name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return _native->isResourceGroupLoaded(o_name);
}

bool ResourceGroupManager::ResourceGroupExists(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return _native->resourceGroupExists(o_name);
}

void ResourceGroupManager::AddResourceLocation(String^ name, String^ locType, String^ resGroup, bool recursive)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_locType, locType);
	DECLARE_NATIVE_STRING(o_resGroup, resGroup);

	_native->addResourceLocation(o_name, o_locType, o_resGroup, recursive);
}
void ResourceGroupManager::AddResourceLocation(String^ name, String^ locType, String^ resGroup)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_locType, locType);
	DECLARE_NATIVE_STRING(o_resGroup, resGroup);

	_native->addResourceLocation(o_name, o_locType, o_resGroup);
}
void ResourceGroupManager::AddResourceLocation(String^ name, String^ locType)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_locType, locType);

	_native->addResourceLocation(o_name, o_locType);
}

void ResourceGroupManager::RemoveResourceLocation(String^ name, String^ resGroup)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_resGroup, resGroup);

	_native->removeResourceLocation(o_name, o_resGroup);
}

void ResourceGroupManager::RemoveResourceLocation(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	_native->removeResourceLocation(o_name);
}

//void ResourceGroupManager::DeclareResource(String^ name, String^ resourceType, String^ groupName, Mogre::Const_NameValuePairList^ loadParameters)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_resourceType, resourceType);
//	DECLARE_NATIVE_STRING(o_groupName, groupName);
//
//	_native->declareResource(o_name, o_resourceType, o_groupName, loadParameters);
//}

void ResourceGroupManager::DeclareResource(String^ name, String^ resourceType, String^ groupName)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_resourceType, resourceType);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	_native->declareResource(o_name, o_resourceType, o_groupName);
}

void ResourceGroupManager::DeclareResource(String^ name, String^ resourceType)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_resourceType, resourceType);

	_native->declareResource(o_name, o_resourceType);
}

//void ResourceGroupManager::DeclareResource(String^ name, String^ resourceType, String^ groupName, Mogre::IManualResourceLoader^ loader, Mogre::Const_NameValuePairList^ loadParameters)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_resourceType, resourceType);
//	DECLARE_NATIVE_STRING(o_groupName, groupName);
//
//	_native->declareResource(o_name, o_resourceType, o_groupName, loader, loadParameters);
//}
//
//void ResourceGroupManager::DeclareResource(String^ name, String^ resourceType, String^ groupName, Mogre::IManualResourceLoader^ loader)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_resourceType, resourceType);
//	DECLARE_NATIVE_STRING(o_groupName, groupName);
//
//	_native->declareResource(o_name, o_resourceType, o_groupName, loader);
//}

void ResourceGroupManager::UndeclareResource(String^ name, String^ groupName)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	_native->undeclareResource(o_name, o_groupName);
}

Mogre::DataStreamPtr^ ResourceGroupManager::OpenResource(String^ resourceName, String^ groupName, bool searchGroupsIfNotFound, Mogre::Resource^ resourceBeingLoaded)
{
	DECLARE_NATIVE_STRING(o_resourceName, resourceName);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::ResourceGroupManager*>(_native)->openResource(o_resourceName, o_groupName, searchGroupsIfNotFound, GetPointerOrNull(resourceBeingLoaded));
}

Mogre::DataStreamPtr^ ResourceGroupManager::OpenResource(String^ resourceName, String^ groupName, bool searchGroupsIfNotFound)
{
	DECLARE_NATIVE_STRING(o_resourceName, resourceName);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::ResourceGroupManager*>(_native)->openResource(o_resourceName, o_groupName, searchGroupsIfNotFound);
}
Mogre::DataStreamPtr^ ResourceGroupManager::OpenResource(String^ resourceName, String^ groupName)
{
	DECLARE_NATIVE_STRING(o_resourceName, resourceName);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::ResourceGroupManager*>(_native)->openResource(o_resourceName, o_groupName);
}

Mogre::DataStreamPtr^ ResourceGroupManager::OpenResource(String^ resourceName)
{
	DECLARE_NATIVE_STRING(o_resourceName, resourceName);

	return static_cast<Ogre::ResourceGroupManager*>(_native)->openResource(o_resourceName);
}

Mogre::DataStreamListPtr^ ResourceGroupManager::OpenResources(String^ pattern, String^ groupName)
{
	DECLARE_NATIVE_STRING(o_pattern, pattern);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::ResourceGroupManager*>(_native)->openResources(o_pattern, o_groupName);
}

Mogre::DataStreamListPtr^ ResourceGroupManager::OpenResources(String^ pattern)
{
	DECLARE_NATIVE_STRING(o_pattern, pattern);

	return static_cast<Ogre::ResourceGroupManager*>(_native)->openResources(o_pattern);
}

Mogre::StringVectorPtr^ ResourceGroupManager::ListResourceNames(String^ groupName, bool dirs)
{
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::ResourceGroupManager*>(_native)->listResourceNames(o_groupName, dirs);
}

Mogre::StringVectorPtr^ ResourceGroupManager::ListResourceNames(String^ groupName)
{
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::ResourceGroupManager*>(_native)->listResourceNames(o_groupName);
}

//Mogre::FileInfoListPtr^ ResourceGroupManager::ListResourceFileInfo(String^ groupName, bool dirs)
//{
//	DECLARE_NATIVE_STRING(o_groupName, groupName);
//
//	return static_cast<Ogre::ResourceGroupManager*>(_native)->listResourceFileInfo(o_groupName, dirs);
//}
//Mogre::FileInfoListPtr^ ResourceGroupManager::ListResourceFileInfo(String^ groupName)
//{
//	DECLARE_NATIVE_STRING(o_groupName, groupName);
//
//	return static_cast<Ogre::ResourceGroupManager*>(_native)->listResourceFileInfo(o_groupName);
//}

Mogre::StringVectorPtr^ ResourceGroupManager::FindResourceNames(String^ groupName, String^ pattern, bool dirs)
{
	DECLARE_NATIVE_STRING(o_groupName, groupName);
	DECLARE_NATIVE_STRING(o_pattern, pattern);

	return static_cast<Ogre::ResourceGroupManager*>(_native)->findResourceNames(o_groupName, o_pattern, dirs);
}
Mogre::StringVectorPtr^ ResourceGroupManager::FindResourceNames(String^ groupName, String^ pattern)
{
	DECLARE_NATIVE_STRING(o_groupName, groupName);
	DECLARE_NATIVE_STRING(o_pattern, pattern);

	return static_cast<Ogre::ResourceGroupManager*>(_native)->findResourceNames(o_groupName, o_pattern);
}

bool ResourceGroupManager::ResourceExists(String^ group, String^ filename)
{
	DECLARE_NATIVE_STRING(o_group, group);
	DECLARE_NATIVE_STRING(o_filename, filename);

	return static_cast<Ogre::ResourceGroupManager*>(_native)->resourceExists(o_group, o_filename);
}

String^ ResourceGroupManager::FindGroupContainingResource(String^ filename)
{
	DECLARE_NATIVE_STRING(o_filename, filename);

	return TO_CLR_STRING(static_cast<Ogre::ResourceGroupManager*>(_native)->findGroupContainingResource(o_filename));
}


bool ResourceGroupManager::ResourceLocationExists(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);
	return _native->resourceLocationExists(o_name);
}

bool ResourceGroupManager::ResourceLocationExists(String^ name, String^ resGroup)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_resGroup, resGroup);

	return _native->resourceLocationExists(o_name, o_resGroup);
}


void ResourceGroupManager::ShutdownAll()
{
	_native->shutdownAll();
}

Mogre::StringVector^ ResourceGroupManager::GetResourceGroups()
{
	return Mogre::StringVector::ByValue(_native->getResourceGroups());
}

void ResourceGroupManager::AddBuiltinLocations()
{
	const Ogre::ResourceGroupManager::LocationList genLocs = Ogre::ResourceGroupManager::getSingleton().getResourceLocationList("General");
	Ogre::String arch = genLocs.front()->archive->getName();


	arch = Ogre::StringUtil::replaceAll(arch, "Media/../../Tests/Media", "");
	arch = Ogre::StringUtil::replaceAll(arch, "media/../../Tests/Media", "");

	Ogre::String type = "FileSystem";
	Ogre::String sec = "Popular";

#ifdef OGRE_BUILD_PLUGIN_CG
	bool use_HLSL_Cg_shared = true;
#else
	bool use_HLSL_Cg_shared = Ogre::GpuProgramManager::getSingleton().isSyntaxSupported("hlsl");
#endif

	// Add locations for supported shader languages
	if (Ogre::GpuProgramManager::getSingleton().isSyntaxSupported("glsles"))
	{
		_native->addResourceLocation(arch + "/materials/programs/GLSLES", type, sec);
	}
	else if (Ogre::GpuProgramManager::getSingleton().isSyntaxSupported("glsl"))
	{
		if (Ogre::GpuProgramManager::getSingleton().isSyntaxSupported("glsl150"))
		{
			_native->addResourceLocation(arch + "/materials/programs/GLSL150", type, sec);
		}
		else
		{
			_native->addResourceLocation(arch + "/materials/programs/GLSL", type, sec);
		}

		if (Ogre::GpuProgramManager::getSingleton().isSyntaxSupported("glsl400"))
		{
			_native->addResourceLocation(arch + "/materials/programs/GLSL400", type, sec);
		}
	}
	else if (Ogre::GpuProgramManager::getSingleton().isSyntaxSupported("hlsl"))
	{
		_native->addResourceLocation(arch + "/materials/programs/HLSL", type, sec);
	}

#ifdef OGRE_BUILD_PLUGIN_CG
	_native->addResourceLocation(arch + "/materials/programs/Cg", type, sec);
#endif

	if (use_HLSL_Cg_shared)
	{
		_native->addResourceLocation(arch + "/materials/programs/HLSL_Cg", type, sec);
	}

	if (Ogre::GpuProgramManager::getSingleton().isSyntaxSupported("glsles"))
	{
		_native->addResourceLocation(arch + "/RTShaderLib/GLSLES", type, sec);
	}
	else if (Ogre::GpuProgramManager::getSingleton().isSyntaxSupported("glsl"))
	{
		_native->addResourceLocation(arch + "/RTShaderLib/GLSL", type, sec);
		if (Ogre::GpuProgramManager::getSingleton().isSyntaxSupported("glsl150"))
		{
			_native->addResourceLocation(arch + "/RTShaderLib/GLSL150", type, sec);
		}
	}
	else if (Ogre::GpuProgramManager::getSingleton().isSyntaxSupported("hlsl"))
	{
		_native->addResourceLocation(arch + "/RTShaderLib/HLSL", type, sec);
	}
#ifdef OGRE_BUILD_PLUGIN_CG
	_native->addResourceLocation(arch + "/RTShaderLib/Cg", type, sec);
#endif

	if (use_HLSL_Cg_shared)
	{
		_native->addResourceLocation(arch + "/RTShaderLib/HLSL_Cg", type, sec);
	}
}