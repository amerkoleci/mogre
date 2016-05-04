#include "MogreStableHeaders.h"
#include "Wrapper.h"

typedef System::Runtime::InteropServices::GCHandle GCHandle;

namespace Mogre
{
  namespace Implementation
  {
    void FreeCLRObject(void* handle)
    {
      GCHandle g = __VOIDPTR_TO_GCHANDLE(handle);
      if (g.Target != nullptr)
      {
        // Set _native pointer of CLR object to 0
        static_cast<Mogre::Implementation::Wrapper^>(g.Target)->_native = 0;
        // Dispose the CLR object
        delete g.Target;
      }
      g.Free();
    }

    void FreeCLRHandle(void* handle)
    {
      GCHandle g = __VOIDPTR_TO_GCHANDLE(handle);
      if (g.Target != nullptr)
      {
        // Set _native pointer of CLR object to 0
        static_cast<Mogre::Implementation::INativePointer^>(g.Target)->ClearNativePtr();
        // Dispose the CLR object
        delete g.Target;
      }
      g.Free();
    }
  }
}