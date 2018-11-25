using Microsoft.AspNetCore.Mvc;
using RESTfulODataService.Sample.Data;
using System.Threading.Tasks;

namespace RESTfulODataService.Sample.Controllers
{
    [Route("/books")]
    public class BooksController : Controller
    {
        private readonly IODataService oDataService;
        private readonly IUnitOfWork unitOfWork;

        public BooksController(IUnitOfWork unitOfWork, IODataService oDataService)
        {
            this.oDataService = oDataService;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = unitOfWork.Books.GetAll();

            var filteredBooks = await oDataService.FilterAndGetRESTfulResultAsync(books, Request);

            return Ok(filteredBooks);
        }
    }
}
