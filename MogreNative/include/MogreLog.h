#pragma once

#include "OgreLight.h"
#include "Marshalling.h"

namespace Mogre
{
	public enum class LogMessageLevel
	{
		LML_TRIVIAL = Ogre::LML_TRIVIAL,
		LML_NORMAL = Ogre::LML_NORMAL,
		LML_CRITICAL = Ogre::LML_CRITICAL
	};

	public enum class LoggingLevel
	{
		LL_LOW = Ogre::LL_LOW,
		LL_NORMAL = Ogre::LL_NORMAL,
		LL_BOREME = Ogre::LL_BOREME
	};

	interface class ILogListener_Receiver
	{
		void MessageLogged(String^ message, Mogre::LogMessageLevel lml, bool maskDebug, String^ logName, bool% skipThisMessage);
	};

	public ref class LogListener abstract sealed
	{
	public:
		delegate static void MessageLoggedHandler(String^ message, Mogre::LogMessageLevel lml, bool maskDebug, String^ logName, bool% skipThisMessage);
	};

	//LogListener

	class LogListener_Director : public Ogre::LogListener
	{
	private:
		gcroot<ILogListener_Receiver^> _receiver;
	public:
		LogListener_Director(ILogListener_Receiver^ recv)
			: _receiver(recv), doCallForMessageLogged(false)
		{
		}

		bool doCallForMessageLogged;

		virtual void messageLogged(const Ogre::String& message, Ogre::LogMessageLevel lml, bool maskDebug, const Ogre::String &logName, bool& skipThisMessage) override;
	};

	//Log

	public ref class Log : IMogreDisposable, public ILogListener_Receiver
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;
	private protected:
		LogListener_Director* _logListener;
		Mogre::LogListener::MessageLoggedHandler^ _messageLogged;


		virtual void ClearNativePtr()// = INativePointer::ClearNativePtr
		{
			_native = 0;
		}

	public protected:
		Log(Ogre::Log* obj) : _native(obj), _createdByCLR(false)
		{
		}

		Ogre::Log* _native;
		bool _createdByCLR;

	public:
		~Log();
	protected:
		!Log();

	public:
		Log(String^ name, bool debugOutput, bool suppressFileOutput);
		Log(String^ name, bool debugOutput);
		Log(String^ name);


		event Mogre::LogListener::MessageLoggedHandler^ MessageLogged
		{
			void add(Mogre::LogListener::MessageLoggedHandler^ hnd)
			{
				if (_messageLogged == CLR_NULL)
				{
					if (_logListener == 0)
					{
						_logListener = new LogListener_Director(this);
						static_cast<Ogre::Log*>(_native)->addListener(_logListener);
					}
					_logListener->doCallForMessageLogged = true;
				}
				_messageLogged += hnd;
			}
			void remove(Mogre::LogListener::MessageLoggedHandler^ hnd)
			{
				_messageLogged -= hnd;
				if (_messageLogged == CLR_NULL) _logListener->doCallForMessageLogged = false;
			}
		private:
			void raise(String^ message, Mogre::LogMessageLevel lml, bool maskDebug, String^ logName, bool% skipThisMessage)
			{
				if (_messageLogged)
					_messageLogged->Invoke(message, lml, maskDebug, logName, skipThisMessage);
			}
		}

		property bool IsDisposed
		{
			virtual bool get();
		}


		property bool IsDebugOutputEnabled
		{
		public:
			bool get();
		}

		property bool IsFileOutputSuppressed
		{
		public:
			bool get();
		}

		property Mogre::LoggingLevel LogDetail
		{
		public:
			Mogre::LoggingLevel get();
		public:
			void set(Mogre::LoggingLevel ll);
		}

		property String^ Name
		{
		public:
			String^ get();
		}

		void LogMessage(String^ message, Mogre::LogMessageLevel lml, bool maskDebug);
		void LogMessage(String^ message, Mogre::LogMessageLevel lml);
		void LogMessage(String^ message);

		void SetDebugOutputEnabled(bool debugOutput);

	internal:
		property Ogre::Log* UnmanagedPointer
		{
			Ogre::Log* get();
		}

	protected public:
		virtual void OnMessageLogged(String^ message, Mogre::LogMessageLevel lml, bool maskDebug, String^ logName, bool% skipThisMessage) = ILogListener_Receiver::MessageLogged
		{
			MessageLogged(message, lml, maskDebug, logName, skipThisMessage);
		}
	};
}