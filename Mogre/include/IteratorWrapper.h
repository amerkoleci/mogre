#pragma once

namespace Mogre
{
	// This is just a container for the Ogre iterator. A ref class can refer to the iterator
	// by keeping a pointer to the IteratorHolder
	template <typename T>
	struct IteratorHolder
	{
		T it;
		IteratorHolder(T& iter) : it(iter)
		{
		}
	};
}


#define INC_DECLARE_ITERATOR( CLASS_NAME, ITER, CONTAINER, M, N )					\
ref class CLASS_NAME sealed : Collections::Generic::IEnumerable<M>, Collections::Generic::IEnumerator<M>	\
{																					\
	IteratorHolder<ITER>* _holder;													\
	M _current;																		\
	CONTAINER^ _container;															\
																					\
internal:																			\
	CLASS_NAME(const ITER& iter)													\
	{																				\
		_holder = new IteratorHolder<ITER>( const_cast<ITER&>(iter) );				\
	}																				\
																					\
	!CLASS_NAME()																	\
	{																				\
		if (_holder != 0)															\
		{																			\
			delete _holder;															\
			_holder = 0;															\
		}																			\
	}																				\
	~CLASS_NAME()																	\
	{																				\
		this->!CLASS_NAME();														\
	}																				\
																					\
	inline static operator CLASS_NAME^ (const ITER& iter)							\
	{																				\
		return gcnew CLASS_NAME(iter);												\
	}																				\
	inline static operator ITER& (CLASS_NAME^ iter)									\
	{																				\
		return iter->_holder->it;													\
	}																				\
																					\
public:																				\
	CLASS_NAME(CONTAINER^ container);												\
																					\
	private: virtual Collections::IEnumerator^ NonGenericGetEnumerator() sealed = Collections::IEnumerable::GetEnumerator	\
	{																				\
		return this;																\
	}																				\
	public: virtual Collections::Generic::IEnumerator<M>^ GetEnumerator()			\
	{																				\
		return this;																\
	}																				\
																					\
	virtual bool MoveNext();														\
																					\
	property M Current																\
	{																				\
		virtual M get()																\
		{																			\
			return _current;														\
		}																			\
	}																				\
	property Object^ NonGenericCurrent												\
	{																				\
		private: virtual Object^ get() sealed = Collections::IEnumerator::Current::get	\
		{																			\
			return _current;														\
		}																			\
	}																				\
																					\
	virtual void Reset()															\
	{																				\
		throw gcnew NotSupportedException("Reset is not supported.");				\
	}																				\
};


//###########################################################################################
//###########################################################################################

#define CPP_DECLARE_ITERATOR( PREFIX, CLASS_NAME, ITER, CONTAINER, M, N, CONST )	\
	bool PREFIX##CLASS_NAME::MoveNext()												\
	{																				\
		if (_holder->it.hasMoreElements())											\
		{																			\
			_current = ToManaged<M, N>(_holder->it.getNext());						\
			return true;															\
		}																			\
		else																		\
			return false;															\
	}																				\
																					\
	PREFIX##CLASS_NAME::CLASS_NAME(CONTAINER^ container)							\
	{																				\
		_holder = new IteratorHolder<ITER>( ITER( const_cast<CONST CONTAINER::Container*>(container->_native)->begin(), const_cast<CONST CONTAINER::Container*>(container->_native)->end() ) );	\
		_container = container;														\
	}



//###########################################################################################
//###########################################################################################


