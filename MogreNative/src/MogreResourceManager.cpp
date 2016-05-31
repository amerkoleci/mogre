#include "stdafx.h"
#include "MogreResourceManager.h"

using namespace Mogre;

//CPP_DECLARE_STLHASHMAP(ResourceManager::, ResourceMap, String^, Mogre::ResourcePtr^, Ogre::String, Ogre::ResourcePtr);
//CPP_DECLARE_STLMAP(ResourceManager::, ResourceHandleMap, Mogre::ResourceHandle, Mogre::ResourcePtr^, Ogre::ResourceHandle, Ogre::ResourcePtr);
//CPP_DECLARE_MAP_ITERATOR(ResourceManager::, ResourceMapIterator, Ogre::ResourceManager::ResourceMapIterator, Mogre::ResourceManager::ResourceHandleMap, Mogre::ResourcePtr^, Ogre::ResourcePtr, Mogre::ResourceHandle, Ogre::ResourceHandle, );

//Public Declarations
Ogre::Real ResourceManager::LoadingOrder::get()
{
	return static_cast<const Ogre::ResourceManager*>(_native)->getLoadingOrder();
}

size_t ResourceManager::MemoryBudget::get()
{
	return static_cast<const Ogre::ResourceManager*>(_native)->getMemoryBudget();
}
void ResourceManager::MemoryBudget::set(size_t bytes)
{
	static_cast<Ogre::ResourceManager*>(_native)->setMemoryBudget(bytes);
}

size_t ResourceManager::MemoryUsage::get()
{
	return static_cast<const Ogre::ResourceManager*>(_native)->getMemoryUsage();
}

String^ ResourceManager::ResourceType::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::ResourceManager*>(_native)->getResourceType());
}

//Mogre::ResourcePtr^ ResourceManager::Create(String^ name, String^ group, bool isManual, Mogre::IManualResourceLoader^ loader, Mogre::Const_NameValuePairList^ createParams)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_group, group);
//
//	return static_cast<Ogre::ResourceManager*>(_native)->create(o_name, o_group, isManual, loader, createParams);
//}
//
//Mogre::ResourcePtr^ ResourceManager::Create(String^ name, String^ group, bool isManual, Mogre::IManualResourceLoader^ loader)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_group, group);
//
//	return static_cast<Ogre::ResourceManager*>(_native)->create(o_name, o_group, isManual, loader);
//}

Mogre::ResourcePtr^ ResourceManager::Create(String^ name, String^ group, bool isManual)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	return static_cast<Ogre::ResourceManager*>(_native)->createResource(o_name, o_group, isManual);
}

Mogre::ResourcePtr^ ResourceManager::Create(String^ name, String^ group)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	return static_cast<Ogre::ResourceManager*>(_native)->createResource(o_name, o_group);
}

//Pair<Mogre::ResourcePtr^, bool> ResourceManager::CreateOrRetrieve(String^ name, String^ group, bool isManual, Mogre::IManualResourceLoader^ loader, Mogre::Const_NameValuePairList^ createParams)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_group, group);
//
//	return ToManaged<Pair<Mogre::ResourcePtr^, bool>, Ogre::ResourceManager::ResourceCreateOrRetrieveResult>(static_cast<Ogre::ResourceManager*>(_native)->createOrRetrieve(o_name, o_group, isManual, loader, createParams));
//}
//Pair<Mogre::ResourcePtr^, bool> ResourceManager::CreateOrRetrieve(String^ name, String^ group, bool isManual, Mogre::IManualResourceLoader^ loader)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_group, group);
//
//	return ToManaged<Pair<Mogre::ResourcePtr^, bool>, Ogre::ResourceManager::ResourceCreateOrRetrieveResult>(static_cast<Ogre::ResourceManager*>(_native)->createOrRetrieve(o_name, o_group, isManual, loader));
//}

Pair<Mogre::ResourcePtr^, bool> ResourceManager::CreateOrRetrieve(String^ name, String^ group, bool isManual)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	auto pair = _native->createOrRetrieve(o_name, o_group, isManual);
	return Pair<Mogre::ResourcePtr^, bool>(pair.first, pair.second);
}

Pair<Mogre::ResourcePtr^, bool> ResourceManager::CreateOrRetrieve(String^ name, String^ group)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	auto pair = _native->createOrRetrieve(o_name, o_group);
	return Pair<Mogre::ResourcePtr^, bool>(pair.first, pair.second);
}

void ResourceManager::Unload(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	static_cast<Ogre::ResourceManager*>(_native)->unload(o_name);
}

void ResourceManager::Unload(Mogre::ResourceHandle handle)
{
	static_cast<Ogre::ResourceManager*>(_native)->unload(handle);
}

void ResourceManager::UnloadAll(bool reloadableOnly)
{
	static_cast<Ogre::ResourceManager*>(_native)->unloadAll(reloadableOnly);
}
void ResourceManager::UnloadAll()
{
	static_cast<Ogre::ResourceManager*>(_native)->unloadAll();
}

