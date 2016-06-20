#include "stdafx.h"
#include "MogreOverlayManager.h"
#include "MogreSceneManager.h"
#include "MogreCamera.h"
#include "MogreMaterialManager.h"
#include "MogreSceneNode.h"
#include "MogreDataStream.h"

using namespace Mogre;

// ---------------- OverlayElement ---------------------

OverlayElement::~OverlayElement()
{
	this->!OverlayElement();
}

OverlayElement::!OverlayElement()
{
	OnDisposing(this, nullptr);

	if (IsDisposed)
		return;

	if (_createdByCLR && _native)
	{
		OGRE_DELETE _native;
		_native = 0;
	}

	OnDisposed(this, nullptr);
}

String^ OverlayElement::Caption::get()
{
#ifdef OGRE_UNICODE_SUPPORT
	return UTF_TO_CLR_STRING(static_cast<const Ogre::OverlayElement*>(_native)->getCaption());
#else
	return TO_CLR_STRING(static_cast<const Ogre::OverlayElement*>(_native)->getCaption());
#endif
}

void OverlayElement::Caption::set(String^ text)
{
#ifdef OGRE_UNICODE_SUPPORT
	DECLARE_NATIVE_UTFSTRING(o_text, text);
#else
	DECLARE_NATIVE_STRING(o_text, text);
#endif

	static_cast<Ogre::OverlayElement*>(_native)->setCaption(o_text);
}

Mogre::ColourValue OverlayElement::Colour::get()
{
	return ToColor4(static_cast<const Ogre::OverlayElement*>(_native)->getColour());
}

void OverlayElement::Colour::set(Mogre::ColourValue col)
{
	static_cast<Ogre::OverlayElement*>(_native)->setColour(FromColor4(col));
}

Mogre::Real OverlayElement::Height::get()
{
	return static_cast<const Ogre::OverlayElement*>(_native)->getHeight();
}
void OverlayElement::Height::set(Mogre::Real height)
{
	static_cast<Ogre::OverlayElement*>(_native)->setHeight(height);
}

Mogre::GuiHorizontalAlignment OverlayElement::HorizontalAlignment::get()
{
	return (Mogre::GuiHorizontalAlignment)static_cast<const Ogre::OverlayElement*>(_native)->getHorizontalAlignment();
}
void OverlayElement::HorizontalAlignment::set(Mogre::GuiHorizontalAlignment gha)
{
	static_cast<Ogre::OverlayElement*>(_native)->setHorizontalAlignment((Ogre::GuiHorizontalAlignment)gha);
}

bool OverlayElement::IsCloneable::get()
{
	return static_cast<const Ogre::OverlayElement*>(_native)->isCloneable();
}

bool OverlayElement::IsContainer::get()
{
	return static_cast<const Ogre::OverlayElement*>(_native)->isContainer();
}

bool OverlayElement::IsEnabled::get()
{
	return static_cast<const Ogre::OverlayElement*>(_native)->isEnabled();
}

bool OverlayElement::IsKeyEnabled::get()
{
	return static_cast<const Ogre::OverlayElement*>(_native)->isKeyEnabled();
}

bool OverlayElement::IsVisible::get()
{
	return static_cast<const Ogre::OverlayElement*>(_native)->isVisible();
}

Mogre::Real OverlayElement::Left::get()
{
	return static_cast<const Ogre::OverlayElement*>(_native)->getLeft();
}

void OverlayElement::Left::set(Mogre::Real left)
{
	static_cast<Ogre::OverlayElement*>(_native)->setLeft(left);
}

String^ OverlayElement::MaterialName::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::OverlayElement*>(_native)->getMaterialName());
}

void OverlayElement::MaterialName::set(String^ matName)
{
	DECLARE_NATIVE_STRING(o_matName, matName);

	static_cast<Ogre::OverlayElement*>(_native)->setMaterialName(o_matName);
}

