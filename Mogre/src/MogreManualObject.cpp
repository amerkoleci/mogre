#include "stdafx.h"
#include "MogreManualObject.h"
#include "MogreCommon.h"
#include "MogreMeshManager.h"
#include "MogreMaterialManager.h"
#include "MogreCamera.h"
#include "MogreEdgeListBuilder.h"

using namespace Mogre;

ManualObject::ManualObjectSection::ManualObjectSection(Mogre::ManualObject^ parent, String^ materialName, Mogre::RenderOperation::OperationTypes opType)
{
	_createdByCLR = true;
	DECLARE_NATIVE_STRING(o_materialName, materialName);
	_native = new Ogre::ManualObject::ManualObjectSection(parent, o_materialName, (Ogre::RenderOperation::OperationType)opType);
}

ManualObject::ManualObjectSection::~ManualObjectSection()
{
	this->!ManualObjectSection();
}

ManualObject::ManualObjectSection::!ManualObjectSection()
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


String^ ManualObject::ManualObjectSection::MaterialName::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::ManualObject::ManualObjectSection*>(_native)->getMaterialName());
}

void ManualObject::ManualObjectSection::MaterialName::set(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	static_cast<Ogre::ManualObject::ManualObjectSection*>(_native)->setMaterialName(o_name);
}

Mogre::RenderOperation^ ManualObject::ManualObjectSection::RenderOperation::get()
{
	return static_cast<Ogre::ManualObject::ManualObjectSection*>(_native)->getRenderOperation();
}

Mogre::MaterialPtr^ ManualObject::ManualObjectSection::GetMaterial()
{
	return static_cast<const Ogre::ManualObject::ManualObjectSection*>(_native)->getMaterial();
}

void ManualObject::ManualObjectSection::GetRenderOperation(Mogre::RenderOperation^ op)
{
	static_cast<Ogre::ManualObject::ManualObjectSection*>(_native)->getRenderOperation(op);
}

void ManualObject::ManualObjectSection::GetWorldTransforms(Mogre::Matrix4* xform)
{
	Ogre::Matrix4* o_xform = reinterpret_cast<Ogre::Matrix4*>(xform);

	static_cast<const Ogre::ManualObject::ManualObjectSection*>(_native)->getWorldTransforms(o_xform);
}

Mogre::Real ManualObject::ManualObjectSection::GetSquaredViewDepth(Mogre::Camera^ param1)
{
	return static_cast<const Ogre::ManualObject::ManualObjectSection*>(_native)->getSquaredViewDepth(param1);
}

//Mogre::Const_LightList^ ManualObject::ManualObjectSection::GetLights()
//{
//	return static_cast<const Ogre::ManualObject::ManualObjectSection*>(_native)->getLights();
//}

//------------------------------------------------------------
// Implementation for IRenderable
//------------------------------------------------------------

bool ManualObject::ManualObjectSection::CastsShadows::get()
{
	return static_cast<const Ogre::ManualObject::ManualObjectSection*>(_native)->getCastsShadows();
}

unsigned short ManualObject::ManualObjectSection::NumWorldTransforms::get()
{
	return static_cast<const Ogre::ManualObject::ManualObjectSection*>(_native)->getNumWorldTransforms();
}

bool ManualObject::ManualObjectSection::PolygonModeOverrideable::get()
{
	return static_cast<const Ogre::ManualObject::ManualObjectSection*>(_native)->getPolygonModeOverrideable();
}
void ManualObject::ManualObjectSection::PolygonModeOverrideable::set(bool override)
{
	static_cast<Ogre::ManualObject::ManualObjectSection*>(_native)->setPolygonModeOverrideable(override);
}

Mogre::Technique^ ManualObject::ManualObjectSection::Technique::get()
{
	return static_cast<const Ogre::ManualObject::ManualObjectSection*>(_native)->getTechnique();
}

bool ManualObject::ManualObjectSection::UseIdentityProjection::get()
{
	return static_cast<const Ogre::ManualObject::ManualObjectSection*>(_native)->getUseIdentityProjection();
}
void ManualObject::ManualObjectSection::UseIdentityProjection::set(bool useIdentityProjection)
{
	static_cast<Ogre::ManualObject::ManualObjectSection*>(_native)->setUseIdentityProjection(useIdentityProjection);
}

bool ManualObject::ManualObjectSection::UseIdentityView::get()
{
	return static_cast<const Ogre::ManualObject::ManualObjectSection*>(_native)->getUseIdentityView();
}
void ManualObject::ManualObjectSection::UseIdentityView::set(bool useIdentityView)
{
	static_cast<Ogre::ManualObject::ManualObjectSection*>(_native)->setUseIdentityView(useIdentityView);
}

