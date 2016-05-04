#pragma once

// We have a dependency cycle here: MOGRE (compile-time) depends OGRE and OGRE (runtime) depends 
// on MOGRE. So when you're building OGRE (for MOGRE) the first time, you need to set this to
// 0 until you've build MOGRE.
#define LINK_TO_MOGRE 1


// Without the "mogre.lib" the rendering subsystem won't be available (thus no rendering at all).
#if LINK_TO_MOGRE
#ifdef _DEBUG
#pragma comment(lib, "../../../lib/Debug/mogre.lib")
#else
#pragma comment(lib, "../../../lib/Release/mogre.lib")
#endif
#endif
