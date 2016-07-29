project "OgreOverlay"
	targetname "OgreOverlay"
        language "C++"
	kind "SharedLib"
	
	includedirs {
		"include",
		path.join(OGRE_HOME_SRC, "OgreMain/include"),
		path.join(OGRE_HOME_SRC, "OgreMain/build_include"),
		path.join(THIRD_PARTY_DIR, "include/freetype2"),
	}
	
	files {
		"include/*.h",
		"src/*.cpp",
	}
	
	defines { "OGRE_OVERLAY_EXPORTS", "_MT", "_USRDLL" }
	links { "OgreMain" }
	
	configuration "Debug"
		flags       { "Symbols" }
		targetsuffix "_d"
	
	configuration "Release"
		defines { "NDEBUG" }
	
	configuration "vs*"
	        defines         { "_CRT_SECURE_NO_WARNINGS" }
            	buildoptions    { "/wd4100", "/wd4800" }
            	characterset "MBCS"
            	
        configuration { "windows", "x32", "Release" }
            targetdir (MOGRE_HOME .. "/bin/Release/x86")
            implibdir (OGRE_HOME_SRC .. "/lib/x86/Release")
            
            libdirs {
	        path.join(THIRD_PARTY_DIR, "lib/x86/Release"),
            }
            
            links {
	    	"freetype"
            }
            
        configuration { "windows", "x32", "Debug" }
            targetdir (MOGRE_HOME .. "/bin/Debug/x86")
            implibdir (OGRE_HOME_SRC .. "/lib/x86/Debug")
            
            libdirs {
            	path.join(THIRD_PARTY_DIR, "lib/x86/Debug"),
            }
            
            links {
            	"freetyped"
            }
            
        configuration { "windows", "x64", "Release" }
	    targetdir (MOGRE_HOME .. "/bin/Release/x64")
	    implibdir (OGRE_HOME_SRC .. "/lib/x64/Release")
	    
	    libdirs {
	            path.join(THIRD_PARTY_DIR, "lib/x64/Release"),
            }
            
            links {
	    	  "freetype"
            }
	    
	configuration { "windows", "x64", "Debug" }
            targetdir (MOGRE_HOME .. "/bin/Debug/x64")
            implibdir (OGRE_HOME_SRC .. "/lib/x64/Debug")
            
            libdirs {
             	path.join(THIRD_PARTY_DIR, "lib/x64/Debug"),
            }
            
            links {
	         "freetyped"
            }