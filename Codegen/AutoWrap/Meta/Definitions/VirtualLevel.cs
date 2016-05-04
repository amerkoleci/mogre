﻿namespace AutoWrap.Meta
{
    /// <summary>
    /// Denotes the virtual level of a method in a class.
    /// </summary>
    public enum VirtualLevel
    {
        /// <summary>
        /// The method is neither virtual nor abstract
        /// </summary>
        NotVirtual,

        /// <summary>
        /// The method is virtual but not abstract.
        /// </summary>
        Virtual,

        /// <summary>
        /// The method is abstract (a.k.a. pure virtual) and therefore automatically virtual.
        /// </summary>
        Abstract
    }
}