#define INC_DECLARE_ITERATOR_NOCONSTRUCTOR( CLASS_NAME, ITER, CONTAINER, M, N )		\
ref class CLASS_NAME sealed : Collections::Generic::IEnumerable<M>, Collections::Generic::IEnumerator<M>	\
{																					\
	IteratorHolder<ITER>* _holder;													\
	M _current;																		\
																					\
internal:																			\
	CLASS_NAME(const ITER& iter)													\
	{																				\
		_holder = new IteratorHolder<ITER>( const_cast<ITER&>(iter) );				\
	}																				\
																					\
	!CLASS_NAME()																	\
	{																				\
		if (_holder != 0)															\
		{																			\
			delete _holder;															\
			_holder = 0;															\
		}																			\
	}																				\
	~CLASS_NAME()																	\
	{																				\
		this->!CLASS_NAME();														\
	}																				\
																					\
	inline static operator CLASS_NAME^ (const ITER& iter)							\
	{																				\
		return gcnew CLASS_NAME(iter);												\
	}																				\
	inline static operator ITER& (CLASS_NAME^ iter)									\
	{																				\
		return iter->_holder->it;													\
	}																				\
																					\
public:																				\
																					\
	private: virtual Collections::IEnumerator^ NonGenericGetEnumerator() sealed = Collections::IEnumerable::GetEnumerator	\
	{																				\
		return this;																\
	}																				\
	public: virtual Collections::Generic::IEnumerator<M>^ GetEnumerator()			\
	{																				\
		return this;																\
	}																				\
																					\
	virtual bool MoveNext();														\
																					\
	property M Current																\
	{																				\
		virtual M get()																\
		{																			\
			return _current;														\
		}																			\
	}																				\
	property Object^ NonGenericCurrent												\
	{																				\
		private: virtual Object^ get() sealed = Collections::IEnumerator::Current::get	\
		{																			\
			return _current;														\
		}																			\
	}																				\
																					\
	virtual void Reset()															\
	{																				\
		throw gcnew NotSupportedException("Reset is not supported.");				\
	}																				\
};


//###########################################################################################
//###########################################################################################

#define CPP_DECLARE_ITERATOR_NOCONSTRUCTOR( PREFIX, CLASS_NAME, ITER, CONTAINER, M, N )		\
	bool PREFIX##CLASS_NAME::MoveNext()												\
	{																				\
		if (_holder->it.hasMoreElements())											\
		{																			\
			_current = ToManaged<M, N>(_holder->it.getNext());						\
			return true;															\
		}																			\
		else																		\
			return false;															\
	}




//###########################################################################################
// Iterator for map/multimap
//###########################################################################################


#define INC_DECLARE_MAP_ITERATOR( CLASS_NAME, ITER, CONTAINER, M, N, MK, NK )					\
ref class CLASS_NAME sealed : Collections::Generic::IEnumerable<M>, Collections::Generic::IEnumerator<M>	\
{																					\
	IteratorHolder<ITER>* _holder;													\
	M _current;																		\
	MK _currentKey;																	\
	CONTAINER^ _container;															\
																					\
internal:																			\
	CLASS_NAME(const ITER& iter)													\
	{																				\
		_holder = new IteratorHolder<ITER>( const_cast<ITER&>(iter) );				\
	}																				\
																					\
	!CLASS_NAME()																	\
	{																				\
		if (_holder != 0)															\
		{																			\
			delete _holder;															\
			_holder = 0;															\
		}																			\
	}																				\
	~CLASS_NAME()																	\
	{																				\
		this->!CLASS_NAME();														\
	}																				\
																					\
	inline static operator CLASS_NAME^ (const ITER& iter)							\
	{																				\
		return gcnew CLASS_NAME(iter);												\
	}																				\
	inline static operator ITER& (CLASS_NAME^ iter)									\
	{																				\
		return iter->_holder->it;													\
	}																				\
																					\
public:																				\
	CLASS_NAME(CONTAINER^ container);												\
																					\
	private: virtual Collections::IEnumerator^ NonGenericGetEnumerator() sealed = Collections::IEnumerable::GetEnumerator	\
	{																				\
		return this;																\
	}																				\
	public: virtual Collections::Generic::IEnumerator<M>^ GetEnumerator()			\
	{																				\
		return this;																\
	}																				\
																					\
	virtual bool MoveNext();														\
																					\
	property M Current																\
	{																				\
		virtual M get()																\
		{																			\
			return _current;														\
		}																			\
	}																				\
	property Object^ NonGenericCurrent												\
	{																				\
		private: virtual Object^ get() sealed = Collections::IEnumerator::Current::get	\
		{																			\
			return _current;														\
		}																			\
	}																				\
																					\
	property MK CurrentKey															\
	{																				\
		virtual MK get()															\
		{																			\
			return _currentKey;														\
		}																			\
	}																				\
																					\
	virtual void Reset()															\
	{																				\
		throw gcnew NotSupportedException("Reset is not supported.");				\
	}																				\
};


