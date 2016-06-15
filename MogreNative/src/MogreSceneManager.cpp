#include "stdafx.h"
#include "MogreSceneManager.h"
#include "MogreSceneNode.h"
#include "MogreEntity.h"
#include "MogreAnimation.h"
#include "MogreMeshManager.h"
#include "MogreSceneQuery.h"
#include "MogreMovableObject.h"
#include "MogreMaterialManager.h"
#include "MogreViewport.h"

using namespace Mogre;

void RenderQueueListener_Director::preRenderQueues()
{
	// TODO
}

void RenderQueueListener_Director::postRenderQueues()
{
	// TODO
}

void RenderQueueListener_Director::renderQueueStarted(Ogre::RenderQueue *rq, Ogre::uint8 queueGroupId, const Ogre::String& invocation, bool& skipThisInvocation)
{
	(void)rq;
	if (doCallForRenderQueueStarted)
	{
		_receiver->RenderQueueStarted(queueGroupId, TO_CLR_STRING(invocation), skipThisInvocation);
	}
}

void RenderQueueListener_Director::renderQueueEnded(Ogre::uint8 queueGroupId, const Ogre::String& invocation, bool& repeatThisInvocation)
{
	if (doCallForRenderQueueEnded)
	{
		_receiver->RenderQueueEnded(queueGroupId, TO_CLR_STRING(invocation), repeatThisInvocation);
	}
}

CPP_DECLARE_STLVECTOR(SceneManager::, MovableObjectVec, Mogre::MovableObject^, Ogre::MovableObject*);
CPP_DECLARE_ITERATOR(SceneManager::, MovableObjectIterator, Ogre::SceneManager::MovableObjectIterator, Mogre::SceneManager::MovableObjectVec, Mogre::MovableObject^, Ogre::MovableObject*, );

CPP_DECLARE_STLVECTOR(SceneManager::, CameraList, Mogre::Camera^, Ogre::Camera*);
CPP_DECLARE_ITERATOR_NOCONSTRUCTOR(SceneManager::, CameraIterator, Ogre::SceneManager::CameraIterator, Mogre::SceneManager::CameraList, Mogre::Camera^, Ogre::Camera*);


SceneManager::~SceneManager()
{
	this->!SceneManager();
}

SceneManager::!SceneManager()
{
	OnDisposing(this, nullptr);

	if (IsDisposed)
		return;

	if (_renderQueueListener != 0)
	{
		if (_native != 0) static_cast<Ogre::SceneManager*>(_native)->removeRenderQueueListener(_renderQueueListener);
		delete _renderQueueListener; _renderQueueListener = 0;
	}
	/*if (_shadowListener != 0)
	{
		if (_native != 0) static_cast<Ogre::SceneManager*>(_native)->removeShadowListener(_shadowListener);
		delete _shadowListener; _shadowListener = 0;
	}*/

	if (_createdByCLR && _native) { delete _native; _native = 0; }

	OnDisposed(this, nullptr);
}

bool SceneManager::IsDisposed::get()
{
	return (_native == nullptr);
}

String^ SceneManager::Name::get()
{
	return TO_CLR_STRING(_native->getName());
}

String^ SceneManager::TypeName::get()
{
	return TO_CLR_STRING(_native->getTypeName());
}

Mogre::ColourValue SceneManager::AmbientLight::get()
{
	return ToColor4(_native->getAmbientLight());
}

void SceneManager::AmbientLight::set(Mogre::ColourValue value)
{
	_native->setAmbientLight(FromColor4(value));
}

Mogre::Viewport^ SceneManager::CurrentViewport::get()
{
	return _native->getCurrentViewport();
}

Mogre::Camera^ SceneManager::CameraInProgress::get()
{
	return _native->getCameraInProgress();
}

bool SceneManager::DisplaySceneNodes::get()
{
	return _native->getDisplaySceneNodes();
}

void SceneManager::DisplaySceneNodes::set(bool value)
{
	_native->setDisplaySceneNodes(value);
}

Mogre::SceneNode^ SceneManager::GetRootSceneNode()
{
	ReturnCachedObjectGcnew(Mogre::SceneNode, _sceneRootDynamic, _native->getRootSceneNode());
}

Mogre::SceneNode^ SceneManager::GetRootSceneNode(SceneMemoryMgrTypes sceneType)
{
	if (sceneType == SceneMemoryMgrTypes::Dynamic)
		ReturnCachedObjectGcnew(Mogre::SceneNode, _sceneRootDynamic, _native->getRootSceneNode(Ogre::SCENE_DYNAMIC));

	ReturnCachedObjectGcnew(Mogre::SceneNode, _sceneRootStatic, _native->getRootSceneNode(Ogre::SCENE_STATIC));
}


