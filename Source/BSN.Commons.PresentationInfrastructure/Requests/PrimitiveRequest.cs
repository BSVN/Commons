using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace BSN.Commons.Requests
{
    /// <summary>
    /// Primitive datatype request aimed to be used on Grpc compatible services.
    /// </summary>
    /// <remarks>
    /// This Wrappers is added due to lake of Primitive type support in Grpc methods
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    [DataContract]
    public class PrimitiveRequest<T>
    {
        /// <summary>
        /// Primitive value.
        /// </summary>
        [DataMember(Order = 1)]
        public T Value;
    }
}
