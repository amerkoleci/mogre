#include "OgreStableHeaders.h"
#include "CLRObject.h"
#include "CLRConfig.h"

#if LINK_TO_MOGRE
	#ifdef _DEBUG
		#pragma comment(lib, "Mogre.lib")
	#else
		#pragma comment(lib, "Mogre.lib")
	#endif

namespace Mogre {
#define CLROBJECT(T) \
  __declspec(dllimport) void _Init_CLRObject_##T(CLRObject *pObj);
#include "../auto/include/CLRObjects.inc"
#undef CLROBJECT
}

#define CLROBJECT(T) \
  void __Init_CLRObject_##T(CLRObject *pObj) { \
    Mogre::_Init_CLRObject_##T(pObj); \
  }

namespace Mogre {
  namespace Implementation {
    __declspec(dllimport) void FreeCLRObject(void* handle);
  }
}

void _FreeCLRObject(void* handle)
{
  Mogre::Implementation::FreeCLRObject(handle);
}

#else

#define CLROBJECT(T) \
  void __Init_CLRObject_##T(CLRObject *pObj) { }

void _FreeCLRObject(void* handle) { }

#endif

#include "../auto/include/CLRObjects.inc"
#undef CLROBJECT
