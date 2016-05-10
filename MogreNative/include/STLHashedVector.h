#pragma once

#define INC_DECLARE_STLHASHEDVECTOR_NONCONST( CLASS_NAME, M, N )							\
ref class CLASS_NAME : Collections::Generic::IEnumerable<M>							\
{																					\
public:																				\
	typedef Ogre::HashedVector<N> Container;												\
	typedef Container::iterator Iter;												\
																					\
	ref class Enumerator : Collections::Generic::IEnumerator<M>						\
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
		property M Current															\
		{																			\
			virtual M get();														\
		}																			\
		property Object^ NonGenericCurrent											\
		{																			\
		private:																	\
			virtual Object^ get() sealed = Collections::IEnumerator::Current::get; 	\
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
		property M Value															\
		{																			\
			M get();																\
			void set(M value);														\
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
		if (_nativeWasCreated && _native)											\
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
	public: virtual Collections::Generic::IEnumerator<M>^ GetEnumerator()			\
	{																				\
		return gcnew Enumerator(_native);											\
	}																				\
																					\
	void Assign(int count, M value);												\
																					\
	property M Front																\
	{																				\
		M get();																	\
	}																				\
																					\
	property M Back																	\
	{																				\
		M get();																	\
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
	void Add(M value);																\
																					\
	void RemoveLast()																\
	{																				\
		_native->pop_back();														\
	}																				\
																					\
	/*Resize removed because it requires the native class to have a default constructor*/	\
	/*void Resize(int newSize)*/													\
	/*{							*/													\
	/*	_native->resize(newSize);*/													\
	/*}							*/													\
																					\
	void Resize(int newSize, M value);												\
																					\
	property int Count																\
	{																				\
		int get() { return _native->size(); }										\
	}																				\
																					\
																					\
	property M default[int]															\
	{																				\
		M get(int i);																\
		void set (int i, M value);													\
	}																				\
																					\
	void Erase(int index)															\
	{																				\
		_native->erase( _native->begin() + index );									\
	}																				\
																					\
	void Erase(int index, int count)												\
	{																				\
		Iter& it = _native->begin() + index;										\
		_native->erase( it, it + count );											\
	}																				\
																					\
	void Insert(int index, M value);												\
																					\
	void Insert(int index, int count, M value);										\
																					\
	void Insert(int index, array<M>^ values);										\
																					\
	void TrimExcess()																\
	{																				\
		_native->swap( *_native );													\
	}																				\
																					\
	property int Capacity															\
	{																				\
		int get() { return _native->capacity(); }									\
	}																				\
																					\
	void Reserve(int count)															\
	{																				\
		_native->reserve(count);													\
	}																				\
};


//###########################################################################################
//###########################################################################################


#define INC_DECLARE_STLHASHEDVECTOR_CONST( CLASS_NAME, M, N )								\
ref class Const_##CLASS_NAME : Collections::Generic::IEnumerable<M>					\
{																					\
public:																				\
	typedef Ogre::HashedVector<N> Container;												\
	typedef Container::const_iterator Iter;											\
																					\
	ref class Enumerator : Collections::Generic::IEnumerator<M>						\
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
		property M Current															\
		{																			\
			virtual M get();														\
		}																			\
		property Object^ NonGenericCurrent											\
		{																			\
		private:																	\
			virtual Object^ get() sealed = Collections::IEnumerator::Current::get; 	\
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
		property M Value															\
		{																			\
			M get();																\
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
	public: virtual Collections::Generic::IEnumerator<M>^ GetEnumerator()			\
	{																				\
		return gcnew Enumerator(_native);											\
	}																				\
																					\
	property M Front																\
	{																				\
		M get();																	\
	}																				\
																					\
	property M Back																	\
	{																				\
		M get();																	\
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
	property M default[int]															\
	{																				\
		M get(int i);																\
	}																				\
																					\
	property int Capacity															\
	{																				\
		int get() { return _native->capacity(); }									\
	}																				\
};


//##############################################################################################
//##############################################################################################


#define INC_DECLARE_STLHASHEDVECTOR_EMPTY_NONCONST( CLASS_NAME, M, N )						\
ref class CLASS_NAME																\
{																					\
public:																				\
	typedef Ogre::HashedVector<N> Container;												\
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



#define CPP_DECLARE_STLHASHEDVECTOR_NONCONST( PREFIX, CLASS_NAME, M, N )							\
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
		M PREFIX##CLASS_NAME::Enumerator::Current::get()									\
			{																		\
				return ToManaged<M, Container::value_type>( *(*_pIter) );			\
			}																		\
		Object^ PREFIX##CLASS_NAME::Enumerator::NonGenericCurrent::get()				 	\
			{																		\
				return ToManaged<M, Container::value_type>( *(*_pIter) );			\
			}																		\
																					\
		M PREFIX##CLASS_NAME::Iterator::Value::get()										\
			{																		\
				return ToManaged<M,N>( *(*_native) );								\
			}																		\
		void PREFIX##CLASS_NAME::Iterator::Value::set(M value)								\
			{																		\
				*(*_native) = ToNative<M,N>( value );								\
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
	void PREFIX##CLASS_NAME::Assign(int count, M value)								\
	{																				\
		_native->assign(count, ToNative<M,N>( value ));								\
	}																				\
																					\
	M PREFIX##CLASS_NAME::Front::get()												\
	{																				\
		if (IsEmpty)														\
			throw gcnew System::Exception("The container is empty");				\
		return ToManaged<M,N>( _native->front() );									\
	}																				\
																					\
	M PREFIX##CLASS_NAME::Back::get()												\
	{																				\
		if (IsEmpty)														\
			throw gcnew System::Exception("The container is empty");				\
		return ToManaged<M,N>( _native->back() );									\
	}																				\
																					\
	void PREFIX##CLASS_NAME::Add(M value)											\
	{																				\
		_native->push_back(ToNative<M,N>( value ));									\
	}																				\
																					\
	void PREFIX##CLASS_NAME::Resize(int newSize, M value)							\
	{																				\
		_native->resize(newSize, ToNative<M,N>( value ));							\
	}																				\
																					\
																					\
	M PREFIX##CLASS_NAME::default::get(int i)										\
		{																			\
			return ToManaged<M,N>( _native->at(i) );								\
		}																			\
	void PREFIX##CLASS_NAME::default::set(int i, M value)							\
		{																			\
			_native->at(i) = ToNative<M,N>( value );								\
		}																			\
																					\
	void PREFIX##CLASS_NAME::Insert(int index, M value)								\
	{																				\
		_native->insert( _native->begin() + index, ToNative<M,N>( value ) );		\
	}																				\
																					\
	void PREFIX##CLASS_NAME::Insert(int index, int count, M value)							\
	{																				\
		_native->insert( _native->begin() + index, count, ToNative<M,N>( value ) );	\
	}																				\
																					\
	void PREFIX##CLASS_NAME::Insert(int index, array<M>^ values)							\
	{																				\
		for (int i=0; i < values->Length; i++)										\
		{																			\
			M value = values[i];													\
			_native->insert( _native->begin() + index + i, ToNative<M,N>( value ) );	\
		}																			\
	}


//###########################################################################################
//###########################################################################################


#define CPP_DECLARE_STLHASHEDVECTOR_CONST( PREFIX, CLASS_NAME, M, N )								\
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
		M PREFIX##Const_##CLASS_NAME::Enumerator::Current::get()					\
			{																		\
				return ToManaged<M, Container::value_type>( *(*_pIter) );			\
			}																		\
		Object^ PREFIX##Const_##CLASS_NAME::Enumerator::NonGenericCurrent::get()	\
			{																		\
				return ToManaged<M, Container::value_type>( *(*_pIter) );			\
			}																		\
																					\
		M PREFIX##Const_##CLASS_NAME::ConstIterator::Value::get()					\
			{																		\
				return ToManaged<M,N>( *(*_native) );								\
			}																		\
																					\
		M PREFIX##Const_##CLASS_NAME::Front::get()									\
		{																			\
			if (IsEmpty)													\
				throw gcnew System::Exception("The container is empty");			\
			return ToManaged<M,N>( _native->front() );								\
		}																			\
																					\
		M PREFIX##Const_##CLASS_NAME::Back::get()									\
		{																			\
			if (IsEmpty)													\
				throw gcnew System::Exception("The container is empty");			\
			return ToManaged<M,N>( _native->back() );								\
		}																			\
																					\
	M PREFIX##Const_##CLASS_NAME::default::get(int i)								\
		{																			\
			return ToManaged<M,N>( _native->at(i) );								\
		}


//##############################################################################################
//##############################################################################################


#define CPP_DECLARE_STLHASHEDVECTOR_EMPTY_NONCONST( PREFIX, CLASS_NAME, M, N )						\
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



#define INC_DECLARE_STLHASHEDVECTOR( CLASS_NAME, M, N, PUBLIC, PRIVATE )					\
ref class Const_##CLASS_NAME;														\
PUBLIC INC_DECLARE_STLHASHEDVECTOR_NONCONST( CLASS_NAME, M, N )							\
PUBLIC INC_DECLARE_STLHASHEDVECTOR_CONST( CLASS_NAME, M, N )


#define INC_DECLARE_STLHASHEDVECTOR_READONLY( CLASS_NAME, M, N, PUBLIC, PRIVATE )			\
ref class Const_##CLASS_NAME;														\
PRIVATE INC_DECLARE_STLHASHEDVECTOR_EMPTY_NONCONST( CLASS_NAME, M, N )						\
PUBLIC INC_DECLARE_STLHASHEDVECTOR_CONST( CLASS_NAME, M, N )



#define CPP_DECLARE_STLHASHEDVECTOR( PREFIX, CLASS_NAME, M, N )									\
CPP_DECLARE_STLHASHEDVECTOR_NONCONST( PREFIX, CLASS_NAME, M, N )									\
CPP_DECLARE_STLHASHEDVECTOR_CONST( PREFIX, CLASS_NAME, M, N )


#define CPP_DECLARE_STLHASHEDVECTOR_READONLY( PREFIX, CLASS_NAME, M, N )							\
CPP_DECLARE_STLHASHEDVECTOR_EMPTY_NONCONST( PREFIX, CLASS_NAME, M, N )								\
CPP_DECLARE_STLHASHEDVECTOR_CONST( PREFIX, CLASS_NAME, M, N )
