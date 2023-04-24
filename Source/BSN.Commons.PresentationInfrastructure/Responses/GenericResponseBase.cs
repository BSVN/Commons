namespace BSN.Commons.Responses
{
    public class GenericResponseBase<T> : Response where T : class
    {
        public T Data { get; set; }

        public GenericResponseBase() { }
    }
}
