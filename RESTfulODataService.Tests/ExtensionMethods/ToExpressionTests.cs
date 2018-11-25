using RESTfulODataService.Extensions;
using RESTfulODataService.Sample.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace RESTfulODataService.Tests.ExtensionMethods
{
    public class ToExpressionTests
    {
        [Fact(DisplayName = "ToExpression_ShouldGetIntExpression_WhenPassedStringThatCanBeParsedToInt")]
        public async Task Should_Get_Expression_From_Int_Type()
        {
            //Arrange
            var intType = typeof(int);

            //Act
            var intExpression = intType.ToConstantExpression("7");

            //Assert
            Assert.Equal(typeof(int), intExpression.Type);
        }

        [Fact(DisplayName = "ToExpression_ShouldGetBoolExpression_WhenPassedStringThatCanBeParsedToBool")]
        public async Task Should_Get_Expression_From_Bool_Type()
        {
            //Arrange
            var boolType = typeof(bool);

            //Act
            var boolExpression = boolType.ToConstantExpression("true");

            //Assert
            Assert.Equal(typeof(bool), boolExpression.Type);
        }

        [Fact(DisplayName = "ToExpression_ShouldGetEnumExpression_WhenPassedStringThatCanBeParsedToEnum")]
        public async Task Should_Get_Expression_From_Enum_Type()
        {
            //Arrange
            var enumType = typeof(BookType);

            //Act
            var enumExpression = enumType.ToConstantExpression("Novel");

            //Assert
            Assert.Equal(typeof(BookType), enumExpression.Type);
        }

        [Fact(DisplayName = "ToExpression_ShouldGetDoubleExpression_WhenPassedStringThatCanBeParsedToDouble")]
        public async Task Should_Get_Expression_From_Double_Type()
        {
            //Arrange
            var doubleType = typeof(double);

            //Act
            var doubleExpression = doubleType.ToConstantExpression("7.0");

            //Assert
            Assert.Equal(typeof(double), doubleExpression.Type);
        }

        [Fact(DisplayName = "ToExpression_ShouldGetDateTimeExpression_WhenPassedStringThatCanBeParsedToDateTime")]
        public async Task Should_Get_Expression_From_DateTime_Type()
        {
            //Arrange
            var dateTimeType = typeof(DateTime);

            //Act
            var dateTimeExpression = dateTimeType.ToConstantExpression("2018-01-01");

            //Assert
            Assert.Equal(typeof(DateTime), dateTimeExpression.Type);
        }
    }
}
