#include "stdafx.h"
#include "MogreHighLevelGpuProgramManager.h"

using namespace Mogre;


Mogre::GpuNamedConstants_NativePtr HighLevelGpuProgram::ConstantDefinitions::get()
{
	return static_cast<const Ogre::HighLevelGpuProgram*>(_native)->getConstantDefinitions();
}

Mogre::GpuProgramParametersSharedPtr^ HighLevelGpuProgram::CreateParameters()
{
	return static_cast<Ogre::HighLevelGpuProgram*>(_native)->createParameters();
}

Mogre::GpuProgram^ HighLevelGpuProgram::_getBindingDelegate()
{
	return static_cast<Ogre::HighLevelGpuProgram*>(_native)->_getBindingDelegate();
}

HighLevelGpuProgramManager::HighLevelGpuProgramManager() : ResourceManager((Ogre::ResourceManager*) 0)
{
	_createdByCLR = true;
	_native = new Ogre::HighLevelGpuProgramManager();
}

Mogre::HighLevelGpuProgramPtr^ HighLevelGpuProgramManager::CreateProgram(String^ name, String^ groupName, String^ language, Mogre::GpuProgramType gptype)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);
	DECLARE_NATIVE_STRING(o_language, language);

	return static_cast<Ogre::HighLevelGpuProgramManager*>(_native)->createProgram(o_name, o_groupName, o_language, (Ogre::GpuProgramType)gptype);
}