Mogre::GuiMetricsMode OverlayElement::MetricsMode::get()
{
	return (Mogre::GuiMetricsMode)static_cast<const Ogre::OverlayElement*>(_native)->getMetricsMode();
}

void OverlayElement::MetricsMode::set(Mogre::GuiMetricsMode gmm)
{
	static_cast<Ogre::OverlayElement*>(_native)->setMetricsMode((Ogre::GuiMetricsMode)gmm);
}

String^ OverlayElement::Name::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::OverlayElement*>(_native)->getName());
}

Mogre::OverlayContainer^ OverlayElement::Parent::get()
{
	return static_cast<Ogre::OverlayElement*>(_native)->getParent();
}

Mogre::OverlayElement^ OverlayElement::SourceTemplate::get()
{
	return static_cast<const Ogre::OverlayElement*>(_native)->getSourceTemplate();
}

Mogre::Real OverlayElement::Top::get()
{
	return static_cast<const Ogre::OverlayElement*>(_native)->getTop();
}
void OverlayElement::Top::set(Mogre::Real Top)
{
	static_cast<Ogre::OverlayElement*>(_native)->setTop(Top);
}

String^ OverlayElement::TypeName::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::OverlayElement*>(_native)->getTypeName());
}

Mogre::GuiVerticalAlignment OverlayElement::VerticalAlignment::get()
{
	return (Mogre::GuiVerticalAlignment)static_cast<const Ogre::OverlayElement*>(_native)->getVerticalAlignment();
}
void OverlayElement::VerticalAlignment::set(Mogre::GuiVerticalAlignment gva)
{
	static_cast<Ogre::OverlayElement*>(_native)->setVerticalAlignment((Ogre::GuiVerticalAlignment)gva);
}

Mogre::Real OverlayElement::Width::get()
{
	return static_cast<const Ogre::OverlayElement*>(_native)->getWidth();
}
void OverlayElement::Width::set(Mogre::Real width)
{
	static_cast<Ogre::OverlayElement*>(_native)->setWidth(width);
}

Mogre::ushort OverlayElement::ZOrder::get()
{
	return static_cast<const Ogre::OverlayElement*>(_native)->getZOrder();
}

void OverlayElement::Initialise()
{
	static_cast<Ogre::OverlayElement*>(_native)->initialise();
}

void OverlayElement::Show()
{
	static_cast<Ogre::OverlayElement*>(_native)->show();
}

void OverlayElement::Hide()
{
	static_cast<Ogre::OverlayElement*>(_native)->hide();
}

void OverlayElement::SetEnabled(bool b)
{
	static_cast<Ogre::OverlayElement*>(_native)->setEnabled(b);
}

void OverlayElement::SetDimensions(Mogre::Real width, Mogre::Real height)
{
	static_cast<Ogre::OverlayElement*>(_native)->setDimensions(width, height);
}

void OverlayElement::SetPosition(Mogre::Real left, Mogre::Real top)
{
	static_cast<Ogre::OverlayElement*>(_native)->setPosition(left, top);
}

Mogre::Real OverlayElement::_getLeft()
{
	return static_cast<const Ogre::OverlayElement*>(_native)->_getLeft();
}

Mogre::Real OverlayElement::_getTop()
{
	return static_cast<const Ogre::OverlayElement*>(_native)->_getTop();
}

Mogre::Real OverlayElement::_getWidth()
{
	return static_cast<const Ogre::OverlayElement*>(_native)->_getWidth();
}

Mogre::Real OverlayElement::_getHeight()
{
	return static_cast<const Ogre::OverlayElement*>(_native)->_getHeight();
}

void OverlayElement::_setLeft(Mogre::Real left)
{
	static_cast<Ogre::OverlayElement*>(_native)->_setLeft(left);
}

void OverlayElement::_setTop(Mogre::Real top)
{
	static_cast<Ogre::OverlayElement*>(_native)->_setTop(top);
}

void OverlayElement::_setWidth(Mogre::Real width)
{
	static_cast<Ogre::OverlayElement*>(_native)->_setWidth(width);
}