Mogre::SceneNode^ SceneManager::CreateSceneNode()
{
	return gcnew Mogre::SceneNode(_native->createSceneNode());
}

Mogre::SceneNode^ SceneManager::CreateSceneNode(SceneMemoryMgrTypes sceneType)
{
	return gcnew Mogre::SceneNode(_native->createSceneNode((Ogre::SceneMemoryMgrTypes)sceneType));
}

Mogre::SceneNode^ SceneManager::CreateSceneNode(String^ name)
{
	Mogre::SceneNode^ result = gcnew Mogre::SceneNode(_native->createSceneNode());
	result->Name = name;
	_sceneNodes[name] = result;
	return result;
}

Mogre::SceneNode^ SceneManager::CreateSceneNode(String^ name, SceneMemoryMgrTypes sceneType)
{
	Mogre::SceneNode^ result = gcnew Mogre::SceneNode(_native->createSceneNode((Ogre::SceneMemoryMgrTypes)sceneType));
	result->Name = name;
	_sceneNodes[name] = result;
	return result;
}

void SceneManager::DestroySceneNode(Mogre::SceneNode^ node)
{
	_native->destroySceneNode(node);
}

void SceneManager::DestroySceneNode(String^ name)
{
	SceneNode^ node;
	if (_sceneNodes->TryGetValue(name, node))
	{
		_native->destroySceneNode(node);
	}
}

Mogre::SceneNode^ SceneManager::GetSceneNode(Ogre::IdType id)
{
	return _native->getSceneNode(id);
}

Mogre::SceneNode^ SceneManager::GetSceneNode(String^ name)
{
	if (!_sceneNodes->ContainsKey(name))
		throw gcnew ArgumentException(String::Format("SceneNode with '{0}' name not found", name));

	return _sceneNodes[name];
}

bool SceneManager::HasSceneNode(String^ name)
{
	return _sceneNodes->ContainsKey(name);
}

Mogre::BillboardSet^ SceneManager::CreateBillboardSet(unsigned int poolSize)
{
	return gcnew Mogre::BillboardSet(_native->createBillboardSet(poolSize));
}

void SceneManager::DestroyBillboardSet(Mogre::BillboardSet^ set)
{
	_native->destroyBillboardSet(set);
}

void SceneManager::DestroyAllBillboardSets()
{
	_native->destroyAllBillboardSets();
}

Mogre::BillboardChain^ SceneManager::CreateBillboardChain()
{
	return gcnew Mogre::BillboardChain(_native->createBillboardChain());
}

void SceneManager::DestroyBillboardChain(Mogre::BillboardChain^ obj)
{
	_native->destroyBillboardChain(obj);
}

void SceneManager::DestroyAllBillboardChains()
{
	_native->destroyAllBillboardChains();
}

Mogre::ManualObject^ SceneManager::CreateManualObject(SceneMemoryMgrTypes sceneType)
{
	return gcnew Mogre::ManualObject(_native->createManualObject((Ogre::SceneMemoryMgrTypes)sceneType));
}

Mogre::ManualObject^ SceneManager::CreateManualObject()
{
	return gcnew Mogre::ManualObject(_native->createManualObject());
}

Mogre::ManualObject^ SceneManager::CreateManualObject(String^ name)
{
	Mogre::ManualObject^ result = gcnew Mogre::ManualObject(_native->createManualObject());
	result->Name = name;
	if (!String::IsNullOrEmpty(name))
	{
		_manualObjects[name] = result;
	}
	return result;
}

Mogre::ManualObject^ SceneManager::CreateManualObject(String^ name, SceneMemoryMgrTypes sceneType)
{
	Mogre::ManualObject^ result = gcnew Mogre::ManualObject(_native->createManualObject((Ogre::SceneMemoryMgrTypes)sceneType));
	result->Name = name;
	if (!String::IsNullOrEmpty(name))
	{
		_manualObjects[name] = result;
	}
	return result;
}

void SceneManager::DestroyManualObject(Mogre::ManualObject^ obj)
{
	_native->destroyManualObject(obj);
}

void SceneManager::DestroyAllManualObjects()
{
	_native->destroyAllManualObjects();
}

Mogre::ManualObject^ SceneManager::GetManualObject(String^ name)
{
	if (!_manualObjects->ContainsKey(name))
		throw gcnew ArgumentException(String::Format("ManualObject with '{0}' name not found", name));

	return _manualObjects[name];
}

bool SceneManager::HasManualObject(String^ name)
{
	return _manualObjects->ContainsKey(name);
}

