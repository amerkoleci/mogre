#include "OgreStableHeaders.h"
#include "CLRHandle.h"
#include "CLRConfig.h"

#if LINK_TO_MOGRE

namespace Mogre {
  namespace Implementation {
    __declspec(dllimport) void FreeCLRHandle(void* handle);
  }
}

void _FreeCLRHandle(void* handle)
{
  Mogre::Implementation::FreeCLRHandle(handle);
}

#else

void _FreeCLRHandle(void* handle) { }

#endif
