using System;
using System.Collections.Generic;
using RESTfulODataService.Models.RESTful;

namespace RESTfulODataService.Sample.Models
{
    public class ReaderModel : IRESTfulItemResult
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<BookReaderModel> BookReaders { get; set; }

        public string Href { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
