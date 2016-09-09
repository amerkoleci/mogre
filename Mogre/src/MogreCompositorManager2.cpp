#include "stdafx.h"
#include "Marshalling.h"
#include "MogreCompositorManager2.h"
#include "MogreSceneManager.h"
#include "MogreRenderTarget.h"
#include "MogreCamera.h"
#include "MogreRenderSystem.h"

using namespace Mogre;

// CompositorTargetDef
CompositorTargetDef::CompositorTargetDef(Ogre::CompositorTargetDef* obj) : _native(obj)
{
	_compositorPasses = gcnew System::Collections::Generic::List<CompositorPassDef^>();
	Ogre::CompositorPassDefVec &passes = _native->getCompositorPassesNonConst();
	for (size_t i = 0; i < passes.size(); ++i)
	{
		auto passSceneDef = dynamic_cast<Ogre::CompositorPassSceneDef*>(passes[i]);
		if (passSceneDef)
		{
			_compositorPasses->Add(gcnew CompositorPassSceneDef(passSceneDef));
		}
		else
		{
			auto managed = gcnew Mogre::CompositorPassDef(passes[i]);
			_compositorPasses->Add(managed);
		}
	}
}

CompositorPassDef^ CompositorTargetDef::GetCompositorPass(int index)
{
	return _compositorPasses[index];
}

System::Collections::Generic::IEnumerable<CompositorPassDef^>^ CompositorTargetDef::CompositorPasses::get()
{
	return _compositorPasses;
}


Ogre::uint32 CompositorTargetDef::RenderTargetName::get()
{
	return _native->getRenderTargetName().mHash;
}

// CompositorPassSceneDef
Ogre::uint32 CompositorPassSceneDef::VisibilityMask::get()
{
	return static_cast<Ogre::CompositorPassSceneDef*>(_native)->mVisibilityMask;
}

void CompositorPassSceneDef::VisibilityMask::set(Ogre::uint32 value)
{
	static_cast<Ogre::CompositorPassSceneDef*>(_native)->setVisibilityMask(value);
}

Ogre::uint32 CompositorPassSceneDef::ShadowNode::get()
{
	return static_cast<Ogre::CompositorPassSceneDef*>(_native)->mShadowNode.mHash;
}

void CompositorPassSceneDef::ShadowNodeName::set(String^ value)
{
	DECLARE_NATIVE_STRING(o_value, value);
	static_cast<Ogre::CompositorPassSceneDef*>(_native)->mShadowNode = o_value;
}

// CompositorNodeDef
CompositorNodeDef::CompositorNodeDef(Ogre::CompositorNodeDef* obj) : TextureDefinitionBase(obj)
{
	_targetPasses = gcnew System::Collections::Generic::List<CompositorTargetDef^>();
	for (size_t i = 0; i < obj->getNumTargetPasses(); ++i)
	{
		auto managed = gcnew Mogre::CompositorTargetDef(obj->getTargetPass(i));
		_targetPasses->Add(managed);
	}
}

CompositorTargetDef^ CompositorNodeDef::GetTargetPass(int index)
{
	return _targetPasses[index];
}

int CompositorNodeDef::TargetPassesCount::get()
{
	return _targetPasses->Count;
}

System::Collections::Generic::IEnumerable<CompositorTargetDef^>^ CompositorNodeDef::TargetPasses::get()
{
	return _targetPasses;
}

// CompositorPass
CompositorPass::~CompositorPass()
{
	this->!CompositorPass();
}

CompositorPass::!CompositorPass()
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

void CompositorPass::Execute(Camera^ lodCamera)
{
	_native->execute(lodCamera);
}

Mogre::CompositorPassDef^ CompositorPass::Definition::get()
{
	auto nativeDef = const_cast<Ogre::CompositorPassDef*>(_native->getDefinition());
	ReturnCachedObjectGcnew(Mogre::CompositorPassDef, _definition, nativeDef);
}

// TextureDefinitionBase
TextureDefinitionBase::~TextureDefinitionBase()
{
	this->!TextureDefinitionBase();
}

TextureDefinitionBase::!TextureDefinitionBase()
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

