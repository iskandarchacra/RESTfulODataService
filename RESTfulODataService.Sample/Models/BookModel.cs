using System;
using System.Collections.Generic;
using RESTfulODataService.Models.RESTful;

namespace RESTfulODataService.Sample.Models
{
    public class BookModel : IRESTfulItemResult
    {
        public string Id { get; set; }

        public string Href { get; set; }

        public string Title { get; set; }

        public int TotalPages { get; set; }

        public double Ratings { get; set; }

        public BookType Type { get; set; }

        public string Label { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public List<BookReaderModel> BookReaders { get; set; }

        public List<ChapterModel> Chapters { get; set; }

        public AuthorModel Author { get; set; }

        public string AuthorId { get; set; }
    }
}
