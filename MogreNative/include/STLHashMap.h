#pragma once

#define INC_DECLARE_STLHASHMAP_NONCONST( CLASS_NAME, MK, MV, NK, NV )				\
ref class CLASS_NAME : Collections::Generic::IEnumerable<Collections::Generic::KeyValuePair<MK, MV>>	\
{																					\
public:																				\
	typedef OGRE_HashMap<NK,NV> Container;										\
	typedef Container::iterator Iter;												\
																					\
	ref class Enumerator : Collections::Generic::IEnumerator<Collections::Generic::KeyValuePair<MK,MV>>		\
	{																				\
		Container* _pCont;															\
		Iter* _pIter;																\
																					\
	internal:																		\
		Enumerator(Container* pContainer) : _pCont(pContainer), _pIter(0)			\
		{																			\
		}																			\
																					\
		!Enumerator()																\
		{																			\
			if (_pIter != 0)														\
			{																		\
				delete _pIter;														\
				_pIter = 0;															\
			}																		\
		}																			\
		~Enumerator()																\
		{																			\
			this->!Enumerator();													\
		}																			\
																					\
	public:																			\
		virtual bool MoveNext();													\
																					\
		property Collections::Generic::KeyValuePair<MK,MV> Current					\
		{																			\
			virtual Collections::Generic::KeyValuePair<MK,MV> get();				\
		}																			\
		property Object^ NonGenericCurrent											\
		{																			\
			private: virtual Object^ get() sealed = Collections::IEnumerator::Current::get;	\
		}																			\
																					\
		virtual void Reset()														\
		{																			\
			this->!Enumerator();													\
		}																			\
	};																				\
																					\
	ref class Iterator																\
	{																				\
		Iter* _native;																\
																					\
	internal:																		\
		Iterator( Iter& iter )														\
		{																			\
			_native = new Iter;														\
			(*_native) = iter;														\
		}																			\
																					\
		!Iterator()																	\
		{																			\
			if (_native != 0)														\
			{																		\
				delete _native;														\
				_native = 0;														\
			}																		\
		}																			\
		~Iterator()																	\
		{																			\
			this->!Iterator();														\
		}																			\
																					\
		inline static operator Iter& (Iterator^ it)									\
		{																			\
			return *(it->_native);													\
		}																			\
																					\
		inline static operator Iterator^ (Iter& it)									\
		{																			\
			return gcnew Iterator(it);												\
		}																			\
																					\
	public:																			\
		property MK Key																\
		{																			\
			MK get();																\
		}																			\
																					\
		property MV Value															\
		{																			\
			MV get();																\
			void set(MV value);														\
		}																			\
																					\
		virtual bool Equals(Object^ obj) override									\
		{																			\
			Iterator^ clr = dynamic_cast<Iterator^>(obj);							\
			if (clr == CLR_NULL)													\
			{																		\
				return false;														\
			}																		\
																					\
			return (this == clr);													\
		}																			\
																					\
		inline static bool operator ==(Iterator^ it1, Iterator^ it2)				\
		{																			\
			if ((Object^)it1 == (Object^)it2) return true;							\
			if ((Object^)it1 == nullptr || (Object^)it2 == nullptr) return false;	\
																					\
			return (*it1->_native) == (*it2->_native);								\
		}																			\
																					\
		inline static bool operator !=(Iterator^ it1, Iterator^ it2)				\
		{																			\
			return !(it1 == it2);													\
		}																			\
																					\
		inline static Iterator^ operator ++(Iterator^ it)							\
		{																			\
			++(*it->_native);														\
			return it;																\
		}																			\
																					\
		inline static Iterator^ operator --(Iterator^ it)							\
		{																			\
			--(*it->_native);														\
			return it;																\
		}																			\
																					\
		void MoveNext()																\
		{																			\
			++(*_native);															\
		}																			\
																					\
		void MovePrevious()															\
		{																			\
			--(*_native);															\
		}																			\
	};																				\
																					\
internal:																			\
	Container* _native;																\
	bool _nativeWasCreated;															\
	Const_##CLASS_NAME^ _readOnly;													\
																					\
	CLASS_NAME( Container& cont ) :													\
		_native(&cont),																\
		_nativeWasCreated(false)													\
	{																				\
	}																				\
																					\
	CLASS_NAME( Container* cont ) :													\
		_native(cont),																\
		_nativeWasCreated(false)													\
	{																				\
	}																				\
																					\
	static CLASS_NAME^ ByValue( Container& cont )									\
	{																				\
		CLASS_NAME^ clr = gcnew CLASS_NAME();										\
		*clr->_native = cont;														\
		return clr;																	\
	}																				\
																					\
	inline static operator Container& (CLASS_NAME^ cont)							\
	{																				\
		return *(cont->_native);													\
	}																				\
																					\
	inline static operator CLASS_NAME^ (Container& cont)							\
	{																				\
		return gcnew CLASS_NAME(cont);												\
	}																				\
																					\
	inline static operator Container* (CLASS_NAME^ cont)							\
	{																				\
		return (cont == CLR_NULL) ? 0 : cont->_native;								\
	}																				\
																					\
	inline static operator CLASS_NAME^ (Container* cont)							\
	{																				\
		return (cont == 0) ? nullptr : gcnew CLASS_NAME(*cont);						\
	}																				\
																					\
public:																				\
	CLASS_NAME() :																	\
		_native(new Container),														\
		_nativeWasCreated(true)														\
	{																				\
	}																				\
																					\
	!CLASS_NAME()																	\
	{																				\
		if (_nativeWasCreated && _native)										\
		{																			\
			delete _native;															\
			_native = 0;															\
		}																			\
	}																				\
	~CLASS_NAME()																	\
	{																				\
		this->!CLASS_NAME();														\
	}																				\
																					\
	property Const_##CLASS_NAME^ ReadOnlyInstance									\
	{																				\
		Const_##CLASS_NAME^ get();													\
	}																				\
																					\
	static operator Const_##CLASS_NAME^ (CLASS_NAME^ obj)							\
	{																				\
		return obj->ReadOnlyInstance;												\
	}																				\
																					\
	private: virtual Collections::IEnumerator^ NonGenericGetEnumerator() sealed = Collections::IEnumerable::GetEnumerator	\
	{																				\
		return gcnew Enumerator(_native);											\
	}																				\
	public: virtual Collections::Generic::IEnumerator<Collections::Generic::KeyValuePair<MK, MV>>^ GetEnumerator()			\
	{																				\
		return gcnew Enumerator(_native);											\
	}																				\
																					\
	Iterator^ Begin()																\
	{																				\
		return _native->begin();													\
	}																				\
																					\
	Iterator^ End()																	\
	{																				\
		return _native->end();														\
	}																				\
																					\
	void Clear()																	\
	{																				\
		_native->clear();															\
	}																				\
																					\
	property bool IsEmpty															\
	{																				\
		bool get() { return _native->empty(); }										\
	}																				\
																					\
	void Erase(Iterator^ iterWhere)													\
	{																				\
		_native->erase(iterWhere);													\
	}																				\
																					\
	void Erase(Iterator^ first, Iterator^ last)										\
	{																				\
		_native->erase(first, last);												\
	}																				\
																					\
	void Erase(MK key);																\
																					\
	Mogre::Pair<Iterator^, bool> Insert(MK key, MV value);							\
																					\
	Iterator^ Insert(Iterator^ iterWhere, MK key, MV value);						\
																					\
	property int Count																\
	{																				\
		int get() { return _native->size(); }										\
	}																				\
																					\
	bool ContainsKey(MK key);														\
																					\
	Iterator^ Find(MK key);															\
																					\
	Iterator^ LowerBound(MK key);													\
																					\
	Iterator^ UpperBound(MK key);													\
																					\
	property MV default[MK]															\
	{																				\
		MV get(MK key);																\
		void set (MK key, MV value);												\
	}																				\
};


