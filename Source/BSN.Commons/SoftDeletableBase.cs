using System;
using System.Collections.Generic;
using System.Text;

namespace BSN.Commons
{
    /// <inheritdoc/>
    public abstract class SoftDeletableBase : ISoftDeletable
    {
        /// <inheritdoc/>
        public DateTime DeletedAt
        {
            get;
            private set;
        }

        /// <inheritdoc/>
        public bool IsDeleted => DeletedAt != DateTime.MinValue;

        /// <inheritdoc/>
        public virtual void Delete()
        {
            if (DeletedAt != DateTime.MinValue)
            {
                throw new InvalidOperationException("This object is already deleted.");
            }

            DeletedAt = DateTime.UtcNow;
        }
    }
}
