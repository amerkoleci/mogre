#pragma once

#include "OgreGpuProgramManager.h"
#include "OgreGpuProgram.h"
#include "MogreResourceManager.h"
#include "MogreResource.h"

namespace Mogre
{
	public enum class GpuConstantType
	{
		GCT_FLOAT1 = Ogre::GCT_FLOAT1,
		GCT_FLOAT2 = Ogre::GCT_FLOAT2,
		GCT_FLOAT3 = Ogre::GCT_FLOAT3,
		GCT_FLOAT4 = Ogre::GCT_FLOAT4,
		GCT_SAMPLER1D = Ogre::GCT_SAMPLER1D,
		GCT_SAMPLER2D = Ogre::GCT_SAMPLER2D,
		GCT_SAMPLER3D = Ogre::GCT_SAMPLER3D,
		GCT_SAMPLERCUBE = Ogre::GCT_SAMPLERCUBE,
		GCT_SAMPLER1DSHADOW = Ogre::GCT_SAMPLER1DSHADOW,
		GCT_SAMPLER2DSHADOW = Ogre::GCT_SAMPLER2DSHADOW,
		GCT_MATRIX_2X2 = Ogre::GCT_MATRIX_2X2,
		GCT_MATRIX_2X3 = Ogre::GCT_MATRIX_2X3,
		GCT_MATRIX_2X4 = Ogre::GCT_MATRIX_2X4,
		GCT_MATRIX_3X2 = Ogre::GCT_MATRIX_3X2,
		GCT_MATRIX_3X3 = Ogre::GCT_MATRIX_3X3,
		GCT_MATRIX_3X4 = Ogre::GCT_MATRIX_3X4,
		GCT_MATRIX_4X2 = Ogre::GCT_MATRIX_4X2,
		GCT_MATRIX_4X3 = Ogre::GCT_MATRIX_4X3,
		GCT_MATRIX_4X4 = Ogre::GCT_MATRIX_4X4,
		GCT_INT1 = Ogre::GCT_INT1,
		GCT_INT2 = Ogre::GCT_INT2,
		GCT_INT3 = Ogre::GCT_INT3,
		GCT_INT4 = Ogre::GCT_INT4,
		GCT_UNKNOWN = Ogre::GCT_UNKNOWN
	};

	public enum class GpuProgramType
	{
		GPT_VERTEX_PROGRAM = Ogre::GPT_VERTEX_PROGRAM,
		GPT_FRAGMENT_PROGRAM = Ogre::GPT_FRAGMENT_PROGRAM
	};

	public ref class GpuProgram : public Resource
	{
	public protected:
		GpuProgram(Ogre::GpuProgram* obj) : Resource(obj)
		{
		}

		GpuProgram(Ogre::Resource* obj) : Resource(obj)
		{
		}

	public:
		property bool HasCompileError
		{
		public:
			bool get();
		}

		property bool HasDefaultParameters
		{
		public:
			bool get();
		}

		property bool IsMorphAnimationIncluded
		{
		public:
			bool get();
		}

		property bool IsPoseAnimationIncluded
		{
		public:
			bool get();
		}

		property bool IsSkeletalAnimationIncluded
		{
		public:
			bool get();
		}

		property bool IsSupported
		{
		public:
			bool get();
		}

		property bool IsVertexTextureFetchRequired
		{
		public:
			bool get();
		}

		property String^ Language
		{
		public:
			String^ get();
		}

		property Ogre::ushort NumberOfPosesIncluded
		{
		public:
			Ogre::ushort get();
		}

		property bool PassSurfaceAndLightStates
		{
		public:
			bool get();
		}

		property String^ Source
		{
		public:
			String^ get();
		public:
			void set(String^ source);
		}

		property String^ SourceFile
		{
		public:
			String^ get();
		public:
			void set(String^ filename);
		}

		property String^ SyntaxCode
		{
		public:
			String^ get();
		public:
			void set(String^ syntax);
		}

		property Mogre::GpuProgramType Type
		{
		public:
			Mogre::GpuProgramType get();
		public:
			void set(Mogre::GpuProgramType t);
		}

		Mogre::GpuProgram^ GetBindingDelegate();

		//Mogre::GpuProgramParametersSharedPtr^ CreateParameters();

		void SetSkeletalAnimationIncluded(bool included);

		void SetMorphAnimationIncluded(bool included);

		void SetPoseAnimationIncluded(Ogre::ushort poseCount);

		void SetVertexTextureFetchRequired(bool r);

		//Mogre::GpuProgramParametersSharedPtr^ GetDefaultParameters();

		void ResetCompileError();

		//DEFINE_MANAGED_NATIVE_CONVERSIONS(GpuProgram);
	};