void OverlayElement::_setHeight(Mogre::Real height)
{
	static_cast<Ogre::OverlayElement*>(_native)->_setHeight(height);
}

void OverlayElement::_setPosition(Mogre::Real left, Mogre::Real top)
{
	static_cast<Ogre::OverlayElement*>(_native)->_setPosition(left, top);
}

void OverlayElement::_setDimensions(Mogre::Real width, Mogre::Real height)
{
	static_cast<Ogre::OverlayElement*>(_native)->_setDimensions(width, height);
}

Mogre::MaterialPtr^ OverlayElement::GetMaterial()
{
	return static_cast<const Ogre::OverlayElement*>(_native)->getMaterial();
}

void OverlayElement::GetWorldTransforms(Mogre::Matrix4* xform)
{
	Ogre::Matrix4* o_xform = reinterpret_cast<Ogre::Matrix4*>(xform);

	static_cast<const Ogre::OverlayElement*>(_native)->getWorldTransforms(o_xform);
}

void OverlayElement::_positionsOutOfDate()
{
	static_cast<Ogre::OverlayElement*>(_native)->_positionsOutOfDate();
}

void OverlayElement::_update()
{
	static_cast<Ogre::OverlayElement*>(_native)->_update();
}

void OverlayElement::_updateFromParent()
{
	static_cast<Ogre::OverlayElement*>(_native)->_updateFromParent();
}

void OverlayElement::_notifyParent(Mogre::OverlayContainer^ parent, Mogre::Overlay^ overlay)
{
	static_cast<Ogre::OverlayElement*>(_native)->_notifyParent(parent, overlay);
}

Mogre::Real OverlayElement::_getDerivedLeft()
{
	return static_cast<Ogre::OverlayElement*>(_native)->_getDerivedLeft();
}

Mogre::Real OverlayElement::_getDerivedTop()
{
	return static_cast<Ogre::OverlayElement*>(_native)->_getDerivedTop();
}

Mogre::Real OverlayElement::_getRelativeWidth()
{
	return static_cast<Ogre::OverlayElement*>(_native)->_getRelativeWidth();
}

Mogre::Real OverlayElement::_getRelativeHeight()
{
	return static_cast<Ogre::OverlayElement*>(_native)->_getRelativeHeight();
}

//void OverlayElement::_getClippingRegion(Mogre::Rectangle clippingRegion)
//{
//	static_cast<Ogre::OverlayElement*>(_native)->_getClippingRegion(clippingRegion);
//}

void OverlayElement::_notifyZOrder(Mogre::ushort newZOrder)
{
	static_cast<Ogre::OverlayElement*>(_native)->_notifyZOrder(newZOrder);
}

void OverlayElement::_notifyWorldTransforms(Mogre::Matrix4 xform)
{
	pin_ptr<Ogre::Matrix4> p_xform = interior_ptr<Ogre::Matrix4>(&xform.m00);

	static_cast<Ogre::OverlayElement*>(_native)->_notifyWorldTransforms(*p_xform);
}

void OverlayElement::_notifyViewport()
{
	static_cast<Ogre::OverlayElement*>(_native)->_notifyViewport();
}

//void OverlayElement::_updateRenderQueue(Mogre::RenderQueue^ queue)
//{
//	static_cast<Ogre::OverlayElement*>(_native)->_updateRenderQueue(queue);
//}

bool OverlayElement::Contains(Mogre::Real x, Mogre::Real y)
{
	return static_cast<const Ogre::OverlayElement*>(_native)->contains(x, y);
}

Mogre::OverlayElement^ OverlayElement::FindElementAt(Mogre::Real x, Mogre::Real y)
{
	return _native->findElementAt(x, y);
}

void OverlayElement::SetCloneable(bool c)
{
	_native->setCloneable(c);
}

void OverlayElement::_setParent(Mogre::OverlayContainer^ parent)
{
	_native->_setParent(parent);
}

Mogre::Real OverlayElement::GetSquaredViewDepth(Mogre::Camera^ cam)
{
	return _native->getSquaredViewDepth(cam);
}