//###########################################################################################
//###########################################################################################

#define CPP_DECLARE_MAP_ITERATOR( PREFIX, CLASS_NAME, ITER, CONTAINER, M, N, MK, NK, CONST )	\
	bool PREFIX##CLASS_NAME::MoveNext()												\
	{																				\
		if (_holder->it.hasMoreElements())											\
		{																			\
			_currentKey = ToManaged<MK, NK>(_holder->it.peekNextKey());				\
			_current = ToManaged<M, N>(_holder->it.getNext());						\
			return true;															\
		}																			\
		else																		\
			return false;															\
	}																				\
																					\
	PREFIX##CLASS_NAME::CLASS_NAME(CONTAINER^ container)							\
	{																				\
		_holder = new IteratorHolder<ITER>( ITER( const_cast<CONST CONTAINER::Container*>(container->_native)->begin(), const_cast<CONST CONTAINER::Container*>(container->_native)->end() ) );	\
		_container = container;														\
	}



//###########################################################################################
//###########################################################################################


#define INC_DECLARE_MAP_ITERATOR_NOCONSTRUCTOR( CLASS_NAME, ITER, CONTAINER, M, N, MK, NK )		\
ref class CLASS_NAME sealed : Collections::Generic::IEnumerable<M>, Collections::Generic::IEnumerator<M>	\
{																					\
	IteratorHolder<ITER>* _holder;													\
	M _current;																		\
	MK _currentKey;																	\
																					\
internal:																			\
	CLASS_NAME(const ITER& iter)													\
	{																				\
		_holder = new IteratorHolder<ITER>( const_cast<ITER&>(iter) );				\
	}																				\
																					\
	!CLASS_NAME()																	\
	{																				\
		if (_holder != 0)															\
		{																			\
			delete _holder;															\
			_holder = 0;															\
		}																			\
	}																				\
	~CLASS_NAME()																	\
	{																				\
		this->!CLASS_NAME();														\
	}																				\
																					\
	inline static operator CLASS_NAME^ (const ITER& iter)							\
	{																				\
		return gcnew CLASS_NAME(iter);												\
	}																				\
	inline static operator ITER& (CLASS_NAME^ iter)									\
	{																				\
		return iter->_holder->it;													\
	}																				\
																					\
public:																				\
																					\
	private: virtual Collections::IEnumerator^ NonGenericGetEnumerator() sealed = Collections::IEnumerable::GetEnumerator	\
	{																				\
		return this;																\
	}																				\
	public: virtual Collections::Generic::IEnumerator<M>^ GetEnumerator()			\
	{																				\
		return this;																\
	}																				\
																					\
	virtual bool MoveNext();														\
																					\
	property M Current																\
	{																				\
		virtual M get()																\
		{																			\
			return _current;														\
		}																			\
	}																				\
	property Object^ NonGenericCurrent												\
	{																				\
		private: virtual Object^ get() sealed = Collections::IEnumerator::Current::get	\
		{																			\
			return _current;														\
		}																			\
	}																				\
																					\
	property MK CurrentKey															\
	{																				\
		virtual MK get()															\
		{																			\
			return _currentKey;														\
		}																			\
	}																				\
																					\
	virtual void Reset()															\
	{																				\
		throw gcnew NotSupportedException("Reset is not supported.");				\
	}																				\
};


//###########################################################################################
//###########################################################################################

#define CPP_DECLARE_MAP_ITERATOR_NOCONSTRUCTOR( PREFIX, CLASS_NAME, ITER, CONTAINER, M, N, MK, NK )		\
	bool PREFIX##CLASS_NAME::MoveNext()												\
	{																				\
		if (_holder->it.hasMoreElements())											\
		{																			\
			_currentKey = ToManaged<MK, NK>(_holder->it.peekNextKey());				\
			_current = ToManaged<M, N>(_holder->it.getNext());						\
			return true;															\
		}																			\
		else																		\
			return false;															\
	}
