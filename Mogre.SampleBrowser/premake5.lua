group "Managed"
    	external "Mogre.SampleBrowser"
		location (path.join(MOGRE_HOME, "Mogre.SampleBrowser"))
		uuid "B6A3EA75-1A57-4035-A765-460A6E3E4159"
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