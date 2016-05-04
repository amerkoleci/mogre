using System;

namespace AutoWrap.Meta
{
    /// <summary>
    /// Visibility/protection level.
    /// </summary>
    public enum ProtectionLevel
    {
        Public,
        Private,
        Protected
    }

    public static class ProtectionLevelExtensions
    {
        /// <summary>
        /// Returns the C++/CLI protection/visibility level name for the C++ level name.
        /// </summary>
        public static string GetCLRProtectionName(this ProtectionLevel prot)
        {
            switch (prot)
            {
                case ProtectionLevel.Public:
                    return "public";
                case ProtectionLevel.Protected:
                    return "protected public";
                case ProtectionLevel.Private:
                    return "private";
                default:
                    throw new Exception("Unexpected");
            }
        }

        public static ProtectionLevel ParseProtectionLevel(string level)
        {
            if (level == "")
                return ProtectionLevel.Public;
            
            return (ProtectionLevel) Enum.Parse(typeof (ProtectionLevel), level, true);
        }
    }
}