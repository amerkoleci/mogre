#pragma once

#include "STLContainerWrappers.h"
#include "IteratorWrapper.h"
#include "OgreConfigFile.h"
#include "MogreStringVector.h"

namespace Mogre
{
	ref class DataStreamPtr;

	public ref class ConfigFile 
	{
		//Nested Types
	public: 
		ref class SettingsMultiMap;
		ref class SettingsBySection;

		INC_DECLARE_STLMULTIMAP(SettingsMultiMap, String^, String^, Ogre::String, Ogre::String, public:, private:);
		INC_DECLARE_STLMAP(SettingsBySection, String^, Mogre::ConfigFile::SettingsMultiMap^, Ogre::String, Ogre::ConfigFile::SettingsMultiMap*, public:, private:);
		INC_DECLARE_MAP_ITERATOR(SettingsIterator, Ogre::ConfigFile::SettingsIterator, Mogre::ConfigFile::SettingsMultiMap, String^, Ogre::String, String^, Ogre::String);
		INC_DECLARE_MAP_ITERATOR(SectionIterator, Ogre::ConfigFile::SectionIterator, Mogre::ConfigFile::SettingsBySection, Mogre::ConfigFile::SettingsMultiMap^, Ogre::ConfigFile::SettingsMultiMap*, String^, Ogre::String);

	private:
		Ogre::ConfigFile* _native;

	public:
		/// <summary>Creates a new instance of the ConfigFile class.</summary>
		ConfigFile();

	public:
		~ConfigFile();
	protected:
		!ConfigFile();
	public:
		void Load(String^ fileName, String^ separators, bool trimWhitespace);
		void Load(String^ fileName, String^ separators);
		void Load(String^ fileName);
		void Load(String^ filename, String^ resourceGroup, String^ separators, bool trimWhitespace);
		void Load(Mogre::DataStreamPtr^ stream, String^ separators, bool trimWhitespace);

		void LoadDirect(String^ filename, String^ separators, bool trimWhitespace);
		void LoadDirect(String^ filename, String^ separators);
		void LoadDirect(String^ filename);

		void LoadFromResourceSystem(String^ filename, String^ resourceGroup, String^ separators, bool trimWhitespace);
		void LoadFromResourceSystem(String^ filename, String^ resourceGroup, String^ separators);
		void LoadFromResourceSystem(String^ filename, String^ resourceGroup);

		String^ GetSetting(String^ key, String^ section);
		String^ GetSetting(String^ key);

		Mogre::ConfigFile::SectionIterator^ GetSectionIterator();

		Mogre::ConfigFile::SettingsIterator^ GetSettingsIterator(String^ section);
		Mogre::ConfigFile::SettingsIterator^ GetSettingsIterator();

		void Clear();
	};
}