void SceneManager::DestroyManualObject(String^ name)
{
	ManualObject^ manualObject;
	if (_manualObjects->TryGetValue(name, manualObject))
	{
		_native->destroyManualObject(manualObject);
	}
}

Mogre::RibbonTrail^ SceneManager::CreateRibbonTrail()
{
	return gcnew Mogre::RibbonTrail(_native->createRibbonTrail());
}

void SceneManager::DestroyRibbonTrail(Mogre::RibbonTrail^ obj)
{
	_native->destroyRibbonTrail(obj);
}

void SceneManager::DestroyAllRibbonTrails()
{
	_native->destroyAllRibbonTrails();
}

Mogre::ParticleSystem^ SceneManager::CreateParticleSystem(String^ templateName)
{
	DECLARE_NATIVE_STRING(o_templateName, templateName);

	return gcnew Mogre::ParticleSystem(_native->createParticleSystem(o_templateName));
}

Mogre::ParticleSystem^ SceneManager::CreateParticleSystem(size_t quota, String^ resourceGroup)
{
	DECLARE_NATIVE_STRING(o_resourceGroup, resourceGroup);

	return gcnew Mogre::ParticleSystem(_native->createParticleSystem(quota, o_resourceGroup));
}

Mogre::ParticleSystem^ SceneManager::CreateParticleSystem(size_t quota)
{
	return gcnew Mogre::ParticleSystem(_native->createParticleSystem(quota));
}

void SceneManager::DestroyParticleSystem(Mogre::ParticleSystem^ obj)
{
	_native->destroyParticleSystem(obj);
}

void SceneManager::DestroyAllParticleSystems()
{
	static_cast<Ogre::SceneManager*>(_native)->destroyAllParticleSystems();
}

Mogre::Light^ SceneManager::CreateLight()
{
	return gcnew Mogre::Light(_native->createLight());
}

void SceneManager::DestroyLight(Mogre::Light^ light)
{
	static_cast<Ogre::SceneManager*>(_native)->destroyLight(light);
}

void SceneManager::DestroyAllLights()
{
	_native->destroyAllLights();
}

Mogre::Camera^ SceneManager::CreateCamera(String^ name)
{
	return CreateCamera(name, true, false);
}

Mogre::Camera^ SceneManager::CreateCamera(String^ name, bool notShadowCaster)
{
	return CreateCamera(name, notShadowCaster, false);
}

Mogre::Camera^ SceneManager::CreateCamera(String^ name, bool notShadowCaster, bool forCubemapping)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return gcnew Mogre::Camera(_native->createCamera(o_name, notShadowCaster, forCubemapping));
}

Mogre::Camera^ SceneManager::FindCamera(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return ObjectTable::GetOrCreateObject<Mogre::Camera^>((IntPtr)_native->findCamera(o_name));
}

Mogre::Camera^ SceneManager::FindCameraNoThrow(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return ObjectTable::GetOrCreateObject<Mogre::Camera^>((IntPtr)_native->findCameraNoThrow(o_name));
}

Mogre::Camera^ SceneManager::GetCamera(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return ObjectTable::GetOrCreateObject<Mogre::Camera^>((IntPtr)_native->findCamera(o_name));
}

bool SceneManager::HasCamera(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return _native->findCameraNoThrow(o_name) != nullptr;
}

void SceneManager::DestroyCamera(Mogre::Camera^ camera)
{
	_native->destroyCamera(camera);
}

void SceneManager::DestroyCamera(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);
	Ogre::Camera* camera = _native->findCameraNoThrow(o_name);
	if (camera)
	{
		_native->destroyCamera(camera);
	}
}

void SceneManager::DestroyAllCameras()
{
	_native->destroyAllCameras();
}

Mogre::Entity^ SceneManager::CreateEntity(String^ meshName)
{
	DECLARE_NATIVE_STRING(o_meshName, meshName);

	return gcnew Mogre::Entity(_native->createEntity(o_meshName));
}

Mogre::Entity^ SceneManager::CreateEntity(String^ meshName, String^ groupName, SceneMemoryMgrTypes sceneType)
{
	DECLARE_NATIVE_STRING(o_meshName, meshName);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return gcnew Mogre::Entity(_native->createEntity(o_meshName, o_groupName, (Ogre::SceneMemoryMgrTypes)sceneType));
}

Mogre::Entity^ SceneManager::CreateEntity(Mogre::SceneManager::PrefabType ptype)
{
	return gcnew Mogre::Entity(_native->createEntity((Ogre::SceneManager::PrefabType)ptype));
}

