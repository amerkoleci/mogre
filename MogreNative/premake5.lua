project "MogreNative"
	kind "SharedLib"
	language "C++"
	debugdir (MOGRE_BIN_DIR)
	targetdir (MOGRE_BIN_DIR)

	files {
		"src/*.cpp",
		"src/*.h"
	}

	includedirs  { 
		"src",
		path.join(OGRE_HOME, "include/OGRE"),
	}
	
	defines { "MOGRE_EXPORTS" }
	
	configuration { "Debug"}
		defines { 
			"_DEBUG",
		}
		flags { "Symbols", "FloatFast" }

	configuration { "Release" }
		defines { "NDEBUG" }
		-- optimize "On"

	flags { "FloatFast", "OptimizeSpeed" }
	
	configuration { "vs*", "x32" }
		flags {
			"EnableSSE2"
	}
	
	configuration { "vs*" }
		defines {
			"_CRT_SECURE_NO_WARNINGS",
			"_CRT_SECURE_NO_DEPRECATE",
			"strdup=_strdup",
			"WIN32_LEAN_AND_MEAN",
		}
		
	configuration { "x32", "windows", "debug" }
			links { 
				path.join(OGRE_HOME, "lib", "Debug", "OgreMain_d.lib")
			}

	configuration { "x32", "windows", "release" }
			links { 
				path.join(MOGRE_HOME, "ogre", "lib", "Release", "OgreMain.lib")
			}

	configuration { "x64", "windows", "debug" }
			links { 
				path.join(MOGRE_HOME, "ogre", "lib", "Debug", "OgreMain_d.lib")
			}

	configuration { "x64", "windows", "release" }
			links { 
				path.join(MOGRE_HOME, "ogre", "lib", "Release", "OgreMain.lib")
			}
		
	configuration {} -- reset configuration