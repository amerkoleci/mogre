/*
** Copyright (C) Amer Koleci
**
** This file is subject to the terms and conditions defined in
** file 'LICENSE.txt', which is part of this source code package.
*/

#include "OgreExports.h"

namespace Ogre
{
	char* CreateOutString(const char* str)
	{
		char* result = new char[strlen(str) + 1];
		strcpy(result, str);
		return result;
	}

	char* CreateOutString(const Ogre::String& str)
	{
		char* result = new char[str.length() + 1];
		strcpy(result, str.c_str());
		return result;
	}
}

OGRE_C_EXPORT OGRE_INTERFACE_EXPORT void Native_DeleteString(char* str)
{
	delete[] str;
}
