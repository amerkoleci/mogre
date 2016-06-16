#pragma once

#include "OgreParticleSystem.h"
#include "OgreParticleEmitter.h"
#include "OgreParticleSystemRenderer.h"
#include "MogreMovableObject.h"
#include "MogreCommon.h"

namespace Mogre
{
	public ref class ParticleEmitter // : public IStringInterface
	{
	internal:
		Ogre::ParticleEmitter* _native;
		bool _createdByCLR;

	public protected:
		ParticleEmitter(IntPtr ptr) : _native((Ogre::ParticleEmitter*)ptr.ToPointer())
		{

		}

		ParticleEmitter(Ogre::ParticleEmitter* ptr) : _native(ptr)
		{

		}

		//virtual Ogre::StringInterface* _IStringInterface_GetNativePtr() = IStringInterface::_GetNativePtr;

	public:
		property Mogre::Radian Angle
		{
		public:
			Mogre::Radian get();
		public:
			void set(Mogre::Radian angle);
		}

		property Mogre::ColourValue Colour
		{
		public:
			Mogre::ColourValue get();
		public:
			void set(Mogre::ColourValue colour);
		}

		property Mogre::ColourValue ColourRangeEnd
		{
		public:
			Mogre::ColourValue get();
		public:
			void set(Mogre::ColourValue colour);
		}

		property Mogre::ColourValue ColourRangeStart
		{
		public:
			Mogre::ColourValue get();
		public:
			void set(Mogre::ColourValue colour);
		}

		property Mogre::Vector3 Direction
		{
		public:
			Mogre::Vector3 get();
		public:
			void set(Mogre::Vector3 direction);
		}

		property Ogre::Real Duration
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real duration);
		}

		property Ogre::Real EmissionRate
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real particlesPerSecond);
		}

		property String^ EmittedEmitter
		{
		public:
			String^ get();
		public:
			void set(String^ emittedEmitter);
		}

		property bool Enabled
		{
		public:
			bool get();
		public:
			void set(bool enabled);
		}

		property bool IsEmitted
		{
		public:
			bool get();
		}

		property Ogre::Real MaxDuration
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real max);
		}

		property Ogre::Real MaxParticleVelocity
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real max);
		}

		property Ogre::Real MaxRepeatDelay
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real max);
		}

		property Ogre::Real MaxTimeToLive
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real max);
		}

		property Ogre::Real MinDuration
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real min);
		}

		property Ogre::Real MinParticleVelocity
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real min);
		}

		property Ogre::Real MinRepeatDelay
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real min);
		}

		property Ogre::Real MinTimeToLive
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real min);
		}

		property String^ Name
		{
		public:
			String^ get();
		public:
			void set(String^ newName);
		}

		property Ogre::Real ParticleVelocity
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real speed);
		}

		property Mogre::Vector3 Position
		{
		public:
			Mogre::Vector3 get();
		public:
			void set(Mogre::Vector3 pos);
		}

		property Ogre::Real RepeatDelay
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real duration);
		}

		property Ogre::Real StartTime
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real startTime);
		}

		property Ogre::Real TimeToLive
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real ttl);
		}

		property String^ Type
		{
		public:
			String^ get();
		}

		void SetParticleVelocity(Ogre::Real min, Ogre::Real max);

		void SetTimeToLive(Ogre::Real minTtl, Ogre::Real maxTtl);

		void SetColour(Mogre::ColourValue colourStart, Mogre::ColourValue colourEnd);

		void SetDuration(Ogre::Real min, Ogre::Real max);

		void SetRepeatDelay(Ogre::Real min, Ogre::Real max);

		void SetEmitted(bool emitted);

		//Mogre::Const_ParameterList^ GetParameters();
		virtual bool SetParameter(String^ name, String^ value);
		virtual void SetParameterList(Mogre::Const_NameValuePairList^ paramList);
		virtual String^ GetParameter(String^ name);

	internal:
		property Ogre::ParticleEmitter* UnmanagedPointer
		{
			Ogre::ParticleEmitter* get();
		}
	};

	public ref class ParticleSystemRenderer // : public IStringInterface
	{
	internal:
		Ogre::ParticleSystemRenderer* _native;
		bool _createdByCLR;

	public protected:
		ParticleSystemRenderer(IntPtr ptr) : _native((Ogre::ParticleSystemRenderer*)ptr.ToPointer())
		{

		}

		ParticleSystemRenderer(Ogre::ParticleSystemRenderer* ptr) : _native(ptr)
		{

		}

		//virtual Ogre::StringInterface* _IStringInterface_GetNativePtr() = IStringInterface::_GetNativePtr;

	public:
		property String^ Type
		{
		public:
			String^ get();
		}

		//Mogre::Const_ParameterList^ GetParameters();

		virtual bool SetParameter(String^ name, String^ value);
		virtual String^ GetParameter(String^ name);

		virtual void SetParameterList(Mogre::Const_NameValuePairList^ paramList);

	internal:
		property Ogre::ParticleSystemRenderer* UnmanagedPointer
		{
			Ogre::ParticleSystemRenderer* get();
		}
	};

	public ref class ParticleSystem : public MovableObject
	{
	public protected:
		ParticleSystem(IntPtr ptr) : MovableObject(ptr)
		{

		}

		ParticleSystem(Ogre::ParticleSystem* ptr) : MovableObject(ptr)
		{

		}

	public:

		property bool CullIndividually
		{
		public:
			bool get();
		public:
			void set(bool cullIndividual);
		}

		property Ogre::Real DefaultHeight
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real height);
		}

		property Ogre::Real DefaultIterationInterval
		{
		public:
			static Ogre::Real get();
		public:
			static void set(Ogre::Real iterationInterval);
		}

		property Ogre::Real DefaultNonVisibleUpdateTimeout
		{
		public:
			static Ogre::Real get();
		public:
			static void set(Ogre::Real timeout);
		}

		property Ogre::Real DefaultWidth
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real width);
		}

		property size_t EmittedEmitterQuota
		{
		public:
			size_t get();
		public:
			void set(size_t quota);
		}

		property Ogre::Real IterationInterval
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real iterationInterval);
		}

		property bool KeepParticlesInLocalSpace
		{
		public:
			bool get();
		public:
			void set(bool keepLocal);
		}

		property String^ MaterialName
		{
		public:
			String^ get();
		public:
			void set(String^ name);
		}

		property String^ MovableType
		{
		public:
			String^ get();
		}

		property Ogre::Real NonVisibleUpdateTimeout
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real timeout);
		}

		property unsigned short NumAffectors
		{
		public:
			unsigned short get();
		}

		property unsigned short NumEmitters
		{
		public:
			unsigned short get();
		}

		property size_t NumParticles
		{
		public:
			size_t get();
		}

		property String^ Origin
		{
		public:
			String^ get();
		}

		property size_t ParticleQuota
		{
		public:
			size_t get();
		public:
			void set(size_t quota);
		}

		property Mogre::ParticleSystemRenderer^ Renderer
		{
		public:
			Mogre::ParticleSystemRenderer^ get();
		}

		property String^ RendererName
		{
		public:
			String^ get();
		}

		property Ogre::uint8 RenderQueueGroup
		{
		public:
			Ogre::uint8 get();
		public:
			void set(Ogre::uint8 queueID);
		}

		property String^ ResourceGroupName
		{
		public:
			String^ get();
		}

		property bool SortingEnabled
		{
		public:
			bool get();
		public:
			void set(bool enabled);
		}

		property Ogre::Real SpeedFactor
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real speedFactor);
		}

		void SetRenderer(String^ typeName);

		Mogre::ParticleEmitter^ AddEmitter(String^ emitterType);
		Mogre::ParticleEmitter^ GetEmitter(unsigned short index);
		void RemoveEmitter(unsigned short index);
		void RemoveEmitter(Mogre::ParticleEmitter^ emitter);
		void RemoveAllEmitters();

		void Clear();

		void FastForward(Mogre::Real time, Mogre::Real interval);
		void FastForward(Mogre::Real time);

		void SetDefaultDimensions(Mogre::Real width, Mogre::Real height);

		void SetBoundsAutoUpdated(bool autoUpdate, Mogre::Real stopIn);
		void SetBoundsAutoUpdated(bool autoUpdate);

		//Mogre::Const_ParameterList^ GetParameters();
		virtual bool SetParameter(String^ name, String^ value);
		virtual void SetParameterList(Mogre::Const_NameValuePairList^ paramList);
		virtual String^ GetParameter(String^ name);

		DEFINE_MANAGED_NATIVE_CONVERSIONS(ParticleSystem);
	};
}