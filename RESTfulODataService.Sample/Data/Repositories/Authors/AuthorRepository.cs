using System.Linq;
using RESTfulODataService.Sample.Models;

namespace RESTfulODataService.Sample.Data.Repositories.Authors
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryDbContext context;

        public AuthorRepository(LibraryDbContext context)
        {
            this.context = context;
        }

        public IQueryable<AuthorModel> GetAll()
        {
            return context.Authors;
        }
    }
}
