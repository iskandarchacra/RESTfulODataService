using RESTfulODataService.Models.RESTful;
using System;
using System.Collections.Generic;

namespace RESTfulODataService.Sample.Models
{
    public class AuthorModel : IRESTfulItemResult
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<BookModel> Books { get; set; }

        public string Href { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
