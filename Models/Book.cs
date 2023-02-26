namespace HomeLibrary.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public long Isbn { get; set; }

        public List<Author> Authors { get; set; }
        public List<Genre> Genres { get; set; }
        public Publisher Publisher { get; set; }
    }
}
