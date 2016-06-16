#include "stdafx.h"
#include "MogreFrustum.h"
#include "MogreCamera.h"
#include "MogreMaterialManager.h"
#include "Marshalling.h"

using namespace Mogre;

Mogre::Real Frustum::INFINITE_FAR_PLANE_ADJUST::get()
{
	return Ogre::Frustum::INFINITE_FAR_PLANE_ADJUST;
}

Mogre::Real Frustum::AspectRatio::get()
{
	return static_cast<const Ogre::Frustum*>(_native)->getAspectRatio();
}
void Frustum::AspectRatio::set(Mogre::Real ratio)
{
	static_cast<Ogre::Frustum*>(_native)->setAspectRatio(ratio);
}

Mogre::AxisAlignedBox^ Frustum::BoundingBox::get()
{
	return ToAxisAlignedBounds(static_cast<const Ogre::Frustum*>(_native)->getBoundingBox());
}

Mogre::Real Frustum::FarClipDistance::get()
{
	return static_cast<const Ogre::Frustum*>(_native)->getFarClipDistance();
}
void Frustum::FarClipDistance::set(Mogre::Real farDist)
{
	static_cast<Ogre::Frustum*>(_native)->setFarClipDistance(farDist);
}

Mogre::Real Frustum::FocalLength::get()
{
	return static_cast<const Ogre::Frustum*>(_native)->getFocalLength();
}
void Frustum::FocalLength::set(Mogre::Real focalLength)
{
	static_cast<Ogre::Frustum*>(_native)->setFocalLength(focalLength);
}

Mogre::Radian Frustum::FOVy::get()
{
	return Mogre::Radian(static_cast<const Ogre::Frustum*>(_native)->getFOVy().valueRadians());
}

void Frustum::FOVy::set(Mogre::Radian fovy)
{
	static_cast<Ogre::Frustum*>(_native)->setFOVy(Ogre::Radian(fovy.ValueRadians));
}

Mogre::Vector2 Frustum::FrustumOffset::get()
{
	return ToVector2(static_cast<const Ogre::Frustum*>(_native)->getFrustumOffset());
}

void Frustum::FrustumOffset::set(Mogre::Vector2 offset)
{
	static_cast<Ogre::Frustum*>(_native)->setFrustumOffset(FromVector2(offset));
}

const Mogre::Plane* Frustum::FrustumPlanes::get()
{
	return reinterpret_cast<const Mogre::Plane*>(static_cast<const Ogre::Frustum*>(_native)->getFrustumPlanes());
}

bool Frustum::IsCustomNearClipPlaneEnabled::get()
{
	return static_cast<const Ogre::Frustum*>(_native)->isCustomNearClipPlaneEnabled();
}

bool Frustum::IsCustomProjectionMatrixEnabled::get()
{
	return static_cast<const Ogre::Frustum*>(_native)->isCustomProjectionMatrixEnabled();
}

bool Frustum::IsCustomViewMatrixEnabled::get()
{
	return static_cast<const Ogre::Frustum*>(_native)->isCustomViewMatrixEnabled();
}

bool Frustum::IsReflected::get()
{
	return static_cast<const Ogre::Frustum*>(_native)->isReflected();
}

String^ Frustum::MovableType::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::Frustum*>(_native)->getMovableType());
}

Mogre::Real Frustum::NearClipDistance::get()
{
	return static_cast<const Ogre::Frustum*>(_native)->getNearClipDistance();
}
void Frustum::NearClipDistance::set(Mogre::Real nearDist)
{
	static_cast<Ogre::Frustum*>(_native)->setNearClipDistance(nearDist);
}

Mogre::Matrix4 Frustum::ProjectionMatrix::get()
{
	return ToMatrix4(static_cast<const Ogre::Frustum*>(_native)->getProjectionMatrix());
}

Mogre::Matrix4 Frustum::ProjectionMatrixRS::get()
{
	return ToMatrix4(static_cast<const Ogre::Frustum*>(_native)->getProjectionMatrixRS());
}

Mogre::Matrix4 Frustum::ProjectionMatrixWithRSDepth::get()
{
	return ToMatrix4(static_cast<const Ogre::Frustum*>(_native)->getProjectionMatrixWithRSDepth());
}

Mogre::ProjectionType Frustum::ProjectionType::get()
{
	return (Mogre::ProjectionType)static_cast<const Ogre::Frustum*>(_native)->getProjectionType();
}

void Frustum::ProjectionType::set(Mogre::ProjectionType pt)
{
	static_cast<Ogre::Frustum*>(_native)->setProjectionType((Ogre::ProjectionType)pt);
}

Mogre::Matrix4 Frustum::ReflectionMatrix::get()
{
	return ToMatrix4(static_cast<const Ogre::Frustum*>(_native)->getReflectionMatrix());
}

