/*
** Copyright (C) Amer Koleci
**
** This file is subject to the terms and conditions defined in
** file 'LICENSE.txt', which is part of this source code package.
*/

#pragma once

#include "OgrePrerequisites.h"

/**
* DLL export macros
*/
#if !defined(OGRE_C_EXPORT) 
#	if defined(WIN32)
#		define OGRE_C_EXPORT extern "C"
#	else
#		define OGRE_C_EXPORT
#	endif
#endif

#if defined(__CYGWIN32__)
#	define OGRE_INTERFACE_API __stdcall
#	define OGRE_INTERFACE_EXPORT __declspec(dllexport)
#	define OGRE_INTERFACE_IMPORT __declspec(dllimport)

#elif defined(WIN32) || defined(_WIN32) || defined(__WIN32__) || defined(_WIN64) || defined(WINAPI_FAMILY)
#	define OGRE_INTERFACE_API __stdcall
#	define OGRE_INTERFACE_EXPORT __declspec(dllexport)
#	define OGRE_INTERFACE_IMPORT __declspec(dllimport)
#elif defined(__MACH__) || defined(__ANDROID__) || defined(__linux__) || defined(__QNX__)
#	define OGRE_INTERFACE_API
#	define OGRE_INTERFACE_EXPORT
#	define OGRE_INTERFACE_IMPORT
#else
#	define OGRE_INTERFACE_API
#	define OGRE_INTERFACE_EXPORT
#	define OGRE_INTERFACE_IMPORT
#endif

namespace Ogre
{
	char* CreateOutString(const char* str);
	char* CreateOutString(const Ogre::String& str);
}