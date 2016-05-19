group "Managed"
    	external "Mogre.Framework"
		location (path.join(MOGRE_HOME, "Samples/Mogre.Framework"))
		uuid "F5076FC9-2BB9-4588-B217-BC2B070FFBC2"
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