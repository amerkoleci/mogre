#pragma once

#include "OgreDataStream.h"
#include "Marshalling.h"
#include "STLContainerWrappers.h"

namespace Mogre
{
	ref class DataStreamPtr;

	public ref class DataStream
	{
	internal:
		Ogre::DataStream* _native;
		bool _createdByCLR;

	protected:
		DataStream(Ogre::DataStream* obj) : _native(obj), _createdByCLR(false)
		{
		}

		~DataStream()
		{
			if (_createdByCLR && _native)
			{
				delete _native;
				_native = 0;
			}
		}

	public:
		property String^ AsString
		{
		public:
			String^ get();
		}

		property String^ Name
		{
		public:
			String^ get();
		}

		size_t Read(void* buf, size_t count);

		size_t ReadLine([Out] char% buf, size_t maxCount, String^ delim);
		size_t ReadLine([Out] char% buf, size_t maxCount);

		String^ GetLine(bool trimAfter);
		String^ GetLine();

		size_t SkipLine(String^ delim);
		size_t SkipLine();

		void Skip(long count);

		void Seek(size_t pos);

		size_t Tell();

		bool Eof();

		size_t Size();

		void Close();

		DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_PLAINWRAPPER(DataStream);
	};

	public ref class MemoryDataStream : public DataStream
	{
	public protected:
		MemoryDataStream(Ogre::DataStream* obj) : DataStream(obj)
		{
		}


		//Public Declarations
	public:
		MemoryDataStream(void* pMem, size_t size);
		MemoryDataStream(String^ name, void* pMem, size_t size);
		MemoryDataStream(Mogre::DataStream^ sourceStream);
		MemoryDataStream(Mogre::DataStreamPtr^ sourceStream);
		MemoryDataStream(String^ name, Mogre::DataStream^ sourceStream);
		MemoryDataStream(String^ name, Mogre::DataStreamPtr^ sourceStream);
		MemoryDataStream(size_t size);
		MemoryDataStream(String^ name, size_t size);

		property Ogre::uchar* CurrentPtr
		{
		public:
			Ogre::uchar* get();
		}

		property Ogre::uchar* Ptr
		{
		public:
			Ogre::uchar* get();
		}

		size_t Read(void* buf, size_t count);

		size_t ReadLine([Out] char% buf, size_t maxCount, String^ delim);
		size_t ReadLine([Out] char% buf, size_t maxCount);

		size_t SkipLine(String^ delim);
		size_t SkipLine();

		void Skip(long count);

		void Seek(size_t pos);

		size_t Tell();

		bool Eof();

		void Close();

		void SetFreeOnClose(bool free);

		DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_PLAINWRAPPER(MemoryDataStream);
	};

	public ref class DataStreamPtr : public DataStream
	{
	public protected:
		Ogre::DataStreamPtr* _sharedPtr;

		DataStreamPtr(Ogre::DataStreamPtr& sharedPtr) : DataStream(sharedPtr.getPointer())
		{
			_sharedPtr = new Ogre::DataStreamPtr(sharedPtr);
		}

		!DataStreamPtr()
		{
			if (_sharedPtr != 0)
			{
				delete _sharedPtr;
				_sharedPtr = 0;
			}
		}

		~DataStreamPtr()
		{
			this->!DataStreamPtr();
		}

	public:
		static operator DataStreamPtr ^ (const Ogre::DataStreamPtr& ptr)
		{
			if (ptr.isNull()) return nullptr;
			return gcnew DataStreamPtr(*(new Ogre::DataStreamPtr(ptr)));
		}

		static operator Ogre::DataStreamPtr& (DataStreamPtr^ t)
		{
			if (CLR_NULL == t) return Ogre::DataStreamPtr();
			return *(t->_sharedPtr);
		}

		static operator Ogre::DataStreamPtr* (DataStreamPtr^ t)
		{
			if (CLR_NULL == t) return nullptr;
			return t->_sharedPtr;
		}

		DataStreamPtr(DataStream^ obj) : DataStream(obj->_native)
		{
			_sharedPtr = new Ogre::DataStreamPtr(static_cast<Ogre::DataStream*>(obj->_native));
		}

		virtual bool Equals(Object^ obj) override
		{
			DataStreamPtr^ clr = dynamic_cast<DataStreamPtr^>(obj);
			if (clr == CLR_NULL)
			{
				return false;
			}

			return (_native == clr->_native);
		}
		bool Equals(DataStreamPtr^ obj)
		{
			if (obj == CLR_NULL)
			{
				return false;
			}

			return (_native == obj->_native);
		}

		static bool operator == (DataStreamPtr^ val1, DataStreamPtr^ val2)
		{
			if ((Object^)val1 == (Object^)val2) return true;
			if ((Object^)val1 == nullptr || (Object^)val2 == nullptr) return false;
			return (val1->_native == val2->_native);
		}

		static bool operator != (DataStreamPtr^ val1, DataStreamPtr^ val2)
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

		property DataStream^ Target
		{
			DataStream^ get()
			{
				return static_cast<Ogre::DataStream*>(_native);
			}
		}
	};

	INC_DECLARE_STLLIST(DataStreamList, Mogre::DataStreamPtr^, Ogre::DataStreamPtr, public, private);

