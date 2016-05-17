#pragma once

#include "OgreTextureManager.h"
#include "MogreResourceManager.h"
#include "MogrePixelFormat.h"

namespace Mogre
{
	public enum class TextureMipmap
	{
		MIP_UNLIMITED = Ogre::MIP_UNLIMITED,
		MIP_DEFAULT = Ogre::MIP_DEFAULT
	};

	public enum class TextureType
	{
		TEX_TYPE_1D = Ogre::TEX_TYPE_1D,
		TEX_TYPE_2D = Ogre::TEX_TYPE_2D,
		TEX_TYPE_3D = Ogre::TEX_TYPE_3D,
		TEX_TYPE_CUBE_MAP = Ogre::TEX_TYPE_CUBE_MAP
	};

	public enum class TextureUsage
	{
		TU_STATIC = Ogre::TU_STATIC,
		TU_DYNAMIC = Ogre::TU_DYNAMIC,
		TU_WRITE_ONLY = Ogre::TU_WRITE_ONLY,
		TU_STATIC_WRITE_ONLY = Ogre::TU_STATIC_WRITE_ONLY,
		TU_DYNAMIC_WRITE_ONLY = Ogre::TU_DYNAMIC_WRITE_ONLY,
		TU_DYNAMIC_WRITE_ONLY_DISCARDABLE = Ogre::TU_DYNAMIC_WRITE_ONLY_DISCARDABLE,
		TU_AUTOMIPMAP = Ogre::TU_AUTOMIPMAP,
		TU_RENDERTARGET = Ogre::TU_RENDERTARGET,
		TU_DEFAULT = Ogre::TU_DEFAULT
	};

