mogre.project.library "RenderSystem_GL"
	kind "SharedLib"
	
	includedirs {
		path.join(OGRE_HOME_SRC, "OgreMain/include"),
		path.join(OGRE_HOME_SRC, "OgreMain/build_include"),
		path.join(OGRE_HOME_SRC, "OgreMain/include/Threading"),
		"include",
		"src",
		"src/StateCacheManager",
		"src/GLSL/include",
		"src/atifs/include",
		"src/nvparse",
	}
	
	files {
		"include/*.h",
		"src/*.cpp",
		"src/GLSL/include/*.h",
		"src/GLSL/src/*.cpp",
		"src/atifs/include/*.h",
		"src/atifs/src/*.cpp",
		"src/nvparse/*.cpp",
		"src/StateCacheManager/OgreGLNullStateCacheManagerImp.cpp",
		"src/StateCacheManager/OgreGLNullUniformCacheImp.cpp",
	}
	
	excludes { "src/nvparse/ps1.0__test_main.cpp" }
	
	defines { "OGRE_GLPLUGIN_EXPORTS" }
	
	links { "OgreMain" }
            	
        configuration { "windows" }
            	includedirs {
            		"include/Win32",
            		"src/Win32",
            		"src/nvparse/winheaders",
            	}
            	
            	files {
            		"include/Win32/*.h",
            		"src/Win32/*.h",
            		"src/Win32/*.cpp",
            	}
            	
            	links { "OpenGL32", "glu32" }
            	
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