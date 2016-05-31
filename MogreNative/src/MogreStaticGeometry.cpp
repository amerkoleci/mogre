#include "stdafx.h"
#include "MogreStaticGeometry.h"

using namespace Mogre;

StaticGeometry::~StaticGeometry()
{
	this->!StaticGeometry();
}

StaticGeometry::!StaticGeometry()
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