mogre.sdk = {}

mogre.sdk.path = function(p)
	-- note: cannot use path.join() here because `p` can start with '/'
	return path.getabsolute(MOGRE_HOME .. '/' .. p)
end