	public ref class GpuProgramPtr : public GpuProgram
	{
	public protected:
		Ogre::GpuProgramPtr* _sharedPtr;

		GpuProgramPtr(Ogre::GpuProgramPtr& sharedPtr) : GpuProgram(sharedPtr.getPointer())
		{
			_sharedPtr = new Ogre::GpuProgramPtr(sharedPtr);
		}

		GpuProgramPtr(Ogre::ResourcePtr& sharedPtr) : GpuProgram(sharedPtr.getPointer())
		{
			_sharedPtr = new Ogre::GpuProgramPtr(static_cast<Ogre::GpuProgram*>(sharedPtr.getPointer()));
		}

		!GpuProgramPtr()
		{
			if (_sharedPtr != 0)
			{
				delete _sharedPtr;
				_sharedPtr = 0;
			}
		}

		~GpuProgramPtr()
		{
			this->!GpuProgramPtr();
		}

	public:
		DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_SHAREDPTR(GpuProgramPtr);

		static GpuProgramPtr^ FromResourcePtr(ResourcePtr^ ptr)
		{
			return (GpuProgramPtr^)ptr;
		}

		static operator GpuProgramPtr^ (ResourcePtr^ ptr)
		{
			if (CLR_NULL == ptr) return nullptr;
			Ogre::GpuProgram* castptr = dynamic_cast<Ogre::GpuProgram*>(ptr->_native);
			if (castptr == 0) throw gcnew InvalidCastException("The underlying type of the ResourcePtr object is not of type GpuProgram.");
			return gcnew GpuProgramPtr(Ogre::GpuProgramPtr(castptr));
		}

		GpuProgramPtr(GpuProgram^ obj) : GpuProgram(obj->_native)
		{
			_sharedPtr = new Ogre::GpuProgramPtr(static_cast<Ogre::GpuProgram*>(obj->_native));
		}

		virtual bool Equals(Object^ obj) override
		{
			GpuProgramPtr^ clr = dynamic_cast<GpuProgramPtr^>(obj);
			if (clr == CLR_NULL)
			{
				return false;
			}

			return (_native == clr->_native);
		}
		bool Equals(GpuProgramPtr^ obj)
		{
			if (obj == CLR_NULL)
			{
				return false;
			}

			return (_native == obj->_native);
		}

		static bool operator == (GpuProgramPtr^ val1, GpuProgramPtr^ val2)
		{
			if ((Object^)val1 == (Object^)val2) return true;
			if ((Object^)val1 == nullptr || (Object^)val2 == nullptr) return false;
			return (val1->_native == val2->_native);
		}

		static bool operator != (GpuProgramPtr^ val1, GpuProgramPtr^ val2)
		{
			return !(val1 == val2);
		}

		virtual int GetHashCode() override
		{
			return reinterpret_cast<int>(_native);
		}

		property IntPtr NativePtr
		{
			IntPtr get() { return (IntPtr)_sharedPtr; }
		}

		property bool Unique
		{
			bool get()
			{
				return (*_sharedPtr).unique();
			}
		}

		property int UseCount
		{
			int get()
			{
				return (*_sharedPtr).useCount();
			}
		}

		property GpuProgram^ Target
		{
			GpuProgram^ get()
			{
				return ObjectTable::GetOrCreateObject<GpuProgram^>((intptr_t)static_cast<Ogre::GpuProgram*>(_native));
			}
		}
	};

	public ref class GpuProgramManager : public ResourceManager
	{
	public: 
		ref class SyntaxCodes;

		INC_DECLARE_STLSET(SyntaxCodes, String^, Ogre::String, public:, private:);

	private protected:
		static GpuProgramManager^ _singleton;

	public protected:
		GpuProgramManager(Ogre::GpuProgramManager* obj) : ResourceManager(obj)
		{
		}

	public:

		static property GpuProgramManager^ Singleton
		{
			GpuProgramManager^ get()
			{
				if (_singleton == CLR_NULL)
				{
					Ogre::GpuProgramManager* ptr = Ogre::GpuProgramManager::getSingletonPtr();
					if (ptr) _singleton = gcnew GpuProgramManager(ptr);
				}
				return _singleton;
			}
		}

		Mogre::GpuProgramPtr^ Load(String^ name, String^ groupName, String^ filename, Mogre::GpuProgramType gptype, String^ syntaxCode);

		Mogre::GpuProgramPtr^ LoadFromString(String^ name, String^ groupName, String^ code, Mogre::GpuProgramType gptype, String^ syntaxCode);

		Mogre::GpuProgramManager::Const_SyntaxCodes^ GetSupportedSyntax();

		bool IsSyntaxSupported(String^ syntaxCode);

		//Mogre::GpuProgramParametersSharedPtr^ CreateParameters();

		Mogre::GpuProgramPtr^ CreateProgram(String^ name, String^ groupName, String^ filename, Mogre::GpuProgramType gptype, String^ syntaxCode);

		Mogre::GpuProgramPtr^ CreateProgramFromString(String^ name, String^ groupName, String^ code, Mogre::GpuProgramType gptype, String^ syntaxCode);

		//Mogre::ResourcePtr^ Create(String^ name, String^ group, Mogre::GpuProgramType gptype, String^ syntaxCode, bool isManual, Mogre::IManualResourceLoader^ loader);
		Mogre::ResourcePtr^ Create(String^ name, String^ group, Mogre::GpuProgramType gptype, String^ syntaxCode, bool isManual);
		Mogre::ResourcePtr^ Create(String^ name, String^ group, Mogre::GpuProgramType gptype, String^ syntaxCode);

		//void _pushSyntaxCode(String^ syntaxCode);

		Mogre::GpuProgramPtr^ GetByName(String^ name, bool preferHighLevelPrograms);
		Mogre::GpuProgramPtr^ GetByName(String^ name);
	};
}