#include "stdafx.h"
#include "MogreGpuProgramManager.h"

using namespace Mogre;

CPP_DECLARE_STLSET(GpuProgramManager::, SyntaxCodes, String^, Ogre::String);

bool GpuProgram::HasCompileError::get()
{
	return static_cast<const Ogre::GpuProgram*>(_native)->hasCompileError();
}

bool GpuProgram::HasDefaultParameters::get()
{
	return static_cast<const Ogre::GpuProgram*>(_native)->hasDefaultParameters();
}

bool GpuProgram::IsMorphAnimationIncluded::get()
{
	return static_cast<const Ogre::GpuProgram*>(_native)->isMorphAnimationIncluded();
}

bool GpuProgram::IsPoseAnimationIncluded::get()
{
	return static_cast<const Ogre::GpuProgram*>(_native)->isPoseAnimationIncluded();
}

bool GpuProgram::IsSkeletalAnimationIncluded::get()
{
	return static_cast<const Ogre::GpuProgram*>(_native)->isSkeletalAnimationIncluded();
}

bool GpuProgram::IsSupported::get()
{
	return static_cast<const Ogre::GpuProgram*>(_native)->isSupported();
}

bool GpuProgram::IsVertexTextureFetchRequired::get()
{
	return static_cast<const Ogre::GpuProgram*>(_native)->isVertexTextureFetchRequired();
}

String^ GpuProgram::Language::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::GpuProgram*>(_native)->getLanguage());
}

Ogre::ushort GpuProgram::NumberOfPosesIncluded::get()
{
	return static_cast<const Ogre::GpuProgram*>(_native)->getNumberOfPosesIncluded();
}

bool GpuProgram::PassSurfaceAndLightStates::get()
{
	return static_cast<const Ogre::GpuProgram*>(_native)->getPassSurfaceAndLightStates();
}

String^ GpuProgram::Source::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::GpuProgram*>(_native)->getSource());
}

void GpuProgram::Source::set(String^ source)
{
	DECLARE_NATIVE_STRING(o_source, source);

	static_cast<Ogre::GpuProgram*>(_native)->setSource(o_source);
}

String^ GpuProgram::SourceFile::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::GpuProgram*>(_native)->getSourceFile());
}

void GpuProgram::SourceFile::set(String^ filename)
{
	DECLARE_NATIVE_STRING(o_filename, filename);

	static_cast<Ogre::GpuProgram*>(_native)->setSourceFile(o_filename);
}

String^ GpuProgram::SyntaxCode::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::GpuProgram*>(_native)->getSyntaxCode());
}

void GpuProgram::SyntaxCode::set(String^ syntax)
{
	DECLARE_NATIVE_STRING(o_syntax, syntax);

	static_cast<Ogre::GpuProgram*>(_native)->setSyntaxCode(o_syntax);
}

Mogre::GpuProgramType GpuProgram::Type::get()
{
	return (Mogre::GpuProgramType)static_cast<const Ogre::GpuProgram*>(_native)->getType();
}

void GpuProgram::Type::set(Mogre::GpuProgramType t)
{
	static_cast<Ogre::GpuProgram*>(_native)->setType((Ogre::GpuProgramType)t);
}

Mogre::GpuProgram^ GpuProgram::GetBindingDelegate()
{
	return ObjectTable::GetOrCreateObject<Mogre::GpuProgram^>((intptr_t)static_cast<Ogre::GpuProgram*>(_native)->_getBindingDelegate());
}

//Mogre::GpuProgramParametersSharedPtr^ GpuProgram::CreateParameters()
//{
//	return static_cast<Ogre::GpuProgram*>(_native)->createParameters();
//}

void GpuProgram::SetSkeletalAnimationIncluded(bool included)
{
	static_cast<Ogre::GpuProgram*>(_native)->setSkeletalAnimationIncluded(included);
}

void GpuProgram::SetMorphAnimationIncluded(bool included)
{
	static_cast<Ogre::GpuProgram*>(_native)->setMorphAnimationIncluded(included);
}

void GpuProgram::SetPoseAnimationIncluded(Ogre::ushort poseCount)
{
	static_cast<Ogre::GpuProgram*>(_native)->setPoseAnimationIncluded(poseCount);
}

void GpuProgram::SetVertexTextureFetchRequired(bool r)
{
	static_cast<Ogre::GpuProgram*>(_native)->setVertexTextureFetchRequired(r);
}

//Mogre::GpuProgramParametersSharedPtr^ GpuProgram::GetDefaultParameters()
//{
//	return static_cast<Ogre::GpuProgram*>(_native)->getDefaultParameters();
//}

void GpuProgram::ResetCompileError()
{
	static_cast<Ogre::GpuProgram*>(_native)->resetCompileError();
}


