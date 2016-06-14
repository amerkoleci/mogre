#include "stdafx.h"
#include "MogreLog.h"

using namespace Mogre;

void LogListener_Director::messageLogged(const Ogre::String& message, Ogre::LogMessageLevel lml, bool maskDebug, const Ogre::String& logName, bool& skipThisMessage)
{
	if (doCallForMessageLogged)
	{
		_receiver->MessageLogged(TO_CLR_STRING(message), (Mogre::LogMessageLevel)lml, maskDebug, TO_CLR_STRING(logName), skipThisMessage);
	}
}

Log::Log(String^ name, bool debugOutput, bool suppressFileOutput)
{
	_createdByCLR = true;
	DECLARE_NATIVE_STRING(o_name, name);

	_native = new Ogre::Log(o_name, debugOutput, suppressFileOutput);
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

Log::Log(String^ name, bool debugOutput)
{
	_createdByCLR = true;
	DECLARE_NATIVE_STRING(o_name, name);

	_native = new Ogre::Log(o_name, debugOutput);
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

Log::Log(String^ name)
{
	_createdByCLR = true;
	DECLARE_NATIVE_STRING(o_name, name);

	_native = new Ogre::Log(o_name);
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

Log::~Log()
{
	this->!Log();
}

Log::!Log()
{
	OnDisposing(this, nullptr);

	if (IsDisposed)
		return;

	if (_logListener != 0)
	{
		if (_native != 0) static_cast<Ogre::Log*>(_native)->removeListener(_logListener);
		delete _logListener; _logListener = 0;
	}
	if (_createdByCLR &&_native)
	{
		ObjectTable::Remove((intptr_t)_native);
		delete _native;
		_native = 0;
	}

	OnDisposed(this, nullptr);
}

bool Log::IsDisposed::get()
{
	return (_native == nullptr);
}

bool Log::IsDebugOutputEnabled::get()
{
	return static_cast<const Ogre::Log*>(_native)->isDebugOutputEnabled();
}

bool Log::IsFileOutputSuppressed::get()
{
	return static_cast<const Ogre::Log*>(_native)->isFileOutputSuppressed();
}

Mogre::LoggingLevel Log::LogDetail::get()
{
	return (Mogre::LoggingLevel)static_cast<const Ogre::Log*>(_native)->getLogDetail();
}
void Log::LogDetail::set(Mogre::LoggingLevel ll)
{
	static_cast<Ogre::Log*>(_native)->setLogDetail((Ogre::LoggingLevel)ll);
}

String^ Log::Name::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::Log*>(_native)->getName());
}

void Log::LogMessage(String^ message, Mogre::LogMessageLevel lml, bool maskDebug)
{
	DECLARE_NATIVE_STRING(o_message, message);

	static_cast<Ogre::Log*>(_native)->logMessage(o_message, (Ogre::LogMessageLevel)lml, maskDebug);
}

void Log::LogMessage(String^ message, Mogre::LogMessageLevel lml)
{
	DECLARE_NATIVE_STRING(o_message, message);

	static_cast<Ogre::Log*>(_native)->logMessage(o_message, (Ogre::LogMessageLevel)lml);
}

void Log::LogMessage(String^ message)
{
	DECLARE_NATIVE_STRING(o_message, message);

	static_cast<Ogre::Log*>(_native)->logMessage(o_message);
}

void Log::SetDebugOutputEnabled(bool debugOutput)
{
	static_cast<Ogre::Log*>(_native)->setDebugOutputEnabled(debugOutput);
}

Ogre::Log* Log::UnmanagedPointer::get()
{
	return static_cast<Ogre::Log*>(_native);
}