using RESTfulODataService.Sample.Data.Repositories;

namespace RESTfulODataService.Sample.Data
{
    public interface IUnitOfWork
    {
        IBookRepository Books { get; }

        IReaderRepository Readers { get; }

        int Complete();
    }
}