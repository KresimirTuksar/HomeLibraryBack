namespace HomeLibrary.Requests
{
    public class CreateBookRequest
    {
        public string Title { get; set; }
        public long Isbn { get; set; }
        public List<AuthorRequest> Authors { get; set; }
        public List<string> Genres { get; set; }
        public string Publisher { get; set; }
    }

}
