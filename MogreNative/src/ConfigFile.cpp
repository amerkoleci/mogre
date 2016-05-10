#include "stdafx.h"
#include "ConfigFile.h"
#include "Util.h"
using namespace Mogre;


//Nested Types
#define STLDECL_MANAGEDKEY String^
#define STLDECL_MANAGEDVALUE String^
#define STLDECL_NATIVEKEY Ogre::String
#define STLDECL_NATIVEVALUE Ogre::String
CPP_DECLARE_STLMULTIMAP(ConfigFile::, SettingsMultiMap, STLDECL_MANAGEDKEY, STLDECL_MANAGEDVALUE, STLDECL_NATIVEKEY, STLDECL_NATIVEVALUE)
#undef STLDECL_MANAGEDKEY
#undef STLDECL_MANAGEDVALUE
#undef STLDECL_NATIVEKEY
#undef STLDECL_NATIVEVALUE

#define STLDECL_MANAGEDKEY String^
#define STLDECL_MANAGEDVALUE Mogre::ConfigFile::SettingsMultiMap^
#define STLDECL_NATIVEKEY Ogre::String
#define STLDECL_NATIVEVALUE Ogre::ConfigFile::SettingsMultiMap*
CPP_DECLARE_STLMAP(ConfigFile::, SettingsBySection, STLDECL_MANAGEDKEY, STLDECL_MANAGEDVALUE, STLDECL_NATIVEKEY, STLDECL_NATIVEVALUE)
#undef STLDECL_MANAGEDKEY
#undef STLDECL_MANAGEDVALUE
#undef STLDECL_NATIVEKEY
#undef STLDECL_NATIVEVALUE

CPP_DECLARE_MAP_ITERATOR(ConfigFile::, SettingsIterator, Ogre::ConfigFile::SettingsIterator, Mogre::ConfigFile::SettingsMultiMap, String^, Ogre::String, String^, Ogre::String, )

CPP_DECLARE_MAP_ITERATOR(ConfigFile::, SectionIterator, Ogre::ConfigFile::SectionIterator, Mogre::ConfigFile::SettingsBySection, Mogre::ConfigFile::SettingsMultiMap^, Ogre::ConfigFile::SettingsMultiMap*, String^, Ogre::String, )


ConfigFile::ConfigFile()
{
	_native = OGRE_NEW_T(Ogre::ConfigFile, Ogre::MEMCATEGORY_GENERAL)();
}

ConfigFile::~ConfigFile()
{
	this->!ConfigFile();
}

ConfigFile::!ConfigFile()
{
	OGRE_DELETE_T(_native, ConfigFile, Ogre::MEMCATEGORY_GENERAL);
	_native = nullptr;
}

void ConfigFile::Clear()
{
	_native->clear();
}

void ConfigFile::Load(String^ fileName, String^ separators, bool trimWhitespace)
{
	_native->load(
		Util::ToUnmanagedString(fileName),
		Util::ToUnmanagedString(separators),
		trimWhitespace
	);
}

void ConfigFile::Load(String^ fileName, String^ separators)
{
	_native->load(
		Util::ToUnmanagedString(fileName),
		Util::ToUnmanagedString(separators),
		true
	);
}

void ConfigFile::Load(String^ fileName)
{
	_native->load(Util::ToUnmanagedString(fileName));
}

ConfigFile::SectionIterator^ ConfigFile::GetSectionIterator()
{
	return static_cast<Ogre::ConfigFile*>(_native)->getSectionIterator();
}

ConfigFile::SettingsIterator^ ConfigFile::GetSettingsIterator(String^ section)
{
	DECLARE_NATIVE_STRING(o_section, section)

	return static_cast<Ogre::ConfigFile*>(_native)->getSettingsIterator(o_section);
}

ConfigFile::SettingsIterator^ ConfigFile::GetSettingsIterator()
{
	return static_cast<Ogre::ConfigFile*>(_native)->getSettingsIterator();
}