using RESTfulODataService.Sample.Models;
using System.Collections.Generic;
using System.Linq;

namespace RESTfulODataService.Sample.Data.Repositories
{
    public interface IBookRepository
    {
        IQueryable<BookModel> GetAll();

        IQueryable<BookModel> GetAllInAuthor(string authorId);

        IQueryable<BookModel> GetAllInReader(string readerId);
    }
}