// CompositorWorkspaceDef
void CompositorWorkspaceDef::Connect(String^ outNode, Ogre::uint32 outChannel, String^ inNode, Ogre::uint32 inChannel)
{
	DECLARE_NATIVE_STRING(o_outNode, outNode);
	DECLARE_NATIVE_STRING(o_inNode, inNode);

	static_cast<Ogre::CompositorWorkspaceDef*>(_native)->connect(o_outNode, outChannel, o_inNode, inChannel);
}

void CompositorWorkspaceDef::Connect(String^ outNode, String^ inNode)
{
	DECLARE_NATIVE_STRING(o_outNode, outNode);
	DECLARE_NATIVE_STRING(o_inNode, inNode);

	static_cast<Ogre::CompositorWorkspaceDef*>(_native)->connect(o_outNode, o_inNode);
}

void CompositorWorkspaceDef::ConnectOutput(String^ inNode, Ogre::uint32 inChannel)
{
	DECLARE_NATIVE_STRING(o_inNode, inNode);

	static_cast<Ogre::CompositorWorkspaceDef*>(_native)->connectOutput(o_inNode, inChannel);
}

void CompositorWorkspaceDef::ClearAllInterNodeConnections()
{
	static_cast<Ogre::CompositorWorkspaceDef*>(_native)->clearAllInterNodeConnections();
}

void CompositorWorkspaceDef::ClearOutputConnections()
{
	static_cast<Ogre::CompositorWorkspaceDef*>(_native)->clearOutputConnections();
}

void CompositorWorkspaceDef::ClearAll()
{
	static_cast<Ogre::CompositorWorkspaceDef*>(_native)->clearAll();
}

void CompositorWorkspaceDef::AddNodeAlias(String^ alias, String^ nodeName)
{
	DECLARE_NATIVE_STRING(o_alias, alias);
	DECLARE_NATIVE_STRING(o_nodeName, nodeName);

	static_cast<Ogre::CompositorWorkspaceDef*>(_native)->addNodeAlias(o_alias, o_nodeName);
}

void CompositorWorkspaceDef::RemoveNodeAlias(String^ alias)
{
	DECLARE_NATIVE_STRING(o_alias, alias);
	static_cast<Ogre::CompositorWorkspaceDef*>(_native)->removeNodeAlias(o_alias);
}

// CompositorWorkspaceListener
class CompositorWorkspaceListener_Proxy : public Ogre::CompositorWorkspaceListener
{
public:
	friend ref class Mogre::CompositorWorkspaceListener;

	gcroot<Mogre::CompositorWorkspaceListener^> _managed;

	CompositorWorkspaceListener_Proxy(Mogre::CompositorWorkspaceListener^ managedObj) : Ogre::CompositorWorkspaceListener()
		, _managed(managedObj)
	{
	}

	virtual void workspacePreUpdate(Ogre::CompositorWorkspace *workspace) override;
	virtual void passPreExecute(Ogre::CompositorPass *pass) override;
};

Ogre::CompositorWorkspaceListener* CompositorWorkspaceListener::_IListener_GetNativePtr()
{
	return static_cast<Ogre::CompositorWorkspaceListener*>(static_cast<CompositorWorkspaceListener_Proxy*>(_native));
}

void CompositorWorkspaceListener_Proxy::workspacePreUpdate(Ogre::CompositorWorkspace *workspace)
{
	_managed->WorkspacePreUpdate(workspace);
}

void CompositorWorkspaceListener_Proxy::passPreExecute(Ogre::CompositorPass *pass)
{
	_managed->PassPreExecute(pass);
}

CompositorWorkspaceListener::CompositorWorkspaceListener()
{
	_createdByCLR = true;
	//Type^ thisType = this->GetType();
	CompositorWorkspaceListener_Proxy* proxy = new CompositorWorkspaceListener_Proxy(this);
	_native = proxy;
}

CompositorWorkspaceListener::~CompositorWorkspaceListener()
{
	this->!CompositorWorkspaceListener();
}

CompositorWorkspaceListener::!CompositorWorkspaceListener()
{
	OnDisposing(this, nullptr);

	if (IsDisposed)
		return;

	if (_createdByCLR && _native != 0)
	{
		delete _native;
		_native = 0;
	}

	OnDisposed(this, nullptr);
}

void CompositorWorkspaceListener::WorkspacePreUpdate(Mogre::CompositorWorkspace^ workspace)
{
}

void CompositorWorkspaceListener::PassPreExecute(Mogre::CompositorPass^ pass)
{
}

