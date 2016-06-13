#pragma once

#include "OgreMaterial.h"
#include "OgreTechnique.h"
#include "OgrePass.h"
#include "OgreTextureUnitState.h"
#include "OgreBlendMode.h"
#include "MogreResource.h"
#include "MogreResourceManager.h"
#include "MogreTextureManager.h"
#include "MogreCommon.h"
#include "MogreLight.h"
#include "Marshalling.h"
#include "STLContainerWrappers.h"
#include "IteratorWrapper.h"

namespace Mogre
{
	public enum class SceneBlendFactor
	{
		SBF_ONE = Ogre::SBF_ONE,
		SBF_ZERO = Ogre::SBF_ZERO,
		SBF_DEST_COLOUR = Ogre::SBF_DEST_COLOUR,
		SBF_SOURCE_COLOUR = Ogre::SBF_SOURCE_COLOUR,
		SBF_ONE_MINUS_DEST_COLOUR = Ogre::SBF_ONE_MINUS_DEST_COLOUR,
		SBF_ONE_MINUS_SOURCE_COLOUR = Ogre::SBF_ONE_MINUS_SOURCE_COLOUR,
		SBF_DEST_ALPHA = Ogre::SBF_DEST_ALPHA,
		SBF_SOURCE_ALPHA = Ogre::SBF_SOURCE_ALPHA,
		SBF_ONE_MINUS_DEST_ALPHA = Ogre::SBF_ONE_MINUS_DEST_ALPHA,
		SBF_ONE_MINUS_SOURCE_ALPHA = Ogre::SBF_ONE_MINUS_SOURCE_ALPHA
	};

	public enum class SceneBlendOperation
	{
		SBO_ADD = Ogre::SBO_ADD,
		SBO_SUBTRACT = Ogre::SBO_SUBTRACT,
		SBO_REVERSE_SUBTRACT = Ogre::SBO_REVERSE_SUBTRACT,
		SBO_MIN = Ogre::SBO_MIN,
		SBO_MAX = Ogre::SBO_MAX
	};

	public enum class SceneBlendType
	{
		SBT_TRANSPARENT_ALPHA = Ogre::SBT_TRANSPARENT_ALPHA,
		SBT_TRANSPARENT_COLOUR = Ogre::SBT_TRANSPARENT_COLOUR,
		SBT_ADD = Ogre::SBT_ADD,
		SBT_MODULATE = Ogre::SBT_MODULATE,
		SBT_REPLACE = Ogre::SBT_REPLACE
	};

	public enum class LayerBlendSource
	{
		LBS_CURRENT = Ogre::LBS_CURRENT,
		LBS_TEXTURE = Ogre::LBS_TEXTURE,
		LBS_DIFFUSE = Ogre::LBS_DIFFUSE,
		LBS_SPECULAR = Ogre::LBS_SPECULAR,
		LBS_MANUAL = Ogre::LBS_MANUAL
	};

	public enum class LayerBlendOperationEx
	{
		LBX_SOURCE1 = Ogre::LBX_SOURCE1,
		LBX_SOURCE2 = Ogre::LBX_SOURCE2,
		LBX_MODULATE = Ogre::LBX_MODULATE,
		LBX_MODULATE_X2 = Ogre::LBX_MODULATE_X2,
		LBX_MODULATE_X4 = Ogre::LBX_MODULATE_X4,
		LBX_ADD = Ogre::LBX_ADD,
		LBX_ADD_SIGNED = Ogre::LBX_ADD_SIGNED,
		LBX_ADD_SMOOTH = Ogre::LBX_ADD_SMOOTH,
		LBX_SUBTRACT = Ogre::LBX_SUBTRACT,
		LBX_BLEND_DIFFUSE_ALPHA = Ogre::LBX_BLEND_DIFFUSE_ALPHA,
		LBX_BLEND_TEXTURE_ALPHA = Ogre::LBX_BLEND_TEXTURE_ALPHA,
		LBX_BLEND_CURRENT_ALPHA = Ogre::LBX_BLEND_CURRENT_ALPHA,
		LBX_BLEND_MANUAL = Ogre::LBX_BLEND_MANUAL,
		LBX_DOTPRODUCT = Ogre::LBX_DOTPRODUCT,
		LBX_BLEND_DIFFUSE_COLOUR = Ogre::LBX_BLEND_DIFFUSE_COLOUR
	};

	public enum class LayerBlendOperation
	{
		LBO_REPLACE = Ogre::LBO_REPLACE,
		LBO_ADD = Ogre::LBO_ADD,
		LBO_MODULATE = Ogre::LBO_MODULATE,
		LBO_ALPHA_BLEND = Ogre::LBO_ALPHA_BLEND
	};

	public enum class LayerBlendType
	{
		LBT_COLOUR = Ogre::LBT_COLOUR,
		LBT_ALPHA = Ogre::LBT_ALPHA
	};

	public enum class IlluminationStage
	{
		IS_AMBIENT = Ogre::IS_AMBIENT,
		IS_PER_LIGHT = Ogre::IS_PER_LIGHT,
		IS_DECAL = Ogre::IS_DECAL
	};

	ref class Material;
	ref class MaterialPtr;
	ref class Technique;
	ref class Pass;
	ref class TextureUnitState;
	ref class GpuProgramPtr;
	ref class GpuProgramParametersSharedPtr;
	ref class Frustum;
	ref class TexturePtr;

	public value class IlluminationPass_NativePtr
	{
	private protected:
		Ogre::IlluminationPass* _native;

	public:
		property Mogre::IlluminationStage stage
		{
		public:
			Mogre::IlluminationStage get();
		public:
			void set(Mogre::IlluminationStage value);
		}

		property Mogre::Pass^ pass
		{
		public:
			Mogre::Pass^ get();
		public:
			void set(Mogre::Pass^ value);
		}

		property bool destroyOnShutdown
		{
		public:
			bool get();
		public:
			void set(bool value);
		}

		property Mogre::Pass^ originalPass
		{
		public:
			Mogre::Pass^ get();
		public:
			void set(Mogre::Pass^ value);
		}

		DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_NATIVEPTRVALUECLASS(IlluminationPass_NativePtr, Ogre::IlluminationPass);

		property IntPtr NativePtr
		{
			IntPtr get() { return (IntPtr)_native; }
		}

		static IlluminationPass_NativePtr Create();

		void DestroyNativePtr()
		{
			if (_native) { delete _native; _native = 0; }
		}

		property bool IsNull
		{
			bool get() { return (_native == 0); }
		}
	};

