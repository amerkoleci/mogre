#include "StdAfx.h"
#include "Util.h"

using namespace Mogre;


String^ Util::ToManagedString(const char* string)
{
	return Marshal::PtrToStringAnsi((IntPtr)(char*)string);
}

String^ Util::ToManagedString(const std::string& string)
{
	return Marshal::PtrToStringAnsi((IntPtr)(char*)string.c_str());
}

char* Util::ToUnmanagedString(String^ string)
{
	return (char*)Marshal::StringToHGlobalAnsi(string).ToPointer();
}
