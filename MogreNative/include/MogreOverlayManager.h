#pragma once

#include "OgreOverlaySystem.h"
#include "OgreOverlayManager.h"
#include "OgreOverlay.h"
#include "OgreOverlayElement.h"
#include "OgreOverlayContainer.h"
#include "MogreCommon.h"
#include "MogreStringVector.h"
#include "STLContainerWrappers.h"
#include "IteratorWrapper.h"
#include "Marshalling.h"

namespace Mogre
{
	ref class SceneManager;
	ref class OverlayContainer;
	ref class Overlay;
	ref class Technique;
	ref class Material;
	ref class MaterialPtr;
	ref class Camera;
	ref class SceneNode;
	ref class DataStreamPtr;

	public enum class GuiVerticalAlignment
	{
		GVA_TOP = Ogre::GVA_TOP,
		GVA_CENTER = Ogre::GVA_CENTER,
		GVA_BOTTOM = Ogre::GVA_BOTTOM
	};

	public enum class GuiHorizontalAlignment
	{
		GHA_LEFT = Ogre::GHA_LEFT,
		GHA_CENTER = Ogre::GHA_CENTER,
		GHA_RIGHT = Ogre::GHA_RIGHT
	};

	public enum class GuiMetricsMode
	{
		GMM_RELATIVE = Ogre::GMM_RELATIVE,
		GMM_PIXELS = Ogre::GMM_PIXELS,
		GMM_RELATIVE_ASPECT_ADJUSTED = Ogre::GMM_RELATIVE_ASPECT_ADJUSTED
	};

	public ref class OverlayElement : IMogreDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	public protected:
		Ogre::OverlayElement* _native;
		bool _createdByCLR;

		OverlayElement(IntPtr ptr) : _native((Ogre::OverlayElement*)ptr.ToPointer())
		{

		}

		OverlayElement(Ogre::OverlayElement* obj) : _native(obj)
		{

		}

	public:
		~OverlayElement();
	protected:
		!OverlayElement();

	public:
		property bool IsDisposed
		{
			virtual bool get()
			{
				return _native == nullptr;
			}
		}

		property String^ Caption
		{
		public:
			String^ get();
		public:
			void set(String^ text);
		}

		property Mogre::ColourValue Colour
		{
		public:
			Mogre::ColourValue get();
		public:
			void set(Mogre::ColourValue col);
		}

		property Mogre::Real Height
		{
		public:
			Mogre::Real get();
		public:
			void set(Mogre::Real height);
		}

		property Mogre::GuiHorizontalAlignment HorizontalAlignment
		{
		public:
			Mogre::GuiHorizontalAlignment get();
		public:
			void set(Mogre::GuiHorizontalAlignment gha);
		}

		property bool IsCloneable
		{
		public:
			bool get();
		}

		property bool IsContainer
		{
		public:
			bool get();
		}

		property bool IsEnabled
		{
		public:
			bool get();
		}

		property bool IsKeyEnabled
		{
		public:
			bool get();
		}

		property bool IsVisible
		{
		public:
			bool get();
		}

		property Mogre::Real Left
		{
		public:
			Mogre::Real get();
		public:
			void set(Mogre::Real left);
		}

		property String^ MaterialName
		{
		public:
			String^ get();
		public:
			void set(String^ matName);
		}

		property Mogre::GuiMetricsMode MetricsMode
		{
		public:
			Mogre::GuiMetricsMode get();
		public:
			void set(Mogre::GuiMetricsMode gmm);
		}

		property String^ Name
		{
		public:
			String^ get();
		}

		property Mogre::OverlayContainer^ Parent
		{
		public:
			Mogre::OverlayContainer^ get();
		}

		property Mogre::OverlayElement^ SourceTemplate
		{
		public:
			Mogre::OverlayElement^ get();
		}

		property Mogre::Real Top
		{
		public:
			Mogre::Real get();
		public:
			void set(Mogre::Real Top);
		}

