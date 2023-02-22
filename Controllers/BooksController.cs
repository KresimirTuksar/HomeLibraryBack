
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeLibrary.Models;
using HomeLibrary.Requests;
using Microsoft.OpenApi.Extensions;
using HomeLibrary.Responses;
using Microsoft.Identity.Client;

namespace HomeLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly HomeLibraryDbContext _context;

        public BooksController(HomeLibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("getAllBooks")]
        public async Task<ActionResult<List<BookResponse>>> GetAllBooks()
        {
            if (_context.Books == null)
            {
                return NotFound();
            }
            var query = await _context.Books
                .Include(x => x.Authors)
                .Include(x => x.Genres)
                .ToListAsync();

            var result = new List<BookResponse>();
            foreach (var book in query)
            {
                var bookResponse = new BookResponse
                {
                    Title = book.Title,
                    Isbn = book.Isbn,
                    Authors = ParseAuthors(book.Authors),
                    Genres = ParseGenres(book.Genres)
                };

                result.Add(bookResponse);
            }
            return result;
        }


        [HttpPost]
        [Route("getBooks")]
        public async Task<ActionResult<List<BookResponse>>> GetBooks(GetBookRequest request)
        {

            var query =  await _context.Books
                .Include(x => x.Authors)
                .Include(x => x.Genres)
                .ToListAsync();

            if (!string.IsNullOrEmpty(request.Title))
            {
                query = query.Where(x => x.Title == request.Title).ToList();
            }

            if (!string.IsNullOrEmpty(request.Author))
            {
                query = query.Where(x => x.Authors.Any(x => x.FirstName.ToLower().Contains(request.Author.ToLower()))).ToList();
            }

            var result = new List<BookResponse>();
            foreach (var book in query)
            {
                var bookResponse = new BookResponse
                {
                    Title = book.Title,
                    Isbn = book.Isbn,
                    Authors = ParseAuthors(book.Authors)
                };

                result.Add(bookResponse);
            }

            return result;
        }


        [HttpPost]
        [Route("updateBook")]
        public async Task<IActionResult> PutBook(CreateBookRequest command)
        {
            Book bookEntity;
            bookEntity = await _context.Books.Where(x => x.Isbn == command.Isbn).FirstOrDefaultAsync();

            if (bookEntity is null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(command.Title)) 
            {
                bookEntity.Title = command.Title;
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpPost]
        [Route("insertBook")]
        public async Task<ActionResult<Book>> PostBook(CreateBookRequest request)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'HomeLibraryDbContext.Books'  is null.");
            }

            if (_context.Authors == null)
            {
                return Problem("Entity set 'HomeLibraryDbContext.Authors'  is null.");
            }

            var existingBook = _context.Books.FirstOrDefault(a => a.Isbn == request.Isbn);
            if (existingBook == null)
            {
                var book = new Book
                {
                    Id = new Guid(),
                    Title = request.Title,
                    Isbn = request.Isbn,
                    Authors = new(),
                    Genres = new(),
                };
                foreach (var author in request.Authors)
                {
                    var bookAuthor = new Author
                    {
                        FirstName = author.FirstName,
                        LastName = author.LastName,
                        BirthDate = author.BirthDate,
                        AuthorNormalized = author.FirstName.ToLower() + author.LastName.ToLower() + author.BirthDate.ToString(),
                        Books = new List<Book> { book },
                    };

                    var existingAuthor = _context.Authors.Include(x => x.Books).FirstOrDefault(a => a.AuthorNormalized == bookAuthor.AuthorNormalized);

                    if (existingAuthor is not null)
                    {
                        existingAuthor.Books.Add(book);
                    }
                    else
                    {
                        _context.Authors.Add(bookAuthor);
                    }

                }

                foreach (var genre in request.Genres)
                {
                    var bookGenre = new Genre
                    {
                        Name = genre,
                        Books = new List<Book> { book },
                    };
                    _context.Genres.Add(bookGenre);
                }

                _context.Books.Add(book);
                await _context.SaveChangesAsync();


                return Ok();

            }
            else
            {
                return BadRequest("Book Exists");
            }
        }

        // DELETE: api/Books/5
        //[HttpDelete("{id}")]
        //[Route("deleteBook")]
        //public async Task<IActionResult> DeleteBook(Guid id)
        //{
        //    if (_context.Books == null)
        //    {
        //        return NotFound();
        //    }
        //    var book = await _context.Books.FindAsync(id);
        //    if (book == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Books.Remove(book);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool BookExists(Guid id)
        //{
        //    return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        //}


        //HELPER METHODS
        private List<string> ParseAuthors(List<Author> authors)
        {
            var parsedAuthors = new List<string>();
            foreach (var author in authors)
            {
                parsedAuthors.Add(author.FirstName + " " + author.LastName);
            }
            return parsedAuthors;
        }

        private List<string> ParseGenres(List<Genre> genres)
        {
            var parsedGenres = new List<string>();
            foreach (var genre in genres)
            {
                parsedGenres.Add(genre.Name);
            }
            return parsedGenres;
        }

    }
}
