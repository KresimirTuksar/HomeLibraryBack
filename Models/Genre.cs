namespace HomeLibrary.Models
{
    public class Genre
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Book> Books { get; set; }
    }
}
