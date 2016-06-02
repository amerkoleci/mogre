#include "stdafx.h"
#include "MogreException.h"

using namespace Mogre;

OgreException::OgreException(int number, String^ description, String^ source)
{
	_createdByCLR = true;
	DECLARE_NATIVE_STRING(o_description, description);
	DECLARE_NATIVE_STRING(o_source, source);

	_native = new Ogre::Exception(number, o_description, o_source);
}

OgreException::OgreException(int number, String^ description, String^ source, const char* type, const char* file, long line)
{
	_createdByCLR = true;
	DECLARE_NATIVE_STRING(o_description, description);
	DECLARE_NATIVE_STRING(o_source, source);

	_native = new Ogre::Exception(number, o_description, o_source, type, file, line);
}

OgreException::OgreException(Mogre::OgreException^ rhs)
{
	_createdByCLR = true;
	_native = new Ogre::Exception(rhs);
}

String^ OgreException::Description::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::Exception*>(_native)->getDescription());
}

String^ OgreException::File::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::Exception*>(_native)->getFile());
}

String^ OgreException::FullDescription::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::Exception*>(_native)->getFullDescription());
}

bool OgreException::IsThrown::get()
{
	// TODO:
	//return Ogre::Exception::getIsThrown();
	return false;
}


Mogre::OgreException^ OgreException::LastException::get()
{
	//return Ogre::Exception::getLastException();
	return nullptr;
}

long OgreException::Line::get()
{
	return static_cast<const Ogre::Exception*>(_native)->getLine();
}

Mogre::OgreException::ExceptionCodes OgreException::Number::get()
{
	return (Mogre::OgreException::ExceptionCodes)static_cast<const Ogre::Exception*>(_native)->getNumber();
}

String^ OgreException::Source::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::Exception*>(_native)->getSource());
}

const char* OgreException::What()
{
	return static_cast<const Ogre::Exception*>(_native)->what();
}