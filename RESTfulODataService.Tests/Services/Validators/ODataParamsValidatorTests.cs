using System.Collections.Generic;
using Xunit;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;
using RESTfulODataService.Services.Validation.ODataParameters;
using RESTfulODataService.Services.RegularExpressions;
using RESTfulODataService.Services.RegularExpressions.Singleton;
using RESTfulODataService.Services.Parser;
using RESTfulODataService.Services.Validation.ODataElements.Singleton;
using RESTfulODataService.Models.OData;
using RESTfulODataService.Sample.Models;

namespace RESTfulODataService.Tests.Services.Validators
{
    public class ODataParamsValidatorTests
    {
        public IODataParametersValidator ODataParamsValidator
        {
            get
            {
                IRegexSingleton regex = new RegexSingleton();
                IRegexUtilizer regexUtilizer = new RegexUtilizer(regex);
                IODataParser oDataParser = new ODataParser(regexUtilizer);
                IODataElementsValidatorSingleton oDataElementsValidatorSingleton = new ODataElementsValidatorSingleton(regexUtilizer, oDataParser);

                return new ODataParametersValidator(oDataElementsValidatorSingleton);
            }
        }

        [Fact(DisplayName = "ODataParamsValidator_ShouldReturnTrue_OnValid'$filter'ODataParameter")]
        public async Task IsValidFilterOdata()
        {
            var errors = new List<string>();
            var oDataExpressionsList = new List<ODataFilterModel>();
            var validList = new List<KeyValuePair<string, StringValues>>();

            validList.Add(new KeyValuePair<string, StringValues>("$filter", "title lt 1"));
            Assert.True(ODataParamsValidator.IsValid<BookModel>(validList, out oDataExpressionsList, out errors));

            validList.Clear();
            validList.Add(new KeyValuePair<string, StringValues>("$filter", "title gt 1"));
            Assert.True(ODataParamsValidator.IsValid<BookModel>(validList, out oDataExpressionsList, out errors));

            validList.Clear();
            validList.Add(new KeyValuePair<string, StringValues>("$filter", "title ne 1" ));
            Assert.True(ODataParamsValidator.IsValid<BookModel>(validList, out oDataExpressionsList, out errors));

            validList.Clear();
            validList.Add(new KeyValuePair<string, StringValues>("$filter", "title le 1"));
            Assert.True(ODataParamsValidator.IsValid<BookModel>(validList, out oDataExpressionsList, out errors));

            validList.Clear();
            validList.Add(new KeyValuePair<string, StringValues>("$filter", "title eq 1"));
            Assert.True(ODataParamsValidator.IsValid<BookModel>(validList, out oDataExpressionsList, out errors));

            validList.Clear();
            validList.Add(new KeyValuePair<string, StringValues>("$filter", "title eq 'test string'"));
            Assert.True(ODataParamsValidator.IsValid<BookModel>(validList, out oDataExpressionsList, out errors));

            validList.Clear();
            validList.Add(new KeyValuePair<string, StringValues>("$filter", "startswith(title, 'isk')"));
            Assert.True(ODataParamsValidator.IsValid<BookModel>(validList, out oDataExpressionsList, out errors));

            validList.Clear();
            validList.Add(new KeyValuePair<string, StringValues>("$filter", "contains(title, 'an')"));
            Assert.True(ODataParamsValidator.IsValid<BookModel>(validList, out oDataExpressionsList, out errors));

            validList.Clear();
            validList.Add(new KeyValuePair<string, StringValues>("$filter", "endswith(title, 'dar')"));
            Assert.True(ODataParamsValidator.IsValid<BookModel>(validList, out oDataExpressionsList, out errors));

            validList.Clear();
            validList.Add(new KeyValuePair<string, StringValues>("$filter", "startswith(title ,'isk')"));
            Assert.True(ODataParamsValidator.IsValid<BookModel>(validList, out oDataExpressionsList, out errors));
        }

