#include "stdafx.h"
#include "FileSystemLayer.h"
#include "Marshalling.h"
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
	DECLARE_NATIVE_STRING(o_filename, filename);

	return TO_CLR_STRING(_native->getConfigFilePath(o_filename));
}

String^ FileSystemLayer::GetWritablePath(String^ filename)
{
	DECLARE_NATIVE_STRING(o_filename, filename);
	return TO_CLR_STRING(_native->getWritablePath(o_filename));
}