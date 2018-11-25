using RESTfulODataService.Extensions;
using RESTfulODataService.Models.RESTful;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RESTfulODataService.Services.Parser;
using RESTfulODataService.Services.Validation.ODataParameters;

namespace RESTfulODataService
{
    public class ODataService : IODataService
    {
        private readonly IODataParser oDataParser;
        private readonly IODataParametersValidator oDataParamsValidator;

        public ODataService(IODataParser oDataParser, IODataParametersValidator oDataParamsValidator)
        {
            this.oDataParser = oDataParser;
            this.oDataParamsValidator = oDataParamsValidator;
        }

        /// <summary>
        /// Filters an IQueryable of IRESTfulItemResult by OData parameters received in the route of the HTTP Request and returns as a RESTfulResult.
        /// </summary>
        /// <typeparam name="T">IRESTfulItemResult.</typeparam>
        /// <param name="itemsList">IQueryable of IRESTfulItemResult.</param>
        /// <param name="httpRequest">HTTP Request with the route containing OData parameters.</param>
        /// <param name="customHrefHelper">Only use if you want to make make the value of the Href property of items inside the collection different from the value of the Href of the entire collection.</param>
        /// <returns></returns>
        public async Task<RESTfulResult<T>> FilterAndGetRESTfulResultAsync<T>(IQueryable<T> itemsList, HttpRequest httpRequest, CustomHrefHelperModel customHrefHelper = null) where T : class, IRESTfulItemResult
        {
            var result = await itemsList.TryFilterWithODataAsync(oDataParamsValidator, oDataParser, httpRequest);

            return result.ToRESTfulResult(httpRequest, customHrefHelper);
        }

        /// <summary>
        /// Filters a List of IRESTfulItemResult by OData parameters received in the route of the HTTP Request and returns as a RESTfulResult.
        /// </summary>
        /// <typeparam name="T">IRESTfulItemResult.</typeparam>
        /// <param name="itemsList">List of IRESTfulItemResult.</param>
        /// <param name="httpRequest">HTTP Request with the route containing OData parameters.</param>
        /// <param name="customHrefHelper">Only use if you want to make make the value of the Href property of items inside the collection different from the value of the Href of the entire collection.</param>
        /// <returns></returns>
        public RESTfulResult<T> FilterAndGetRESTfulResult<T>(List<T> itemsList, HttpRequest httpRequest, CustomHrefHelperModel customHrefHelper = null) where T : class, IRESTfulItemResult
        {
            var result = itemsList.TryFilterWithOData(oDataParamsValidator, oDataParser, httpRequest, out int count);

            return result.ToRESTfulResult(count, httpRequest, customHrefHelper);
        }

        /// <summary>
        /// Filters an IQueryable of IRESTfulItemResult by OData parameters received in the route of the HTTP Request.
        /// </summary>
        /// <typeparam name="T">IRESTfulItemResult.</typeparam>
        /// <param name="itemsList">IQueryable of IRESTfulItemResult.</param>
        /// <param name="httpRequest">HTTP Request with the route containing OData parameters.</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> FilterResultAsync<T>(IQueryable<T> itemsList, HttpRequest httpRequest) where T : class, IRESTfulItemResult
        {
            var (filteredQuery, count) = await itemsList.TryFilterWithODataAsync(oDataParamsValidator, oDataParser, httpRequest);

            return filteredQuery;
        }

        /// <summary>
        /// Filters a List of IRESTfulItemResult by OData parameters received in the route of the HTTP Request.
        /// </summary>
        /// <typeparam name="T">IRESTfulItemResult.</typeparam>
        /// <param name="itemsList">List of IRESTfulItemResult.</param>
        /// <param name="httpRequest">HTTP Request with the route containing OData parameters.</param>
        /// <returns></returns>
        public IEnumerable<T> FilterResult<T>(List<T> itemsList, HttpRequest httpRequest) where T : class, IRESTfulItemResult
        {
            return itemsList.TryFilterWithOData(oDataParamsValidator, oDataParser, httpRequest, out int count);
        }
    }
}
