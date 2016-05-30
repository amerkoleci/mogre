#include "stdafx.h"
#include "MogreAnimation.h"
#include "MogreNode.h"
#include "MogreEntity.h"

using namespace Mogre;

TimeIndex::TimeIndex(Ogre::Real timePos)
{
	_createdByCLR = true;
	_native = new Ogre::TimeIndex(timePos);
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

TimeIndex::TimeIndex(Ogre::Real timePos, Mogre::uint keyIndex)
{
	_createdByCLR = true;
	_native = new Ogre::TimeIndex(timePos, keyIndex);
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

TimeIndex::~TimeIndex()
{
	this->!TimeIndex();
}

TimeIndex::!TimeIndex()
{
	OnDisposing(this, nullptr);

	if (IsDisposed)
		return;

	if (_createdByCLR && _native)
	{
		delete _native;
		_native = 0;
	}

	OnDisposed(this, nullptr);
}

bool TimeIndex::IsDisposed::get()
{
	return (_native == nullptr);
}

bool TimeIndex::HasKeyIndex::get()
{
	return static_cast<const Ogre::TimeIndex*>(_native)->hasKeyIndex();
}

Ogre::uint TimeIndex::KeyIndex::get()
{
	return static_cast<const Ogre::TimeIndex*>(_native)->getKeyIndex();
}

Ogre::Real TimeIndex::TimePos::get()
{
	return static_cast<const Ogre::TimeIndex*>(_native)->getTimePos();
}

Ogre::TimeIndex* TimeIndex::UnmanagedPointer::get()
{
	return _native;
}

// Keyrame
KeyFrame::KeyFrame(Mogre::AnimationTrack^ parent, Ogre::Real time) 
{
	_createdByCLR = true;
	_native = new Ogre::KeyFrame(GetPointerOrNull(parent), time);
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

KeyFrame::~KeyFrame()
{
	this->!KeyFrame();
}

KeyFrame::!KeyFrame()
{
	OnDisposing(this, nullptr);

	if (IsDisposed)
		return;

	if (_createdByCLR && _native)
	{
		delete _native;
		_native = 0;
	}

	OnDisposed(this, nullptr);
}

bool KeyFrame::IsDisposed::get()
{
	return (_native == nullptr);
}

Mogre::Real KeyFrame::Time::get()
{
	return static_cast<const Ogre::KeyFrame*>(_native)->getTime();
}

Mogre::KeyFrame^ KeyFrame::_clone(Mogre::AnimationTrack^ newParent)
{
	return ObjectTable::GetOrCreateObject<Mogre::KeyFrame^>((intptr_t)
		static_cast<const Ogre::KeyFrame*>(_native)->_clone(GetPointerOrNull(newParent))
		);
}

// NumericKeyFrame
NumericKeyFrame::NumericKeyFrame(Mogre::AnimationTrack^ parent, Mogre::Real time) : KeyFrame((Ogre::KeyFrame*)nullptr)
{
	_createdByCLR = true;
	_native = new Ogre::NumericKeyFrame(GetPointerOrNull(parent), time);
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

Mogre::KeyFrame^ NumericKeyFrame::_clone(Mogre::AnimationTrack^ newParent)
{
	return ObjectTable::GetOrCreateObject<Mogre::KeyFrame^>((intptr_t)
		static_cast<const Ogre::NumericKeyFrame*>(_native)->_clone(GetPointerOrNull(newParent))
		);
}

// TransformKeyFrame
TransformKeyFrame::TransformKeyFrame(Mogre::AnimationTrack^ parent, Mogre::Real time) : KeyFrame((Ogre::KeyFrame*)0)
{
	_createdByCLR = true;
	_native = new Ogre::TransformKeyFrame(GetPointerOrNull(parent), time);
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

Mogre::Quaternion TransformKeyFrame::Rotation::get()
{
	return ToQuaternion(static_cast<const Ogre::TransformKeyFrame*>(_native)->getRotation());
}

void TransformKeyFrame::Rotation::set(Mogre::Quaternion rot)
{
	static_cast<Ogre::TransformKeyFrame*>(_native)->setRotation(FromQuaternion(rot));
}

Mogre::Vector3 TransformKeyFrame::Scale::get()
{
	return ToVector3(static_cast<const Ogre::TransformKeyFrame*>(_native)->getScale());
}

void TransformKeyFrame::Scale::set(Mogre::Vector3 scale)
{
	static_cast<Ogre::TransformKeyFrame*>(_native)->setScale(FromVector3(scale));
}

Mogre::Vector3 TransformKeyFrame::Translate::get()
{
	return ToVector3(static_cast<const Ogre::TransformKeyFrame*>(_native)->getTranslate());
}

void TransformKeyFrame::Translate::set(Mogre::Vector3 trans)
{
	static_cast<Ogre::TransformKeyFrame*>(_native)->setTranslate(FromVector3(trans));
}

Mogre::KeyFrame^ TransformKeyFrame::_clone(Mogre::AnimationTrack^ newParent)
{
	return ObjectTable::GetOrCreateObject<Mogre::KeyFrame^>((intptr_t)
		static_cast<const Ogre::TransformKeyFrame*>(_native)->_clone(GetPointerOrNull(newParent))
		);
}

// AnimationTrack
AnimationTrack::~AnimationTrack()
{
	this->!AnimationTrack();
}

AnimationTrack::!AnimationTrack()
{
	OnDisposing(this, nullptr);

	if (IsDisposed)
		return;

	if (_createdByCLR && _native)
	{
		delete _native;
		_native = 0;
	}

	OnDisposed(this, nullptr);
}

unsigned short AnimationTrack::Handle::get()
{
	return static_cast<const Ogre::AnimationTrack*>(_native)->getHandle();
}

bool AnimationTrack::HasNonZeroKeyFrames::get()
{
	return static_cast<const Ogre::AnimationTrack*>(_native)->hasNonZeroKeyFrames();
}

unsigned short AnimationTrack::NumKeyFrames::get()
{
	return static_cast<const Ogre::AnimationTrack*>(_native)->getNumKeyFrames();
}

Mogre::KeyFrame^ AnimationTrack::GetKeyFrame(unsigned short index)
{
	return ObjectTable::GetOrCreateObject<Mogre::KeyFrame^>((intptr_t)
		static_cast<const Ogre::AnimationTrack*>(_native)->getKeyFrame(index)
		);
}

Mogre::Real AnimationTrack::GetKeyFramesAtTime(Mogre::TimeIndex^ timeIndex, [Out] Mogre::KeyFrame^% keyFrame1, [Out] Mogre::KeyFrame^% keyFrame2, [Out] unsigned short% firstKeyIndex)
{
	Ogre::KeyFrame* out_keyFrame1;
	Ogre::KeyFrame* out_keyFrame2;
	pin_ptr<unsigned short> p_firstKeyIndex = &firstKeyIndex;

	Mogre::Real retres = static_cast<const Ogre::AnimationTrack*>(_native)->getKeyFramesAtTime(*timeIndex->UnmanagedPointer, &out_keyFrame1, &out_keyFrame2, p_firstKeyIndex);
	keyFrame1 = ObjectTable::GetOrCreateObject<Mogre::KeyFrame^>((intptr_t)out_keyFrame1);
	keyFrame2 = ObjectTable::GetOrCreateObject<Mogre::KeyFrame^>((intptr_t)out_keyFrame2);

	return retres;
}

Mogre::Real AnimationTrack::GetKeyFramesAtTime(Mogre::TimeIndex^ timeIndex, [Out] Mogre::KeyFrame^% keyFrame1, [Out] Mogre::KeyFrame^% keyFrame2)
{
	Ogre::KeyFrame* out_keyFrame1;
	Ogre::KeyFrame* out_keyFrame2;

	Mogre::Real retres = static_cast<const Ogre::AnimationTrack*>(_native)->getKeyFramesAtTime(*timeIndex->UnmanagedPointer, &out_keyFrame1, &out_keyFrame2);
	keyFrame1 = ObjectTable::GetOrCreateObject<Mogre::KeyFrame^>((intptr_t)out_keyFrame1);
	keyFrame2 = ObjectTable::GetOrCreateObject<Mogre::KeyFrame^>((intptr_t)out_keyFrame2);

	return retres;
}

Mogre::KeyFrame^ AnimationTrack::CreateKeyFrame(Mogre::Real timePos)
{
	return ObjectTable::GetOrCreateObject<Mogre::KeyFrame^>((intptr_t)
		static_cast<Ogre::AnimationTrack*>(_native)->createKeyFrame(timePos)
		);
}

void AnimationTrack::RemoveKeyFrame(unsigned short index)
{
	static_cast<Ogre::AnimationTrack*>(_native)->removeKeyFrame(index);
}

void AnimationTrack::RemoveAllKeyFrames()
{
	static_cast<Ogre::AnimationTrack*>(_native)->removeAllKeyFrames();
}

void AnimationTrack::GetInterpolatedKeyFrame(Mogre::TimeIndex^ timeIndex, Mogre::KeyFrame^ kf)
{
	static_cast<const Ogre::AnimationTrack*>(_native)->getInterpolatedKeyFrame(*timeIndex->UnmanagedPointer, GetPointerOrNull(kf));
}

void AnimationTrack::Apply(Mogre::TimeIndex^ timeIndex, Mogre::Real weight, Mogre::Real scale)
{
	static_cast<Ogre::AnimationTrack*>(_native)->apply(*timeIndex->UnmanagedPointer, weight, scale);
}

void AnimationTrack::Apply(Mogre::TimeIndex^ timeIndex, Mogre::Real weight)
{
	static_cast<Ogre::AnimationTrack*>(_native)->apply(*timeIndex->UnmanagedPointer, weight);
}

void AnimationTrack::Apply(Mogre::TimeIndex^ timeIndex)
{
	static_cast<Ogre::AnimationTrack*>(_native)->apply(*timeIndex->UnmanagedPointer);
}

void AnimationTrack::Optimise()
{
	static_cast<Ogre::AnimationTrack*>(_native)->optimise();
}

Ogre::AnimationTrack* AnimationTrack::UnmanagedPointer::get()
{
	return _native;
}

// NodeAnimationTrack
NodeAnimationTrack::NodeAnimationTrack(Mogre::Animation^ parent, unsigned short handle) : AnimationTrack((Ogre::AnimationTrack*)0)
{
	_createdByCLR = true;
	_native = new Ogre::NodeAnimationTrack(parent, handle);
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

NodeAnimationTrack::NodeAnimationTrack(Mogre::Animation^ parent, unsigned short handle, Mogre::Node^ targetNode) : AnimationTrack((Ogre::AnimationTrack*)0)
{
	_createdByCLR = true;
	_native = new Ogre::NodeAnimationTrack(parent, handle, GetPointerOrNull(targetNode));
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

Mogre::Node^ NodeAnimationTrack::AssociatedNode::get()
{
	return ObjectTable::GetOrCreateObject<Mogre::Node^>((intptr_t)
		static_cast<const Ogre::NodeAnimationTrack*>(_native)->getAssociatedNode()
		);
}

void NodeAnimationTrack::AssociatedNode::set(Mogre::Node^ node)
{
	static_cast<Ogre::NodeAnimationTrack*>(_native)->setAssociatedNode(GetPointerOrNull(node));
}

bool NodeAnimationTrack::HasNonZeroKeyFrames::get()
{
	return static_cast<const Ogre::NodeAnimationTrack*>(_native)->hasNonZeroKeyFrames();
}

bool NodeAnimationTrack::UseShortestRotationPath::get()
{
	return static_cast<const Ogre::NodeAnimationTrack*>(_native)->getUseShortestRotationPath();
}

void NodeAnimationTrack::UseShortestRotationPath::set(bool useShortestPath)
{
	static_cast<Ogre::NodeAnimationTrack*>(_native)->setUseShortestRotationPath(useShortestPath);
}

Mogre::TransformKeyFrame^ NodeAnimationTrack::CreateNodeKeyFrame(Mogre::Real timePos)
{
	return static_cast<Ogre::NodeAnimationTrack*>(_native)->createNodeKeyFrame(timePos);
}

void NodeAnimationTrack::ApplyToNode(Mogre::Node^ node, Mogre::TimeIndex^ timeIndex, Mogre::Real weight, Mogre::Real scale)
{
	static_cast<Ogre::NodeAnimationTrack*>(_native)->applyToNode(GetPointerOrNull(node), *timeIndex->UnmanagedPointer, weight, scale);
}
void NodeAnimationTrack::ApplyToNode(Mogre::Node^ node, Mogre::TimeIndex^ timeIndex, Mogre::Real weight)
{
	static_cast<Ogre::NodeAnimationTrack*>(_native)->applyToNode(GetPointerOrNull(node), *timeIndex->UnmanagedPointer, weight);
}
void NodeAnimationTrack::ApplyToNode(Mogre::Node^ node, Mogre::TimeIndex^ timeIndex)
{
	static_cast<Ogre::NodeAnimationTrack*>(_native)->applyToNode(GetPointerOrNull(node), *timeIndex->UnmanagedPointer);
}

void NodeAnimationTrack::GetInterpolatedKeyFrame(Mogre::TimeIndex^ timeIndex, Mogre::KeyFrame^ kf)
{
	static_cast<const Ogre::NodeAnimationTrack*>(_native)->getInterpolatedKeyFrame(*timeIndex->UnmanagedPointer, GetPointerOrNull(kf));
}

void NodeAnimationTrack::Apply(Mogre::TimeIndex^ timeIndex, Mogre::Real weight, Mogre::Real scale)
{
	static_cast<Ogre::NodeAnimationTrack*>(_native)->apply(*timeIndex->UnmanagedPointer, weight, scale);
}

void NodeAnimationTrack::Apply(Mogre::TimeIndex^ timeIndex, Mogre::Real weight)
{
	static_cast<Ogre::NodeAnimationTrack*>(_native)->apply(*timeIndex->UnmanagedPointer, weight);
}

void NodeAnimationTrack::Apply(Mogre::TimeIndex^ timeIndex)
{
	static_cast<Ogre::NodeAnimationTrack*>(_native)->apply(*timeIndex->UnmanagedPointer);
}

Mogre::TransformKeyFrame^ NodeAnimationTrack::GetNodeKeyFrame(unsigned short index)
{
	return static_cast<const Ogre::NodeAnimationTrack*>(_native)->getNodeKeyFrame(index);
}

void NodeAnimationTrack::Optimise()
{
	static_cast<Ogre::NodeAnimationTrack*>(_native)->optimise();
}

// ------------------- AnimationState -----
AnimationState::AnimationState(String^ animName, Mogre::AnimationStateSet^ parent, Mogre::Real timePos, Mogre::Real length, Mogre::Real weight, bool enabled) 
{
	_createdByCLR = true;
	DECLARE_NATIVE_STRING(o_animName, animName);
	_native = new Ogre::AnimationState(o_animName, GetPointerOrNull(parent), timePos, length, weight, enabled);
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

AnimationState::AnimationState(String^ animName, Mogre::AnimationStateSet^ parent, Mogre::Real timePos, Mogre::Real length, Mogre::Real weight)
{
	_createdByCLR = true;
	DECLARE_NATIVE_STRING(o_animName, animName);
	_native = new Ogre::AnimationState(o_animName, GetPointerOrNull(parent), timePos, length, weight);
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

AnimationState::AnimationState(String^ animName, Mogre::AnimationStateSet^ parent, Mogre::Real timePos, Mogre::Real length)
{
	_createdByCLR = true;
	DECLARE_NATIVE_STRING(o_animName, animName);
	_native = new Ogre::AnimationState(o_animName, GetPointerOrNull(parent), timePos, length);
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

AnimationState::AnimationState(Mogre::AnimationStateSet^ parent, Mogre::AnimationState^ rhs) 
{
	_createdByCLR = true;
	_native = new Ogre::AnimationState(GetPointerOrNull(parent), *rhs->UnmanagedPointer);
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

String^ AnimationState::AnimationName::get()
{
	return (CLR_NULL == _animationName) ? (_animationName = TO_CLR_STRING(static_cast<const Ogre::AnimationState*>(_native)->getAnimationName())) : _animationName;
}

AnimationState::~AnimationState()
{
	this->!AnimationState();
}

AnimationState::!AnimationState()
{
	OnDisposing(this, nullptr);

	if (IsDisposed)
		return;

	if (_createdByCLR && _native != 0) 
	{
		delete _native; _native = 0; 
	}

	OnDisposed(this, nullptr);
}

bool AnimationState::Enabled::get()
{
	return static_cast<const Ogre::AnimationState*>(_native)->getEnabled();
}
void AnimationState::Enabled::set(bool enabled)
{
	static_cast<Ogre::AnimationState*>(_native)->setEnabled(enabled);
}

bool AnimationState::HasEnded::get()
{
	return static_cast<const Ogre::AnimationState*>(_native)->hasEnded();
}

Mogre::Real AnimationState::Length::get()
{
	return static_cast<const Ogre::AnimationState*>(_native)->getLength();
}
void AnimationState::Length::set(Mogre::Real len)
{
	static_cast<Ogre::AnimationState*>(_native)->setLength(len);
}

bool AnimationState::Loop::get()
{
	return static_cast<const Ogre::AnimationState*>(_native)->getLoop();
}
void AnimationState::Loop::set(bool loop)
{
	static_cast<Ogre::AnimationState*>(_native)->setLoop(loop);
}

Mogre::AnimationStateSet^ AnimationState::Parent::get()
{
	return ObjectTable::GetOrCreateObject<Mogre::AnimationStateSet^>((intptr_t)
		static_cast<const Ogre::AnimationState*>(_native)->getParent()
		);
}

Mogre::Real AnimationState::TimePosition::get()
{
	return static_cast<const Ogre::AnimationState*>(_native)->getTimePosition();
}
void AnimationState::TimePosition::set(Mogre::Real timePos)
{
	static_cast<Ogre::AnimationState*>(_native)->setTimePosition(timePos);
}

Mogre::Real AnimationState::Weight::get()
{
	return static_cast<const Ogre::AnimationState*>(_native)->getWeight();
}
void AnimationState::Weight::set(Mogre::Real weight)
{
	static_cast<Ogre::AnimationState*>(_native)->setWeight(weight);
}

void AnimationState::AddTime(Mogre::Real offset)
{
	static_cast<Ogre::AnimationState*>(_native)->addTime(offset);
}

bool AnimationState::Equals(Object^ obj)
{
	AnimationState^ clr = dynamic_cast<AnimationState^>(obj);
	if (clr == CLR_NULL)
	{
		return false;
	}

	if (_native == NULL) throw gcnew Exception("The underlying native object for the caller is null.");
	if (clr->_native == NULL) throw gcnew ArgumentException("The underlying native object for parameter 'obj' is null.");

	return *(static_cast<Ogre::AnimationState*>(_native)) == *(static_cast<Ogre::AnimationState*>(clr->_native));
}

bool AnimationState::Equals(AnimationState^ obj)
{
	if (obj == CLR_NULL)
	{
		return false;
	}

	if (_native == NULL) throw gcnew Exception("The underlying native object for the caller is null.");
	if (obj->_native == NULL) throw gcnew ArgumentException("The underlying native object for parameter 'obj' is null.");

	return *(static_cast<Ogre::AnimationState*>(_native)) == *(static_cast<Ogre::AnimationState*>(obj->_native));
}

bool AnimationState::operator ==(AnimationState^ obj1, AnimationState^ obj2)
{
	if ((Object^)obj1 == (Object^)obj2) return true;
	if ((Object^)obj1 == nullptr || (Object^)obj2 == nullptr) return false;

	return obj1->Equals(obj2);
}

bool AnimationState::operator !=(AnimationState^ obj1, AnimationState^ obj2)
{
	return !(obj1 == obj2);
}


void AnimationState::CopyStateFrom(Mogre::AnimationState^ animState)
{
	static_cast<Ogre::AnimationState*>(_native)->copyStateFrom(animState);
}

CPP_DECLARE_STLMAP(, AnimationStateMap, String^, Mogre::AnimationState^, Ogre::String, Ogre::AnimationState*);
CPP_DECLARE_MAP_ITERATOR(, AnimationStateIterator, Ogre::AnimationStateIterator, Mogre::AnimationStateMap, Mogre::AnimationState^, Ogre::AnimationState*, String^, Ogre::String, );
CPP_DECLARE_STLLIST(, EnabledAnimationStateList, Mogre::AnimationState^, Ogre::AnimationState*);
CPP_DECLARE_ITERATOR(, ConstEnabledAnimationStateIterator, Ogre::ConstEnabledAnimationStateIterator, Mogre::EnabledAnimationStateList, Mogre::AnimationState^, Ogre::AnimationState*, const);


// AnimationStateSet
AnimationStateSet::~AnimationStateSet()
{
	this->!AnimationStateSet();
}

AnimationStateSet::!AnimationStateSet()
{
	OnDisposing(this, nullptr);

	if (IsDisposed)
		return;

	if (_createdByCLR && _native != 0)
	{
		delete _native; _native = 0;
	}

	OnDisposed(this, nullptr);
}

AnimationStateSet::AnimationStateSet()
{
	_createdByCLR = true;
	_native = new Ogre::AnimationStateSet();
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

AnimationStateSet::AnimationStateSet(Mogre::AnimationStateSet^ rhs)
{
	_createdByCLR = true;
	_native = new Ogre::AnimationStateSet(*rhs->UnmanagedPointer);
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

unsigned long AnimationStateSet::DirtyFrameNumber::get()
{
	return static_cast<const Ogre::AnimationStateSet*>(_native)->getDirtyFrameNumber();
}

bool AnimationStateSet::HasEnabledAnimationState::get()
{
	return static_cast<const Ogre::AnimationStateSet*>(_native)->hasEnabledAnimationState();
}

Mogre::AnimationState^ AnimationStateSet::CreateAnimationState(String^ animName, Mogre::Real timePos, Mogre::Real length, Mogre::Real weight, bool enabled)
{
	DECLARE_NATIVE_STRING(o_animName, animName);

	return static_cast<Ogre::AnimationStateSet*>(_native)->createAnimationState(o_animName, timePos, length, weight, enabled);
}

Mogre::AnimationState^ AnimationStateSet::CreateAnimationState(String^ animName, Mogre::Real timePos, Mogre::Real length, Mogre::Real weight)
{
	DECLARE_NATIVE_STRING(o_animName, animName);

	return static_cast<Ogre::AnimationStateSet*>(_native)->createAnimationState(o_animName, timePos, length, weight);
}

Mogre::AnimationState^ AnimationStateSet::CreateAnimationState(String^ animName, Mogre::Real timePos, Mogre::Real length)
{
	DECLARE_NATIVE_STRING(o_animName, animName);

	return static_cast<Ogre::AnimationStateSet*>(_native)->createAnimationState(o_animName, timePos, length);
}

Mogre::AnimationState^ AnimationStateSet::GetAnimationState(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return static_cast<const Ogre::AnimationStateSet*>(_native)->getAnimationState(o_name);
}

bool AnimationStateSet::HasAnimationState(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return static_cast<const Ogre::AnimationStateSet*>(_native)->hasAnimationState(o_name);
}

void AnimationStateSet::RemoveAnimationState(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	static_cast<Ogre::AnimationStateSet*>(_native)->removeAnimationState(o_name);
}

void AnimationStateSet::RemoveAllAnimationStates()
{
	static_cast<Ogre::AnimationStateSet*>(_native)->removeAllAnimationStates();
}

Mogre::AnimationStateIterator^ AnimationStateSet::GetAnimationStateIterator()
{
	return static_cast<Ogre::AnimationStateSet*>(_native)->getAnimationStateIterator();
}

void AnimationStateSet::CopyMatchingState(Mogre::AnimationStateSet^ target)
{
	static_cast<const Ogre::AnimationStateSet*>(_native)->copyMatchingState(GetPointerOrNull(target));
}

void AnimationStateSet::_notifyDirty()
{
	static_cast<Ogre::AnimationStateSet*>(_native)->_notifyDirty();
}

void AnimationStateSet::_notifyAnimationStateEnabled(Mogre::AnimationState^ target, bool enabled)
{
	static_cast<Ogre::AnimationStateSet*>(_native)->_notifyAnimationStateEnabled(target, enabled);
}

Mogre::ConstEnabledAnimationStateIterator^ AnimationStateSet::GetEnabledAnimationStateIterator()
{
	return static_cast<const Ogre::AnimationStateSet*>(_native)->getEnabledAnimationStateIterator();
}

// Animation
Animation::~Animation()
{
	this->!Animation();
}

Animation::!Animation()
{
	OnDisposing(this, nullptr);

	if (IsDisposed)
		return;

	if (_createdByCLR && _native)
	{
		delete _native;
		_native = 0;
	}

	OnDisposed(this, nullptr);
}

Animation::Animation(String^ name, Mogre::Real length) 
{
	_createdByCLR = true;
	DECLARE_NATIVE_STRING(o_name, name);
	_native = new Ogre::Animation(o_name, length);
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

bool Animation::IsDisposed::get()
{
	return (_native == nullptr);
}

Mogre::Animation::InterpolationMode Animation::DefaultInterpolationMode::get()
{
	return (Mogre::Animation::InterpolationMode)Ogre::Animation::getDefaultInterpolationMode();
}

void Animation::DefaultInterpolationMode::set(Mogre::Animation::InterpolationMode im)
{
	Ogre::Animation::setDefaultInterpolationMode((Ogre::Animation::InterpolationMode)im);
}

Mogre::Animation::RotationInterpolationMode Animation::DefaultRotationInterpolationMode::get()
{
	return (Mogre::Animation::RotationInterpolationMode)Ogre::Animation::getDefaultRotationInterpolationMode();
}

void Animation::DefaultRotationInterpolationMode::set(Mogre::Animation::RotationInterpolationMode im)
{
	Ogre::Animation::setDefaultRotationInterpolationMode((Ogre::Animation::RotationInterpolationMode)im);
}

Mogre::Real Animation::Length::get()
{
	return static_cast<const Ogre::Animation*>(_native)->getLength();
}

String^ Animation::Name::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::Animation*>(_native)->getName());
}

unsigned short Animation::NumNodeTracks::get()
{
	return static_cast<const Ogre::Animation*>(_native)->getNumNodeTracks();
}

unsigned short Animation::NumNumericTracks::get()
{
	return static_cast<const Ogre::Animation*>(_native)->getNumNumericTracks();
}

unsigned short Animation::NumVertexTracks::get()
{
	return static_cast<const Ogre::Animation*>(_native)->getNumVertexTracks();
}

Mogre::NodeAnimationTrack^ Animation::CreateNodeTrack()
{
	return static_cast<Ogre::Animation*>(_native)->createNodeTrack();
}

Mogre::NodeAnimationTrack^ Animation::CreateNodeTrack(Mogre::Node^ node)
{
	return static_cast<Ogre::Animation*>(_native)->createNodeTrack(GetPointerOrNull(node));
}

Mogre::NumericAnimationTrack^ Animation::CreateNumericTrack(unsigned short handle)
{
	return static_cast<Ogre::Animation*>(_native)->createNumericTrack(handle);
}

Mogre::VertexAnimationTrack^ Animation::CreateVertexTrack(unsigned short handle, Mogre::VertexAnimationType animType)
{
	return static_cast<Ogre::Animation*>(_native)->createVertexTrack(handle, (Ogre::VertexAnimationType)animType);
}

Mogre::NodeAnimationTrack^ Animation::GetNodeTrack(unsigned short handle)
{
	return static_cast<const Ogre::Animation*>(_native)->getNodeTrack(handle);
}

Mogre::NumericAnimationTrack^ Animation::GetNumericTrack(unsigned short handle)
{
	return static_cast<const Ogre::Animation*>(_native)->getNumericTrack(handle);
}

bool Animation::HasNumericTrack(unsigned short handle)
{
	return static_cast<const Ogre::Animation*>(_native)->hasNumericTrack(handle);
}

Mogre::VertexAnimationTrack^ Animation::GetVertexTrack(unsigned short handle)
{
	return static_cast<const Ogre::Animation*>(_native)->getVertexTrack(handle);
}

bool Animation::HasVertexTrack(unsigned short handle)
{
	return static_cast<const Ogre::Animation*>(_native)->hasVertexTrack(handle);
}

void Animation::DestroyNumericTrack(unsigned short handle)
{
	static_cast<Ogre::Animation*>(_native)->destroyNumericTrack(handle);
}

void Animation::DestroyVertexTrack(unsigned short handle)
{
	static_cast<Ogre::Animation*>(_native)->destroyVertexTrack(handle);
}

void Animation::DestroyAllTracks()
{
	static_cast<Ogre::Animation*>(_native)->destroyAllTracks();
}

void Animation::DestroyAllNodeTracks()
{
	static_cast<Ogre::Animation*>(_native)->destroyAllNodeTracks();
}

void Animation::DestroyAllNumericTracks()
{
	static_cast<Ogre::Animation*>(_native)->destroyAllNumericTracks();
}

void Animation::DestroyAllVertexTracks()
{
	static_cast<Ogre::Animation*>(_native)->destroyAllVertexTracks();
}

void Animation::Apply(Mogre::Real timePos, Mogre::Real weight, Mogre::Real scale)
{
	static_cast<Ogre::Animation*>(_native)->apply(timePos, weight, scale);
}
void Animation::Apply(Mogre::Real timePos, Mogre::Real weight)
{
	static_cast<Ogre::Animation*>(_native)->apply(timePos, weight);
}
void Animation::Apply(Mogre::Real timePos)
{
	static_cast<Ogre::Animation*>(_native)->apply(timePos);
}

//void Animation::Apply(Mogre::Skeleton^ skeleton, Mogre::Real timePos, Mogre::Real weight, Mogre::Real scale)
//{
//	static_cast<Ogre::Animation*>(_native)->apply(skeleton, timePos, weight, scale);
//}
//
//void Animation::Apply(Mogre::Skeleton^ skeleton, Mogre::Real timePos, Mogre::Real weight)
//{
//	static_cast<Ogre::Animation*>(_native)->apply(skeleton, timePos, weight);
//}
//
//void Animation::Apply(Mogre::Skeleton^ skeleton, Mogre::Real timePos)
//{
//	static_cast<Ogre::Animation*>(_native)->apply(skeleton, timePos);
//}

void Animation::Apply(Mogre::Entity^ entity, Mogre::Real timePos, Mogre::Real weight, bool software, bool hardware)
{
	static_cast<Ogre::Animation*>(_native)->apply(GetPointerOrNull(entity), timePos, weight, software, hardware);
}

void Animation::SetInterpolationMode(Mogre::Animation::InterpolationMode im)
{
	static_cast<Ogre::Animation*>(_native)->setInterpolationMode((Ogre::Animation::InterpolationMode)im);
}

Mogre::Animation::InterpolationMode Animation::GetInterpolationMode()
{
	return (Mogre::Animation::InterpolationMode)static_cast<const Ogre::Animation*>(_native)->getInterpolationMode();
}

void Animation::SetRotationInterpolationMode(Mogre::Animation::RotationInterpolationMode im)
{
	static_cast<Ogre::Animation*>(_native)->setRotationInterpolationMode((Ogre::Animation::RotationInterpolationMode)im);
}

Mogre::Animation::RotationInterpolationMode Animation::GetRotationInterpolationMode()
{
	return (Mogre::Animation::RotationInterpolationMode)static_cast<const Ogre::Animation*>(_native)->getRotationInterpolationMode();
}

//Mogre::Animation::NodeTrackIterator^ Animation::GetNodeTrackIterator()
//{
//	return static_cast<const Ogre::Animation*>(_native)->getNodeTrackIterator();
//}

Mogre::Animation::NumericTrackIterator^ Animation::GetNumericTrackIterator()
{
	return static_cast<const Ogre::Animation*>(_native)->getNumericTrackIterator();
}

Mogre::Animation::VertexTrackIterator^ Animation::GetVertexTrackIterator()
{
	return static_cast<const Ogre::Animation*>(_native)->getVertexTrackIterator();
}

void Animation::Optimise(bool discardIdentityNodeTracks)
{
	static_cast<Ogre::Animation*>(_native)->optimise(discardIdentityNodeTracks);
}

void Animation::Optimise()
{
	static_cast<Ogre::Animation*>(_native)->optimise();
}

Mogre::Animation^ Animation::Clone(String^ newName)
{
	DECLARE_NATIVE_STRING(o_newName, newName);

	return static_cast<const Ogre::Animation*>(_native)->clone(o_newName);
}

//void Animation::_keyFrameListChanged()
//{
//	static_cast<Ogre::Animation*>(_native)->_keyFrameListChanged();
//}

Mogre::TimeIndex^ Animation::_getTimeIndex(Mogre::Real timePos)
{
	return ObjectTable::GetOrCreateObject<Mogre::TimeIndex^>((intptr_t)
		&static_cast<const Ogre::Animation*>(_native)->_getTimeIndex(timePos)
		);
}

CPP_DECLARE_STLMAP(Animation::, NodeTrackList, unsigned short, Mogre::NodeAnimationTrack^, unsigned short, Ogre::NodeAnimationTrack*);
CPP_DECLARE_STLMAP(Animation::, NumericTrackList, unsigned short, Mogre::NumericAnimationTrack^, unsigned short, Ogre::NumericAnimationTrack*);
CPP_DECLARE_STLMAP(Animation::, VertexTrackList, unsigned short, Mogre::VertexAnimationTrack^, unsigned short, Ogre::VertexAnimationTrack*);
CPP_DECLARE_STLSET(Animation::, TrackHandleList, Mogre::ushort, Ogre::ushort);

//CPP_DECLARE_MAP_ITERATOR(Animation::, NodeTrackIterator, Ogre::Animation::NodeTrackIterator, Mogre::Animation::NodeTrackList, Mogre::NodeAnimationTrack^, Ogre::NodeAnimationTrack*, unsigned short, unsigned short, );
CPP_DECLARE_MAP_ITERATOR(Animation::, NumericTrackIterator, Ogre::Animation::NumericTrackIterator, Mogre::Animation::NumericTrackList, Mogre::NumericAnimationTrack^, Ogre::NumericAnimationTrack*, unsigned short, unsigned short, );
CPP_DECLARE_MAP_ITERATOR(Animation::, VertexTrackIterator, Ogre::Animation::VertexTrackIterator, Mogre::Animation::VertexTrackList, Mogre::VertexAnimationTrack^, Ogre::VertexAnimationTrack*, unsigned short, unsigned short, );
