#pragma once

#include "CLRObject.h"

namespace Mogre
{
	namespace Implementation
	{
		using namespace System;
		using namespace System::Reflection;
		using namespace System::Collections::Generic;

		public ref class Wrapper abstract
		{
		internal:
			CLRObject* _native;

			//Currently, when creating an overridable object, first the Wrapper( CLRObject* nativePtr ) constructor is called
			//with 0 as parameter and these fields are set afterwards.
			//Otherwise they will be false;
			bool _isOverriden;
			bool _createdByCLR;

			Wrapper( CLRObject* nativePtr ) : _native(nativePtr)
			{
			}

			~Wrapper()
			{
				if (_createdByCLR && _native != 0) { delete _native; _native = 0; }
			}

			Wrapper() : _native(0)
			{
			}

		public:
			property CLRObject* NativePtr
			{
				CLRObject* get()
				{
					return _native;
				}
			}
		};

		public interface class INativePointer
		{
			void ClearNativePtr();
		};

                __declspec(dllexport) void FreeCLRObject(void* handle);
                __declspec(dllexport) void FreeCLRHandle(void* handle);
	}
}