#pragma once

#include "OgreRenderSystem.h"
#include "STLContainerWrappers.h"

namespace Mogre
{
	public enum class StencilOperation
	{
		SOP_KEEP = Ogre::SOP_KEEP,
		SOP_ZERO = Ogre::SOP_ZERO,
		SOP_REPLACE = Ogre::SOP_REPLACE,
		SOP_INCREMENT = Ogre::SOP_INCREMENT,
		SOP_DECREMENT = Ogre::SOP_DECREMENT,
		SOP_INCREMENT_WRAP = Ogre::SOP_INCREMENT_WRAP,
		SOP_DECREMENT_WRAP = Ogre::SOP_DECREMENT_WRAP,
		SOP_INVERT = Ogre::SOP_INVERT,
		Keep = Ogre::SOP_KEEP,
		Zero = Ogre::SOP_ZERO,
		Replace = Ogre::SOP_REPLACE,
		Increment = Ogre::SOP_INCREMENT,
		Decrement = Ogre::SOP_DECREMENT,
		IncrementWrap = Ogre::SOP_INCREMENT_WRAP,
		DecrementWrap = Ogre::SOP_DECREMENT_WRAP,
		Invert = Ogre::SOP_INVERT
	};

	public enum class TexCoordCalcMethod
	{
		TEXCALC_NONE = Ogre::TEXCALC_NONE,
		TEXCALC_ENVIRONMENT_MAP = Ogre::TEXCALC_ENVIRONMENT_MAP,
		TEXCALC_ENVIRONMENT_MAP_PLANAR = Ogre::TEXCALC_ENVIRONMENT_MAP_PLANAR,
		TEXCALC_ENVIRONMENT_MAP_REFLECTION = Ogre::TEXCALC_ENVIRONMENT_MAP_REFLECTION,
		TEXCALC_ENVIRONMENT_MAP_NORMAL = Ogre::TEXCALC_ENVIRONMENT_MAP_NORMAL,
		TEXCALC_PROJECTIVE_TEXTURE = Ogre::TEXCALC_PROJECTIVE_TEXTURE,
		None = Ogre::TEXCALC_NONE,
		EnvironmentMap = Ogre::TEXCALC_ENVIRONMENT_MAP,
		EnvironmentMapPlanar = Ogre::TEXCALC_ENVIRONMENT_MAP_PLANAR,
		EnvironmentMapReflection = Ogre::TEXCALC_ENVIRONMENT_MAP_REFLECTION,
		EnvironmentMapNormal = Ogre::TEXCALC_ENVIRONMENT_MAP_NORMAL,
		ProjectiveTexture = Ogre::TEXCALC_PROJECTIVE_TEXTURE
	};

	interface class IRenderSystem_Listener_Receiver
	{
		void EventOccurred(String^ eventName/*, Mogre::Const_NameValuePairList^ parameters*/);

	};

	//################################################################
	//Listener
	//################################################################

	class RenderSystem_Listener_Director : public Ogre::RenderSystem::Listener
	{
	private:
		gcroot<IRenderSystem_Listener_Receiver^> _receiver;
	public:
		RenderSystem_Listener_Director(IRenderSystem_Listener_Receiver^ recv)
			: _receiver(recv), doCallForEventOccurred(false)
		{
		}

		bool doCallForEventOccurred;

		virtual void eventOccurred(const Ogre::String& eventName, const Ogre::NameValuePairList* parameters) override;
	};

	public ref class RenderSystem : public IRenderSystem_Listener_Receiver
	{
	public:
		ref class Listener abstract sealed
		{
		public:
			delegate static void EventOccurredHandler(String^ eventName/*, Mogre::Const_NameValuePairList^ parameters*/);
		};

	internal:
		Ogre::RenderSystem* _native;

	private protected:
		RenderSystem_Listener_Director* _listener;
		Mogre::RenderSystem::Listener::EventOccurredHandler^ _eventOccurred;

	public protected:
		RenderSystem(intptr_t ptr) : _native((Ogre::RenderSystem*)ptr)
		{

		}

		~RenderSystem()
		{
			if (_listener != 0)
			{
				if (_native != 0) 
					_native->removeListener(_listener);
				delete _listener; _listener = 0;
			}
		}

	public:

		event Mogre::RenderSystem::Listener::EventOccurredHandler^ EventOccurred
		{
			void add(Mogre::RenderSystem::Listener::EventOccurredHandler^ hnd)
			{
				if (_eventOccurred == CLR_NULL)
				{
					if (_listener == 0)
					{
						_listener = new RenderSystem_Listener_Director(this);
						static_cast<Ogre::RenderSystem*>(_native)->addListener(_listener);
					}
					_listener->doCallForEventOccurred = true;
				}
				_eventOccurred += hnd;
			}
			void remove(Mogre::RenderSystem::Listener::EventOccurredHandler^ hnd)
			{
				_eventOccurred -= hnd;
				if (_eventOccurred == CLR_NULL) _listener->doCallForEventOccurred = false;
			}
		private:
			void raise(String^ eventName/*, Mogre::Const_NameValuePairList^ parameters*/)
			{
				if (_eventOccurred)
					_eventOccurred->Invoke(eventName/*, parameters*/);
			}
		}

		void SetConfigOption(String^ name, String^ value);

	internal:
		property Ogre::RenderSystem* UnmanagedPointer
		{
			Ogre::RenderSystem* get();
		}

	protected public:

		virtual void OnEventOccurred(String^ eventName/*, Mogre::Const_NameValuePairList^ parameters*/) = IRenderSystem_Listener_Receiver::EventOccurred
		{
			EventOccurred(eventName/*, parameters*/);
		}
	};

	//INC_DECLARE_STLVECTOR(RenderSystemList, Mogre::RenderSystem^, Ogre::RenderSystem*, public, private);
}