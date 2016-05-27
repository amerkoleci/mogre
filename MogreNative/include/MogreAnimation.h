#pragma once

#include "OgreAnimationTrack.h"
#include "OgreAnimationState.h"
#include "OgreKeyFrame.h"
#include "OgreAnimation.h"
#include "MogreCommon.h"
#include "Marshalling.h"
#include "IteratorWrapper.h"

namespace Mogre
{
	ref class AnimationTrack;
	ref class Animation;
	ref class Node;
	ref class Entity;

	public enum class VertexAnimationType
	{
		VAT_NONE = Ogre::VAT_NONE,
		VAT_MORPH = Ogre::VAT_MORPH,
		VAT_POSE = Ogre::VAT_POSE
	};

	//################################################################
	//TimeIndex
	//################################################################

	public ref class TimeIndex : IDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	public protected:
		TimeIndex(intptr_t obj) : _native((Ogre::TimeIndex*)obj), _createdByCLR(false)
		{
		}

		TimeIndex(Ogre::TimeIndex* obj) : _native(obj), _createdByCLR(false)
		{
		}

		Ogre::TimeIndex* _native;
		bool _createdByCLR;

	public:
		~TimeIndex();
	protected:
		!TimeIndex();

	public:
		TimeIndex(Ogre::Real timePos);
		TimeIndex(Ogre::Real timePos, Ogre::uint keyIndex);

		property bool IsDisposed
		{
			virtual bool get();
		}

		property bool HasKeyIndex
		{
		public:
			bool get();
		}

		property Ogre::uint KeyIndex
		{
		public:
			Ogre::uint get();
		}

		property Ogre::Real TimePos
		{
		public:
			Ogre::Real get();
		}

