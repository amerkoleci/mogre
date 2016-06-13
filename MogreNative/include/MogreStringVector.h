#pragma once

#include "OgreStringVector.h"
#include "STLContainerWrappers.h"

namespace Mogre
{
	INC_DECLARE_STLVECTOR(StringVector, String^, Ogre::String, public, private);

	public ref class StringVectorPtr : public StringVector
	{
	public protected:
		Ogre::StringVectorPtr* _sharedPtr;

		StringVectorPtr(Ogre::StringVectorPtr& sharedPtr) : StringVector(sharedPtr.getPointer())
		{
			_sharedPtr = new Ogre::StringVectorPtr(sharedPtr);
		}

		!StringVectorPtr()
		{
			if (_sharedPtr != 0)
			{
				delete _sharedPtr;
				_sharedPtr = 0;
			}
		}

		~StringVectorPtr()
		{
			this->!StringVectorPtr();
		}

	public:
		static operator StringVectorPtr ^ (const Ogre::StringVectorPtr& ptr)
		{
			if (ptr.isNull()) return nullptr;
			return gcnew StringVectorPtr(*(new Ogre::StringVectorPtr(ptr)));
		}

		static operator Ogre::StringVectorPtr& (StringVectorPtr^ t)
		{
			if (CLR_NULL == t) return Ogre::StringVectorPtr();
			return *(t->_sharedPtr);
		}

		static operator Ogre::StringVectorPtr* (StringVectorPtr^ t)
		{
			if (CLR_NULL == t) return nullptr;
			return t->_sharedPtr;
		}


		StringVectorPtr(StringVector^ obj) : StringVector(obj->_native)
		{
			_sharedPtr = new Ogre::StringVectorPtr(static_cast<Ogre::StringVector*>(obj->_native));
		}

		virtual bool Equals(Object^ obj) override
		{
			StringVectorPtr^ clr = dynamic_cast<StringVectorPtr^>(obj);
			if (clr == CLR_NULL)
			{
				return false;
			}

			return (_native == clr->_native);
		}
		bool Equals(StringVectorPtr^ obj)
		{
			if (obj == CLR_NULL)
			{
				return false;
			}

			return (_native == obj->_native);
		}

		static bool operator == (StringVectorPtr^ val1, StringVectorPtr^ val2)
		{
			if ((Object^)val1 == (Object^)val2) return true;
			if ((Object^)val1 == nullptr || (Object^)val2 == nullptr) return false;
			return (val1->_native == val2->_native);
		}

		static bool operator != (StringVectorPtr^ val1, StringVectorPtr^ val2)
		{
			return !(val1 == val2);
		}

		virtual int GetHashCode() override
		{
			return reinterpret_cast<int>(_native);
		}

		property IntPtr NativePtr
		{
			IntPtr get() { return (IntPtr)_sharedPtr; }
		}

		property bool Unique
		{
			bool get()
			{
				return (*_sharedPtr).unique();
			}
		}

		property int UseCount
		{
			int get()
			{
				return (*_sharedPtr).useCount();
			}
		}

		property StringVector^ Target
		{
			StringVector^ get()
			{
				return static_cast<Ogre::StringVector*>(_native);
			}
		}
	};
}