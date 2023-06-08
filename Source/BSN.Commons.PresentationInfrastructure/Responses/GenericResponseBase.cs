namespace BSN.Commons.Responses
{
    /// <summary>
    /// Generic base response. 
    /// </summary>
    /// <typeparam name="T"> The type of object or value handled by the class. </typeparam>
    public class GenericResponseBase<T> : Response where T : class
    {
        public T Data { get; set; }
    }
}
