#pragma once

#include "IDisposable.h"
#include "Compositor/OgreCompositorManager2.h"
#include "Compositor/OgreCompositorWorkspace.h"
#include "MogreCommon.h"

namespace Mogre
{
	public ref class CompositorWorkspace
	{
	private:
		Ogre::CompositorWorkspace* _native;

	public protected:
		CompositorWorkspace(intptr_t ptr) : _native((Ogre::CompositorWorkspace*)ptr)
		{

		}
	};

	ref class SceneManager;
	ref class RenderTarget;
	ref class Camera;

	public ref class CompositorManager2
	{
	private:
		Ogre::CompositorManager2* _native;
		bool _createdByCLR;

	private:
		CompositorManager2(Ogre::CompositorManager2* obj) : _native(obj)
		{
		}

	public protected:
		CompositorManager2(intptr_t ptr) : _native((Ogre::CompositorManager2*)ptr)
		{

		}

	public:
		bool HasWorkspaceDefinition(String^ name);
		void CreateBasicWorkspaceDef(String^ name, Color4 backgroundColor);

		CompositorWorkspace^ AddWorkspace(SceneManager^ sceneManager, RenderTarget^ finalRenderTarget, Camera^ defaultCamera, String^ definitionName);
		CompositorWorkspace^ AddWorkspace(SceneManager^ sceneManager, RenderTarget^ finalRenderTarget, Camera^ defaultCamera, String^ definitionName, bool enabled);
		CompositorWorkspace^ AddWorkspace(SceneManager^ sceneManager, RenderTarget^ finalRenderTarget, Camera^ defaultCamera, String^ definitionName, bool enabled, int position);

	internal:
		property Ogre::CompositorManager2* UnmanagedPointer
		{
			Ogre::CompositorManager2* get();
		}
	};
}