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

	public value class ConfigOption_NativePtr
	{
	private protected:
		Ogre::_ConfigOption* _native;

	public:
		property String^ name
		{
		public:
			String^ get();
		public:
			void set(String^ value);
		}

		property String^ currentValue
		{
		public:
			String^ get();
		public:
			void set(String^ value);
		}

		property Mogre::StringVector^ possibleValues
		{
		public:
			Mogre::StringVector^ get();
		public:
			void set(Mogre::StringVector^ value);
		}

		property bool immutable
		{
		public:
			bool get();
		public:
			void set(bool value);
		}

		DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_NATIVEPTRVALUECLASS(ConfigOption_NativePtr, Ogre::_ConfigOption);

		property IntPtr NativePtr
		{
			IntPtr get() { return (IntPtr)_native; }
		}

		static ConfigOption_NativePtr Create();

		void DestroyNativePtr()
		{
			if (_native) { delete _native; _native = 0; }
		}

		property bool IsNull
		{
			bool get() { return (_native == 0); }
		}
	};

	typedef Mogre::ConfigOption_NativePtr ConfigOption_NativePtr;
	INC_DECLARE_STLMAP(ConfigOptionMap, String^, Mogre::ConfigOption_NativePtr, Ogre::String, Ogre::ConfigOption, public, private);
}