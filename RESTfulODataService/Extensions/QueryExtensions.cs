using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RESTfulODataService.Models.RESTful;
using RESTfulODataService.Models.Sorting;

namespace RESTfulODataService.Extensions
{
    public static class QueryExtensions
    {
        public static RESTfulResult<T> ToRESTfulResult<T>(this IEnumerable<T> itemsList, int length, HttpRequest request, CustomHrefHelperModel customHrefHelper = null) where T : IRESTfulItemResult
        {
            var pathValue = request.Path.Value;
            var queryString = request.QueryString;

            itemsList.SetHrefProperty(pathValue, customHrefHelper);

            if (queryString.HasValue)
            {
                return new RESTfulResult<T>(itemsList, length, new Uri(pathValue + queryString, UriKind.Relative));
            }

            return new RESTfulResult<T>(itemsList, length, new Uri(pathValue, UriKind.Relative));
        }

        public static RESTfulResult<T> ToRESTfulResult<T>(this (IEnumerable<T> itemsList, int length) items, HttpRequest request, CustomHrefHelperModel customHrefHelper = null) where T : IRESTfulItemResult
        {
            var pathValue = request.Path.Value;
            var queryString = request.QueryString;

            items.itemsList.SetHrefProperty(pathValue, customHrefHelper);

            if (queryString.HasValue)
            {
                return new RESTfulResult<T>(items.itemsList, items.length, new Uri(pathValue + queryString, UriKind.Relative));
            }

            return new RESTfulResult<T>(items.itemsList, items.length, new Uri(pathValue, UriKind.Relative));
        }

        private static void SetHrefProperty<T>(this IEnumerable<T> itemsList, string pathValue, CustomHrefHelperModel customHrefHelper) where T : IRESTfulItemResult
        {
            if (customHrefHelper == null)
            {
                foreach (var item in itemsList)
                {
                    item.Href = pathValue + "/" + item.Id;
                }

                return;
            }

            if (customHrefHelper.HrefType == CustomHrefType.Relative)
            {
                foreach (var item in itemsList)
                {
                    item.Href = customHrefHelper.CustomHref + "/" + item.Id;
                }

                return;
            }

            if (customHrefHelper.HrefType == CustomHrefType.Absolute)
            {
                foreach (var item in itemsList)
                {
                    item.Href = customHrefHelper.CustomHref;
                }

                return;
            }
        }

        public static Task<List<TSource>> ToListSafeAsync<TSource>(this IQueryable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (!(source is IAsyncEnumerable<TSource>))
            {
                return Task.FromResult(source.ToList());
            }

            return source.ToListAsync();
        }
    }
}