Mogre::Plane Frustum::ReflectionPlane::get()
{
	return ToPlane(static_cast<const Ogre::Frustum*>(_native)->getReflectionPlane());
}

Mogre::Matrix4 Frustum::ViewMatrix::get()
{
	return ToMatrix4(static_cast<const Ogre::Frustum*>(_native)->getViewMatrix());
}

const Mogre::Vector3* Frustum::WorldSpaceCorners::get()
{
	return reinterpret_cast<const Mogre::Vector3*>(static_cast<const Ogre::Frustum*>(_native)->getWorldSpaceCorners());
}

void Frustum::SetFrustumOffset(Mogre::Real horizontal, Mogre::Real vertical)
{
	static_cast<Ogre::Frustum*>(_native)->setFrustumOffset(horizontal, vertical);
}
void Frustum::SetFrustumOffset(Mogre::Real horizontal)
{
	static_cast<Ogre::Frustum*>(_native)->setFrustumOffset(horizontal);
}
void Frustum::SetFrustumOffset()
{
	static_cast<Ogre::Frustum*>(_native)->setFrustumOffset();
}

void Frustum::SetCustomViewMatrix(bool enable, Mogre::Matrix4 viewMatrix)
{
	pin_ptr<Ogre::Matrix4> p_viewMatrix = interior_ptr<Ogre::Matrix4>(&viewMatrix.m00);

	static_cast<Ogre::Frustum*>(_native)->setCustomViewMatrix(enable, *p_viewMatrix);
}
void Frustum::SetCustomViewMatrix(bool enable)
{
	static_cast<Ogre::Frustum*>(_native)->setCustomViewMatrix(enable);
}

void Frustum::SetCustomProjectionMatrix(bool enable, Mogre::Matrix4 projectionMatrix)
{
	pin_ptr<Ogre::Matrix4> p_projectionMatrix = interior_ptr<Ogre::Matrix4>(&projectionMatrix.m00);

	static_cast<Ogre::Frustum*>(_native)->setCustomProjectionMatrix(enable, *p_projectionMatrix);
}

void Frustum::SetCustomProjectionMatrix(bool enable)
{
	static_cast<Ogre::Frustum*>(_native)->setCustomProjectionMatrix(enable);
}

Mogre::Plane Frustum::GetFrustumPlane(unsigned short plane)
{
	return ToPlane(static_cast<const Ogre::Frustum*>(_native)->getFrustumPlane(plane));
}

bool Frustum::IsVisible(Mogre::AxisAlignedBox^ bound, [Out] Mogre::FrustumPlane% culledBy)
{
	pin_ptr<Mogre::FrustumPlane> p_culledBy = &culledBy;

	return static_cast<const Ogre::Frustum*>(_native)->isVisible(FromAxisAlignedBounds(bound), (Ogre::FrustumPlane*)p_culledBy);
}

bool Frustum::IsVisible(Mogre::AxisAlignedBox^ bound)
{
	return static_cast<const Ogre::Frustum*>(_native)->isVisible(FromAxisAlignedBounds(bound));
}

bool Frustum::IsVisible(Mogre::Sphere bound, [Out] Mogre::FrustumPlane% culledBy)
{
	pin_ptr<Mogre::FrustumPlane> p_culledBy = &culledBy;

	return static_cast<const Ogre::Frustum*>(_native)->isVisible(FromSphere(bound), (Ogre::FrustumPlane*)p_culledBy);
}

bool Frustum::IsVisible(Mogre::Sphere bound)
{
	return static_cast<const Ogre::Frustum*>(_native)->isVisible(FromSphere(bound));
}

bool Frustum::IsVisible(Mogre::Vector3 vert, [Out] Mogre::FrustumPlane% culledBy)
{
	pin_ptr<Mogre::FrustumPlane> p_culledBy = &culledBy;

	return static_cast<const Ogre::Frustum*>(_native)->isVisible(FromVector3(vert), (Ogre::FrustumPlane*)p_culledBy);
}
bool Frustum::IsVisible(Mogre::Vector3 vert)
{
	return static_cast<const Ogre::Frustum*>(_native)->isVisible(FromVector3(vert));
}

//void Frustum::_updateRenderQueue(Mogre::RenderQueue^ queue)
//{
//	static_cast<Ogre::Frustum*>(_native)->_updateRenderQueue(queue);
//}
//
//void Frustum::_notifyCurrentCamera(Mogre::Camera^ cam)
//{
//	static_cast<Ogre::Frustum*>(_native)->_notifyCurrentCamera(cam);
//}
//
//Mogre::MaterialPtr^ Frustum::GetMaterial()
//{
//	return static_cast<const Ogre::Frustum*>(_native)->getMaterial();
//}
//
//void Frustum::GetRenderOperation(Mogre::RenderOperation^ op)
//{
//	static_cast<Ogre::Frustum*>(_native)->getRenderOperation(op);
//}

