#pragma once

#include <gcroot.h>
#pragma warning(push, 0)
#pragma managed(push, off)
#include "OgreDataStream.h"
#pragma managed(pop)
#pragma warning(pop)
#include "MogreDataStream.h"

namespace Mogre
{
    public ref class ManagedDataStream : DataStream
    {
        Ogre::DataStream* _createNative(System::IO::Stream^ stream);

    internal:
        System::IO::Stream^ _stream;

    public:
        ManagedDataStream(System::IO::Stream^ stream) : DataStream( _createNative(stream) )
        {
            _createdByCLR = true;
        }

        property System::IO::Stream^ Stream
        {
            System::IO::Stream^ get()
            {
                return _stream;
            }
        }

    };

    class DataStream_Proxy : public Ogre::DataStream
    {
        gcroot<ManagedDataStream^> _managed;
        long _initPos;

    public:
        DataStream_Proxy( ManagedDataStream^ managed ) : _managed(managed)
        {
            _initPos = managed->_stream->Position;
            mSize = managed->_stream->Length - _initPos;
        }

        ~DataStream_Proxy()
        {
            _managed->_native = 0;
        }

        virtual size_t read(void* buf, size_t count);

        /* Skip a defined number of bytes. This can also be a negative value, in which case
        the file pointer rewinds a defined number of bytes. */
        virtual void skip(long count) ;

        // Repositions the read point to a specified byte.
        virtual void seek( size_t pos );

        // Returns the current byte offset from beginning
        virtual size_t tell(void) const;

        // Returns true if the stream has reached the end.
        virtual bool eof(void) const;

        // Close the stream; this makes further operations invalid.
        virtual void close(void);
    };
}