	public value class LayerBlendModeEx_NativePtr
	{
	private protected:
		Ogre::LayerBlendModeEx* _native;

	public:
		property Mogre::LayerBlendType blendType
		{
		public:
			Mogre::LayerBlendType get();
		public:
			void set(Mogre::LayerBlendType value);
		}

		property Mogre::LayerBlendOperationEx operation
		{
		public:
			Mogre::LayerBlendOperationEx get();
		public:
			void set(Mogre::LayerBlendOperationEx value);
		}

		property Mogre::LayerBlendSource source1
		{
		public:
			Mogre::LayerBlendSource get();
		public:
			void set(Mogre::LayerBlendSource value);
		}

		property Mogre::LayerBlendSource source2
		{
		public:
			Mogre::LayerBlendSource get();
		public:
			void set(Mogre::LayerBlendSource value);
		}

		property Mogre::ColourValue colourArg1
		{
		public:
			Mogre::ColourValue get();
		public:
			void set(Mogre::ColourValue value);
		}

		property Mogre::ColourValue colourArg2
		{
		public:
			Mogre::ColourValue get();
		public:
			void set(Mogre::ColourValue value);
		}

		property Mogre::Real alphaArg1
		{
		public:
			Mogre::Real get();
		public:
			void set(Mogre::Real value);
		}

		property Mogre::Real alphaArg2
		{
		public:
			Mogre::Real get();
		public:
			void set(Mogre::Real value);
		}

		property Mogre::Real factor
		{
		public:
			Mogre::Real get();
		public:
			void set(Mogre::Real value);
		}

		virtual bool Equals(Object^ obj) override;
		bool Equals(LayerBlendModeEx_NativePtr obj);
		static bool operator == (LayerBlendModeEx_NativePtr obj1, LayerBlendModeEx_NativePtr obj2);
		static bool operator != (LayerBlendModeEx_NativePtr obj1, LayerBlendModeEx_NativePtr obj2);

		void CopyTo(LayerBlendModeEx_NativePtr dest)
		{
			if (_native == NULL) throw gcnew Exception("The underlying native object for the caller is null.");
			if (dest._native == NULL) throw gcnew ArgumentException("The underlying native object for parameter 'dest' is null.");

			*(dest._native) = *_native;
		}

		DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_NATIVEPTRVALUECLASS(LayerBlendModeEx_NativePtr, Ogre::LayerBlendModeEx);


		property IntPtr NativePtr
		{
			IntPtr get() { return (IntPtr)_native; }
		}

		static LayerBlendModeEx_NativePtr Create();

		void DestroyNativePtr()
		{
			if (_native) { delete _native; _native = 0; }
		}

		property bool IsNull
		{
			bool get() { return (_native == 0); }
		}
	};

	public ref class TextureUnitState : IMogreDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	public:
		enum class TextureEffectType
		{
			ET_ENVIRONMENT_MAP = Ogre::TextureUnitState::ET_ENVIRONMENT_MAP,
			ET_PROJECTIVE_TEXTURE = Ogre::TextureUnitState::ET_PROJECTIVE_TEXTURE,
			ET_UVSCROLL = Ogre::TextureUnitState::ET_UVSCROLL,
			ET_USCROLL = Ogre::TextureUnitState::ET_USCROLL,
			ET_VSCROLL = Ogre::TextureUnitState::ET_VSCROLL,
			ET_ROTATE = Ogre::TextureUnitState::ET_ROTATE,
			ET_TRANSFORM = Ogre::TextureUnitState::ET_TRANSFORM
		};

		enum class EnvMapType
		{
			ENV_PLANAR = Ogre::TextureUnitState::ENV_PLANAR,
			ENV_CURVED = Ogre::TextureUnitState::ENV_CURVED,
			ENV_REFLECTION = Ogre::TextureUnitState::ENV_REFLECTION,
			ENV_NORMAL = Ogre::TextureUnitState::ENV_NORMAL
		};

		enum class TextureTransformType
		{
			TT_TRANSLATE_U = Ogre::TextureUnitState::TT_TRANSLATE_U,
			TT_TRANSLATE_V = Ogre::TextureUnitState::TT_TRANSLATE_V,
			TT_SCALE_U = Ogre::TextureUnitState::TT_SCALE_U,
			TT_SCALE_V = Ogre::TextureUnitState::TT_SCALE_V,
			TT_ROTATE = Ogre::TextureUnitState::TT_ROTATE
		};

		enum class TextureAddressingMode
		{
			TAM_WRAP = Ogre::TextureUnitState::TAM_WRAP,
			TAM_MIRROR = Ogre::TextureUnitState::TAM_MIRROR,
			TAM_CLAMP = Ogre::TextureUnitState::TAM_CLAMP,
			TAM_BORDER = Ogre::TextureUnitState::TAM_BORDER
		};

		enum class TextureCubeFace
		{
			CUBE_FRONT = Ogre::TextureUnitState::CUBE_FRONT,
			CUBE_BACK = Ogre::TextureUnitState::CUBE_BACK,
			CUBE_LEFT = Ogre::TextureUnitState::CUBE_LEFT,
			CUBE_RIGHT = Ogre::TextureUnitState::CUBE_RIGHT,
			CUBE_UP = Ogre::TextureUnitState::CUBE_UP,
			CUBE_DOWN = Ogre::TextureUnitState::CUBE_DOWN
		};

		enum class BindingType
		{
			BT_FRAGMENT = Ogre::TextureUnitState::BT_FRAGMENT,
			BT_VERTEX = Ogre::TextureUnitState::BT_VERTEX,
			BT_GEOMETRY = Ogre::TextureUnitState::BT_GEOMETRY,
			BT_TESSELLATION_HULL = Ogre::TextureUnitState::BT_TESSELLATION_HULL,
			BT_TESSELLATION_DOMAIN = Ogre::TextureUnitState::BT_TESSELLATION_DOMAIN,
			BT_COMPUTE = Ogre::TextureUnitState::BT_COMPUTE
		};

		enum class ContentType
		{
			CONTENT_NAMED = Ogre::TextureUnitState::CONTENT_NAMED,
			CONTENT_SHADOW = Ogre::TextureUnitState::CONTENT_SHADOW,
			CONTENT_COMPOSITOR = Ogre::TextureUnitState::CONTENT_COMPOSITOR
		};


		value class TextureEffect_NativePtr
		{
		private protected:
			Ogre::TextureUnitState::TextureEffect* _native;

		public:
			property Mogre::TextureUnitState::TextureEffectType type
			{
			public:
				Mogre::TextureUnitState::TextureEffectType get();
			public:
				void set(Mogre::TextureUnitState::TextureEffectType value);
			}

			property int subtype
			{
			public:
				int get();
			public:
				void set(int value);
			}

			property Mogre::Real arg1
			{
			public:
				Mogre::Real get();
			public:
				void set(Mogre::Real value);
			}

			property Mogre::Real arg2
			{
			public:
				Mogre::Real get();
			public:
				void set(Mogre::Real value);
			}

			property Mogre::WaveformType waveType
			{
			public:
				Mogre::WaveformType get();
			public:
				void set(Mogre::WaveformType value);
			}

			property Mogre::Real base
			{
			public:
				Mogre::Real get();
			public:
				void set(Mogre::Real value);
			}

			property Mogre::Real frequency
			{
			public:
				Mogre::Real get();
			public:
				void set(Mogre::Real value);
			}

			property Mogre::Real phase
			{
			public:
				Mogre::Real get();
			public:
				void set(Mogre::Real value);
			}

			property Mogre::Real amplitude
			{
			public:
				Mogre::Real get();
			public:
				void set(Mogre::Real value);
			}

			property Mogre::Frustum^ frustum
			{
			public:
				Mogre::Frustum^ get();
			}

			DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_NATIVEPTRVALUECLASS(TextureUnitState::TextureEffect_NativePtr, Ogre::TextureUnitState::TextureEffect);


			property IntPtr NativePtr
			{
				IntPtr get() { return (IntPtr)_native; }
			}

			static TextureEffect_NativePtr Create();

			void DestroyNativePtr()
			{
				if (_native) { delete _native; _native = 0; }
			}

			property bool IsNull
			{
				bool get() { return (_native == 0); }
			}
		};

