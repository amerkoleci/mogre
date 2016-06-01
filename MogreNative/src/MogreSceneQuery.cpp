#include "stdafx.h"
#include "MogreSceneQuery.h"
#include "MogreMovableObject.h"
using namespace Mogre;

//CPP_DECLARE_STLLIST(SceneQuery::WorldFragment::, STLList_Plane, Mogre::Plane, Ogre::Plane);

SceneQuery::WorldFragment::WorldFragment()
{
	_createdByCLR = true;
	_native = new Ogre::SceneQuery::WorldFragment();
}

Mogre::SceneQuery::WorldFragmentType SceneQuery::WorldFragment::fragmentType::get()
{
	return (Mogre::SceneQuery::WorldFragmentType)static_cast<Ogre::SceneQuery::WorldFragment*>(_native)->fragmentType;
}
void SceneQuery::WorldFragment::fragmentType::set(Mogre::SceneQuery::WorldFragmentType value)
{
	static_cast<Ogre::SceneQuery::WorldFragment*>(_native)->fragmentType = (Ogre::SceneQuery::WorldFragmentType)value;
}

Mogre::Vector3 SceneQuery::WorldFragment::singleIntersection::get()
{
	return ToVector3(static_cast<Ogre::SceneQuery::WorldFragment*>(_native)->singleIntersection);
}

void SceneQuery::WorldFragment::singleIntersection::set(Mogre::Vector3 value)
{
	static_cast<Ogre::SceneQuery::WorldFragment*>(_native)->singleIntersection = FromVector3(value);
}

/*SceneQuery::WorldFragment::STLList_Plane^ SceneQuery::WorldFragment::planes::get()
{
	return (CLR_NULL == _planes) ? (_planes = static_cast<Ogre::SceneQuery::WorldFragment*>(_native)->planes) : _planes;
}*/

void* SceneQuery::WorldFragment::geometry::get()
{
	return static_cast<Ogre::SceneQuery::WorldFragment*>(_native)->geometry;
}
void SceneQuery::WorldFragment::geometry::set(void* value)
{
	static_cast<Ogre::SceneQuery::WorldFragment*>(_native)->geometry = value;
}

Mogre::RenderOperation^ SceneQuery::WorldFragment::renderOp::get()
{
	return static_cast<Ogre::SceneQuery::WorldFragment*>(_native)->renderOp;
}

void SceneQuery::WorldFragment::renderOp::set(Mogre::RenderOperation^ value)
{
	static_cast<Ogre::SceneQuery::WorldFragment*>(_native)->renderOp = value;
}

//SceneQuery::SceneQuery(Mogre::SceneManager^ mgr) : Wrapper((CLRObject*)0)
//{
//	_createdByCLR = true;
//	_native = new Ogre::SceneQuery(mgr);
//ObjectTable:Add((intptr_t)_native, this, nullptr);
//}

SceneQuery::~SceneQuery()
{
	this->!SceneQuery();
}

SceneQuery::!SceneQuery()
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

Ogre::uint32 SceneQuery::QueryMask::get()
{
	return static_cast<const Ogre::SceneQuery*>(_native)->getQueryMask();
}

void SceneQuery::QueryMask::set(Ogre::uint32 mask)
{
	static_cast<Ogre::SceneQuery*>(_native)->setQueryMask(mask);
}

void SceneQuery::SetWorldFragmentType(Mogre::SceneQuery::WorldFragmentType wft)
{
	static_cast<Ogre::SceneQuery*>(_native)->setWorldFragmentType((Ogre::SceneQuery::WorldFragmentType)wft);
}

Mogre::SceneQuery::WorldFragmentType SceneQuery::GetWorldFragmentType()
{
	return (Mogre::SceneQuery::WorldFragmentType)static_cast<const Ogre::SceneQuery*>(_native)->getWorldFragmentType();
}

