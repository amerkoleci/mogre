using System;
using System.Collections.Generic;

namespace AutoWrap.Meta
{
    internal class Mogre17
    {
        public static string GetBaseType(TypedefDefinition typedef)
        {
            string baseTypeName = typedef.BaseTypeName;

            if (string.IsNullOrEmpty(baseTypeName))
                return baseTypeName;

            int charPos = baseTypeName.IndexOf("<");
            string ogreTypeDef = charPos == -1 ? baseTypeName : baseTypeName.Substring(0, charPos);

            if (IsStdCollection(ogreTypeDef))
                return "std::" + ogreTypeDef;

            if (ogreTypeDef.Equals("HashedVector", StringComparison.Ordinal))
                return ogreTypeDef;

            return baseTypeName;
        }

        private static readonly IEnumerable<string> s_StdHacks = new[]
        {
            "vector", "set", "deque", "list", "map", "multimap", "hash_map", "pair"
        };

        public static bool IsStdCollection(string baseTypeName)
        {
            foreach (var thing in s_StdHacks)
            {
                if (baseTypeName.Equals(thing, StringComparison.Ordinal))
                    return true;
            }

            return false;
        }

        public static bool IsCollection(string baseTypeName)
        {
            return IsStdCollection(baseTypeName) ||
                   baseTypeName.Equals("HashedVector", StringComparison.Ordinal);
        }
    }
}