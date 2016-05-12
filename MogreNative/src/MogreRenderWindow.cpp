#include "stdafx.h"
#include "MogreRenderWindow.h"
#include "Marshalling.h"

using namespace Mogre;

Ogre::RenderWindow* RenderWindow::UnmanagedPointer::get()
{
	return static_cast<Ogre::RenderWindow*>(_native);
}