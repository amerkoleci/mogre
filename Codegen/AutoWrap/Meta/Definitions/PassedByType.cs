namespace AutoWrap.Meta
{
    /// <summary>
    /// Describes how values are passed. This is used for parameters, return values, and class fields.
    /// </summary>
    public enum PassedByType
    {
        /// <summary>
        /// Passed by copy.
        /// </summary>
        Value,

        /// <summary>
        /// Passed as C reference (<c>&</c>)
        /// </summary>
        Reference,

        /// <summary>
        /// Passed as C pointer (<c>*</c>)
        /// </summary>
        Pointer,

        /// <summary>
        /// Passed as pointer to a pointer (<c>**</c>)
        /// </summary>
        PointerPointer
    }
}