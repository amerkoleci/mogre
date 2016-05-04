#pragma once

/* Embeds a native class. Example:

struct NativePoint {
        int x, y;
      };

      ref class R {
        Embedded<NativePoint> np;
      };
*/
template<typename T>
  ref class Embedded {
    T* t;

    !Embedded() {
      if (t != nullptr) {
        delete t;
        t = nullptr;
      }
    }

    ~Embedded() {
      this->!Embedded();
    }

  public:
    Embedded() : t(new T) {}

    static T* operator&(Embedded% e) { return e.t; }
    static T* operator->(Embedded% e) { return e.t; }
  };


  /* Replacement for native arrays inside managed classes. Example:
     ref class R {
        inline_array<int, 10> arr;
      };
*/
  template<typename T, int length>
  [System::Runtime::InteropServices::StructLayout
    (
      System::Runtime::InteropServices::LayoutKind::Explicit,
      Size=(sizeof(T)*length)
    )
  ]
  public value struct inline_array {
  private:
    [System::Runtime::InteropServices::FieldOffset(0)]
    T dummy_item;

  public:
    inline T% operator[](int index) {
      return *((&dummy_item)+index);
    }

    static operator interior_ptr<T>(inline_array<T,length>% ia) {
      return &ia.dummy_item;
    }
  };


  //To use inline_array with value classes, the size of each array item must be
  //manually declared. Example:
  //   ref class R {
  //      inline_array_explicit<Vector3, 3*sizeof(Real), 10> arr;
  //    };
  template<typename T, int size, int length>
  [System::Runtime::InteropServices::StructLayout
    (
      System::Runtime::InteropServices::LayoutKind::Explicit,
      Size=(size*length)
    )
  ]
  public value struct inline_array_explicit {
  private:
    [System::Runtime::InteropServices::FieldOffset(0)]
    T dummy_item;

  public:
    inline T% operator[](int index) {
      return *((&dummy_item)+index);
    }

    static operator interior_ptr<T>(inline_array_explicit<T,size,length>% ia) {
      return &ia.dummy_item;
    }
  };
