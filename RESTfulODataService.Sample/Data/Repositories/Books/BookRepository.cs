using RESTfulODataService.Sample.Models;
using System.Linq;

namespace RESTfulODataService.Sample.Data.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext context;

        public BookRepository(LibraryDbContext context)
        {
            this.context = context;
        }

        public IQueryable<BookModel> GetAll()
        {
            return context.Books;
        }

        public IQueryable<BookModel> GetAllInAuthor(string authorId)
        {
            return context.Books
                .Where(b => b.AuthorId == authorId);
        }

        public IQueryable<BookModel> GetAllInReader(string readerId)
        {
            return context.Books
                .Where(f => f.BookReaders.Any(x => x.ReaderId == readerId));
        }
    }
}
