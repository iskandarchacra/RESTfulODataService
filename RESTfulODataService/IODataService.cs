using RESTfulODataService.Models.RESTful;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTfulODataService
{
    public interface IODataService
    {
        /// <summary>
        /// Filters an IQueryable of IRESTfulItemResult by OData parameters received in the route of the HTTP Request and returns as a RESTfulResult.
        /// </summary>
        /// <typeparam name="T">IRESTfulItemResult.</typeparam>
        /// <param name="itemsList">IQueryable of IRESTfulItemResult.</param>
        /// <param name="httpRequest">HTTP Request with the route containing OData parameters.</param>
        /// <param name="customHrefHelper">Only use if you want to make make the value of the Href property of items inside the collection different from the value of the Href of the entire collection.</param>
        /// <returns></returns>
        Task<RESTfulResult<T>> FilterAndGetRESTfulResultAsync<T>(IQueryable<T> itemsList, HttpRequest httpRequest, CustomHrefHelperModel customHrefHelper = null) where T : class, IRESTfulItemResult;

        /// <summary>
        /// Filters a List of IRESTfulItemResult by OData parameters received in the route of the HTTP Request and returns as a RESTfulResult.
        /// </summary>
        /// <typeparam name="T">IRESTfulItemResult.</typeparam>
        /// <param name="itemsList">List of IRESTfulItemResult.</param>
        /// <param name="httpRequest">HTTP Request with the route containing OData parameters.</param>
        /// <param name="customHrefHelper">Only use if you want to make make the value of the Href property of items inside the collection different from the value of the Href of the entire collection.</param>
        /// <returns></returns>
        RESTfulResult<T> FilterAndGetRESTfulResult<T>(List<T> itemsList, HttpRequest httpRequest, CustomHrefHelperModel customHrefHelper = null) where T : class, IRESTfulItemResult;

        /// <summary>
        /// Filters an IQueryable of IRESTfulItemResult by OData parameters received in the route of the HTTP Request.
        /// </summary>
        /// <typeparam name="T">IRESTfulItemResult.</typeparam>
        /// <param name="itemsList">IQueryable of IRESTfulItemResult.</param>
        /// <param name="httpRequest">HTTP Request with the route containing OData parameters.</param>
        /// <returns></returns>
        Task<IEnumerable<T>> FilterResultAsync<T>(IQueryable<T> itemsList, HttpRequest httpRequest) where T : class, IRESTfulItemResult;

        /// <summary>
        /// Filters a List of IRESTfulItemResult by OData parameters received in the route of the HTTP Request.
        /// </summary>
        /// <typeparam name="T">IRESTfulItemResult.</typeparam>
        /// <param name="itemsList">List of IRESTfulItemResult.</param>
        /// <param name="httpRequest">HTTP Request with the route containing OData parameters.</param>
        /// <returns></returns>
        IEnumerable<T> FilterResult<T>(List<T> itemsList, HttpRequest httpRequest) where T : class, IRESTfulItemResult;
    }
}
