#pragma once

#include "OgreCamera.h"
#include "MogreFrustum.h"
#include "MogreCommon.h"

namespace Mogre
{
	ref class SceneNode;
	ref class SceneManager;
	ref class Viewport;

	public ref class Camera : public Frustum
	{
	public protected:
		Camera(intptr_t ptr) : Frustum(ptr)
		{

		}

	public:
		property bool AutoAspectRatio
		{
		public:
			bool get();
		public:
			void set(bool autoratio);
		}

		property Mogre::Vector3 AutoTrackOffset
		{
		public:
			Mogre::Vector3 get();
		}

		property Mogre::SceneNode^ AutoTrackTarget
		{
		public:
			Mogre::SceneNode^ get();
		}

		property Mogre::Frustum^ CullingFrustum
		{
		public:
			Mogre::Frustum^ get();
		public:
			void set(Mogre::Frustum^ frustum);
		}

		property Mogre::Vector3 DerivedDirection
		{
		public:
			Mogre::Vector3 get();
		}

		property Mogre::Quaternion DerivedOrientation
		{
		public:
			Mogre::Quaternion get();
		}

		property Mogre::Vector3 DerivedPosition
		{
		public:
			Mogre::Vector3 get();
		}

		property Mogre::Vector3 DerivedRight
		{
		public:
			Mogre::Vector3 get();
		}

		property Mogre::Vector3 DerivedUp
		{
		public:
			Mogre::Vector3 get();
		}

		property Mogre::Vector3 Direction
		{
		public:
			Mogre::Vector3 get();
		public:
			void set(Mogre::Vector3 vec);
		}

		property Ogre::Real FarClipDistance
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real farDist);
		}

		property bool IsWindowSet
		{
		public:
			bool get();
		}

		property Ogre::Real LodBias
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real factor);
		}

		property String^ MovableType
		{
		public:
			String^ get();
		}

		property String^ Name
		{
		public:
			String^ get();
		}

		property Ogre::Real NearClipDistance
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real nearDist);
		}

		property Mogre::Quaternion Orientation
		{
		public:
			Mogre::Quaternion get();
		public:
			void set(Mogre::Quaternion q);
		}

		property Mogre::PolygonMode PolygonMode
		{
		public:
			Mogre::PolygonMode get();
		public:
			void set(Mogre::PolygonMode sd);
		}

		property Mogre::Vector3 Position
		{
		public:
			Mogre::Vector3 get();
		public:
			void set(Mogre::Vector3 vec);
		}

		property Mogre::Vector3 RealDirection
		{
		public:
			Mogre::Vector3 get();
		}

		property Mogre::Quaternion RealOrientation
		{
		public:
			Mogre::Quaternion get();
		}

		property Mogre::Vector3 RealPosition
		{
		public:
			Mogre::Vector3 get();
		}

		property Mogre::Vector3 RealRight
		{
		public:
			Mogre::Vector3 get();
		}

		property Mogre::Vector3 RealUp
		{
		public:
			Mogre::Vector3 get();
		}

		property Mogre::Vector3 Right
		{
		public:
			Mogre::Vector3 get();
		}

		property Mogre::SceneManager^ SceneManager
		{
		public:
			Mogre::SceneManager^ get();
		}

		property Mogre::Vector3 Up
		{
		public:
			Mogre::Vector3 get();
		}

		property bool UseRenderingDistance
		{
		public:
			bool get();
		public:
			void set(bool use);
		}

		property Mogre::Viewport^ LastViewport
		{
		public:
			Mogre::Viewport^ get();
		}


		property Mogre::Matrix4^ ViewMatrix
		{
		public:
			Mogre::Matrix4^ get();
		}
		
		property const Mogre::Vector3* WorldSpaceCorners
		{
		public:
			const Mogre::Vector3* get();
		}

		void SetPosition(Ogre::Real x, Ogre::Real y, Ogre::Real z);

		void Move(Mogre::Vector3 vec);

		void MoveRelative(Mogre::Vector3 vec);

		void SetDirection(Ogre::Real x, Ogre::Real y, Ogre::Real z);

		void LookAt(Mogre::Vector3 targetPoint);

		void LookAt(Ogre::Real x, Ogre::Real y, Ogre::Real z);

		void Roll(Mogre::Radian angle);

		void Yaw(Mogre::Radian angle);

		void Pitch(Mogre::Radian angle);

		void Rotate(Mogre::Vector3 axis, Mogre::Radian angle);

		void Rotate(Mogre::Quaternion q);

		void SetFixedYawAxis(bool useFixed, Mogre::Vector3 fixedAxis);
		void SetFixedYawAxis(bool useFixed);

		void SetAutoTracking(bool enabled, Mogre::SceneNode^ target, Mogre::Vector3 offset);
		void SetAutoTracking(bool enabled, Mogre::SceneNode^ target);
		void SetAutoTracking(bool enabled);

		Mogre::Ray GetCameraToViewportRay(Ogre::Real screenx, Ogre::Real screeny);
		void SetWindow(Mogre::Real Left, Mogre::Real Top, Mogre::Real Right, Mogre::Real Bottom);

		void ResetWindow();

		bool IsVisible(Mogre::AxisAlignedBox^ bound, [Out] Mogre::FrustumPlane% culledBy);
		bool IsVisible(Mogre::AxisAlignedBox^ bound);

		bool IsVisible(Mogre::Sphere bound, [Out] Mogre::FrustumPlane% culledBy);
		bool IsVisible(Mogre::Sphere bound);

		bool IsVisible(Mogre::Vector3 vert, [Out] Mogre::FrustumPlane% culledBy);
		bool IsVisible(Mogre::Vector3 vert);

		Mogre::Plane GetFrustumPlane(unsigned short plane);

		bool ProjectSphere(Mogre::Sphere sphere, [Out] Mogre::Real% left, [Out] Mogre::Real% top, [Out] Mogre::Real% right, [Out] Mogre::Real% bottom);

		Mogre::Matrix4^ GetViewMatrix(bool ownFrustumOnly);

		DEFINE_MANAGED_NATIVE_CONVERSIONS(Camera);

	internal:
		property Ogre::Camera* UnmanagedPointer
		{
			Ogre::Camera* get();
		}
	};
}