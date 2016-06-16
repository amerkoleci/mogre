external "Mogre.Base"
	location (path.join(MOGRE_HOME, "Mogre.Base"))
	uuid "6F7DD672-BBDD-4EED-B207-1BCBEA6D68B4"
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