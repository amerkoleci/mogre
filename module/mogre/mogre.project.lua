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
		defines         { "_CRT_SECURE_NO_WARNINGS" }
	        buildoptions    { "/wd4100", "/wd4800" }
            	characterset "MBCS"

	
	configuration { "windows", "x32" }
		-- Enable SSE2 vector processing
		vectorextensions "SSE2"

	configuration { }
end

mogre.project.application = function(name)
	mogre.project.library(name)

	kind "ConsoleApp"
	
end