#pragma once

#include "OgreGpuProgramManager.h"
#include "OgreGpuProgram.h"
#include "MogreResourceManager.h"
#include "MogreResource.h"
#include "IteratorWrapper.h"
#include "Marshalling.h"

namespace Mogre
{
	ref class GpuConstantDefinitionMap;
	ref class GpuConstantDefinitionIterator;
	ref class GpuProgramParametersSharedPtr;
	value class GpuConstantDefinition_NativePtr;

	public enum class BaseConstantType
	{
		BCT_FLOAT = Ogre::BCT_FLOAT,
		BCT_INT = Ogre::BCT_INT,
		BCT_DOUBLE = Ogre::BCT_DOUBLE,
		BCT_UINT = Ogre::BCT_UINT,
		BCT_BOOL = Ogre::BCT_BOOL,
		BCT_SAMPLER = Ogre::BCT_SAMPLER,
		BCT_SUBROUTINE = Ogre::BCT_SUBROUTINE,
		BCT_UNKNOWN = Ogre::BCT_UNKNOWN,
	};

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
		GCT_SUBROUTINE = Ogre::GCT_SUBROUTINE,
		GCT_DOUBLE1 = Ogre::GCT_DOUBLE1,
		GCT_DOUBLE2 = Ogre::GCT_DOUBLE2,
		GCT_DOUBLE3 = Ogre::GCT_DOUBLE3,
		GCT_DOUBLE4 = Ogre::GCT_DOUBLE4,
		GCT_MATRIX_DOUBLE_2X2 = Ogre::GCT_MATRIX_DOUBLE_2X2,
		GCT_MATRIX_DOUBLE_2X3 = Ogre::GCT_MATRIX_DOUBLE_2X3,
		GCT_MATRIX_DOUBLE_2X4 = Ogre::GCT_MATRIX_DOUBLE_2X4,
		GCT_MATRIX_DOUBLE_3X2 = Ogre::GCT_MATRIX_DOUBLE_3X2,
		GCT_MATRIX_DOUBLE_3X3 = Ogre::GCT_MATRIX_DOUBLE_3X3,
		GCT_MATRIX_DOUBLE_3X4 = Ogre::GCT_MATRIX_DOUBLE_3X4,
		GCT_MATRIX_DOUBLE_4X2 = Ogre::GCT_MATRIX_DOUBLE_4X2,
		GCT_MATRIX_DOUBLE_4X3 = Ogre::GCT_MATRIX_DOUBLE_4X3,
		GCT_MATRIX_DOUBLE_4X4 = Ogre::GCT_MATRIX_DOUBLE_4X4,
		GCT_UINT1 = Ogre::GCT_UINT1,
		GCT_UINT2 = Ogre::GCT_UINT2,
		GCT_UINT3 = Ogre::GCT_UINT3,
		GCT_UINT4 = Ogre::GCT_UINT4,
		GCT_BOOL1 = Ogre::GCT_BOOL1,
		GCT_BOOL2 = Ogre::GCT_BOOL2,
		GCT_BOOL3 = Ogre::GCT_BOOL3,
		GCT_BOOL4 = Ogre::GCT_BOOL4,
		GCT_SAMPLER_WRAPPER1D = Ogre::GCT_SAMPLER_WRAPPER1D,
		GCT_SAMPLER_WRAPPER2D = Ogre::GCT_SAMPLER_WRAPPER2D,
		GCT_SAMPLER_WRAPPER3D = Ogre::GCT_SAMPLER_WRAPPER3D,
		GCT_SAMPLER_WRAPPERCUBE = Ogre::GCT_SAMPLER_WRAPPERCUBE,
		GCT_SAMPLER_STATE = Ogre::GCT_SAMPLER_STATE,
		GCT_UNKNOWN = Ogre::GCT_UNKNOWN
	};

	public enum class GpuParamVariability
	{
		GPV_GLOBAL = Ogre::GPV_GLOBAL,
		GPV_PER_OBJECT = Ogre::GPV_PER_OBJECT,
		GPV_LIGHTS = Ogre::GPV_LIGHTS,
		GPV_PASS_ITERATION_NUMBER = Ogre::GPV_PASS_ITERATION_NUMBER,
		GPV_ALL = Ogre::GPV_ALL
	};

	public enum class GpuProgramType
	{
		GPT_VERTEX_PROGRAM = Ogre::GPT_VERTEX_PROGRAM,
		GPT_FRAGMENT_PROGRAM = Ogre::GPT_FRAGMENT_PROGRAM,
		GPT_GEOMETRY_PROGRAM = Ogre::GPT_GEOMETRY_PROGRAM,
		GPT_HULL_PROGRAM = Ogre::GPT_GEOMETRY_PROGRAM,
		GPT_DOMAIN_PROGRAM = Ogre::GPT_GEOMETRY_PROGRAM,
		GPT_COMPUTE_PROGRAM = Ogre::GPT_GEOMETRY_PROGRAM
	};

	public value class GpuLogicalBufferStruct_NativePtr
	{
	private protected:
		Ogre::GpuLogicalBufferStruct* _native;

		//Public Declarations
	public:
		property size_t bufferSize
		{
		public:
			size_t get();
		public:
			void set(size_t value);
		}


		GpuLogicalBufferStruct_NativePtr(const Ogre::GpuLogicalBufferStructPtr& obj)
		{
			_native = obj.get();
		}

		DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_NATIVEPTRVALUECLASS(GpuLogicalBufferStruct_NativePtr, Ogre::GpuLogicalBufferStruct);


		property IntPtr NativePtr
		{
			IntPtr get() { return (IntPtr)_native; }
		}

		static GpuLogicalBufferStruct_NativePtr Create();

		void DestroyNativePtr()
		{
			if (_native) { delete _native; _native = 0; }
		}

		property bool IsNull
		{
			bool get() { return (_native == 0); }
		}
	};

	//################################################################
	//GpuNamedConstants_NativePtr
	//################################################################

	public value class GpuNamedConstants_NativePtr
	{
	private protected:
		Ogre::GpuNamedConstants* _native;

		//Public Declarations
	public:
		property size_t floatBufferSize
		{
		public:
			size_t get();
		public:
			void set(size_t value);
		}

		property size_t intBufferSize
		{
		public:
			size_t get();
		public:
			void set(size_t value);
		}

		property Mogre::GpuConstantDefinitionMap^ map
		{
		public:
			Mogre::GpuConstantDefinitionMap^ get();
		public:
			void set(Mogre::GpuConstantDefinitionMap^ value);
		}

		void GenerateConstantDefinitionArrayEntries(String^ paramName, Mogre::GpuConstantDefinition_NativePtr baseDef);

		DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_NATIVEPTRVALUECLASS(GpuNamedConstants_NativePtr, Ogre::GpuNamedConstants);


		property IntPtr NativePtr
		{
			IntPtr get() { return (IntPtr)_native; }
		}

		static GpuNamedConstants_NativePtr Create();

		void DestroyNativePtr()
		{
			if (_native) { delete _native; _native = 0; }
		}

		property bool IsNull
		{
			bool get() { return (_native == 0); }
		}
	};

	//################################################################
	//GpuConstantDefinition_NativePtr
	//################################################################

	public value class GpuConstantDefinition_NativePtr
	{
		//Nested Types

		//Private Declarations
	private protected:
		Ogre::GpuConstantDefinition* _native;

		//Internal Declarations
	public protected:

		//Public Declarations
	public:


		property Mogre::GpuConstantType constType
		{
		public:
			Mogre::GpuConstantType get();
		public:
			void set(Mogre::GpuConstantType value);
		}

		property size_t physicalIndex
		{
		public:
			size_t get();
		public:
			void set(size_t value);
		}

		property size_t elementSize
		{
		public:
			size_t get();
		public:
			void set(size_t value);
		}

		property size_t arraySize
		{
		public:
			size_t get();
		public:
			void set(size_t value);
		}

		property bool IsFloat
		{
		public:
			bool get();
		}

		property bool IsSampler
		{
		public:
			bool get();
		}

		DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_NATIVEPTRVALUECLASS(GpuConstantDefinition_NativePtr, Ogre::GpuConstantDefinition);


		property IntPtr NativePtr
		{
			IntPtr get() { return (IntPtr)_native; }
		}

		static GpuConstantDefinition_NativePtr Create();

		void DestroyNativePtr()
		{
			if (_native) { delete _native; _native = 0; }
		}

		property bool IsNull
		{
			bool get() { return (_native == 0); }
		}
	};

	public ref class GpuProgramParameters : IMogreDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	public:
		ref class AutoConstantList;
		ref class FloatConstantList;
		ref class IntConstantList;

		enum class AutoConstantType
		{
			ACT_WORLD_MATRIX = Ogre::GpuProgramParameters::ACT_WORLD_MATRIX,
			ACT_INVERSE_WORLD_MATRIX = Ogre::GpuProgramParameters::ACT_INVERSE_WORLD_MATRIX,
			ACT_TRANSPOSE_WORLD_MATRIX = Ogre::GpuProgramParameters::ACT_TRANSPOSE_WORLD_MATRIX,
			ACT_INVERSE_TRANSPOSE_WORLD_MATRIX = Ogre::GpuProgramParameters::ACT_INVERSE_TRANSPOSE_WORLD_MATRIX,
			ACT_WORLD_MATRIX_ARRAY_3x4 = Ogre::GpuProgramParameters::ACT_WORLD_MATRIX_ARRAY_3x4,
			ACT_WORLD_MATRIX_ARRAY = Ogre::GpuProgramParameters::ACT_WORLD_MATRIX_ARRAY,
			ACT_VIEW_MATRIX = Ogre::GpuProgramParameters::ACT_VIEW_MATRIX,
			ACT_INVERSE_VIEW_MATRIX = Ogre::GpuProgramParameters::ACT_INVERSE_VIEW_MATRIX,
			ACT_TRANSPOSE_VIEW_MATRIX = Ogre::GpuProgramParameters::ACT_TRANSPOSE_VIEW_MATRIX,
			ACT_INVERSE_TRANSPOSE_VIEW_MATRIX = Ogre::GpuProgramParameters::ACT_INVERSE_TRANSPOSE_VIEW_MATRIX,
			ACT_PROJECTION_MATRIX = Ogre::GpuProgramParameters::ACT_PROJECTION_MATRIX,
			ACT_INVERSE_PROJECTION_MATRIX = Ogre::GpuProgramParameters::ACT_INVERSE_PROJECTION_MATRIX,
			ACT_TRANSPOSE_PROJECTION_MATRIX = Ogre::GpuProgramParameters::ACT_TRANSPOSE_PROJECTION_MATRIX,
			ACT_INVERSE_TRANSPOSE_PROJECTION_MATRIX = Ogre::GpuProgramParameters::ACT_INVERSE_TRANSPOSE_PROJECTION_MATRIX,
			ACT_VIEWPROJ_MATRIX = Ogre::GpuProgramParameters::ACT_VIEWPROJ_MATRIX,
			ACT_INVERSE_VIEWPROJ_MATRIX = Ogre::GpuProgramParameters::ACT_INVERSE_VIEWPROJ_MATRIX,
			ACT_TRANSPOSE_VIEWPROJ_MATRIX = Ogre::GpuProgramParameters::ACT_TRANSPOSE_VIEWPROJ_MATRIX,
			ACT_INVERSE_TRANSPOSE_VIEWPROJ_MATRIX = Ogre::GpuProgramParameters::ACT_INVERSE_TRANSPOSE_VIEWPROJ_MATRIX,
			ACT_WORLDVIEW_MATRIX = Ogre::GpuProgramParameters::ACT_WORLDVIEW_MATRIX,
			ACT_INVERSE_WORLDVIEW_MATRIX = Ogre::GpuProgramParameters::ACT_INVERSE_WORLDVIEW_MATRIX,
			ACT_TRANSPOSE_WORLDVIEW_MATRIX = Ogre::GpuProgramParameters::ACT_TRANSPOSE_WORLDVIEW_MATRIX,
			ACT_INVERSE_TRANSPOSE_WORLDVIEW_MATRIX = Ogre::GpuProgramParameters::ACT_INVERSE_TRANSPOSE_WORLDVIEW_MATRIX,
			ACT_WORLDVIEWPROJ_MATRIX = Ogre::GpuProgramParameters::ACT_WORLDVIEWPROJ_MATRIX,
			ACT_INVERSE_WORLDVIEWPROJ_MATRIX = Ogre::GpuProgramParameters::ACT_INVERSE_WORLDVIEWPROJ_MATRIX,
			ACT_TRANSPOSE_WORLDVIEWPROJ_MATRIX = Ogre::GpuProgramParameters::ACT_TRANSPOSE_WORLDVIEWPROJ_MATRIX,
			ACT_INVERSE_TRANSPOSE_WORLDVIEWPROJ_MATRIX = Ogre::GpuProgramParameters::ACT_INVERSE_TRANSPOSE_WORLDVIEWPROJ_MATRIX,
			ACT_RENDER_TARGET_FLIPPING = Ogre::GpuProgramParameters::ACT_RENDER_TARGET_FLIPPING,
			ACT_FOG_COLOUR = Ogre::GpuProgramParameters::ACT_FOG_COLOUR,
			ACT_FOG_PARAMS = Ogre::GpuProgramParameters::ACT_FOG_PARAMS,
			ACT_SURFACE_AMBIENT_COLOUR = Ogre::GpuProgramParameters::ACT_SURFACE_AMBIENT_COLOUR,
			ACT_SURFACE_DIFFUSE_COLOUR = Ogre::GpuProgramParameters::ACT_SURFACE_DIFFUSE_COLOUR,
			ACT_SURFACE_SPECULAR_COLOUR = Ogre::GpuProgramParameters::ACT_SURFACE_SPECULAR_COLOUR,
			ACT_SURFACE_EMISSIVE_COLOUR = Ogre::GpuProgramParameters::ACT_SURFACE_EMISSIVE_COLOUR,
			ACT_SURFACE_SHININESS = Ogre::GpuProgramParameters::ACT_SURFACE_SHININESS,
			ACT_AMBIENT_LIGHT_COLOUR = Ogre::GpuProgramParameters::ACT_AMBIENT_LIGHT_COLOUR,
			ACT_LIGHT_DIFFUSE_COLOUR = Ogre::GpuProgramParameters::ACT_LIGHT_DIFFUSE_COLOUR,
			ACT_LIGHT_SPECULAR_COLOUR = Ogre::GpuProgramParameters::ACT_LIGHT_SPECULAR_COLOUR,
			ACT_LIGHT_ATTENUATION = Ogre::GpuProgramParameters::ACT_LIGHT_ATTENUATION,
			ACT_SPOTLIGHT_PARAMS = Ogre::GpuProgramParameters::ACT_SPOTLIGHT_PARAMS,
			ACT_LIGHT_POSITION = Ogre::GpuProgramParameters::ACT_LIGHT_POSITION,
			ACT_LIGHT_POSITION_OBJECT_SPACE = Ogre::GpuProgramParameters::ACT_LIGHT_POSITION_OBJECT_SPACE,
			ACT_LIGHT_POSITION_VIEW_SPACE = Ogre::GpuProgramParameters::ACT_LIGHT_POSITION_VIEW_SPACE,
			ACT_LIGHT_DIRECTION = Ogre::GpuProgramParameters::ACT_LIGHT_DIRECTION,
			ACT_LIGHT_DIRECTION_OBJECT_SPACE = Ogre::GpuProgramParameters::ACT_LIGHT_DIRECTION_OBJECT_SPACE,
			ACT_LIGHT_DIRECTION_VIEW_SPACE = Ogre::GpuProgramParameters::ACT_LIGHT_DIRECTION_VIEW_SPACE,
			ACT_LIGHT_DISTANCE_OBJECT_SPACE = Ogre::GpuProgramParameters::ACT_LIGHT_DISTANCE_OBJECT_SPACE,
			ACT_LIGHT_POWER_SCALE = Ogre::GpuProgramParameters::ACT_LIGHT_POWER_SCALE,
			ACT_LIGHT_DIFFUSE_COLOUR_ARRAY = Ogre::GpuProgramParameters::ACT_LIGHT_DIFFUSE_COLOUR_ARRAY,
			ACT_LIGHT_SPECULAR_COLOUR_ARRAY = Ogre::GpuProgramParameters::ACT_LIGHT_SPECULAR_COLOUR_ARRAY,
			ACT_LIGHT_ATTENUATION_ARRAY = Ogre::GpuProgramParameters::ACT_LIGHT_ATTENUATION_ARRAY,
			ACT_LIGHT_POSITION_ARRAY = Ogre::GpuProgramParameters::ACT_LIGHT_POSITION_ARRAY,
			ACT_LIGHT_POSITION_OBJECT_SPACE_ARRAY = Ogre::GpuProgramParameters::ACT_LIGHT_POSITION_OBJECT_SPACE_ARRAY,
			ACT_LIGHT_POSITION_VIEW_SPACE_ARRAY = Ogre::GpuProgramParameters::ACT_LIGHT_POSITION_VIEW_SPACE_ARRAY,
			ACT_LIGHT_DIRECTION_ARRAY = Ogre::GpuProgramParameters::ACT_LIGHT_DIRECTION_ARRAY,
			ACT_LIGHT_DIRECTION_OBJECT_SPACE_ARRAY = Ogre::GpuProgramParameters::ACT_LIGHT_DIRECTION_OBJECT_SPACE_ARRAY,
			ACT_LIGHT_DIRECTION_VIEW_SPACE_ARRAY = Ogre::GpuProgramParameters::ACT_LIGHT_DIRECTION_VIEW_SPACE_ARRAY,
			ACT_LIGHT_DISTANCE_OBJECT_SPACE_ARRAY = Ogre::GpuProgramParameters::ACT_LIGHT_DISTANCE_OBJECT_SPACE_ARRAY,
			ACT_LIGHT_POWER_SCALE_ARRAY = Ogre::GpuProgramParameters::ACT_LIGHT_POWER_SCALE_ARRAY,
			ACT_SPOTLIGHT_PARAMS_ARRAY = Ogre::GpuProgramParameters::ACT_SPOTLIGHT_PARAMS_ARRAY,
			ACT_DERIVED_AMBIENT_LIGHT_COLOUR = Ogre::GpuProgramParameters::ACT_DERIVED_AMBIENT_LIGHT_COLOUR,
			ACT_DERIVED_SCENE_COLOUR = Ogre::GpuProgramParameters::ACT_DERIVED_SCENE_COLOUR,
			ACT_DERIVED_LIGHT_DIFFUSE_COLOUR = Ogre::GpuProgramParameters::ACT_DERIVED_LIGHT_DIFFUSE_COLOUR,
			ACT_DERIVED_LIGHT_SPECULAR_COLOUR = Ogre::GpuProgramParameters::ACT_DERIVED_LIGHT_SPECULAR_COLOUR,
			ACT_DERIVED_LIGHT_DIFFUSE_COLOUR_ARRAY = Ogre::GpuProgramParameters::ACT_DERIVED_LIGHT_DIFFUSE_COLOUR_ARRAY,
			ACT_DERIVED_LIGHT_SPECULAR_COLOUR_ARRAY = Ogre::GpuProgramParameters::ACT_DERIVED_LIGHT_SPECULAR_COLOUR_ARRAY,
			ACT_SHADOW_EXTRUSION_DISTANCE = Ogre::GpuProgramParameters::ACT_SHADOW_EXTRUSION_DISTANCE,
			ACT_CAMERA_POSITION = Ogre::GpuProgramParameters::ACT_CAMERA_POSITION,
			ACT_CAMERA_POSITION_OBJECT_SPACE = Ogre::GpuProgramParameters::ACT_CAMERA_POSITION_OBJECT_SPACE,
			ACT_TEXTURE_VIEWPROJ_MATRIX = Ogre::GpuProgramParameters::ACT_TEXTURE_VIEWPROJ_MATRIX,
			ACT_CUSTOM = Ogre::GpuProgramParameters::ACT_CUSTOM,
			ACT_TIME = Ogre::GpuProgramParameters::ACT_TIME,
			ACT_TIME_0_X = Ogre::GpuProgramParameters::ACT_TIME_0_X,
			ACT_COSTIME_0_X = Ogre::GpuProgramParameters::ACT_COSTIME_0_X,
			ACT_SINTIME_0_X = Ogre::GpuProgramParameters::ACT_SINTIME_0_X,
			ACT_TANTIME_0_X = Ogre::GpuProgramParameters::ACT_TANTIME_0_X,
			ACT_TIME_0_X_PACKED = Ogre::GpuProgramParameters::ACT_TIME_0_X_PACKED,
			ACT_TIME_0_1 = Ogre::GpuProgramParameters::ACT_TIME_0_1,
			ACT_COSTIME_0_1 = Ogre::GpuProgramParameters::ACT_COSTIME_0_1,
			ACT_SINTIME_0_1 = Ogre::GpuProgramParameters::ACT_SINTIME_0_1,
			ACT_TANTIME_0_1 = Ogre::GpuProgramParameters::ACT_TANTIME_0_1,
			ACT_TIME_0_1_PACKED = Ogre::GpuProgramParameters::ACT_TIME_0_1_PACKED,
			ACT_TIME_0_2PI = Ogre::GpuProgramParameters::ACT_TIME_0_2PI,
			ACT_COSTIME_0_2PI = Ogre::GpuProgramParameters::ACT_COSTIME_0_2PI,
			ACT_SINTIME_0_2PI = Ogre::GpuProgramParameters::ACT_SINTIME_0_2PI,
			ACT_TANTIME_0_2PI = Ogre::GpuProgramParameters::ACT_TANTIME_0_2PI,
			ACT_TIME_0_2PI_PACKED = Ogre::GpuProgramParameters::ACT_TIME_0_2PI_PACKED,
			ACT_FRAME_TIME = Ogre::GpuProgramParameters::ACT_FRAME_TIME,
			ACT_FPS = Ogre::GpuProgramParameters::ACT_FPS,
			ACT_VIEWPORT_WIDTH = Ogre::GpuProgramParameters::ACT_VIEWPORT_WIDTH,
			ACT_VIEWPORT_HEIGHT = Ogre::GpuProgramParameters::ACT_VIEWPORT_HEIGHT,
			ACT_INVERSE_VIEWPORT_WIDTH = Ogre::GpuProgramParameters::ACT_INVERSE_VIEWPORT_WIDTH,
			ACT_INVERSE_VIEWPORT_HEIGHT = Ogre::GpuProgramParameters::ACT_INVERSE_VIEWPORT_HEIGHT,
			ACT_VIEWPORT_SIZE = Ogre::GpuProgramParameters::ACT_VIEWPORT_SIZE,
			ACT_VIEW_DIRECTION = Ogre::GpuProgramParameters::ACT_VIEW_DIRECTION,
			ACT_VIEW_SIDE_VECTOR = Ogre::GpuProgramParameters::ACT_VIEW_SIDE_VECTOR,
			ACT_VIEW_UP_VECTOR = Ogre::GpuProgramParameters::ACT_VIEW_UP_VECTOR,
			ACT_FOV = Ogre::GpuProgramParameters::ACT_FOV,
			ACT_NEAR_CLIP_DISTANCE = Ogre::GpuProgramParameters::ACT_NEAR_CLIP_DISTANCE,
			ACT_FAR_CLIP_DISTANCE = Ogre::GpuProgramParameters::ACT_FAR_CLIP_DISTANCE,
			ACT_PASS_NUMBER = Ogre::GpuProgramParameters::ACT_PASS_NUMBER,
			ACT_PASS_ITERATION_NUMBER = Ogre::GpuProgramParameters::ACT_PASS_ITERATION_NUMBER,
			ACT_ANIMATION_PARAMETRIC = Ogre::GpuProgramParameters::ACT_ANIMATION_PARAMETRIC,
			ACT_TEXEL_OFFSETS = Ogre::GpuProgramParameters::ACT_TEXEL_OFFSETS,
			ACT_SCENE_DEPTH_RANGE = Ogre::GpuProgramParameters::ACT_SCENE_DEPTH_RANGE,
			ACT_SHADOW_SCENE_DEPTH_RANGE = Ogre::GpuProgramParameters::ACT_SHADOW_SCENE_DEPTH_RANGE,
			ACT_TEXTURE_SIZE = Ogre::GpuProgramParameters::ACT_TEXTURE_SIZE,
			ACT_INVERSE_TEXTURE_SIZE = Ogre::GpuProgramParameters::ACT_INVERSE_TEXTURE_SIZE,
			ACT_PACKED_TEXTURE_SIZE = Ogre::GpuProgramParameters::ACT_PACKED_TEXTURE_SIZE
		};

		enum class ACDataType
		{
			ACDT_NONE = Ogre::GpuProgramParameters::ACDT_NONE,
			ACDT_INT = Ogre::GpuProgramParameters::ACDT_INT,
			ACDT_REAL = Ogre::GpuProgramParameters::ACDT_REAL
		};

		enum class ElementType
		{
			ET_INT = Ogre::GpuProgramParameters::ET_INT,
			ET_REAL = Ogre::GpuProgramParameters::ET_REAL
		};

		//################################################################
		//AutoConstantDefinition_NativePtr
		//################################################################

		value class AutoConstantDefinition_NativePtr
		{
		private protected:
			Ogre::GpuProgramParameters::AutoConstantDefinition* _native;


		public:

			property Mogre::GpuProgramParameters::AutoConstantType acType
			{
			public:
				Mogre::GpuProgramParameters::AutoConstantType get();
			public:
				void set(Mogre::GpuProgramParameters::AutoConstantType value);
			}

			property String^ name
			{
			public:
				String^ get();
			public:
				void set(String^ value);
			}

			property size_t elementCount
			{
			public:
				size_t get();
			public:
				void set(size_t value);
			}

			property Mogre::GpuProgramParameters::ElementType elementType
			{
			public:
				Mogre::GpuProgramParameters::ElementType get();
			public:
				void set(Mogre::GpuProgramParameters::ElementType value);
			}

			property Mogre::GpuProgramParameters::ACDataType dataType
			{
			public:
				Mogre::GpuProgramParameters::ACDataType get();
			public:
				void set(Mogre::GpuProgramParameters::ACDataType value);
			}

			DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_NATIVEPTRVALUECLASS(GpuProgramParameters::AutoConstantDefinition_NativePtr, Ogre::GpuProgramParameters::AutoConstantDefinition);

			property IntPtr NativePtr
			{
				IntPtr get() { return (IntPtr)_native; }
			}

			static AutoConstantDefinition_NativePtr Create(Mogre::GpuProgramParameters::AutoConstantType _acType, String^ _name, size_t _elementCount, Mogre::GpuProgramParameters::ElementType _elementType, Mogre::GpuProgramParameters::ACDataType _dataType);

			void DestroyNativePtr()
			{
				if (_native) { delete _native; _native = 0; }
			}

			property bool IsNull
			{
				bool get() { return (_native == 0); }
			}
		};

		//################################################################
		//AutoConstantEntry_NativePtr
		//################################################################

		value class AutoConstantEntry_NativePtr
		{
		private protected:
			Ogre::GpuProgramParameters::AutoConstantEntry* _native;

			//Public Declarations
		public:
			property Mogre::GpuProgramParameters::AutoConstantType paramType
			{
			public:
				Mogre::GpuProgramParameters::AutoConstantType get();
			public:
				void set(Mogre::GpuProgramParameters::AutoConstantType value);
			}

			property size_t physicalIndex
			{
			public:
				size_t get();
			public:
				void set(size_t value);
			}

			property size_t elementCount
			{
			public:
				size_t get();
			public:
				void set(size_t value);
			}

			property size_t data
			{
			public:
				size_t get();
			public:
				void set(size_t value);
			}

			property Mogre::Real fData
			{
			public:
				Mogre::Real get();
			public:
				void set(Mogre::Real value);
			}

			DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_NATIVEPTRVALUECLASS(GpuProgramParameters::AutoConstantEntry_NativePtr, Ogre::GpuProgramParameters::AutoConstantEntry);

			property IntPtr NativePtr
			{
				IntPtr get() { return (IntPtr)_native; }
			}

			static AutoConstantEntry_NativePtr Create(Mogre::GpuProgramParameters::AutoConstantType theType, size_t theIndex, size_t theData, size_t theElemCount, Ogre::uint16 theVariability);
			static AutoConstantEntry_NativePtr Create(Mogre::GpuProgramParameters::AutoConstantType theType, size_t theIndex, size_t theData, Ogre::uint16 theVariability);
			static AutoConstantEntry_NativePtr Create(Mogre::GpuProgramParameters::AutoConstantType theType, size_t theIndex, Mogre::Real theData, size_t theElemCount, Ogre::uint16 theVariability);
			static AutoConstantEntry_NativePtr Create(Mogre::GpuProgramParameters::AutoConstantType theType, size_t theIndex, Mogre::Real theData, Ogre::uint16 theVariability);

			void DestroyNativePtr()
			{
				if (_native) { delete _native; _native = 0; }
			}

			property bool IsNull
			{
				bool get() { return (_native == 0); }
			}
		};

	public:
		INC_DECLARE_STLVECTOR(AutoConstantList, Mogre::GpuProgramParameters::AutoConstantEntry_NativePtr, Ogre::GpuProgramParameters::AutoConstantEntry, public:, private:);
		INC_DECLARE_STLVECTOR(FloatConstantList, float, float, public:, private:);
		INC_DECLARE_STLVECTOR(IntConstantList, int, int, public:, private:);
		INC_DECLARE_ITERATOR(AutoConstantIterator, Ogre::GpuProgramParameters::AutoConstantIterator, Mogre::GpuProgramParameters::AutoConstantList, Mogre::GpuProgramParameters::AutoConstantEntry_NativePtr, Ogre::GpuProgramParameters::AutoConstantEntry);

	private protected:

		virtual void ClearNativePtr()// = INativePointer::ClearNativePtr
		{
			_native = 0;
		}

	public protected:
		GpuProgramParameters(Ogre::GpuProgramParameters* obj) : _native(obj), _createdByCLR(false)
		{
		}

		Ogre::GpuProgramParameters* _native;
		bool _createdByCLR;


	public:
		~GpuProgramParameters();
	protected:
		!GpuProgramParameters();

	public:
		GpuProgramParameters();
		GpuProgramParameters(Mogre::GpuProgramParameters^ oth);

		property bool IsDisposed
		{
			virtual bool get()
			{
				return (_native == nullptr);
			}
		}

		property size_t AutoConstantCount
		{
		public:
			size_t get();
		}

		property Mogre::GpuNamedConstants_NativePtr ConstantDefinitions
		{
		public:
			Mogre::GpuNamedConstants_NativePtr get();
		}

		property Mogre::GpuLogicalBufferStruct_NativePtr FloatLogicalBufferStruct
		{
		public:
			Mogre::GpuLogicalBufferStruct_NativePtr get();
		}

		property bool HasAutoConstants
		{
		public:
			bool get();
		}

		property bool HasLogicalIndexedParameters
		{
		public:
			bool get();
		}

		property bool HasNamedParameters
		{
		public:
			bool get();
		}

		property bool HasPassIterationNumber
		{
		public:
			bool get();
		}

		property Mogre::GpuLogicalBufferStruct_NativePtr IntLogicalBufferStruct
		{
		public:
			Mogre::GpuLogicalBufferStruct_NativePtr get();
		}

		property size_t NumAutoConstantDefinitions
		{
		public:
			static size_t get();
		}

		property size_t PassIterationNumberIndex
		{
		public:
			size_t get();
		}

		property bool TransposeMatrices
		{
		public:
			bool get();
		public:
			void set(bool val);
		}

		void CopyTo(GpuProgramParameters^ dest)
		{
			if (_native == NULL) throw gcnew Exception("The underlying native object for the caller is null.");
			if (dest->_native == NULL) throw gcnew ArgumentException("The underlying native object for parameter 'dest' is null.");

			*(dest->_native) = *_native;
		}

		//void _setNamedConstants(Mogre::GpuNamedConstants_NativePtr constantmap);

		void SetConstant(size_t index, Mogre::Vector4 vec);

		void SetConstant(size_t index, Mogre::Real val);

		void SetConstant(size_t index, Mogre::Vector3 vec);

		void SetConstant(size_t index, Mogre::Matrix4 m);

		void SetConstant(size_t index, const Mogre::Matrix4* m, size_t numEntries);

		void SetConstant(size_t index, const float* val, size_t count);

		void SetConstant(size_t index, const double* val, size_t count);

		void SetConstant(size_t index, Mogre::ColourValue colour);

		void SetConstant(size_t index, const int* val, size_t count);

		void _writeRawConstants(size_t physicalIndex, const float* val, size_t count);

		void _writeRawConstants(size_t physicalIndex, const double* val, size_t count);

		void _writeRawConstants(size_t physicalIndex, const int* val, size_t count);

		void _readRawConstants(size_t physicalIndex, size_t count, [Out] float% dest);

		void _readRawConstants(size_t physicalIndex, size_t count, [Out] int% dest);

		void _writeRawConstant(size_t physicalIndex, Mogre::Vector4 vec, size_t count);
		void _writeRawConstant(size_t physicalIndex, Mogre::Vector4 vec);

		void _writeRawConstant(size_t physicalIndex, Mogre::Real val);

		void _writeRawConstant(size_t physicalIndex, int val);

		void _writeRawConstant(size_t physicalIndex, Mogre::Vector3 vec);

		void _writeRawConstant(size_t physicalIndex, const Mogre::Matrix4* m, size_t numEntries);

		void _writeRawConstant(size_t physicalIndex, Mogre::ColourValue colour, size_t count);
		void _writeRawConstant(size_t physicalIndex, Mogre::ColourValue colour);

		Mogre::GpuConstantDefinitionIterator^ GetConstantDefinitionIterator();

		Mogre::GpuConstantDefinition_NativePtr GetConstantDefinition(String^ name);

		size_t GetFloatLogicalIndexForPhysicalIndex(size_t physicalIndex);

		size_t GetIntLogicalIndexForPhysicalIndex(size_t physicalIndex);

		Mogre::GpuProgramParameters::Const_FloatConstantList^ GetFloatConstantList();

		float* GetFloatPointer(size_t pos);

		Mogre::GpuProgramParameters::Const_IntConstantList^ GetIntConstantList();

		int* GetIntPointer(size_t pos);

		Mogre::GpuProgramParameters::Const_AutoConstantList^ GetAutoConstantList();

		void SetAutoConstant(size_t index, Mogre::GpuProgramParameters::AutoConstantType acType, size_t extraInfo);
		void SetAutoConstant(size_t index, Mogre::GpuProgramParameters::AutoConstantType acType);

		void SetAutoConstantReal(size_t index, Mogre::GpuProgramParameters::AutoConstantType acType, Mogre::Real rData);

		//void _setRawAutoConstant(size_t physicalIndex, Mogre::GpuProgramParameters::AutoConstantType acType, size_t extraInfo, size_t elementSize);
		//void _setRawAutoConstant(size_t physicalIndex, Mogre::GpuProgramParameters::AutoConstantType acType, size_t extraInfo);

		//void _setRawAutoConstantReal(size_t physicalIndex, Mogre::GpuProgramParameters::AutoConstantType acType, Mogre::Real rData, size_t elementSize);
		//void _setRawAutoConstantReal(size_t physicalIndex, Mogre::GpuProgramParameters::AutoConstantType acType, Mogre::Real rData);

		void ClearAutoConstant(size_t index);

		void SetConstantFromTime(size_t index, Mogre::Real factor);

		void ClearAutoConstants();

		Mogre::GpuProgramParameters::AutoConstantIterator^ GetAutoConstantIterator();

		Mogre::GpuProgramParameters::AutoConstantEntry_NativePtr GetAutoConstantEntry(size_t index);

		Mogre::GpuProgramParameters::AutoConstantEntry_NativePtr FindFloatAutoConstantEntry(size_t logicalIndex);

		Mogre::GpuProgramParameters::AutoConstantEntry_NativePtr FindIntAutoConstantEntry(size_t logicalIndex);

		Mogre::GpuProgramParameters::AutoConstantEntry_NativePtr FindAutoConstantEntry(String^ paramName);

		Mogre::GpuProgramParameters::AutoConstantEntry_NativePtr _findRawAutoConstantEntryFloat(size_t physicalIndex);

		Mogre::GpuProgramParameters::AutoConstantEntry_NativePtr _findRawAutoConstantEntryInt(size_t physicalIndex);

		void SetIgnoreMissingParams(bool state);

		void SetNamedConstant(String^ name, Mogre::Real val);

		void SetNamedConstant(String^ name, int val);

		void SetNamedConstant(String^ name, Mogre::Vector4 vec);

		void SetNamedConstant(String^ name, Mogre::Vector3 vec);

		void SetNamedConstant(String^ name, Mogre::Matrix4 m);

		void SetNamedConstant(String^ name, const Mogre::Matrix4* m, size_t numEntries);

		void SetNamedConstant(String^ name, const float* val, size_t count, size_t multiple);
		void SetNamedConstant(String^ name, const float* val, size_t count);

		void SetNamedConstant(String^ name, const double* val, size_t count, size_t multiple);
		void SetNamedConstant(String^ name, const double* val, size_t count);

		void SetNamedConstant(String^ name, Mogre::ColourValue colour);

		void SetNamedConstant(String^ name, const int* val, size_t count, size_t multiple);
		void SetNamedConstant(String^ name, const int* val, size_t count);

		void SetNamedAutoConstant(String^ name, Mogre::GpuProgramParameters::AutoConstantType acType, size_t extraInfo);
		void SetNamedAutoConstant(String^ name, Mogre::GpuProgramParameters::AutoConstantType acType);

		void SetNamedAutoConstantReal(String^ name, Mogre::GpuProgramParameters::AutoConstantType acType, Mogre::Real rData);

		void SetNamedConstantFromTime(String^ name, Mogre::Real factor);

		void ClearNamedAutoConstant(String^ name);

		Mogre::GpuConstantDefinition_NativePtr _findNamedConstantDefinition(String^ name, bool throwExceptionIfMissing);
		Mogre::GpuConstantDefinition_NativePtr _findNamedConstantDefinition(String^ name);

		size_t _getFloatConstantPhysicalIndex(size_t logicalIndex, size_t requestedSize, Ogre::uint16 variability);

		size_t _getIntConstantPhysicalIndex(size_t logicalIndex, size_t requestedSize, Ogre::uint16 variability);

		void CopyConstantsFrom(Mogre::GpuProgramParameters^ source);

		void IncPassIterationNumber();

		static Mogre::GpuProgramParameters::AutoConstantDefinition_NativePtr GetAutoConstantDefinition(String^ name);

		static Mogre::GpuProgramParameters::AutoConstantDefinition_NativePtr GetAutoConstantDefinition(size_t idx);

		//DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_CLRHANDLE(GpuProgramParameters);
	};

	public ref class GpuProgram : public Resource
	{
	public protected:
		GpuProgram(Ogre::GpuProgram* obj) : Resource(obj)
		{
		}

		GpuProgram(intptr_t ptr) : Resource(ptr)
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

		Mogre::GpuProgramParametersSharedPtr^ CreateParameters();

		void SetSkeletalAnimationIncluded(bool included);

		void SetMorphAnimationIncluded(bool included);

		void SetPoseAnimationIncluded(Ogre::ushort poseCount);

		void SetVertexTextureFetchRequired(bool r);

		Mogre::GpuProgramParametersSharedPtr^ GetDefaultParameters();

		void ResetCompileError();

		DEFINE_MANAGED_NATIVE_CONVERSIONS(GpuProgram);
	};

	public ref class GpuProgramPtr : public GpuProgram
	{
	public protected:
		Ogre::GpuProgramPtr* _sharedPtr;

		GpuProgramPtr(Ogre::GpuProgramPtr& sharedPtr) : GpuProgram(sharedPtr.getPointer())
		{
			_sharedPtr = new Ogre::GpuProgramPtr(sharedPtr);
		}

		!GpuProgramPtr()
		{
			if (_sharedPtr != 0)
			{
				if (_sharedPtr->useCount() > 1)
				{
					delete _sharedPtr;
				}
				_sharedPtr = 0;
			}
		}

		~GpuProgramPtr()
		{
			this->!GpuProgramPtr();
		}

	public:
		static operator GpuProgramPtr ^ (const Ogre::GpuProgramPtr& ptr)
		{
			if (ptr.isNull()) return nullptr;
			Ogre::GpuProgramPtr wrapperPtr = Ogre::GpuProgramPtr(ptr);
			wrapperPtr.setUseCount(wrapperPtr.useCount() + 1);
			return gcnew GpuProgramPtr(wrapperPtr);
		}

		static operator Ogre::GpuProgramPtr& (GpuProgramPtr^ t)
		{
			if (CLR_NULL == t) return Ogre::GpuProgramPtr();
			return *(t->_sharedPtr);
		}

		static operator Ogre::GpuProgramPtr* (GpuProgramPtr^ t)
		{
			if (CLR_NULL == t) return nullptr;
			return t->_sharedPtr;
		}

		static GpuProgramPtr^ FromResourcePtr(ResourcePtr^ ptr)
		{
			if (CLR_NULL == ptr) return nullptr;
			void* castptr = dynamic_cast<Ogre::GpuProgram*>(ptr->_native);
			if (castptr == 0) throw gcnew InvalidCastException("The underlying type of the ResourcePtr object is not of type GpuProgram.");
			Ogre::GpuProgramPtr gpuProgramPtr = ptr->_sharedPtr->staticCast<Ogre::GpuProgram>();
			return gcnew GpuProgramPtr(gpuProgramPtr);
		}

		static operator GpuProgramPtr ^ (ResourcePtr^ ptr)
		{
			GpuProgramPtr^ res = FromResourcePtr(ptr);
			// invalidate previous pointer and return converted pointer
			delete ptr;
			return res;
		}

		GpuProgramPtr(GpuProgram^ obj) : GpuProgram(static_cast<Ogre::GpuProgram*>(obj->_native))
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

	INC_DECLARE_STLMAP(GpuConstantDefinitionMap, String^, Mogre::GpuConstantDefinition_NativePtr, Ogre::String, Ogre::GpuConstantDefinition, public, private);
	public INC_DECLARE_MAP_ITERATOR(GpuConstantDefinitionIterator, Ogre::GpuConstantDefinitionIterator, Mogre::GpuConstantDefinitionMap, Mogre::GpuConstantDefinition_NativePtr, Ogre::GpuConstantDefinition, String^, Ogre::String);

	public ref class GpuProgramParametersSharedPtr : public GpuProgramParameters
	{
	public protected:
		Ogre::GpuProgramParametersSharedPtr* _sharedPtr;

		GpuProgramParametersSharedPtr(Ogre::GpuProgramParametersSharedPtr& sharedPtr) : GpuProgramParameters(sharedPtr.getPointer())
		{
			_sharedPtr = new Ogre::GpuProgramParametersSharedPtr(sharedPtr);
		}

		!GpuProgramParametersSharedPtr()
		{
			if (_sharedPtr != 0)
			{
				delete _sharedPtr;
				_sharedPtr = 0;
			}
		}

		~GpuProgramParametersSharedPtr()
		{
			this->!GpuProgramParametersSharedPtr();
		}

	public:
		static operator GpuProgramParametersSharedPtr ^ (const Ogre::GpuProgramParametersSharedPtr& ptr)
		{
			if (ptr.isNull()) return nullptr;
			return gcnew GpuProgramParametersSharedPtr(*(new Ogre::GpuProgramParametersSharedPtr(ptr)));
		}

		static operator Ogre::GpuProgramParametersSharedPtr& (GpuProgramParametersSharedPtr^ t)
		{
			if (CLR_NULL == t) return Ogre::GpuProgramParametersSharedPtr();
			return *(t->_sharedPtr);
		}

		static operator Ogre::GpuProgramParametersSharedPtr* (GpuProgramParametersSharedPtr^ t)
		{
			if (CLR_NULL == t) return nullptr;
			return t->_sharedPtr;
		}

		GpuProgramParametersSharedPtr(GpuProgramParameters^ obj) : GpuProgramParameters(obj->_native)
		{
			_sharedPtr = new Ogre::GpuProgramParametersSharedPtr(static_cast<Ogre::GpuProgramParameters*>(obj->_native));
		}

		virtual bool Equals(Object^ obj) override
		{
			GpuProgramParametersSharedPtr^ clr = dynamic_cast<GpuProgramParametersSharedPtr^>(obj);
			if (clr == CLR_NULL)
			{
				return false;
			}

			return (_native == clr->_native);
		}
		bool Equals(GpuProgramParametersSharedPtr^ obj)
		{
			if (obj == CLR_NULL)
			{
				return false;
			}

			return (_native == obj->_native);
		}

		static bool operator == (GpuProgramParametersSharedPtr^ val1, GpuProgramParametersSharedPtr^ val2)
		{
			if ((Object^)val1 == (Object^)val2) return true;
			if ((Object^)val1 == nullptr || (Object^)val2 == nullptr) return false;
			return (val1->_native == val2->_native);
		}

		static bool operator != (GpuProgramParametersSharedPtr^ val1, GpuProgramParametersSharedPtr^ val2)
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

		property GpuProgramParameters^ Target
		{
			GpuProgramParameters^ get()
			{
				return ObjectTable::GetOrCreateObject<GpuProgramParameters^>((intptr_t) static_cast<Ogre::GpuProgramParameters*>(_native));
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

		Mogre::GpuProgramParametersSharedPtr^ CreateParameters();

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