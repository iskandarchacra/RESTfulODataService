using RESTfulODataService.Sample.Data.Repositories;
using RESTfulODataService.Sample.Data.Repositories.Authors;

namespace RESTfulODataService.Sample.Data
{
    public interface IUnitOfWork
    {
        IBookRepository Books { get; }
        IAuthorRepository Authors { get; }
        IReaderRepository Readers { get; }

        int Complete();
    }
}