Mogre::Entity^ SceneManager::CreateEntity(Mogre::SceneManager::PrefabType ptype, SceneMemoryMgrTypes sceneType)
{
	return gcnew Mogre::Entity(_native->createEntity((Ogre::SceneManager::PrefabType)ptype, (Ogre::SceneMemoryMgrTypes)sceneType));
}

Mogre::Entity^ SceneManager::CreateEntity(MeshPtr^ mesh, SceneMemoryMgrTypes sceneType)
{
	return gcnew Mogre::Entity(_native->createEntity(mesh, (Ogre::SceneMemoryMgrTypes)sceneType));
}

Mogre::Entity^ SceneManager::CreateEntity(MeshPtr^ mesh)
{
	return _native->createEntity(mesh);
}

Mogre::Entity^ SceneManager::CreateEntity(String^ name, String^ meshName)
{
	DECLARE_NATIVE_STRING(o_meshName, meshName);
	Mogre::Entity^ result = gcnew Mogre::Entity(_native->createEntity(o_meshName));
	result->Name = name;
	if (!String::IsNullOrEmpty(name))
	{
		_entities[name] = result;
	}
	return result;
}

Mogre::Entity^ SceneManager::CreateEntity(String^ name, String^ meshName, String^ groupName)
{
	DECLARE_NATIVE_STRING(o_meshName, meshName);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	Mogre::Entity^ result = gcnew Mogre::Entity(_native->createEntity(o_meshName, o_groupName));
	result->Name = name;
	if (!String::IsNullOrEmpty(name))
	{
		_entities[name] = result;
	}
	return result;
}

Mogre::Entity^ SceneManager::CreateEntity(String^ name, String^ meshName, String^ groupName, SceneMemoryMgrTypes sceneType)
{
	DECLARE_NATIVE_STRING(o_meshName, meshName);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	Mogre::Entity^ result = gcnew Mogre::Entity(_native->createEntity(o_meshName, o_groupName, (Ogre::SceneMemoryMgrTypes)sceneType));
	result->Name = name;
	if (!String::IsNullOrEmpty(name))
	{
		_entities[name] = result;
	}
	return result;
}

Mogre::Entity^ SceneManager::CreateEntity(String^ name, Mogre::SceneManager::PrefabType ptype)
{
	Mogre::Entity^ result = gcnew Mogre::Entity(_native->createEntity((Ogre::SceneManager::PrefabType)ptype));
	result->Name = name;
	if (!String::IsNullOrEmpty(name))
	{
		_entities[name] = result;
	}
	return result;
}

Mogre::Entity^ SceneManager::CreateEntity(String^ name, Mogre::SceneManager::PrefabType ptype, SceneMemoryMgrTypes sceneType)
{
	Mogre::Entity^ result = gcnew Mogre::Entity(_native->createEntity((Ogre::SceneManager::PrefabType)ptype, (Ogre::SceneMemoryMgrTypes)sceneType));
	result->Name = name;
	if (!String::IsNullOrEmpty(name))
	{
		_entities[name] = result;
	}
	return result;
}

Mogre::Entity^ SceneManager::CreateEntity(String^ name, MeshPtr^ mesh, SceneMemoryMgrTypes sceneType)
{
	Mogre::Entity^ result = gcnew Mogre::Entity(_native->createEntity(mesh, (Ogre::SceneMemoryMgrTypes)sceneType));
	result->Name = name;
	if (!String::IsNullOrEmpty(name))
	{
		_entities[name] = result;
	}
	return result;
}

Mogre::Entity^ SceneManager::CreateEntity(String^ name, MeshPtr^ mesh)
{
	Mogre::Entity^ result = gcnew Mogre::Entity(_native->createEntity(mesh));
	result->Name = name;
	if (!String::IsNullOrEmpty(name))
	{
		_entities[name] = result;
	}
	return result;
}

void SceneManager::DestroyEntity(Mogre::Entity^ ent)
{
	_native->destroyEntity(ent);
}

void SceneManager::DestroyEntity(String^ name)
{
	Entity^ entity;
	if (_entities->TryGetValue(name, entity))
	{
		_native->destroyEntity(entity);
	}
}

Mogre::Entity^ SceneManager::GetEntity(String^ name)
{
	if (!_entities->ContainsKey(name))
		throw gcnew ArgumentException(String::Format("Entity with '{0}' name not found", name));

	return _entities[name];
}

bool SceneManager::HasEntity(String^ name)
{
	return _entities->ContainsKey(name);
}

void SceneManager::DestroyAllEntities()
{
	_native->destroyAllEntities();
}

void SceneManager::SetSkyPlane(bool enable, Mogre::Plane plane, String^ materialName, Ogre::Real scale, Ogre::Real tiling, bool drawFirst, Ogre::Real bow, int xsegments, int ysegments, String^ groupName)
{
	DECLARE_NATIVE_STRING(o_materialName, materialName);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	_native->setSkyPlane(enable, FromPlane(plane), o_materialName, scale, tiling, drawFirst, bow, xsegments, ysegments, o_groupName);
}

