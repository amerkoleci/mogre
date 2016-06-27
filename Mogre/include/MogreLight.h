#pragma once

#include "OgreLight.h"
#include "MogreMovableObject.h"

namespace Mogre
{
	public ref class Light : public MovableObject
	{
	public:
		enum class LightTypes
		{
			LT_POINT = Ogre::Light::LT_POINT,
			LT_DIRECTIONAL = Ogre::Light::LT_DIRECTIONAL,
			LT_SPOTLIGHT = Ogre::Light::LT_SPOTLIGHT,
			Point = Ogre::Light::LT_POINT,
			Directional = Ogre::Light::LT_DIRECTIONAL,
			Spotlight = Ogre::Light::LT_SPOTLIGHT
		};

	public protected:
		Light(Ogre::Light* obj) : MovableObject(obj)
		{

		}

	public:

		property Ogre::Real tempSquareDist
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real value);
		}

		property Mogre::Vector4 As4DVector
		{
		public:
			Mogre::Vector4 get();
		}

		property Ogre::Real AttenuationConstant
		{
		public:
			Ogre::Real get();
		}

		property Ogre::Real AttenuationLinear
		{
		public:
			Ogre::Real get();
		}

		property Ogre::Real AttenuationQuadric
		{
		public:
			Ogre::Real get();
		}

		property Ogre::Real AttenuationRange
		{
		public:
			Ogre::Real get();
		}

		property Mogre::Vector3 DerivedDirection
		{
		public:
			Mogre::Vector3 get();
		}

		property Mogre::Vector3 DerivedDirectionUpdated
		{
		public:
			Mogre::Vector3 get();
		}

		property Mogre::ColourValue DiffuseColour
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
			void set(Mogre::Vector3 vec);
		}

		property String^ MovableType
		{
		public:
			String^ get();
		}

		property Ogre::Real PowerScale
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real power);
		}

		property Mogre::ColourValue SpecularColour
		{
		public:
			Mogre::ColourValue get();
		public:
			void set(Mogre::ColourValue colour);
		}

		property Ogre::Real SpotlightFalloff
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real val);
		}

		property Mogre::Radian SpotlightInnerAngle
		{
		public:
			Mogre::Radian get();
		public:
			void set(Mogre::Radian val);
		}

		property Mogre::Radian SpotlightOuterAngle
		{
		public:
			Mogre::Radian get();
		public:
			void set(Mogre::Radian val);
		}

		property Mogre::Light::LightTypes Type
		{
		public:
			Mogre::Light::LightTypes get();
		public:
			void set(Mogre::Light::LightTypes type);
		}

		property bool Visible
		{
		public:
			bool get();
		public:
			void set(bool visible);
		}

		property Ogre::Real ShadowNearClipDistance
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real visible);
		}

		property Ogre::Real ShadowFarClipDistance
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real visible);
		}

		void _calcTempSquareDist(Mogre::Vector3 worldPos);

		void SetDiffuseColour(Ogre::Real red, Ogre::Real green, Ogre::Real blue);

		void SetSpecularColour(Ogre::Real red, Ogre::Real green, Ogre::Real blue);

		void SetAttenuation(Ogre::Real range, Ogre::Real constant, Ogre::Real linear, Ogre::Real quadratic);

		void SetDirection(Ogre::Real x, Ogre::Real y, Ogre::Real z);

		void SetSpotlightRange(Mogre::Radian innerAngle, Mogre::Radian outerAngle, Ogre::Real falloff);
		void SetSpotlightRange(Mogre::Radian innerAngle, Mogre::Radian outerAngle);

		DEFINE_MANAGED_NATIVE_CONVERSIONS_GET_MANAGED(Light);
	};
}