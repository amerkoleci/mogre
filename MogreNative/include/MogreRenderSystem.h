#pragma once

#include "OgreRenderSystem.h"
#include "MogreHardwareBuffer.h"
#include "MogreMaterialManager.h"
#include "MogreGpuProgramManager.h"
#include "MogreCommon.h"
#include "STLContainerWrappers.h"
#include "Marshalling.h"

namespace Mogre
{
	ref class ConfigOptionMap;
	ref class TexturePtr;
	ref class RenderTarget;
	ref class Frustum;
	ref class RenderOperation;

	public enum class StencilOperation
	{
		SOP_KEEP = Ogre::SOP_KEEP,
		SOP_ZERO = Ogre::SOP_ZERO,
		SOP_REPLACE = Ogre::SOP_REPLACE,
		SOP_INCREMENT = Ogre::SOP_INCREMENT,
		SOP_DECREMENT = Ogre::SOP_DECREMENT,
		SOP_INCREMENT_WRAP = Ogre::SOP_INCREMENT_WRAP,
		SOP_DECREMENT_WRAP = Ogre::SOP_DECREMENT_WRAP,
		SOP_INVERT = Ogre::SOP_INVERT,
		Keep = Ogre::SOP_KEEP,
		Zero = Ogre::SOP_ZERO,
		Replace = Ogre::SOP_REPLACE,
		Increment = Ogre::SOP_INCREMENT,
		Decrement = Ogre::SOP_DECREMENT,
		IncrementWrap = Ogre::SOP_INCREMENT_WRAP,
		DecrementWrap = Ogre::SOP_DECREMENT_WRAP,
		Invert = Ogre::SOP_INVERT
	};

	public enum class TexCoordCalcMethod
	{
		TEXCALC_NONE = Ogre::TEXCALC_NONE,
		TEXCALC_ENVIRONMENT_MAP = Ogre::TEXCALC_ENVIRONMENT_MAP,
		TEXCALC_ENVIRONMENT_MAP_PLANAR = Ogre::TEXCALC_ENVIRONMENT_MAP_PLANAR,
		TEXCALC_ENVIRONMENT_MAP_REFLECTION = Ogre::TEXCALC_ENVIRONMENT_MAP_REFLECTION,
		TEXCALC_ENVIRONMENT_MAP_NORMAL = Ogre::TEXCALC_ENVIRONMENT_MAP_NORMAL,
		TEXCALC_PROJECTIVE_TEXTURE = Ogre::TEXCALC_PROJECTIVE_TEXTURE,
		None = Ogre::TEXCALC_NONE,
		EnvironmentMap = Ogre::TEXCALC_ENVIRONMENT_MAP,
		EnvironmentMapPlanar = Ogre::TEXCALC_ENVIRONMENT_MAP_PLANAR,
		EnvironmentMapReflection = Ogre::TEXCALC_ENVIRONMENT_MAP_REFLECTION,
		EnvironmentMapNormal = Ogre::TEXCALC_ENVIRONMENT_MAP_NORMAL,
		ProjectiveTexture = Ogre::TEXCALC_PROJECTIVE_TEXTURE
	};

	interface class IRenderSystem_Listener_Receiver
	{
		void EventOccurred(String^ eventName/*, Mogre::Const_NameValuePairList^ parameters*/);

	};

	//################################################################
	//Listener
	//################################################################

	class RenderSystem_Listener_Director : public Ogre::RenderSystem::Listener
	{
	private:
		gcroot<IRenderSystem_Listener_Receiver^> _receiver;
	public:
		RenderSystem_Listener_Director(IRenderSystem_Listener_Receiver^ recv)
			: _receiver(recv), doCallForEventOccurred(false)
		{
		}

		bool doCallForEventOccurred;

		virtual void eventOccurred(const Ogre::String& eventName, const Ogre::NameValuePairList* parameters) override;
	};