void SceneManager::SetSkyPlane(bool enable, Mogre::Plane plane, String^ materialName, Ogre::Real scale, Ogre::Real tiling, bool drawFirst, Ogre::Real bow, int xsegments, int ysegments)
{
	DECLARE_NATIVE_STRING(o_materialName, materialName);

	_native->setSkyPlane(enable, FromPlane(plane), o_materialName, scale, tiling, drawFirst, bow, xsegments, ysegments);
}

void SceneManager::SetSkyPlane(bool enable, Mogre::Plane plane, String^ materialName, Ogre::Real scale, Ogre::Real tiling, bool drawFirst, Ogre::Real bow, int xsegments)
{
	DECLARE_NATIVE_STRING(o_materialName, materialName);

	_native->setSkyPlane(enable, FromPlane(plane), o_materialName, scale, tiling, drawFirst, bow, xsegments);
}

void SceneManager::SetSkyPlane(bool enable, Mogre::Plane plane, String^ materialName, Ogre::Real scale, Ogre::Real tiling, bool drawFirst, Ogre::Real bow)
{
	DECLARE_NATIVE_STRING(o_materialName, materialName);

	_native->setSkyPlane(enable, FromPlane(plane), o_materialName, scale, tiling, drawFirst, bow);
}

void SceneManager::SetSkyPlane(bool enable, Mogre::Plane plane, String^ materialName, Ogre::Real scale, Ogre::Real tiling, bool drawFirst)
{
	DECLARE_NATIVE_STRING(o_materialName, materialName);

	_native->setSkyPlane(enable, FromPlane(plane), o_materialName, scale, tiling, drawFirst);
}

void SceneManager::SetSkyPlane(bool enable, Mogre::Plane plane, String^ materialName, Ogre::Real scale, Ogre::Real tiling)
{
	DECLARE_NATIVE_STRING(o_materialName, materialName);

	_native->setSkyPlane(enable, FromPlane(plane), o_materialName, scale, tiling);
}

void SceneManager::SetSkyPlane(bool enable, Mogre::Plane plane, String^ materialName, Ogre::Real scale)
{
	DECLARE_NATIVE_STRING(o_materialName, materialName);

	_native->setSkyPlane(enable, FromPlane(plane), o_materialName, scale);
}

void SceneManager::SetSkyPlane(bool enable, Mogre::Plane plane, String^ materialName)
{
	DECLARE_NATIVE_STRING(o_materialName, materialName);

	_native->setSkyPlane(enable, FromPlane(plane), o_materialName);
}

void SceneManager::SetSkyBox(bool enable, String^ materialName, Ogre::Real distance, bool drawFirst, Mogre::Quaternion orientation, String^ groupName)
{
	DECLARE_NATIVE_STRING(o_materialName, materialName);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	_native->setSkyBox(enable, o_materialName, distance, drawFirst, FromQuaternion(orientation), o_groupName);
}

void SceneManager::SetSkyBox(bool enable, String^ materialName, Ogre::Real distance, bool drawFirst, Mogre::Quaternion orientation)
{
	DECLARE_NATIVE_STRING(o_materialName, materialName);

	_native->setSkyBox(enable, o_materialName, distance, drawFirst, FromQuaternion(orientation));
}

void SceneManager::SetSkyBox(bool enable, String^ materialName, Ogre::Real distance, bool drawFirst)
{
	DECLARE_NATIVE_STRING(o_materialName, materialName);

	_native->setSkyBox(enable, o_materialName, distance, drawFirst);
}

void SceneManager::SetSkyBox(bool enable, String^ materialName, Ogre::Real distance)
{
	DECLARE_NATIVE_STRING(o_materialName, materialName);

	_native->setSkyBox(enable, o_materialName, distance);
}

void SceneManager::SetSkyBox(bool enable, String^ materialName)
{
	DECLARE_NATIVE_STRING(o_materialName, materialName);

	_native->setSkyBox(enable, o_materialName);
}

void SceneManager::SetSkyDome(bool enable, String^ materialName, Ogre::Real curvature, Ogre::Real tiling, Ogre::Real distance, bool drawFirst, Mogre::Quaternion orientation, int xsegments, int ysegments, int ysegments_keep, String^ groupName)
{
	DECLARE_NATIVE_STRING(o_materialName, materialName);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	_native->setSkyDome(enable, o_materialName, curvature, tiling, distance, drawFirst, FromQuaternion(orientation), xsegments, ysegments, ysegments_keep, o_groupName);
}