	public ref class DataStreamListPtr : public DataStreamList
	{
	public protected:
		Ogre::DataStreamListPtr* _sharedPtr;

		DataStreamListPtr(Ogre::DataStreamListPtr& sharedPtr) : DataStreamList(sharedPtr.getPointer())
		{
			_sharedPtr = new Ogre::DataStreamListPtr(sharedPtr);
		}

		!DataStreamListPtr()
		{
			if (_sharedPtr != 0)
			{
				delete _sharedPtr;
				_sharedPtr = 0;
			}
		}

		~DataStreamListPtr()
		{
			this->!DataStreamListPtr();
		}

	public:
		static operator DataStreamListPtr ^ (const Ogre::DataStreamListPtr& ptr)
		{
			if (ptr.isNull()) return nullptr;
			return gcnew DataStreamListPtr(*(new Ogre::DataStreamListPtr(ptr)));
		}

		static operator Ogre::DataStreamListPtr& (DataStreamListPtr^ t)
		{
			if (CLR_NULL == t) return Ogre::DataStreamListPtr();
			return *(t->_sharedPtr);
		}

		static operator Ogre::DataStreamListPtr* (DataStreamListPtr^ t)
		{
			if (CLR_NULL == t) return nullptr;
			return t->_sharedPtr;
		}

		DataStreamListPtr(DataStreamList^ obj) : DataStreamList(obj->_native)
		{
			_sharedPtr = new Ogre::DataStreamListPtr(static_cast<Ogre::DataStreamList*>(obj->_native));
		}

		virtual bool Equals(Object^ obj) override
		{
			DataStreamListPtr^ clr = dynamic_cast<DataStreamListPtr^>(obj);
			if (clr == CLR_NULL)
			{
				return false;
			}

			return (_native == clr->_native);
		}
		bool Equals(DataStreamListPtr^ obj)
		{
			if (obj == CLR_NULL)
			{
				return false;
			}

			return (_native == obj->_native);
		}

		static bool operator == (DataStreamListPtr^ val1, DataStreamListPtr^ val2)
		{
			if ((Object^)val1 == (Object^)val2) return true;
			if ((Object^)val1 == nullptr || (Object^)val2 == nullptr) return false;
			return (val1->_native == val2->_native);
		}

		static bool operator != (DataStreamListPtr^ val1, DataStreamListPtr^ val2)
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

		property DataStreamList^ Target
		{
			DataStreamList^ get()
			{
				return static_cast<Ogre::DataStreamList*>(_native);
			}
		}
	};


	public ref class MemoryDataStreamPtr : public MemoryDataStream
	{
	public protected:
		Ogre::MemoryDataStreamPtr* _sharedPtr;

		MemoryDataStreamPtr(Ogre::MemoryDataStreamPtr& sharedPtr) : MemoryDataStream(sharedPtr.getPointer())
		{
			_sharedPtr = new Ogre::MemoryDataStreamPtr(sharedPtr);
		}

		!MemoryDataStreamPtr()
		{
			if (_sharedPtr != 0)
			{
				delete _sharedPtr;
				_sharedPtr = 0;
			}
		}

		~MemoryDataStreamPtr()
		{
			this->!MemoryDataStreamPtr();
		}

	public:
		static operator MemoryDataStreamPtr ^ (const Ogre::MemoryDataStreamPtr& ptr)
		{
			if (ptr.isNull()) return nullptr;
			return gcnew MemoryDataStreamPtr(*(new Ogre::MemoryDataStreamPtr(ptr)));
		}

		static operator Ogre::MemoryDataStreamPtr& (MemoryDataStreamPtr^ t)
		{
			if (CLR_NULL == t) return Ogre::MemoryDataStreamPtr();
			return *(t->_sharedPtr);
		}

		static operator Ogre::MemoryDataStreamPtr* (MemoryDataStreamPtr^ t)
		{
			if (CLR_NULL == t) return nullptr;
			return t->_sharedPtr;
		}

		MemoryDataStreamPtr(MemoryDataStream^ obj) : MemoryDataStream(obj->_native)
		{
			_sharedPtr = new Ogre::MemoryDataStreamPtr(static_cast<Ogre::MemoryDataStream*>(obj->_native));
		}

		virtual bool Equals(Object^ obj) override
		{
			MemoryDataStreamPtr^ clr = dynamic_cast<MemoryDataStreamPtr^>(obj);
			if (clr == CLR_NULL)
			{
				return false;
			}

			return (_native == clr->_native);
		}
		bool Equals(MemoryDataStreamPtr^ obj)
		{
			if (obj == CLR_NULL)
			{
				return false;
			}

			return (_native == obj->_native);
		}

		static bool operator == (MemoryDataStreamPtr^ val1, MemoryDataStreamPtr^ val2)
		{
			if ((Object^)val1 == (Object^)val2) return true;
			if ((Object^)val1 == nullptr || (Object^)val2 == nullptr) return false;
			return (val1->_native == val2->_native);
		}

		static bool operator != (MemoryDataStreamPtr^ val1, MemoryDataStreamPtr^ val2)
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

		property MemoryDataStream^ Target
		{
			MemoryDataStream^ get()
			{
				return static_cast<Ogre::MemoryDataStream*>(_native);
			}
		}
	};
}