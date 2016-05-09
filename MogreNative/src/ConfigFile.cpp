#include "stdafx.h"

extern "C"
{
	MOGRE_EXPORTS_API Ogre::ConfigFile* ConfigFile_new()
	{
		return new Ogre::ConfigFile();
	}

	MOGRE_EXPORTS_API void ConfigFile_delete(Ogre::ConfigFile* _this)
	{
		SafeDelete(_this);
	}

	MOGRE_EXPORTS_API void ConfigFile_clear(Ogre::ConfigFile* _this)
	{
		_this->clear();
	}

	MOGRE_EXPORTS_API void ConfigFile_load(Ogre::ConfigFile* _this, const char* fileName, const char* separators, bool trimWhitespace)
	{
		_this->load(fileName, separators, trimWhitespace);
	}

	MOGRE_EXPORTS_API void ConfigFile_load2(Ogre::ConfigFile* _this, const char* fileName, const char* resourceGroup, const char* separators, bool trimWhitespace)
	{
		_this->load(fileName, resourceGroup, separators, trimWhitespace);
	}

	MOGRE_EXPORTS_API void ConfigFile_loadDirect(Ogre::ConfigFile* _this, const char* fileName, const char* separators, bool trimWhitespace)
	{
		_this->loadDirect(fileName, separators, trimWhitespace);
	}

	MOGRE_EXPORTS_API char* ConfigFile_getSetting(Ogre::ConfigFile* _this, const char* key, const char* section, const char* defaultValue)
	{
		return CreateOutString(_this->getSetting(key, section, defaultValue));
	}

	MOGRE_EXPORTS_API Ogre::ConfigFile::SectionIterator* ConfigFile_getSectionIterator(Ogre::ConfigFile* _this)
	{
		return new Ogre::ConfigFile::SectionIterator(_this->getSectionIterator());
	}

	MOGRE_EXPORTS_API void SectionIterator_delete(Ogre::ConfigFile::SectionIterator* _this)
	{
		SafeDelete(_this);
	}

	MOGRE_EXPORTS_API bool SectionIterator_hasMoreElements(Ogre::ConfigFile::SectionIterator* _this)
	{
		return _this->hasMoreElements();
	}

	MOGRE_EXPORTS_API char* SectionIterator_peekNextKey(Ogre::ConfigFile::SectionIterator* _this)
	{
		return CreateOutString(_this->peekNextKey());
	}

	MOGRE_EXPORTS_API Ogre::ConfigFile::SettingsMultiMap* SectionIterator_getNext(Ogre::ConfigFile::SectionIterator* _this)
	{
		return _this->getNext();
	}

	typedef void(*SettingsMultiMap_Iterate)(const char*, const char*);

	MOGRE_EXPORTS_API void SettingsMultiMap_iterate(
		Ogre::ConfigFile::SettingsMultiMap* _this, SettingsMultiMap_Iterate callback)
	{
		Ogre::String type, arch;
		Ogre::ConfigFile::SettingsMultiMap::iterator i;
		for (i = _this->begin(); i != _this->end(); i++)
		{
			type = i->first;
			arch = i->second;
			callback(type.c_str(), arch.c_str());
		}
	}
}