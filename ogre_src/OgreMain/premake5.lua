project "OgreMain"
	targetname "OgreMain"
        language "C++"
	kind "SharedLib"
	
	includedirs {
		path.join(OGRE_HOME_SRC, "OgreMain/include"),
		path.join(OGRE_HOME_SRC, "OgreMain/build_include"),
		path.join(THIRD_PARTY_DIR, "include"),
		path.join(OGRE_HOME_SRC, "OgreMain/src/nedmalloc"),
	}
	
	files {
		path.join(OGRE_HOME_SRC, "OgreMain/build_include/**.h"),
		path.join(OGRE_HOME_SRC, "OgreMain/include/**.h"),
		path.join(OGRE_HOME_SRC, "OgreMain/src/*.cpp"),
		--path.join(OGRE_HOME_SRC, "OgreMain/src/nedmalloc/nedmalloc.c"),
		path.join(OGRE_HOME_SRC, "OgreMain/src/Animation/*.cpp"),
		path.join(OGRE_HOME_SRC, "OgreMain/src/Compositor/**.cpp"),
		path.join(OGRE_HOME_SRC, "OgreMain/src/Hash/*.cpp"),
		path.join(OGRE_HOME_SRC, "OgreMain/src/Math/Array/*.cpp"),
		path.join(OGRE_HOME_SRC, "OgreMain/src/Math/Simple/C/*.cpp"),
	}
	
	defines { "OGRE_NONCLIENT_BUILD", "FREEIMAGE_LIB", "_MT", "_USRDLL" }
	
	-- No threading
	files {
		path.join(OGRE_HOME_SRC, "OgreMain/src/Threading/OgreDefaultWorkQueueStandard.cpp"),
	}
	
	configuration "Debug"
		flags       { "Symbols" }
		targetsuffix "_d"
		
	configuration "vs*"
	        defines         { "_CRT_SECURE_NO_WARNINGS" }
            	buildoptions    { "/wd4100", "/wd4800" }
            	characterset "MBCS"
          
         configuration { "windows" }
       		files {
       			path.join(OGRE_HOME_SRC, "OgreMain/src/WIN32/OgreConfigDialog.cpp"),
       			path.join(OGRE_HOME_SRC, "OgreMain/src/WIN32/OgreErrorDialog.cpp"),
       			path.join(OGRE_HOME_SRC, "OgreMain/src/WIN32/OgreFileSystemLayer.cpp"),
       			path.join(OGRE_HOME_SRC, "OgreMain/src/WIN32/OgreTimer.cpp"),
       			path.join(OGRE_HOME_SRC, "OgreMain/src/WIN32/OgreOptimisedUtilDirectXMath.cpp"),
       			path.join(OGRE_HOME_SRC, "OgreMain/src/Threading/OgreBarrierWin.cpp"),
			path.join(OGRE_HOME_SRC, "OgreMain/src/Threading/OgreLightweightMutexWin.cpp"),
      			path.join(OGRE_HOME_SRC, "OgreMain/src/Threading/OgreThreadsWin.cpp"),
      			path.join(OGRE_HOME_SRC, "OgreMain/src/Math/Array/SSE2/Single/*.cpp"),
       		}  
          
          
        configuration { "windows", "x32", "Release" }
            targetdir (MOGRE_HOME .. "/bin/Release/x86")
            implibdir (OGRE_HOME_SRC .. "/lib/x86/Release")
            
            libdirs {
            	path.join(THIRD_PARTY_DIR, "lib/x86/Release"),
            }
            
            links {
	              "zlib",
	              "zziplib",
	              "FreeImageLib",
            }
            
        configuration { "windows", "x32", "Debug" }
            targetdir (MOGRE_HOME .. "/bin/Debug/x86")
            implibdir (OGRE_HOME_SRC .. "/lib/x86/Debug")
            
            libdirs {
	            path.join(THIRD_PARTY_DIR, "lib/x86/Debug"),
            }
            
            links {
            	"zlibd",
            	"zziplibd",
            	"FreeImageLibd",
            }
            
        configuration { "windows", "x64", "Release" }
	    targetdir (MOGRE_HOME .. "/bin/Release/x64")
	    implibdir (OGRE_HOME_SRC .. "/lib/x64/Release")
	    
	    libdirs {
	            path.join(THIRD_PARTY_DIR, "lib/x64/Release"),
            }
            
            links {
		      "zlib",
		      "zziplib",
		      "FreeImageLib",
            }
	    
	configuration { "windows", "x64", "Debug" }
            targetdir (MOGRE_HOME .. "/bin/Debug/x64")
            implibdir (OGRE_HOME_SRC .. "/lib/x64/Debug")
            
            libdirs {
             	path.join(THIRD_PARTY_DIR, "lib/x64/Debug"),
            }
            
            links {
		"zlibd",
		"zziplibd",
		"FreeImageLibd",
            }