	public enum class Capabilities
	{
		RSC_AUTOMIPMAP = Ogre::RSC_AUTOMIPMAP,
		RSC_BLENDING = Ogre::RSC_BLENDING,
		RSC_ANISOTROPY = Ogre::RSC_ANISOTROPY,
		RSC_DOT3 = Ogre::RSC_DOT3,
		RSC_CUBEMAPPING = Ogre::RSC_CUBEMAPPING,
		RSC_HWSTENCIL = Ogre::RSC_HWSTENCIL,
		RSC_VBO = Ogre::RSC_VBO,
		RSC_32BIT_INDEX = Ogre::RSC_32BIT_INDEX,
		RSC_VERTEX_PROGRAM = Ogre::RSC_VERTEX_PROGRAM,
		RSC_FRAGMENT_PROGRAM = Ogre::RSC_FRAGMENT_PROGRAM,
		RSC_SCISSOR_TEST = Ogre::RSC_SCISSOR_TEST,
		RSC_TWO_SIDED_STENCIL = Ogre::RSC_TWO_SIDED_STENCIL,
		RSC_STENCIL_WRAP = Ogre::RSC_STENCIL_WRAP,
		RSC_HWOCCLUSION = Ogre::RSC_HWOCCLUSION,
		RSC_USER_CLIP_PLANES = Ogre::RSC_USER_CLIP_PLANES,
		RSC_VERTEX_FORMAT_UBYTE4 = Ogre::RSC_VERTEX_FORMAT_UBYTE4,
		RSC_INFINITE_FAR_PLANE = Ogre::RSC_INFINITE_FAR_PLANE,
		RSC_HWRENDER_TO_TEXTURE = Ogre::RSC_HWRENDER_TO_TEXTURE,
		RSC_TEXTURE_FLOAT = Ogre::RSC_TEXTURE_FLOAT,
		RSC_NON_POWER_OF_2_TEXTURES = Ogre::RSC_NON_POWER_OF_2_TEXTURES,
		RSC_TEXTURE_3D = Ogre::RSC_TEXTURE_3D,
		RSC_POINT_SPRITES = Ogre::RSC_POINT_SPRITES,
		RSC_POINT_EXTENDED_PARAMETERS = Ogre::RSC_POINT_EXTENDED_PARAMETERS,
		RSC_VERTEX_TEXTURE_FETCH = Ogre::RSC_VERTEX_TEXTURE_FETCH,
		RSC_MIPMAP_LOD_BIAS = Ogre::RSC_MIPMAP_LOD_BIAS,
		RSC_GEOMETRY_PROGRAM = Ogre::RSC_GEOMETRY_PROGRAM,
		RSC_HWRENDER_TO_VERTEX_BUFFER = Ogre::RSC_HWRENDER_TO_VERTEX_BUFFER,
		RSC_COMPLETE_TEXTURE_BINDING = Ogre::RSC_COMPLETE_TEXTURE_BINDING,
		RSC_TEXTURE_COMPRESSION = Ogre::RSC_TEXTURE_COMPRESSION,
		RSC_TEXTURE_COMPRESSION_DXT = Ogre::RSC_TEXTURE_COMPRESSION_DXT,
		RSC_TEXTURE_COMPRESSION_VTC = Ogre::RSC_TEXTURE_COMPRESSION_VTC,
		RSC_TEXTURE_COMPRESSION_PVRTC = Ogre::RSC_TEXTURE_COMPRESSION_PVRTC,
		RSC_TEXTURE_COMPRESSION_ATC = Ogre::RSC_TEXTURE_COMPRESSION_ATC,
		RSC_TEXTURE_COMPRESSION_ETC1 = Ogre::RSC_TEXTURE_COMPRESSION_ETC1,
		RSC_TEXTURE_COMPRESSION_ETC2 = Ogre::RSC_TEXTURE_COMPRESSION_ETC2,
		RSC_TEXTURE_COMPRESSION_BC4_BC5 = Ogre::RSC_TEXTURE_COMPRESSION_BC4_BC5,
		RSC_TEXTURE_COMPRESSION_BC6H_BC7 = Ogre::RSC_TEXTURE_COMPRESSION_BC6H_BC7,
		RSC_FIXED_FUNCTION = Ogre::RSC_FIXED_FUNCTION,
		RSC_MRT_DIFFERENT_BIT_DEPTHS = Ogre::RSC_MRT_DIFFERENT_BIT_DEPTHS,
		RSC_ALPHA_TO_COVERAGE = Ogre::RSC_ALPHA_TO_COVERAGE,
		RSC_ADVANCED_BLEND_OPERATIONS = Ogre::RSC_ADVANCED_BLEND_OPERATIONS,
		RSC_RTT_SEPARATE_DEPTHBUFFER = Ogre::RSC_RTT_SEPARATE_DEPTHBUFFER,
		RSC_RTT_MAIN_DEPTHBUFFER_ATTACHABLE = Ogre::RSC_RTT_MAIN_DEPTHBUFFER_ATTACHABLE,
		RSC_RTT_DEPTHBUFFER_RESOLUTION_LESSEQUAL = Ogre::RSC_RTT_DEPTHBUFFER_RESOLUTION_LESSEQUAL,
		RSC_VERTEX_BUFFER_INSTANCE_DATA = Ogre::RSC_VERTEX_BUFFER_INSTANCE_DATA,
		RSC_SHADER_SUBROUTINE = Ogre::RSC_SHADER_SUBROUTINE,
		RSC_HWRENDER_TO_TEXTURE_3D = Ogre::RSC_HWRENDER_TO_TEXTURE_3D,
		RSC_TEXTURE_1D = Ogre::RSC_TEXTURE_1D,
		RSC_TESSELLATION_HULL_PROGRAM = Ogre::RSC_TESSELLATION_HULL_PROGRAM,
		RSC_TESSELLATION_DOMAIN_PROGRAM = Ogre::RSC_TESSELLATION_DOMAIN_PROGRAM,
		RSC_COMPUTE_PROGRAM = Ogre::RSC_COMPUTE_PROGRAM,
		RSC_HWOCCLUSION_ASYNCHRONOUS = Ogre::RSC_HWOCCLUSION_ASYNCHRONOUS,
		RSC_ATOMIC_COUNTERS = Ogre::RSC_ATOMIC_COUNTERS,
		RSC_READ_BACK_AS_TEXTURE = Ogre::RSC_READ_BACK_AS_TEXTURE,
		RSC_EXPLICIT_FSAA_RESOLVE = Ogre::RSC_EXPLICIT_FSAA_RESOLVE,
		RSC_PERSTAGECONSTANT = Ogre::RSC_PERSTAGECONSTANT,
		RSC_GL1_5_NOVBO = Ogre::RSC_GL1_5_NOVBO,
		RSC_FBO = Ogre::RSC_FBO,
		RSC_FBO_ARB = Ogre::RSC_FBO_ARB,
		RSC_FBO_ATI = Ogre::RSC_FBO_ATI,
		RSC_PBUFFER = Ogre::RSC_PBUFFER,
		RSC_GL1_5_NOHWOCCLUSION = Ogre::RSC_GL1_5_NOHWOCCLUSION,
		RSC_POINT_EXTENDED_PARAMETERS_ARB = Ogre::RSC_POINT_EXTENDED_PARAMETERS_ARB,
		RSC_POINT_EXTENDED_PARAMETERS_EXT = Ogre::RSC_POINT_EXTENDED_PARAMETERS_EXT,
		RSC_SEPARATE_SHADER_OBJECTS = Ogre::RSC_SEPARATE_SHADER_OBJECTS,
		RSC_VAO = Ogre::RSC_VAO
	};

