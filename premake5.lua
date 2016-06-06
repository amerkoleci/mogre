--
-- Copyright 2016 Amer Koleci. All rights reserved.
--

MOGRE_HOME		= path.getabsolute(os.getcwd())
MOGRE_BUILD_DIR   	= MOGRE_HOME .. "/.build/" .. _ACTION .. "/"
MOGRE_BIN_DIR   	= MOGRE_HOME .. "/bin"
OGRE_HOME   		= MOGRE_HOME .. "/ogre"

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

-- Mogre.SampleBrowser
include ("Mogre.SampleBrowser")