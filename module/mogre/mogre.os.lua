mogre.os = {}

mogre.os.copyfiles = function(src, dst)
  assert(os.isdir(src), 'bad argument #1 to mogre.os.copyfiles "' .. src .. '" (directory expected)')
  assert(os.isdir(dst), 'bad argument #2 to mogre.os.copyfiles "' .. dst .. ' (directory expected)')
  local list = os.matchfiles(path.join(src, '**'))
  
  for _, v in pairs(list) do
	local dir = path.join(dst, path.getrelative(src, path.getdirectory(v)))
	
	if not os.isdir(dir) then
		os.mkdir(dir)
	end
	
    os.copyfile(v, dir .. '/' .. path.getname(v))
  end
end