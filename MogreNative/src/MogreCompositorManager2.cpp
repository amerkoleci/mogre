#include "stdafx.h"
#include "Marshalling.h"
#include "MogreCompositorManager2.h"
#include "MogreSceneManager.h"
#include "MogreRenderTarget.h"
#include "MogreCamera.h"

using namespace Mogre;

Ogre::CompositorWorkspace* CompositorWorkspace::UnmanagedPointer::get()
{
	return _native;
}

bool CompositorManager2::HasWorkspaceDefinition(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return _native->hasWorkspaceDefinition(o_name);
}

void CompositorManager2::CreateBasicWorkspaceDef(String^ name, Color4 backgroundColor)
{
	DECLARE_NATIVE_STRING(o_name, name);
	_native->createBasicWorkspaceDef(o_name, FromColor4(backgroundColor));
}


CompositorWorkspace^ CompositorManager2::AddWorkspace(SceneManager^ sceneManager, RenderTarget^ finalRenderTarget, Camera^ defaultCamera, String^ definitionName)
{
	DECLARE_NATIVE_STRING(o_definitionName, definitionName);
	auto workspace = _native->addWorkspace(
		GetPointerOrNull(sceneManager),
		GetPointerOrNull(finalRenderTarget),
		GetPointerOrNull(defaultCamera),
		o_definitionName,
		true);

	return ObjectTable::GetOrCreateObject<Mogre::CompositorWorkspace^>((intptr_t)workspace);
}

CompositorWorkspace^ CompositorManager2::AddWorkspace(SceneManager^ sceneManager, RenderTarget^ finalRenderTarget, Camera^ defaultCamera, String^ definitionName, bool enabled)
{
	DECLARE_NATIVE_STRING(o_definitionName, definitionName);
	auto workspace = _native->addWorkspace(
		GetPointerOrNull(sceneManager),
		GetPointerOrNull(finalRenderTarget),
		GetPointerOrNull(defaultCamera),
		o_definitionName,
		enabled);

	return ObjectTable::GetOrCreateObject<Mogre::CompositorWorkspace^>((intptr_t)workspace);
}

CompositorWorkspace^ CompositorManager2::AddWorkspace(SceneManager^ sceneManager, RenderTarget^ finalRenderTarget, Camera^ defaultCamera, String^ definitionName, bool enabled, int position)
{
	DECLARE_NATIVE_STRING(o_definitionName, definitionName);
	auto workspace = _native->addWorkspace(
		GetPointerOrNull(sceneManager),
		GetPointerOrNull(finalRenderTarget),
		GetPointerOrNull(defaultCamera),
		o_definitionName,
		enabled,
		position);

	return ObjectTable::GetOrCreateObject<Mogre::CompositorWorkspace^>((intptr_t)workspace);
}

void CompositorManager2::RemoveWorkspace(CompositorWorkspace^ workspace)
{
	_native->removeWorkspace(GetPointerOrNull(workspace));
}

void CompositorManager2::RemoveAllWorkspaces()
{
	_native->removeAllWorkspaces();
}

void CompositorManager2::RemoveAllWorkspaceDefinitions()
{
	_native->removeAllWorkspaceDefinitions();
}

Ogre::CompositorManager2* CompositorManager2::UnmanagedPointer::get()
{
	return _native;
}

