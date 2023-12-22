using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BSN.Commons.Responses
{
    /// <summary>
    /// Collection schema for generating responses..
    /// </summary>
    /// <typeparam name="T">Elements type.</typeparam>
    [DataContract]
    public class CollectionViewModel<T>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public CollectionViewModel() { }

        /// <summary>
        /// Elements.
        /// </summary>
        [DataMember(Order = 1)]
        public IEnumerable<T> Items { get; set; }
    }
}
