using System;
using System.Linq.Expressions;

namespace RESTfulODataService.Models.Sorting
{
    public class Sorting<T>
    {
        private Lazy<Expression<Func<T, object>>> _orderBy;
        private Lazy<string> _path;

        public Expression<Func<T, object>> OrderBy
        {
            get { return _orderBy != null ? _orderBy.Value : null; }
            set
            {
                _orderBy = new Lazy<Expression<Func<T, object>>>(() => value);
                _path = new Lazy<string>(() => value?.Body.ToString());
            }
        }

        public bool Reverse { get; set; }

        public static Sorting<T> Create(Expression<Func<T, object>> orderBy, bool reverse = false)
        {
            var sorting = new Sorting<T>()
            {
                OrderBy = orderBy,
                Reverse = reverse
            };

            return sorting;
        }
    }
}