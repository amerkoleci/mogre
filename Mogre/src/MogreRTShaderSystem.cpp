#include "stdafx.h"
#ifdef INCLUDE_RTSHADER_SYSTEM
#include "MogreRTShaderSystem.h"
#include "MogreSceneManager.h"

using namespace Mogre;
using namespace Mogre::RTShader;

/** This class demonstrates basic usage of the RTShader system.
It sub class the material manager listener class and when a target scheme callback
is invoked with the shader generator scheme it tries to create an equivalent shader
based technique based on the default technique of the given material.
*/
class ShaderGeneratorTechniqueResolverListener : public Ogre::MaterialManager::Listener
{
public:

	ShaderGeneratorTechniqueResolverListener(Ogre::RTShader::ShaderGenerator* pShaderGenerator)
	{
		_shaderGenerator = pShaderGenerator;
	}

	/** This is the hook point where shader based technique will be created.
	It will be called whenever the material manager won't find appropriate technique
	that satisfy the target scheme name. If the scheme name is out target RT Shader System
	scheme name we will try to create shader generated technique for it.
	*/
	virtual Ogre::Technique* handleSchemeNotFound(unsigned short schemeIndex,
		const Ogre::String& schemeName, Ogre::Material* originalMaterial, unsigned short lodIndex,
		const Ogre::Renderable* rend)
	{
		if (schemeName != Ogre::RTShader::ShaderGenerator::DEFAULT_SCHEME_NAME)
		{
			return NULL;
		}
		// Case this is the default shader generator scheme.

		// Create shader generated technique for this material.
		bool techniqueCreated = _shaderGenerator->createShaderBasedTechnique(
			originalMaterial->getName(),
			Ogre::MaterialManager::DEFAULT_SCHEME_NAME,
			schemeName);

		if (!techniqueCreated)
		{
			return NULL;
		}
		// Case technique registration succeeded.

		// Force creating the shaders for the generated technique.
		_shaderGenerator->validateMaterial(schemeName, originalMaterial->getName());

		// Grab the generated technique.
		Ogre::Material::TechniqueIterator itTech = originalMaterial->getTechniqueIterator();

		while (itTech.hasMoreElements())
		{
			Ogre::Technique* curTech = itTech.getNext();

			if (curTech->getSchemeName() == schemeName)
			{
				return curTech;
			}
		}

		return NULL;
	}

	virtual bool afterIlluminationPassesCreated(Ogre::Technique* tech)
	{
		if (tech->getSchemeName() == Ogre::RTShader::ShaderGenerator::DEFAULT_SCHEME_NAME)
		{
			Ogre::Material* mat = tech->getParent();
			_shaderGenerator->validateMaterialIlluminationPasses(tech->getSchemeName(), mat->getName(), mat->getGroup());
			return true;
		}
		return false;
	}

	virtual bool beforeIlluminationPassesCleared(Ogre::Technique* tech)
	{
		if (tech->getSchemeName() == Ogre::RTShader::ShaderGenerator::DEFAULT_SCHEME_NAME)
		{
			Ogre::Material* mat = tech->getParent();
			_shaderGenerator->invalidateMaterialIlluminationPasses(tech->getSchemeName(), mat->getName(), mat->getGroup());
			return true;
		}
		return false;
	}

protected:
	Ogre::RTShader::ShaderGenerator* _shaderGenerator;                       // The shader generator instance.
};

ShaderGeneratorTechniqueResolverListener* _materialMgrListener = nullptr;       // Shader generator material manager listener.  

ShaderGenerator::~ShaderGenerator()
{
	this->!ShaderGenerator();
}

ShaderGenerator::!ShaderGenerator()
{
	OnDisposing(this, nullptr);

	if (IsDisposed)
		return;

	// Restore default scheme.
	Ogre::MaterialManager::getSingleton().setActiveScheme(Ogre::MaterialManager::DEFAULT_SCHEME_NAME);

	// Unregister the material manager listener.
	if (_materialMgrListener != nullptr)
	{
		Ogre::MaterialManager::getSingleton().removeListener(_materialMgrListener);
		delete _materialMgrListener;
		_materialMgrListener = nullptr;
	}

	if (_native)
	{
		Ogre::RTShader::ShaderGenerator::destroy();
		_native = 0;
	}
	_singleton = nullptr;

	OnDisposed(this, nullptr);
}

bool ShaderGenerator::IsDisposed::get()
{
	return (_native == nullptr);
}

bool ShaderGenerator::Initialize()
{
	bool result = Ogre::RTShader::ShaderGenerator::initialize();
	return result;
}

