#include "stdafx.h"
#include "FileSystemLayer.h"
#include "Util.h"
using namespace Mogre;

FileSystemLayer::FileSystemLayer()
{
	_native = OGRE_NEW_T(Ogre::FileSystemLayer, Ogre::MEMCATEGORY_GENERAL)(OGRE_VERSION_NAME);
}

FileSystemLayer::~FileSystemLayer()
{
	this->!FileSystemLayer();
}

FileSystemLayer::!FileSystemLayer()
{
	OnDisposing(this, nullptr);

	if (IsDisposed)
		return;

	OGRE_DELETE_T(_native, FileSystemLayer, Ogre::MEMCATEGORY_GENERAL);
	_native = nullptr;

	OnDisposed(this, nullptr);
}

bool FileSystemLayer::IsDisposed::get()
{
	return (_native == nullptr);
}

String^ FileSystemLayer::GetConfigFilePath(String^ filename)
{
	return Util::ToManagedString(_native->getConfigFilePath(Util::ToUnmanagedString(filename)));
}

String^ FileSystemLayer::GetWritablePath(String^ filename)
{
	return Util::ToManagedString(_native->getWritablePath(Util::ToUnmanagedString(filename)));
}