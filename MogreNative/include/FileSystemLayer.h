#pragma once

#include "IDisposable.h"
#include "OgreFileSystemLayer.h"

namespace Mogre
{
	public ref class FileSystemLayer : IDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	private:
		Ogre::FileSystemLayer* _native;

	public:
		/// <summary>Creates a new instance of the FileSystemLayer class.</summary>
		FileSystemLayer();

	public:
		~FileSystemLayer();
	protected:
		!FileSystemLayer();
	public:
		property bool IsDisposed
		{
			virtual bool get();
		}

		String^ GetConfigFilePath(String^ filename);
		String^ GetWritablePath(String^ filename);
	};
}