#include "stdafx.h"
#include "MogreMaterialManager.h"

using namespace Mogre;

MaterialManager::MaterialManager() : ResourceManager((Ogre::ResourceManager*) 0)
{
	_createdByCLR = true;
	_native = new Ogre::MaterialManager();
}

String^ MaterialManager::DEFAULT_SCHEME_NAME::get()
{
	return TO_CLR_STRING(Ogre::MaterialManager::DEFAULT_SCHEME_NAME);
}
void MaterialManager::DEFAULT_SCHEME_NAME::set(String^ value)
{
	DECLARE_NATIVE_STRING(o_value, value);

	Ogre::MaterialManager::DEFAULT_SCHEME_NAME = o_value;
}

String^ MaterialManager::ActiveScheme::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::MaterialManager*>(_native)->getActiveScheme());
}
void MaterialManager::ActiveScheme::set(String^ schemeName)
{
	DECLARE_NATIVE_STRING(o_schemeName, schemeName);

	static_cast<Ogre::MaterialManager*>(_native)->setActiveScheme(o_schemeName);
}

unsigned int MaterialManager::DefaultAnisotropy::get()
{
	return static_cast<const Ogre::MaterialManager*>(_native)->getDefaultAnisotropy();
}
void MaterialManager::DefaultAnisotropy::set(unsigned int maxAniso)
{
	static_cast<Ogre::MaterialManager*>(_native)->setDefaultAnisotropy(maxAniso);
}

void MaterialManager::Initialise()
{
	static_cast<Ogre::MaterialManager*>(_native)->initialise();
}

void MaterialManager::ParseScript(Mogre::DataStreamPtr^ stream, String^ groupName)
{
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	static_cast<Ogre::MaterialManager*>(_native)->parseScript((Ogre::DataStreamPtr&)stream, o_groupName);
}

void MaterialManager::SetDefaultTextureFiltering(Mogre::TextureFilterOptions fo)
{
	static_cast<Ogre::MaterialManager*>(_native)->setDefaultTextureFiltering((Ogre::TextureFilterOptions)fo);
}

void MaterialManager::SetDefaultTextureFiltering(Mogre::FilterType ftype, Mogre::FilterOptions opts)
{
	static_cast<Ogre::MaterialManager*>(_native)->setDefaultTextureFiltering((Ogre::FilterType)ftype, (Ogre::FilterOptions)opts);
}

void MaterialManager::SetDefaultTextureFiltering(Mogre::FilterOptions minFilter, Mogre::FilterOptions magFilter, Mogre::FilterOptions mipFilter)
{
	static_cast<Ogre::MaterialManager*>(_native)->setDefaultTextureFiltering((Ogre::FilterOptions)minFilter, (Ogre::FilterOptions)magFilter, (Ogre::FilterOptions)mipFilter);
}

Mogre::FilterOptions MaterialManager::GetDefaultTextureFiltering(Mogre::FilterType ftype)
{
	return (Mogre::FilterOptions)static_cast<const Ogre::MaterialManager*>(_native)->getDefaultTextureFiltering((Ogre::FilterType)ftype);
}

Mogre::MaterialPtr^ MaterialManager::GetDefaultSettings()
{
	return static_cast<const Ogre::MaterialManager*>(_native)->getDefaultSettings();
}

unsigned short MaterialManager::_getSchemeIndex(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return static_cast<Ogre::MaterialManager*>(_native)->_getSchemeIndex(o_name);
}

String^ MaterialManager::_getSchemeName(unsigned short index)
{
	return TO_CLR_STRING(static_cast<Ogre::MaterialManager*>(_native)->_getSchemeName(index));
}

unsigned short MaterialManager::_getActiveSchemeIndex()
{
	return static_cast<const Ogre::MaterialManager*>(_native)->_getActiveSchemeIndex();
}