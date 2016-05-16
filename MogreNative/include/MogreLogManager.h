#pragma once

#include "OgreLogManager.h"
#include "MogreLog.h"

namespace Mogre
{
	public ref class LogManager
	{
	private protected:
		static LogManager^ _singleton;
		Ogre::LogManager* _native;
		bool _createdByCLR;

	public protected:
		LogManager(Ogre::LogManager* obj) : _native(obj)
		{
		}

	public:
		LogManager();

		static property LogManager^ Singleton
		{
			LogManager^ get()
			{
				if (_singleton == CLR_NULL)
				{
					Ogre::LogManager* ptr = Ogre::LogManager::getSingletonPtr();
					if (ptr) _singleton = gcnew LogManager(ptr);
				}
				return _singleton;
			}
		}

		property Mogre::Log^ DefaultLog
		{
		public:
			Mogre::Log^ get();
		}

		Mogre::Log^ CreateLog(String^ name, bool defaultLog, bool debuggerOutput, bool suppressFileOutput);
		Mogre::Log^ CreateLog(String^ name, bool defaultLog, bool debuggerOutput);
		Mogre::Log^ CreateLog(String^ name, bool defaultLog);
		Mogre::Log^ CreateLog(String^ name);

		Mogre::Log^ GetLog(String^ name);

		void DestroyLog(String^ name);

		void DestroyLog(Mogre::Log^ log);

		Mogre::Log^ SetDefaultLog(Mogre::Log^ newLog);

		void LogMessage(String^ message, Mogre::LogMessageLevel lml, bool maskDebug);
		void LogMessage(String^ message, Mogre::LogMessageLevel lml);
		void LogMessage(String^ message);

		void LogMessage(Mogre::LogMessageLevel lml, String^ message, bool maskDebug);
		void LogMessage(Mogre::LogMessageLevel lml, String^ message);

		void SetLogDetail(Mogre::LoggingLevel ll);
	};
}