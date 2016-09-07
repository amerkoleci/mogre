// Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Mogre
{
	/// <summary>
	/// Helper class for hashing.
	/// </summary>
	public static class HashCodeHelper
	{
		/// <summary>
		/// Combines two hash codes, useful for combining hash codes of individual vector elements
		/// </summary>
		public static int CombineHashCodes(int h1, int h2)
		{
			return (((h1 << 5) + h1) ^ h2);
		}
	}
}
