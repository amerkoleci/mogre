#include "MogreStableHeaders.h"

#include "Custom\ManagedDataStream.h"

namespace Mogre
{
	Ogre::DataStream* ManagedDataStream::_createNative(System::IO::Stream^ stream)
	{
		_stream = stream;
		return new DataStream_Proxy( this );
	}


	size_t DataStream_Proxy::read(void* buf, size_t count)
	{
		array<System::Byte>^ managedBuf = gcnew array<System::Byte>(count);
		int bytesRead = _managed->_stream->Read(managedBuf, 0, count);

		Marshal::Copy(managedBuf, 0, (IntPtr)buf, bytesRead);

		return bytesRead;
	}


	void DataStream_Proxy::skip(long count)
	{
		_managed->_stream->Seek(count, System::IO::SeekOrigin::Current);
	}


	void DataStream_Proxy::seek( size_t pos )
	{
		_managed->_stream->Seek(_initPos + pos, System::IO::SeekOrigin::Begin);
	}
	

	size_t DataStream_Proxy::tell(void) const
	{
		return _managed->_stream->Position - _initPos;
	}


	bool DataStream_Proxy::eof(void) const
	{
		return _managed->_stream->Position >= _managed->Stream->Length - 1;
	}


	void DataStream_Proxy::close(void)
	{
		_managed->_stream->Close();
	}
}