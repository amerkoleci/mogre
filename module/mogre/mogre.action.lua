mogre.action = {}

local function iscygwin()
	return string.startswith(os.getenv('OSTYPE'), 'CYGWIN')
end

local function gettargetdir()
	if os.is('windows') and not iscygwin() then
		if string.startswith(_ACTION, "gmake") then
			return '$(subst /,\\,$(TARGETDIR))'
		else
			return '$(TargetDir)'
		end
	else
		if (_ACTION == "xcode-osx") then
			return '${TARGET_BUILD_DIR}'
		elseif (_ACTION == "xcode-ios") then
			return '${TARGET_BUILD_DIR}/${TARGET_NAME}.app'
		else
			return '${TARGETDIR}'
		end
	end
end

mogre.action.fail = function(target)
	if not target then
		if os.is('windows') and not string.startswith(_ACTION, "gmake") then
			target = '$(Target)'
		else
			target = '${TARGET}'
		end
	end

	if os.is('windows') then
		return 'call "' .. path.translate(mogre.sdk.path('/tool/win/script/fail.bat')) .. '" "' .. target .. '"'
	else
		return 'bash ' .. mogre.sdk.path('/tool/lin/script/fail.sh') .. ' "' .. target .. '"'
	end
end

mogre.action.copy = function(sourcePath, copySub)
	
	if os.is('windows') then
		sourcePath = path.translate(sourcePath)

		local targetDir = string.startswith(_ACTION, "gmake") and '$(subst /,\\,$(TARGETDIR))' or '$(TargetDir)'

		if os.isdir(sourcePath) then
			targetDir = targetDir .. '\\' .. path.getbasename(sourcePath)
		end

		local existenceTest = string.find(sourcePath, '*') and '' or ('if exist "' .. sourcePath .. '" ')

		if copySub then
			return existenceTest .. 'xcopy /y /i /e "' .. sourcePath .. '" "' .. targetDir .. '"'
		else
			return existenceTest .. 'xcopy /y /i "' .. sourcePath .. '" "' .. targetDir .. '"'
		end
	elseif os.is("macosx") then
		local targetDir = '${TARGETDIR}'

		if (_ACTION == "xcode-osx") then
			targetDir = '${TARGET_BUILD_DIR}'
		elseif (_ACTION == "xcode-ios") then
			targetDir = '${TARGET_BUILD_DIR}/${TARGET_NAME}.app'
		end

		local existenceTest = string.find(sourcePath, '*') and '' or ('test -e ' .. sourcePath .. ' && ')

		return existenceTest .. 'cp -R ' .. sourcePath .. ' "' .. targetDir .. '" || :'
	else
		local targetDir = '${TARGETDIR}'

		local existenceTest = string.find(sourcePath, '*') and '' or ('test -e ' .. sourcePath .. ' && ')

		return existenceTest .. 'cp -R ' .. sourcePath .. ' "' .. targetDir .. '" || :'
	end
end

mogre.action.link = function(sourcePath)
	if os.is('windows') then
		-- fixme: not needed yet
	else
		local targetDir = string.startswith(_ACTION, "xcode") and '${TARGET_BUILD_DIR}/${TARGET_NAME}.app' or '${TARGETDIR}'

		return 'test -e ' .. sourcePath .. ' && ln -s -f ' .. sourcePath .. ' "' .. targetDir .. '" || :'
	end
end

mogre.action.clean = function()
	if not os.isfile("sdk.lua") then
		error("cannot clean from outside the Mogre SDK")
	end

	local cmd = 'git clean -X -d -f'

	os.execute(cmd)

	for _, pattern in ipairs { "framework", "plugin/*", "test", "example/*" } do
		local dirs = os.matchdirs(pattern)

		for _, dir in ipairs(dirs) do
			local cwd = os.getcwd()
			os.chdir(dir)
			os.execute(cmd)
			os.chdir(cwd)
		end
	end
end

mogre.action.zip = function(directory, archive)
	if os.is('windows') then
		os.execute('7za a "' .. archive .. '" "' .. path.translate(directory) .. '"')
	else
		os.execute('zip -r "' .. archive .. '" "' .. directory .. '"')
	end
end

--
-- Allows copying directories.
-- It uses the premake4 patterns (**=recursive match, *=file match)
-- NOTE: It won't copy empty directories!
-- Example: we have a file: src/test.h
--	os.copydir("src", "include") simple copy, makes include/test.h
--	os.copydir("src", "include", "*.h") makes include/test.h
--	os.copydir(".", "include", "src/*.h") makes include/src/test.h
--	os.copydir(".", "include", "**.h") makes include/src/test.h
--	os.copydir(".", "include", "**.h", true) will force it to include dir, makes include/test.h
--
-- @param src_dir
--    Source directory, which will be copied to dst_dir.
-- @param dst_dir
--    Destination directory.
-- @param filter
--    Optional, defaults to "**". Only filter matches will be copied. It can contain **(recursive) and *(filename).
-- @param single_dst_dir
--    Optional, defaults to false. Allows putting all files to dst_dir without subdirectories.
--    Only useful with recursive (**) filter.
-- @returns
--    True if successful, otherwise nil.
--
mogre.action.copydir = function(src_dir, dst_dir, filter, single_dst_dir)
	if not os.isdir(src_dir) then error(src_dir .. " is not an existing directory!") end
	filter = filter or "**"
	src_dir = src_dir .. "/"
	print('copy "' .. src_dir .. filter .. '" to "' .. dst_dir .. '".')
	dst_dir = dst_dir .. "/"
	local dir = path.rebase(".",path.getabsolute("."), src_dir) -- root dir, relative from src_dir
 
	os.chdir( src_dir ) -- change current directory to src_dir
		local matches = os.matchfiles(filter)
	os.chdir( dir ) -- change current directory back to root
 
	local counter = 0
	for k, v in ipairs(matches) do
		local target = iif(single_dst_dir, path.getname(v), v)
		--make sure, that directory exists or os.copyfile() fails
		os.mkdir( path.getdirectory(dst_dir .. target))
		if os.copyfile( src_dir .. v, dst_dir .. target) then
			counter = counter + 1
		end
	end
 
	if counter == #matches then
		print( counter .. " files copied.")
		return true
	else
		print( "Error: " .. counter .. "/" .. #matches .. " files copied.")
		return nil
	end
end