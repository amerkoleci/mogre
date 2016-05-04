#pragma once

#pragma managed(push, off)
#include "OgrePlatform.h"
#pragma managed(pop)

#if defined( __cplusplus_cli )
#pragma managed(push, on)
#endif

class CLRObject;

#define CLROBJECT(T) \
  void _OgreExport __Init_CLRObject_##T(CLRObject *pObj);
#include "../auto/include/CLRObjects.inc"
#undef CLROBJECT

//for subclasses of CLRObject
#define DECLARE_INIT_CLROBJECT_METHOD_OVERRIDE(T)	virtual void _Init_CLRObject(void) { __Init_CLRObject_##T(this); }


#if defined( __cplusplus_cli )
	//Make CLRObject and CLRHandle public native types for Mogre
	#define PUBLIC_FOR_CLI	public

	// If this file is included in managed code expose all the managed stuff,
	// otherwise hide them.
	#define SHOW_MANAGED_CLROBJECT
	#define __GCHANDLE_TO_VOIDPTR(x) ((GCHandle::operator System::IntPtr(x)).ToPointer())
	#define __VOIDPTR_TO_GCHANDLE(x) (GCHandle::operator GCHandle(System::IntPtr(x)))
#else
	#define PUBLIC_FOR_CLI
#endif

// Free the handle in a separate function in managed context. CLRObject's destructor
// will call it only if it's necessary.
void _OgreExport _FreeCLRObject(void* handle);

PUBLIC_FOR_CLI  class CLRObject
{
	void* _handle;

public:
	CLRObject() : _handle(0)
	{
	}
	// Don't let the GC handle get copied by value, otherwise it may get freed multiple times, leading to crashes.
	CLRObject(const CLRObject& clrObject) : _handle(0)
	{
	}
	CLRObject& operator=(const CLRObject& clrObject)
	{
		return *this;
	}
	virtual ~CLRObject()
	{
		if (_handle != 0)
			_FreeCLRObject(_handle);
	}

	virtual void _Init_CLRObject(void) = 0;

#ifdef SHOW_MANAGED_CLROBJECT

	typedef System::Runtime::InteropServices::GCHandle GCHandle;
	typedef System::Runtime::InteropServices::GCHandleType GCHandleType;
	typedef System::Object Object;

	CLRObject(Object^ t)
	{
		_handle = __GCHANDLE_TO_VOIDPTR(GCHandle::Alloc(t, GCHandleType::Normal));
	}

	inline CLRObject& operator=(Object^ t) {
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