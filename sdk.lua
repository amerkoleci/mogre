if not MOGRE_HOME then
	if os.getenv('MOGRE_HOME') then
		MOGRE_HOME = os.getenv('MOGRE_HOME');
	else
		error('You must define the environment variable MOGRE_HOME.')
	end
end

if not os.isfile(MOGRE_HOME .. '/sdk.lua') then
	error('MOGRE_HOME does not point to a valid Mogre SDK.')
end

package.path = MOGRE_HOME .. "/module/?/?.lua;".. package.path

print('Mogre SDK home directory: ' .. MOGRE_HOME)

require 'ext'
require 'mogre'