void ResourceManager::ReloadAll(bool reloadableOnly)
{
	static_cast<Ogre::ResourceManager*>(_native)->reloadAll(reloadableOnly);
}
void ResourceManager::ReloadAll()
{
	static_cast<Ogre::ResourceManager*>(_native)->reloadAll();
}

void ResourceManager::UnloadUnreferencedResources(bool reloadableOnly)
{
	static_cast<Ogre::ResourceManager*>(_native)->unloadUnreferencedResources(reloadableOnly);
}
void ResourceManager::UnloadUnreferencedResources()
{
	static_cast<Ogre::ResourceManager*>(_native)->unloadUnreferencedResources();
}

void ResourceManager::ReloadUnreferencedResources(bool reloadableOnly)
{
	static_cast<Ogre::ResourceManager*>(_native)->reloadUnreferencedResources(reloadableOnly);
}
void ResourceManager::ReloadUnreferencedResources()
{
	static_cast<Ogre::ResourceManager*>(_native)->reloadUnreferencedResources();
}

void ResourceManager::Remove(Mogre::ResourcePtr^ r)
{
	static_cast<Ogre::ResourceManager*>(_native)->remove((Ogre::ResourcePtr&)r);
}

void ResourceManager::Remove(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	static_cast<Ogre::ResourceManager*>(_native)->remove(o_name);
}

void ResourceManager::Remove(Mogre::ResourceHandle handle)
{
	static_cast<Ogre::ResourceManager*>(_native)->remove(handle);
}

void ResourceManager::RemoveAll()
{
	static_cast<Ogre::ResourceManager*>(_native)->removeAll();
}

Mogre::ResourcePtr^ ResourceManager::GetByName(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return _native->getResourceByName(o_name);
}

Mogre::ResourcePtr^ ResourceManager::GetResourceByName(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return _native->getResourceByName(o_name);
}

Mogre::ResourcePtr^ ResourceManager::GetByHandle(Mogre::ResourceHandle handle)
{
	return static_cast<Ogre::ResourceManager*>(_native)->getByHandle(handle);
}

bool ResourceManager::ResourceExists(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return static_cast<Ogre::ResourceManager*>(_native)->resourceExists(o_name);
}

bool ResourceManager::ResourceExists(Mogre::ResourceHandle handle)
{
	return static_cast<Ogre::ResourceManager*>(_native)->resourceExists(handle);
}

void ResourceManager::_notifyResourceTouched(Mogre::Resource^ res)
{
	static_cast<Ogre::ResourceManager*>(_native)->_notifyResourceTouched(GetPointerOrNull(res));
}

void ResourceManager::_notifyResourceLoaded(Mogre::Resource^ res)
{
	static_cast<Ogre::ResourceManager*>(_native)->_notifyResourceLoaded(GetPointerOrNull(res));
}

void ResourceManager::_notifyResourceUnloaded(Mogre::Resource^ res)
{
	static_cast<Ogre::ResourceManager*>(_native)->_notifyResourceUnloaded(GetPointerOrNull(res));
}

//Mogre::ResourcePtr^ ResourceManager::Load(String^ name, String^ group, bool isManual, Mogre::IManualResourceLoader^ loader, Mogre::Const_NameValuePairList^ loadParams)
//{
//	DECLARE_NATIVE_STRING(o_name, name)
//	DECLARE_NATIVE_STRING(o_group, group)
//
//	return static_cast<Ogre::ResourceManager*>(_native)->load(o_name, o_group, isManual, loader, loadParams);
//}
//Mogre::ResourcePtr^ ResourceManager::Load(String^ name, String^ group, bool isManual, Mogre::IManualResourceLoader^ loader)
//{
//	DECLARE_NATIVE_STRING(o_name, name)
//	DECLARE_NATIVE_STRING(o_group, group)
//
//	return static_cast<Ogre::ResourceManager*>(_native)->load(o_name, o_group, isManual, loader);
//}

Mogre::ResourcePtr^ ResourceManager::Load(String^ name, String^ group, bool isManual)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	return static_cast<Ogre::ResourceManager*>(_native)->load(o_name, o_group, isManual);
}

Mogre::ResourcePtr^ ResourceManager::Load(String^ name, String^ group)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);

	return _native->load(o_name, o_group);
}

void ResourceManager::RemoveUnreferencedResources()
{
	_native->removeUnreferencedResources();
}

void ResourceManager::RemoveUnreferencedResources(bool reloadableOnly)
{
	_native->removeUnreferencedResources(reloadableOnly);
}

Mogre::Const_StringVector^ ResourceManager::GetScriptPatterns()
{
	return static_cast<const Ogre::ResourceManager*>(_native)->getScriptPatterns();
}

void ResourceManager::ParseScript(Mogre::DataStreamPtr^ stream, String^ groupName)
{
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	static_cast<Ogre::ResourceManager*>(_native)->parseScript((Ogre::DataStreamPtr&)stream, o_groupName);
}

//Mogre::ResourceManager::ResourceMapIterator^ ResourceManager::GetResourceIterator()
//{
//	return static_cast<Ogre::ResourceManager*>(_native)->getResourceIterator();
//}
