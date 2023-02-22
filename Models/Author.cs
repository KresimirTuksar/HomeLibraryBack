namespace HomeLibrary.Models
{
    public class Author
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string? AuthorNormalized { get; set; }
        public List<Book> Books{ get; set; }
    }
            //AuthorNormalized = FirstName.ToLower() + LastName.ToLower() + BirthDate.ToString();
}
