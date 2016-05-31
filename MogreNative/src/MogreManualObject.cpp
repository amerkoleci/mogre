#include "stdafx.h"
#include "MogreManualObject.h"
#include "MogreCommon.h"
#include "MogreMeshManager.h"

using namespace Mogre;

bool ManualObject::Dynamic::get()
{
	return static_cast<const Ogre::ManualObject*>(_native)->getDynamic();
}

void ManualObject::Dynamic::set(bool dyn)
{
	static_cast<Ogre::ManualObject*>(_native)->setDynamic(dyn);
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

void ManualObject::TextureCoord(Mogre::Vector2 uv)
{
	static_cast<Ogre::ManualObject*>(_native)->textureCoord(FromVector2(uv));
}

void ManualObject::TextureCoord(Mogre::Vector3 uvw)
{
	static_cast<Ogre::ManualObject*>(_native)->textureCoord(FromVector3(uvw));
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

void ManualObject::Index(Ogre::uint16 idx)
{
	static_cast<Ogre::ManualObject*>(_native)->index(idx);
}

void ManualObject::Triangle(Ogre::uint16 i1, Ogre::uint16 i2, Ogre::uint16 i3)
{
	static_cast<Ogre::ManualObject*>(_native)->triangle(i1, i2, i3);
}

void ManualObject::Quad(Ogre::uint16 i1, Ogre::uint16 i2, Ogre::uint16 i3, Ogre::uint16 i4)
{
	static_cast<Ogre::ManualObject*>(_native)->quad(i1, i2, i3, i4);
}

//Mogre::ManualObject::ManualObjectSection^ ManualObject::End()
//{
//	return static_cast<Ogre::ManualObject*>(_native)->end();
//}

void ManualObject::End()
{
	static_cast<Ogre::ManualObject*>(_native)->end();
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

Ogre::ManualObject* ManualObject::UnmanagedPointer::get()
{
	return static_cast<Ogre::ManualObject*>(_native);
}