//Mogre::Const_LightList^ OverlayElement::GetLights()
//{
//	return static_cast<const Ogre::OverlayElement*>(_native)->getLights();
//}

void OverlayElement::CopyFromTemplate(Mogre::OverlayElement^ templateOverlay)
{
	static_cast<Ogre::OverlayElement*>(_native)->copyFromTemplate(templateOverlay);
}

Mogre::OverlayElement^ OverlayElement::Clone(String^ instanceName)
{
	DECLARE_NATIVE_STRING(o_instanceName, instanceName);

	return gcnew Mogre::OverlayElement(_native->clone(o_instanceName));
}

//------------------------------------------------------------
// Implementation for IStringInterface
//------------------------------------------------------------

//Mogre::ParamDictionary_NativePtr OverlayElement::ParamDictionary::get()
//{
//	return static_cast<Ogre::OverlayElement*>(_native)->getParamDictionary();
//}
//
//Mogre::Const_ParameterList^ OverlayElement::GetParameters()
//{
//	return static_cast<const Ogre::OverlayElement*>(_native)->getParameters();
//}

bool OverlayElement::SetParameter(String^ name, String^ value)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_value, value);

	return _native->setParameter(o_name, o_value);
}

void OverlayElement::SetParameterList(Mogre::Const_NameValuePairList^ paramList)
{
	_native->setParameterList(paramList);
}

String^ OverlayElement::GetParameter(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return TO_CLR_STRING(_native->getParameter(o_name));
}

//void OverlayElement::CopyParametersTo(Mogre::IStringInterface^ dest)
//{
//	static_cast<const Ogre::OverlayElement*>(_native)->copyParametersTo(dest);
//}

//------------------------------------------------------------
// Implementation for IRenderable
//------------------------------------------------------------

bool OverlayElement::CastsShadows::get()
{
	return static_cast<const Ogre::OverlayElement*>(_native)->getCastsShadows();
}

unsigned short OverlayElement::NumWorldTransforms::get()
{
	return static_cast<const Ogre::OverlayElement*>(_native)->getNumWorldTransforms();
}

bool OverlayElement::PolygonModeOverrideable::get()
{
	return static_cast<const Ogre::OverlayElement*>(_native)->getPolygonModeOverrideable();
}
void OverlayElement::PolygonModeOverrideable::set(bool override)
{
	static_cast<Ogre::OverlayElement*>(_native)->setPolygonModeOverrideable(override);
}

Mogre::Technique^ OverlayElement::Technique::get()
{
	return static_cast<const Ogre::OverlayElement*>(_native)->getTechnique();
}

bool OverlayElement::UseIdentityProjection::get()
{
	return static_cast<const Ogre::OverlayElement*>(_native)->getUseIdentityProjection();
}
void OverlayElement::UseIdentityProjection::set(bool useIdentityProjection)
{
	static_cast<Ogre::OverlayElement*>(_native)->setUseIdentityProjection(useIdentityProjection);
}

bool OverlayElement::UseIdentityView::get()
{
	return static_cast<const Ogre::OverlayElement*>(_native)->getUseIdentityView();
}
void OverlayElement::UseIdentityView::set(bool useIdentityView)
{
	static_cast<Ogre::OverlayElement*>(_native)->setUseIdentityView(useIdentityView);
}

//void OverlayElement::GetRenderOperation(Mogre::RenderOperation^ op)
//{
//	static_cast<Ogre::OverlayElement*>(_native)->getRenderOperation(op);
//}
//
//Mogre::Const_PlaneList^ OverlayElement::GetClipPlanes()
//{
//	return static_cast<const Ogre::OverlayElement*>(_native)->getClipPlanes();
//}

void OverlayElement::SetCustomParameter(size_t index, Mogre::Vector4 value)
{
	static_cast<Ogre::OverlayElement*>(_native)->setCustomParameter(index, FromVector4(value));
}