// CompositorWorkspace
CompositorWorkspace::~CompositorWorkspace()
{
	this->!CompositorWorkspace();
}

CompositorWorkspace::!CompositorWorkspace()
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

bool CompositorWorkspace::Enabled::get()
{
	return _native->getEnabled();
}

void CompositorWorkspace::Enabled::set(bool enabled)
{
	_native->setEnabled(enabled);
}

void CompositorWorkspace::SetListener(Mogre::ICompositorWorkspaceListener^ listener)
{
	_listener = listener;
	_native->setListener(listener->_GetNativePtr());
}

Mogre::ICompositorWorkspaceListener^ CompositorWorkspace::GetListener()
{
	return _listener;
}

void CompositorWorkspace::RecreateAllNodes()
{
	_native->recreateAllNodes();
}

void CompositorWorkspace::ReconnectAllNodes()
{
	_native->reconnectAllNodes();
}

void CompositorWorkspace::_beginUpdate(bool forceBeginFrame)
{
	_native->_beginUpdate(forceBeginFrame);
}

void CompositorWorkspace::_update()
{
	_native->_update();
}

void CompositorWorkspace::_endUpdate(bool forceEndFrame)
{
	_native->_endUpdate(forceEndFrame);
}

void CompositorWorkspace::_swapFinalTarget()
{
	_native->_swapFinalTarget();
}

void CompositorWorkspace::_validateFinalTarget()
{
	_native->_validateFinalTarget();
}

Ogre::CompositorWorkspace* CompositorWorkspace::UnmanagedPointer::get()
{
	return _native;
}

// CompositorWorkspace
CompositorManager2::CompositorManager2(RenderSystem^ renderSystem)
{
	_createdByCLR = true;
	_native = new Ogre::CompositorManager2(renderSystem);
}

CompositorManager2::~CompositorManager2()
{
	this->!CompositorManager2();
}

CompositorManager2::!CompositorManager2()
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

bool CompositorManager2::HasNodeDefinition(String^ nodeDefName)
{
	DECLARE_NATIVE_STRING(o_nodeDefName, nodeDefName);

	return _native->hasNodeDefinition(o_nodeDefName);
}

CompositorNodeDef^ CompositorManager2::AddNodeDefinition(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);
	auto result = gcnew CompositorNodeDef(_native->addNodeDefinition(o_name));
	_nodeDefinitions->Add(name, result);
	return result;
}

CompositorNodeDef^ CompositorManager2::GetNodeDefinition(String^ name)
{
	CompositorNodeDef^ result;
	if (!_nodeDefinitions->TryGetValue(name, result))
	{
		DECLARE_NATIVE_STRING(o_name, name);
		auto nativeDefinition = _native->getNodeDefinitionNonConst(o_name);
		if (!nativeDefinition)
			return nullptr;

		result = gcnew CompositorNodeDef(nativeDefinition);
		_nodeDefinitions->Add(name, result);
	}

	return result;
}

bool CompositorManager2::HasWorkspaceDefinition(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return _native->hasWorkspaceDefinition(o_name);
}

CompositorWorkspaceDef^ CompositorManager2::GetWorkspaceDefinition(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return _native->getWorkspaceDefinition(o_name);
}

CompositorWorkspaceDef^ CompositorManager2::AddWorkspaceDefinition(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return _native->addWorkspaceDefinition(o_name);
}

void CompositorManager2::CreateBasicWorkspaceDef(String^ name, ColourValue backgroundColor)
{
	DECLARE_NATIVE_STRING(o_name, name);
	_native->createBasicWorkspaceDef(o_name, FromColor4(backgroundColor));
}


CompositorWorkspace^ CompositorManager2::AddWorkspace(SceneManager^ sceneManager, RenderTarget^ finalRenderTarget, Camera^ defaultCamera, String^ definitionName)
{
	DECLARE_NATIVE_STRING(o_definitionName, definitionName);
	auto workspace = _native->addWorkspace(
		sceneManager,
		finalRenderTarget,
		defaultCamera,
		o_definitionName,
		true);

	return gcnew Mogre::CompositorWorkspace(workspace);
}