	//################################################################
	//RenderSystemCapabilities
	//################################################################
	ref class Log;

	public ref class RenderSystemCapabilities
	{
	private protected:
		virtual void ClearNativePtr()
		{
			_native = 0;
		}

	public protected:
		RenderSystemCapabilities(Ogre::RenderSystemCapabilities* obj) : _native(obj), _createdByCLR(false)
		{
		}

		RenderSystemCapabilities(intptr_t ptr) : _native((Ogre::RenderSystemCapabilities*)ptr), _createdByCLR(false)
		{
		}

		~RenderSystemCapabilities()
		{
			if (_createdByCLR &&_native)
			{
				delete _native;
				_native = 0;
			}
		}

		Ogre::RenderSystemCapabilities* _native;
		bool _createdByCLR;

	public:
		RenderSystemCapabilities();

		property Ogre::ushort FragmentProgramConstantBoolCount
		{
		public:
			Ogre::ushort get();
		public:
			void set(Ogre::ushort c);
		}

		property Ogre::ushort FragmentProgramConstantFloatCount
		{
		public:
			Ogre::ushort get();
		public:
			void set(Ogre::ushort c);
		}

		property Ogre::ushort FragmentProgramConstantIntCount
		{
		public:
			Ogre::ushort get();
		public:
			void set(Ogre::ushort c);
		}

		property Ogre::Real MaxPointSize
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real s);
		}

		property bool NonPOW2TexturesLimited
		{
		public:
			bool get();
		public:
			void set(bool l);
		}

		property Ogre::ushort NumTextureUnits
		{
		public:
			Ogre::ushort get();
		public:
			void set(Ogre::ushort num);
		}

		property Ogre::ushort NumVertexTextureUnits
		{
		public:
			Ogre::ushort get();
		public:
			void set(Ogre::ushort n);
		}

		property Ogre::ushort NumWorldMatricies
		{
		public:
			Ogre::ushort get();
		public:
			void set(Ogre::ushort num);
		}

		property Ogre::ushort StencilBufferBitDepth
		{
		public:
			Ogre::ushort get();
		public:
			void set(Ogre::ushort num);
		}

		property Ogre::ushort VertexProgramConstantBoolCount
		{
		public:
			Ogre::ushort get();
		public:
			void set(Ogre::ushort c);
		}

		property Ogre::ushort VertexProgramConstantFloatCount
		{
		public:
			Ogre::ushort get();
		public:
			void set(Ogre::ushort c);
		}

		property Ogre::ushort VertexProgramConstantIntCount
		{
		public:
			Ogre::ushort get();
		public:
			void set(Ogre::ushort c);
		}

		property bool VertexTextureUnitsShared
		{
		public:
			bool get();
		public:
			void set(bool shared);
		}

		void SetNumVertexBlendMatrices(Ogre::ushort num);

		void SetNumMultiRenderTargets(Ogre::ushort num);

		Ogre::ushort NumVertexBlendMatrices();

		Ogre::ushort NumMultiRenderTargets();

		void SetCapability(Mogre::Capabilities c);

		bool HasCapability(Mogre::Capabilities c);

		void Log(Mogre::Log^ pLog);
	};

	public ref class RenderSystem : public IRenderSystem_Listener_Receiver
	{
	public:
		ref class Listener abstract sealed
		{
		public:
			delegate static void EventOccurredHandler(String^ eventName/*, Mogre::Const_NameValuePairList^ parameters*/);
		};

	internal:
		Ogre::RenderSystem* _native;

	private protected:
		RenderSystem_Listener_Director* _listener;
		Mogre::RenderSystem::Listener::EventOccurredHandler^ _eventOccurred;

	public protected:
		RenderSystem(intptr_t ptr) : _native((Ogre::RenderSystem*)ptr)
		{

		}

		~RenderSystem()
		{
			if (_listener != 0)
			{
				if (_native != 0)
					_native->removeListener(_listener);
				delete _listener; _listener = 0;
			}
		}

	public:

		event Mogre::RenderSystem::Listener::EventOccurredHandler^ EventOccurred
		{
			void add(Mogre::RenderSystem::Listener::EventOccurredHandler^ hnd)
			{
				if (_eventOccurred == CLR_NULL)
				{
					if (_listener == 0)
					{
						_listener = new RenderSystem_Listener_Director(this);
						static_cast<Ogre::RenderSystem*>(_native)->addListener(_listener);
					}
					_listener->doCallForEventOccurred = true;
				}
				_eventOccurred += hnd;
			}
			void remove(Mogre::RenderSystem::Listener::EventOccurredHandler^ hnd)
			{
				_eventOccurred -= hnd;
				if (_eventOccurred == CLR_NULL) _listener->doCallForEventOccurred = false;
			}
		private:
			void raise(String^ eventName/*, Mogre::Const_NameValuePairList^ parameters*/)
			{
				if (_eventOccurred)
					_eventOccurred->Invoke(eventName/*, parameters*/);
			}
		}

		property Mogre::RenderSystemCapabilities^ Capabilities
		{
		public:
			Mogre::RenderSystemCapabilities^ get();
		}

		property Mogre::VertexElementType ColourVertexElementType
		{
		public:
			Mogre::VertexElementType get();
		}

		property Ogre::Real HorizontalTexelOffset
		{
		public:
			Ogre::Real get();
		}

		property Ogre::Real MaximumDepthInputValue
		{
		public:
			Ogre::Real get();
		}

		property Ogre::Real MinimumDepthInputValue
		{
		public:
			Ogre::Real get();
		}

		property String^ Name
		{
		public:
			String^ get();
		}

		property Ogre::Real VerticalTexelOffset
		{
		public:
			Ogre::Real get();
		}

		Mogre::ConfigOptionMap^ GetConfigOptions();
		void SetConfigOption(String^ name, String^ value);

		void SetStencilCheckEnabled(bool enabled);

		void SetStencilBufferParams(Mogre::CompareFunction func, Ogre::uint32 refValue, Ogre::uint32 compareMask, Ogre::uint32 writeMask, Mogre::StencilOperation stencilFailOp, Mogre::StencilOperation depthFailOp, Mogre::StencilOperation passOp, bool twoSidedOperation);
		void SetStencilBufferParams(Mogre::CompareFunction func, Ogre::uint32 refValue, Ogre::uint32 compareMask, Ogre::uint32 writeMask, Mogre::StencilOperation stencilFailOp, Mogre::StencilOperation depthFailOp, Mogre::StencilOperation passOp);
		void SetStencilBufferParams(Mogre::CompareFunction func, Ogre::uint32 refValue, Ogre::uint32 compareMask, Ogre::uint32 writeMask, Mogre::StencilOperation stencilFailOp, Mogre::StencilOperation depthFailOp);
		void SetStencilBufferParams(Mogre::CompareFunction func, Ogre::uint32 refValue, Ogre::uint32 compareMask, Ogre::uint32 writeMask, Mogre::StencilOperation stencilFailOp);
		void SetStencilBufferParams(Mogre::CompareFunction func, Ogre::uint32 refValue, Ogre::uint32 compareMask, Ogre::uint32 writeMask);
		void SetStencilBufferParams(Mogre::CompareFunction func, Ogre::uint32 refValue, Ogre::uint32 compareMask);
		void SetStencilBufferParams(Mogre::CompareFunction func, Ogre::uint32 refValue);
		void SetStencilBufferParams(Mogre::CompareFunction func);
		void SetStencilBufferParams();

		void SetInvertVertexWinding(bool invert);

		void SetScissorTest(bool enabled, size_t left, size_t top, size_t right, size_t bottom);
		void SetScissorTest(bool enabled, size_t left, size_t top, size_t right);
		void SetScissorTest(bool enabled, size_t left, size_t top);
		void SetScissorTest(bool enabled, size_t left);
		void SetScissorTest(bool enabled);

		void ClearFrameBuffer(unsigned int buffers, Mogre::ColourValue colour, Mogre::Real depth, unsigned short stencil);
		void ClearFrameBuffer(unsigned int buffers, Mogre::ColourValue colour, Mogre::Real depth);
		void ClearFrameBuffer(unsigned int buffers, Mogre::ColourValue colour);
		void ClearFrameBuffer(unsigned int buffers);

		void SetCurrentPassIterationCount(size_t count);

		void ConvertColourValue(Mogre::ColourValue colour, [Out] Ogre::uint32% pDest);

		void BindGpuProgram(Mogre::GpuProgram^ prg);
		void BindGpuProgramParameters(Mogre::GpuProgramType gptype, Mogre::GpuProgramParametersSharedPtr^ params, Ogre::uint16 variabilityMask);
		void BindGpuProgramPassIterationParameters(Mogre::GpuProgramType gptype);
		void UnbindGpuProgram(Mogre::GpuProgramType gptype);
		bool IsGpuProgramBound(Mogre::GpuProgramType gptype);

		void _convertProjectionMatrix(Mogre::Matrix4^ matrix, Mogre::Matrix4^ dest, bool forGpuProgram);
		void _convertProjectionMatrix(Mogre::Matrix4^ matrix, Mogre::Matrix4^ dest);

		void _makeProjectionMatrix(Mogre::Radian fovy, Mogre::Real aspect, Mogre::Real nearPlane, Mogre::Real farPlane, Mogre::Matrix4^ dest, bool forGpuProgram);
		void _makeProjectionMatrix(Mogre::Radian fovy, Mogre::Real aspect, Mogre::Real nearPlane, Mogre::Real farPlane, Mogre::Matrix4^ dest);

		void _makeProjectionMatrix(Mogre::Real left, Mogre::Real right, Mogre::Real bottom, Mogre::Real top, Mogre::Real nearPlane, Mogre::Real farPlane, Mogre::Matrix4^ dest, bool forGpuProgram);
		void _makeProjectionMatrix(Mogre::Real left, Mogre::Real right, Mogre::Real bottom, Mogre::Real top, Mogre::Real nearPlane, Mogre::Real farPlane, Mogre::Matrix4^ dest);

		void _makeOrthoMatrix(Mogre::Radian fovy, Mogre::Real aspect, Mogre::Real nearPlane, Mogre::Real farPlane, Mogre::Matrix4^ dest, bool forGpuProgram);
		void _makeOrthoMatrix(Mogre::Radian fovy, Mogre::Real aspect, Mogre::Real nearPlane, Mogre::Real farPlane, Mogre::Matrix4^ dest);

		void _setWorldMatrix(Mogre::Matrix4^ m);
		void _setWorldMatrices(const Mogre::Matrix4* m, unsigned short count);
		void _setViewMatrix(Mogre::Matrix4^ m);
		void _setProjectionMatrix(Mogre::Matrix4^ m);

		void _setTextureUnitSettings(size_t texUnit, Mogre::TextureUnitState^ tl);
		void _setBindingType(Mogre::TextureUnitState::BindingType bindigType);
		void _disableTextureUnit(size_t texUnit);
		void _disableTextureUnitsFrom(size_t texUnit);

		void _setTexture(size_t unit, bool enabled, TexturePtr^ texPtr);
		void _setTexture(size_t unit, bool enabled, String^ texName);
		void _setVertexTexture(size_t unit, TexturePtr^ tex);
		void _setGeometryTexture(size_t unit, TexturePtr^ tex);
		void _setComputeTexture(size_t unit, TexturePtr^ tex);
		void _setTessellationHullTexture(size_t unit, TexturePtr^ tex);
		void _setTessellationDomainTexture(size_t unit, TexturePtr^ tex);

		void _setTextureCoordSet(size_t unit, size_t index);

		void _setTextureCoordCalculation(size_t unit, Mogre::TexCoordCalcMethod m, Mogre::Frustum^ frustum);
		void _setTextureCoordCalculation(size_t unit, Mogre::TexCoordCalcMethod m);

		void _setTextureBlendMode(size_t unit, Mogre::LayerBlendModeEx_NativePtr bm);

		void _setTextureUnitFiltering(size_t unit, Mogre::FilterOptions minFilter, Mogre::FilterOptions magFilter, Mogre::FilterOptions mipFilter);

		void _setTextureUnitFiltering(size_t unit, Mogre::FilterType ftype, Mogre::FilterOptions filter);

		void _setTextureLayerAnisotropy(size_t unit, unsigned int maxAnisotropy);

		void _setTextureAddressingMode(size_t unit, Mogre::TextureUnitState::UVWAddressingMode uvw);

		void _setTextureBorderColour(size_t unit, Mogre::ColourValue colour);

		void _setTextureMipmapBias(size_t unit, float bias);

		void _setTextureMatrix(size_t unit, Mogre::Matrix4^ xform);

		void _setSceneBlending(Mogre::SceneBlendFactor sourceFactor, Mogre::SceneBlendFactor destFactor);

		void _setAlphaRejectSettings(Mogre::CompareFunction func, unsigned char value, bool alphaToCoverage);

		void _setRenderTarget(RenderTarget^ target);
		void _render(Mogre::RenderOperation^ op);

		DEFINE_MANAGED_NATIVE_CONVERSIONS(RenderSystem);

	internal:
		property Ogre::RenderSystem* UnmanagedPointer
		{
			Ogre::RenderSystem* get();
		}

	protected public:

		virtual void OnEventOccurred(String^ eventName/*, Mogre::Const_NameValuePairList^ parameters*/) = IRenderSystem_Listener_Receiver::EventOccurred
		{
			EventOccurred(eventName/*, parameters*/);
		}
	};
}