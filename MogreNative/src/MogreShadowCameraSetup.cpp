#include "stdafx.h"
#include "MogreShadowCameraSetup.h"
#include "MogreSceneManager.h"
#include "MogreCamera.h"
#include "MogreLight.h"
#include "MogreViewport.h"

using namespace Mogre;

ShadowCameraSetup::~ShadowCameraSetup()
{
	this->!ShadowCameraSetup();
}

ShadowCameraSetup::!ShadowCameraSetup()
{
	OnDisposing(this, nullptr);

	if (IsDisposed)
		return;

	if (_createdByCLR && _native)
	{
		delete _native;
		_native = 0;
	}

	OnDisposed(this, nullptr);
}

void ShadowCameraSetup::GetShadowCamera(Mogre::SceneManager^ sm, Mogre::Camera^ cam, Mogre::Light^ light, Mogre::Camera^ texCam, size_t iteration)
{
	_native->getShadowCamera(sm, cam, light, texCam, iteration);
}

DefaultShadowCameraSetup::DefaultShadowCameraSetup() : ShadowCameraSetup((Ogre::ShadowCameraSetup*)0)
{
	_createdByCLR = true;
	_native = new Ogre::DefaultShadowCameraSetup();
}

void DefaultShadowCameraSetup::GetShadowCamera(Mogre::SceneManager^ sm, Mogre::Camera^ cam, Mogre::Light^ light, Mogre::Camera^ texCam, size_t iteration)
{
	static_cast<const Ogre::DefaultShadowCameraSetup*>(_native)->getShadowCamera(sm, cam, light, texCam, iteration);
}