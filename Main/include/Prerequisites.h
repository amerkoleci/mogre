#pragma once

// Compiler has a bug that emits this warning when "protected public" access modifier
// is used for forward class declarations
#pragma warning( disable : 4240 )

// Disable "overflow in constant arithmetic" warning
#pragma warning( disable : 4756 )

#define STATIC_ASSERT(A) typedef int __assertchecker##__COUNTER__[(A) ? +1 : -1];

#include "CLRHelp.h"
#include "Marshalling.h"
#include "Wrapper.h"
#include "STLContainerWrappers.h"
#include "SubclassingManager.h"
#include "MogrePrerequisites.h"
#include "MogrePlatform.h"
#include "Custom\MogreIteratorWrapper.h"

namespace Mogre
{
	using namespace System::Collections;
	using namespace System::Collections::Specialized;
	using namespace Mogre::Implementation;

	typedef System::String String;

	#include "PreDeclarations.h"
}