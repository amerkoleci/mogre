#pragma once

#include "OgreException.h"
#include "MogreCommon.h"
#include "STLContainerWrappers.h"
#include "IteratorWrapper.h"
#include "Marshalling.h"

namespace Mogre
{
	public ref class OgreException
	{
	public:
		enum class ExceptionCodes
		{
			ERR_CANNOT_WRITE_TO_FILE = Ogre::Exception::ERR_CANNOT_WRITE_TO_FILE,
			ERR_INVALID_STATE = Ogre::Exception::ERR_INVALID_STATE,
			ERR_INVALIDPARAMS = Ogre::Exception::ERR_INVALIDPARAMS,
			ERR_RENDERINGAPI_ERROR = Ogre::Exception::ERR_RENDERINGAPI_ERROR,
			ERR_DUPLICATE_ITEM = Ogre::Exception::ERR_DUPLICATE_ITEM,
			ERR_ITEM_NOT_FOUND = Ogre::Exception::ERR_ITEM_NOT_FOUND,
			ERR_FILE_NOT_FOUND = Ogre::Exception::ERR_FILE_NOT_FOUND,
			ERR_INTERNAL_ERROR = Ogre::Exception::ERR_INTERNAL_ERROR,
			ERR_RT_ASSERTION_FAILED = Ogre::Exception::ERR_RT_ASSERTION_FAILED,
			ERR_NOT_IMPLEMENTED = Ogre::Exception::ERR_NOT_IMPLEMENTED
		};

		//Internal Declarations
	public protected:
		OgreException(Ogre::Exception* obj) : _native(obj), _createdByCLR(false)
		{
		}

		~OgreException()
		{
			this->!OgreException();
		}
		!OgreException()
		{
			if (_createdByCLR &&_native)
			{
				delete _native;
				_native = 0;
			}
		}

		Ogre::Exception* _native;
		bool _createdByCLR;


		//Public Declarations
	public:
		OgreException(int number, String^ description, String^ source);
		OgreException(int number, String^ description, String^ source, const char* type, const char* file, long line);
		OgreException(Mogre::OgreException^ rhs);


		property String^ Description
		{
		public:
			String^ get();
		}

		property String^ File
		{
		public:
			String^ get();
		}

		property String^ FullDescription
		{
		public:
			String^ get();
		}

		property bool IsThrown
		{
		public:
			static bool get();
		}

		property Mogre::OgreException^ LastException
		{
		public:
			static Mogre::OgreException^ get();
		}

		property long Line
		{
		public:
			long get();
		}

		property Mogre::OgreException::ExceptionCodes Number
		{
		public:
			Mogre::OgreException::ExceptionCodes get();
		}

		property String^ Source
		{
		public:
			String^ get();
		}

		void CopyTo(OgreException^ dest)
		{
			if (_native == NULL) throw gcnew Exception("The underlying native object for the caller is null.");
			if (dest->_native == NULL) throw gcnew ArgumentException("The underlying native object for parameter 'dest' is null.");

			*(dest->_native) = *_native;
		}

		String^ What();

		DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_PLAINWRAPPER_EXPLICIT(OgreException, Exception);
	};
}