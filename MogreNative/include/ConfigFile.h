#pragma once

#include "IDisposable.h"
#include "STLContainerWrappers.h"
#include "IteratorWrapper.h"
#include "OgreConfigFile.h"

namespace Mogre
{
	public ref class ConfigFile 
	{
		//Nested Types
	public: ref class SettingsMultiMap;
	public: ref class SettingsBySection;

#define STLDECL_MANAGEDVALUE String^
#define STLDECL_NATIVEKEY Ogre::String
#define STLDECL_NATIVEVALUE Ogre::String
	public: INC_DECLARE_STLMULTIMAP(SettingsMultiMap, String^, STLDECL_MANAGEDVALUE, STLDECL_NATIVEKEY, STLDECL_NATIVEVALUE, public:, private:)
#undef STLDECL_MANAGEDVALUE
#undef STLDECL_NATIVEKEY
#undef STLDECL_NATIVEVALUE

#define STLDECL_MANAGEDKEY String^
#define STLDECL_MANAGEDVALUE Mogre::ConfigFile::SettingsMultiMap^
#define STLDECL_NATIVEKEY Ogre::String
#define STLDECL_NATIVEVALUE Ogre::ConfigFile::SettingsMultiMap*
	public: INC_DECLARE_STLMAP(SettingsBySection, STLDECL_MANAGEDKEY, STLDECL_MANAGEDVALUE, STLDECL_NATIVEKEY, STLDECL_NATIVEVALUE, public:, private:)
#undef STLDECL_MANAGEDKEY
#undef STLDECL_MANAGEDVALUE
#undef STLDECL_NATIVEKEY
#undef STLDECL_NATIVEVALUE

	public: INC_DECLARE_MAP_ITERATOR(SettingsIterator, Ogre::ConfigFile::SettingsIterator, Mogre::ConfigFile::SettingsMultiMap, String^, Ogre::String, String^, Ogre::String)

	public: INC_DECLARE_MAP_ITERATOR(SectionIterator, Ogre::ConfigFile::SectionIterator, Mogre::ConfigFile::SettingsBySection, Mogre::ConfigFile::SettingsMultiMap^, Ogre::ConfigFile::SettingsMultiMap*, String^, Ogre::String)

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

		void Clear();
		void Load(String^ fileName, String^ separators, bool trimWhitespace);
		void Load(String^ fileName, String^ separators);
		void Load(String^ fileName);

		Mogre::ConfigFile::SectionIterator^ GetSectionIterator();

		Mogre::ConfigFile::SettingsIterator^ GetSettingsIterator(String^ section);
		Mogre::ConfigFile::SettingsIterator^ GetSettingsIterator();
	};
}