Mogre::GpuProgramPtr^ GpuProgramManager::Load(String^ name, String^ groupName, String^ filename, Mogre::GpuProgramType gptype, String^ syntaxCode)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);
	DECLARE_NATIVE_STRING(o_filename, filename);
	DECLARE_NATIVE_STRING(o_syntaxCode, syntaxCode);

	return static_cast<Ogre::GpuProgramManager*>(_native)->load(o_name, o_groupName, o_filename, (Ogre::GpuProgramType)gptype, o_syntaxCode);
}

Mogre::GpuProgramPtr^ GpuProgramManager::LoadFromString(String^ name, String^ groupName, String^ code, Mogre::GpuProgramType gptype, String^ syntaxCode)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);
	DECLARE_NATIVE_STRING(o_code, code);
	DECLARE_NATIVE_STRING(o_syntaxCode, syntaxCode);

	return static_cast<Ogre::GpuProgramManager*>(_native)->loadFromString(o_name, o_groupName, o_code, (Ogre::GpuProgramType)gptype, o_syntaxCode);
}

Mogre::GpuProgramManager::Const_SyntaxCodes^ GpuProgramManager::GetSupportedSyntax()
{
	return static_cast<const Ogre::GpuProgramManager*>(_native)->getSupportedSyntax();
}

bool GpuProgramManager::IsSyntaxSupported(String^ syntaxCode)
{
	DECLARE_NATIVE_STRING(o_syntaxCode, syntaxCode);

	return static_cast<const Ogre::GpuProgramManager*>(_native)->isSyntaxSupported(o_syntaxCode);
}

//Mogre::GpuProgramParametersSharedPtr^ GpuProgramManager::CreateParameters()
//{
//	return static_cast<Ogre::GpuProgramManager*>(_native)->createParameters();
//}

Mogre::GpuProgramPtr^ GpuProgramManager::CreateProgram(String^ name, String^ groupName, String^ filename, Mogre::GpuProgramType gptype, String^ syntaxCode)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);
	DECLARE_NATIVE_STRING(o_filename, filename);
	DECLARE_NATIVE_STRING(o_syntaxCode, syntaxCode);

	return static_cast<Ogre::GpuProgramManager*>(_native)->createProgram(o_name, o_groupName, o_filename, (Ogre::GpuProgramType)gptype, o_syntaxCode);
}

Mogre::GpuProgramPtr^ GpuProgramManager::CreateProgramFromString(String^ name, String^ groupName, String^ code, Mogre::GpuProgramType gptype, String^ syntaxCode)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);
	DECLARE_NATIVE_STRING(o_code, code);
	DECLARE_NATIVE_STRING(o_syntaxCode, syntaxCode);

	return static_cast<Ogre::GpuProgramManager*>(_native)->createProgramFromString(o_name, o_groupName, o_code, (Ogre::GpuProgramType)gptype, o_syntaxCode);
}

//Mogre::ResourcePtr^ GpuProgramManager::Create(String^ name, String^ group, Mogre::GpuProgramType gptype, String^ syntaxCode, bool isManual, Mogre::IManualResourceLoader^ loader)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_group, group);
//	DECLARE_NATIVE_STRING(o_syntaxCode, syntaxCode);
//
//	return static_cast<Ogre::GpuProgramManager*>(_native)->create(o_name, o_group, (Ogre::GpuProgramType)gptype, o_syntaxCode, isManual, loader);
//}

Mogre::ResourcePtr^ GpuProgramManager::Create(String^ name, String^ group, Mogre::GpuProgramType gptype, String^ syntaxCode, bool isManual)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);
	DECLARE_NATIVE_STRING(o_syntaxCode, syntaxCode);

	return static_cast<Ogre::GpuProgramManager*>(_native)->create(o_name, o_group, (Ogre::GpuProgramType)gptype, o_syntaxCode, isManual);
}

Mogre::ResourcePtr^ GpuProgramManager::Create(String^ name, String^ group, Mogre::GpuProgramType gptype, String^ syntaxCode)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);
	DECLARE_NATIVE_STRING(o_syntaxCode, syntaxCode);

	return static_cast<Ogre::GpuProgramManager*>(_native)->create(o_name, o_group, (Ogre::GpuProgramType)gptype, o_syntaxCode);
}

//void GpuProgramManager::_pushSyntaxCode(String^ syntaxCode)
//{
//	DECLARE_NATIVE_STRING(o_syntaxCode, syntaxCode);
//
//	static_cast<Ogre::GpuProgramManager*>(_native)->_pushSyntaxCode(o_syntaxCode);
//}

Mogre::GpuProgramPtr^ GpuProgramManager::GetByName(String^ name, bool preferHighLevelPrograms)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return static_cast<Ogre::GpuProgramManager*>(_native)->getByName(o_name, preferHighLevelPrograms);
}

Mogre::GpuProgramPtr^ GpuProgramManager::GetByName(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return static_cast<Ogre::GpuProgramManager*>(_native)->getByName(o_name);
}