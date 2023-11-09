using System;

namespace BSN.Commons
{
    /// <summary>
    /// Provides a mechanism for soft deletion.
    ///
    /// Soft deletion means that this object is not completely destroyed, but it goes in a deleted state,
    /// and you can ask this object if you are deleted or not?
    /// </summary>
    public interface ISoftDeletable
    {
        /// <summary>
        /// Shows the time of deletion.
        /// </summary>
        /// <remarks>
        /// If this object is not deleted, this property returns <see cref="DateTime.MinValue"/>.
        /// </remarks>
        DateTime DeletedAt
        {
            get;
        }

        /// <summary>
        /// Delete this object.
        /// </summary>
        void Delete();
    }
}
