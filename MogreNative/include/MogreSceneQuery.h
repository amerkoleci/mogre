#pragma once

#include "OgreSceneQuery.h"
#include "MogreCommon.h"
#include "MogreRenderOperation.h"
#include "Marshalling.h"
#include "STLContainerWrappers.h"
#include "IteratorWrapper.h"

namespace Mogre
{
	ref class MovableObject;
	ref class RaySceneQueryResult;
	ref class SceneQueryMovableIntersectionList;
	ref class SceneQueryMovableWorldFragmentIntersectionList;
	ref class SceneQueryResultMovableList;
	ref class SceneQueryResultWorldFragmentList;

	public ref class SceneQuery : IMogreDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	public:
		enum class WorldFragmentType
		{
			WFT_NONE = Ogre::SceneQuery::WFT_NONE,
			WFT_PLANE_BOUNDED_REGION = Ogre::SceneQuery::WFT_PLANE_BOUNDED_REGION,
			WFT_SINGLE_INTERSECTION = Ogre::SceneQuery::WFT_SINGLE_INTERSECTION,
			WFT_CUSTOM_GEOMETRY = Ogre::SceneQuery::WFT_CUSTOM_GEOMETRY,
			WFT_RENDER_OPERATION = Ogre::SceneQuery::WFT_RENDER_OPERATION
		};

		ref class WorldFragment
		{
		public:
			//INC_DECLARE_STLLIST(STLList_Plane, Mogre::Plane, Ogre::Plane, public:, private:);

		private protected:
			//STLList_Plane^ _planes;

		public protected:
			WorldFragment(Ogre::SceneQuery::WorldFragment* obj) : _native(obj), _createdByCLR(false)
			{
			}

			~WorldFragment()
			{
				this->!WorldFragment();
			}

			!WorldFragment()
			{
				if (_createdByCLR &&_native)
				{
					delete _native;
					_native = 0;
				}
			}

			Ogre::SceneQuery::WorldFragment* _native;
			bool _createdByCLR;


		public:
			WorldFragment();

			property Mogre::SceneQuery::WorldFragmentType fragmentType
			{
			public:
				Mogre::SceneQuery::WorldFragmentType get();
			public:
				void set(Mogre::SceneQuery::WorldFragmentType value);
			}

			property Mogre::Vector3 singleIntersection
			{
			public:
				Mogre::Vector3 get();
			public:
				void set(Mogre::Vector3 value);
			}

			/*property STLList_Plane^ planes
			{
			public:
				STLList_Plane^ get();
			}*/

			property void* geometry
			{
			public:
				void* get();
			public:
				void set(void* value);
			}

			property Mogre::RenderOperation^ renderOp
			{
			public:
				Mogre::RenderOperation^ get();
			public:
				void set(Mogre::RenderOperation^ value);
			}

			DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_PLAINWRAPPER(SceneQuery::WorldFragment);
		};

	internal:
		Ogre::SceneQuery* _native;
		bool _createdByCLR;

	public protected:
		SceneQuery(intptr_t ptr) : _native((Ogre::SceneQuery*)ptr)
		{

		}

		SceneQuery(Ogre::SceneQuery* obj) : _native(obj)
		{

		}

	public:
		~SceneQuery();
	protected:
		!SceneQuery();

	public:
		property bool IsDisposed
		{
			virtual bool get()
			{
				return _native == nullptr;
			}
		}

		property Ogre::uint32 QueryMask
		{
		public:
			Ogre::uint32 get();
		public:
			void set(Ogre::uint32 mask);
		}

		void SetWorldFragmentType(Mogre::SceneQuery::WorldFragmentType wft);

		Mogre::SceneQuery::WorldFragmentType GetWorldFragmentType();

		DEFINE_MANAGED_NATIVE_CONVERSIONS(SceneQuery);

