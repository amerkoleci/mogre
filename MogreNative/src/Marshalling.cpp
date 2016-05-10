#include "Marshalling.h"
#include <iostream>
#include <stdio.h>


namespace Mogre
{
	void InitNativeStringWithCLRString(Ogre::String& ostr, System::String^ mstr)
	{
		if (mstr == nullptr)
			throw gcnew System::NullReferenceException("A null string cannot be converted to an Ogre string.");

		IntPtr p_mstr = Marshal::StringToHGlobalAnsi(mstr);
		ostr = static_cast<char*>(p_mstr.ToPointer());
		Marshal::FreeHGlobal( p_mstr );

	}

	void InitNativeUTFStringWithCLRString(Ogre::UTFString& ostr, System::String^ mstr)
	{
		if (mstr == nullptr)
			throw gcnew System::NullReferenceException("A null string cannot be converted to an Ogre UTFString.");
				
		pin_ptr<const wchar_t> p_mstr = PtrToStringChars(mstr);	
		Ogre::UTFString tmp;
		ostr = tmp.assign(p_mstr);
	}

	void FillMapFromNameValueCollection( std::map<Ogre::String,Ogre::String>& map, Collections::Specialized::NameValueCollection^ col )
	{
		int count = col->Count;
		for (int i=0; i < count; i++)
		{
			DECLARE_NATIVE_STRING( o_key, col->Keys[i] )
			DECLARE_NATIVE_STRING( o_val, col[i] )
			map.insert( std::pair<Ogre::String,Ogre::String>( o_key, o_val ) );
		}
	}
}