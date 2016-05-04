#pragma once

#if defined(_MANAGED)
#define BOOST_USE_WINDOWS_H
#endif

#include "Prerequisites.h"

#include "MakePublicDeclarations.h"
#pragma make_public( Ogre::Degree )
#pragma make_public( Ogre::Angle )


#include "PortedClasses.h"
