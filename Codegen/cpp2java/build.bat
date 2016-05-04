mkdir build 2>nul
mkdir build\doxyxml 2>nul

doxygen.exe ogre.doxygen

%WinDir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe transform.msbuild