Ogre::SceneQueryListener* SceneQueryListener::_ISceneQueryListener_GetNativePtr()
{
	return static_cast<Ogre::SceneQueryListener*>(static_cast<SceneQueryListener_Proxy*>(_native));
}


//################################################################
//ISceneQueryListener
//################################################################

SceneQueryListener::SceneQueryListener() 
{
	_createdByCLR = true;
	Type^ thisType = this->GetType();
	SceneQueryListener_Proxy* proxy = new SceneQueryListener_Proxy(this);
	//proxy->_overriden = Implementation::SubclassingManager::Instance->GetOverridenMethodsArrayPointer(thisType, SceneQueryListener::typeid, 0);
	_native = proxy;
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

SceneQueryListener::~SceneQueryListener()
{
	this->!SceneQueryListener();
}

SceneQueryListener::!SceneQueryListener()
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

//SceneQueryResult
SceneQueryResult::SceneQueryResult()
{
	_createdByCLR = true;
	_native = new Ogre::SceneQueryResult();
}

Mogre::SceneQueryResultMovableList^ SceneQueryResult::movables::get()
{
	return (CLR_NULL == _movables) ? (_movables = static_cast<Ogre::SceneQueryResult*>(_native)->movables) : _movables;
}

Mogre::SceneQueryResultWorldFragmentList^ SceneQueryResult::worldFragments::get()
{
	return (CLR_NULL == _worldFragments) ? (_worldFragments = static_cast<Ogre::SceneQueryResult*>(_native)->worldFragments) : _worldFragments;
}

//################################################################
//RegionSceneQuery
//################################################################

Ogre::SceneQueryListener* RegionSceneQuery::_ISceneQueryListener_GetNativePtr()
{
	return static_cast<Ogre::SceneQueryListener*>(static_cast<Ogre::RegionSceneQuery*>(_native));
}


//Public Declarations
Mogre::SceneQueryResult^ RegionSceneQuery::LastResults::get()
{
	return static_cast<const Ogre::RegionSceneQuery*>(_native)->getLastResults();
}

Mogre::SceneQueryResult^ RegionSceneQuery::Execute()
{
	return static_cast<Ogre::RegionSceneQuery*>(_native)->execute();
}

void RegionSceneQuery::Execute(Mogre::ISceneQueryListener^ listener)
{
	static_cast<Ogre::RegionSceneQuery*>(_native)->execute(listener->_GetNativePtr());
}

void RegionSceneQuery::ClearResults()
{
	static_cast<Ogre::RegionSceneQuery*>(_native)->clearResults();
}

bool RegionSceneQuery::QueryResult(Mogre::MovableObject^ first)
{
	return static_cast<Ogre::RegionSceneQuery*>(_native)->queryResult(first);
}

bool RegionSceneQuery::QueryResult(Mogre::SceneQuery::WorldFragment^ fragment)
{
	return static_cast<Ogre::RegionSceneQuery*>(_native)->queryResult(fragment);
}

//################################################################
//AxisAlignedBoxSceneQuery
//################################################################

Mogre::AxisAlignedBox^ AxisAlignedBoxSceneQuery::Box::get()
{
	return ToAxisAlignedBounds(static_cast<const Ogre::AxisAlignedBoxSceneQuery*>(_native)->getBox());
}
void AxisAlignedBoxSceneQuery::Box::set(Mogre::AxisAlignedBox^ box)
{
	static_cast<Ogre::AxisAlignedBoxSceneQuery*>(_native)->setBox(FromAxisAlignedBounds(box));
}


//################################################################
//SphereSceneQuery
//################################################################

Mogre::Sphere SphereSceneQuery::Sphere::get()
{
	return ToSphere( static_cast<const Ogre::SphereSceneQuery*>(_native)->getSphere() );
}

void SphereSceneQuery::Sphere::set(Mogre::Sphere sphere)
{
	static_cast<Ogre::SphereSceneQuery*>(_native)->setSphere(FromSphere(sphere));
}

//################################################################
//PlaneBoundedVolumeListSceneQuery
//################################################################
//void PlaneBoundedVolumeListSceneQuery::SetVolumes(Mogre::Const_PlaneBoundedVolumeList^ volumes)
//{
//	static_cast<Ogre::PlaneBoundedVolumeListSceneQuery*>(_native)->setVolumes(volumes);
//}
//
//Mogre::Const_PlaneBoundedVolumeList^ PlaneBoundedVolumeListSceneQuery::GetVolumes()
//{
//	return static_cast<const Ogre::PlaneBoundedVolumeListSceneQuery*>(_native)->getVolumes();
//}

//################################################################
//IRaySceneQueryListener
//################################################################

Ogre::RaySceneQueryListener* RaySceneQueryListener::_IRaySceneQueryListener_GetNativePtr()
{
	return static_cast<Ogre::RaySceneQueryListener*>(static_cast<RaySceneQueryListener_Proxy*>(_native));
}

RaySceneQueryListener::RaySceneQueryListener() 
{
	_createdByCLR = true;
	Type^ thisType = this->GetType();
	RaySceneQueryListener_Proxy* proxy = new RaySceneQueryListener_Proxy(this);
	//proxy->_overriden = Implementation::SubclassingManager::Instance->GetOverridenMethodsArrayPointer(thisType, RaySceneQueryListener::typeid, 0);
	_native = proxy;
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

RaySceneQueryListener::~RaySceneQueryListener()
{
	this->!RaySceneQueryListener();
}

RaySceneQueryListener::!RaySceneQueryListener()
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

//################################################################
//RaySceneQueryResultEntry
//################################################################

RaySceneQueryResultEntry::RaySceneQueryResultEntry()
{
	_createdByCLR = true;
	_native = new Ogre::RaySceneQueryResultEntry();
}

Mogre::Real RaySceneQueryResultEntry::distance::get()
{
	return static_cast<Ogre::RaySceneQueryResultEntry*>(_native)->distance;
}
void RaySceneQueryResultEntry::distance::set(Mogre::Real value)
{
	static_cast<Ogre::RaySceneQueryResultEntry*>(_native)->distance = value;
}

Mogre::MovableObject^ RaySceneQueryResultEntry::movable::get()
{
	return static_cast<Ogre::RaySceneQueryResultEntry*>(_native)->movable;
}

void RaySceneQueryResultEntry::movable::set(Mogre::MovableObject^ value)
{
	static_cast<Ogre::RaySceneQueryResultEntry*>(_native)->movable = value;
}

Mogre::SceneQuery::WorldFragment^ RaySceneQueryResultEntry::worldFragment::get()
{
	return static_cast<Ogre::RaySceneQueryResultEntry*>(_native)->worldFragment;
}
void RaySceneQueryResultEntry::worldFragment::set(Mogre::SceneQuery::WorldFragment^ value)
{
	static_cast<Ogre::RaySceneQueryResultEntry*>(_native)->worldFragment = value;
}


//################################################################
//RaySceneQuery
//################################################################

Ogre::RaySceneQueryListener* RaySceneQuery::_IRaySceneQueryListener_GetNativePtr()
{
	return static_cast<Ogre::RaySceneQueryListener*>(static_cast<Ogre::RaySceneQuery*>(_native));
}
Mogre::ushort RaySceneQuery::MaxResults::get()
{
	return static_cast<const Ogre::RaySceneQuery*>(_native)->getMaxResults();
}

Mogre::Ray RaySceneQuery::Ray::get()
{
	return ToRay(static_cast<const Ogre::RaySceneQuery*>(_native)->getRay());
}

void RaySceneQuery::Ray::set(Mogre::Ray ray)
{
	static_cast<Ogre::RaySceneQuery*>(_native)->setRay(FromRay(ray));
}

bool RaySceneQuery::SortByDistance::get()
{
	return static_cast<const Ogre::RaySceneQuery*>(_native)->getSortByDistance();
}

void RaySceneQuery::SetSortByDistance(bool sort, Mogre::ushort maxresults)
{
	static_cast<Ogre::RaySceneQuery*>(_native)->setSortByDistance(sort, maxresults);
}
void RaySceneQuery::SetSortByDistance(bool sort)
{
	static_cast<Ogre::RaySceneQuery*>(_native)->setSortByDistance(sort);
}

Mogre::RaySceneQueryResult^ RaySceneQuery::Execute()
{
	return static_cast<Ogre::RaySceneQuery*>(_native)->execute();
}

void RaySceneQuery::Execute(Mogre::IRaySceneQueryListener^ listener)
{
	static_cast<Ogre::RaySceneQuery*>(_native)->execute(listener->_GetNativePtr());
}

Mogre::RaySceneQueryResult^ RaySceneQuery::GetLastResults()
{
	return static_cast<Ogre::RaySceneQuery*>(_native)->getLastResults();
}

void RaySceneQuery::ClearResults()
{
	static_cast<Ogre::RaySceneQuery*>(_native)->clearResults();
}

bool RaySceneQuery::QueryResult(Mogre::MovableObject^ obj, Mogre::Real distance)
{
	return static_cast<Ogre::RaySceneQuery*>(_native)->queryResult(obj, distance);
}

bool RaySceneQuery::QueryResult(Mogre::SceneQuery::WorldFragment^ fragment, Mogre::Real distance)
{
	return static_cast<Ogre::RaySceneQuery*>(_native)->queryResult(fragment, distance);
}

//################################################################
//IIntersectionSceneQueryListener
//################################################################

Ogre::IntersectionSceneQueryListener* IntersectionSceneQueryListener::_IIntersectionSceneQueryListener_GetNativePtr()
{
	return static_cast<Ogre::IntersectionSceneQueryListener*>(static_cast<IntersectionSceneQueryListener_Proxy*>(_native));
}

IntersectionSceneQueryListener::IntersectionSceneQueryListener() 
{
	_createdByCLR = true;
	Type^ thisType = this->GetType();
	IntersectionSceneQueryListener_Proxy* proxy = new IntersectionSceneQueryListener_Proxy(this);
	//proxy->_overriden = Implementation::SubclassingManager::Instance->GetOverridenMethodsArrayPointer(thisType, IntersectionSceneQueryListener::typeid, 0);
	_native = proxy;
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

IntersectionSceneQueryListener::~IntersectionSceneQueryListener()
{
	this->!IntersectionSceneQueryListener();
}

IntersectionSceneQueryListener::!IntersectionSceneQueryListener()
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


//################################################################
//IntersectionSceneQueryResult
//################################################################

IntersectionSceneQueryResult::IntersectionSceneQueryResult()
{
	_createdByCLR = true;
	_native = new Ogre::IntersectionSceneQueryResult();
}

Mogre::SceneQueryMovableIntersectionList^ IntersectionSceneQueryResult::movables2movables::get()
{
	return (CLR_NULL == _movables2movables) ? (_movables2movables = static_cast<Ogre::IntersectionSceneQueryResult*>(_native)->movables2movables) : _movables2movables;
}

Mogre::SceneQueryMovableWorldFragmentIntersectionList^ IntersectionSceneQueryResult::movables2world::get()
{
	return (CLR_NULL == _movables2world) ? (_movables2world = static_cast<Ogre::IntersectionSceneQueryResult*>(_native)->movables2world) : _movables2world;
}

//################################################################
//IntersectionSceneQuery
//################################################################

Ogre::IntersectionSceneQueryListener* IntersectionSceneQuery::_IIntersectionSceneQueryListener_GetNativePtr()
{
	return static_cast<Ogre::IntersectionSceneQueryListener*>(static_cast<Ogre::IntersectionSceneQuery*>(_native));
}

Mogre::IntersectionSceneQueryResult^ IntersectionSceneQuery::LastResults::get()
{
	return static_cast<const Ogre::IntersectionSceneQuery*>(_native)->getLastResults();
}

Mogre::IntersectionSceneQueryResult^ IntersectionSceneQuery::Execute()
{
	return static_cast<Ogre::IntersectionSceneQuery*>(_native)->execute();
}

void IntersectionSceneQuery::Execute(Mogre::IIntersectionSceneQueryListener^ listener)
{
	static_cast<Ogre::IntersectionSceneQuery*>(_native)->execute(listener->_GetNativePtr());
}

void IntersectionSceneQuery::ClearResults()
{
	static_cast<Ogre::IntersectionSceneQuery*>(_native)->clearResults();
}

bool IntersectionSceneQuery::QueryResult(Mogre::MovableObject^ first, Mogre::MovableObject^ second)
{
	return static_cast<Ogre::IntersectionSceneQuery*>(_native)->queryResult(first, second);
}

bool IntersectionSceneQuery::QueryResult(Mogre::MovableObject^ movable, Mogre::SceneQuery::WorldFragment^ fragment)
{
	return static_cast<Ogre::IntersectionSceneQuery*>(_native)->queryResult(movable, fragment);
}

CPP_DECLARE_STLLIST(, SceneQueryResultMovableList, Mogre::MovableObject^, Ogre::MovableObject*);
CPP_DECLARE_STLLIST(, SceneQueryResultWorldFragmentList, Mogre::SceneQuery::WorldFragment^, Ogre::SceneQuery::WorldFragment*);
CPP_DECLARE_STLVECTOR(, RaySceneQueryResult, Mogre::RaySceneQueryResultEntry^, Ogre::RaySceneQueryResultEntry);

#define STLDECL_MANAGEDTYPE Pair<Mogre::MovableObject^, Mogre::MovableObject^>
CPP_DECLARE_STLLIST(, SceneQueryMovableIntersectionList, STLDECL_MANAGEDTYPE, Ogre::SceneQueryMovableObjectPair);
#undef STLDECL_MANAGEDTYPE

#define STLDECL_MANAGEDTYPE Pair<Mogre::MovableObject^, Mogre::SceneQuery::WorldFragment^>
CPP_DECLARE_STLLIST(, SceneQueryMovableWorldFragmentIntersectionList, STLDECL_MANAGEDTYPE, Ogre::SceneQueryMovableObjectWorldFragmentPair);
#undef STLDECL_MANAGEDTYPE

//################################################################
//SceneQueryListener_Proxy
//################################################################

bool SceneQueryListener_Proxy::queryResult(Ogre::MovableObject* object)
{
	bool mp_return = _managed->QueryResult(object);
	return mp_return;
}

bool SceneQueryListener_Proxy::queryResult(Ogre::SceneQuery::WorldFragment* fragment)
{
	bool mp_return = _managed->QueryResult(fragment);
	return mp_return;
}

//################################################################
//RaySceneQueryListener_Proxy
//################################################################
bool RaySceneQueryListener_Proxy::queryResult(Ogre::MovableObject* obj, Ogre::Real distance)
{
	bool mp_return = _managed->QueryResult(obj, distance);
	return mp_return;
}

bool RaySceneQueryListener_Proxy::queryResult(Ogre::SceneQuery::WorldFragment* fragment, Ogre::Real distance)
{
	bool mp_return = _managed->QueryResult(fragment, distance);
	return mp_return;
}

bool IntersectionSceneQueryListener_Proxy::queryResult(Ogre::MovableObject* first, Ogre::MovableObject* second)
{
	bool mp_return = _managed->QueryResult(first, second);
	return mp_return;
}

bool IntersectionSceneQueryListener_Proxy::queryResult(Ogre::MovableObject* movable, Ogre::SceneQuery::WorldFragment* fragment)
{
	bool mp_return = _managed->QueryResult(movable, fragment);
	return mp_return;
}
