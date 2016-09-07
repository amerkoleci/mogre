external "Miyagi"
	location (path.join(MOGRE_HOME, "Miyagi/Miyagi"))
	uuid "E57299CC-E298-4413-9756-EF3870033EF4"
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