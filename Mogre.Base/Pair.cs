using System;

namespace Mogre
{
    [Serializable]
    public struct Pair<Type1, Type2>
    {
        public Type1 first;
        public Type2 second;

        public Pair(Type1 first, Type2 second)
        {
            this.first = first;
            this.second = second;
        }
    }
}
