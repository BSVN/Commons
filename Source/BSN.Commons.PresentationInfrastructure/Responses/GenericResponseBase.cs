using System;

namespace BSN.Commons.Responses
{
    /// <summary>
    /// Generic base response. 
    /// </summary>
    /// <typeparam name="T"> The type of object or value handled by the class. </typeparam>
    [Obsolete("Due to incompatability with Grpc this response type is only used for backward compatibility.")]
    public class GenericResponseBase<T> : Response where T : class
    {
        public GenericResponseBase() { }

        public T Data { get; set; }
    }
}
