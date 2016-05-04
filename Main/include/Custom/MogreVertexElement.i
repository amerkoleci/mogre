/** <summary>This class declares the usage of a single vertex buffer as a component
of a complete VertexDeclaration.</summary>
<remarks>
Several vertex buffers can be used to supply the input geometry for a
rendering operation, and in each case a vertex buffer can be used in
different ways for different operations; the buffer itself does not
define the semantics (position, normal etc), the VertexElement
class does.
</remarks>
*/
public ref class VertexElement
{
protected public:
    // A value class to map the fields of the Ogre::VertexElement
    [StructLayout(LayoutKind::Sequential)]
    value struct Data
    {
        // The source vertex buffer, as bound to an index using VertexBufferBinding
        unsigned short mSource;
        // The offset in the buffer that this element starts at
        size_t mOffset;
        // The type of element
        VertexElementType mType;
        // The meaning of the element
        VertexElementSemantic mSemantic;
        // Index of the item, only applicable for some elements like texture coords
        unsigned short mIndex;
    };

public:
    static operator VertexElement^ (const Ogre::VertexElement& elem)
    {
        VertexElement^ clrelem = gcnew VertexElement;
        pin_ptr<Ogre::VertexElement> ptr = interior_ptr<Ogre::VertexElement>(&clrelem->data);
        *ptr = elem;
        return clrelem;
    }

    static operator VertexElement^ (const Ogre::VertexElement* pelem)
    {
        if (pelem)
            return (VertexElement^) *pelem;
        else
            return nullptr;
    }

    static operator Ogre::VertexElement (VertexElement^ elem)
    {
        pin_ptr<Ogre::VertexElement> ptr = interior_ptr<Ogre::VertexElement>(&elem->data);
        return *ptr;
    }

internal:
    VertexElement()
    {
    }

protected public:
    Data data;

public:
    /// <summary>Constructor, should not be called directly, call VertexDeclaration.AddElement</summary>
    VertexElement(unsigned short source, size_t offset, VertexElementType theType,
        VertexElementSemantic semantic, unsigned short index);
    /// <summary>Gets the vertex buffer index from where this element draws it's values</summary>
    property unsigned short Source
    {
        unsigned short get() { return data.mSource; }
    }
    /// <summary>Gets the offset into the buffer where this element starts</summary>
    property size_t Offset
    {
        size_t get() { return data.mOffset; }
    }
    /// <summary>Gets the data format of this element</summary>
    property VertexElementType Type
    {
        VertexElementType get() { return data.mType; }
    }
    /// <summary>Gets the meaning of this element</summary>
    property VertexElementSemantic Semantic
    {
        VertexElementSemantic get() { return data.mSemantic; }
    }
    /// <summary>Gets the index of this element, only applicable for repeating elements</summary>
    property unsigned short Index
    {
        unsigned short get() { return data.mIndex; }
    }
    /// <summary>Gets the size of this element in bytes</summary>
    property size_t Size
    {
        size_t get();
    }
    /// <summary>Utility method for helping to calculate offsets</summary>
    static size_t GetTypeSize(VertexElementType etype);
    /// <summary>Utility method which returns the count of values in a given type</summary>
    static unsigned short GetTypeCount(VertexElementType etype);
    /** <summary>Simple converter function which will turn a single-value type into a
    multi-value type based on a parameter.</summary>
    */
    static VertexElementType MultiplyTypeCount(VertexElementType baseType, unsigned short count);
    /** <summary>Simple converter function which will a type into it's single-value
    equivalent - makes switches on type easier.</summary>
    */
    static VertexElementType GetBaseType(VertexElementType multiType);

    /** <summary>Utility method for converting colour from
    one packed 32-bit colour type to another.</summary>
    <param name="srcType">The source type</param>
    <param name="dstType">The destination type</param>
    <param name="ptr">Read / write value to change</param>
    */
    static void ConvertColourValue(VertexElementType srcType,
        VertexElementType dstType, uint32% ptr);

    /** <summary>Utility method for converting colour to
    a packed 32-bit colour type.</summary>
    <param name="src">source colour</param>
    <param name="dst">The destination type</param>
    */
    static uint32 ConvertColourValue(ColourValue src,
        VertexElementType dst);

    /** <summary>Utility method to get the most appropriate packed colour vertex element format.</summary> */
    property VertexElementType BestColourVertexElementType
    {
        static VertexElementType get();
    }

    virtual bool Equals(Object^ obj) override
    {
        VertexElement^ clr = dynamic_cast<VertexElement^>(obj);
        if (clr == CLR_NULL)
        {
            return false;
        }

        return (this == clr);
    }

    bool Equals(VertexElement^ obj)
    {
        return (this == obj);
    }

    inline static bool operator== (VertexElement^ lhs, VertexElement^ rhs)
    {
        if ((Object^)lhs == (Object^)rhs) return true;
        if ((Object^)lhs == nullptr || (Object^)rhs == nullptr) return false;

        if (lhs->data.mType != rhs->data.mType ||
            lhs->data.mIndex != rhs->data.mIndex ||
            lhs->data.mOffset != rhs->data.mOffset ||
            lhs->data.mSemantic != rhs->data.mSemantic ||
            lhs->data.mSource != rhs->data.mSource)
            return false;
        else
            return true;

    }
    inline static bool operator!= (VertexElement^ lhs, VertexElement^ rhs)
    {
        return !(lhs == rhs);
    }

    /** <summary>Adjusts a pointer to the base of a vertex to point at this element.</summary>
    <remarks>
    This variant is for void pointers, passed as a parameter because we can't
    rely on covariant return types.
    </remarks>
    <param name="pBase">Pointer to the start of a vertex in this buffer.</param>
    <param name="pElem">Pointer to a pointer which will be set to the start of this element.</param>
    */
    inline void BaseVertexPointerToElement(void* pBase, void** pElem)
    {
        // The only way we can do this is to cast to char* in order to use byte offset
        // then cast back to void*.
        *pElem = static_cast<void*>(
            static_cast<unsigned char*>(pBase) + data.mOffset);
    }
    /** <summary>Adjusts a pointer to the base of a vertex to point at this element.</summary>
    <remarks>
    This variant is for float pointers, passed as a parameter because we can't
    rely on covariant return types.
    </remarks>
    <param name="pBase">Pointer to the start of a vertex in this buffer.</param>
    <param name="pElem">Pointer to a pointer which will be set to the start of this element.</param>
    */
    inline void BaseVertexPointerToElement(void* pBase, float** pElem)
    {
        // The only way we can do this is to cast to char* in order to use byte offset
        // then cast back to float*. However we have to go via void* because casting
        // directly is not allowed
        *pElem = static_cast<float*>(
            static_cast<void*>(
            static_cast<unsigned char*>(pBase) + data.mOffset));
    }

    /** <summary>Adjusts a pointer to the base of a vertex to point at this element.</summary>
    <remarks>
    This variant is for RGBA pointers, passed as a parameter because we can't
    rely on covariant return types.
    </remarks>
    <param name="pBase">Pointer to the start of a vertex in this buffer.</param>
    <param name="pElem">Pointer to a pointer which will be set to the start of this element.</param>
    */
    inline void BaseVertexPointerToElement(void* pBase, RGBA** pElem)
    {
        *pElem = static_cast<RGBA*>(
            static_cast<void*>(
            static_cast<unsigned char*>(pBase) + data.mOffset));
    }
    /** <summary>Adjusts a pointer to the base of a vertex to point at this element.</summary>
    <remarks>
    This variant is for char pointers, passed as a parameter because we can't
    rely on covariant return types.
    </remarks>
    <param name="pBase">Pointer to the start of a vertex in this buffer.</param>
    <param name="pElem">Pointer to a pointer which will be set to the start of this element.</param>
    */
    inline void BaseVertexPointerToElement(void* pBase, unsigned char** pElem)
    {
        *pElem = static_cast<unsigned char*>(pBase) + data.mOffset;
    }

    /** <summary>Adjusts a pointer to the base of a vertex to point at this element.</summary>
    <remarks>
    This variant is for ushort pointers, passed as a parameter because we can't
    rely on covariant return types.
    </remarks>
    <param name="pBase">Pointer to the start of a vertex in this buffer.</param>
    <param name="pElem">Pointer to a pointer which will be set to the start of this element.</param>
    */
    inline void BaseVertexPointerToElement(void* pBase, unsigned short** pElem)
    {
        *pElem = static_cast<unsigned short*>(pBase) + data.mOffset;
    }
};