void SceneManager::SetSkyDome(bool enable, String^ materialName, Ogre::Real curvature, Ogre::Real tiling, Ogre::Real distance, bool drawFirst, Mogre::Quaternion orientation, int xsegments, int ysegments, int ysegments_keep)
{
	DECLARE_NATIVE_STRING(o_materialName, materialName);

	_native->setSkyDome(enable, o_materialName, curvature, tiling, distance, drawFirst, FromQuaternion(orientation), xsegments, ysegments, ysegments_keep);
}

void SceneManager::SetSkyDome(bool enable, String^ materialName, Ogre::Real curvature, Ogre::Real tiling, Ogre::Real distance, bool drawFirst, Mogre::Quaternion orientation, int xsegments, int ysegments)
{
	DECLARE_NATIVE_STRING(o_materialName, materialName);

	_native->setSkyDome(enable, o_materialName, curvature, tiling, distance, drawFirst, FromQuaternion(orientation), xsegments, ysegments);
}

void SceneManager::SetSkyDome(bool enable, String^ materialName, Ogre::Real curvature, Ogre::Real tiling, Ogre::Real distance, bool drawFirst, Mogre::Quaternion orientation, int xsegments)
{
	DECLARE_NATIVE_STRING(o_materialName, materialName);

	_native->setSkyDome(enable, o_materialName, curvature, tiling, distance, drawFirst, FromQuaternion(orientation), xsegments);
}

void SceneManager::SetSkyDome(bool enable, String^ materialName, Ogre::Real curvature, Ogre::Real tiling, Ogre::Real distance, bool drawFirst, Mogre::Quaternion orientation)
{
	DECLARE_NATIVE_STRING(o_materialName, materialName);

	_native->setSkyDome(enable, o_materialName, curvature, tiling, distance, drawFirst, FromQuaternion(orientation));
}

void SceneManager::SetSkyDome(bool enable, String^ materialName, Ogre::Real curvature, Ogre::Real tiling, Ogre::Real distance, bool drawFirst)
{
	DECLARE_NATIVE_STRING(o_materialName, materialName);

	_native->setSkyDome(enable, o_materialName, curvature, tiling, distance, drawFirst);
}

void SceneManager::SetSkyDome(bool enable, String^ materialName, Ogre::Real curvature, Ogre::Real tiling, Ogre::Real distance)
{
	DECLARE_NATIVE_STRING(o_materialName, materialName);

	_native->setSkyDome(enable, o_materialName, curvature, tiling, distance);
}

void SceneManager::SetSkyDome(bool enable, String^ materialName, Ogre::Real curvature, Ogre::Real tiling)
{
	DECLARE_NATIVE_STRING(o_materialName, materialName);

	_native->setSkyDome(enable, o_materialName, curvature, tiling);
}

void SceneManager::SetSkyDome(bool enable, String^ materialName, Ogre::Real curvature)
{
	DECLARE_NATIVE_STRING(o_materialName, materialName);

	_native->setSkyDome(enable, o_materialName, curvature);
}

void SceneManager::SetSkyDome(bool enable, String^ materialName)
{
	DECLARE_NATIVE_STRING(o_materialName, materialName);

	_native->setSkyDome(enable, o_materialName);
}

void SceneManager::SetFog(Mogre::FogMode mode, Mogre::ColourValue colour, Ogre::Real expDensity, Ogre::Real linearStart, Ogre::Real linearEnd)
{
	_native->setFog((Ogre::FogMode)mode, FromColor4(colour), expDensity, linearStart, linearEnd);
}

void SceneManager::SetFog(Mogre::FogMode mode, Mogre::ColourValue colour, Ogre::Real expDensity, Ogre::Real linearStart)
{
	_native->setFog((Ogre::FogMode)mode, FromColor4(colour), expDensity, linearStart);
}

void SceneManager::SetFog(Mogre::FogMode mode, Mogre::ColourValue colour, Ogre::Real expDensity)
{
	_native->setFog((Ogre::FogMode)mode, FromColor4(colour), expDensity);
}

void SceneManager::SetFog(Mogre::FogMode mode, Mogre::ColourValue colour)
{
	_native->setFog((Ogre::FogMode)mode, FromColor4(colour));
}

void SceneManager::SetFog(Mogre::FogMode mode)
{
	_native->setFog((Ogre::FogMode)mode);
}

void SceneManager::SetFog()
{
	_native->setFog();
}

void SceneManager::ClearScene()
{
	_native->clearScene();
}

Mogre::Animation^ SceneManager::CreateAnimation(String^ name, Mogre::Real length)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return gcnew Mogre::Animation(_native->createAnimation(o_name, length));
}

