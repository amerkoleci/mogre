#include "stdafx.h"
#include "MogreLogManager.h"
#include "MogreLog.h"
#include "Marshalling.h"

using namespace Mogre;

LogManager::LogManager()
{
	_createdByCLR = true;
	_native = new Ogre::LogManager();
}

Mogre::Log^ LogManager::DefaultLog::get()
{
	ReturnCachedObjectGcnew(Mogre::Log, _defaultLog, _native->getDefaultLog());
}

Mogre::Log^ LogManager::CreateLog(String^ name, bool defaultLog, bool debuggerOutput, bool suppressFileOutput)
{
	DECLARE_NATIVE_STRING(o_name, name);
	return gcnew Log(_native->createLog(o_name, defaultLog, debuggerOutput, suppressFileOutput));
}

Mogre::Log^ LogManager::CreateLog(String^ name, bool defaultLog, bool debuggerOutput)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return gcnew Log(_native->createLog(o_name, defaultLog, debuggerOutput));
}

Mogre::Log^ LogManager::CreateLog(String^ name, bool defaultLog)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return gcnew Log(_native->createLog(o_name, defaultLog));
}

Mogre::Log^ LogManager::CreateLog(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return gcnew Log(_native->createLog(o_name));
}

Mogre::Log^ LogManager::GetLog(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return _native->getLog(o_name);
}

void LogManager::DestroyLog(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	_native->destroyLog(o_name);
}

void LogManager::DestroyLog(Mogre::Log^ log)
{
	_native->destroyLog(GetPointerOrNull(log));
}

Mogre::Log^ LogManager::SetDefaultLog(Mogre::Log^ newLog)
{
	_defaultLog = newLog;
	auto oldLog = _native->setDefaultLog(GetUnmanagedNullable(newLog));
	ReturnCachedObjectGcnewNullable(Mogre::Log, _oldDefaultLog, oldLog);
}

void LogManager::LogMessage(String^ message, Mogre::LogMessageLevel lml, bool maskDebug)
{
	DECLARE_NATIVE_STRING(o_message, message);

	_native->logMessage(o_message, (Ogre::LogMessageLevel)lml, maskDebug);
}

void LogManager::LogMessage(String^ message, Mogre::LogMessageLevel lml)
{
	DECLARE_NATIVE_STRING(o_message, message);

	_native->logMessage(o_message, (Ogre::LogMessageLevel)lml);
}

void LogManager::LogMessage(String^ message)
{
	DECLARE_NATIVE_STRING(o_message, message);

	_native->logMessage(o_message);
}

void LogManager::LogMessage(Mogre::LogMessageLevel lml, String^ message, bool maskDebug)
{
	DECLARE_NATIVE_STRING(o_message, message);

	_native->logMessage((Ogre::LogMessageLevel)lml, o_message, maskDebug);
}
void LogManager::LogMessage(Mogre::LogMessageLevel lml, String^ message)
{
	DECLARE_NATIVE_STRING(o_message, message);

	_native->logMessage((Ogre::LogMessageLevel)lml, o_message);
}

void LogManager::SetLogDetail(Mogre::LoggingLevel ll)
{
	_native->setLogDetail((Ogre::LoggingLevel)ll);
}