		property String^ TypeName
		{
		public:
			String^ get();
		}

		property Mogre::GuiVerticalAlignment VerticalAlignment
		{
		public:
			Mogre::GuiVerticalAlignment get();
		public:
			void set(Mogre::GuiVerticalAlignment gva);
		}

		property Mogre::Real Width
		{
		public:
			Mogre::Real get();
		public:
			void set(Mogre::Real width);
		}

		property Mogre::ushort ZOrder
		{
		public:
			Mogre::ushort get();
		}

		void Initialise();

		void Show();

		void Hide();

		void SetEnabled(bool b);

		void SetDimensions(Mogre::Real width, Mogre::Real height);

		void SetPosition(Mogre::Real left, Mogre::Real top);

		Mogre::Real _getLeft();

		Mogre::Real _getTop();

		Mogre::Real _getWidth();

		Mogre::Real _getHeight();

		void _setLeft(Mogre::Real left);

		void _setTop(Mogre::Real top);

		void _setWidth(Mogre::Real width);

		void _setHeight(Mogre::Real height);

		void _setPosition(Mogre::Real left, Mogre::Real top);

		void _setDimensions(Mogre::Real width, Mogre::Real height);

		virtual Mogre::MaterialPtr^ GetMaterial();

		virtual void GetWorldTransforms(Mogre::Matrix4* xform);

		void _positionsOutOfDate();

		void _update();

		void _updateFromParent();

		void _notifyParent(Mogre::OverlayContainer^ parent, Mogre::Overlay^ overlay);

		Mogre::Real _getDerivedLeft();

		Mogre::Real _getDerivedTop();

		Mogre::Real _getRelativeWidth();

		Mogre::Real _getRelativeHeight();

		//void _getClippingRegion(Mogre::Rectangle clippingRegion);

		void _notifyZOrder(Mogre::ushort newZOrder);

		void _notifyWorldTransforms(Mogre::Matrix4 xform);

		void _notifyViewport();

		//void _updateRenderQueue(Mogre::RenderQueue^ queue);

		bool Contains(Mogre::Real x, Mogre::Real y);

		Mogre::OverlayElement^ FindElementAt(Mogre::Real x, Mogre::Real y);

		void SetCloneable(bool c);

		void _setParent(Mogre::OverlayContainer^ parent);

		virtual Mogre::Real GetSquaredViewDepth(Mogre::Camera^ cam);

		//virtual Mogre::Const_LightList^ GetLights();

		void CopyFromTemplate(Mogre::OverlayElement^ templateOverlay);

		Mogre::OverlayElement^ Clone(String^ instanceName);

		//Mogre::Const_ParameterList^ GetParameters();
		virtual bool SetParameter(String^ name, String^ value);
		virtual void SetParameterList(Mogre::Const_NameValuePairList^ paramList);
		virtual String^ GetParameter(String^ name);
		//virtual void CopyParametersTo(Mogre::IStringInterface^ dest);

	public:
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

		void SetCustomParameter(size_t index, Mogre::Vector4 value);
		Mogre::Vector4 GetCustomParameter(size_t index);

		DEFINE_MANAGED_NATIVE_CONVERSIONS(OverlayElement);

