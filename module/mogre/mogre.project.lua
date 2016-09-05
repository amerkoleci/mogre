mogre.project = {}

mogre.project.library = function(name)
	project(name)
	
	targetname(name)
	language "C++"
	kind "StaticLib"
	
	configuration { "debug" }
		defines { "DEBUG", "_DEBUG" }
		flags { "Symbols" }
		targetsuffix "_d"
	
	configuration { "release" }
		defines { "NDEBUG" }
		optimize "Speed"
		floatingpoint "Fast"
		
	configuration "vs*"
		defines         { "WIN32", "_WINDOWS" }
	        buildoptions {
			"/wd4201", -- warning C4201: nonstandard extension used: nameless struct/union
			"/wd4324", -- warning C4324: '': structure was padded due to alignment specifier
		}
            	characterset "MBCS"
		disablewarnings { "4786" , "4503" , "4251" , "4275" , "4290" , "4661" , "4996" , "4127" , "4100" }

	
	configuration { "windows", "x32" }
		-- Enable SSE2 vector processing
		vectorextensions "SSE2"
		
	configuration { }
end

mogre.project.application = function(name)
	mogre.project.library(name)

	kind "ConsoleApp"
	
	configuration { "vs*" }
		kind "WindowedApp" 
		flags { "WinMain" }
	
end