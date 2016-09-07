external "Miyagi.Backend.Mogre"
	location (path.join(MOGRE_HOME, "Miyagi/Backends/Miyagi.Backend.Mogre"))
	uuid "60B1CACE-8B81-426C-9978-A58262DCE461"
	kind "SharedLib"
	language "C#"
	removeplatforms "*"
	platforms { "Any CPU" }
	configmap {
	      ["Release"] = "Release",
	      ["Debug"] = "Debug",
	      ["x32"] = "Any CPU",
	      ["x64"] = "Any CPU"
	}