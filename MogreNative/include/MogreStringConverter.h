#pragma once

#include "OgreStringConverter.h"
#include "MogreStringVector.h"
#include "Marshalling.h"

namespace Mogre
{
	public ref class StringConverter
	{
	public protected:
		StringConverter(Ogre::StringConverter* obj) : _native(obj), _createdByCLR(false)
		{
		}

		~StringConverter()
		{
			this->!StringConverter();
		}
		!StringConverter()
		{
			if (_createdByCLR &&_native)
			{
				delete _native;
				_native = 0;
			}
		}

		Ogre::StringConverter* _native;
		bool _createdByCLR;


	public:
		StringConverter();

		static String^ ToString(Ogre::Real val, unsigned short precision, unsigned short width, char fill, std::ios::fmtflags flags);
		static String^ ToString(Ogre::Real val, unsigned short precision, unsigned short width, char fill);
		static String^ ToString(Ogre::Real val, unsigned short precision, unsigned short width);
		static String^ ToString(Ogre::Real val, unsigned short precision);
		static String^ ToString(Ogre::Real val);

		static String^ ToString(Mogre::Radian val, unsigned short precision, unsigned short width, char fill, std::ios::fmtflags flags);
		static String^ ToString(Mogre::Radian val, unsigned short precision, unsigned short width, char fill);
		static String^ ToString(Mogre::Radian val, unsigned short precision, unsigned short width);
		static String^ ToString(Mogre::Radian val, unsigned short precision);
		static String^ ToString(Mogre::Radian val);

		static String^ ToString(Mogre::Degree val, unsigned short precision, unsigned short width, char fill, std::ios::fmtflags flags);
		static String^ ToString(Mogre::Degree val, unsigned short precision, unsigned short width, char fill);
		static String^ ToString(Mogre::Degree val, unsigned short precision, unsigned short width);
		static String^ ToString(Mogre::Degree val, unsigned short precision);
		static String^ ToString(Mogre::Degree val);

		static String^ ToString(int val, unsigned short width, char fill, std::ios::fmtflags flags);
		static String^ ToString(int val, unsigned short width, char fill);
		static String^ ToString(int val, unsigned short width);
		static String^ ToString(int val);

		static String^ ToString(size_t val, unsigned short width, char fill, std::ios::fmtflags flags);
		static String^ ToString(size_t val, unsigned short width, char fill);
		static String^ ToString(size_t val, unsigned short width);
		static String^ ToString(size_t val);

		static String^ ToString(unsigned long val, unsigned short width, char fill, std::ios::fmtflags flags);
		static String^ ToString(unsigned long val, unsigned short width, char fill);
		static String^ ToString(unsigned long val, unsigned short width);
		static String^ ToString(unsigned long val);

		static String^ ToString(long val, unsigned short width, char fill, std::ios::fmtflags flags);
		static String^ ToString(long val, unsigned short width, char fill);
		static String^ ToString(long val, unsigned short width);
		static String^ ToString(long val);

		static String^ ToString(bool val, bool yesNo);
		static String^ ToString(bool val);

		static String^ ToString(Mogre::Vector2 val);

		static String^ ToString(Mogre::Vector3 val);

		static String^ ToString(Mogre::Vector4 val);

		static String^ ToString(Mogre::Matrix3^ val);

		static String^ ToString(Mogre::Matrix4^ val);

		static String^ ToString(Mogre::Quaternion val);

		static String^ ToString(Mogre::ColourValue val);

		static String^ ToString(Mogre::Const_StringVector^ val);

		static Ogre::Real ParseReal(String^ val);

		static Mogre::Radian ParseAngle(String^ val);

		static int ParseInt(String^ val);

		static unsigned int ParseUnsignedInt(String^ val);

		static long ParseLong(String^ val);

		static unsigned long ParseUnsignedLong(String^ val);

		static bool ParseBool(String^ val);

		static Mogre::Vector3 ParseVector3(String^ val);

		static Mogre::Matrix3^ ParseMatrix3(String^ val);

		static Mogre::Matrix4^ ParseMatrix4(String^ val);

		static Mogre::Quaternion ParseQuaternion(String^ val);

		static Mogre::ColourValue ParseColourValue(String^ val);

		static Mogre::StringVector^ ParseStringVector(String^ val);

		static bool IsNumber(String^ val);

		DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_PLAINWRAPPER(StringConverter);
	};
}