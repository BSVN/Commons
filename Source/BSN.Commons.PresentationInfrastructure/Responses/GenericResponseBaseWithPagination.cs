namespace BSN.Commons.Responses
{
    /// <summary>
    /// Add pagination meta data for given type to response class. 
    /// </summary>
    /// <typeparam name="T"> The type of object or value handled by the class. </typeparam>
    public class GenericResponseBaseWithPagination<T> : GenericResponseBase<T> where T : class
    {
        public PaginationMetadata Meta { get; set; }
    }
}
