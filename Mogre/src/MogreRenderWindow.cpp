#include "stdafx.h"
#include "MogreRenderWindow.h"


using namespace Mogre;

bool RenderWindow::IsActive::get()
{
	return static_cast<const Ogre::RenderWindow*>(_native)->isActive();
}

void RenderWindow::IsActive::set(bool state)
{
	static_cast<Ogre::RenderWindow*>(_native)->setActive(state);
}

bool RenderWindow::IsClosed::get()
{
	return static_cast<const Ogre::RenderWindow*>(_native)->isClosed();
}

bool RenderWindow::IsFullScreen::get()
{
	return static_cast<const Ogre::RenderWindow*>(_native)->isFullScreen();
}

bool RenderWindow::IsPrimary::get()
{
	return static_cast<const Ogre::RenderWindow*>(_native)->isPrimary();
}

bool RenderWindow::IsVisible::get()
{
	return static_cast<const Ogre::RenderWindow*>(_native)->isVisible();
}

void RenderWindow::Create(String^ name, unsigned int width, unsigned int height, bool fullScreen, Mogre::Const_NameValuePairList^ miscParams)
{
	DECLARE_NATIVE_STRING(o_name, name);

	static_cast<Ogre::RenderWindow*>(_native)->create(o_name, width, height, fullScreen, miscParams);
}

void RenderWindow::SetFullscreen(bool fullScreen, unsigned int width, unsigned int height)
{
	static_cast<Ogre::RenderWindow*>(_native)->setFullscreen(fullScreen, width, height);
}

void RenderWindow::Destroy()
{
	static_cast<Ogre::RenderWindow*>(_native)->destroy();
}

void RenderWindow::Resize(unsigned int width, unsigned int height)
{
	static_cast<Ogre::RenderWindow*>(_native)->resize(width, height);
}

void RenderWindow::WindowMovedOrResized()
{
	static_cast<Ogre::RenderWindow*>(_native)->windowMovedOrResized();
}

void RenderWindow::Reposition(int left, int top)
{
	static_cast<Ogre::RenderWindow*>(_native)->reposition(left, top);
}

void RenderWindow::SetVisible(bool visible)
{
	static_cast<Ogre::RenderWindow*>(_native)->setVisible(visible);
}

void RenderWindow::SwapBuffers()
{
	static_cast<Ogre::RenderWindow*>(_native)->swapBuffers();
}

void RenderWindow::GetMetrics([Out] unsigned int% width, [Out] unsigned int% height, [Out] unsigned int% colourDepth, [Out] int% left, [Out] int% top)
{
	pin_ptr<unsigned int> p_width = &width;
	pin_ptr<unsigned int> p_height = &height;
	pin_ptr<unsigned int> p_colourDepth = &colourDepth;
	pin_ptr<int> p_left = &left;
	pin_ptr<int> p_top = &top;

	static_cast<Ogre::RenderWindow*>(_native)->getMetrics(*p_width, *p_height, *p_colourDepth, *p_left, *p_top);
}


Ogre::RenderWindow* RenderWindow::UnmanagedPointer::get()
{
	return static_cast<Ogre::RenderWindow*>(_native);
}