		value class UVWAddressingMode
		{
		public:
			Mogre::TextureUnitState::TextureAddressingMode u;

			Mogre::TextureUnitState::TextureAddressingMode v;

			Mogre::TextureUnitState::TextureAddressingMode w;

			DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_VALUECLASS(TextureUnitState::UVWAddressingMode);
		};

		INC_DECLARE_STLMULTIMAP(EffectMap, Mogre::TextureUnitState::TextureEffectType, Mogre::TextureUnitState::TextureEffect_NativePtr, Ogre::TextureUnitState::TextureEffectType, Ogre::TextureUnitState::TextureEffect, public:, private:);

	public protected:
		Ogre::TextureUnitState* _native;
		bool _createdByCLR;

		TextureUnitState(Ogre::TextureUnitState* obj) : _native(obj)
		{
		}

		TextureUnitState(intptr_t obj) : _native((Ogre::TextureUnitState*)obj)
		{
		}

	public:
		~TextureUnitState();
	protected:
		!TextureUnitState();
	public:
		TextureUnitState(Mogre::Pass^ parent);
		TextureUnitState(Mogre::Pass^ parent, Mogre::TextureUnitState^ oth);
		TextureUnitState(Mogre::Pass^ parent, String^ texName, unsigned int texCoordSet);
		TextureUnitState(Mogre::Pass^ parent, String^ texName);

		property Mogre::LayerBlendModeEx_NativePtr AlphaBlendMode
		{
		public:
			Mogre::LayerBlendModeEx_NativePtr get();
		}

		property Mogre::Real AnimationDuration
		{
		public:
			Mogre::Real get();
		}

		property Mogre::SceneBlendFactor ColourBlendFallbackDest
		{
		public:
			Mogre::SceneBlendFactor get();
		}

		property Mogre::SceneBlendFactor ColourBlendFallbackSrc
		{
		public:
			Mogre::SceneBlendFactor get();
		}

		property Mogre::LayerBlendModeEx_NativePtr ColourBlendMode
		{
		public:
			Mogre::LayerBlendModeEx_NativePtr get();
		}

		property unsigned int CurrentFrame
		{
		public:
			unsigned int get();
		public:
			void set(unsigned int frameNumber);
		}

		property Mogre::PixelFormat DesiredFormat
		{
		public:
			Mogre::PixelFormat get();
		public:
			void set(Mogre::PixelFormat desiredFormat);
		}

		property bool HasViewRelativeTextureCoordinateGeneration
		{
		public:
			bool get();
		}

		property bool IsAlpha
		{
		public:
			bool get();
		public:
			void set(bool isAlpha);
		}

		property bool IsBlank
		{
		public:
			bool get();
		}

		property bool IsCubic
		{
		public:
			bool get();
		}

		property bool IsLoaded
		{
		public:
			bool get();
		}

		property bool IsTextureLoadFailing
		{
		public:
			bool get();
		}

		property String^ Name
		{
		public:
			String^ get();
		public:
			void set(String^ name);
		}

		property unsigned int NumFrames
		{
		public:
			unsigned int get();
		}

		property int NumMipmaps
		{
		public:
			int get();
		public:
			void set(int numMipmaps);
		}

		property Mogre::Pass^ Parent
		{
		public:
			Mogre::Pass^ get();
		}

		property unsigned int TextureAnisotropy
		{
		public:
			unsigned int get();
		public:
			void set(unsigned int maxAniso);
		}

		property Mogre::ColourValue TextureBorderColour
		{
		public:
			Mogre::ColourValue get();
		public:
			void set(Mogre::ColourValue colour);
		}

		property unsigned int TextureCoordSet
		{
		public:
			unsigned int get();
		public:
			void set(unsigned int set);
		}

		property float TextureMipmapBias
		{
		public:
			float get();
		public:
			void set(float bias);
		}

		property String^ TextureName
		{
		public:
			String^ get();
		}

		property String^ TextureNameAlias
		{
		public:
			String^ get();
		public:
			void set(String^ name);
		}

		property Mogre::Radian TextureRotate
		{
		public:
			Mogre::Radian get();
		public:
			void set(Mogre::Radian angle);
		}

		property Mogre::Matrix4 TextureTransform
		{
		public:
			Mogre::Matrix4 get();
		public:
			void set(Mogre::Matrix4 xform);
		}

		property Mogre::TextureType TextureType
		{
		public:
			Mogre::TextureType get();
		}

		property Mogre::Real TextureUScale
		{
		public:
			Mogre::Real get();
		public:
			void set(Mogre::Real value);
		}

		property Mogre::Real TextureUScroll
		{
		public:
			Mogre::Real get();
		public:
			void set(Mogre::Real value);
		}

		property Mogre::Real TextureVScale
		{
		public:
			Mogre::Real get();
		public:
			void set(Mogre::Real value);
		}

		property Mogre::Real TextureVScroll
		{
		public:
			Mogre::Real get();
		public:
			void set(Mogre::Real value);
		}

		void CopyTo(TextureUnitState^ dest)
		{
			if (_native == NULL) throw gcnew Exception("The underlying native object for the caller is null.");
			if (dest->_native == NULL) throw gcnew ArgumentException("The underlying native object for parameter 'dest' is null.");

			*(dest->_native) = *_native;
		}

		void SetTextureName(String^ name, Mogre::TextureType ttype);
		void SetTextureName(String^ name);

		void SetCubicTextureName(String^ name, bool forUVW);
		void SetCubicTextureName(String^ name);

		void SetCubicTextureName(array<String^>^ names, bool forUVW);
		void SetCubicTextureName(array<String^>^ names);

		void SetAnimatedTextureName(String^ name, unsigned int numFrames, Mogre::Real duration);
		void SetAnimatedTextureName(String^ name, unsigned int numFrames);

		void SetAnimatedTextureName(array<String^>^ names, unsigned int numFrames, Mogre::Real duration);
		void SetAnimatedTextureName(array<String^>^ names, unsigned int numFrames);

		Pair<size_t, size_t> GetTextureDimensions(unsigned int frame);
		Pair<size_t, size_t> GetTextureDimensions();

		String^ GetFrameTextureName(unsigned int frameNumber);

		void SetFrameTextureName(String^ name, unsigned int frameNumber);

		void AddFrameTextureName(String^ name);

		void DeleteFrameTextureName(size_t frameNumber);

		void SetBindingType(Mogre::TextureUnitState::BindingType bt);

		Mogre::TextureUnitState::BindingType GetBindingType();

		void SetContentType(Mogre::TextureUnitState::ContentType ct);

		Mogre::TextureUnitState::ContentType GetContentType();

		bool Is3D();

		void SetTextureScroll(Mogre::Real u, Mogre::Real v);

		void SetTextureScale(Mogre::Real uScale, Mogre::Real vScale);

		Mogre::TextureUnitState::UVWAddressingMode GetTextureAddressingMode();

