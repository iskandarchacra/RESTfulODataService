using System;
using System.Collections.Generic;

namespace RESTfulODataService.Models.RESTful
{
    public class RESTfulResult<T> : IRESTfulItemResult<T> where T : IRESTfulItemResult
    {
        private readonly IEnumerable<T> _items;
        private readonly int _totalLength;
        private readonly Uri _href;

        public RESTfulResult(IEnumerable<T> items, int totalLength, Uri href)
        {
            _items = items;
            _totalLength = totalLength;
            _href = href;
        }

        public IEnumerable<T> Items => _items;

        public int Length => _totalLength;
        
        public Uri Href => _href;
    }
}