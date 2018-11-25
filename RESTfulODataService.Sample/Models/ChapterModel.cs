using System;
using RESTfulODataService.Models.RESTful;

namespace RESTfulODataService.Sample.Models
{
    public class ChapterModel : IRESTfulItemResult
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Href { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public BookModel Book { get; set; }

        public string BookId { get; set; }
    }
}