Mogre::Vector4 OverlayElement::GetCustomParameter(size_t index)
{
	return ToVector4(static_cast<const Ogre::OverlayElement*>(_native)->getCustomParameter(index));
}

// ---------------- Overlay ---------------------
CPP_DECLARE_STLLIST(Overlay::, OverlayContainerList, Mogre::OverlayContainer^, Ogre::OverlayContainer*);
CPP_DECLARE_ITERATOR(Overlay::, Overlay2DElementsIterator, Ogre::Overlay::Overlay2DElementsIterator, Mogre::Overlay::OverlayContainerList, Mogre::OverlayContainer^, Ogre::OverlayContainer*, );

//Private Declarations

//Internal Declarations

//Public Declarations
Overlay::Overlay(String^ name)
{
	_createdByCLR = true;
	DECLARE_NATIVE_STRING(o_name, name);
	_native = new Ogre::Overlay(o_name);
}

Overlay::~Overlay()
{
	this->!Overlay();
}

Overlay::!Overlay()
{
	OnDisposing(this, nullptr);

	if (IsDisposed)
		return;

	if (_createdByCLR && _native)
	{
		OGRE_DELETE _native;
		_native = 0;
	}

	OnDisposed(this, nullptr);
}

bool Overlay::IsInitialised::get()
{
	return static_cast<const Ogre::Overlay*>(_native)->isInitialised();
}

bool Overlay::IsVisible::get()
{
	return static_cast<const Ogre::Overlay*>(_native)->isVisible();
}

String^ Overlay::Name::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::Overlay*>(_native)->getName());
}

String^ Overlay::Origin::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::Overlay*>(_native)->getOrigin());
}

Mogre::Real Overlay::ScaleX::get()
{
	return static_cast<const Ogre::Overlay*>(_native)->getScaleX();
}

Mogre::Real Overlay::ScaleY::get()
{
	return static_cast<const Ogre::Overlay*>(_native)->getScaleY();
}

Mogre::Real Overlay::ScrollX::get()
{
	return static_cast<const Ogre::Overlay*>(_native)->getScrollX();
}

Mogre::Real Overlay::ScrollY::get()
{
	return static_cast<const Ogre::Overlay*>(_native)->getScrollY();
}

Mogre::ushort Overlay::ZOrder::get()
{
	return static_cast<const Ogre::Overlay*>(_native)->getZOrder();
}
void Overlay::ZOrder::set(Mogre::ushort zorder)
{
	static_cast<Ogre::Overlay*>(_native)->setZOrder(zorder);
}

Mogre::OverlayContainer^ Overlay::GetChild(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return static_cast<Ogre::Overlay*>(_native)->getChild(o_name);
}

void Overlay::Show()
{
	_native->show();
}

void Overlay::Hide()
{
	_native->hide();
}

void Overlay::Add2D(Mogre::OverlayContainer^ cont)
{
	_native->add2D(cont);
}

void Overlay::Remove2D(Mogre::OverlayContainer^ cont)
{
	_native->remove2D(cont);
}

void Overlay::Add3D(Mogre::SceneNode^ node)
{
	_native->add3D(node);
}

void Overlay::Remove3D(Mogre::SceneNode^ node)
{
	_native->remove3D(node);
}

void Overlay::Clear()
{
	_native->clear();
}

void Overlay::SetScroll(Mogre::Real x, Mogre::Real y)
{
	_native->setScroll(x, y);
}

void Overlay::Scroll(Mogre::Real xoff, Mogre::Real yoff)
{
	_native->scroll(xoff, yoff);
}

void Overlay::SetRotate(Mogre::Radian angle)
{
	_native->setRotate(Ogre::Radian(angle.ValueRadians));
}

Mogre::Radian Overlay::GetRotate()
{
	return Mogre::Radian(_native->getRotate().valueRadians());
}

void Overlay::Rotate(Mogre::Radian angle)
{
	_native->rotate(Ogre::Radian(angle.ValueRadians));
}

