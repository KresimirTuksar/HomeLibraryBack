namespace HomeLibrary.Requests
{
    public class GetBookRequest
    {
        public int Page { get; set; }
        public int Pagesize { get; set; }
        public long? Isbn { get; set; }
        public string? Title { get; set; }
        public string? Author{ get; set; }
    }
}