	internal:
		property Ogre::TimeIndex* UnmanagedPointer
		{
			Ogre::TimeIndex* get();
		}
	};

	//################################################################
	//KeyFrame
	//################################################################

	public ref class KeyFrame : IDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	public protected:
		Ogre::KeyFrame* _native;
		bool _createdByCLR;

		KeyFrame(Ogre::KeyFrame* obj) : _native(obj)
		{
		}

		KeyFrame(intptr_t obj) : _native((Ogre::KeyFrame*)obj)
		{
		}

	public:
		~KeyFrame();
	protected:
		!KeyFrame();

	public:
		KeyFrame(Mogre::AnimationTrack^ parent, Ogre::Real time);

		property bool IsDisposed
		{
			virtual bool get();
		}

		property Ogre::Real Time
		{
		public:
			Ogre::Real get();
		}

		Mogre::KeyFrame^ _clone(Mogre::AnimationTrack^ newParent);

	internal:
		property Ogre::KeyFrame* UnmanagedPointer
		{
			Ogre::KeyFrame* get()
			{
				return _native;
			}
		}
	};

	//################################################################
	//NumericKeyFrame
	//################################################################

	public ref class NumericKeyFrame : public KeyFrame
	{
	public protected:
		NumericKeyFrame(Ogre::NumericKeyFrame* obj) : KeyFrame(obj)
		{
		}

		NumericKeyFrame(intptr_t ptr) : KeyFrame(ptr)
		{
		}

	public:
		NumericKeyFrame(Mogre::AnimationTrack^ parent, Mogre::Real time);

		Mogre::KeyFrame^ _clone(Mogre::AnimationTrack^ newParent);

		DEFINE_MANAGED_NATIVE_CONVERSIONS(NumericKeyFrame);

	internal:
		property Ogre::NumericKeyFrame* UnmanagedPointer
		{
			Ogre::NumericKeyFrame* get()
			{
				return static_cast<Ogre::NumericKeyFrame*>(_native);
			}
		}
	};

	//################################################################
	//TransformKeyFrame
	//################################################################

	public ref class TransformKeyFrame : public KeyFrame
	{
	public protected:
		TransformKeyFrame(Ogre::TransformKeyFrame* obj) : KeyFrame(obj)
		{
		}

		TransformKeyFrame(intptr_t ptr) : KeyFrame(ptr)
		{
		}

	public:
		TransformKeyFrame(Mogre::AnimationTrack^ parent, Mogre::Real time);

		property Mogre::Quaternion Rotation
		{
		public:
			Mogre::Quaternion get();
		public:
			void set(Mogre::Quaternion rot);
		}

		property Mogre::Vector3 Scale
		{
		public:
			Mogre::Vector3 get();
		public:
			void set(Mogre::Vector3 scale);
		}

		property Mogre::Vector3 Translate
		{
		public:
			Mogre::Vector3 get();
		public:
			void set(Mogre::Vector3 trans);
		}

		Mogre::KeyFrame^ _clone(Mogre::AnimationTrack^ newParent);

		DEFINE_MANAGED_NATIVE_CONVERSIONS(TransformKeyFrame);

	internal:
		property Ogre::TransformKeyFrame* UnmanagedPointer
		{
			Ogre::TransformKeyFrame* get()
			{
				return static_cast<Ogre::TransformKeyFrame*>(_native);
			}
		}
	};

	//################################################################
	//AnimationTrack
	//################################################################

	public ref class AnimationTrack : IDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	public protected:
		Ogre::AnimationTrack* _native;
		bool _createdByCLR;

		AnimationTrack(Ogre::AnimationTrack* obj) : _native(obj)
		{
		}

		AnimationTrack(intptr_t ptr) : _native((Ogre::AnimationTrack*)ptr)
		{
		}

	public:
		~AnimationTrack();
	protected:
		!AnimationTrack();

	public:
		property bool IsDisposed
		{
			virtual bool get()
			{
				return (_native == nullptr);
			}
		}

		property unsigned short Handle
		{
		public:
			unsigned short get();
		}

		property bool HasNonZeroKeyFrames
		{
		public:
			bool get();
		}

		property unsigned short NumKeyFrames
		{
		public:
			unsigned short get();
		}

		Mogre::KeyFrame^ GetKeyFrame(unsigned short index);

		Mogre::Real GetKeyFramesAtTime(Mogre::TimeIndex^ timeIndex, [Out] Mogre::KeyFrame^% keyFrame1, [Out] Mogre::KeyFrame^% keyFrame2, [Out] unsigned short% firstKeyIndex);
		Mogre::Real GetKeyFramesAtTime(Mogre::TimeIndex^ timeIndex, [Out] Mogre::KeyFrame^% keyFrame1, [Out] Mogre::KeyFrame^% keyFrame2);

		Mogre::KeyFrame^ CreateKeyFrame(Mogre::Real timePos);

		void RemoveKeyFrame(unsigned short index);

		void RemoveAllKeyFrames();

		void GetInterpolatedKeyFrame(Mogre::TimeIndex^ timeIndex, Mogre::KeyFrame^ kf);

		void Apply(Mogre::TimeIndex^ timeIndex, Mogre::Real weight, Mogre::Real scale);
		void Apply(Mogre::TimeIndex^ timeIndex, Mogre::Real weight);
		void Apply(Mogre::TimeIndex^ timeIndex);
		void Optimise();

		DEFINE_MANAGED_NATIVE_CONVERSIONS(AnimationTrack);

	internal:
		property Ogre::AnimationTrack* UnmanagedPointer
		{
			Ogre::AnimationTrack* get();
		}
	};

	//################################################################
	//NumericAnimationTrack
	//################################################################

	public ref class NumericAnimationTrack : public AnimationTrack
	{
	public protected:
		NumericAnimationTrack(Ogre::NumericAnimationTrack* obj) : AnimationTrack(obj)
		{
		}

		NumericAnimationTrack(intptr_t ptr) : AnimationTrack((Ogre::NumericAnimationTrack*)ptr)
		{
		}

	public:
		/*NumericAnimationTrack(Mogre::Animation^ parent, unsigned short handle);
		NumericAnimationTrack(Mogre::Animation^ parent, unsigned short handle, Mogre::AnimableValuePtr^ target);

		Mogre::NumericKeyFrame^ CreateNumericKeyFrame(Mogre::Real timePos);

		void GetInterpolatedKeyFrame(Mogre::TimeIndex^ timeIndex, Mogre::KeyFrame^ kf);

		void Apply(Mogre::TimeIndex^ timeIndex, Mogre::Real weight, Mogre::Real scale);
		void Apply(Mogre::TimeIndex^ timeIndex, Mogre::Real weight);
		void Apply(Mogre::TimeIndex^ timeIndex);

		void ApplyToAnimable(Mogre::AnimableValuePtr^ anim, Mogre::TimeIndex^ timeIndex, Mogre::Real weight, Mogre::Real scale);
		void ApplyToAnimable(Mogre::AnimableValuePtr^ anim, Mogre::TimeIndex^ timeIndex, Mogre::Real weight);
		void ApplyToAnimable(Mogre::AnimableValuePtr^ anim, Mogre::TimeIndex^ timeIndex);

		Mogre::AnimableValuePtr^ GetAssociatedAnimable();

		void SetAssociatedAnimable(Mogre::AnimableValuePtr^ val);

		Mogre::NumericKeyFrame^ GetNumericKeyFrame(unsigned short index);

		Mogre::NumericAnimationTrack^ _clone(Mogre::Animation^ newParent);*/

		DEFINE_MANAGED_NATIVE_CONVERSIONS(NumericAnimationTrack);

	internal:
		property Ogre::NumericAnimationTrack* UnmanagedPointer
		{
			Ogre::NumericAnimationTrack* get()
			{
				return static_cast<Ogre::NumericAnimationTrack*>(_native);
			}
		}
	};

	//################################################################
	//NodeAnimationTrack
	//################################################################

	public ref class NodeAnimationTrack : public AnimationTrack
	{
	public protected:
		NodeAnimationTrack(Ogre::NodeAnimationTrack* obj) : AnimationTrack(obj)
		{
		}

		NodeAnimationTrack(intptr_t ptr) : AnimationTrack((Ogre::NodeAnimationTrack*)ptr)
		{
		}

	public:
		NodeAnimationTrack(Mogre::Animation^ parent, unsigned short handle);
		NodeAnimationTrack(Mogre::Animation^ parent, unsigned short handle, Mogre::Node^ targetNode);


		property Mogre::Node^ AssociatedNode
		{
		public:
			Mogre::Node^ get();
		public:
			void set(Mogre::Node^ node);
		}

		property bool HasNonZeroKeyFrames
		{
		public:
			bool get();
		}

		property bool UseShortestRotationPath
		{
		public:
			bool get();
		public:
			void set(bool useShortestPath);
		}

		Mogre::TransformKeyFrame^ CreateNodeKeyFrame(Mogre::Real timePos);

		void ApplyToNode(Mogre::Node^ node, Mogre::TimeIndex^ timeIndex, Mogre::Real weight, Mogre::Real scale);
		void ApplyToNode(Mogre::Node^ node, Mogre::TimeIndex^ timeIndex, Mogre::Real weight);
		void ApplyToNode(Mogre::Node^ node, Mogre::TimeIndex^ timeIndex);

		void GetInterpolatedKeyFrame(Mogre::TimeIndex^ timeIndex, Mogre::KeyFrame^ kf);

		void Apply(Mogre::TimeIndex^ timeIndex, Mogre::Real weight, Mogre::Real scale);
		void Apply(Mogre::TimeIndex^ timeIndex, Mogre::Real weight);
		void Apply(Mogre::TimeIndex^ timeIndex);

		Mogre::TransformKeyFrame^ GetNodeKeyFrame(unsigned short index);

		void Optimise();

		DEFINE_MANAGED_NATIVE_CONVERSIONS(NodeAnimationTrack);

	internal:
		property Ogre::NodeAnimationTrack* UnmanagedPointer
		{
			Ogre::NodeAnimationTrack* get()
			{
				return static_cast<Ogre::NodeAnimationTrack*>(_native);
			}
		}
	};

	public ref class VertexAnimationTrack : public AnimationTrack
	{
	public:
		enum class TargetMode
		{
			TM_SOFTWARE = Ogre::VertexAnimationTrack::TM_SOFTWARE,
			TM_HARDWARE = Ogre::VertexAnimationTrack::TM_HARDWARE
		};

	public protected:
		VertexAnimationTrack(Ogre::VertexAnimationTrack* obj) : AnimationTrack(obj)
		{
		}

		VertexAnimationTrack(intptr_t ptr) : AnimationTrack((Ogre::VertexAnimationTrack*)ptr)
		{
		}

	public:
		DEFINE_MANAGED_NATIVE_CONVERSIONS(VertexAnimationTrack);

	internal:
		property Ogre::VertexAnimationTrack* UnmanagedPointer
		{
			Ogre::VertexAnimationTrack* get()
			{
				return static_cast<Ogre::VertexAnimationTrack*>(_native);
			}
		}
	};

	//################################################################
	//AnimationState
	//################################################################

	public ref class AnimationState
	{
	private protected:
		String^ _animationName;

	public protected:
		Ogre::AnimationState* _native;
		bool _createdByCLR;

		AnimationState(Ogre::AnimationState* obj) : _native(obj)
		{
		}

		AnimationState(intptr_t ptr) : _native((Ogre::AnimationState*)ptr)
		{
		}

		~AnimationState()
		{
			if (_createdByCLR && _native != 0) { delete _native; _native = 0; }
		}

	public:
		void CopyTo(AnimationState^ dest)
		{
			if (_native == NULL) throw gcnew Exception("The underlying native object for the caller is null.");
			if (dest->_native == NULL) throw gcnew ArgumentException("The underlying native object for parameter 'dest' is null.");

			*(dest->_native) = *_native;
		}

	internal:
		property Ogre::AnimationState* UnmanagedPointer
		{
			Ogre::AnimationState* get()
			{
				return _native;
			}
		}
	};

	//################################################################
	//Animation
	//################################################################

	public ref class Animation : public IDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	public:
		ref class NodeTrackList;
		ref class NumericTrackList;
		ref class VertexTrackList;
		ref class TrackHandleList;

		enum class InterpolationMode
		{
			IM_LINEAR = Ogre::Animation::IM_LINEAR,
			IM_SPLINE = Ogre::Animation::IM_SPLINE
		};

		enum class RotationInterpolationMode
		{
			RIM_LINEAR = Ogre::Animation::RIM_LINEAR,
			RIM_SPHERICAL = Ogre::Animation::RIM_SPHERICAL
		};

		INC_DECLARE_STLMAP(NodeTrackList, unsigned short, Mogre::NodeAnimationTrack^, unsigned short, Ogre::NodeAnimationTrack*, public:, private:);
		INC_DECLARE_STLMAP(NumericTrackList, unsigned short, Mogre::NumericAnimationTrack^, unsigned short, Ogre::NumericAnimationTrack*, public:, private:);
		INC_DECLARE_STLMAP(VertexTrackList, unsigned short, Mogre::VertexAnimationTrack^, unsigned short, Ogre::VertexAnimationTrack*, public:, private:);
		INC_DECLARE_STLSET(TrackHandleList, Mogre::ushort, Ogre::ushort, public:, private:);

		//INC_DECLARE_MAP_ITERATOR(NodeTrackIterator, Ogre::Animation::NodeTrackIterator, Mogre::Animation::NodeTrackList, Mogre::NodeAnimationTrack^, Ogre::NodeAnimationTrack*, unsigned short, unsigned short);
		INC_DECLARE_MAP_ITERATOR(NumericTrackIterator, Ogre::Animation::NumericTrackIterator, Mogre::Animation::NumericTrackList, Mogre::NumericAnimationTrack^, Ogre::NumericAnimationTrack*, unsigned short, unsigned short);
		INC_DECLARE_MAP_ITERATOR(VertexTrackIterator, Ogre::Animation::VertexTrackIterator, Mogre::Animation::VertexTrackList, Mogre::VertexAnimationTrack^, Ogre::VertexAnimationTrack*, unsigned short, unsigned short);


	public protected:
		Animation(intptr_t obj) : _native((Ogre::Animation*)obj), _createdByCLR(false)
		{
		}

		Animation(Ogre::Animation* obj) : _native(obj), _createdByCLR(false)
		{
		}

		Ogre::Animation* _native;
		bool _createdByCLR;

	public:
		~Animation();
	protected:
		!Animation();

	public:
		Animation(String^ name, Mogre::Real length);

		property bool IsDisposed
		{
			virtual bool get();
		}

		property Mogre::Animation::InterpolationMode DefaultInterpolationMode
		{
		public:
			static Mogre::Animation::InterpolationMode get();
		public:
			static void set(Mogre::Animation::InterpolationMode im);
		}

		property Mogre::Animation::RotationInterpolationMode DefaultRotationInterpolationMode
		{
		public:
			static Mogre::Animation::RotationInterpolationMode get();
		public:
			static void set(Mogre::Animation::RotationInterpolationMode im);
		}

		property Mogre::Real Length
		{
		public:
			Mogre::Real get();
		}

		property String^ Name
		{
		public:
			String^ get();
		}

		property unsigned short NumNodeTracks
		{
		public:
			unsigned short get();
		}

		property unsigned short NumNumericTracks
		{
		public:
			unsigned short get();
		}

		property unsigned short NumVertexTracks
		{
		public:
			unsigned short get();
		}

		Mogre::NodeAnimationTrack^ CreateNodeTrack();
		Mogre::NodeAnimationTrack^ CreateNodeTrack(Mogre::Node^ node);

		Mogre::NumericAnimationTrack^ CreateNumericTrack(unsigned short handle);

		Mogre::VertexAnimationTrack^ CreateVertexTrack(unsigned short handle, Mogre::VertexAnimationType animType);


		//Mogre::NumericAnimationTrack^ CreateNumericTrack(unsigned short handle, Mogre::AnimableValuePtr^ anim);
		//Mogre::VertexAnimationTrack^ CreateVertexTrack(unsigned short handle, Mogre::VertexData^ data, Mogre::VertexAnimationType animType);

		Mogre::NodeAnimationTrack^ GetNodeTrack(unsigned short handle);

		Mogre::NumericAnimationTrack^ GetNumericTrack(unsigned short handle);

		bool HasNumericTrack(unsigned short handle);

		Mogre::VertexAnimationTrack^ GetVertexTrack(unsigned short handle);

		bool HasVertexTrack(unsigned short handle);

		void DestroyNumericTrack(unsigned short handle);

		void DestroyVertexTrack(unsigned short handle);

		void DestroyAllTracks();

		void DestroyAllNodeTracks();

		void DestroyAllNumericTracks();

		void DestroyAllVertexTracks();

		void Apply(Mogre::Real timePos, Mogre::Real weight, Mogre::Real scale);
		void Apply(Mogre::Real timePos, Mogre::Real weight);
		void Apply(Mogre::Real timePos);

		//void Apply(Mogre::Skeleton^ skeleton, Mogre::Real timePos, Mogre::Real weight, Mogre::Real scale);
		//void Apply(Mogre::Skeleton^ skeleton, Mogre::Real timePos, Mogre::Real weight);
		//void Apply(Mogre::Skeleton^ skeleton, Mogre::Real timePos);

		void Apply(Mogre::Entity^ entity, Mogre::Real timePos, Mogre::Real weight, bool software, bool hardware);

		void SetInterpolationMode(Mogre::Animation::InterpolationMode im);

		Mogre::Animation::InterpolationMode GetInterpolationMode();

		void SetRotationInterpolationMode(Mogre::Animation::RotationInterpolationMode im);

		Mogre::Animation::RotationInterpolationMode GetRotationInterpolationMode();

		//Mogre::Animation::NodeTrackIterator^ GetNodeTrackIterator();
		Mogre::Animation::NumericTrackIterator^ GetNumericTrackIterator();
		Mogre::Animation::VertexTrackIterator^ GetVertexTrackIterator();

		void Optimise(bool discardIdentityNodeTracks);
		void Optimise();

		Mogre::Animation^ Clone(String^ newName);
		Mogre::TimeIndex^ _getTimeIndex(Mogre::Real timePos);

		DEFINE_MANAGED_NATIVE_CONVERSIONS(Animation);

	internal:
		property Ogre::Animation* UnmanagedPointer
		{
			Ogre::Animation* get()
			{
				return _native;
			}
		}
	};
}