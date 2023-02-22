namespace HomeLibrary.Requests
{
    public class GetBookRequest
    {
        public long? Isbn { get; set; }
        public string? Title { get; set; }
        public string? Author{ get; set; }
    }
}
