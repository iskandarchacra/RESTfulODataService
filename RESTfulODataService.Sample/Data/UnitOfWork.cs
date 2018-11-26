using RESTfulODataService.Sample.Data.Repositories;
using RESTfulODataService.Sample.Data.Repositories.Authors;

namespace RESTfulODataService.Sample.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public LibraryDbContext Context { get; }

        public IBookRepository Books { get; private set; }
        public IReaderRepository Readers { get; private set; }
        public IAuthorRepository Authors { get; private set; }

        public UnitOfWork(LibraryDbContext context, IBookRepository books, IReaderRepository readers, IAuthorRepository authors)
        {
            Context = context;
            this.Books = books;
            this.Readers = readers;
            this.Authors = authors;
        }

        public int Complete()
        {
            return Context.SaveChanges();
        }
    }
}
