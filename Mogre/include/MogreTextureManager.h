#pragma once

#include "OgreTexture.h"
#include "OgreTextureManager.h"
#include "OgreImage.h"
#include "MogreResourceManager.h"
#include "MogrePixelFormat.h"
#include "MogrePixelBox.h"

#ifdef LoadImage
#	undef LoadImage
#endif

namespace Mogre
{
	ref class DataStreamPtr;
	ref class HardwarePixelBufferSharedPtr;

	public ref class Image : IMogreDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	public:
		typedef Mogre::Box Box;
		typedef Mogre::Rect Rect;

		enum class Filter
		{
			FILTER_NEAREST = Ogre::Image::FILTER_NEAREST,
			FILTER_LINEAR = Ogre::Image::FILTER_LINEAR,
			FILTER_BILINEAR = Ogre::Image::FILTER_BILINEAR,
			FILTER_BOX = Ogre::Image::FILTER_BOX,
			FILTER_TRIANGLE = Ogre::Image::FILTER_TRIANGLE,
			FILTER_BICUBIC = Ogre::Image::FILTER_BICUBIC
		};

	private protected:
		virtual void ClearNativePtr()
		{
			_native = 0;
		}

	public protected:
		Image(Ogre::Image* obj) : _native(obj), _createdByCLR(false)
		{
		}

		Ogre::Image* _native;
		bool _createdByCLR;

	public:
		~Image();
	protected:
		!Image();

	public:
		Image();
		Image(Mogre::Image^ img);

		property bool IsDisposed
		{
			virtual bool get();
		}

		property Mogre::uchar BPP
		{
		public:
			Mogre::uchar get();
		}

		property Mogre::uchar* Data
		{
		public:
			Mogre::uchar* get();
		}

		property size_t Depth
		{
		public:
			size_t get();
		}

		property Mogre::PixelFormat Format
		{
		public:
			Mogre::PixelFormat get();
		}

		property bool HasAlpha
		{
		public:
			bool get();
		}

		property size_t Height
		{
		public:
			size_t get();
		}

		property size_t NumFaces
		{
		public:
			size_t get();
		}

		property size_t NumMipmaps
		{
		public:
			size_t get();
		}

		property size_t RowSpan
		{
		public:
			size_t get();
		}

		property size_t Size
		{
		public:
			size_t get();
		}

		property size_t Width
		{
		public:
			size_t get();
		}

		void CopyTo(Image^ dest)
		{
			if (_native == NULL) throw gcnew Exception("The underlying native object for the caller is null.");
			if (dest->_native == NULL) throw gcnew ArgumentException("The underlying native object for parameter 'dest' is null.");

			*(dest->_native) = *_native;
		}

		Mogre::Image^ FlipAroundY();

		Mogre::Image^ FlipAroundX();

		Mogre::Image^ LoadDynamicImage(Mogre::uchar* pData, size_t uWidth, size_t uHeight, size_t depth, Mogre::PixelFormat eFormat, bool autoDelete, size_t numFaces, size_t numMipMaps);
		Mogre::Image^ LoadDynamicImage(Mogre::uchar* pData, size_t uWidth, size_t uHeight, size_t depth, Mogre::PixelFormat eFormat, bool autoDelete, size_t numFaces);
		Mogre::Image^ LoadDynamicImage(Mogre::uchar* pData, size_t uWidth, size_t uHeight, size_t depth, Mogre::PixelFormat eFormat, bool autoDelete);
		Mogre::Image^ LoadDynamicImage(Mogre::uchar* pData, size_t uWidth, size_t uHeight, size_t depth, Mogre::PixelFormat eFormat);

		Mogre::Image^ LoadDynamicImage(Mogre::uchar* pData, size_t uWidth, size_t uHeight, Mogre::PixelFormat eFormat);

