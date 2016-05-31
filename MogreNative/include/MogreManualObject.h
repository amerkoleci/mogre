#pragma once

#include "OgreManualObject.h"
#include "MogreMovableObject.h"
#include "MogreRenderOperation.h"
#include "MogreRenderOperation.h"
#include "Marshalling.h"

namespace Mogre
{
	ref class MeshPtr;

	public ref class ManualObject : public MovableObject
	{
	public protected:
		ManualObject(intptr_t ptr) : MovableObject(ptr)
		{

		}

		ManualObject(Ogre::ManualObject* obj) : MovableObject(obj)
		{

		}

	public:
		property bool Dynamic
		{
		public:
			bool get();
		public:
			void set(bool dyn);
		}

		property String^ MovableType
		{
		public:
			String^ get();
		}

		property unsigned int NumSections
		{
		public:
			unsigned int get();
		}

		property bool UseIdentityProjection
		{
		public:
			bool get();
		public:
			void set(bool useIdentityProjection);
		}

		property bool UseIdentityView
		{
		public:
			bool get();
		public:
			void set(bool useIdentityView);
		}

		void Clear();

		void EstimateVertexCount(size_t vcount);
		void EstimateIndexCount(size_t icount);

		void Begin(String^ materialName, Mogre::RenderOperation::OperationTypes opType);
		void Begin(String^ materialName);

		void BeginUpdate(size_t sectionIndex);

		void Position(Mogre::Vector3 pos);

		void Position(Ogre::Real x, Ogre::Real y, Ogre::Real z);

		void Normal(Mogre::Vector3 norm);

		void Normal(Ogre::Real x, Ogre::Real y, Ogre::Real z);

		void TextureCoord(Ogre::Real u);

		void TextureCoord(Ogre::Real u, Ogre::Real v);

		void TextureCoord(Ogre::Real u, Ogre::Real v, Ogre::Real w);

		void TextureCoord(Mogre::Vector2 uv);

		void TextureCoord(Mogre::Vector3 uvw);

		void Colour(Mogre::ColourValue col);

		void Colour(Ogre::Real r, Ogre::Real g, Ogre::Real b, Ogre::Real a);
		void Colour(Ogre::Real r, Ogre::Real g, Ogre::Real b);

		void Index(Ogre::uint16 idx);

		void Triangle(Ogre::uint16 i1, Ogre::uint16 i2, Ogre::uint16 i3);

		void Quad(Ogre::uint16 i1, Ogre::uint16 i2, Ogre::uint16 i3, Ogre::uint16 i4);

		//Mogre::ManualObject::ManualObjectSection^ End();
		void End();

		void SetMaterialName(size_t subindex, String^ name);

		Mogre::MeshPtr^ ConvertToMesh(String^ meshName, String^ groupName);
		Mogre::MeshPtr^ ConvertToMesh(String^ meshName);
		//Mogre::ManualObject::ManualObjectSection^ GetSection(unsigned int index);

		DEFINE_MANAGED_NATIVE_CONVERSIONS(ManualObject);

	internal:
		property Ogre::ManualObject* UnmanagedPointer
		{
			Ogre::ManualObject* get();
		}
	};
}