CompositorWorkspace^ CompositorManager2::AddWorkspace(SceneManager^ sceneManager, RenderTarget^ finalRenderTarget, Camera^ defaultCamera, String^ definitionName, bool enabled)
{
	DECLARE_NATIVE_STRING(o_definitionName, definitionName);
	auto workspace = _native->addWorkspace(
		sceneManager,
		finalRenderTarget,
		defaultCamera,
		o_definitionName,
		enabled);

	return gcnew Mogre::CompositorWorkspace(workspace);
}

CompositorWorkspace^ CompositorManager2::AddWorkspace(SceneManager^ sceneManager, RenderTarget^ finalRenderTarget, Camera^ defaultCamera, String^ definitionName, bool enabled, int position)
{
	DECLARE_NATIVE_STRING(o_definitionName, definitionName);
	auto workspace = _native->addWorkspace(
		sceneManager,
		finalRenderTarget,
		defaultCamera,
		o_definitionName,
		enabled,
		position);

	return gcnew Mogre::CompositorWorkspace(workspace);
}

CompositorWorkspace^ CompositorManager2::AddWorkspace(SceneManager^ sceneManager, CompositorChannel^ finalRenderTarget, Camera^ defaultCamera, String^ definitionName)
{
	DECLARE_NATIVE_STRING(o_definitionName, definitionName);

	Ogre::CompositorChannel nativeChannel;
	nativeChannel.target = finalRenderTarget->target;
	nativeChannel.textures.resize(finalRenderTarget->textures->Count);
	for (auto i = 0; i < finalRenderTarget->textures->Count; ++i)
	{
		nativeChannel.textures.push_back(*finalRenderTarget->textures[i]->UnmanagedPointer);
	}

	auto workspace = _native->addWorkspace(
		sceneManager,
		nativeChannel,
		defaultCamera,
		o_definitionName,
		true);

	return gcnew Mogre::CompositorWorkspace(workspace);
}

CompositorWorkspace^ CompositorManager2::AddWorkspace(SceneManager^ sceneManager, CompositorChannel^ finalRenderTarget, Camera^ defaultCamera, String^ definitionName, bool enabled)
{
	DECLARE_NATIVE_STRING(o_definitionName, definitionName);

	Ogre::CompositorChannel nativeChannel;
	nativeChannel.target = finalRenderTarget->target;
	nativeChannel.textures.resize(finalRenderTarget->textures->Count);
	for (auto i = 0; i < finalRenderTarget->textures->Count; ++i)
	{
		nativeChannel.textures.push_back(*finalRenderTarget->textures[i]->UnmanagedPointer);
	}

	auto workspace = _native->addWorkspace(
		sceneManager,
		nativeChannel,
		defaultCamera,
		o_definitionName,
		enabled);

	return gcnew Mogre::CompositorWorkspace(workspace);
}

CompositorWorkspace^ CompositorManager2::AddWorkspace(SceneManager^ sceneManager, CompositorChannel^ finalRenderTarget, Camera^ defaultCamera, String^ definitionName, bool enabled, int position)
{
	DECLARE_NATIVE_STRING(o_definitionName, definitionName);

	Ogre::CompositorChannel nativeChannel;
	nativeChannel.target = finalRenderTarget->target;
	nativeChannel.textures.resize(finalRenderTarget->textures->Count);
	for (auto i = 0; i < finalRenderTarget->textures->Count; ++i)
	{
		nativeChannel.textures.push_back(*finalRenderTarget->textures[i]->UnmanagedPointer);
	}

	auto workspace = _native->addWorkspace(
		sceneManager,
		nativeChannel,
		defaultCamera,
		o_definitionName,
		enabled,
		position);

	return gcnew Mogre::CompositorWorkspace(workspace);
}

void CompositorManager2::RemoveWorkspace(CompositorWorkspace^ workspace)
{
	_native->removeWorkspace(workspace);
}

void CompositorManager2::RemoveAllWorkspaces()
{
	_native->removeAllWorkspaces();
}

void CompositorManager2::RemoveAllWorkspaceDefinitions()
{
	_native->removeAllWorkspaceDefinitions();
}

void CompositorManager2::RemoveAllShadowNodeDefinitions()
{
	_native->removeAllShadowNodeDefinitions();
}

void CompositorManager2::RemoveAllNodeDefinitions()
{
	_native->removeAllNodeDefinitions();
	_nodeDefinitions->Clear();
}

Ogre::CompositorManager2* CompositorManager2::UnmanagedPointer::get()
{
	return _native;
}

