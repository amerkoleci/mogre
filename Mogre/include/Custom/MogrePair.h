#pragma once

namespace Mogre
{
	generic <typename Type1, typename Type2>
	[System::Serializable]
	public value struct Pair
	{
		typedef Type1 first_type;
		typedef Type2 second_type;

		Type1 first;
		Type2 second;

		Pair(Type1 first, Type2 second) : first(first), second(second)
		{
		}
	};
}