Mogre::Animation^ SceneManager::GetAnimation(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return ObjectTable::GetOrCreateObject<Mogre::Animation^>((IntPtr)
		static_cast<const Ogre::SceneManager*>(_native)->getAnimation(o_name)
		);
}

bool SceneManager::HasAnimation(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return _native->hasAnimation(o_name);
}

void SceneManager::DestroyAnimation(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);
	_native->destroyAnimation(o_name);
}

void SceneManager::DestroyAllAnimations()
{
	_native->destroyAllAnimations();
}

Mogre::AnimationState^ SceneManager::CreateAnimationState(String^ animName)
{
	DECLARE_NATIVE_STRING(o_animName, animName);

	return gcnew Mogre::AnimationState(_native->createAnimationState(o_animName));
}

Mogre::AnimationState^ SceneManager::GetAnimationState(String^ animName)
{
	DECLARE_NATIVE_STRING(o_animName, animName);

	return ObjectTable::GetOrCreateObject<Mogre::AnimationState^>((IntPtr)
		static_cast<const Ogre::SceneManager*>(_native)->getAnimationState(o_animName)
		);
}

bool SceneManager::HasAnimationState(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return _native->hasAnimationState(o_name);
}

void SceneManager::DestroyAnimationState(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	_native->destroyAnimationState(o_name);
}

void SceneManager::DestroyAllAnimationStates()
{
	_native->destroyAllAnimationStates();
}

Mogre::SceneNode^ SceneManager::RootSceneNode::get()
{
	ReturnCachedObjectGcnew(Mogre::SceneNode, _sceneRootDynamic, _native->getRootSceneNode());
}

Ogre::Real SceneManager::ShadowFarDistance::get()
{
	return _native->getShadowFarDistance();
}

void SceneManager::ShadowFarDistance::set(Ogre::Real distance)
{
	_native->setShadowFarDistance(distance);
}

bool SceneManager::ShadowCasterRenderBackFaces::get()
{
	return _native->getShadowCasterRenderBackFaces();
}

void SceneManager::ShadowCasterRenderBackFaces::set(bool bf)
{
	_native->setShadowCasterRenderBackFaces(bf);
}

Mogre::ColourValue SceneManager::ShadowColour::get()
{
	return ToColor4(_native->getShadowColour());
}

void SceneManager::ShadowColour::set(Mogre::ColourValue colour)
{
	_native->setShadowColour(FromColor4(colour));
}

bool SceneManager::FindVisibleObjects::get()
{
	return _native->getFindVisibleObjects();
}
void SceneManager::FindVisibleObjects::set(bool find)
{
	_native->setFindVisibleObjects(find);
}

Mogre::ColourValue SceneManager::FogColour::get()
{
	return ToColor4(_native->getFogColour());
}

Mogre::Real SceneManager::FogDensity::get()
{
	return _native->getFogDensity();
}

Mogre::Real SceneManager::FogEnd::get()
{
	return _native->getFogEnd();
}

Mogre::FogMode SceneManager::FogMode::get()
{
	return (Mogre::FogMode)_native->getFogMode();
}

Mogre::Real SceneManager::FogStart::get()
{
	return _native->getFogStart();
}


Ogre::uint32 SceneManager::VisibilityMask::get()
{
	return _native->getVisibilityMask();
}

void SceneManager::VisibilityMask::set(Ogre::uint32 vmask)
{
	_native->setVisibilityMask(vmask);
}

Mogre::StaticGeometry^ SceneManager::CreateStaticGeometry(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return _native->createStaticGeometry(o_name);
}

Mogre::StaticGeometry^ SceneManager::GetStaticGeometry(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return _native->getStaticGeometry(o_name);
}

bool SceneManager::HasStaticGeometry(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return _native->hasStaticGeometry(o_name);
}

void SceneManager::DestroyStaticGeometry(Mogre::StaticGeometry^ geom)
{
	_native->destroyStaticGeometry(geom);
}

void SceneManager::DestroyStaticGeometry(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	_native->destroyStaticGeometry(o_name);
}

void SceneManager::DestroyAllStaticGeometry()
{
	_native->destroyAllStaticGeometry();
}

Mogre::AxisAlignedBoxSceneQuery^ SceneManager::CreateAABBQuery(Mogre::AxisAlignedBox^ box, unsigned long mask)
{
	return _native->createAABBQuery(FromAxisAlignedBounds(box), mask);
}

Mogre::AxisAlignedBoxSceneQuery^ SceneManager::CreateAABBQuery(Mogre::AxisAlignedBox^ box)
{
	return _native->createAABBQuery(FromAxisAlignedBounds(box));
}

