#include "stdafx.h"
#include "MogreSceneQuery.h"

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