void Overlay::SetScale(Mogre::Real x, Mogre::Real y)
{
	_native->setScale(x, y);
}

//void Overlay::_findVisibleObjects(Mogre::Camera^ cam, Mogre::RenderQueue^ queue)
//{
//	static_cast<Ogre::Overlay*>(_native)->_findVisibleObjects(cam, queue);
//}

Mogre::OverlayElement^ Overlay::FindElementAt(Mogre::Real x, Mogre::Real y)
{
	return _native->findElementAt(x, y);
}

Mogre::Overlay::Overlay2DElementsIterator^ Overlay::Get2DElementsIterator()
{
	return _native->get2DElementsIterator();
}

void Overlay::_notifyOrigin(String^ origin)
{
	DECLARE_NATIVE_STRING(o_origin, origin);

	_native->_notifyOrigin(o_origin);
}

// ---------------- OverlaySystem ---------------------

OverlaySystem::OverlaySystem()
	: _sceneManager(nullptr)
{
	_createdByCLR = true;
	_native = OGRE_NEW Ogre::OverlaySystem();
}

OverlaySystem::~OverlaySystem()
{
	this->!OverlaySystem();
}

OverlaySystem::!OverlaySystem()
{
	OnDisposing(this, nullptr);

	if (IsDisposed)
		return;

	if (_native)
	{
		GetPointerOrNull(_sceneManager)->removeRenderQueueListener(_native);
	}

	if (_createdByCLR && _native)
	{
		OGRE_DELETE _native;
		_native = 0;
	}

	OnDisposed(this, nullptr);
}

Mogre::SceneManager^ OverlaySystem::SceneManager::get()
{
	return _sceneManager;
}

void OverlaySystem::SceneManager::set(Mogre::SceneManager^ value)
{
	if (value != nullptr)
	{
		GetPointerOrNull(value)->addRenderQueueListener(_native);
		_sceneManager = value;
	}
	else
	{
		if (_sceneManager)
		{
			GetPointerOrNull(_sceneManager)->removeRenderQueueListener(_native);
		}

		_sceneManager = nullptr;
	}
}

// ---------------- OverlayManager ---------------------

OverlayManager::OverlayManager()
{
	_createdByCLR = true;
	_native = new Ogre::OverlayManager();
}

OverlayManager::~OverlayManager()
{
	this->!OverlayManager();
}

OverlayManager::!OverlayManager()
{
	OnDisposing(this, nullptr);

	if (IsDisposed)
		return;

	if (_createdByCLR && _native)
	{
		delete _native;
		_native = 0;
	}

	OnDisposed(this, nullptr);
}

Ogre::Real OverlayManager::LoadingOrder::get()
{
	return _native->getLoadingOrder();
}

Ogre::Real OverlayManager::ViewportAspectRatio::get()
{
	return _native->getViewportAspectRatio();
}

int OverlayManager::ViewportHeight::get()
{
	return _native->getViewportHeight();
}

int OverlayManager::ViewportWidth::get()
{
	return _native->getViewportWidth();
}

Mogre::Const_StringVector^ OverlayManager::GetScriptPatterns()
{
	return static_cast<const Ogre::OverlayManager*>(_native)->getScriptPatterns();
}

void OverlayManager::ParseScript(Mogre::DataStreamPtr^ stream, String^ groupName)
{
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	_native->parseScript((Ogre::DataStreamPtr&)stream, o_groupName);
}

Mogre::Overlay^ OverlayManager::Create(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return gcnew Mogre::Overlay( _native->create(o_name) );
}

Mogre::Overlay^ OverlayManager::GetByName(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return _native->getByName(o_name);
}

void OverlayManager::Destroy(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	_native->destroy(o_name);
}

void OverlayManager::Destroy(Mogre::Overlay^ overlay)
{
	_native->destroy(overlay);
}

void OverlayManager::DestroyAll()
{
	static_cast<Ogre::OverlayManager*>(_native)->destroyAll();
}

