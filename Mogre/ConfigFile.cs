using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Mogre
{
    public class ConfigFile : OgreNativeObject
    {
        public ConfigFile() : base(ConfigFile_new())
        {

        }

        protected override void NativeDelete()
        {
            ConfigFile_delete(_handle);
        }

        public void Clear()
        {
            ConfigFile_clear(_handle);
        }

        public void Load(string fileName, string separators = "\t:=", bool trimWhitespace = true)
        {
            ConfigFile_load(_handle, fileName, separators, trimWhitespace);
        }

        public void Load(string fileName, string resourceGroup, string separators = "\t:=", bool trimWhitespace = true)
        {
            ConfigFile_load2(_handle, fileName, resourceGroup, separators, trimWhitespace);
        }

        public void LoadDirect(string fileName, string separators = "\t:=", bool trimWhitespace = true)
        {
            ConfigFile_loadDirect(_handle, fileName, separators, trimWhitespace);
        }

        public string GetSetting(string key, string section = "", string defaultValue = "")
        {
            using (var wrapperString = new WrapperString(ConfigFile_getSetting(_handle, key, section, defaultValue)))
            {
                return wrapperString.StringValue;
            }
        }

        public SectionIterator GetSectionIterator()
        {
            return new SectionIterator(ConfigFile_getSectionIterator(_handle));
        }

        public sealed class SectionIterator : IEnumerable<KeyValuePair<string, string>>
        {
            readonly IntPtr _handle;
            int _index = 0;
            readonly List<string> _keys = new List<string>();
            string _readKey;
            readonly Dictionary<string, Dictionary<string, string>> _keysValues = new Dictionary<string, Dictionary<string, string>>();

            internal SectionIterator(IntPtr handle)
            {
                _handle = handle;
                while (SectionIterator_hasMoreElements(_handle))
                {
                    using (var wrapper = new WrapperString(SectionIterator_peekNextKey(_handle)))
                    {
                        _keys.Add(wrapper.StringValue);
                        _readKey = wrapper.StringValue;
                    }

                    IntPtr settings =  SectionIterator_getNext(_handle);
                    SettingsMultiMap_iterate(settings, AddSettingsCallback);
                }
            }

            void AddSettingsCallback(string key, string value)
            {
                Dictionary<string, string> values;
                if(_keysValues.TryGetValue(_readKey, out values) ==false)
                {
                    values = new Dictionary<string, string>();
                    _keysValues.Add(_readKey, values);
                }

                values.Add(key, value);
            }

            public string CurrentKey
            {
                get;
                private set;
            }

            public void Dispose()
            {
                SectionIterator_delete(_handle);
            }

            public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
            {
                return _keysValues[CurrentKey].GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _keysValues[CurrentKey].GetEnumerator();
            }

            public bool MoveNext()
            {
                _index++;
                if (_index >= _keys.Count)
                    return false;

                CurrentKey = _keys[_index];
                return true;
            }

            public void Reset()
            {
                throw new NotSupportedException("Reset is not supported.");
            }

            
        }

        #region PInvoke

        [DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr ConfigFile_new();

        [DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        static extern void ConfigFile_delete(IntPtr handle);

        [DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        static extern void ConfigFile_clear(IntPtr handle);

        [DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        static extern void ConfigFile_load(IntPtr handle, string fileName, string separators, [MarshalAs(UnmanagedType.U1)] bool trimWhitespace);

        [DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        static extern void ConfigFile_load2(IntPtr handle, string fileName, string resourceGroup, string separators, [MarshalAs(UnmanagedType.U1)] bool trimWhitespace);

        [DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        static extern void ConfigFile_loadDirect(IntPtr handle, string fileName, string separators, [MarshalAs(UnmanagedType.U1)] bool trimWhitespace);

        [DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr ConfigFile_getSetting(IntPtr handle, string key, string section, string defaultValue);

        [DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr ConfigFile_getSectionIterator(IntPtr handle);

        [DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        static extern void SectionIterator_delete(IntPtr handle);

        [return: MarshalAs(UnmanagedType.U1)]
        [DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        static extern bool SectionIterator_hasMoreElements(IntPtr handle);

        [DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr SectionIterator_peekNextKey(IntPtr handle);

        [DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr SectionIterator_getNext(IntPtr handle);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void SettingsMultiMap_IterateCallback(string key, string value);

        [DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr SettingsMultiMap_iterate(IntPtr handle, SettingsMultiMap_IterateCallback callback);

        #endregion
    }
}
