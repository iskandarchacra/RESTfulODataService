using RESTfulODataService.Sample.Models;
using System.Linq;

namespace RESTfulODataService.Sample.Data.Repositories
{
    public class ReaderRepository : IReaderRepository
    {
        private readonly LibraryDbContext context;

        public ReaderRepository(LibraryDbContext context)
        {
            this.context = context;
        }

        public IQueryable<ReaderModel> GetAll()
        {
            return context.Readers;
        }

        public IQueryable<ReaderModel> GetAllInBook(string bookId)
        {
            return context.Readers
                .Where(f => f.BookReaders.Any(x => x.BookId == bookId));
        }
    }
}
