#include "stdafx.h"
#include "MogreDataStream.h"
using namespace Mogre;

String^ DataStream::AsString::get()
{
	return TO_CLR_STRING(_native->getAsString());
}

String^ DataStream::Name::get()
{
	return TO_CLR_STRING(_native->getName());
}

size_t DataStream::Read(void* buf, size_t count)
{
	return _native->read(buf, count);
}

size_t DataStream::ReadLine([Out] char% buf, size_t maxCount, String^ delim)
{
	pin_ptr<char> p_buf = &buf;
	DECLARE_NATIVE_STRING(o_delim, delim);

	return _native->readLine(p_buf, maxCount, o_delim);
}

size_t DataStream::ReadLine([Out] char% buf, size_t maxCount)
{
	pin_ptr<char> p_buf = &buf;

	return _native->readLine(p_buf, maxCount);
}

String^ DataStream::GetLine(bool trimAfter)
{
	return TO_CLR_STRING(_native->getLine(trimAfter));
}
String^ DataStream::GetLine()
{
	return TO_CLR_STRING(_native->getLine());
}

size_t DataStream::SkipLine(String^ delim)
{
	DECLARE_NATIVE_STRING(o_delim, delim);

	return _native->skipLine(o_delim);
}

size_t DataStream::SkipLine()
{
	return _native->skipLine();
}

void DataStream::Skip(long count)
{
	_native->skip(count);
}

void DataStream::Seek(size_t pos)
{
	_native->seek(pos);
}

size_t DataStream::Tell()
{
	return _native->tell();
}

bool DataStream::Eof()
{
	return _native->eof();
}

size_t DataStream::Size()
{
	return _native->size();
}

void DataStream::Close()
{
	_native->close();
}

// MemoryDataStream
MemoryDataStream::MemoryDataStream(void* pMem, size_t size) : DataStream((Ogre::DataStream*) 0)
{
	_createdByCLR = true;
	_native = new Ogre::MemoryDataStream(pMem, size);
}

MemoryDataStream::MemoryDataStream(String^ name, void* pMem, size_t size) : DataStream((Ogre::DataStream*) 0)
{
	_createdByCLR = true;
	DECLARE_NATIVE_STRING(o_name, name);

	_native = new Ogre::MemoryDataStream(o_name, pMem, size);
}

MemoryDataStream::MemoryDataStream(Mogre::DataStream^ sourceStream) : DataStream((Ogre::DataStream*) 0)
{
	_createdByCLR = true;
	_native = new Ogre::MemoryDataStream(sourceStream);
}

MemoryDataStream::MemoryDataStream(Mogre::DataStreamPtr^ sourceStream) : DataStream((Ogre::DataStream*) 0)
{
	_createdByCLR = true;
	_native = new Ogre::MemoryDataStream((Ogre::DataStreamPtr&)sourceStream);
}

MemoryDataStream::MemoryDataStream(String^ name, Mogre::DataStream^ sourceStream) : DataStream((Ogre::DataStream*) 0)
{
	_createdByCLR = true;
	DECLARE_NATIVE_STRING(o_name, name);

	_native = new Ogre::MemoryDataStream(o_name, sourceStream);
}

MemoryDataStream::MemoryDataStream(String^ name, Mogre::DataStreamPtr^ sourceStream) : DataStream((Ogre::DataStream*) 0)
{
	_createdByCLR = true;
	DECLARE_NATIVE_STRING(o_name, name);

	_native = new Ogre::MemoryDataStream(o_name, (const Ogre::DataStreamPtr&)sourceStream);
}

MemoryDataStream::MemoryDataStream(size_t size) : DataStream((Ogre::DataStream*) 0)
{
	_createdByCLR = true;
	_native = new Ogre::MemoryDataStream(size);
}

MemoryDataStream::MemoryDataStream(String^ name, size_t size) : DataStream((Ogre::DataStream*) 0)
{
	_createdByCLR = true;
	DECLARE_NATIVE_STRING(o_name, name);

	_native = new Ogre::MemoryDataStream(o_name, size);
}

Ogre::uchar* MemoryDataStream::CurrentPtr::get()
{
	return static_cast<Ogre::MemoryDataStream*>(_native)->getCurrentPtr();
}

Ogre::uchar* MemoryDataStream::Ptr::get()
{
	return static_cast<Ogre::MemoryDataStream*>(_native)->getPtr();
}

size_t MemoryDataStream::Read(void* buf, size_t count)
{
	return static_cast<Ogre::MemoryDataStream*>(_native)->read(buf, count);
}

size_t MemoryDataStream::ReadLine([Out] char% buf, size_t maxCount, String^ delim)
{
	pin_ptr<char> p_buf = &buf;
	DECLARE_NATIVE_STRING(o_delim, delim);

	return static_cast<Ogre::MemoryDataStream*>(_native)->readLine(p_buf, maxCount, o_delim);
}

size_t MemoryDataStream::ReadLine([Out] char% buf, size_t maxCount)
{
	pin_ptr<char> p_buf = &buf;

	return static_cast<Ogre::MemoryDataStream*>(_native)->readLine(p_buf, maxCount);
}

size_t MemoryDataStream::SkipLine(String^ delim)
{
	DECLARE_NATIVE_STRING(o_delim, delim);

	return static_cast<Ogre::MemoryDataStream*>(_native)->skipLine(o_delim);
}

size_t MemoryDataStream::SkipLine()
{
	return static_cast<Ogre::MemoryDataStream*>(_native)->skipLine();
}

void MemoryDataStream::Skip(long count)
{
	static_cast<Ogre::MemoryDataStream*>(_native)->skip(count);
}

void MemoryDataStream::Seek(size_t pos)
{
	static_cast<Ogre::MemoryDataStream*>(_native)->seek(pos);
}

size_t MemoryDataStream::Tell()
{
	return static_cast<const Ogre::MemoryDataStream*>(_native)->tell();
}

bool MemoryDataStream::Eof()
{
	return static_cast<const Ogre::MemoryDataStream*>(_native)->eof();
}

void MemoryDataStream::Close()
{
	static_cast<Ogre::MemoryDataStream*>(_native)->close();
}

void MemoryDataStream::SetFreeOnClose(bool free)
{
	static_cast<Ogre::MemoryDataStream*>(_native)->setFreeOnClose(free);
}

CPP_DECLARE_STLLIST(, DataStreamList, Mogre::DataStreamPtr^, Ogre::DataStreamPtr);