//###########################################################################################
//###########################################################################################


#define INC_DECLARE_STLHASHMAP_CONST( CLASS_NAME, MK, MV, NK, NV )					\
ref class Const_##CLASS_NAME : Collections::Generic::IEnumerable<Collections::Generic::KeyValuePair<MK, MV>>			\
{																					\
	typedef OGRE_HashMap<NK,NV> Container;										\
	typedef Container::const_iterator Iter;											\
																					\
public:																				\
	ref class Enumerator : Collections::Generic::IEnumerator<Collections::Generic::KeyValuePair<MK, MV>>	\
	{																				\
		const Container* _pCont;													\
		Iter* _pIter;																\
																					\
	internal:																		\
		Enumerator(const Container* pContainer) : _pCont(pContainer), _pIter(0)		\
		{																			\
		}																			\
																					\
		!Enumerator()																\
		{																			\
			if (_pIter != 0)														\
			{																		\
				delete _pIter;														\
				_pIter = 0;															\
			}																		\
		}																			\
		~Enumerator()																\
		{																			\
			this->!Enumerator();													\
		}																			\
																					\
	public:																			\
		virtual bool MoveNext();													\
																					\
		property Collections::Generic::KeyValuePair<MK,MV> Current					\
		{																			\
			virtual Collections::Generic::KeyValuePair<MK,MV> get();				\
		}																			\
		property Object^ NonGenericCurrent											\
		{																			\
			private: virtual Object^ get() sealed = Collections::IEnumerator::Current::get;	\
		}																			\
																					\
		virtual void Reset()														\
		{																			\
			this->!Enumerator();													\
		}																			\
	};																				\
																					\
	ref class ConstIterator															\
	{																				\
		Iter* _native;																\
																					\
	internal:																		\
		ConstIterator( Iter& iter )													\
		{																			\
			_native = new Iter;														\
			(*_native) = iter;														\
		}																			\
																					\
		!ConstIterator()															\
		{																			\
			if (_native != 0)														\
			{																		\
				delete _native;														\
				_native = 0;														\
			}																		\
		}																			\
		~ConstIterator()															\
		{																			\
			this->!ConstIterator();													\
		}																			\
																					\
		inline static operator Iter& (ConstIterator^ it)							\
		{																			\
			return *(it->_native);													\
		}																			\
																					\
		inline static operator ConstIterator^ (Iter& it)							\
		{																			\
			return gcnew ConstIterator(it);											\
		}																			\
																					\
	public:																			\
		property MK Key																\
		{																			\
			MK get();																\
		}																			\
																					\
		property MV Value															\
		{																			\
			MV get();																\
		}																			\
																					\
		virtual bool Equals(Object^ obj) override									\
		{																			\
			ConstIterator^ clr = dynamic_cast<ConstIterator^>(obj);					\
			if (clr == CLR_NULL)													\
			{																		\
				return false;														\
			}																		\
																					\
			return (this == clr);													\
		}																			\
																					\
		inline static bool operator ==(ConstIterator^ it1, ConstIterator^ it2)		\
		{																			\
			if ((Object^)it1 == (Object^)it2) return true;							\
			if ((Object^)it1 == nullptr || (Object^)it2 == nullptr) return false;	\
																					\
			return (*it1->_native) == (*it2->_native);								\
		}																			\
																					\
		inline static bool operator !=(ConstIterator^ it1, ConstIterator^ it2)		\
		{																			\
			return !(it1 == it2);													\
		}																			\
																					\
		inline static ConstIterator^ operator ++(ConstIterator^ it)					\
		{																			\
			++(*it->_native);														\
			return it;																\
		}																			\
																					\
		inline static ConstIterator^ operator --(ConstIterator^ it)					\
		{																			\
			--(*it->_native);														\
			return it;																\
		}																			\
																					\
		void MoveNext()																\
		{																			\
			++(*_native);															\
		}																			\
																					\
		void MovePrevious()															\
		{																			\
			--(*_native);															\
		}																			\
	};																				\
																					\
internal:																			\
	const Container* _native;														\
	CLASS_NAME^ _baseContainer;														\
																					\
	Const_##CLASS_NAME( const Container& cont ) :									\
		_native(&cont)																\
	{																				\
	}																				\
																					\
	Const_##CLASS_NAME( CLASS_NAME^ baseContainer ) :								\
		_native(baseContainer->_native),											\
		_baseContainer(baseContainer)												\
	{																				\
	}																				\
																					\
	inline static operator const Container& (Const_##CLASS_NAME^ cont)				\
	{																				\
		return *(cont->_native);													\
	}																				\
																					\
	inline static operator Const_##CLASS_NAME^ (const Container& cont)				\
	{																				\
		return gcnew Const_##CLASS_NAME(cont);										\
	}																				\
																					\
	inline static operator const Container* (Const_##CLASS_NAME^ cont)				\
	{																				\
		return (cont == CLR_NULL) ? 0 : cont->_native;								\
	}																				\
																					\
	inline static operator Const_##CLASS_NAME^ (const Container* cont)				\
	{																				\
		return (cont == 0) ? nullptr : gcnew Const_##CLASS_NAME(*cont);				\
	}																				\
																					\
	inline static operator Const_##CLASS_NAME^ (CLASS_NAME^ cont)					\
	{																				\
		return (cont == CLR_NULL) ? nullptr : cont->ReadOnlyInstance;				\
	}																				\
																					\
public:																				\
																					\
	private: virtual Collections::IEnumerator^ NonGenericGetEnumerator() sealed = Collections::IEnumerable::GetEnumerator		\
	{																				\
		return gcnew Enumerator(_native);											\
	}																				\
	public: virtual Collections::Generic::IEnumerator<Collections::Generic::KeyValuePair<MK, MV>>^ GetEnumerator()		\
	{																				\
		return gcnew Enumerator(_native);											\
	}																				\
																					\
	ConstIterator^ Begin()															\
	{																				\
		return _native->begin();													\
	}																				\
																					\
	ConstIterator^ End()															\
	{																				\
		return _native->end();														\
	}																				\
																					\
	property bool IsEmpty															\
	{																				\
		bool get() { return _native->empty(); }										\
	}																				\
																					\
	property int Count																\
	{																				\
		int get() { return _native->size(); }										\
	}																				\
																					\
	bool ContainsKey(MK key);														\
																					\
	ConstIterator^ Find(MK key);													\
																					\
	ConstIterator^ LowerBound(MK key);												\
																					\
	ConstIterator^ UpperBound(MK key);												\
																					\
	property MV default[MK]															\
	{																				\
		MV get(MK key);																\
	}																				\
};


//##############################################################################################
//##############################################################################################


#define INC_DECLARE_STLHASHMAP_EMPTY_NONCONST( CLASS_NAME, MK, MV, NK, NV )			\
ref class CLASS_NAME																\
{																					\
	typedef OGRE_HashMap<NK,NV> Container;										\
																					\
internal:																			\
	Container* _native;																\
	Const_##CLASS_NAME^ _readOnly;													\
																					\
	static CLASS_NAME^ ByValue( Container& cont )									\
	{																				\
		CLASS_NAME^ clr = gcnew CLASS_NAME();										\
		*clr->_native = cont;														\
		return clr;																	\
	}																				\
																					\
	CLASS_NAME() :																	\
		_native(new Container)														\
	{																				\
	}																				\
																					\
	!CLASS_NAME()																	\
	{																				\
		if (_native != 0)															\
		{																			\
			delete _native;															\
			_native = 0;															\
		}																			\
	}																				\
	~CLASS_NAME()																	\
	{																				\
		this->!CLASS_NAME();														\
	}																				\
																					\
	property Const_##CLASS_NAME^ ReadOnlyInstance									\
	{																				\
		Const_##CLASS_NAME^ get();													\
	}																				\
																					\
	static operator Const_##CLASS_NAME^ (CLASS_NAME^ obj)							\
	{																				\
		return obj->ReadOnlyInstance;												\
	}																				\
};


//##############################################################################################
//##############################################################################################



#define CPP_DECLARE_STLHASHMAP_NONCONST( PREFIX, CLASS_NAME, MK, MV, NK, NV )				\
	bool PREFIX##CLASS_NAME::Enumerator::MoveNext()											\
		{																			\
			if (_pIter == 0)														\
			{																		\
				_pIter = new Iter;													\
				(*_pIter) = _pCont->begin();										\
			}																		\
			else																	\
			{																		\
				++(*_pIter);														\
			}																		\
																					\
			if ( (*_pIter) == _pCont->end() )										\
				return false;														\
																					\
			return true;															\
		}																			\
																					\
																					\
		Collections::Generic::KeyValuePair<MK,MV> PREFIX##CLASS_NAME::Enumerator::Current::get()	\
			{																		\
				return Collections::Generic::KeyValuePair<MK,MV>(ToManaged<MK,NK>( (*_pIter)->first ), ToManaged<MV,NV>( (*_pIter)->second ));	\
			}																		\
		Object^ PREFIX##CLASS_NAME::Enumerator::NonGenericCurrent::get()					\
			{																		\
				return Collections::Generic::KeyValuePair<MK,MV>(ToManaged<MK,NK>( (*_pIter)->first ), ToManaged<MV,NV>( (*_pIter)->second ));	\
			}																		\
																					\
		MK PREFIX##CLASS_NAME::Iterator::Key::get() { return ToManaged<MK,NK>( (*_native)->first ); }		\
																					\
		MV PREFIX##CLASS_NAME::Iterator::Value::get()										\
			{																		\
				return ToManaged<MV,NV>( (*_native)->second );						\
			}																		\
		void PREFIX##CLASS_NAME::Iterator::Value::set(MV value)								\
			{																		\
				(*_native)->second = ToNative<MV,NV>( value );						\
			}																		\
																					\
	PREFIX##Const_##CLASS_NAME^ PREFIX##CLASS_NAME::ReadOnlyInstance::get()			\
		{																			\
			if (_readOnly == (Object^)nullptr)										\
				_readOnly = gcnew Const_##CLASS_NAME(this);							\
																					\
			return _readOnly;														\
		}																			\
																					\
	void PREFIX##CLASS_NAME::Erase(MK key)											\
	{																				\
		_native->erase(ToNative<MK,NK>( key ));										\
	}																				\
																					\
	Mogre::Pair<PREFIX##CLASS_NAME::Iterator^, bool> PREFIX##CLASS_NAME::Insert(MK key, MV value)	\
	{																				\
		std::pair<Iter,bool> res = _native->insert( std::pair<NK,NV>( ToNative<MK,NK>( key ), ToNative<MV,NV>( value ) ) );	\
		return Mogre::Pair<PREFIX##CLASS_NAME::Iterator^, bool>(res.first, res.second);		\
	}																				\
																					\
	PREFIX##CLASS_NAME::Iterator^ PREFIX##CLASS_NAME::Insert(PREFIX##CLASS_NAME::Iterator^ iterWhere, MK key, MV value)							\
	{																				\
		return _native->insert( iterWhere, std::pair<NK,NV>( ToNative<MK,NK>( key ), ToNative<MV,NV>( value ) ) );	\
	}																				\
																					\
	bool PREFIX##CLASS_NAME::ContainsKey(MK key)									\
	{																				\
		return (_native->count(ToNative<MK,NK>( key )) > 0);						\
	}																				\
																					\
	PREFIX##CLASS_NAME::Iterator^ PREFIX##CLASS_NAME::Find(MK key)					\
	{																				\
		return _native->find(ToNative<MK,NK>( key ));								\
	}																				\
																					\
	PREFIX##CLASS_NAME::Iterator^ PREFIX##CLASS_NAME::LowerBound(MK key)			\
	{																				\
		return _native->lower_bound(ToNative<MK,NK>( key ));						\
	}																				\
																					\
	PREFIX##CLASS_NAME::Iterator^ PREFIX##CLASS_NAME::UpperBound(MK key)							\
	{																				\
		return _native->upper_bound(ToNative<MK,NK>( key ));						\
	}																				\
																					\
	MV PREFIX##CLASS_NAME::default::get(MK key)												\
		{																			\
			return ToManaged<MV,NV>( (*_native)[ ToNative<MK,NK>( key ) ] );		\
		}																			\
	void PREFIX##CLASS_NAME::default::set (MK key, MV value)								\
		{																			\
			(*_native)[ ToNative<MK,NK>( key ) ] = ToNative<MV,NV>( value );		\
		}


//###########################################################################################
//###########################################################################################


#define CPP_DECLARE_STLHASHMAP_CONST( PREFIX, CLASS_NAME, MK, MV, NK, NV )					\
	bool PREFIX##Const_##CLASS_NAME::Enumerator::MoveNext()									\
		{																			\
			if (_pIter == 0)														\
			{																		\
				_pIter = new Iter;													\
				(*_pIter) = _pCont->begin();										\
			}																		\
			else																	\
			{																		\
				++(*_pIter);														\
			}																		\
																					\
			if ( (*_pIter) == _pCont->end() )										\
				return false;														\
																					\
			return true;															\
		}																			\
																					\
		Collections::Generic::KeyValuePair<MK,MV> PREFIX##Const_##CLASS_NAME::Enumerator::Current::get()		\
			{																		\
				return Collections::Generic::KeyValuePair<MK,MV>(ToManaged<MK,NK>( (*_pIter)->first ), ToManaged<MV,NV>( (*_pIter)->second ));	\
			}																		\
		Object^ PREFIX##Const_##CLASS_NAME::Enumerator::NonGenericCurrent::get()				\
			{																		\
				return Collections::Generic::KeyValuePair<MK,MV>(ToManaged<MK,NK>( (*_pIter)->first ), ToManaged<MV,NV>( (*_pIter)->second ));	\
			}																		\
																					\
		MK PREFIX##Const_##CLASS_NAME::ConstIterator::Key::get() { return ToManaged<MK,NK>( (*_native)->first ); }		\
																					\
		MV PREFIX##Const_##CLASS_NAME::ConstIterator::Value::get()							\
			{																		\
				return ToManaged<MV,NV>( (*_native)->second );						\
			}																		\
																					\
																					\
	bool PREFIX##Const_##CLASS_NAME::ContainsKey(MK key)										\
	{																				\
		return (_native->count(ToNative<MK,NK>( key )) > 0);						\
	}																				\
																					\
	PREFIX##Const_##CLASS_NAME::ConstIterator^ PREFIX##Const_##CLASS_NAME::Find(MK key)				\
	{																				\
		return _native->find(ToNative<MK,NK>( key ));								\
	}																				\
																					\
	PREFIX##Const_##CLASS_NAME::ConstIterator^ PREFIX##Const_##CLASS_NAME::LowerBound(MK key)			\
	{																				\
		return _native->lower_bound(ToNative<MK,NK>( key ));						\
	}																				\
																					\
	PREFIX##Const_##CLASS_NAME::ConstIterator^ PREFIX##Const_##CLASS_NAME::UpperBound(MK key)			\
	{																				\
		return _native->upper_bound(ToNative<MK,NK>( key ));						\
	}																				\
																					\
	MV PREFIX##Const_##CLASS_NAME::default::get(MK key)										\
		{																			\
			return ToManaged<MV,NV>( (*const_cast<Container*>(_native))[ ToNative<MK,NK>( key ) ] );		\
		}


//##############################################################################################
//##############################################################################################


#define CPP_DECLARE_STLHASHMAP_EMPTY_NONCONST( PREFIX, CLASS_NAME, MK, MV, NK, NV )			\
																					\
	PREFIX##Const_##CLASS_NAME^ PREFIX##CLASS_NAME::ReadOnlyInstance::get()							\
		{																			\
			if (_readOnly == (Object^)nullptr)										\
				_readOnly = gcnew Const_##CLASS_NAME(this);							\
																					\
			return _readOnly;														\
		}



//##############################################################################################
//##############################################################################################



#define INC_DECLARE_STLHASHMAP( CLASS_NAME, MK, MV, NK, NV, PUBLIC, PRIVATE )		\
ref class Const_##CLASS_NAME;														\
PUBLIC INC_DECLARE_STLHASHMAP_NONCONST( CLASS_NAME, MK, MV, NK, NV )				\
PUBLIC INC_DECLARE_STLHASHMAP_CONST( CLASS_NAME, MK, MV, NK, NV )


#define INC_DECLARE_STLHASHMAP_READONLY( CLASS_NAME, MK, MV, NK, NV, PUBLIC, PRIVATE )		\
ref class Const_##CLASS_NAME;														\
PRIVATE INC_DECLARE_STLHASHMAP_EMPTY_NONCONST( CLASS_NAME, MK, MV, NK, NV )			\
PUBLIC INC_DECLARE_STLHASHMAP_CONST( CLASS_NAME, MK, MV, NK, NV )



#define CPP_DECLARE_STLHASHMAP( PREFIX, CLASS_NAME, MK, MV, NK, NV )						\
CPP_DECLARE_STLHASHMAP_NONCONST( PREFIX, CLASS_NAME, MK, MV, NK, NV )						\
CPP_DECLARE_STLHASHMAP_CONST( PREFIX, CLASS_NAME, MK, MV, NK, NV )


#define CPP_DECLARE_STLHASHMAP_READONLY( PREFIX, CLASS_NAME, MK, MV, NK, NV )				\
CPP_DECLARE_STLHASHMAP_EMPTY_NONCONST( PREFIX, CLASS_NAME, MK, MV, NK, NV )					\
CPP_DECLARE_STLHASHMAP_CONST( PREFIX, CLASS_NAME, MK, MV, NK, NV )