Mogre::SphereSceneQuery^ SceneManager::CreateSphereQuery(Mogre::Sphere sphere, unsigned long mask)
{
	return _native->createSphereQuery(FromSphere(sphere), mask);
}
Mogre::SphereSceneQuery^ SceneManager::CreateSphereQuery(Mogre::Sphere sphere)
{
	return _native->createSphereQuery(FromSphere(sphere));
}

//Mogre::PlaneBoundedVolumeListSceneQuery^ SceneManager::CreatePlaneBoundedVolumeQuery(Mogre::Const_PlaneBoundedVolumeList^ volumes, unsigned long mask)
//{
//	return _native->createPlaneBoundedVolumeQuery(volumes, mask);
//}
//
//Mogre::PlaneBoundedVolumeListSceneQuery^ SceneManager::CreatePlaneBoundedVolumeQuery(Mogre::Const_PlaneBoundedVolumeList^ volumes)
//{
//	return _native->createPlaneBoundedVolumeQuery(volumes);
//}

Mogre::RaySceneQuery^ SceneManager::CreateRayQuery(Mogre::Ray ray, unsigned long mask)
{
	return _native->createRayQuery(FromRay(ray), mask);
}

Mogre::RaySceneQuery^ SceneManager::CreateRayQuery(Mogre::Ray ray)
{
	return _native->createRayQuery(FromRay(ray));
}

Mogre::IntersectionSceneQuery^ SceneManager::CreateIntersectionQuery(unsigned long mask)
{
	return _native->createIntersectionQuery(mask);
}

Mogre::IntersectionSceneQuery^ SceneManager::CreateIntersectionQuery()
{
	return _native->createIntersectionQuery();
}

void SceneManager::DestroyQuery(Mogre::SceneQuery^ query)
{
	_native->destroyQuery(query);
}

Mogre::SceneManager::CameraIterator^ SceneManager::GetCameraIterator()
{
	return _native->getCameraIterator();
}

bool SceneManager::HasMovableObject(MovableObject^ movable)
{
	return _native->hasMovableObject(movable);
}

void SceneManager::DestroyMovableObject(MovableObject^ movable, String^ typeName)
{
	DECLARE_NATIVE_STRING(o_typeName, typeName);

	_native->destroyMovableObject(movable, o_typeName);
}

void SceneManager::DestroyMovableObject(MovableObject^ movable)
{
	_native->destroyMovableObject(movable);
}

void SceneManager::DestroyAllMovableObjectsByType(String^ typeName)
{
	DECLARE_NATIVE_STRING(o_typeName, typeName);

	_native->destroyAllMovableObjectsByType(o_typeName);
}

void SceneManager::DestroyAllMovableObjects()
{
	_native->destroyAllMovableObjects();
}

Mogre::SceneManager::MovableObjectIterator^ SceneManager::GetMovableObjectIterator(String^ typeName)
{
	DECLARE_NATIVE_STRING(o_typeName, typeName);

	return _native->getMovableObjectIterator(o_typeName);
}

void SceneManager::InjectMovableObject(Mogre::MovableObject^ m)
{
	_native->injectMovableObject(m);
}

void SceneManager::ExtractMovableObject(Mogre::MovableObject^ m)
{
	_native->extractMovableObject(m);
}

void SceneManager::ExtractAllMovableObjectsByType(String^ typeName)
{
	DECLARE_NATIVE_STRING(o_typeName, typeName);

	_native->extractAllMovableObjectsByType(o_typeName);
}

Mogre::Pass^ SceneManager::_setPass(Mogre::Pass^ pass, bool evenIfSuppressed, bool shadowDerivation)
{
	return _native->_setPass(pass, evenIfSuppressed, shadowDerivation);
}

Mogre::Pass^ SceneManager::_setPass(Mogre::Pass^ pass, bool evenIfSuppressed)
{
	return _native->_setPass(pass, evenIfSuppressed);
}

Mogre::Pass^ SceneManager::_setPass(Mogre::Pass^ pass)
{
	return _native->_setPass(pass);
}

Ogre::SceneManager* SceneManager::UnmanagedPointer::get()
{
	return _native;
}


CPP_DECLARE_STLMAP(SceneManagerEnumerator::, Instances, String^, Mogre::SceneManager^, Ogre::String, Ogre::SceneManager*);
CPP_DECLARE_MAP_ITERATOR(SceneManagerEnumerator::, SceneManagerIterator, Ogre::SceneManagerEnumerator::SceneManagerIterator, Mogre::SceneManagerEnumerator::Instances, Mogre::SceneManager^, Ogre::SceneManager*, String^, Ogre::String, );