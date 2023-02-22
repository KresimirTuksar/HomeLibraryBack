using HomeLibrary.Models;

namespace HomeLibrary.Responses
{
    public class BookResponse
    {
        public string Title { get; set; }
        public long Isbn { get; set; }

        public List<string> Authors { get; set; }
        public List<string> Genres { get; set; }

    }
}