	internal:
		property Ogre::SceneQuery* UnmanagedPointer
		{
			Ogre::SceneQuery* get() { return _native; }
		}
	};

	//################################################################
	//ISceneQueryListener
	//################################################################

	public interface class ISceneQueryListener
	{
		virtual Ogre::SceneQueryListener* _GetNativePtr();

	public:
		virtual bool QueryResult(Mogre::MovableObject^ object);

		virtual bool QueryResult(Mogre::SceneQuery::WorldFragment^ fragment);
	};

	//################################################################
	//ISceneQueryListener
	//################################################################

	public ref class SceneQueryListener abstract : public IMogreDisposable, public ISceneQueryListener
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	internal:
		Ogre::SceneQueryListener* _native;
		bool _createdByCLR;

	public protected:
		SceneQueryListener(intptr_t ptr) : _native((Ogre::SceneQueryListener*)ptr)
		{
		}

		SceneQueryListener(Ogre::SceneQueryListener* obj) : _native(obj)
		{
		}

		virtual Ogre::SceneQueryListener* _ISceneQueryListener_GetNativePtr() = ISceneQueryListener::_GetNativePtr;

	public:
		~SceneQueryListener();
	protected:
		!SceneQueryListener();

	public:
		SceneQueryListener();

		virtual bool QueryResult(Mogre::MovableObject^ object) abstract;
		virtual bool QueryResult(Mogre::SceneQuery::WorldFragment^ fragment) abstract;

		property bool IsDisposed
		{
			virtual bool get()
			{
				return _native == nullptr;
			}
		}
	};


	//################################################################
	//SceneQueryResult
	//################################################################

	public ref class SceneQueryResult
	{
	private protected:
		//Cached fields
		Mogre::SceneQueryResultMovableList^ _movables;
		Mogre::SceneQueryResultWorldFragmentList^ _worldFragments;

	public protected:
		SceneQueryResult(Ogre::SceneQueryResult* obj) : _native(obj), _createdByCLR(false)
		{
		}

		SceneQueryResult(intptr_t ptr) : _native((Ogre::SceneQueryResult*)ptr), _createdByCLR(false)
		{
		}

		~SceneQueryResult()
		{
			this->!SceneQueryResult();
		}

		!SceneQueryResult()
		{
			if (_createdByCLR &&_native)
			{
				delete _native;
				_native = 0;
			}
		}

		Ogre::SceneQueryResult* _native;
		bool _createdByCLR;
		
	public:
		SceneQueryResult();

		property Mogre::SceneQueryResultMovableList^ movables
		{
		public:
			Mogre::SceneQueryResultMovableList^ get();
		}

		property Mogre::SceneQueryResultWorldFragmentList^ worldFragments
		{
		public:
			Mogre::SceneQueryResultWorldFragmentList^ get();
		}

		DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_PLAINWRAPPER(SceneQueryResult);
	};

	//################################################################
	//RegionSceneQuery
	//################################################################

	public ref class RegionSceneQuery : public SceneQuery, public ISceneQueryListener
	{
	public protected:
		RegionSceneQuery(Ogre::RegionSceneQuery* obj) : SceneQuery(obj)
		{
		}

		RegionSceneQuery(intptr_t ptr) : SceneQuery(ptr)
		{
		}

		virtual Ogre::SceneQueryListener* _ISceneQueryListener_GetNativePtr() = ISceneQueryListener::_GetNativePtr;


	public:
		property Mogre::SceneQueryResult^ LastResults
		{
		public:
			Mogre::SceneQueryResult^ get();
		}

		Mogre::SceneQueryResult^ Execute();

		void Execute(Mogre::ISceneQueryListener^ listener);

		void ClearResults();

		virtual bool QueryResult(Mogre::MovableObject^ first);

		virtual bool QueryResult(Mogre::SceneQuery::WorldFragment^ fragment);

		//------------------------------------------------------------
		// Implementation for ISceneQueryListener
		//------------------------------------------------------------

		DEFINE_MANAGED_NATIVE_CONVERSIONS(RegionSceneQuery);
	};

	//################################################################
	//AxisAlignedBoxSceneQuery
	//################################################################

	public ref class AxisAlignedBoxSceneQuery : public RegionSceneQuery
	{
		//Internal Declarations
	public protected:
		AxisAlignedBoxSceneQuery(Ogre::AxisAlignedBoxSceneQuery* obj) : RegionSceneQuery(obj)
		{
		}

		AxisAlignedBoxSceneQuery(intptr_t ptr) : RegionSceneQuery(ptr)
		{
		}

	public:
		property Mogre::AxisAlignedBox^ Box
		{
		public:
			Mogre::AxisAlignedBox^ get();
		public:
			void set(Mogre::AxisAlignedBox^ box);
		}

		DEFINE_MANAGED_NATIVE_CONVERSIONS(AxisAlignedBoxSceneQuery);
	};

	//################################################################
	//SphereSceneQuery
	//################################################################

	public ref class SphereSceneQuery : public RegionSceneQuery
	{
	public protected:
		SphereSceneQuery(Ogre::SphereSceneQuery* obj) : RegionSceneQuery(obj)
		{
		}

		SphereSceneQuery(intptr_t ptr) : RegionSceneQuery(ptr)
		{
		}

	public:
		property Mogre::Sphere Sphere
		{
		public:
			Mogre::Sphere get();
		public:
			void set(Mogre::Sphere sphere);
		}

		DEFINE_MANAGED_NATIVE_CONVERSIONS(SphereSceneQuery);
	};

	//################################################################
	//PlaneBoundedVolumeListSceneQuery
	//################################################################

	public ref class PlaneBoundedVolumeListSceneQuery : public RegionSceneQuery
	{
	public protected:
		PlaneBoundedVolumeListSceneQuery(Ogre::PlaneBoundedVolumeListSceneQuery* obj) : RegionSceneQuery(obj)
		{
		}

		PlaneBoundedVolumeListSceneQuery(intptr_t ptr) : RegionSceneQuery(ptr)
		{
		}

	public:
		//void SetVolumes(Mogre::Const_PlaneBoundedVolumeList^ volumes);
		//Mogre::Const_PlaneBoundedVolumeList^ GetVolumes();

		DEFINE_MANAGED_NATIVE_CONVERSIONS(PlaneBoundedVolumeListSceneQuery);
	};

	//################################################################
	//IRaySceneQueryListener
	//################################################################

	public interface class IRaySceneQueryListener
	{
		virtual Ogre::RaySceneQueryListener* _GetNativePtr();

	public:
		virtual bool QueryResult(Mogre::MovableObject^ obj, Mogre::Real distance);
		virtual bool QueryResult(Mogre::SceneQuery::WorldFragment^ fragment, Mogre::Real distance);

	};

	//################################################################
	//IRaySceneQueryListener
	//################################################################

	public ref class RaySceneQueryListener abstract : public IMogreDisposable, public IRaySceneQueryListener
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	internal:
		Ogre::RaySceneQueryListener* _native;
		bool _createdByCLR;

	public protected:
		RaySceneQueryListener(intptr_t ptr) : _native((Ogre::RaySceneQueryListener*)ptr)
		{
		}

		RaySceneQueryListener(Ogre::RaySceneQueryListener* obj) : _native(obj)
		{
		}

		virtual Ogre::RaySceneQueryListener* _IRaySceneQueryListener_GetNativePtr() = IRaySceneQueryListener::_GetNativePtr;

	public:
		~RaySceneQueryListener();
	protected:
		!RaySceneQueryListener();

	public:
		RaySceneQueryListener();

		virtual bool QueryResult(Mogre::MovableObject^ obj, Mogre::Real distance) abstract;
		virtual bool QueryResult(Mogre::SceneQuery::WorldFragment^ fragment, Mogre::Real distance) abstract;

		property bool IsDisposed
		{
			virtual bool get()
			{
				return _native == nullptr;
			}
		}
	};


	//################################################################
	//RaySceneQueryResultEntry
	//################################################################

	public ref class RaySceneQueryResultEntry
	{
		//Internal Declarations
	public protected:
		RaySceneQueryResultEntry(Ogre::RaySceneQueryResultEntry* obj) : _native(obj), _createdByCLR(false)
		{
		}

		RaySceneQueryResultEntry(intptr_t ptr) : _native((Ogre::RaySceneQueryResultEntry*)ptr), _createdByCLR(false)
		{

		}

		~RaySceneQueryResultEntry()
		{
			this->!RaySceneQueryResultEntry();
		}
		!RaySceneQueryResultEntry()
		{
			if (_createdByCLR &&_native)
			{
				delete _native;
				_native = 0;
			}
		}

		Ogre::RaySceneQueryResultEntry* _native;
		bool _createdByCLR;


		//Public Declarations
	public:
		RaySceneQueryResultEntry();

		property Ogre::Real distance
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real value);
		}

		property Mogre::MovableObject^ movable
		{
		public:
			Mogre::MovableObject^ get();
		public:
			void set(Mogre::MovableObject^ value);
		}

		property Mogre::SceneQuery::WorldFragment^ worldFragment
		{
		public:
			Mogre::SceneQuery::WorldFragment^ get();
		public:
			void set(Mogre::SceneQuery::WorldFragment^ value);
		}

		DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_PLAINWRAPPER(RaySceneQueryResultEntry);
	};

	//################################################################
	//RaySceneQuery
	//################################################################

	public ref class RaySceneQuery : public SceneQuery, public IRaySceneQueryListener
	{
	public protected:
		RaySceneQuery(Ogre::RaySceneQuery* obj) : SceneQuery(obj)
		{
		}

		RaySceneQuery(intptr_t ptr) : SceneQuery(ptr)
		{

		}

		virtual Ogre::RaySceneQueryListener* _IRaySceneQueryListener_GetNativePtr() = IRaySceneQueryListener::_GetNativePtr;


	public:
		property Mogre::ushort MaxResults
		{
		public:
			Mogre::ushort get();
		}

		property Mogre::Ray Ray
		{
		public:
			Mogre::Ray get();
		public:
			void set(Mogre::Ray ray);
		}

		property bool SortByDistance
		{
		public:
			bool get();
		}

		void SetSortByDistance(bool sort, Mogre::ushort maxresults);
		void SetSortByDistance(bool sort);

		Mogre::RaySceneQueryResult^ Execute();

		void Execute(Mogre::IRaySceneQueryListener^ listener);

		Mogre::RaySceneQueryResult^ GetLastResults();

		void ClearResults();

		virtual bool QueryResult(Mogre::MovableObject^ obj, Mogre::Real distance);

		virtual bool QueryResult(Mogre::SceneQuery::WorldFragment^ fragment, Mogre::Real distance);

		//------------------------------------------------------------
		// Implementation for IRaySceneQueryListener
		//------------------------------------------------------------

		DEFINE_MANAGED_NATIVE_CONVERSIONS(RaySceneQuery);
	};

	//################################################################
	//IIntersectionSceneQueryListener
	//################################################################

	public interface class IIntersectionSceneQueryListener
	{
		virtual Ogre::IntersectionSceneQueryListener* _GetNativePtr();

	public:
		virtual bool QueryResult(Mogre::MovableObject^ first, Mogre::MovableObject^ second);
		virtual bool QueryResult(Mogre::MovableObject^ movable, Mogre::SceneQuery::WorldFragment^ fragment);
	};

	//################################################################
	//IIntersectionSceneQueryListener
	//################################################################

	public ref class IntersectionSceneQueryListener abstract : public IMogreDisposable, public IIntersectionSceneQueryListener
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	internal:
		Ogre::IntersectionSceneQueryListener* _native;
		bool _createdByCLR;

	public protected:
		IntersectionSceneQueryListener(intptr_t ptr) : _native((Ogre::IntersectionSceneQueryListener*)ptr)
		{

		}

		IntersectionSceneQueryListener(Ogre::IntersectionSceneQueryListener* obj) : _native(obj)
		{
		}

		virtual Ogre::IntersectionSceneQueryListener* _IIntersectionSceneQueryListener_GetNativePtr() = IIntersectionSceneQueryListener::_GetNativePtr;

	public:
		~IntersectionSceneQueryListener();
	protected:
		!IntersectionSceneQueryListener();

		//Public Declarations
	public:
		IntersectionSceneQueryListener();

		virtual bool QueryResult(Mogre::MovableObject^ first, Mogre::MovableObject^ second) abstract;
		virtual bool QueryResult(Mogre::MovableObject^ movable, Mogre::SceneQuery::WorldFragment^ fragment) abstract;

		property bool IsDisposed
		{
			virtual bool get()
			{
				return _native == nullptr;
			}
		}
	};


	//################################################################
	//IntersectionSceneQueryResult
	//################################################################

	public ref class IntersectionSceneQueryResult
	{
		//Private Declarations
	private protected:
		//Cached fields
		Mogre::SceneQueryMovableIntersectionList^ _movables2movables;
		Mogre::SceneQueryMovableWorldFragmentIntersectionList^ _movables2world;

		//Internal Declarations
	public protected:
		IntersectionSceneQueryResult(Ogre::IntersectionSceneQueryResult* obj) : _native(obj), _createdByCLR(false)
		{
		}

		IntersectionSceneQueryResult(intptr_t ptr) : _native((Ogre::IntersectionSceneQueryResult*)ptr), _createdByCLR(false)
		{
		}

		~IntersectionSceneQueryResult()
		{
			this->!IntersectionSceneQueryResult();
		}
		!IntersectionSceneQueryResult()
		{
			if (_createdByCLR &&_native)
			{
				delete _native;
				_native = 0;
			}
		}

		Ogre::IntersectionSceneQueryResult* _native;
		bool _createdByCLR;


		//Public Declarations
	public:
		IntersectionSceneQueryResult();


		property Mogre::SceneQueryMovableIntersectionList^ movables2movables
		{
		public:
			Mogre::SceneQueryMovableIntersectionList^ get();
		}

		property Mogre::SceneQueryMovableWorldFragmentIntersectionList^ movables2world
		{
		public:
			Mogre::SceneQueryMovableWorldFragmentIntersectionList^ get();
		}

		DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_PLAINWRAPPER(IntersectionSceneQueryResult);
	};

	//################################################################
	//IntersectionSceneQuery
	//################################################################

	public ref class IntersectionSceneQuery : public SceneQuery, public IIntersectionSceneQueryListener
	{
		//Internal Declarations
	public protected:
		IntersectionSceneQuery(Ogre::IntersectionSceneQuery* obj) : SceneQuery(obj)
		{
		}

		IntersectionSceneQuery(intptr_t ptr) : SceneQuery(ptr)
		{
		}

		virtual Ogre::IntersectionSceneQueryListener* _IIntersectionSceneQueryListener_GetNativePtr() = IIntersectionSceneQueryListener::_GetNativePtr;

	public:
		property Mogre::IntersectionSceneQueryResult^ LastResults
		{
		public:
			Mogre::IntersectionSceneQueryResult^ get();
		}

		Mogre::IntersectionSceneQueryResult^ Execute();

		void Execute(Mogre::IIntersectionSceneQueryListener^ listener);

		void ClearResults();

		virtual bool QueryResult(Mogre::MovableObject^ first, Mogre::MovableObject^ second);

		virtual bool QueryResult(Mogre::MovableObject^ movable, Mogre::SceneQuery::WorldFragment^ fragment);

		//------------------------------------------------------------
		// Implementation for IIntersectionSceneQueryListener
		//------------------------------------------------------------

		DEFINE_MANAGED_NATIVE_CONVERSIONS(IntersectionSceneQuery);
	};

	INC_DECLARE_STLLIST(SceneQueryResultMovableList, Mogre::MovableObject^, Ogre::MovableObject*, public, private);
	INC_DECLARE_STLLIST(SceneQueryResultWorldFragmentList, Mogre::SceneQuery::WorldFragment^, Ogre::SceneQuery::WorldFragment*, public, private);
	INC_DECLARE_STLVECTOR(RaySceneQueryResult, Mogre::RaySceneQueryResultEntry^, Ogre::RaySceneQueryResultEntry, public, private);
