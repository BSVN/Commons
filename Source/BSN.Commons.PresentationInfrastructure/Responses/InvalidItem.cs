using System.Runtime.Serialization;

namespace BSN.Commons.Responses
{
    /// <summary>
    /// Represents a request validation issue for the API consumers.
    /// </summary>
    [DataContract]
    public class InvalidItem
    {
        public InvalidItem() { }

        /// <summary>
        /// Name of the request item.
        /// </summary>
        [DataMember(Order = 1)]
        public string Name { get; set; }

        /// <summary>
        /// The reason caused the validation issue.
        /// </summary>
        [DataMember(Order = 2)]
        public string Reason { get; set; }
    }
}
