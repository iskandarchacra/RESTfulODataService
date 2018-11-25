using System;
using System.Collections.Generic;

namespace RESTfulODataService.Models.RESTful
{
    public interface IRESTfulItemResult<out T> where T : IRESTfulItemResult
    {
        IEnumerable<T> Items { get; }

        int Length { get; }

        Uri Href { get; }
    }
}