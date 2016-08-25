mogre.project.library "OgreRTShaderSystem"
	kind "SharedLib"
	
	includedirs {
		"include",
		path.join(OGRE_HOME_SRC, "OgreMain/include"),
		path.join(OGRE_HOME_SRC, "OgreMain/build_include"),
	}
	
	files {
		"include/*.h",
		"src/*.cpp",
	}
	
	defines { "OgreRTShaderSystem_EXPORTS" }
	links { "OgreMain" }
            	
        configuration { "windows", "x32", "Release" }
            targetdir (MOGRE_HOME .. "/bin/Release/x86")
            implibdir (OGRE_HOME_SRC .. "/lib/x86/Release")
            
            libdirs {
	        path.join(THIRD_PARTY_DIR, "lib/x86/Release"),
            }
            
        configuration { "windows", "x32", "Debug" }
            targetdir (MOGRE_HOME .. "/bin/Debug/x86")
            implibdir (OGRE_HOME_SRC .. "/lib/x86/Debug")
            
            libdirs {
            	path.join(THIRD_PARTY_DIR, "lib/x86/Debug"),
            }
            
        configuration { "windows", "x64", "Release" }
	    targetdir (MOGRE_HOME .. "/bin/Release/x64")
	    implibdir (OGRE_HOME_SRC .. "/lib/x64/Release")
	    
	    libdirs {
	            path.join(THIRD_PARTY_DIR, "lib/x64/Release"),
            }
	    
	configuration { "windows", "x64", "Debug" }
            targetdir (MOGRE_HOME .. "/bin/Debug/x64")
            implibdir (OGRE_HOME_SRC .. "/lib/x64/Debug")
            
            libdirs {
             	path.join(THIRD_PARTY_DIR, "lib/x64/Debug"),
            }