void Frustum::GetWorldTransforms(Mogre::Matrix4* xform)
{
	Ogre::Matrix4* o_xform = reinterpret_cast<Ogre::Matrix4*>(xform);

	static_cast<const Ogre::Frustum*>(_native)->getWorldTransforms(o_xform);
}

Mogre::Real Frustum::GetSquaredViewDepth(Mogre::Camera^ cam)
{
	return static_cast<const Ogre::Frustum*>(_native)->getSquaredViewDepth(cam);
}

//Mogre::Const_LightList^ Frustum::GetLights()
//{
//	return static_cast<const Ogre::Frustum*>(_native)->getLights();
//}

void Frustum::EnableReflection(Mogre::Plane p)
{
	static_cast<Ogre::Frustum*>(_native)->enableReflection(FromPlane(p));
}

//void Frustum::EnableReflection(Mogre::MovablePlane^ p)
//{
//	static_cast<Ogre::Frustum*>(_native)->enableReflection(p);
//}

void Frustum::DisableReflection()
{
	static_cast<Ogre::Frustum*>(_native)->disableReflection();
}

bool Frustum::ProjectSphere(Mogre::Sphere sphere, [Out] Mogre::Real% left, [Out] Mogre::Real% top, [Out] Mogre::Real% right, [Out] Mogre::Real% bottom)
{
	pin_ptr<Mogre::Real> p_left = &left;
	pin_ptr<Mogre::Real> p_top = &top;
	pin_ptr<Mogre::Real> p_right = &right;
	pin_ptr<Mogre::Real> p_bottom = &bottom;

	return static_cast<const Ogre::Frustum*>(_native)->projectSphere(FromSphere(sphere), p_left, p_top, p_right, p_bottom);
}

//void Frustum::EnableCustomNearClipPlane(Mogre::MovablePlane^ plane)
//{
//	static_cast<Ogre::Frustum*>(_native)->enableCustomNearClipPlane(plane);
//}

void Frustum::EnableCustomNearClipPlane(Mogre::Plane plane)
{
	static_cast<Ogre::Frustum*>(_native)->enableCustomNearClipPlane(FromPlane(plane));
}

void Frustum::DisableCustomNearClipPlane()
{
	static_cast<Ogre::Frustum*>(_native)->disableCustomNearClipPlane();
}

//------------------------------------------------------------
// Implementation for IRenderable
//------------------------------------------------------------

bool Frustum::CastsShadows::get()
{
	return static_cast<const Ogre::Frustum*>(_native)->getCastsShadows();
}

unsigned short Frustum::NumWorldTransforms::get()
{
	return static_cast<const Ogre::Frustum*>(_native)->getNumWorldTransforms();
}

bool Frustum::PolygonModeOverrideable::get()
{
	return static_cast<const Ogre::Frustum*>(_native)->getPolygonModeOverrideable();
}
void Frustum::PolygonModeOverrideable::set(bool override)
{
	static_cast<Ogre::Frustum*>(_native)->setPolygonModeOverrideable(override);
}

Mogre::Technique^ Frustum::Technique::get()
{
	return static_cast<const Ogre::Frustum*>(_native)->getTechnique();
}

bool Frustum::UseIdentityProjection::get()
{
	return static_cast<const Ogre::Frustum*>(_native)->getUseIdentityProjection();
}
void Frustum::UseIdentityProjection::set(bool useIdentityProjection)
{
	static_cast<Ogre::Frustum*>(_native)->setUseIdentityProjection(useIdentityProjection);
}

bool Frustum::UseIdentityView::get()
{
	return static_cast<const Ogre::Frustum*>(_native)->getUseIdentityView();
}
void Frustum::UseIdentityView::set(bool useIdentityView)
{
	static_cast<Ogre::Frustum*>(_native)->setUseIdentityView(useIdentityView);
}

//Mogre::Const_PlaneList^ Frustum::GetClipPlanes()
//{
//	return static_cast<const Ogre::Frustum*>(_native)->getClipPlanes();
//}

void Frustum::SetCustomParameter(size_t index, Mogre::Vector4 value)
{
	static_cast<Ogre::Frustum*>(_native)->setCustomParameter(index, FromVector4(value));
}

Mogre::Vector4 Frustum::GetCustomParameter(size_t index)
{
	return ToVector4(static_cast<const Ogre::Frustum*>(_native)->getCustomParameter(index));
}

//void Frustum::_updateCustomGpuParameter(Mogre::GpuProgramParameters::AutoConstantEntry_NativePtr constantEntry, Mogre::GpuProgramParameters^ params)
//{
//	static_cast<const Ogre::Frustum*>(_native)->_updateCustomGpuParameter(constantEntry, params);
//}