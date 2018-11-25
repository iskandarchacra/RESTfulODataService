using System;

namespace RESTfulODataService.Models.RESTful
{
    public interface IRESTfulItemResult
    {
        string Id { get; }

        string Href { get; set; }

        DateTime Created { get; set; }

        DateTime Modified { get; set; }
    }
}