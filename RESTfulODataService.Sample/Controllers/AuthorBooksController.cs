using Microsoft.AspNetCore.Mvc;
using RESTfulODataService.Sample.Data;
using System.Threading.Tasks;

namespace RESTfulODataService.Sample.Controllers
{
    [Route("/authors/{authorId}/books")]
    public class AuthorBooksController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IODataService oDataService;

        public AuthorBooksController(IUnitOfWork unitOfWork, IODataService oDataService)
        {
            this.unitOfWork = unitOfWork;
            this.oDataService = oDataService;
        }

        public async Task<IActionResult> GetAllInAuthor(string authorId)
        {
            var authors = unitOfWork.Books.GetAllInAuthor(authorId);

            var filteredAuthors = await oDataService.FilterAndGetRESTfulResultAsync(authors, Request);

            return Ok(filteredAuthors);
        }
    }
}
