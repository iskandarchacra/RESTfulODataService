using RESTfulODataService.Extensions;
using RESTfulODataService.Models.OData;
using RESTfulODataService.Sample.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RESTfulODataService.Tests.ExtensionMethods
{
    public class FilterWithODataAsyncTests
    {
        [Fact(DisplayName = "FilterWithODataAsync_ShouldFilterIQueryable_On'Criteria'ProvidedInODataModel")]
        public async Task QueryListWithCriteria()
        {
            IQueryable<BookModel> eventsList = new List<BookModel>
            {
                new BookModel
                {
                    Id = "123"
                },
                new BookModel
                {
                    Id = "abc"
                }
            }.AsQueryable();

            var oDataModel = new ODataModel
            {
                Filter = new List<ODataFilterModel>
                {
                    new ODataFilterModel
                    {
                        Key = "Id",
                        OperatorType = ODataOperatorType.eq,
                        Value = "123"
                    }
                },
                OrderBy = "created"
            };


            var events = eventsList.FilterWithODataAsync(oDataModel);
            var query = await events.query;

            Assert.Single(query);
            Assert.Equal("123", query[0].Id);
        }

        [Fact(DisplayName = "FilterWithODataAsync_ShouldOrderIQueryableAscending_On'OrderBy'ProvidedInODataModel")]
        public async Task QueryListWithOrderBy()
        {
            IQueryable<BookModel> eventsList = new List<BookModel>
            {

                new BookModel
                {
                    Id = "3"
                },
                new BookModel
                {
                    Id = "1"
                },
                new BookModel
                {
                    Id = "2"
                }
            }.AsQueryable();

            var oDataModel = new ODataModel
            {
                OrderBy = "Id"
            };

            var events = eventsList.FilterWithODataAsync(oDataModel);
            var query = await events.query;

            Assert.Equal("1", query[0].Id);
            Assert.Equal("2", query[1].Id);
            Assert.Equal("3", query[2].Id);
        }

        [Fact(DisplayName = "FilterWithODataAsync_ShouldOrderIQueryableDescending_On'Reverse and OrderBy'ProvidedInODataModel")]
        public async Task QueryListWithOrderByAndReverse()
        {
            IQueryable<BookModel> eventsList = new List<BookModel>
            {

                new BookModel
                {
                    Id = "1"
                },
                new BookModel
                {
                    Id = "3"
                },
                new BookModel
                {
                    Id = "2"
                }
            }.AsQueryable();

            var oDataModel = new ODataModel
            {
                OrderBy = "Id",
                Reverse = true
            };


            var events = eventsList.FilterWithODataAsync(oDataModel);
            var query = await events.query;

            Assert.Equal("3", query[0].Id);
            Assert.Equal("2", query[1].Id);
            Assert.Equal("1", query[2].Id);
        }

        [Fact(DisplayName = "FilterWithODataAsync_ShouldFilterIQueryable_With'Top'ProvidedInODataModel")]
        public async Task QueryListWithTop()
        {
            IQueryable<BookModel> eventsList = new List<BookModel>
            {
                new BookModel
                {
                    Id = "1"
                },
                new BookModel
                {
                    Id = "3"
                },
                new BookModel
                {
                    Id = "2"
                }
            }.AsQueryable();

            var oDataModel = new ODataModel
            {
                Top = 1,
                OrderBy = "created"
            };

            var events = eventsList.FilterWithODataAsync(oDataModel);
            var query = await events.query;

            Assert.Single(query);
            Assert.Equal("1", query[0].Id);
        }

        [Fact(DisplayName = "FilterWithODataAsync_ShouldFilterIQueryable_With'Skip'ProvidedInODataModel")]
        public async Task QueryListWithSkip()
        {
            IQueryable<BookModel> eventsList = new List<BookModel>
            {
                new BookModel
                {
                    Id = "1"
                },
                new BookModel
                {
                    Id = "2"
                },
                new BookModel
                {
                    Id = "3"
                }
            }.AsQueryable();

            var oDataModel = new ODataModel
            {
                Skip = 1,
                OrderBy = "created"
            };

            var events = eventsList.FilterWithODataAsync(oDataModel);
            var query = await events.query;

            Assert.Equal(2, query.Count);
            Assert.Equal("2", query[0].Id);
            Assert.Equal("3", query[1].Id);
        }
    }
}