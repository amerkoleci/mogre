#pragma once

#include "OgreManualObject.h"
#include "MogreMovableObject.h"
#include "MogreRenderOperation.h"
#include "STLContainerWrappers.h"
#include "IteratorWrapper.h"
#include "Marshalling.h"

namespace Mogre
{
	ref class Mesh;
	ref class MeshPtr;
	ref class Technique;
	ref class Material;
	ref class MaterialPtr;
	ref class Camera;
	ref class ManualObject;
	ref class EdgeData;

	public ref class ManualObject : public MovableObject
	{
		//################################################################
		//ManualObjectSection
		//################################################################

	public:
		ref class ManualObjectSection : public IMogreDisposable//, public IRenderable
		{
		public:
			/// <summary>Raised before any disposing is performed.</summary>
			virtual event EventHandler^ OnDisposing;
			/// <summary>Raised once all disposing is performed.</summary>
			virtual event EventHandler^ OnDisposed;

		internal:
			Ogre::ManualObject::ManualObjectSection* _native;
			bool _createdByCLR;

		public protected:
			ManualObjectSection(Ogre::ManualObject::ManualObjectSection* obj) : _native(obj)
			{
			}

			ManualObjectSection(intptr_t ptr) : _native((Ogre::ManualObject::ManualObjectSection*)ptr)
			{

			}

		public:
			~ManualObjectSection();
		protected:
			!ManualObjectSection();

			//virtual Ogre::Renderable* _IRenderable_GetNativePtr() = IRenderable::_GetNativePtr;
			//Public Declarations
		public:
			ManualObjectSection(Mogre::ManualObject^ parent, String^ materialName, Mogre::RenderOperation::OperationTypes opType);

			property bool IsDisposed
			{
				virtual bool get()
				{
					return _native == nullptr;
				}
			}

			property String^ MaterialName
			{
			public:
				String^ get();
			public:
				void set(String^ name);
			}

			property Mogre::RenderOperation^ RenderOperation
			{
			public:
				Mogre::RenderOperation^ get();
			}

			virtual Mogre::MaterialPtr^ GetMaterial();

			virtual void GetRenderOperation(Mogre::RenderOperation^ op);

			virtual void GetWorldTransforms(Mogre::Matrix4* xform);

			virtual Ogre::Real GetSquaredViewDepth(Mogre::Camera^ param1);

			//virtual Mogre::Const_LightList^ GetLights();

			//------------------------------------------------------------
			// Implementation for IRenderable
			//------------------------------------------------------------

			property bool CastsShadows
			{
			public:
				virtual bool get();
			}

			property unsigned short NumWorldTransforms
			{
			public:
				virtual unsigned short get();
			}

			property bool PolygonModeOverrideable
			{
			public:
				virtual bool get();
			public:
				virtual void set(bool override);
			}

			property Mogre::Technique^ Technique
			{
			public:
				virtual Mogre::Technique^ get();
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

		public:
			//virtual Mogre::Const_PlaneList^ GetClipPlanes();
			void SetCustomParameter(size_t index, Mogre::Vector4 value);
			Mogre::Vector4 GetCustomParameter(size_t index);

		public:
			//virtual void _updateCustomGpuParameter(Mogre::GpuProgramParameters::AutoConstantEntry_NativePtr constantEntry, Mogre::GpuProgramParameters^ params);

			DEFINE_MANAGED_NATIVE_CONVERSIONS(ManualObject::ManualObjectSection);
		};

#define STLDECL_MANAGEDTYPE Mogre::ManualObject::ManualObjectSection^
#define STLDECL_NATIVETYPE Ogre::ManualObject::ManualObjectSection*
		INC_DECLARE_STLVECTOR(SectionList, STLDECL_MANAGEDTYPE, STLDECL_NATIVETYPE, public:, private:);
#undef STLDECL_MANAGEDTYPE
#undef STLDECL_NATIVETYPE

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

		property Mogre::EdgeData^ EdgeList
		{
		public:
			Mogre::EdgeData^ get();
		}

		property bool HasEdgeList
		{
		public:
			bool get();
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

		void Tangent(Mogre::Vector3 norm);
		void Tangent(Ogre::Real x, Ogre::Real y, Ogre::Real z);

		void TextureCoord(Ogre::Real u);
		void TextureCoord(Ogre::Real u, Ogre::Real v);
		void TextureCoord(Ogre::Real u, Ogre::Real v, Ogre::Real w);
		void TextureCoord(Ogre::Real x, Ogre::Real y, Ogre::Real z, Ogre::Real w);

		void TextureCoord(Mogre::Vector2 uv);
		void TextureCoord(Mogre::Vector3 uvw);
		void TextureCoord(Mogre::Vector4 xyzw);

		void Colour(Mogre::ColourValue col);

		void Colour(Ogre::Real r, Ogre::Real g, Ogre::Real b, Ogre::Real a);
		void Colour(Ogre::Real r, Ogre::Real g, Ogre::Real b);

		void Index(Ogre::uint32 idx);

		void Triangle(Ogre::uint32 i1, Ogre::uint32 i2, Ogre::uint32 i3);

		void Quad(Ogre::uint32 i1, Ogre::uint32 i2, Ogre::uint32 i3, Ogre::uint32 i4);

		Mogre::ManualObject::ManualObjectSection^ End();

		void SetMaterialName(size_t subindex, String^ name);

		Mogre::MeshPtr^ ConvertToMesh(String^ meshName, String^ groupName);
		Mogre::MeshPtr^ ConvertToMesh(String^ meshName);
		Mogre::ManualObject::ManualObjectSection^ GetSection(unsigned int index);

		DEFINE_MANAGED_NATIVE_CONVERSIONS(ManualObject);

	internal:
		property Ogre::ManualObject* UnmanagedPointer
		{
			Ogre::ManualObject* get();
		}
	};
}