void ShaderGenerator::AddSceneManager(Mogre::SceneManager^ manager)
{
	_native->addSceneManager(GetPointerOrNull(manager));

#if OGRE_PLATFORM != OGRE_PLATFORM_ANDROID && OGRE_PLATFORM != OGRE_PLATFORM_NACL && OGRE_PLATFORM != OGRE_PLATFORM_WINRT
	// Setup core libraries and shader cache path.
	Ogre::StringVector groupVector = Ogre::ResourceGroupManager::getSingleton().getResourceGroups();
	Ogre::StringVector::iterator itGroup = groupVector.begin();
	Ogre::StringVector::iterator itGroupEnd = groupVector.end();
	Ogre::String shaderCoreLibsPath;
	Ogre::String shaderCachePath;

	for (; itGroup != itGroupEnd; ++itGroup)
	{
		Ogre::ResourceGroupManager::LocationList resLocationsList = Ogre::ResourceGroupManager::getSingleton().getResourceLocationList(*itGroup);
		Ogre::ResourceGroupManager::LocationList::iterator it = resLocationsList.begin();
		Ogre::ResourceGroupManager::LocationList::iterator itEnd = resLocationsList.end();
		bool coreLibsFound = false;

		// Try to find the location of the core shader lib functions and use it
		// as shader cache path as well - this will reduce the number of generated files
		// when running from different directories.
		for (; it != itEnd; ++it)
		{
			if ((*it)->archive->getName().find("RTShaderLib") != Ogre::String::npos)
			{
				shaderCoreLibsPath = (*it)->archive->getName() + "/cache/";
				shaderCachePath = shaderCoreLibsPath;
				coreLibsFound = true;
				break;
			}
		}
		// Core libs path found in the current group.
		if (coreLibsFound)
			break;
	}

	// Core shader libs not found -> shader generating will fail.
	if (shaderCoreLibsPath.empty())
		return;

#ifdef _RTSS_WRITE_SHADERS_TO_DISK
	// Set shader cache path.
#if OGRE_PLATFORM == OGRE_PLATFORM_APPLE_IOS
	shaderCachePath = Ogre::macCachePath();
#elif OGRE_PLATFORM == OGRE_PLATFORM_APPLE
	shaderCachePath = Ogre::macCachePath() + "/org.ogre3d.RTShaderCache";
#endif
	_native->setShaderCachePath(shaderCachePath);
#endif
#endif
	// Create and register the material manager listener if it doesn't exist yet.
	if (_materialMgrListener == NULL)
	{
		_materialMgrListener = new ShaderGeneratorTechniqueResolverListener(_native);
		Ogre::MaterialManager::getSingleton().addListener(_materialMgrListener);
	}

	if (Ogre::Root::getSingletonPtr()->getRenderSystem()->getCapabilities()->hasCapability(Ogre::RSC_FIXED_FUNCTION) == false)
	{
		// creates shaders for base material BaseWhite using the RTSS
		Ogre::MaterialPtr baseWhite = Ogre::MaterialManager::getSingleton().getByName("BaseWhite", Ogre::ResourceGroupManager::INTERNAL_RESOURCE_GROUP_NAME);
		_native->createShaderBasedTechnique(
			"BaseWhite",
			Ogre::MaterialManager::DEFAULT_SCHEME_NAME,
			Ogre::RTShader::ShaderGenerator::DEFAULT_SCHEME_NAME);
		_native->validateMaterial(Ogre::RTShader::ShaderGenerator::DEFAULT_SCHEME_NAME,
			"BaseWhite");
		if (baseWhite->getNumTechniques() > 1)
		{
			baseWhite->getTechnique(0)->getPass(0)->setVertexProgram(
				baseWhite->getTechnique(1)->getPass(0)->getVertexProgram()->getName());
			baseWhite->getTechnique(0)->getPass(0)->setFragmentProgram(
				baseWhite->getTechnique(1)->getPass(0)->getFragmentProgram()->getName());
		}

		// creates shaders for base material BaseWhiteNoLighting using the RTSS
		_native->createShaderBasedTechnique(
			"BaseWhiteNoLighting",
			Ogre::MaterialManager::DEFAULT_SCHEME_NAME,
			Ogre::RTShader::ShaderGenerator::DEFAULT_SCHEME_NAME);
		_native->validateMaterial(Ogre::RTShader::ShaderGenerator::DEFAULT_SCHEME_NAME,
			"BaseWhiteNoLighting");
		Ogre::MaterialPtr baseWhiteNoLighting = Ogre::MaterialManager::getSingleton().getByName("BaseWhiteNoLighting", Ogre::ResourceGroupManager::INTERNAL_RESOURCE_GROUP_NAME);
		if (baseWhiteNoLighting->getNumTechniques() > 1)
		{
			baseWhiteNoLighting->getTechnique(0)->getPass(0)->setVertexProgram(
				baseWhiteNoLighting->getTechnique(1)->getPass(0)->getVertexProgram()->getName());
			baseWhiteNoLighting->getTechnique(0)->getPass(0)->setFragmentProgram(
				baseWhiteNoLighting->getTechnique(1)->getPass(0)->getFragmentProgram()->getName());
		}
	}
}

Ogre::RTShader::ShaderGenerator* ShaderGenerator::UnmanagedPointer::get()
{
	return _native;
}
#endif