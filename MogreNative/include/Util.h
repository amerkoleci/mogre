#pragma once

namespace Mogre
{
	private ref class Util
	{
	internal:
		static String^ ToManagedString(const char* string);
		static String^ ToManagedString(const std::string& string);
		static char* ToUnmanagedString(String^ string);
	};
}