	public ref class TextureManager : public ResourceManager
	{
	private protected:
		static TextureManager^ _singleton;

	public protected:
		TextureManager(Ogre::TextureManager* obj) : ResourceManager(obj)
		{
		}

	public:

		static property TextureManager^ Singleton
		{
			TextureManager^ get()
			{
				if (_singleton == CLR_NULL)
				{
					Ogre::TextureManager* ptr = Ogre::TextureManager::getSingletonPtr();
					if (ptr) _singleton = gcnew TextureManager(ptr);
				}
				return _singleton;
			}
		}

		property size_t DefaultNumMipmaps
		{
		public:
			size_t get();
		public:
			void set(size_t num);
		}

		property Ogre::ushort PreferredFloatBitDepth
		{
		public:
			Ogre::ushort get();
		}

		property Ogre::ushort PreferredIntegerBitDepth
		{
		public:
			Ogre::ushort get();
		}

		/*Mogre::TexturePtr^ Load(String^ name, String^ group, Mogre::TextureType texType, int numMipmaps, Mogre::Real gamma, bool isAlpha, Mogre::PixelFormat desiredFormat);
		Mogre::TexturePtr^ Load(String^ name, String^ group, Mogre::TextureType texType, int numMipmaps, Mogre::Real gamma, bool isAlpha);
		Mogre::TexturePtr^ Load(String^ name, String^ group, Mogre::TextureType texType, int numMipmaps, Mogre::Real gamma);
		Mogre::TexturePtr^ Load(String^ name, String^ group, Mogre::TextureType texType, int numMipmaps);
		Mogre::TexturePtr^ Load(String^ name, String^ group, Mogre::TextureType texType);
		Mogre::TexturePtr^ Load(String^ name, String^ group);

		Mogre::TexturePtr^ LoadImage(String^ name, String^ group, Mogre::Image^ img, Mogre::TextureType texType, int iNumMipmaps, Mogre::Real gamma, bool isAlpha, Mogre::PixelFormat desiredFormat);
		Mogre::TexturePtr^ LoadImage(String^ name, String^ group, Mogre::Image^ img, Mogre::TextureType texType, int iNumMipmaps, Mogre::Real gamma, bool isAlpha);
		Mogre::TexturePtr^ LoadImage(String^ name, String^ group, Mogre::Image^ img, Mogre::TextureType texType, int iNumMipmaps, Mogre::Real gamma);
		Mogre::TexturePtr^ LoadImage(String^ name, String^ group, Mogre::Image^ img, Mogre::TextureType texType, int iNumMipmaps);
		Mogre::TexturePtr^ LoadImage(String^ name, String^ group, Mogre::Image^ img, Mogre::TextureType texType);
		Mogre::TexturePtr^ LoadImage(String^ name, String^ group, Mogre::Image^ img);

		Mogre::TexturePtr^ LoadRawData(String^ name, String^ group, Mogre::DataStreamPtr^ stream, Mogre::ushort uWidth, Mogre::ushort uHeight, Mogre::PixelFormat format, Mogre::TextureType texType, int iNumMipmaps, Mogre::Real gamma);
		Mogre::TexturePtr^ LoadRawData(String^ name, String^ group, Mogre::DataStreamPtr^ stream, Mogre::ushort uWidth, Mogre::ushort uHeight, Mogre::PixelFormat format, Mogre::TextureType texType, int iNumMipmaps);
		Mogre::TexturePtr^ LoadRawData(String^ name, String^ group, Mogre::DataStreamPtr^ stream, Mogre::ushort uWidth, Mogre::ushort uHeight, Mogre::PixelFormat format, Mogre::TextureType texType);
		Mogre::TexturePtr^ LoadRawData(String^ name, String^ group, Mogre::DataStreamPtr^ stream, Mogre::ushort uWidth, Mogre::ushort uHeight, Mogre::PixelFormat format);

		Mogre::TexturePtr^ CreateManual(String^ name, String^ group, Mogre::TextureType texType, Mogre::uint width, Mogre::uint height, Mogre::uint depth, int num_mips, Mogre::PixelFormat format, int usage, Mogre::IManualResourceLoader^ loader);
		Mogre::TexturePtr^ CreateManual(String^ name, String^ group, Mogre::TextureType texType, Mogre::uint width, Mogre::uint height, Mogre::uint depth, int num_mips, Mogre::PixelFormat format, int usage);
		Mogre::TexturePtr^ CreateManual(String^ name, String^ group, Mogre::TextureType texType, Mogre::uint width, Mogre::uint height, Mogre::uint depth, int num_mips, Mogre::PixelFormat format);

		Mogre::TexturePtr^ CreateManual(String^ name, String^ group, Mogre::TextureType texType, Mogre::uint width, Mogre::uint height, int num_mips, Mogre::PixelFormat format, int usage, Mogre::IManualResourceLoader^ loader);
		Mogre::TexturePtr^ CreateManual(String^ name, String^ group, Mogre::TextureType texType, Mogre::uint width, Mogre::uint height, int num_mips, Mogre::PixelFormat format, int usage);
		Mogre::TexturePtr
		^ CreateManual(String^ name, String^ group, Mogre::TextureType texType, Mogre::uint width, Mogre::uint height, int num_mips, Mogre::PixelFormat format);
		*/
		void SetPreferredIntegerBitDepth(Ogre::ushort bits, bool reloadTextures);
		void SetPreferredIntegerBitDepth(Ogre::ushort bits);

		void SetPreferredFloatBitDepth(Ogre::ushort bits, bool reloadTextures);
		void SetPreferredFloatBitDepth(Ogre::ushort bits);

		void SetPreferredBitDepths(Ogre::ushort integerBits, Ogre::ushort floatBits, bool reloadTextures);
		void SetPreferredBitDepths(Ogre::ushort integerBits, Ogre::ushort floatBits);

		bool IsFormatSupported(Mogre::TextureType ttype, Mogre::PixelFormat format, int usage);

		bool IsEquivalentFormatSupported(Mogre::TextureType ttype, Mogre::PixelFormat format, int usage);

		Mogre::PixelFormat GetNativeFormat(Mogre::TextureType ttype, Mogre::PixelFormat format, int usage);

		bool IsHardwareFilteringSupported(Mogre::TextureType ttype, Mogre::PixelFormat format, int usage, bool preciseFormatOnly);
		bool IsHardwareFilteringSupported(Mogre::TextureType ttype, Mogre::PixelFormat format, int usage);
	};
}