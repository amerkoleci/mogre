#pragma once

// C++ includes
#include <cmath>
#include <cstdio>
#include <cstdlib>
#include <cstdarg>
#include <cassert>
#include <cmath>
#include <cfloat>
#include <ctime>
#include <cstring>
#include <iostream>
#include <fstream>
#include <sstream>
#include <string>
#include <vector>
#include <list>
#include <map>
#include <algorithm>
#include <sys/stat.h>


using std::memcpy;
using std::size_t;
using std::min;
using std::max;
using std::ostream;
using std::basic_ostream;
using std::endl;
using std::string;

#if defined(WIN32)
#pragma warning( disable : 4005 )
#pragma warning( disable : 4172 )
#pragma warning( disable : 4189)
#pragma warning( disable : 4244 )
#pragma warning( disable : 4267 )
#pragma warning( disable : 4311 )
#pragma warning( disable : 4390 )
#pragma warning( disable : 4456 )
#pragma warning( disable : 4458 )
#pragma warning( disable : 4477 )
#pragma warning( disable : 4701 )
#pragma warning( disable : 4800 )
#pragma warning( disable : 4996 )
#endif

#include <Ogre.h>
using namespace Ogre;

#if defined(__CYGWIN32__)
#	define MOGRE_INTERFACE_EXPORT __declspec(dllexport)
#	define MOGRE_INTERFACE_IMPORT __declspec(dllimport)

#elif defined(WIN32) || defined(_WIN32) || defined(__WIN32__) || defined(_WIN64) || defined(WINAPI_FAMILY)
#	define MOGRE_INTERFACE_EXPORT __declspec(dllexport)
#	define MOGRE_INTERFACE_IMPORT __declspec(dllimport)
#elif defined(__MACH__) || defined(__ANDROID__) || defined(__linux__) || defined(__QNX__)
#	define MOGRE_INTERFACE_EXPORT
#	define MOGRE_INTERFACE_IMPORT
#else
#	define MOGRE_INTERFACE_EXPORT
#	define MOGRE_INTERFACE_IMPORT
#endif

#ifdef MOGRE_EXPORTS
#	define MOGRE_EXPORTS_API			MOGRE_INTERFACE_EXPORT
#else
#	define MOGRE_EXPORTS_API			MOGRE_INTERFACE_IMPORT
#endif

template <typename T>
void SafeDelete(T*& resource)
{
	delete resource;
	resource = nullptr;
}
template <typename T>
void SafeDeleteArray(T*& resource)
{
	delete[] resource;
	resource = nullptr;
}

char* CreateOutString(const string& str);
char* CreateOutString(const char* str);