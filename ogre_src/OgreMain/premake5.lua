project "OgreMain"
	targetname "OgreMain"
        language "C++"
	kind "SharedLib"
	
	includedirs {
		path.join(OGRE_HOME_SRC, "OgreMain/include"),
		path.join(OGRE_HOME_SRC, "OgreMain/build_include"),
	}
	
	files {
		path.join(OGRE_HOME_SRC, "OgreMain/build_include/**.h"),
		path.join(OGRE_HOME_SRC, "OgreMain/include/**.h"),
		path.join(OGRE_HOME_SRC, "OgreMain/src/*.cpp"),
	}
	
	defines         { "OGRE_NONCLIENT_BUILD", "FREEIMAGE_LIB", "_MT", "_USRDLL" }
	
	configuration "vs*"
	        defines         { "_CRT_SECURE_NO_WARNINGS", "UNICODE" }
	        flags           { "NoNativeWChar" }
            	buildoptions    { "/wd4100", "/wd4800" }
            	
        configuration { "windows", "x32", "Release" }
            targetdir (OGRE_HOME_SRC .. "/lib/x86/Release")
            
        configuration { "windows", "x32", "Debug" }
            targetdir (OGRE_HOME_SRC .. "/lib/x86/Debug")
            
        configuration { "windows", "x64", "Release" }
	    targetdir (OGRE_HOME_SRC .. "/lib/x64/Release")
	            
	configuration { "windows", "x64", "Debug" }
            targetdir (OGRE_HOME_SRC .. "/lib/x64/Debug")