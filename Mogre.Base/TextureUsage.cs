// Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Mogre
{
	public enum TextureUsage
	{
		TU_STATIC = 1,
		TU_DYNAMIC = 2,
		TU_WRITE_ONLY = 4,
		TU_STATIC_WRITE_ONLY = 5,
		TU_DYNAMIC_WRITE_ONLY = 5,
		TU_DYNAMIC_WRITE_ONLY_DISCARDABLE = 14,
		TU_AUTOMIPMAP = 16,
		TU_RENDERTARGET = 32,
		TU_DEFAULT = 21,
		Static = TU_STATIC,
		Dynamic = TU_DYNAMIC,
		WriteOnly = TU_WRITE_ONLY,
		StaticWriteOnly = TU_STATIC_WRITE_ONLY,
		DynamicWriteOnly = TU_DYNAMIC_WRITE_ONLY,
		DynamicWriteOnlyDiscardable = TU_DYNAMIC_WRITE_ONLY_DISCARDABLE,
		AutoMipMap = TU_AUTOMIPMAP,
		RenderTarget = TU_RENDERTARGET,
		Default = TU_DEFAULT
	}
}