//Mogre::Const_PlaneList^ ManualObject::ManualObjectSection::GetClipPlanes()
//{
//	return static_cast<const Ogre::ManualObject::ManualObjectSection*>(_native)->getClipPlanes();
//}

void ManualObject::ManualObjectSection::SetCustomParameter(size_t index, Mogre::Vector4 value)
{
	static_cast<Ogre::ManualObject::ManualObjectSection*>(_native)->setCustomParameter(index, FromVector4(value));
}

Mogre::Vector4 ManualObject::ManualObjectSection::GetCustomParameter(size_t index)
{
	return ToVector4(
		static_cast<const Ogre::ManualObject::ManualObjectSection*>(_native)->getCustomParameter(index)
	);
}

//void ManualObject::ManualObjectSection::_updateCustomGpuParameter(Mogre::GpuProgramParameters::AutoConstantEntry_NativePtr constantEntry, Mogre::GpuProgramParameters^ params)
//{
//	static_cast<const Ogre::ManualObject::ManualObjectSection*>(_native)->_updateCustomGpuParameter(constantEntry, params);
//}

#define STLDECL_MANAGEDTYPE Mogre::ManualObject::ManualObjectSection^
#define STLDECL_NATIVETYPE Ogre::ManualObject::ManualObjectSection*
CPP_DECLARE_STLVECTOR(ManualObject::, SectionList, STLDECL_MANAGEDTYPE, STLDECL_NATIVETYPE);
#undef STLDECL_MANAGEDTYPE
#undef STLDECL_NATIVETYPE

bool ManualObject::Dynamic::get()
{
	return static_cast<const Ogre::ManualObject*>(_native)->getDynamic();
}

void ManualObject::Dynamic::set(bool dyn)
{
	static_cast<Ogre::ManualObject*>(_native)->setDynamic(dyn);
}

Mogre::EdgeData^ ManualObject::EdgeList::get()
{
	return static_cast<Ogre::ManualObject*>(_native)->getEdgeList();
}

bool ManualObject::HasEdgeList::get()
{
	return static_cast<Ogre::ManualObject*>(_native)->hasEdgeList();
}

String^ ManualObject::MovableType::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::ManualObject*>(_native)->getMovableType());
}

unsigned int ManualObject::NumSections::get()
{
	return static_cast<const Ogre::ManualObject*>(_native)->getNumSections();
}

bool ManualObject::UseIdentityProjection::get()
{
	return static_cast<const Ogre::ManualObject*>(_native)->getUseIdentityProjection();
}

void ManualObject::UseIdentityProjection::set(bool useIdentityProjection)
{
	static_cast<Ogre::ManualObject*>(_native)->setUseIdentityProjection(useIdentityProjection);
}

bool ManualObject::UseIdentityView::get()
{
	return static_cast<const Ogre::ManualObject*>(_native)->getUseIdentityView();
}

void ManualObject::UseIdentityView::set(bool useIdentityView)
{
	static_cast<Ogre::ManualObject*>(_native)->setUseIdentityView(useIdentityView);
}

void ManualObject::Clear()
{
	static_cast<Ogre::ManualObject*>(_native)->clear();
}

void ManualObject::EstimateVertexCount(size_t vcount)
{
	static_cast<Ogre::ManualObject*>(_native)->estimateVertexCount(vcount);
}

void ManualObject::EstimateIndexCount(size_t icount)
{
	static_cast<Ogre::ManualObject*>(_native)->estimateIndexCount(icount);
}

void ManualObject::Begin(String^ materialName, Mogre::RenderOperation::OperationTypes opType)
{
	DECLARE_NATIVE_STRING(o_materialName, materialName);

	static_cast<Ogre::ManualObject*>(_native)->begin(o_materialName, (Ogre::RenderOperation::OperationType)opType);
}

void ManualObject::Begin(String^ materialName)
{
	DECLARE_NATIVE_STRING(o_materialName, materialName);

	static_cast<Ogre::ManualObject*>(_native)->begin(o_materialName);
}

void ManualObject::BeginUpdate(size_t sectionIndex)
{
	static_cast<Ogre::ManualObject*>(_native)->beginUpdate(sectionIndex);
}

void ManualObject::Position(Mogre::Vector3 pos)
{
	static_cast<Ogre::ManualObject*>(_native)->position(FromVector3(pos));
}

