#pragma once

#pragma managed(push, off)
#include "OgrePlatform.h"
#pragma managed(pop)
#include "CLRObject.h"

#if defined( __cplusplus_cli )
#pragma managed(push, on)
#endif

#define DECLARE_CLRHANDLE CLRHandle _CLRHandle;

// Free the handle in a separate function in managed context. CLRHandle's destructor
// will call it only if it's necessary.
void _OgreExport _FreeCLRHandle(void* handle);

// This is used for plain, struct-like classes that don't form inheritance chains. It is used only for mapping the CLR object that wraps
// the native object. It doesn't have any virtual method so it's a little bit more lightweight that CLRObject and it avoids
// problems with classes that crash when getting a virtual destructor (like GpuProgramParameters).
// CLRHandle is included as public field to a class with "DECLARE_CLRHANDLE".
PUBLIC_FOR_CLI  class CLRHandle
{
	void* _handle;

public:
	CLRHandle() : _handle(0)
	{
	}
	// Don't let the GC handle get copied by value, otherwise it may get freed multiple times, leading to crashes.
	CLRHandle(const CLRHandle& clrHandle) : _handle(0)
	{
	}
	CLRHandle& operator=(const CLRHandle& clrHandle)
	{
		return *this;
	}
	~CLRHandle()
	{
		if (_handle != 0)
			_FreeCLRHandle(_handle);
	}

#ifdef SHOW_MANAGED_CLROBJECT

	typedef System::Runtime::InteropServices::GCHandle GCHandle;
	typedef System::Runtime::InteropServices::GCHandleType GCHandleType;
	typedef System::Object Object;

	CLRHandle(Object^ t)
	{
		_handle = __GCHANDLE_TO_VOIDPTR(GCHandle::Alloc(t, GCHandleType::Normal));
	}

	inline CLRHandle& operator=(Object^ t) {
		if (_handle == 0)
			_handle = __GCHANDLE_TO_VOIDPTR(GCHandle::Alloc(t, GCHandleType::Normal));
		else
			__VOIDPTR_TO_GCHANDLE(_handle).Target = t;

		return *this;
	}

	void _MapToCLRObject(Object^ t, GCHandleType handleType)
	{
		if (_handle == 0)
			_handle = __GCHANDLE_TO_VOIDPTR(GCHandle::Alloc(t, handleType));
		else
			throw gcnew System::Exception("_MapToCLRObject was called on a already mapped native object.");
	}

	inline operator Object^ () {
		if (_handle == 0)
			return nullptr;
		else
			return __VOIDPTR_TO_GCHANDLE(_handle).Target;
	}

#endif

};

#if defined( __cplusplus_cli )
#pragma managed(pop)
#endif