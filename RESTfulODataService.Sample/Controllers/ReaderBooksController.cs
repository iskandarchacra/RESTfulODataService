using Microsoft.AspNetCore.Mvc;
using RESTfulODataService.Sample.Data;
using System.Threading.Tasks;

namespace RESTfulODataService.Sample.Controllers
{
    [Route("/readers/{readerId}/books")]
    public class ReaderBooksController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IODataService oDataService;

        public ReaderBooksController(IUnitOfWork unitOfWork, IODataService oDataService)
        {
            this.unitOfWork = unitOfWork;
            this.oDataService = oDataService;
        }

        public async Task<IActionResult> GetAll(string readerId)
        {
            var books = unitOfWork.Books.GetAllInReader(readerId);

            var filteredBooks =  await oDataService.FilterAndGetRESTfulResultAsync(books, Request);

            return Ok(filteredBooks);
        }
    }
}
