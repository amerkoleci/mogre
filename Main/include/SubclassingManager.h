#pragma once

#pragma managed(push, off)
#include "OgrePrerequisites.h"
#pragma managed(pop)

namespace Mogre
{
	namespace Implementation
	{
		using namespace System;
		using namespace System::Reflection;
		using namespace System::Collections::Generic;

		static const int CACHED_RETURN_SIZE = 512;

		extern unsigned char cachedReturn[CACHED_RETURN_SIZE];
		extern Ogre::String cachedReturnString;


		ref struct MethodIndexAttribute : Attribute
		{
			int index;
			MethodIndexAttribute(int mindex) : index(mindex)
			{
			}
		};


		ref class SubclassingManager
		{
			static SubclassingManager^ _instance = gcnew SubclassingManager;

			Dictionary<Type^,IntPtr>^ _overridenMethodArrays;

		public:
			SubclassingManager()
			{
				_overridenMethodArrays = gcnew Dictionary<Type^,IntPtr>;
			}

			!SubclassingManager()
			{
				if (_overridenMethodArrays != nullptr)
				{
					for each (IntPtr ptr in _overridenMethodArrays->Values)
					{
						bool* parr = (bool*)(void*)ptr;
						delete[] parr;
					}

					_overridenMethodArrays = nullptr;
				}
			}
			~SubclassingManager()
			{
				this->!SubclassingManager();
			}

			static property SubclassingManager^ Instance
			{
				SubclassingManager^ get()
				{
					return _instance;
				}
			}

			bool* GetOverridenMethodsArrayPointer(Type^ subclass, Type^ baseclass, int arraySize);

			void SetOverridenMethods(Type^ subclass, Type^ baseclass, bool* pOverriden, int arraySize);
		};
	}
}