//Mogre::OverlayManager::OverlayMapIterator^ OverlayManager::GetOverlayIterator()
//{
//	return static_cast<Ogre::OverlayManager*>(_native)->getOverlayIterator();
//}
//
//void OverlayManager::_queueOverlaysForRendering(Mogre::Camera^ cam, Mogre::RenderQueue^ pQueue, Mogre::Viewport^ vp)
//{
//	static_cast<Ogre::OverlayManager*>(_native)->_queueOverlaysForRendering(cam, pQueue, vp);
//}

Mogre::OverlayElement^ ResolveFromNativeInstance(Ogre::OverlayElement* native)
{
	Ogre::BorderPanelOverlayElement* borderPanel = dynamic_cast<Ogre::BorderPanelOverlayElement*>(native);
	if (borderPanel)
		return gcnew Mogre::BorderPanelOverlayElement(borderPanel);

	Ogre::PanelOverlayElement* panel = dynamic_cast<Ogre::PanelOverlayElement*>(native);
	if (panel)
		return gcnew Mogre::PanelOverlayElement(panel);

	Ogre::TextAreaOverlayElement* textAreaOverlayElement = dynamic_cast<Ogre::TextAreaOverlayElement*>(native);
	if (textAreaOverlayElement)
		return gcnew Mogre::TextAreaOverlayElement(textAreaOverlayElement);

	Ogre::OverlayContainer* overlayContainer = dynamic_cast<Ogre::OverlayContainer*>(native);
	if (overlayContainer)
		return gcnew Mogre::OverlayContainer(overlayContainer);

	return gcnew Mogre::OverlayElement(native);
}

Mogre::OverlayElement^ OverlayManager::CreateOverlayElement(String^ typeName, String^ instanceName, bool isTemplate)
{
	DECLARE_NATIVE_STRING(o_typeName, typeName);
	DECLARE_NATIVE_STRING(o_instanceName, instanceName);

	auto instance = ResolveFromNativeInstance(_native->createOverlayElement(o_typeName, o_instanceName, isTemplate));
	if(isTemplate)
		_templates->Add(instanceName, instance);
	else
		_instances->Add(instanceName, instance);
	return instance;
}

Mogre::OverlayElement^ OverlayManager::CreateOverlayElement(String^ typeName, String^ instanceName)
{
	DECLARE_NATIVE_STRING(o_typeName, typeName);
	DECLARE_NATIVE_STRING(o_instanceName, instanceName);

	auto instance = ResolveFromNativeInstance(_native->createOverlayElement(o_typeName, o_instanceName));
	_instances->Add(instanceName, instance);
	return instance;
}

Mogre::OverlayElement^ OverlayManager::GetOverlayElement(String^ name, bool isTemplate)
{
	DECLARE_NATIVE_STRING(o_name, name);
	auto collection = isTemplate ? _templates : _instances;
	Mogre::OverlayElement^ element;
	if (collection->TryGetValue(name, element))
		return element;

	auto instance = ResolveFromNativeInstance(_native->getOverlayElement(o_name, isTemplate));
	collection->Add(name, instance);
	return instance;
}

Mogre::OverlayElement^ OverlayManager::GetOverlayElement(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);
	Mogre::OverlayElement^ element;
	if (_instances->TryGetValue(name, element))
		return element;

	auto instance = ResolveFromNativeInstance(_native->getOverlayElement(o_name));
	_instances->Add(name, instance);
	return instance;
}

void OverlayManager::DestroyOverlayElement(String^ instanceName, bool isTemplate)
{
	DECLARE_NATIVE_STRING(o_instanceName, instanceName);
	_native->destroyOverlayElement(o_instanceName, isTemplate);
	auto collection = isTemplate ? _templates : _instances;
	collection->Remove(instanceName);
}

void OverlayManager::DestroyOverlayElement(String^ instanceName)
{
	DECLARE_NATIVE_STRING(o_instanceName, instanceName);

	_native->destroyOverlayElement(o_instanceName);
	_instances->Remove(instanceName);
}

