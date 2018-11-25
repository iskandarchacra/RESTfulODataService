using Microsoft.Extensions.Primitives;
using RESTfulODataService.Services.Parser;
using RESTfulODataService.Services.RegularExpressions;
using RESTfulODataService.Services.RegularExpressions.Singleton;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RESTfulODataService.Tests.Services.Parser
{
    public class ODataParserTests
    {
        public ODataParser ODataParser
        {
            get
            {
                IRegexSingleton regexSingleton = new RegexSingleton();
                IRegexUtilizer regexUtilizer = new RegexUtilizer(regexSingleton);

                return new ODataParser(regexUtilizer);
            }
        }

        [Fact(DisplayName = "ODataParser_ShouldReturnODataExpressionModelWith'Startswith'Type_OnValidStartswithQuery")]
        public async Task Should_Parse_Odata_Startswith()
        {
            //Arrange
            var query = "Startswith(label , 'test label')";

            //Act
            var odata = ODataParser.ParseOData(query);

            //Assert
            Assert.Equal("Label", odata.Key);
            Assert.Equal("test label", odata.Value);
            Assert.Equal("startswith", odata.OperatorType.ToString());
        }

        [Fact(DisplayName = "ODataParser_ShouldReturnODataExpressionModelWith'Endswith'Type_OnValidEndswithQuery")]
        public async Task Should_Parse_Odata_Endswith()
        {
            //Arrange
            var query = "Endswith(label, 'E')";

            //Act
            var odata = ODataParser.ParseOData(query);

            //Assert
            Assert.Equal("Label", odata.Key);
            Assert.Equal("E", odata.Value);
            Assert.Equal("endswith", odata.OperatorType.ToString());
        }

        [Fact(DisplayName = "ODataParser_ShouldReturnODataExpressionModelWith'Contains'Type_OnValidContainsQuery")]
        public async Task Should_Parse_Odata_Contains()
        {
            //Arrange
            var query = "Contains(label, 'be')";

            //Act
            var odata = ODataParser.ParseOData(query);

            //Assert
            Assert.Equal("Label", odata.Key);
            Assert.Equal("be", odata.Value);
            Assert.Equal("contains", odata.OperatorType.ToString());
        }

        [Fact(DisplayName = "ODataParser_ShouldReturnODataExpressionModelWith'Eq'Type_OnEqQueryWithInt")]
        public async Task Should_Parse_OData_With_Integer()
        {
            //Arrange
            var query = "Label eq 7";

            //Act
            var odata = ODataParser.ParseOData(query);

            //Assert
            Assert.Equal("Label", odata.Key);
            Assert.Equal("7", odata.Value.Trim());
            Assert.Equal("eq", odata.OperatorType.ToString());
        }

        [Fact(DisplayName = "ODataParser_ShouldReturnODataExpressionModelWith'Eq'Type_OnEqQueryWithDouble")]
        public async Task Should_Parse_OData_With_Double()
        {
            //Arrange
            var query = "Label eq 7.0";

            //Act
            var odata = ODataParser.ParseOData(query);

            //Assert
            Assert.Equal("Label", odata.Key);
            Assert.Equal("7.0", odata.Value.Trim());
            Assert.Equal("eq", odata.OperatorType.ToString());
        }

        [Fact(DisplayName = "ODataParser_ShouldReturnODataExpressionModelWith'Eq'Type_OnEqQueryWithBool")]
        public async Task Should_Parse_OData_With_Boolean()
        {
            //Arrange
            var query = "isPrivate eq true";

            //Act
            var odata = ODataParser.ParseOData(query);

            //Assert
            Assert.Equal("IsPrivate", odata.Key);
            Assert.Equal("true", odata.Value.Trim());
            Assert.Equal("eq", odata.OperatorType.ToString());
        }

        [Fact(DisplayName = "ODataParser_ShouldReturnODataExpressionModelWith'Eq'Type_OnEqQueryWithString")]
        public async Task Should_Parse_Query()
        {
            //Arrange
            var keys = new List<KeyValuePair<string, StringValues>>
            {
                new KeyValuePair<string, StringValues>("$orderBy", "label asc"),
                new KeyValuePair<string, StringValues>("$filter", "label eq 'label'")
            };

            //Act
            var oDataModel = ODataParser.ParseQuery(keys);

            //Assert
            Assert.Equal("label", oDataModel.OrderBy);
            Assert.False(oDataModel.Reverse);
        }
    }
}