	internal:
		property Ogre::OverlayElement* UnmanagedPointer
		{
			Ogre::OverlayElement* get() { return _native; }
		}
	};

	public ref class OverlayContainer : public OverlayElement
	{
	public protected:
		OverlayContainer(Ogre::OverlayContainer* obj) : OverlayElement(obj)
		{
		}

		OverlayContainer(IntPtr ptr) : OverlayElement(ptr)
		{
		}

		DEFINE_MANAGED_NATIVE_CONVERSIONS(OverlayContainer);

	internal:
		property Ogre::OverlayContainer* UnmanagedPointer
		{
			Ogre::OverlayContainer* get() { return static_cast<Ogre::OverlayContainer*>(_native); }
		}
	};

	public ref class Overlay : IMogreDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	public:
		INC_DECLARE_STLLIST(OverlayContainerList, Mogre::OverlayContainer^, Ogre::OverlayContainer*, public:, private:);
		INC_DECLARE_ITERATOR(Overlay2DElementsIterator, Ogre::Overlay::Overlay2DElementsIterator, Mogre::Overlay::OverlayContainerList, Mogre::OverlayContainer^, Ogre::OverlayContainer*);


	public protected:
		Ogre::Overlay* _native;
		bool _createdByCLR;

		Overlay(IntPtr ptr) : _native((Ogre::Overlay*)ptr.ToPointer())
		{

		}

		Overlay(Ogre::Overlay* obj) : _native(obj)
		{

		}

	public:
		~Overlay();
	protected:
		!Overlay();

	public:
		Overlay(String^ name);

		property bool IsDisposed
		{
			virtual bool get()
			{
				return _native == nullptr;
			}
		}

		property bool IsInitialised
		{
		public:
			bool get();
		}

		property bool IsVisible
		{
		public:
			bool get();
		}

		property String^ Name
		{
		public:
			String^ get();
		}

		property String^ Origin
		{
		public:
			String^ get();
		}

		property Mogre::Real ScaleX
		{
		public:
			Mogre::Real get();
		}

		property Mogre::Real ScaleY
		{
		public:
			Mogre::Real get();
		}

		property Mogre::Real ScrollX
		{
		public:
			Mogre::Real get();
		}

		property Mogre::Real ScrollY
		{
		public:
			Mogre::Real get();
		}

		property Mogre::ushort ZOrder
		{
		public:
			Mogre::ushort get();
		public:
			void set(Mogre::ushort zorder);
		}

		Mogre::OverlayContainer^ GetChild(String^ name);

		void Show();

		void Hide();

		void Add2D(Mogre::OverlayContainer^ cont);

		void Remove2D(Mogre::OverlayContainer^ cont);

		void Add3D(Mogre::SceneNode^ node);

		void Remove3D(Mogre::SceneNode^ node);

		void Clear();

		void SetScroll(Mogre::Real x, Mogre::Real y);

		void Scroll(Mogre::Real xoff, Mogre::Real yoff);

		void SetRotate(Mogre::Radian angle);

		Mogre::Radian GetRotate();

		void Rotate(Mogre::Radian angle);

		void SetScale(Mogre::Real x, Mogre::Real y);

		//void _findVisibleObjects(Mogre::Camera^ cam, Mogre::RenderQueue^ queue);

		Mogre::OverlayElement^ FindElementAt(Mogre::Real x, Mogre::Real y);

		Mogre::Overlay::Overlay2DElementsIterator^ Get2DElementsIterator();
		void _notifyOrigin(String^ origin);

		DEFINE_MANAGED_NATIVE_CONVERSIONS(Overlay);

	internal:
		property Ogre::Overlay* UnmanagedPointer
		{
			Ogre::Overlay* get() { return _native; }
		}
	};

	public ref class OverlaySystem : IMogreDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	private:
		SceneManager^ _sceneManager;

	public protected:
		Ogre::OverlaySystem* _native;
		bool _createdByCLR;

		OverlaySystem(IntPtr ptr) : _native((Ogre::OverlaySystem*)ptr.ToPointer())
		{

		}

		OverlaySystem(Ogre::OverlaySystem* obj) : _native(obj)
		{

		}

	public:
		OverlaySystem();

		~OverlaySystem();
	protected:
		!OverlaySystem();

	public:
		property bool IsDisposed
		{
			virtual bool get()
			{
				return _native == nullptr;
			}
		}

		property Mogre::SceneManager^ SceneManager
		{
		public:
			Mogre::SceneManager^ get();
		public:
			void set(Mogre::SceneManager^ value);
		}

	internal:
		property Ogre::OverlaySystem* UnmanagedPointer
		{
			Ogre::OverlaySystem* get() { return _native; }
		}
	};

	public ref class OverlayManager : IMogreDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	public protected:
		static OverlayManager^ _singleton;
		Ogre::OverlayManager* _native;
		bool _createdByCLR;

		OverlayManager(IntPtr ptr) : _native((Ogre::OverlayManager*)ptr.ToPointer())
		{

		}

		OverlayManager(Ogre::OverlayManager* obj) : _native(obj)
		{

		}

	public:
		~OverlayManager();
	protected:
		!OverlayManager();

	public:
		OverlayManager();

		static property OverlayManager^ Singleton
		{
			OverlayManager^ get()
			{
				if (_singleton == CLR_NULL)
				{
					Ogre::OverlayManager* ptr = Ogre::OverlayManager::getSingletonPtr();
					if (ptr) _singleton = gcnew OverlayManager(ptr);
				}
				return _singleton;
			}
		}

		property bool IsDisposed
		{
			virtual bool get()
			{
				return _native == nullptr;
			}
		}

		property Ogre::Real LoadingOrder
		{
		public:
			Ogre::Real get();
		}

		property Ogre::Real ViewportAspectRatio
		{
		public:
			Ogre::Real get();
		}

		property int ViewportHeight
		{
		public:
			int get();
		}

		property int ViewportWidth
		{
		public:
			int get();
		}

		Mogre::Const_StringVector^ GetScriptPatterns();

		void ParseScript(Mogre::DataStreamPtr^ stream, String^ groupName);

		Mogre::Overlay^ Create(String^ name);

		Mogre::Overlay^ GetByName(String^ name);

		void Destroy(String^ name);

		void Destroy(Mogre::Overlay^ overlay);

		void DestroyAll();

		//Mogre::OverlayManager::OverlayMapIterator^ GetOverlayIterator();
		//void _queueOverlaysForRendering(Mogre::Camera^ cam, Mogre::RenderQueue^ pQueue, Mogre::Viewport^ vp);

		Mogre::OverlayElement^ CreateOverlayElement(String^ typeName, String^ instanceName, bool isTemplate);
		Mogre::OverlayElement^ CreateOverlayElement(String^ typeName, String^ instanceName);

		Mogre::OverlayElement^ GetOverlayElement(String^ name, bool isTemplate);
		Mogre::OverlayElement^ GetOverlayElement(String^ name);

		void DestroyOverlayElement(String^ instanceName, bool isTemplate);
		void DestroyOverlayElement(String^ instanceName);

		void DestroyOverlayElement(Mogre::OverlayElement^ pInstance, bool isTemplate);
		void DestroyOverlayElement(Mogre::OverlayElement^ pInstance);

		void DestroyAllOverlayElements(bool isTemplate);
		void DestroyAllOverlayElements();

		bool HasOverlayElement(String^ instanceName, bool isTemplate);
		bool HasOverlayElement(String^ instanceName);

		//void AddOverlayElementFactory(Mogre::OverlayElementFactory^ elemFactory);

		Mogre::OverlayElement^ CreateOverlayElementFromTemplate(String^ templateName, String^ typeName, String^ instanceName, bool isTemplate);
		Mogre::OverlayElement^ CreateOverlayElementFromTemplate(String^ templateName, String^ typeName, String^ instanceName);
		Mogre::OverlayElement^ CloneOverlayElementFromTemplate(String^ templateName, String^ instanceName);
		Mogre::OverlayElement^ CreateOverlayElementFromFactory(String^ typeName, String^ instanceName);

		//Mogre::OverlayManager::TemplateIterator^ GetTemplateIterator();

		bool IsTemplate(String^ strName);

	internal:
		property Ogre::OverlayManager* UnmanagedPointer
		{
			Ogre::OverlayManager* get() { return _native; }
		}
	};
}