void OverlayManager::DestroyOverlayElement(Mogre::OverlayElement^ pInstance, bool isTemplate)
{
	auto collection = isTemplate ? _templates : _instances;
	if (collection->ContainsKey(pInstance->Name))
		collection->Remove(pInstance->Name);

	_native->destroyOverlayElement(pInstance, isTemplate);
}

void OverlayManager::DestroyOverlayElement(Mogre::OverlayElement^ pInstance)
{
	if (_instances->ContainsKey(pInstance->Name))
		_instances->Remove(pInstance->Name);

	_native->destroyOverlayElement(pInstance);
}

void OverlayManager::DestroyAllOverlayElements(bool isTemplate)
{
	_native->destroyAllOverlayElements(isTemplate);
	if (isTemplate)
		_templates->Clear();
	else
		_instances->Clear();
}

void OverlayManager::DestroyAllOverlayElements()
{
	_native->destroyAllOverlayElements();
	_instances->Clear();
}

bool OverlayManager::HasOverlayElement(String^ instanceName, bool isTemplate)
{
	DECLARE_NATIVE_STRING(o_instanceName, instanceName);

	return _native->hasOverlayElement(o_instanceName, isTemplate);
}

bool OverlayManager::HasOverlayElement(String^ instanceName)
{
	DECLARE_NATIVE_STRING(o_instanceName, instanceName);

	return _native->hasOverlayElement(o_instanceName);
}

//void OverlayManager::AddOverlayElementFactory(Mogre::OverlayElementFactory^ elemFactory)
//{
//	static_cast<Ogre::OverlayManager*>(_native)->addOverlayElementFactory(elemFactory);
//}

Mogre::OverlayElement^ OverlayManager::CreateOverlayElementFromTemplate(String^ templateName, String^ typeName, String^ instanceName, bool isTemplate)
{
	DECLARE_NATIVE_STRING(o_templateName, templateName);
	DECLARE_NATIVE_STRING(o_typeName, typeName);
	DECLARE_NATIVE_STRING(o_instanceName, instanceName);

	return static_cast<Ogre::OverlayManager*>(_native)->createOverlayElementFromTemplate(o_templateName, o_typeName, o_instanceName, isTemplate);
}
Mogre::OverlayElement^ OverlayManager::CreateOverlayElementFromTemplate(String^ templateName, String^ typeName, String^ instanceName)
{
	DECLARE_NATIVE_STRING(o_templateName, templateName);
	DECLARE_NATIVE_STRING(o_typeName, typeName);
	DECLARE_NATIVE_STRING(o_instanceName, instanceName);

	return static_cast<Ogre::OverlayManager*>(_native)->createOverlayElementFromTemplate(o_templateName, o_typeName, o_instanceName);
}

Mogre::OverlayElement^ OverlayManager::CloneOverlayElementFromTemplate(String^ templateName, String^ instanceName)
{
	DECLARE_NATIVE_STRING(o_templateName, templateName);
	DECLARE_NATIVE_STRING(o_instanceName, instanceName);

	return static_cast<Ogre::OverlayManager*>(_native)->cloneOverlayElementFromTemplate(o_templateName, o_instanceName);
}

Mogre::OverlayElement^ OverlayManager::CreateOverlayElementFromFactory(String^ typeName, String^ instanceName)
{
	DECLARE_NATIVE_STRING(o_typeName, typeName);
	DECLARE_NATIVE_STRING(o_instanceName, instanceName);

	return static_cast<Ogre::OverlayManager*>(_native)->createOverlayElementFromFactory(o_typeName, o_instanceName);
}

//Mogre::OverlayManager::TemplateIterator^ OverlayManager::GetTemplateIterator()
//{
//	return static_cast<Ogre::OverlayManager*>(_native)->getTemplateIterator();
//}

bool OverlayManager::IsTemplate(String^ strName)
{
	DECLARE_NATIVE_STRING(o_strName, strName);

	return static_cast<const Ogre::OverlayManager*>(_native)->isTemplate(o_strName);
}
