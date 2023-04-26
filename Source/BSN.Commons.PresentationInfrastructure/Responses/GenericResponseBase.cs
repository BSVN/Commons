namespace BSN.Commons.Responses
{
    /// <summary>
    /// Add data as given type to response class. 
    /// </summary>
    /// <typeparam name="T"> The type of object or value handled by the class. </typeparam>
    public class GenericResponseBase<T> : Response where T : class
    {
        public GenericResponseBase() { }

        public T Data { get; set; }
    }
}
