namespace AutoWrap.Meta
{
    /// <summary>
    /// An entity (type, method, field) that can be renamed.
    /// </summary>
    public interface IRenamable
    {
        /// <summary>
        /// The native name of this entity.
        /// </summary>
        string NativeName { get; }

        /// <summary>
        /// Checks whether this set contains the specified attribute.
        /// </summary>
        /// <typeparam name="T">the attribute to look for (specified by its type)</typeparam>
        /// <remarks>This method is usually implemented by subclassing <see cref="AttributeSet"/>.</remarks>
        bool HasAttribute<T>() where T : AutoWrapAttribute;

        /// <summary>
        /// Returns the attribute with the specified type T.
        /// </summary>
        /// <typeparam name="T">The attribute's type (i.e. the kind of attribute); if this
        /// attribute isn't part of this set a <c>KeyNotFoundException</c> will be thrown.</typeparam>
        /// <remarks>This method is usually implemented by subclassing <see cref="AttributeSet"/>.</remarks>
        T GetAttribute<T>() where T : AutoWrapAttribute;
    }

    public static class IRenamableExtensions
    {
        /// <summary>
        /// Returns the native name for this entity. If this entity has a <see cref="RenameAttribute"/>
        /// attached, the renamed name will be returned.
        /// </summary>
        public static string GetRenameName(this IRenamable renamable)
        {
            return renamable.HasAttribute<RenameAttribute>() ? renamable.GetAttribute<RenameAttribute>().Name
                       : renamable.NativeName;
        }
    }
}