        [Fact(DisplayName = "ODataParamsValidator_ShouldReturnFalse_OnInvalid'$filter'ODataParameter")]
        public async Task IsNotValidFilterOData()
        {
            var errors = new List<string>();
            var oDataExpressionsList = new List<ODataFilterModel>();
            var invalidList = new List<KeyValuePair<string, StringValues>>();

            invalidList.Add(new KeyValuePair<string, StringValues>("$filtesr", "title lt 1"));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(invalidList, out oDataExpressionsList, out errors));

            invalidList.Clear();
            invalidList.Add(new KeyValuePair<string, StringValues>("$filter", "title eq1"));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(invalidList, out oDataExpressionsList, out errors));

            invalidList.Clear();
            invalidList.Add(new KeyValuePair<string, StringValues>("$filter", "titlegt 1"));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(invalidList, out oDataExpressionsList, out errors));

            invalidList.Clear();
            invalidList.Add(new KeyValuePair<string, StringValues>("$filter", "title ngse 1"));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(invalidList, out oDataExpressionsList, out errors));

            invalidList.Clear();
            invalidList.Add(new KeyValuePair<string, StringValues>("$filter", "title le 1''"));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(invalidList, out oDataExpressionsList, out errors));

            invalidList.Clear();
            invalidList.Add(new KeyValuePair<string, StringValues>("$filter", "title lt 1'"));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(invalidList, out oDataExpressionsList, out errors));

            invalidList.Clear();
            invalidList.Add(new KeyValuePair<string, StringValues>("$filter", "contains(title, an'"));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(invalidList, out oDataExpressionsList, out errors));

            invalidList.Clear();
            invalidList.Add(new KeyValuePair<string, StringValues>("$filter", "starts(title, 'an'"));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(invalidList, out oDataExpressionsList, out errors));

            invalidList.Clear();
            invalidList.Add(new KeyValuePair<string, StringValues>("$filter", "startswith('title', 'an'"));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(invalidList, out oDataExpressionsList, out errors));

            invalidList.Clear();
            invalidList.Add(new KeyValuePair<string, StringValues>("$filter", "endswith(title, 'an'"));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(invalidList, out oDataExpressionsList, out errors));
        }

        [Fact(DisplayName = "ODataParamsValidator_ShouldReturnTrue_OnValid'$orderBy'OData")]
        public async Task IsValidOrderByOdata()
        {
            var errors = new List<string>();
            var oDataExpressionsList = new List<ODataFilterModel>();
            var validList = new List<KeyValuePair<string, StringValues>>();

            validList.Add(new KeyValuePair<string, StringValues>("$orderBy", "title asc"));
            Assert.True(ODataParamsValidator.IsValid<BookModel>(validList, out oDataExpressionsList, out errors));

            validList.Clear();
            validList.Add(new KeyValuePair<string, StringValues>("$orderBy", "title desc"));
            Assert.True(ODataParamsValidator.IsValid<BookModel>(validList, out oDataExpressionsList, out errors));

            validList.Clear();
            validList.Add(new KeyValuePair<string, StringValues>("$orderBy", "title DESC"));
            Assert.True(ODataParamsValidator.IsValid<BookModel>(validList, out oDataExpressionsList, out errors));

            validList.Clear();
            validList.Add(new KeyValuePair<string, StringValues>("$orderBy", "title ASC"));
            Assert.True(ODataParamsValidator.IsValid<BookModel>(validList, out oDataExpressionsList, out errors));

            validList.Clear();
            validList.Add(new KeyValuePair<string, StringValues>("$orderBy", "title"));
            Assert.True(ODataParamsValidator.IsValid<BookModel>(validList, out oDataExpressionsList, out errors));
        }

        [Fact(DisplayName = "ODataParamsValidator_ShouldReturnFalse_OnInvalid'$orderBy'OData")]
        public async Task IsNotValidOrderByOData()
        {
            var errors = new List<string>();
            var oDataExpressionsList = new List<ODataFilterModel>();
            var invalidList = new List<KeyValuePair<string, StringValues>>();

            invalidList.Add(new KeyValuePair<string, StringValues>("$ordeBy", "title asc"));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(invalidList, out oDataExpressionsList, out errors));

            invalidList.Clear();
            invalidList.Add(new KeyValuePair<string, StringValues>("$orderBy", "title descc"));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(invalidList, out oDataExpressionsList, out errors));

            invalidList.Clear();
            invalidList.Add(new KeyValuePair<string, StringValues>("$orderBy", "title ascc"));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(invalidList, out oDataExpressionsList, out errors));
        }

        [Fact(DisplayName = "ODataParamsValidator_ShouldReturnTrue_OnValid'$top'ODataParamter")]
        public async Task IsValidTopOdata()
        {
            var errors = new List<string>();
            var oDataExpressionsList = new List<ODataFilterModel>();
            var validList = new List<KeyValuePair<string, StringValues>>();

            validList.Add(new KeyValuePair<string, StringValues>("$top", "1"));
            Assert.True(ODataParamsValidator.IsValid<BookModel>(validList, out oDataExpressionsList, out errors));

            validList.Clear();
            validList.Add(new KeyValuePair<string, StringValues>("$top", "12"));
            Assert.True(ODataParamsValidator.IsValid<BookModel>(validList, out oDataExpressionsList, out errors));
        }

        [Fact(DisplayName = "ODataParamsValidator_ShouldReturnFalse_OnInvalid'$top'ODataParamter")]
        public async Task IsInvalidTopOData()
        {
            var errors = new List<string>();
            var oDataExpressionsList = new List<ODataFilterModel>();
            var invalidList = new List<KeyValuePair<string, StringValues>>();

            invalidList.Add(new KeyValuePair<string, StringValues>("$top", "s1"));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(invalidList, out oDataExpressionsList, out errors));

            invalidList.Clear();
            invalidList.Add(new KeyValuePair<string, StringValues>("$top", "error test"));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(invalidList, out oDataExpressionsList, out errors));

            invalidList.Clear();
            invalidList.Add(new KeyValuePair<string, StringValues>("$to", "1"));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(invalidList, out oDataExpressionsList, out errors));

            invalidList.Clear();
            invalidList.Add(new KeyValuePair<string, StringValues>("$top", "1 2"));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(invalidList, out oDataExpressionsList, out errors));
        }

        [Fact(DisplayName = "ODataParamsValidator_ShouldReturnTrue_OnValid'$skip'ODataParameter")]
        public async Task IsValidSkipOdata()
        {
            var oDataExpressionsList = new List<ODataFilterModel>();
            var errors = new List<string>();
            var validList = new List<KeyValuePair<string, StringValues>>();

            validList.Add(new KeyValuePair<string, StringValues>("$skip", "1"));
            Assert.True(ODataParamsValidator.IsValid<BookModel>(validList, out oDataExpressionsList, out errors));

            validList.Clear();
            validList.Add(new KeyValuePair<string, StringValues>("$skip", "12"));
            Assert.True(ODataParamsValidator.IsValid<BookModel>(validList, out oDataExpressionsList, out errors));
        }

        [Fact(DisplayName = "ODataParamsValidator_ShouldReturnFalse_OnInValid'$skip'ODataParameter")]
        public async Task IsInvalidSkipOData()
        {
            var oDataExpressionsList = new List<ODataFilterModel>();
            var errors = new List<string>();
            var invalidList = new List<KeyValuePair<string, StringValues>>();

            invalidList.Add(new KeyValuePair<string, StringValues>("$skip", "s1"));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(invalidList, out oDataExpressionsList, out errors));

            invalidList.Clear();
            invalidList.Add(new KeyValuePair<string, StringValues>("$skip", "error test"));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(invalidList, out oDataExpressionsList, out errors));

            invalidList.Clear();
            invalidList.Add(new KeyValuePair<string, StringValues>("$sk", "1"));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(invalidList, out oDataExpressionsList, out errors));

            invalidList.Clear();
            invalidList.Add(new KeyValuePair<string, StringValues>("$skip", "1 2"));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(invalidList, out oDataExpressionsList, out errors));
        }

        [Fact(DisplayName = "ODataParamsValidator_ShouldReturnTrue_OnValid'$filter and $skip'ODataParameters")]
        public async Task IsValidFilterAndSkipOdata()
        {
            var oDataExpressionsList = new List<ODataFilterModel>();
            var errors = new List<string>();

            var validList = new List<KeyValuePair<string, StringValues>>()
            {
                new KeyValuePair<string, StringValues>("$filter", "title lt 1"),
                new KeyValuePair<string, StringValues>("$skip", "12"),
            };

            Assert.True(ODataParamsValidator.IsValid<BookModel>(validList, out oDataExpressionsList, out errors));
        }

        [Fact(DisplayName = "ODataParamsValidator_ShouldReturnFalse_OnInvalid'$filter and $skip'ODataParameters")]
        public async Task IsInvalidFilterAndSkipOData()
        {
            var oDataExpressionsList = new List<ODataFilterModel>();
            var errors = new List<string>();

            var firstInvalidList = new List<KeyValuePair<string, StringValues>>()
            {
                new KeyValuePair<string, StringValues>("$filter", "title lt 1"),
                new KeyValuePair<string, StringValues>("$ski", "12"),
            };

            var secondInvalidList = new List<KeyValuePair<string, StringValues>>()
            {
                new KeyValuePair<string, StringValues>("$filt", "title lt 1"),
                new KeyValuePair<string, StringValues>("$skip", "12"),
            };

            var thirdInvalidList = new List<KeyValuePair<string, StringValues>>()
            {
                new KeyValuePair<string, StringValues>("$filter", "title lt1"),
                new KeyValuePair<string, StringValues>("$skip", "12"),
            };

            var fourthInvalidList = new List<KeyValuePair<string, StringValues>>()
            {
                new KeyValuePair<string, StringValues>("$filter", "title lt 1"),
                new KeyValuePair<string, StringValues>("$skip", "testerror"),
            };

            Assert.True(!ODataParamsValidator.IsValid<BookModel>(firstInvalidList, out oDataExpressionsList, out errors));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(secondInvalidList, out oDataExpressionsList, out errors));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(thirdInvalidList, out oDataExpressionsList, out errors));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(fourthInvalidList, out oDataExpressionsList, out errors));
        }

        [Fact(DisplayName = "ODataParamsValidator_ShouldReturnTrue_OnValid'$filter and $top'ODataParameters")]
        public async Task IsValidFilterAndTopOdata()
        {
            var oDataExpressionsList = new List<ODataFilterModel>();
            var errors = new List<string>();

            var validList = new List<KeyValuePair<string, StringValues>>()
            {
                new KeyValuePair<string, StringValues>("$filter", "title lt 1"),
                new KeyValuePair<string, StringValues>("$top", "12"),
            };

            Assert.True(ODataParamsValidator.IsValid<BookModel>(validList, out oDataExpressionsList, out errors));
        }

        [Fact(DisplayName = "ODataParamsValidator_ShouldReturnFalse_OnInvalid'$filter and $top'ODataParameters")]
        public async Task IsNotValidFilterAndTopOdata()
        {
            var oDataExpressionsList = new List<ODataFilterModel>();
            var errors = new List<string>();

            var firstInvalidList = new List<KeyValuePair<string, StringValues>>()
            {
                new KeyValuePair<string, StringValues>("$filter", "title lt 1"),
                new KeyValuePair<string, StringValues>("$to", "12"),
            };

            var secondInvalidList = new List<KeyValuePair<string, StringValues>>()
            {
                new KeyValuePair<string, StringValues>("$filt", "title lt 1"),
                new KeyValuePair<string, StringValues>("$top", "12"),
            };

            var thirdInvalidList = new List<KeyValuePair<string, StringValues>>()
            {
                new KeyValuePair<string, StringValues>("$filter", "title lt1"),
                new KeyValuePair<string, StringValues>("$top", "12"),
            };

            var fourthInvalidList = new List<KeyValuePair<string, StringValues>>()
            {
                new KeyValuePair<string, StringValues>("$filter", "title lt 1"),
                new KeyValuePair<string, StringValues>("$top", "testerror"),
            };

            Assert.True(!ODataParamsValidator.IsValid<BookModel>(firstInvalidList, out oDataExpressionsList, out errors));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(secondInvalidList, out oDataExpressionsList, out errors));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(thirdInvalidList, out oDataExpressionsList, out errors));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(fourthInvalidList, out oDataExpressionsList, out errors));
        }

        [Fact(DisplayName = "ODataParamsValidator_ShouldReturnTrue_OnValid'$filter and $top and $skip'ODataParameters")]
        public async Task IsValidFilterAndTopAndSkipOdata()
        {
            var oDataExpressionsList = new List<ODataFilterModel>();
            var errors = new List<string>();

            var validList = new List<KeyValuePair<string, StringValues>>()
            {
                new KeyValuePair<string, StringValues>("$filter", "title lt 1"),
                new KeyValuePair<string, StringValues>("$top", "12"),
                new KeyValuePair<string, StringValues>("$skip", "10")
            };

            Assert.True(ODataParamsValidator.IsValid<BookModel>(validList, out oDataExpressionsList, out errors));
        }

        [Fact(DisplayName = "ODataParamsValidator_ShouldReturnFalse_OnInvalid'$filter and $top and $skip'ODataParameters")]
        public async Task IsNotValidFilterAndTopAndSkipOdata()
        {
            var oDataExpressionsList = new List<ODataFilterModel>();
            var errors = new List<string>();

            var firstInvalidList = new List<KeyValuePair<string, StringValues>>()
            {
                new KeyValuePair<string, StringValues>("$filter", "totalPages lt 1"),
                new KeyValuePair<string, StringValues>("$to", "12"),
                new KeyValuePair<string, StringValues>("$skip", "10")
            };

            var secondInvalidList = new List<KeyValuePair<string, StringValues>>()
            {
                new KeyValuePair<string, StringValues>("$filt", "totalPages lt 1"),
                new KeyValuePair<string, StringValues>("$top", "12"),
                new KeyValuePair<string, StringValues>("$skip", "10"),
            };

            var thirdInvalidList = new List<KeyValuePair<string, StringValues>>()
            {
                new KeyValuePair<string, StringValues>("$filter", "title lt1"),
                new KeyValuePair<string, StringValues>("$top", "12"),
                new KeyValuePair<string, StringValues>("$skp", "10"),
            };

            var fourthInvalidList = new List<KeyValuePair<string, StringValues>>()
            {
                new KeyValuePair<string, StringValues>("$filter", "title lt 1"),
                new KeyValuePair<string, StringValues>("$top", "12"),
                new KeyValuePair<string, StringValues>("$skip", "testerror"),
            };

            var fifthInvalidList = new List<KeyValuePair<string, StringValues>>()
            {
                new KeyValuePair<string, StringValues>("$filter", "title lt 1"),
                new KeyValuePair<string, StringValues>("$top", "testerror"),
                new KeyValuePair<string, StringValues>("$skip", "10"),
            };

            var sixthInvalidList = new List<KeyValuePair<string, StringValues>>()
            {
                new KeyValuePair<string, StringValues>("$filter", "titleeq 1"),
                new KeyValuePair<string, StringValues>("$top", "testerror"),
                new KeyValuePair<string, StringValues>("$skip", "10"),
            };

            Assert.True(!ODataParamsValidator.IsValid<BookModel>(firstInvalidList, out oDataExpressionsList, out errors));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(secondInvalidList, out oDataExpressionsList, out errors));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(thirdInvalidList, out oDataExpressionsList, out errors));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(fourthInvalidList, out oDataExpressionsList, out errors));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(fifthInvalidList, out oDataExpressionsList, out errors));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(sixthInvalidList, out oDataExpressionsList, out errors));
        }

        [Fact(DisplayName = "ODataParamsValidator_ShouldReturnTrue_OnValid'$filter and $orderBy'ODataParameters")]
        public async Task IsValidFilterAndOrderByOdata()
        {
            var oDataExpressionsList = new List<ODataFilterModel>();
            var errors = new List<string>();

            var validList = new List<KeyValuePair<string, StringValues>>()
            {
                new KeyValuePair<string, StringValues>("$filter", "title lt 1"),
                new KeyValuePair<string, StringValues>("$orderBy", "title")
            };

            var secondValidList = new List<KeyValuePair<string, StringValues>>()
            {
                new KeyValuePair<string, StringValues>("$filter", "startswith(title,'ame, isk')"),
                new KeyValuePair<string, StringValues>("$orderBy", "title asc")
            };

            var thirdValidList = new List<KeyValuePair<string, StringValues>>()
            {
                new KeyValuePair<string, StringValues>("$filter", "title lt 1"),
                new KeyValuePair<string, StringValues>("$orderBy", "title desc")
            };


            Assert.True(ODataParamsValidator.IsValid<BookModel>(validList, out oDataExpressionsList, out errors));
            Assert.True(ODataParamsValidator.IsValid<BookModel>(secondValidList, out oDataExpressionsList, out errors));
            Assert.True(ODataParamsValidator.IsValid<BookModel>(thirdValidList, out oDataExpressionsList, out errors));
        }

        [Fact(DisplayName = "ODataParamsValidator_ShouldReturnFalse_OnInvalid'$filter and $orderBy'ODataParameters")]
        public async Task IsNotValidFilterAndOrderByOdata()
        {
            var oDataExpressionsList = new List<ODataFilterModel>();
            var errors = new List<string>();
            var firstInvalidList = new List<KeyValuePair<string, StringValues>>()
            {
                new KeyValuePair<string, StringValues>("$fter", "title lt 1"),
                new KeyValuePair<string, StringValues>("$orderBy", "title"),
            };

            var secondInvalidList = new List<KeyValuePair<string, StringValues>>()
            {
                new KeyValuePair<string, StringValues>("$filter", "title lt 1"),
                new KeyValuePair<string, StringValues>("$orBy", "title"),
            };

            var thirdInvalidList = new List<KeyValuePair<string, StringValues>>()
            {
                new KeyValuePair<string, StringValues>("$filter", "titlelt 1"),
                new KeyValuePair<string, StringValues>("$orderBy", "title"),
            };

            var fourthInvalidList = new List<KeyValuePair<string, StringValues>>()
            {
                new KeyValuePair<string, StringValues>("$filter", "title lt 1"),
                new KeyValuePair<string, StringValues>("$orderBy", "title asc cas"),
            };

            var fifthInvalidList = new List<KeyValuePair<string, StringValues>>()
            {
                new KeyValuePair<string, StringValues>("$filter", "title lt 1"),
                new KeyValuePair<string, StringValues>("$orderBy", "title assc"),
            };

            var sixthInvalidList = new List<KeyValuePair<string, StringValues>>()
            {
                new KeyValuePair<string, StringValues>("$filter", "startswith(s' d,dl')"),
                new KeyValuePair<string, StringValues>("$orderBy", "title asc"),
            };

            Assert.True(!ODataParamsValidator.IsValid<BookModel>(firstInvalidList, out oDataExpressionsList, out errors));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(secondInvalidList, out oDataExpressionsList, out errors));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(thirdInvalidList, out oDataExpressionsList, out errors));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(fourthInvalidList, out oDataExpressionsList, out errors));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(fifthInvalidList, out oDataExpressionsList, out errors));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(sixthInvalidList, out oDataExpressionsList, out errors));
        }

        [Fact(DisplayName = "ODataParamsValidator_ShouldReturnTrue_OnValidMultipleODataQueries")]
        public async Task IsValidFilterODataWithAndAndOR()
        {
            var oDataExpressionsList = new List<ODataFilterModel>();
            var errors = new List<string>();
            var validList = new List<KeyValuePair<string, StringValues>>();

            validList.Add(new KeyValuePair<string, StringValues>("$filter", "startswith(title, 'isk') and contains(title, '4')"));
            Assert.True(ODataParamsValidator.IsValid<BookModel>(validList, out oDataExpressionsList, out errors));

            validList.Clear();
            validList.Add(new KeyValuePair<string, StringValues>("$filter", "startswith(title, 'isk') or contains(title, '4')"));
            Assert.True(ODataParamsValidator.IsValid<BookModel>(validList, out oDataExpressionsList, out errors));

            validList.Clear();
            validList.Add(new KeyValuePair<string, StringValues>("$filter", "startswith(title, 'isk') and contains(title, '4') or totalPages lt 2"));
            Assert.True(ODataParamsValidator.IsValid<BookModel>(validList, out oDataExpressionsList, out errors));

            validList.Clear();
            validList.Add(new KeyValuePair<string, StringValues>("$filter", "startswith(title, 'isk') and contains(title, '4') or totalPages lt 2 or title eq 'or'"));
            Assert.True(ODataParamsValidator.IsValid<BookModel>(validList, out oDataExpressionsList, out errors));

            validList.Clear();
            validList.Add(new KeyValuePair<string, StringValues>("$filter", "startswith(title, 'isk') and contains(title, '4') or title eq 'Iskandar' "));
            Assert.True(ODataParamsValidator.IsValid<BookModel>(validList, out oDataExpressionsList, out errors));

            validList.Clear();
            validList.Add(new KeyValuePair<string, StringValues>("$filter", "startswith(title, 'isk') and contains(title, '4') and totalPages lt 1 and totalPages ne 3"));
            Assert.True(ODataParamsValidator.IsValid<BookModel>(validList, out oDataExpressionsList, out errors));

            validList.Clear();
            validList.Add(new KeyValuePair<string, StringValues>("$filter", "startswith(title, 'isk') and contains(title, '4') or totalPages lt 1 or startswith(title, 'test value')"));
            Assert.True(ODataParamsValidator.IsValid<BookModel>(validList, out oDataExpressionsList, out errors));

            validList.Clear();
            validList.Add(new KeyValuePair<string, StringValues>("$filter", "startswith(title, 'isk') and contains(title, '4') and totalPages ge 6"));
            Assert.True(ODataParamsValidator.IsValid<BookModel>(validList, out oDataExpressionsList, out errors));
        }

        [Fact(DisplayName = "ODataParamsValidator_ShouldReturnFalse_OnInvalidMultipleODataQueries")]
        public async Task IsNotValidFilterODataWithAndAndOR()
        {
            var oDataExpressionsList = new List<ODataFilterModel>();
            var errors = new List<string>();
            var invalidList = new List<KeyValuePair<string, StringValues>>();

            invalidList.Add(new KeyValuePair<string, StringValues>("$filter", "startswith(title, 'isk') andcontains(title, '4')"));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(invalidList, out oDataExpressionsList, out errors));

            invalidList.Clear();
            invalidList.Add(new KeyValuePair<string, StringValues>("$filter", "startswith(title, 'isk') and contains(title, '4')or totalPages lt 3"));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(invalidList, out oDataExpressionsList, out errors));

            invalidList.Clear();
            invalidList.Add(new KeyValuePair<string, StringValues>("$filter", "startswith(title, 'isk') and contains(title, '4') or totalPages lt3"));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(invalidList, out oDataExpressionsList, out errors));

            invalidList.Clear();
            invalidList.Add(new KeyValuePair<string, StringValues>("$filter", "startswith(title, 'isk') and contains(title, '4') and startswith(title, 's)"));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(invalidList, out oDataExpressionsList, out errors));

            invalidList.Clear();
            invalidList.Add(new KeyValuePair<string, StringValues>("$filter", "startswith(title, 'isk') and contains(title, '4') and contains(title, 'title'"));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(invalidList, out oDataExpressionsList, out errors));

            invalidList.Clear();
            invalidList.Add(new KeyValuePair<string, StringValues>("$filter", "startswith(title, 'isk') and contains(title, '4') or totalPages eq ne 3"));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(invalidList, out oDataExpressionsList, out errors));

            invalidList.Clear();
            invalidList.Add(new KeyValuePair<string, StringValues>("$filter", "startswith(title, 'isk') and contains(title, '4') or totalPages eq 5 or contains(title, '3')or contains(title, 'erik')"));
            Assert.True(!ODataParamsValidator.IsValid<BookModel>(invalidList, out oDataExpressionsList, out errors));
        }
    }
}