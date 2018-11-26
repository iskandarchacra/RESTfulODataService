using RESTfulODataService.Sample.Models;
using System.Linq;

namespace RESTfulODataService.Sample.Data.Repositories.Authors
{
    public interface IAuthorRepository
    {
        IQueryable<AuthorModel> GetAll();
    }
}
