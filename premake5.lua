--
-- Copyright 2016 Amer Koleci. All rights reserved.
--

newoption {
	trigger = "with-ogre",
	description = "Compile with Ogre",
}

MOGRE_HOME		= path.getabsolute(os.getcwd())
MOGRE_BUILD_DIR   	= MOGRE_HOME .. "/.build/" .. _ACTION .. "/"
MOGRE_BIN_DIR   	= MOGRE_HOME .. "/bin"
OGRE_HOME   		= MOGRE_HOME .. "/ogre"
OGRE_HOME_SRC   	= MOGRE_HOME .. "/ogre_src"
THIRD_PARTY_DIR   	= MOGRE_HOME .. "/ThirdParty"

printf("Premake home " .. MOGRE_HOME)
printf("Build directory " .. MOGRE_BUILD_DIR)

solution "Mogre"
	location (MOGRE_BUILD_DIR)
	language "C++"
	
	configurations {
		"Debug",
		"Release",
	}

	platforms {
		"x32",
		"x64"
	}
	
	startproject "Mogre.SampleBrowser"

if _OPTIONS["with-ogre"] then
	group "Ogre"
		include ("ogre_src/OgreMain")
		include ("ogre_src/Components/Overlay")
		include ("ogre_src/Components/RTShaderSystem")
		
		-- RenderSystems
		include ("ogre_src/RenderSystems/Direct3D9")
	group ""
end

-- MogreNative
include ("MogreNative")

-- Mogre
include ("Mogre.Base")

-- Mogre.Framework
include ("Samples/Mogre.Framework")

-- Now Samples
include ("Samples/Mogre.Sample.SkyBox")
include ("Samples/Mogre.Sample.SkyPlane")
include ("Samples/Mogre.Sample.CubeMapping")
include ("Samples/Mogre.Sample.CameraTrack")

-- Mogre.SampleBrowser
include ("Mogre.SampleBrowser")