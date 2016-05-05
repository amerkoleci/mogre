#include "stdafx.h"


static char* CreateOutString(const string& str)
{
	char* result = new char[str.length() + 1];
	strcpy(result, str.c_str());
	return result;
}

static char* CreateOutString(const char* str)
{
	char* result = new char[strlen(str) + 1];
	strcpy(result, str);
	return result;
}

extern "C"
{
	MOGRE_EXPORTS_API void Mogre_DeleteString(char* str)
	{
		delete[] str;
	}

	MOGRE_EXPORTS_API Ogre::Root* Root_New(const char* pluginFileName, const char* configFileName, const char* logFileName)
	{
		return new Ogre::Root(pluginFileName, configFileName, logFileName);
	}

	MOGRE_EXPORTS_API void Root_Delete(Ogre::Root* _this)
	{
		SafeDelete(_this);
	}
}