		Mogre::Image^ LoadRawData(Mogre::DataStreamPtr^ stream, size_t uWidth, size_t uHeight, size_t uDepth, Mogre::PixelFormat eFormat, size_t numFaces, size_t numMipMaps);
		Mogre::Image^ LoadRawData(Mogre::DataStreamPtr^ stream, size_t uWidth, size_t uHeight, size_t uDepth, Mogre::PixelFormat eFormat, size_t numFaces);
		Mogre::Image^ LoadRawData(Mogre::DataStreamPtr^ stream, size_t uWidth, size_t uHeight, size_t uDepth, Mogre::PixelFormat eFormat);

		Mogre::Image^ LoadRawData(Mogre::DataStreamPtr^ stream, size_t uWidth, size_t uHeight, Mogre::PixelFormat eFormat);

		Mogre::Image^ Load(String^ strFileName, String^ groupName);

		Mogre::Image^ Load(Mogre::DataStreamPtr^ stream, String^ type);

		void Save(String^ filename);

		bool HasFlag(Mogre::ImageFlags imgFlag);

		Mogre::ColourValue GetColourAt(int x, int y, int z);

		Mogre::PixelBox GetPixelBox(size_t face, size_t mipmap);
		Mogre::PixelBox GetPixelBox(size_t face);
		Mogre::PixelBox GetPixelBox();

		void Resize(Mogre::ushort width, Mogre::ushort height, Mogre::Image::Filter filter);
		void Resize(Mogre::ushort width, Mogre::ushort height);

		static void ApplyGamma([Out] Mogre::uchar% buffer, Mogre::Real gamma, size_t size, Mogre::uchar bpp);

		static void Scale(Mogre::PixelBox src, Mogre::PixelBox dst, Mogre::Image::Filter filter);
		static void Scale(Mogre::PixelBox src, Mogre::PixelBox dst);

		static size_t CalculateSize(size_t mipmaps, size_t faces, size_t width, size_t height, size_t depth, Mogre::PixelFormat format);

		DEFINE_MANAGED_NATIVE_CONVERSIONS(Image);