		void SetTextureAddressingMode(Mogre::TextureUnitState::TextureAddressingMode tam);

		void SetTextureAddressingMode(Mogre::TextureUnitState::TextureAddressingMode u, Mogre::TextureUnitState::TextureAddressingMode v, Mogre::TextureUnitState::TextureAddressingMode w);

		void SetTextureAddressingMode(Mogre::TextureUnitState::UVWAddressingMode uvw);

		void SetColourOperationEx(Mogre::LayerBlendOperationEx op, Mogre::LayerBlendSource source1, Mogre::LayerBlendSource source2, Mogre::ColourValue arg1, Mogre::ColourValue arg2, Mogre::Real manualBlend);
		void SetColourOperationEx(Mogre::LayerBlendOperationEx op, Mogre::LayerBlendSource source1, Mogre::LayerBlendSource source2, Mogre::ColourValue arg1, Mogre::ColourValue arg2);
		void SetColourOperationEx(Mogre::LayerBlendOperationEx op, Mogre::LayerBlendSource source1, Mogre::LayerBlendSource source2, Mogre::ColourValue arg1);
		void SetColourOperationEx(Mogre::LayerBlendOperationEx op, Mogre::LayerBlendSource source1, Mogre::LayerBlendSource source2);
		void SetColourOperationEx(Mogre::LayerBlendOperationEx op, Mogre::LayerBlendSource source1);
		void SetColourOperationEx(Mogre::LayerBlendOperationEx op);

		void SetColourOperation(Mogre::LayerBlendOperation op);

		void SetColourOpMultipassFallback(Mogre::SceneBlendFactor sourceFactor, Mogre::SceneBlendFactor destFactor);

		void SetAlphaOperation(Mogre::LayerBlendOperationEx op, Mogre::LayerBlendSource source1, Mogre::LayerBlendSource source2, Mogre::Real arg1, Mogre::Real arg2, Mogre::Real manualBlend);
		void SetAlphaOperation(Mogre::LayerBlendOperationEx op, Mogre::LayerBlendSource source1, Mogre::LayerBlendSource source2, Mogre::Real arg1, Mogre::Real arg2);
		void SetAlphaOperation(Mogre::LayerBlendOperationEx op, Mogre::LayerBlendSource source1, Mogre::LayerBlendSource source2, Mogre::Real arg1);
		void SetAlphaOperation(Mogre::LayerBlendOperationEx op, Mogre::LayerBlendSource source1, Mogre::LayerBlendSource source2);
		void SetAlphaOperation(Mogre::LayerBlendOperationEx op, Mogre::LayerBlendSource source1);
		void SetAlphaOperation(Mogre::LayerBlendOperationEx op);

		void AddEffect(Mogre::TextureUnitState::TextureEffect_NativePtr effect);

		void SetEnvironmentMap(bool enable, Mogre::TextureUnitState::EnvMapType envMapType);
		void SetEnvironmentMap(bool enable);

		void SetScrollAnimation(Mogre::Real uSpeed, Mogre::Real vSpeed);

		void SetRotateAnimation(Mogre::Real speed);

		void SetTransformAnimation(Mogre::TextureUnitState::TextureTransformType ttype, Mogre::WaveformType waveType, Mogre::Real base, Mogre::Real frequency, Mogre::Real phase, Mogre::Real amplitude);
		void SetTransformAnimation(Mogre::TextureUnitState::TextureTransformType ttype, Mogre::WaveformType waveType, Mogre::Real base, Mogre::Real frequency, Mogre::Real phase);
		void SetTransformAnimation(Mogre::TextureUnitState::TextureTransformType ttype, Mogre::WaveformType waveType, Mogre::Real base, Mogre::Real frequency);
		void SetTransformAnimation(Mogre::TextureUnitState::TextureTransformType ttype, Mogre::WaveformType waveType, Mogre::Real base);
		void SetTransformAnimation(Mogre::TextureUnitState::TextureTransformType ttype, Mogre::WaveformType waveType);

		void SetProjectiveTexturing(bool enabled, Mogre::Frustum^ projectionSettings);
		void SetProjectiveTexturing(bool enabled);

		void RemoveAllEffects();

		void RemoveEffect(Mogre::TextureUnitState::TextureEffectType type);

		void SetBlank();

		void RetryTextureLoad();

		Mogre::TextureUnitState::Const_EffectMap^ GetEffects();

		void SetTextureFiltering(Mogre::TextureFilterOptions filterType);

		void SetTextureFiltering(Mogre::FilterType ftype, Mogre::FilterOptions opts);

		void SetTextureFiltering(Mogre::FilterOptions minFilter, Mogre::FilterOptions magFilter, Mogre::FilterOptions mipFilter);

		Mogre::FilterOptions GetTextureFiltering(Mogre::FilterType ftpye);

		void _load();

		void _unload();

		void _notifyNeedsRecompile();

		bool ApplyTextureAliases(Mogre::Const_AliasTextureNamePairList^ aliasList, bool apply);
		bool ApplyTextureAliases(Mogre::Const_AliasTextureNamePairList^ aliasList);

		void _notifyParent(Mogre::Pass^ parent);

		Mogre::TexturePtr^ _getTexturePtr();
		Mogre::TexturePtr^ _getTexturePtr(size_t frame);
		void _setTexturePtr(Mogre::TexturePtr^ texptr);
		void _setTexturePtr(Mogre::TexturePtr^ texptr, size_t frame);

		property bool IsDisposed
		{
			virtual bool get() { return _native == nullptr; }
		}

