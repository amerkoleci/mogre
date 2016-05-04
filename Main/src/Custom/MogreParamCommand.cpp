#include "MogreStableHeaders.h"

#include "Custom\MogreParamCommand.h"
#include "MogreStringInterface.h"

namespace Mogre
{
	ParamCommand::ParamCommand()
	{
		_native = new ParamCommand_Proxy( this );
	}

	Ogre::String ParamCommand_Proxy::doGet(const void* target) const
	{
		String^ str = _managed->DoGet( (IStringInterface^) static_cast<const Ogre::StringInterface*>( target ) );
		DECLARE_NATIVE_STRING( o_str, str )
		return o_str;
	}

	void ParamCommand_Proxy::doSet(void* target, const Ogre::String& val)
	{
		_managed->DoSet( (IStringInterface^) static_cast<Ogre::StringInterface*>( target ), TO_CLR_STRING( val ) );
	}
}