	internal:
		property Ogre::Image* UnmanagedPointer
		{
			Ogre::Image* get()
			{
				return _native;
			}
		}
	};

	public ref class Texture : public Resource
	{
	private:
		Ogre::TexturePtr* _texturePtr;
		bool _isDisposed;

	public protected:
		Texture(Ogre::TexturePtr* ptr) : Resource(ptr->get()), _texturePtr(ptr)
		{
		}

	public:
		~Texture();
	protected:
		!Texture();

	public:
		property bool IsDisposed
		{
			virtual bool get();
		}

	public:
		property size_t Depth
		{
		public:
			size_t get();
		public:
			void set(size_t d);
		}

		property Mogre::ushort DesiredFloatBitDepth
		{
		public:
			Mogre::ushort get();
		public:
			void set(Mogre::ushort bits);
		}

		property Mogre::PixelFormat DesiredFormat
		{
		public:
			Mogre::PixelFormat get();
		}

		property Mogre::ushort DesiredIntegerBitDepth
		{
		public:
			Mogre::ushort get();
		public:
			void set(Mogre::ushort bits);
		}

		property Mogre::PixelFormat Format
		{
		public:
			Mogre::PixelFormat get();
		public:
			void set(Mogre::PixelFormat pf);
		}

		property float Gamma
		{
		public:
			float get();
		public:
			void set(float g);
		}

		property bool HasAlpha
		{
		public:
			bool get();
		}

		property size_t Height
		{
		public:
			size_t get();
		public:
			void set(size_t h);
		}

		property bool MipmapsHardwareGenerated
		{
		public:
			bool get();
		}

		property size_t NumFaces
		{
		public:
			size_t get();
		}

		property size_t NumMipmaps
		{
		public:
			size_t get();
		public:
			void set(size_t num);
		}

		property size_t SrcDepth
		{
		public:
			size_t get();
		}

		property Mogre::PixelFormat SrcFormat
		{
		public:
			Mogre::PixelFormat get();
		}

		property size_t SrcHeight
		{
		public:
			size_t get();
		}

		property size_t SrcWidth
		{
		public:
			size_t get();
		}

		property Mogre::TextureType TextureType
		{
		public:
			Mogre::TextureType get();
		public:
			void set(Mogre::TextureType ttype);
		}

		property bool TreatLuminanceAsAlpha
		{
		public:
			bool get();
		public:
			void set(bool asAlpha);
		}

		property int Usage
		{
		public:
			int get();
		public:
			void set(int u);
		}

		property size_t Width
		{
		public:
			size_t get();
		public:
			void set(size_t w);
		}

		void CreateInternalResources();

		void FreeInternalResources();

		void CopyToTexture(Mogre::Texture^ target);

		void LoadImage(Mogre::Image^ img);
		void LoadRawData(Mogre::DataStreamPtr^ stream, Mogre::ushort uWidth, Mogre::ushort uHeight, Mogre::PixelFormat eFormat);

		void SetDesiredBitDepths(Mogre::ushort integerBits, Mogre::ushort floatBits);

		Mogre::HardwarePixelBufferSharedPtr^ GetBuffer(size_t face, size_t mipmap);
		Mogre::HardwarePixelBufferSharedPtr^ GetBuffer(size_t face);
		Mogre::HardwarePixelBufferSharedPtr^ GetBuffer();

	internal:
		property Ogre::TexturePtr* UnmanagedPointer
		{
			Ogre::TexturePtr* get()
			{
				return _texturePtr;
			}
		}
	};

	typedef Mogre::Texture TexturePtr;

	public ref class TextureManager : public ResourceManager
	{
	private protected:
		static TextureManager^ _singleton;
		System::Collections::Generic::Dictionary<String^, System::Collections::Generic::Dictionary<String^, Texture^>^ >^ _textures;

	public protected:
		TextureManager(Ogre::TextureManager* obj) : ResourceManager(obj)
		{
			_textures = gcnew System::Collections::Generic::Dictionary<String^, System::Collections::Generic::Dictionary<String^, Texture^>^>();
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

		Mogre::Texture^ GetByName(String^ name);
		Mogre::Texture^ GetByName(String^ name, String^ groupName);

		Mogre::TexturePtr^ Load(String^ name, String^ group, Mogre::TextureType texType, int numMipmaps, Mogre::Real gamma, bool isAlpha, Mogre::PixelFormat desiredFormat);
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

		//Mogre::TexturePtr^ CreateManual(String^ name, String^ group, Mogre::TextureType texType, Mogre::uint width, Mogre::uint height, Mogre::uint depth, int num_mips, Mogre::PixelFormat format, int usage, Mogre::IManualResourceLoader^ loader);
		Mogre::TexturePtr^ CreateManual(String^ name, String^ group, Mogre::TextureType texType, Mogre::uint width, Mogre::uint height, Mogre::uint depth, int num_mips, Mogre::PixelFormat format, int usage);
		Mogre::TexturePtr^ CreateManual(String^ name, String^ group, Mogre::TextureType texType, Mogre::uint width, Mogre::uint height, Mogre::uint depth, int num_mips, Mogre::PixelFormat format);

		//Mogre::TexturePtr^ CreateManual(String^ name, String^ group, Mogre::TextureType texType, Mogre::uint width, Mogre::uint height, int num_mips, Mogre::PixelFormat format, int usage, Mogre::IManualResourceLoader^ loader);
		Mogre::TexturePtr^ CreateManual(String^ name, String^ group, Mogre::TextureType texType, Mogre::uint width, Mogre::uint height, int num_mips, Mogre::PixelFormat format, int usage);
		Mogre::TexturePtr^ CreateManual(String^ name, String^ group, Mogre::TextureType texType, Mogre::uint width, Mogre::uint height, int num_mips, Mogre::PixelFormat format);

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

	internal:
		void Shutdown();
		void RemoveTextureInternal(Mogre::Texture^ texture);

	private:
		System::Collections::Generic::Dictionary<String^, Texture^>^ GetTextureCache(String^ groupName);
	};
}