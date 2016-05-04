#pragma once

#include <gcroot.h>
#pragma managed(push, off)
#include "OgreStringInterface.h"
#pragma managed(pop)

namespace Mogre
{
	ref class StringInterface;


	public ref class ParamCommand abstract
	{
		Ogre::ParamCommand* _native;

	public:
		inline static operator Ogre::ParamCommand* ( ParamCommand^ obj )
		{
			return obj->_native;
		}

		ParamCommand();

		~ParamCommand()
		{
			if (_native)
			{
				delete _native;
				_native = 0;
			}
		}

        virtual String^ DoGet(IStringInterface^ target) abstract;
        virtual void DoSet(IStringInterface^ target, String^ val) abstract;
	};

	class ParamCommand_Proxy : public Ogre::ParamCommand
	{
		gcroot<Mogre::ParamCommand^> _managed;

	public:
		ParamCommand_Proxy( Mogre::ParamCommand^ managed ) : _managed(managed)
		{
		}

		virtual Ogre::String doGet(const void* target) const;
		virtual void doSet(void* target, const Ogre::String& val);
	};
}