		DEFINE_MANAGED_NATIVE_CONVERSIONS(TextureUnitState);
	};

	public ref class Pass : IMogreDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	public:
		enum class BuiltinHashFunction
		{
			MIN_TEXTURE_CHANGE = Ogre::Pass::MIN_TEXTURE_CHANGE,
			MIN_GPU_PROGRAM_CHANGE = Ogre::Pass::MIN_GPU_PROGRAM_CHANGE
		};
		INC_DECLARE_STLSET(PassSet, Mogre::Pass^, Ogre::Pass*, public:, private:);
		INC_DECLARE_ITERATOR_NOCONSTRUCTOR(TextureUnitStateIterator, Ogre::Pass::TextureUnitStateIterator, Mogre::Pass::TextureUnitStates, Mogre::TextureUnitState^, Ogre::TextureUnitState*);

	public protected:
		Ogre::Pass* _native;
		bool _createdByCLR;

		Pass(Ogre::Pass* obj) : _native(obj)
		{
		}

		Pass(intptr_t obj) : _native((Ogre::Pass*)obj)
		{
		}

	public:
		~Pass();
	protected:
		!Pass();
	public:
		Pass(Mogre::Technique^ parent, unsigned short index);
		Pass(Mogre::Technique^ parent, unsigned short index, Mogre::Pass^ oth);


		property Mogre::CompareFunction AlphaRejectFunction
		{
		public:
			Mogre::CompareFunction get();
		public:
			void set(Mogre::CompareFunction func);
		}

		property unsigned char AlphaRejectValue
		{
		public:
			unsigned char get();
		public:
			void set(unsigned char val);
		}

		property Mogre::ColourValue Ambient
		{
		public:
			Mogre::ColourValue get();
		public:
			void set(Mogre::ColourValue ambient);
		}

		property bool ColourWriteEnabled
		{
		public:
			bool get();
		public:
			void set(bool enabled);
		}

		property Mogre::CullingMode CullingMode
		{
		public:
			Mogre::CullingMode get();
		public:
			void set(Mogre::CullingMode mode);
		}

		property float DepthBiasConstant
		{
		public:
			float get();
		}

		property float DepthBiasSlopeScale
		{
		public:
			float get();
		}

		property bool DepthCheckEnabled
		{
		public:
			bool get();
		public:
			void set(bool enabled);
		}

		property Mogre::CompareFunction DepthFunction
		{
		public:
			Mogre::CompareFunction get();
		public:
			void set(Mogre::CompareFunction func);
		}

		property bool DepthWriteEnabled
		{
		public:
			bool get();
		public:
			void set(bool enabled);
		}

		property Mogre::SceneBlendFactor DestBlendFactor
		{
		public:
			Mogre::SceneBlendFactor get();
		}

		property Mogre::ColourValue Diffuse
		{
		public:
			Mogre::ColourValue get();
		public:
			void set(Mogre::ColourValue diffuse);
		}

		property Mogre::ColourValue FogColour
		{
		public:
			Mogre::ColourValue get();
		}

		property Mogre::Real FogDensity
		{
		public:
			Mogre::Real get();
		}

		property Mogre::Real FogEnd
		{
		public:
			Mogre::Real get();
		}

		property Mogre::FogMode FogMode
		{
		public:
			Mogre::FogMode get();
		}

		property bool FogOverride
		{
		public:
			bool get();
		}

		property Mogre::Real FogStart
		{
		public:
			Mogre::Real get();
		}

		property String^ FragmentProgramName
		{
		public:
			String^ get();
		}

		property bool HasFragmentProgram
		{
		public:
			bool get();
		}

		property Ogre::uint32 Hash
		{
		public:
			Ogre::uint32 get();
		}

		property bool HasShadowCasterVertexProgram
		{
		public:
			bool get();
		}

		property bool HasVertexProgram
		{
		public:
			bool get();
		}

		property unsigned short Index
		{
		public:
			unsigned short get();
		}

		property bool IsAmbientOnly
		{
		public:
			bool get();
		}

		property bool IsLoaded
		{
		public:
			bool get();
		}

		property bool IsPointAttenuationEnabled
		{
		public:
			bool get();
		}

		property bool IsProgrammable
		{
		public:
			bool get();
		}

		property bool IsTransparent
		{
		public:
			bool get();
		}

		property bool IteratePerLight
		{
		public:
			bool get();
		}

		property unsigned short LightCountPerIteration
		{
		public:
			unsigned short get();
		public:
			void set(unsigned short c);
		}

		property bool LightingEnabled
		{
		public:
			bool get();
		public:
			void set(bool enabled);
		}

		property Mogre::ManualCullingMode ManualCullingMode
		{
		public:
			Mogre::ManualCullingMode get();
		public:
			void set(Mogre::ManualCullingMode mode);
		}

		property unsigned short MaxSimultaneousLights
		{
		public:
			unsigned short get();
		public:
			void set(unsigned short maxLights);
		}

		property String^ Name
		{
		public:
			String^ get();
		public:
			void set(String^ name);
		}

		property unsigned short NumTextureUnitStates
		{
		public:
			unsigned short get();
		}

		property Mogre::Light::LightTypes OnlyLightType
		{
		public:
			Mogre::Light::LightTypes get();
		}

		property Mogre::Technique^ Parent
		{
		public:
			Mogre::Technique^ get();
		}

		property size_t PassIterationCount
		{
		public:
			size_t get();
		}

		property Mogre::Real PointAttenuationConstant
		{
		public:
			Mogre::Real get();
		}

		property Mogre::Real PointAttenuationLinear
		{
		public:
			Mogre::Real get();
		}

		property Mogre::Real PointAttenuationQuadratic
		{
		public:
			Mogre::Real get();
		}

		property Mogre::Real PointMaxSize
		{
		public:
			Mogre::Real get();
		public:
			void set(Mogre::Real max);
		}

		property Mogre::Real PointMinSize
		{
		public:
			Mogre::Real get();
		public:
			void set(Mogre::Real min);
		}

		property Mogre::Real PointSize
		{
		public:
			Mogre::Real get();
		public:
			void set(Mogre::Real ps);
		}

		property bool PointSpritesEnabled
		{
		public:
			bool get();
		public:
			void set(bool enabled);
		}

		property Mogre::PolygonMode PolygonMode
		{
		public:
			Mogre::PolygonMode get();
		public:
			void set(Mogre::PolygonMode mode);
		}

		property String^ ResourceGroup
		{
		public:
			String^ get();
		}

		property bool RunOnlyForOneLightType
		{
		public:
			bool get();
		}

		property Mogre::ColourValue SelfIllumination
		{
		public:
			Mogre::ColourValue get();
		public:
			void set(Mogre::ColourValue selfIllum);
		}

		property Mogre::ShadeOptions ShadingMode
		{
		public:
			Mogre::ShadeOptions get();
		public:
			void set(Mogre::ShadeOptions mode);
		}

		property String^ ShadowCasterVertexProgramName
		{
		public:
			String^ get();
		}

		property Mogre::Real Shininess
		{
		public:
			Mogre::Real get();
		public:
			void set(Mogre::Real val);
		}

		property Mogre::SceneBlendFactor SourceBlendFactor
		{
		public:
			Mogre::SceneBlendFactor get();
		}

		property Mogre::ColourValue Specular
		{
		public:
			Mogre::ColourValue get();
		public:
			void set(Mogre::ColourValue specular);
		}

		property unsigned short StartLight
		{
		public:
			unsigned short get();
		public:
			void set(unsigned short startLight);
		}

		property Mogre::TrackVertexColourType VertexColourTracking
		{
		public:
			Mogre::TrackVertexColourType get();
		public:
			void set(Mogre::TrackVertexColourType tracking);
		}

		property String^ VertexProgramName
		{
		public:
			String^ get();
		}

		void CopyTo(Pass^ dest)
		{
			if (_native == NULL) throw gcnew Exception("The underlying native object for the caller is null.");
			if (dest->_native == NULL) throw gcnew ArgumentException("The underlying native object for parameter 'dest' is null.");

			*(dest->_native) = *_native;
		}

		void SetAmbient(Mogre::Real red, Mogre::Real green, Mogre::Real blue);

		void SetDiffuse(Mogre::Real red, Mogre::Real green, Mogre::Real blue, Mogre::Real alpha);

		void SetSpecular(Mogre::Real red, Mogre::Real green, Mogre::Real blue, Mogre::Real alpha);

		void SetSelfIllumination(Mogre::Real red, Mogre::Real green, Mogre::Real blue);

		void SetPointAttenuation(bool enabled, Mogre::Real constant, Mogre::Real linear, Mogre::Real quadratic);
		void SetPointAttenuation(bool enabled, Mogre::Real constant, Mogre::Real linear);
		void SetPointAttenuation(bool enabled, Mogre::Real constant);
		void SetPointAttenuation(bool enabled);

		Mogre::TextureUnitState^ CreateTextureUnitState();

		Mogre::TextureUnitState^ CreateTextureUnitState(String^ textureName, unsigned short texCoordSet);
		Mogre::TextureUnitState^ CreateTextureUnitState(String^ textureName);

		void AddTextureUnitState(Mogre::TextureUnitState^ state);

		Mogre::TextureUnitState^ GetTextureUnitState(unsigned short index);

		Mogre::TextureUnitState^ GetTextureUnitState(String^ name);

		unsigned short GetTextureUnitStateIndex(Mogre::TextureUnitState^ state);

		Mogre::Pass::TextureUnitStateIterator^ GetTextureUnitStateIterator();

		void RemoveTextureUnitState(unsigned short index);

		void RemoveAllTextureUnitStates();

		void SetSceneBlending(Mogre::SceneBlendType sbt);

		void SetSceneBlending(Mogre::SceneBlendFactor sourceFactor, Mogre::SceneBlendFactor destFactor);

		void SetFog(bool overrideScene, Mogre::FogMode mode, Mogre::ColourValue colour, Mogre::Real expDensity, Mogre::Real linearStart, Mogre::Real linearEnd);
		void SetFog(bool overrideScene, Mogre::FogMode mode, Mogre::ColourValue colour, Mogre::Real expDensity, Mogre::Real linearStart);
		void SetFog(bool overrideScene, Mogre::FogMode mode, Mogre::ColourValue colour, Mogre::Real expDensity);
		void SetFog(bool overrideScene, Mogre::FogMode mode, Mogre::ColourValue colour);
		void SetFog(bool overrideScene, Mogre::FogMode mode);
		void SetFog(bool overrideScene);

		void SetDepthBias(float constantBias, float slopeScaleBias);
		void SetDepthBias(float constantBias);

		void SetAlphaRejectSettings(Mogre::CompareFunction func, unsigned char value);

		void SetIteratePerLight(bool enabled, bool onlyForOneLightType, Mogre::Light::LightTypes lightType);
		void SetIteratePerLight(bool enabled, bool onlyForOneLightType);
		void SetIteratePerLight(bool enabled);

		void SetVertexProgram(String^ name, bool resetParams);
		void SetVertexProgram(String^ name);

		void SetVertexProgramParameters(Mogre::GpuProgramParametersSharedPtr^ params);

		Mogre::GpuProgramParametersSharedPtr^ GetVertexProgramParameters();

		Mogre::GpuProgramPtr^ GetVertexProgram();

		void SetShadowCasterVertexProgram(String^ name);

		void SetShadowCasterVertexProgramParameters(Mogre::GpuProgramParametersSharedPtr^ params);

		Mogre::GpuProgramParametersSharedPtr^ GetShadowCasterVertexProgramParameters();

		Mogre::GpuProgramPtr^ GetShadowCasterVertexProgram();

		void SetFragmentProgram(String^ name, bool resetParams);
		void SetFragmentProgram(String^ name);
		void SetFragmentProgramParameters(Mogre::GpuProgramParametersSharedPtr^ params);
		Mogre::GpuProgramParametersSharedPtr^ GetFragmentProgramParameters();
		Mogre::GpuProgramPtr^ GetFragmentProgram();

		Mogre::Pass^ _split(unsigned short numUnits);

		void _notifyIndex(unsigned short index);

		void _load();

		void _unload();

		void _dirtyHash();

		void _recalculateHash();

		void _notifyNeedsRecompile();

		unsigned short _getTextureUnitWithContentTypeIndex(Mogre::TextureUnitState::ContentType contentType, unsigned short index);

		void SetTextureFiltering(Mogre::TextureFilterOptions filterType);

		void SetTextureAnisotropy(unsigned int maxAniso);

		void QueueForDeletion();

		void SetPassIterationCount(size_t count);

		bool ApplyTextureAliases(Mogre::Const_AliasTextureNamePairList^ aliasList, bool apply);
		bool ApplyTextureAliases(Mogre::Const_AliasTextureNamePairList^ aliasList);

		static Mogre::Pass::Const_PassSet^ GetDirtyHashList();

		static Mogre::Pass::Const_PassSet^ GetPassGraveyard();

		static void ClearDirtyHashList();

		static void ProcessPendingPassUpdates();

		static void SetHashFunction(Mogre::Pass::BuiltinHashFunction builtin);

		property bool IsDisposed
		{
			virtual bool get() { return _native == nullptr; }
		}

		DEFINE_MANAGED_NATIVE_CONVERSIONS(Pass);
	};

	INC_DECLARE_STLVECTOR(IlluminationPassList, Mogre::IlluminationPass_NativePtr, Ogre::IlluminationPass*, public, private);

	public ref class Technique : IMogreDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	public:
		INC_DECLARE_ITERATOR_NOCONSTRUCTOR(PassIterator, Ogre::Technique::PassIterator, Mogre::Technique::Passes, Mogre::Pass^, Ogre::Pass*);
		INC_DECLARE_ITERATOR(IlluminationPassIterator, Ogre::Technique::IlluminationPassIterator, Mogre::IlluminationPassList, Mogre::IlluminationPass_NativePtr, Ogre::IlluminationPass*);

	public protected:
		Ogre::Technique* _native;
		bool _createdByCLR;

		Technique(Ogre::Technique* obj) : _native(obj)
		{
		}

		Technique(intptr_t obj) : _native((Ogre::Technique*)obj)
		{
		}

	public:
		~Technique();
	protected:
		!Technique();
	public:
		property bool IsDisposed
		{
			virtual bool get() { return _native == nullptr; }
		}

	public:
		Technique(Mogre::Material^ parent);
		Technique(Mogre::Material^ parent, Mogre::Technique^ oth);


		property bool HasColourWriteDisabled
		{
		public:
			bool get();
		}

		property bool IsDepthCheckEnabled
		{
		public:
			bool get();
		}

		property bool IsDepthWriteEnabled
		{
		public:
			bool get();
		}

		property bool IsLoaded
		{
		public:
			bool get();
		}

		property bool IsSupported
		{
		public:
			bool get();
		}

		property bool IsTransparent
		{
		public:
			bool get();
		}

		property unsigned short LodIndex
		{
		public:
			unsigned short get();
		public:
			void set(unsigned short index);
		}

		property String^ Name
		{
		public:
			String^ get();
		public:
			void set(String^ name);
		}

		property unsigned short NumPasses
		{
		public:
			unsigned short get();
		}

		property Mogre::Material^ Parent
		{
		public:
			Mogre::Material^ get();
		}

		property String^ ResourceGroup
		{
		public:
			String^ get();
		}

		property String^ SchemeName
		{
		public:
			String^ get();
		public:
			void set(String^ schemeName);
		}

		String^ _compile(bool autoManageTextureUnits);

		void _compileIlluminationPasses();

		Mogre::Pass^ CreatePass();

		Mogre::Pass^ GetPass(unsigned short index);

		Mogre::Pass^ GetPass(String^ name);

		void RemovePass(unsigned short index);

		void RemoveAllPasses();

		bool MovePass(unsigned short sourceIndex, unsigned short destinationIndex);

		Mogre::Technique::PassIterator^ GetPassIterator();

		Mogre::Technique::IlluminationPassIterator^ GetIlluminationPassIterator();

		void CopyTo(Technique^ dest)
		{
			if (_native == NULL) throw gcnew Exception("The underlying native object for the caller is null.");
			if (dest->_native == NULL) throw gcnew ArgumentException("The underlying native object for parameter 'dest' is null.");

			*(dest->_native) = *_native;
		}

		void _load();

		void _unload();

		void _notifyNeedsRecompile();

		void SetPointSize(Mogre::Real ps);

		void SetAmbient(Mogre::Real red, Mogre::Real green, Mogre::Real blue);

		void SetAmbient(Mogre::ColourValue ambient);

		void SetDiffuse(Mogre::Real red, Mogre::Real green, Mogre::Real blue, Mogre::Real alpha);

		void SetDiffuse(Mogre::ColourValue diffuse);

		void SetSpecular(Mogre::Real red, Mogre::Real green, Mogre::Real blue, Mogre::Real alpha);

		void SetSpecular(Mogre::ColourValue specular);

		void SetShininess(Mogre::Real val);

		void SetSelfIllumination(Mogre::Real red, Mogre::Real green, Mogre::Real blue);

		void SetSelfIllumination(Mogre::ColourValue selfIllum);

		void SetDepthCheckEnabled(bool enabled);

		void SetDepthWriteEnabled(bool enabled);

		void SetDepthFunction(Mogre::CompareFunction func);

		void SetColourWriteEnabled(bool enabled);

		void SetCullingMode(Mogre::CullingMode mode);

		void SetManualCullingMode(Mogre::ManualCullingMode mode);

		void SetLightingEnabled(bool enabled);

		void SetShadingMode(Mogre::ShadeOptions mode);

		void SetFog(bool overrideScene, Mogre::FogMode mode, Mogre::ColourValue colour, Mogre::Real expDensity, Mogre::Real linearStart, Mogre::Real linearEnd);
		void SetFog(bool overrideScene, Mogre::FogMode mode, Mogre::ColourValue colour, Mogre::Real expDensity, Mogre::Real linearStart);
		void SetFog(bool overrideScene, Mogre::FogMode mode, Mogre::ColourValue colour, Mogre::Real expDensity);
		void SetFog(bool overrideScene, Mogre::FogMode mode, Mogre::ColourValue colour);
		void SetFog(bool overrideScene, Mogre::FogMode mode);
		void SetFog(bool overrideScene);

		void SetDepthBias(float constantBias, float slopeScaleBias);

		void SetTextureFiltering(Mogre::TextureFilterOptions filterType);

		void SetTextureAnisotropy(unsigned int maxAniso);

		void SetSceneBlending(Mogre::SceneBlendType sbt);

		void SetSceneBlending(Mogre::SceneBlendFactor sourceFactor, Mogre::SceneBlendFactor destFactor);

		unsigned short _getSchemeIndex();

		bool ApplyTextureAliases(Mogre::Const_AliasTextureNamePairList^ aliasList, bool apply);
		bool ApplyTextureAliases(Mogre::Const_AliasTextureNamePairList^ aliasList);

		DEFINE_MANAGED_NATIVE_CONVERSIONS(Technique);
	};

	public ref class Material : public Resource
	{
	public:
		INC_DECLARE_STLVECTOR(LodDistanceList, Mogre::Real, Ogre::Real, public:, private:);
		INC_DECLARE_ITERATOR_NOCONSTRUCTOR(TechniqueIterator, Ogre::Material::TechniqueIterator, Mogre::Material::Techniques, Mogre::Technique^, Ogre::Technique*);

	public protected:
		Material(Ogre::Material* obj) : Resource(obj)
		{
		}

		Material(intptr_t ptr) : Resource((Ogre::Material*)ptr)
		{
		}

		Material(Ogre::Resource* obj) : Resource(obj)
		{
		}

	public:
		//Material(Mogre::ResourceManager^ creator, String^ name, Mogre::ResourceHandle handle, String^ group, bool isManual, Mogre::IManualResourceLoader^ loader);
		Material(Mogre::ResourceManager^ creator, String^ name, Mogre::ResourceHandle handle, String^ group, bool isManual);
		Material(Mogre::ResourceManager^ creator, String^ name, Mogre::ResourceHandle handle, String^ group);

		property bool CompilationRequired
		{
		public:
			bool get();
		}

		property bool IsTransparent
		{
		public:
			bool get();
		}

		property unsigned short NumSupportedTechniques
		{
		public:
			unsigned short get();
		}

		property unsigned short NumTechniques
		{
		public:
			unsigned short get();
		}

		property bool ReceiveShadows
		{
		public:
			bool get();
		public:
			void set(bool enabled);
		}

		property bool TransparencyCastsShadows
		{
		public:
			bool get();
		public:
			void set(bool enabled);
		}

		property String^ UnsupportedTechniquesExplanation
		{
		public:
			String^ get();
		}

		void CopyTo(Material^ dest)
		{
			if (_native == NULL) throw gcnew Exception("The underlying native object for the caller is null.");
			if (dest->_native == NULL) throw gcnew ArgumentException("The underlying native object for parameter 'dest' is null.");

			*(static_cast<Ogre::Material*>(dest->_native)) = *(static_cast<Ogre::Material*>(_native));
		}

		Mogre::Technique^ CreateTechnique();

		Mogre::Technique^ GetTechnique(unsigned short index);

		Mogre::Technique^ GetTechnique(String^ name);

		void RemoveTechnique(unsigned short index);

		void RemoveAllTechniques();

		Mogre::Material::TechniqueIterator^ GetTechniqueIterator();

		Mogre::Material::TechniqueIterator^ GetSupportedTechniqueIterator();

		Mogre::Technique^ GetSupportedTechnique(unsigned short index);

		unsigned short GetNumLodLevels(unsigned short schemeIndex);

		unsigned short GetNumLodLevels(String^ schemeName);

		Mogre::Technique^ GetBestTechnique(unsigned short lodIndex);
		Mogre::Technique^ GetBestTechnique();

		Mogre::MaterialPtr^ Clone(String^ newName, bool changeGroup, String^ newGroup);
		Mogre::MaterialPtr^ Clone(String^ newName, bool changeGroup);
		Mogre::MaterialPtr^ Clone(String^ newName);

		void CopyDetailsTo(Mogre::MaterialPtr^ mat);

		void Compile(bool autoManageTextureUnits);
		void Compile();

		void SetPointSize(Mogre::Real ps);

		void SetAmbient(Mogre::Real red, Mogre::Real green, Mogre::Real blue);

		void SetAmbient(Mogre::ColourValue ambient);

		void SetDiffuse(Mogre::Real red, Mogre::Real green, Mogre::Real blue, Mogre::Real alpha);

		void SetDiffuse(Mogre::ColourValue diffuse);

		void SetSpecular(Mogre::Real red, Mogre::Real green, Mogre::Real blue, Mogre::Real alpha);

		void SetSpecular(Mogre::ColourValue specular);

		void SetShininess(Mogre::Real val);

		void SetSelfIllumination(Mogre::Real red, Mogre::Real green, Mogre::Real blue);

		void SetSelfIllumination(Mogre::ColourValue selfIllum);

		void SetDepthCheckEnabled(bool enabled);

		void SetDepthWriteEnabled(bool enabled);

		void SetDepthFunction(Mogre::CompareFunction func);

		void SetColourWriteEnabled(bool enabled);

		void SetCullingMode(Mogre::CullingMode mode);

		void SetManualCullingMode(Mogre::ManualCullingMode mode);

		void SetLightingEnabled(bool enabled);

		void SetShadingMode(Mogre::ShadeOptions mode);

		void SetFog(bool overrideScene, Mogre::FogMode mode, Mogre::ColourValue colour, Mogre::Real expDensity, Mogre::Real linearStart, Mogre::Real linearEnd);
		void SetFog(bool overrideScene, Mogre::FogMode mode, Mogre::ColourValue colour, Mogre::Real expDensity, Mogre::Real linearStart);
		void SetFog(bool overrideScene, Mogre::FogMode mode, Mogre::ColourValue colour, Mogre::Real expDensity);
		void SetFog(bool overrideScene, Mogre::FogMode mode, Mogre::ColourValue colour);
		void SetFog(bool overrideScene, Mogre::FogMode mode);
		void SetFog(bool overrideScene);

		void SetDepthBias(float constantBias, float slopeScaleBias);

		void SetTextureFiltering(Mogre::TextureFilterOptions filterType);

		void SetTextureAnisotropy(int maxAniso);

		void SetSceneBlending(Mogre::SceneBlendType sbt);

		void SetSceneBlending(Mogre::SceneBlendFactor sourceFactor, Mogre::SceneBlendFactor destFactor);

		void _notifyNeedsRecompile();

		//void SetLodLevels(Mogre::Material::Const_LodDistanceList^ lodDistances);

		void Touch();

		bool ApplyTextureAliases(Mogre::Const_AliasTextureNamePairList^ aliasList, bool apply);
		bool ApplyTextureAliases(Mogre::Const_AliasTextureNamePairList^ aliasList);

		DEFINE_MANAGED_NATIVE_CONVERSIONS(Material);

	internal:
		property Ogre::Material* UnmanagedPointer
		{
			Ogre::Material* get()
			{
				return static_cast<Ogre::Material*>(_native);
			}
		}
	};

	public ref class MaterialPtr : public Material
	{
	public protected:
		Ogre::MaterialPtr* _sharedPtr;

		MaterialPtr(Ogre::MaterialPtr& sharedPtr) : Material(sharedPtr.getPointer())
		{
			_sharedPtr = new Ogre::MaterialPtr(sharedPtr);
		}

		!MaterialPtr()
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

		~MaterialPtr()
		{
			this->!MaterialPtr();
		}

	public:
		static operator MaterialPtr ^ (const Ogre::MaterialPtr& ptr)
		{
			if (ptr.isNull()) return nullptr;
			Ogre::MaterialPtr wrapperPtr = Ogre::MaterialPtr(ptr);
			wrapperPtr.setUseCount(wrapperPtr.useCount() + 1);
			return gcnew MaterialPtr(wrapperPtr);
		}

		static operator Ogre::MaterialPtr& (MaterialPtr^ t)
		{
			if (CLR_NULL == t) return Ogre::MaterialPtr();
			return *(t->_sharedPtr);
		}

		static operator Ogre::MaterialPtr* (MaterialPtr^ t)
		{
			if (CLR_NULL == t) return nullptr;
			return t->_sharedPtr;
		}

		static MaterialPtr^ FromResourcePtr(ResourcePtr^ ptr)
		{
			if (CLR_NULL == ptr) return nullptr;
			void* castptr = dynamic_cast<Ogre::Material*>(ptr->_native);
			if (castptr == 0) throw gcnew InvalidCastException("The underlying type of the ResourcePtr object is not of type Material.");
			Ogre::MaterialPtr materialPtr = ptr->_sharedPtr->staticCast<Ogre::Material>();
			return gcnew MaterialPtr(materialPtr);
		}

		static operator MaterialPtr ^ (ResourcePtr^ ptr)
		{
			MaterialPtr^ res = FromResourcePtr(ptr);
			// invalidate previous pointer and return converted pointer
			delete ptr;
			return res;
		}

		MaterialPtr(Material^ obj) : Material(obj->_native)
		{
			_sharedPtr = new Ogre::MaterialPtr(static_cast<Ogre::Material*>(obj->_native));
		}

		virtual bool Equals(Object^ obj) override
		{
			MaterialPtr^ clr = dynamic_cast<MaterialPtr^>(obj);
			if (clr == CLR_NULL)
			{
				return false;
			}

			return (_native == clr->_native);
		}
		bool Equals(MaterialPtr^ obj)
		{
			if (obj == CLR_NULL)
			{
				return false;
			}

			return (_native == obj->_native);
		}

		static bool operator == (MaterialPtr^ val1, MaterialPtr^ val2)
		{
			if ((Object^)val1 == (Object^)val2) return true;
			if ((Object^)val1 == nullptr || (Object^)val2 == nullptr) return false;
			return (val1->_native == val2->_native);
		}

		static bool operator != (MaterialPtr^ val1, MaterialPtr^ val2)
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

		property Material^ Target
		{
			Material^ get()
			{
				return static_cast<Ogre::Material*>(_native);
			}
		}
	};

	public ref class MaterialManager : public ResourceManager
	{
	private protected:
		static MaterialManager^ _singleton;

	public protected:
		MaterialManager(Ogre::MaterialManager* obj) : ResourceManager(obj)
		{
		}

	public:
		MaterialManager();

		static property MaterialManager^ Singleton
		{
			MaterialManager^ get()
			{
				if (_singleton == CLR_NULL)
				{
					Ogre::MaterialManager* ptr = Ogre::MaterialManager::getSingletonPtr();
					if (ptr) _singleton = gcnew MaterialManager(ptr);
				}
				return _singleton;
			}
		}

		static property String^ DEFAULT_SCHEME_NAME
		{
		public:
			String^ get();
		public:
			void set(String^ value);
		}

		property String^ ActiveScheme
		{
		public:
			String^ get();
		public:
			void set(String^ schemeName);
		}

		property unsigned int DefaultAnisotropy
		{
		public:
			unsigned int get();
		public:
			void set(unsigned int maxAniso);
		}

		Mogre::MaterialPtr^ GetByName(String^ name);
		Mogre::MaterialPtr^ GetByName(String^ name, String^ groupName);

		void Initialise();

		void ParseScript(Mogre::DataStreamPtr^ stream, String^ groupName);

		void SetDefaultTextureFiltering(Mogre::TextureFilterOptions fo);

		void SetDefaultTextureFiltering(Mogre::FilterType ftype, Mogre::FilterOptions opts);

		void SetDefaultTextureFiltering(Mogre::FilterOptions minFilter, Mogre::FilterOptions magFilter, Mogre::FilterOptions mipFilter);

		Mogre::FilterOptions GetDefaultTextureFiltering(Mogre::FilterType ftype);

		Mogre::MaterialPtr^ GetDefaultSettings();

		unsigned short _getSchemeIndex(String^ name);

		String^ _getSchemeName(unsigned short index);

		unsigned short _getActiveSchemeIndex();
	};
}