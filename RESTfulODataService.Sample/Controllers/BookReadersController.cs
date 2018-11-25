using Microsoft.AspNetCore.Mvc;
using RESTfulODataService.Sample.Data;
using System.Threading.Tasks;

namespace RESTfulODataService.Sample.Controllers
{
    [Route("/books/{bookId}/readers")]
    public class BookReadersController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IODataService oDataService;

        public BookReadersController(IUnitOfWork unitOfWork, IODataService oDataService)
        {
            this.unitOfWork = unitOfWork;
            this.oDataService = oDataService;
        }

        public async Task<IActionResult> GetAll(string bookId)
        {
            var readers = unitOfWork.Readers.GetAllInBook(bookId);

            var filteredReaders = await oDataService.FilterAndGetRESTfulResultAsync(readers, Request);

            return Ok(filteredReaders);
        }
    }
}
