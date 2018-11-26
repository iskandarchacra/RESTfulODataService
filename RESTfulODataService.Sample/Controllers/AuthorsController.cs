using Microsoft.AspNetCore.Mvc;
using RESTfulODataService.Sample.Data;
using System.Threading.Tasks;

namespace RESTfulODataService.Sample.Controllers
{
    [Route("/authors")]
    public class AuthorsController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IODataService oDataService;

        public AuthorsController(IUnitOfWork unitOfWork, IODataService oDataService)
        {
            this.unitOfWork = unitOfWork;
            this.oDataService = oDataService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var authors = unitOfWork.Authors.GetAll();

            var filteredBooks = await oDataService.FilterAndGetRESTfulResultAsync(authors, Request);

            return Ok(filteredBooks);
        }
    }
}