void ManualObject::Position(Mogre::Real x, Mogre::Real y, Mogre::Real z)
{
	static_cast<Ogre::ManualObject*>(_native)->position(x, y, z);
}

void ManualObject::Normal(Mogre::Vector3 norm)
{
	static_cast<Ogre::ManualObject*>(_native)->normal(FromVector3(norm));
}

void ManualObject::Normal(Mogre::Real x, Mogre::Real y, Mogre::Real z)
{
	static_cast<Ogre::ManualObject*>(_native)->normal(x, y, z);
}

void ManualObject::Tangent(Mogre::Vector3 norm)
{
	static_cast<Ogre::ManualObject*>(_native)->tangent(FromVector3(norm));
}

void ManualObject::Tangent(Mogre::Real x, Mogre::Real y, Mogre::Real z)
{
	static_cast<Ogre::ManualObject*>(_native)->tangent(x, y, z);
}

void ManualObject::TextureCoord(Mogre::Real u)
{
	static_cast<Ogre::ManualObject*>(_native)->textureCoord(u);
}

void ManualObject::TextureCoord(Mogre::Real u, Mogre::Real v)
{
	static_cast<Ogre::ManualObject*>(_native)->textureCoord(u, v);
}

void ManualObject::TextureCoord(Mogre::Real u, Mogre::Real v, Mogre::Real w)
{
	static_cast<Ogre::ManualObject*>(_native)->textureCoord(u, v, w);
}

void ManualObject::TextureCoord(Ogre::Real x, Ogre::Real y, Ogre::Real z, Ogre::Real w)
{
	static_cast<Ogre::ManualObject*>(_native)->textureCoord(x, y, z, w);
}

void ManualObject::TextureCoord(Mogre::Vector2 uv)
{
	static_cast<Ogre::ManualObject*>(_native)->textureCoord(FromVector2(uv));
}

void ManualObject::TextureCoord(Mogre::Vector3 uvw)
{
	static_cast<Ogre::ManualObject*>(_native)->textureCoord(FromVector3(uvw));
}

void ManualObject::TextureCoord(Mogre::Vector4 xyzw)
{
	static_cast<Ogre::ManualObject*>(_native)->textureCoord(FromVector4(xyzw));
}

void ManualObject::Colour(Mogre::ColourValue col)
{
	static_cast<Ogre::ManualObject*>(_native)->colour(FromColor4(col));
}

void ManualObject::Colour(Mogre::Real r, Mogre::Real g, Mogre::Real b, Mogre::Real a)
{
	static_cast<Ogre::ManualObject*>(_native)->colour(r, g, b, a);
}
void ManualObject::Colour(Mogre::Real r, Mogre::Real g, Mogre::Real b)
{
	static_cast<Ogre::ManualObject*>(_native)->colour(r, g, b);
}

void ManualObject::Index(Ogre::uint32 idx)
{
	static_cast<Ogre::ManualObject*>(_native)->index(idx);
}

void ManualObject::Triangle(Ogre::uint32 i1, Ogre::uint32 i2, Ogre::uint32 i3)
{
	static_cast<Ogre::ManualObject*>(_native)->triangle(i1, i2, i3);
}

void ManualObject::Quad(Ogre::uint32 i1, Ogre::uint32 i2, Ogre::uint32 i3, Ogre::uint32 i4)
{
	static_cast<Ogre::ManualObject*>(_native)->quad(i1, i2, i3, i4);
}

Mogre::ManualObject::ManualObjectSection^ ManualObject::End()
{
	return gcnew Mogre::ManualObject::ManualObjectSection(static_cast<Ogre::ManualObject*>(_native)->end());
}

Mogre::ManualObject::ManualObjectSection^ ManualObject::GetSection(unsigned int index)
{
	return static_cast<const Ogre::ManualObject*>(_native)->getSection(index);
}

void ManualObject::SetMaterialName(size_t subindex, String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	static_cast<Ogre::ManualObject*>(_native)->setMaterialName(subindex, o_name);
}

Mogre::MeshPtr^ ManualObject::ConvertToMesh(String^ meshName, String^ groupName)
{
	DECLARE_NATIVE_STRING(o_meshName, meshName);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::ManualObject*>(_native)->convertToMesh(o_meshName, o_groupName);
}

Mogre::MeshPtr^ ManualObject::ConvertToMesh(String^ meshName)
{
	DECLARE_NATIVE_STRING(o_meshName, meshName);

	return static_cast<Ogre::ManualObject*>(_native)->convertToMesh(o_meshName);
}