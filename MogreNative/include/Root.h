#pragma once

#include "IDisposable.h"

namespace Mogre
{
	public ref class Root : IDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	private:
		static bool _instantiated;

		Ogre::Root* _root;

	public:
		static Root();

		/// <summary>Creates a new instance of the Root class.</summary>
		Root(String^ pluginFileName, String^ configFileName, String^ logFileName);

		/// <summary>Creates a new instance of the Root class.</summary>
		Root(String^ pluginFileName, String^ configFileName);

		/// <summary>Creates a new instance of the Root class.</summary>
		Root(String^ pluginFileName);

		/// <summary>Creates a new instance of the Root class.</summary>
		Root();

	public:
		~Root();
	protected:
		!Root();
	public:
		property bool IsDisposed
		{
			virtual bool get();
		}

		void Shutdown();
		void StartRendering();
		bool RenderOneFrame();
		bool RenderOneFrame(float timeSinceLastFrame);

	private:
		void Create(const char* pluginFileName, const char* configFileName, const char* logFileName);
		void Init();
		void PostInit();
	};

	public enum class InstancingThreadedCullingMethod
	{
		SingleThread = Ogre::INSTANCING_CULLING_SINGLETHREAD,
		Threaded = Ogre::INSTANCING_CULLING_THREADED
	};
}