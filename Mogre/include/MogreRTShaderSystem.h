#pragma once

#ifdef INCLUDE_RTSHADER_SYSTEM

#include "OgreRTShaderSystem.h"
#include "MogreCommon.h"
#include "MogreCamera.h"
#include "Marshalling.h"

namespace Mogre
{
	ref class SceneManager;

	namespace RTShader
	{
		public ref class ShaderGenerator : IMogreDisposable
		{
		public:
			/// <summary>Raised before any disposing is performed.</summary>
			virtual event EventHandler^ OnDisposing;
			/// <summary>Raised once all disposing is performed.</summary>
			virtual event EventHandler^ OnDisposed;

		private:
			Ogre::RTShader::ShaderGenerator* _native;
			static ShaderGenerator^ _singleton;

		public protected:
			ShaderGenerator(Ogre::RTShader::ShaderGenerator* obj) : _native(obj)
			{
			}

		public:
			~ShaderGenerator();
		protected:
			!ShaderGenerator();
		public:
			static property ShaderGenerator^ Singleton
			{
				ShaderGenerator^ get()
				{
					if (_singleton == CLR_NULL)
					{
						auto ptr = Ogre::RTShader::ShaderGenerator::getSingletonPtr();
						if (ptr) _singleton = gcnew ShaderGenerator(ptr);
					}
					return _singleton;
				}
			}

			property bool IsDisposed
			{
				virtual bool get();
			}

			static bool Initialize();

			void AddSceneManager(Mogre::SceneManager^ manager);

		internal:
			property Ogre::RTShader::ShaderGenerator* UnmanagedPointer
			{
				Ogre::RTShader::ShaderGenerator* get();
			}
		};
	}
}

#endif