namespace HomeLibrary.Responses
{
    public class PaginatedResponseModel<T>
    {
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int NumberOfPages { get; set; }
        public int TotalResultCount { get; set; }
        public List<BookResponse>? Books { get; set; }
    }
}
