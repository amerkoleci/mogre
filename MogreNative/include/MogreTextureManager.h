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
		TEX_TYPE_CUBE_MAP = Ogre::TEX_TYPE_CUBE_MAP,
		TEX_TYPE_2D_ARRAY = Ogre::TEX_TYPE_2D_ARRAY,
		TEX_TYPE_2D_RECT = Ogre::TEX_TYPE_2D_RECT
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

	public enum class ImageFlags
	{
		IF_COMPRESSED = Ogre::IF_COMPRESSED,
		IF_CUBEMAP = Ogre::IF_CUBEMAP,
		IF_3D_TEXTURE = Ogre::IF_3D_TEXTURE
	};

	ref class TexturePtr;
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

		Image(intptr_t obj) : _native((Ogre::Image*)obj), _createdByCLR(false)
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
	public protected:
		Texture(Ogre::Texture* obj) : Resource(obj)
		{
		}

		Texture(intptr_t ptr) : Resource(ptr)
		{
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

		void CopyToTexture(Mogre::TexturePtr^ target);

		void LoadImage(Mogre::Image^ img);
		void LoadRawData(Mogre::DataStreamPtr^ stream, Mogre::ushort uWidth, Mogre::ushort uHeight, Mogre::PixelFormat eFormat);

		void SetDesiredBitDepths(Mogre::ushort integerBits, Mogre::ushort floatBits);

		Mogre::HardwarePixelBufferSharedPtr^ GetBuffer(size_t face, size_t mipmap);
		Mogre::HardwarePixelBufferSharedPtr^ GetBuffer(size_t face);
		Mogre::HardwarePixelBufferSharedPtr^ GetBuffer();

		DEFINE_MANAGED_NATIVE_CONVERSIONS(Texture);

	internal:
		property Ogre::Texture* UnmanagedPointer
		{
			Ogre::Texture* get()
			{
				return static_cast<Ogre::Texture*>(_native);
			}
		}
	};

	public ref class TexturePtr : public Texture
	{
	public protected:
		Ogre::TexturePtr* _sharedPtr;

		TexturePtr(Ogre::TexturePtr& sharedPtr) : Texture(sharedPtr.getPointer())
		{
			_sharedPtr = new Ogre::TexturePtr(sharedPtr);
		}

		!TexturePtr()
		{
			if (_sharedPtr != 0)
			{
				delete _sharedPtr;
				_sharedPtr = 0;
			}
		}

		~TexturePtr()
		{
			this->!TexturePtr();
		}

	public:
		DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_SHAREDPTR(TexturePtr);

		static TexturePtr^ FromResourcePtr(ResourcePtr^ ptr)
		{
			return (TexturePtr^)ptr;
		}

		static operator TexturePtr ^ (ResourcePtr^ ptr)
		{
			if (CLR_NULL == ptr) return nullptr;
			void* castptr = dynamic_cast<Ogre::Texture*>(ptr->_native);
			if (castptr == 0) throw gcnew InvalidCastException("The underlying type of the ResourcePtr object is not of type Texture.");
			return gcnew TexturePtr(Ogre::TexturePtr(ptr->_sharedPtr->dynamicCast<Ogre::Texture>()));
		}

		TexturePtr(Texture^ obj) : Texture(obj->_native)
		{
			_sharedPtr = new Ogre::TexturePtr(static_cast<Ogre::Texture*>(obj->_native));
		}

		virtual bool Equals(Object^ obj) override
		{
			TexturePtr^ clr = dynamic_cast<TexturePtr^>(obj);
			if (clr == CLR_NULL)
			{
				return false;
			}

			return (_native == clr->_native);
		}
		bool Equals(TexturePtr^ obj)
		{
			if (obj == CLR_NULL)
			{
				return false;
			}

			return (_native == obj->_native);
		}

		static bool operator == (TexturePtr^ val1, TexturePtr^ val2)
		{
			if ((Object^)val1 == (Object^)val2) return true;
			if ((Object^)val1 == nullptr || (Object^)val2 == nullptr) return false;
			return (val1->_native == val2->_native);
		}

		static bool operator != (TexturePtr^ val1, TexturePtr^ val2)
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

		property Texture^ Target
		{
			Texture^ get()
			{
				return static_cast<Ogre::Texture*>(_native);
			}
		}
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
	};
}