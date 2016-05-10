#include "stdafx.h"
#include "Compositor/OgreCompositorManager2.h"

extern "C"
{
	MOGRE_EXPORTS_API bool CompositorManager2_hasWorkspaceDefinition(Ogre::CompositorManager2* _this, const char* name)
	{
		return _this->hasWorkspaceDefinition(name);
	}

	MOGRE_EXPORTS_API void CompositorManager2_createBasicWorkspaceDef(Ogre::CompositorManager2* _this, const char* name, const Ogre::ColourValue &backgroundColour)
	{
		_this->createBasicWorkspaceDef(name, backgroundColour, Ogre::IdString());
	}

	MOGRE_EXPORTS_API Ogre::CompositorWorkspace* CompositorManager2_addWorkspace(
		Ogre::CompositorManager2* _this,
		Ogre::SceneManager* sceneManager,
		Ogre::RenderTarget* finalRenderTarget,
		Ogre::Camera* defaultCamera,
		const char* name,
		bool enabled)
	{
		return _this->addWorkspace(sceneManager, finalRenderTarget, defaultCamera, name, enabled);
	}
}