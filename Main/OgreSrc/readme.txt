put the ogre source repository clone here.
Use: hg clone http://bitbucket.org/sinbad/ogre/ -u v1-7
For example, if you installed Mogre under C:\Mogre, you will have a
path with the ogre main sources under:
C:\Mogre\Main\OgreSrc\ogre\OgreMain

Let CMake point to the subdirectory build here, e.g. if you installed
Mogre as C:\Mogre, use this as CMake output build path:
C:\Mogre\Main\OgreSrc\build