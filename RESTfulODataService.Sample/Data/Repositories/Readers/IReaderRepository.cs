using RESTfulODataService.Sample.Models;
using System.Linq;

namespace RESTfulODataService.Sample.Data.Repositories
{
    public interface IReaderRepository
    {
        IQueryable<ReaderModel> GetAll();

        IQueryable<ReaderModel> GetAllInBook(string bookId);
    }
}