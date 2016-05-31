#include "stdafx.h"
#include "MogreConfigFile.h"
using namespace Mogre;

CPP_DECLARE_STLMULTIMAP(ConfigFile::, SettingsMultiMap, String^, String^, Ogre::String, Ogre::String);
CPP_DECLARE_STLMAP(ConfigFile::, SettingsBySection, String^, Mogre::ConfigFile::SettingsMultiMap^, Ogre::String, Ogre::ConfigFile::SettingsMultiMap*);
CPP_DECLARE_MAP_ITERATOR(ConfigFile::, SettingsIterator, Ogre::ConfigFile::SettingsIterator, Mogre::ConfigFile::SettingsMultiMap, String^, Ogre::String, String^, Ogre::String, );
CPP_DECLARE_MAP_ITERATOR(ConfigFile::, SectionIterator, Ogre::ConfigFile::SectionIterator, Mogre::ConfigFile::SettingsBySection, Mogre::ConfigFile::SettingsMultiMap^, Ogre::ConfigFile::SettingsMultiMap*, String^, Ogre::String, );


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
	DECLARE_NATIVE_STRING(o_filename, fileName);
	DECLARE_NATIVE_STRING(o_separators, separators);

	_native->load(
		o_filename,
		o_separators,
		trimWhitespace
	);
}

void ConfigFile::Load(String^ fileName, String^ separators)
{
	DECLARE_NATIVE_STRING(o_filename, fileName);
	DECLARE_NATIVE_STRING(o_separators, separators);

	_native->load(
		o_filename,
		o_separators,
		true
	);
}

void ConfigFile::Load(String^ filename)
{
	DECLARE_NATIVE_STRING(o_filename, filename);

	_native->load(o_filename);
}

void ConfigFile::Load(String^ filename, String^ resourceGroup, String^ separators, bool trimWhitespace)
{
	DECLARE_NATIVE_STRING(o_filename, filename);
	DECLARE_NATIVE_STRING(o_resourceGroup, resourceGroup);
	DECLARE_NATIVE_STRING(o_separators, separators);

	static_cast<Ogre::ConfigFile*>(_native)->load(o_filename, o_resourceGroup, o_separators, trimWhitespace);
}

void ConfigFile::Load(Mogre::DataStreamPtr^ stream, String^ separators, bool trimWhitespace)
{
	DECLARE_NATIVE_STRING(o_separators, separators);

	_native->load((const Ogre::DataStreamPtr&)stream, o_separators, trimWhitespace);
}

void ConfigFile::LoadDirect(String^ filename, String^ separators, bool trimWhitespace)
{
	DECLARE_NATIVE_STRING(o_filename, filename);
	DECLARE_NATIVE_STRING(o_separators, separators);

	_native->loadDirect(o_filename, o_separators, trimWhitespace);
}
void ConfigFile::LoadDirect(String^ filename, String^ separators)
{
	DECLARE_NATIVE_STRING(o_filename, filename);
	DECLARE_NATIVE_STRING(o_separators, separators);

	_native->loadDirect(o_filename, o_separators);
}

void ConfigFile::LoadDirect(String^ filename)
{
	DECLARE_NATIVE_STRING(o_filename, filename);
	_native->loadDirect(o_filename);
}

void ConfigFile::LoadFromResourceSystem(String^ filename, String^ resourceGroup, String^ separators, bool trimWhitespace)
{
	DECLARE_NATIVE_STRING(o_filename, filename);
	DECLARE_NATIVE_STRING(o_resourceGroup, resourceGroup);
	DECLARE_NATIVE_STRING(o_separators, separators);

	_native->loadFromResourceSystem(o_filename, o_resourceGroup, o_separators, trimWhitespace);
}

void ConfigFile::LoadFromResourceSystem(String^ filename, String^ resourceGroup, String^ separators)
{
	DECLARE_NATIVE_STRING(o_filename, filename);
	DECLARE_NATIVE_STRING(o_resourceGroup, resourceGroup);
	DECLARE_NATIVE_STRING(o_separators, separators);

	_native->loadFromResourceSystem(o_filename, o_resourceGroup, o_separators);
}

void ConfigFile::LoadFromResourceSystem(String^ filename, String^ resourceGroup)
{
	DECLARE_NATIVE_STRING(o_filename, filename);
	DECLARE_NATIVE_STRING(o_resourceGroup, resourceGroup);

	_native->loadFromResourceSystem(o_filename, o_resourceGroup);
}

String^ ConfigFile::GetSetting(String^ key, String^ section)
{
	DECLARE_NATIVE_STRING(o_key, key);
	DECLARE_NATIVE_STRING(o_section, section);

	return TO_CLR_STRING(_native->getSetting(o_key, o_section));
}

String^ ConfigFile::GetSetting(String^ key)
{
	DECLARE_NATIVE_STRING(o_key, key);

	return TO_CLR_STRING(_native->getSetting(o_key));
}

ConfigFile::SectionIterator^ ConfigFile::GetSectionIterator()
{
	return _native->getSectionIterator();
}

ConfigFile::SettingsIterator^ ConfigFile::GetSettingsIterator(String^ section)
{
	DECLARE_NATIVE_STRING(o_section, section);

	return _native->getSettingsIterator(o_section);
}

ConfigFile::SettingsIterator^ ConfigFile::GetSettingsIterator()
{
	return _native->getSettingsIterator();
}

String^ ConfigOption_NativePtr::name::get()
{
	return TO_CLR_STRING(_native->name);
}

void ConfigOption_NativePtr::name::set(String^ value)
{
	DECLARE_NATIVE_STRING(o_value, value);

	_native->name = o_value;
}

String^ ConfigOption_NativePtr::currentValue::get()
{
	return TO_CLR_STRING(_native->currentValue);
}

void ConfigOption_NativePtr::currentValue::set(String^ value)
{
	DECLARE_NATIVE_STRING(o_value, value);

	_native->currentValue = o_value;
}

Mogre::StringVector^ ConfigOption_NativePtr::possibleValues::get()
{
	return Mogre::StringVector::ByValue(_native->possibleValues);
}
void ConfigOption_NativePtr::possibleValues::set(Mogre::StringVector^ value)
{
	_native->possibleValues = value;
}

bool ConfigOption_NativePtr::immutable::get()
{
	return _native->immutable;
}
void ConfigOption_NativePtr::immutable::set(bool value)
{
	_native->immutable = value;
}


Mogre::ConfigOption_NativePtr ConfigOption_NativePtr::Create()
{
	ConfigOption_NativePtr ptr;
	ptr._native = new Ogre::_ConfigOption();
	return ptr;
}

CPP_DECLARE_STLMAP(, ConfigOptionMap, String^, Mogre::ConfigOption_NativePtr, Ogre::String, Ogre::ConfigOption);