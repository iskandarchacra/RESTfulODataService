using Moq;
using RESTfulODataService.Models.OData;
using RESTfulODataService.Sample.Models;
using RESTfulODataService.Services.Parser;
using RESTfulODataService.Services.RegularExpressions;
using RESTfulODataService.Services.Validation.ODataElements;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RESTfulODataService.Tests.Services.Validators
{
    public class ODataElementsValidatorTests
    {
        private Mock<IRegexUtilizer> RegexUtilizerMock;
        private Mock<IODataParser> ODataParserMock;
        private string QueryValue { get; set; }

        public ODataElementsValidatorTests()
        {
            RegexUtilizerMock = new Mock<IRegexUtilizer>();
            ODataParserMock = new Mock<IODataParser>();
        }

        [Fact (DisplayName = "ODataElementsValidator_ShouldReturnTrue_OnValid'OrderBy'Query")]
        public async Task ValidateValidOrderByODataElement()
        {
            //Arrange
            QueryValue = "title";

            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnWords(QueryValue))
                .Returns(new string[] { "title" });

            IODataElementsValidator oDataElementsValidator = new ODataElementsValidator(RegexUtilizerMock.Object, ODataParserMock.Object, QueryValue);

            //Act
            var isValid = oDataElementsValidator.IsValidOrderByElements<BookModel>();

            //Assert
            Assert.True(isValid);
        }

        [Fact(DisplayName = "ODataElementsValidator_ShouldReturnTrue_OnValid'OrderBy'AscendingQuery")]
        public async Task ValidateValidOrderByAscendingODataElement()
        {
            //Arrange
            QueryValue = "title asc";

            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnWords(QueryValue))
                .Returns(new string[] { "title", "asc" });

            IODataElementsValidator oDataElementsValidator = new ODataElementsValidator(RegexUtilizerMock.Object, ODataParserMock.Object, QueryValue);

            //Act
            var isValid = oDataElementsValidator.IsValidOrderByElements<BookModel>();

            //Assert
            Assert.True(isValid);
        }

        [Fact(DisplayName = "ODataElementsValidator_ShouldReturnTrue_OnValid'OrderBy'DescendingQuery")]
        public async Task ValidateValidOrderByDescendingODataElement()
        {
            //Arrange
            QueryValue = "title desc";

            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnWords(QueryValue))
                .Returns(new string[] { "title", "desc" });

            IODataElementsValidator oDataElementsValidator = new ODataElementsValidator(RegexUtilizerMock.Object, ODataParserMock.Object, QueryValue);

            //Act
            var isValid = oDataElementsValidator.IsValidOrderByElements<BookModel>();

            //Assert
            Assert.True(isValid);
        }

        [Fact(DisplayName = "ODataElementsValidator_ShouldReturnFalse_OnInvalidProperty'OrderBy'Query")]
        public async Task ValidateInvalidPropertyOrderByODataElement()
        {
            //Arrange
            QueryValue = "propertyThatDoesNotExistInBookModel";

            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnWords(QueryValue))
                .Returns(new string[] { "propertyThatDoesNotExistInBookModel" });

            IODataElementsValidator oDataElementsValidator = new ODataElementsValidator(RegexUtilizerMock.Object, ODataParserMock.Object, QueryValue);

            //Act
            var isValid = oDataElementsValidator.IsValidOrderByElements<BookModel>();

            //Assert
            Assert.False(isValid);
        }

        [Fact (DisplayName = "ODataElementsValidator_ShouldReturnFalse_OnInvalid'OrderBy'AscendingQuery")]
        public async Task ValidateInvalidOrderByAscendingODataElement()
        {
            //Arrange
            QueryValue = "title ascending";

            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnWords(QueryValue))
                .Returns(new string[] { "title", "ascending" });

            IODataElementsValidator oDataElementsValidator = new ODataElementsValidator(RegexUtilizerMock.Object, ODataParserMock.Object, QueryValue);

            //Act
            var isValid = oDataElementsValidator.IsValidOrderByElements<BookModel>();

            //Assert
            Assert.False(isValid);
        }

        [Fact(DisplayName = "ODataElementsValidator_ShouldReturnFalse_OnInvalid'OrderBy'DescendingQuery")]
        public async Task ValidateInvalidOrderByDescendingODataElement()
        {
            //Arrange
            QueryValue = "title descending";

            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnWords(QueryValue))
                .Returns(new string[] { "title", "descending" });

            IODataElementsValidator oDataElementsValidator = new ODataElementsValidator(RegexUtilizerMock.Object, ODataParserMock.Object, QueryValue);

            //Act
            var isValid = oDataElementsValidator.IsValidOrderByElements<BookModel>();

            //Assert
            Assert.False(isValid);
        }

        [Fact (DisplayName = "ODataElementsValidator_ShouldReturnTrue_OnValid'Filter'EqualQuery")]
        public async Task ValidateValidFilterEqualODataElement()
        {
            //Arrange
            var resultExpressions = new List<ODataFilterModel>();

            QueryValue = "title eq 'Sample title'";

            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnAndOrOrOutsideApostrophes(QueryValue))
                .Returns(new string[] { "title eq 'Sample title'" });
            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnWhiteSpacesOutsideApostrophes(It.IsAny<string>()))
                .Returns(new string[] { "title", "eq", "'Sample title'" });
            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnApostrophes(It.IsAny<string>()))
                .Returns(new string[] { "title eq", "'Sample title'", "" });
            ODataParserMock.Setup(oDataParser => oDataParser.ParseOData("title eq 'Sample title'"))
                .Returns(new ODataFilterModel { Key = "Title", OperatorType = ODataOperatorType.eq, Value = "Sample title" });

            IODataElementsValidator oDataElementsValidator = new ODataElementsValidator(RegexUtilizerMock.Object, ODataParserMock.Object, QueryValue);

            //Act
            var isValid = oDataElementsValidator.IsValidFilter<BookModel>(out resultExpressions);

            //Assert
            Assert.True(isValid);
        }

        [Fact(DisplayName = "ODataElementsValidator_ShouldReturnTrue_OnValid'Filter'LessThanQuery")]
        public async Task ValidateValidFilterLessThanODataElement()
        {
            //Arrange
            var resultExpressions = new List<ODataFilterModel>();

            QueryValue = "totalPages lt 3";

            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnAndOrOrOutsideApostrophes(QueryValue))
                .Returns(new string[] { "totalPages lt 3" });
            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnWhiteSpacesOutsideApostrophes(It.IsAny<string>()))
                .Returns(new string[] { "totalPages", "lt", "3" });
            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnApostrophes(It.IsAny<string>()))
                .Returns(new string[] { "totalPages lt 3" });
            ODataParserMock.Setup(oDataParser => oDataParser.ParseOData("totalPages lt 3"))
                .Returns(new ODataFilterModel { Key = "TotalPages", OperatorType = ODataOperatorType.lt, Value = "3" });

            IODataElementsValidator oDataElementsValidator = new ODataElementsValidator(RegexUtilizerMock.Object, ODataParserMock.Object, QueryValue);

            //Act
            var isValid = oDataElementsValidator.IsValidFilter<BookModel>(out resultExpressions);

            //Assert
            Assert.True(isValid);
        }

        [Fact(DisplayName = "ODataElementsValidator_ShouldReturnTrue_OnValid'Filter'GreaterThanQuery")]
        public async Task ValidateValidFilterGreaterThanODataElement()
        {
            //Arrange
            var resultExpressions = new List<ODataFilterModel>();

            QueryValue = "totalPages gt 3";

            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnAndOrOrOutsideApostrophes(QueryValue))
                .Returns(new string[] { "totalPages gt 3" });
            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnWhiteSpacesOutsideApostrophes(It.IsAny<string>()))
                .Returns(new string[] { "totalPages", "gt", "3" });
            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnApostrophes(It.IsAny<string>()))
                .Returns(new string[] { "totalPages gt 3" });
            ODataParserMock.Setup(oDataParser => oDataParser.ParseOData("totalPages gt 3"))
                .Returns(new ODataFilterModel { Key = "TotalPages", OperatorType = ODataOperatorType.gt, Value = "3" });

            IODataElementsValidator oDataElementsValidator = new ODataElementsValidator(RegexUtilizerMock.Object, ODataParserMock.Object, QueryValue);

            //Act
            var isValid = oDataElementsValidator.IsValidFilter<BookModel>(out resultExpressions);

            //Assert
            Assert.True(isValid);
        }

        [Fact(DisplayName = "ODataElementsValidator_ShouldReturnTrue_OnValid'Filter'NotEqualQuery")]
        public async Task ValidateValidFilterNotEqualODataElement()
        {
            //Arrange
            var resultExpressions = new List<ODataFilterModel>();

            QueryValue = "totalPages ne 3";

            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnAndOrOrOutsideApostrophes(QueryValue))
                .Returns(new string[] { "totalPages ne 3" });
            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnWhiteSpacesOutsideApostrophes(It.IsAny<string>()))
                .Returns(new string[] { "totalPages", "ne", "3" });
            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnApostrophes(It.IsAny<string>()))
                .Returns(new string[] { "totalPages ne 3" });
            ODataParserMock.Setup(oDataParser => oDataParser.ParseOData("totalPages ne 3"))
                .Returns(new ODataFilterModel { Key = "TotalPages", OperatorType = ODataOperatorType.ne, Value = "3" });

            IODataElementsValidator oDataElementsValidator = new ODataElementsValidator(RegexUtilizerMock.Object, ODataParserMock.Object, QueryValue);

            //Act
            var isValid = oDataElementsValidator.IsValidFilter<BookModel>(out resultExpressions);

            //Assert
            Assert.True(isValid);
        }

        [Fact(DisplayName = "ODataElementsValidator_ShouldReturnTrue_OnValid'Filter'GreaterThanOrEqualQuery")]
        public async Task ValidateValidFilterGreaterThanOrEqualODataElement()
        {
            //Arrange
            var resultExpressions = new List<ODataFilterModel>();

            QueryValue = "totalPages ge 3";

            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnAndOrOrOutsideApostrophes(QueryValue))
                .Returns(new string[] { "totalPages ge 3" });
            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnWhiteSpacesOutsideApostrophes(It.IsAny<string>()))
                .Returns(new string[] { "totalPages", "ge", "3" });
            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnApostrophes(It.IsAny<string>()))
                .Returns(new string[] { "totalPages ge 3" });
            ODataParserMock.Setup(oDataParser => oDataParser.ParseOData("totalPages ge 3"))
                .Returns(new ODataFilterModel { Key = "TotalPages", OperatorType = ODataOperatorType.ge, Value = "3" });

            IODataElementsValidator oDataElementsValidator = new ODataElementsValidator(RegexUtilizerMock.Object, ODataParserMock.Object, QueryValue);

            //Act
            var isValid = oDataElementsValidator.IsValidFilter<BookModel>(out resultExpressions);

            //Assert
            Assert.True(isValid);
        }

        [Fact(DisplayName = "ODataElementsValidator_ShouldReturnTrue_OnValid'Filter'LessThanOrEqualQuery")]
        public async Task ValidateValidFilterLessThanOrEqualODataElement()
        {
            //Arrange
            var resultExpressions = new List<ODataFilterModel>();

            QueryValue = "totalPages le 3";

            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnAndOrOrOutsideApostrophes(QueryValue))
                .Returns(new string[] { "totalPages le 3" });
            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnWhiteSpacesOutsideApostrophes(It.IsAny<string>()))
                .Returns(new string[] { "totalPages", "le", "3" });
            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnApostrophes(It.IsAny<string>()))
                .Returns(new string[] { "totalPages le 3" });
            ODataParserMock.Setup(oDataParser => oDataParser.ParseOData("totalPages le 3"))
                .Returns(new ODataFilterModel { Key = "TotalPages", OperatorType = ODataOperatorType.le, Value = "3" });

            IODataElementsValidator oDataElementsValidator = new ODataElementsValidator(RegexUtilizerMock.Object, ODataParserMock.Object, QueryValue);

            //Act
            var isValid = oDataElementsValidator.IsValidFilter<BookModel>(out resultExpressions);

            //Assert
            Assert.True(isValid);
        }

        [Fact(DisplayName = "ODataElementsValidator_ShouldReturnTrue_OnValid'Filter'StartswithQuery")]
        public async Task ValidateValidFilterStartswithODataElement()
        {
            //Arrange
            var resultExpressions = new List<ODataFilterModel>();

            QueryValue = "startswith(title, 'a')";

            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnAndOrOrOutsideApostrophes(QueryValue))
                .Returns(new string[] { "startswith(title, 'a')" });
            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnWhiteSpacesOutsideApostrophes(It.IsAny<string>()))
                .Returns(new string[] { "startswith(title,", "'a'" });
            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnApostrophes(It.IsAny<string>()))
                .Returns(new string[] { "startswith(title,", "a", ")" });
            ODataParserMock.Setup(oDataParser => oDataParser.ParseOData("startswith(title, 'a')"))
                .Returns(new ODataFilterModel { Key = "Title", OperatorType = ODataOperatorType.startswith, Value = "a" });

            IODataElementsValidator oDataElementsValidator = new ODataElementsValidator(RegexUtilizerMock.Object, ODataParserMock.Object, QueryValue);

            //Act
            var isValid = oDataElementsValidator.IsValidFilter<BookModel>(out resultExpressions);

            //Assert
            Assert.True(isValid);
        }

        [Fact(DisplayName = "ODataElementsValidator_ShouldReturnTrue_OnValid'Filter'EndswithQuery")]
        public async Task ValidateValidFilterEndswithODataElement()
        {
            //Arrange
            var resultExpressions = new List<ODataFilterModel>();

            QueryValue = "endswith(title, 'a')";

            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnAndOrOrOutsideApostrophes(QueryValue))
                .Returns(new string[] { "endswith(title, 'a')" });
            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnWhiteSpacesOutsideApostrophes(It.IsAny<string>()))
                .Returns(new string[] { "endswith(title,", "'a'" });
            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnApostrophes(It.IsAny<string>()))
                .Returns(new string[] { "endswith(title,", "a", ")" });
            ODataParserMock.Setup(oDataParser => oDataParser.ParseOData("endswith(title, 'a')"))
                .Returns(new ODataFilterModel { Key = "Title", OperatorType = ODataOperatorType.endswith, Value = "a" });

            IODataElementsValidator oDataElementsValidator = new ODataElementsValidator(RegexUtilizerMock.Object, ODataParserMock.Object, QueryValue);

            //Act
            var isValid = oDataElementsValidator.IsValidFilter<BookModel>(out resultExpressions);

            //Assert
            Assert.True(isValid);
        }

        [Fact(DisplayName = "ODataElementsValidator_ShouldReturnTrue_OnValid'Filter'ContainsQuery")]
        public async Task ValidateValidFilterContainsODataElement()
        {
            //Arrange
            var resultExpressions = new List<ODataFilterModel>();

            QueryValue = "contains(title, 'a')";

            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnAndOrOrOutsideApostrophes(QueryValue))
                .Returns(new string[] { "contains(title, 'a')" });
            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnWhiteSpacesOutsideApostrophes(It.IsAny<string>()))
                .Returns(new string[] { "contains(title,", "'a'" });
            RegexUtilizerMock.Setup(regexUtilizer => regexUtilizer.SplitOnApostrophes(It.IsAny<string>()))
                .Returns(new string[] { "contains(title,", "a", ")" });
            ODataParserMock.Setup(oDataParser => oDataParser.ParseOData("contains(title, 'a')"))
                .Returns(new ODataFilterModel { Key = "Title", OperatorType = ODataOperatorType.contains, Value = "a" });

            IODataElementsValidator oDataElementsValidator = new ODataElementsValidator(RegexUtilizerMock.Object, ODataParserMock.Object, QueryValue);

            //Act
            var isValid = oDataElementsValidator.IsValidFilter<BookModel>(out resultExpressions);

            //Assert
            Assert.True(isValid);
        }
    }
}
