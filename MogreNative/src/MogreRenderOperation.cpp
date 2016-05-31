#include "stdafx.h"
#include "MogreRenderOperation.h"

using namespace Mogre;

RenderOperation::RenderOperation()
{
	_createdByCLR = true;
	_native = new Ogre::RenderOperation();
}