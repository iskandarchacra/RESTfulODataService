using Microsoft.AspNetCore.Mvc;
using RESTfulODataService.Sample.Data;
using System.Threading.Tasks;

namespace RESTfulODataService.Sample.Controllers
{
    [Route("/readers")]
    public class ReadersController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IODataService oDataService;

        public ReadersController(IUnitOfWork unitOfWork, IODataService oDataService)
        {
            this.unitOfWork = unitOfWork;
            this.oDataService = oDataService;
        }

        public async Task<IActionResult> GetAll()
        {
            var readers = unitOfWork.Readers.GetAll();

            var filteredReaders = await oDataService.FilterAndGetRESTfulResultAsync(readers, Request);

            return Ok(filteredReaders);
        }
    }
}