#define STLDECL_MANAGEDTYPE Pair<Mogre::MovableObject^, Mogre::MovableObject^>
#define STLDECL_NATIVETYPE Ogre::SceneQueryMovableObjectPair
	INC_DECLARE_STLLIST(SceneQueryMovableIntersectionList, STLDECL_MANAGEDTYPE, STLDECL_NATIVETYPE, public, private);
#undef STLDECL_MANAGEDTYPE
#undef STLDECL_NATIVETYPE
	
#define STLDECL_MANAGEDTYPE Pair<Mogre::MovableObject^, Mogre::SceneQuery::WorldFragment^>
#define STLDECL_NATIVETYPE Ogre::SceneQueryMovableObjectWorldFragmentPair
	INC_DECLARE_STLLIST(SceneQueryMovableWorldFragmentIntersectionList, STLDECL_MANAGEDTYPE, STLDECL_NATIVETYPE, public, private);
#undef STLDECL_MANAGEDTYPE
#undef STLDECL_NATIVETYPE

	//################################################################
	//SceneQueryListener_Proxy
	//################################################################

	class SceneQueryListener_Proxy : public Ogre::SceneQueryListener
	{
	public:
		friend ref class Mogre::SceneQueryListener;

		bool* _overriden;

		gcroot<Mogre::SceneQueryListener^> _managed;

		SceneQueryListener_Proxy(Mogre::SceneQueryListener^ managedObj) :
			_managed(managedObj)
		{
		}

		virtual bool queryResult(Ogre::MovableObject* object) override;

		virtual bool queryResult(Ogre::SceneQuery::WorldFragment* fragment) override;
	};

	//################################################################
	//RaySceneQueryListener_Proxy
	//################################################################

	class RaySceneQueryListener_Proxy : public Ogre::RaySceneQueryListener
	{
	public:
		friend ref class Mogre::RaySceneQueryListener;

		bool* _overriden;

		gcroot<Mogre::RaySceneQueryListener^> _managed;

		RaySceneQueryListener_Proxy(Mogre::RaySceneQueryListener^ managedObj) :
			_managed(managedObj)
		{
		}

		virtual bool queryResult(Ogre::MovableObject* obj, Ogre::Real distance) override;

		virtual bool queryResult(Ogre::SceneQuery::WorldFragment* fragment, Ogre::Real distance) override;
	};

	//################################################################
	//IntersectionSceneQueryListener_Proxy
	//################################################################

	class IntersectionSceneQueryListener_Proxy : public Ogre::IntersectionSceneQueryListener
	{
	public:
		friend ref class Mogre::IntersectionSceneQueryListener;

		bool* _overriden;

		gcroot<Mogre::IntersectionSceneQueryListener^> _managed;

		IntersectionSceneQueryListener_Proxy(Mogre::IntersectionSceneQueryListener^ managedObj) :
			_managed(managedObj)
		{
		}

		virtual bool queryResult(Ogre::MovableObject* first, Ogre::MovableObject* second) override;
		virtual bool queryResult(Ogre::MovableObject* movable, Ogre::SceneQuery::WorldFragment* fragment) override;
	};
}