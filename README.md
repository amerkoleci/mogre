# Mogre (.NET Wrapper for Ogre3D).
Mogre Ported to version 2.0

This repository contains new PInvoke based Mogre (Managed Ogre) on Ogre3D version 2.0.

Our approach:
- Premake5 autogeneration build system.
- Separated from Ogre3D and can with ease update.
- Binding stuff similar to BulletSharp or PhysX .NET.
- Full Portable Class Library math library written in C#.
- One .bat click will generate Visual Studio 2010, 2012, 2013 or 2015 projects.
- Can easily update to newest Ogre3D versions.
- Where possible, will try to reflect current Mogre API.
- High Performance and there is a object cache that won't create multiple .NET object as Mogre currently does.
- Can distribute .nuget packages.
