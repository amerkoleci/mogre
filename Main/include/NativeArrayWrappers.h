namespace Mogre
{
	//###########################################################################################
	// NativeArray_Real
	//###########################################################################################
	public value class Const_NativeArray_Real
	{
		Real* _ptr;

	internal:
		Const_NativeArray_Real(Real* ptr) : _ptr(ptr)
		{
		}

	public:
		property Real default[int]
		{
			Real get(int index)
			{
				return *(_ptr + index);
			}
		}
	};

	public value class NativeArray_Real
	{
		Real* _ptr;

	public:
		NativeArray_Real(Real* ptr) : _ptr(ptr)
		{
		}

		property Real default[int]
		{
			Real get(int index)
			{
				return *(_ptr + index);
			}
			void set(int index, Real value)
			{
				*(_ptr + index) = value;
			}
		}

		property Const_NativeArray_Real ReadOnlyInstance
		{
			Const_NativeArray_Real get()
			{
				return Const_NativeArray_Real(_ptr);
			}
		}

		inline static operator Const_NativeArray_Real (NativeArray_Real narray)
		{
			return Const_NativeArray_Real(_ptr);
		}
	};
	//###########################################################################################


	//###########################################################################################
	// NativeArray_ushort
	//###########################################################################################
	public value class Const_NativeArray_ushort
	{
		unsigned short* _ptr;

	internal:
		Const_NativeArray_ushort(unsigned short* ptr) : _ptr(ptr)
		{
		}

	public:
		property unsigned short default[int]
		{
			unsigned short get(int index)
			{
				return *(_ptr + index);
			}
		}
	};

	public value class NativeArray_ushort
	{
		unsigned short* _ptr;

	public:
		NativeArray_ushort(unsigned short* ptr) : _ptr(ptr)
		{
		}

		property unsigned short default[int]
		{
			unsigned short get(int index)
			{
				return *(_ptr + index);
			}
			void set(int index, unsigned short value)
			{
				*(_ptr + index) = value;
			}
		}

		property Const_NativeArray_ushort ReadOnlyInstance
		{
			Const_NativeArray_ushort get()
			{
				return Const_NativeArray_ushort(_ptr);
			}
		}

		inline static operator Const_NativeArray_ushort (NativeArray_ushort narray)
		{
			return Const_NativeArray_ushort(_ptr);
		}
	};
	//###########################################################################################


	//###########################################################################################
	// NativeArray_Vector3
	//###########################################################################################
	public value class Const_NativeArray_Vector3
	{
		Vector3* _ptr;

	internal:
		Const_NativeArray_Vector3(Vector3* ptr) : _ptr(ptr)
		{
		}

	public:
		property Vector3 default[int]
		{
			Vector3 get(int index)
			{
				return *(_ptr + index);
			}
		}
	};

	public value class NativeArray_Vector3
	{
		Vector3* _ptr;

	public:
		NativeArray_Vector3(Vector3* ptr) : _ptr(ptr)
		{
		}

		property Vector3 default[int]
		{
			Vector3 get(int index)
			{
				return *(_ptr + index);
			}
			void set(int index, Vector3 value)
			{
				*(_ptr + index) = value;
			}
		}

		property Const_NativeArray_Vector3 ReadOnlyInstance
		{
			Const_NativeArray_Vector3 get()
			{
				return Const_NativeArray_Vector3(_ptr);
			}
		}

		inline static operator Const_NativeArray_Vector3 (NativeArray_Vector3 narray)
		{
			return Const_NativeArray_Vector3(_ptr);
		}
	};
	//###########################################################################################
}