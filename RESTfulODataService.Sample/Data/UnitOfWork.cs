using RESTfulODataService.Sample.Data.Repositories;

namespace RESTfulODataService.Sample.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public LibraryDbContext Context { get; }

        public IBookRepository Books { get; private set; }
        public IReaderRepository Readers { get; private set; }

        public UnitOfWork(LibraryDbContext context, IBookRepository books, IReaderRepository readers)
        {
            Context = context;
            this.Books = books;
            this.Readers = readers;
        }

        public int Complete()
        {
            return Context.SaveChanges();
        }
    }
}
