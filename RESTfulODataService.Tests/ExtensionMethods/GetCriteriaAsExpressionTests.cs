using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;
using RESTfulODataService.Models.OData;
using RESTfulODataService.Extensions;
using RESTfulODataService.Sample.Models;

namespace RESTfulODataService.Tests.ExtensionMethods
{
    public class GetCriteriaAsExpressionTests
    {
        [Fact(DisplayName = "GetCriteriaAsExpression_ShouldReturnCriteriaFromODataAsAndExpression_WhenODataExpressionListContainsAndJoinType")]
        public async Task Should_Convert_LambdaExpression_With_AndJoinType()
        {
            //Arrange
            var oDataExpressionList = new List<ODataFilterModel>
            {
                new ODataFilterModel
                {
                    Key = "Label",
                    OperatorType = ODataOperatorType.eq,
                    Value = "label",
                    JoinType = FilterJoinType.And
                },
                new ODataFilterModel
                {
                    Key = "Id",
                    OperatorType = ODataOperatorType.contains,
                    Value = "1"
                }
            };

            var oDataModel = new ODataModel
            {
                Filter = oDataExpressionList
            };

            //Act
            var andExpression = oDataModel.GetFilterAsExpression<BookModel>();
                
            //Assert
            Assert.Equal("((x.Label == \"label\") AndAlso Invoke(x => x.Id.Contains(\"1\"), x))", andExpression.Body.ToString());
        }

        [Fact(DisplayName = "GetCriteriaAsExpression_ShouldReturnCriteriaFromODataAsOrExpression_WhenODataExpressionListContainsOrJoinType")]
        public async Task Should_Convert_LambdaExpression_With_OrJoinType()
        {
            //Arrange
            var oDataExpressionList = new List<ODataFilterModel>
            {
                new ODataFilterModel
                {
                    Key = "Label",
                    OperatorType = ODataOperatorType.eq,
                    Value = "label",
                    JoinType = FilterJoinType.Or
                },
                new ODataFilterModel
                {
                    Key = "Id",
                    OperatorType = ODataOperatorType.contains,
                    Value = "1"
                }
            };

            var oDataModel = new ODataModel
            {
                Filter = oDataExpressionList
            };

            //Act
            var orExpression = oDataModel.GetFilterAsExpression<BookModel>();

            //Assert
            Assert.Equal("((x.Label == \"label\") OrElse Invoke(x => x.Id.Contains(\"1\"), x))", orExpression.Body.ToString());
        }

        [Fact(DisplayName = "GetOrderByAsExpression_ShouldReturnOrderByFromODataAsExpression_WhenOrderByHasPropertyName")]
        public async Task Should_Convert_Sort()
        {
            //Arrange
            var oDataModel = new ODataModel
            {
                OrderBy = "Label"
            };

            //Act
            var sortExpression = oDataModel.GetOrderByAsExpression<BookModel>();

            //Assert
            Assert.Equal("Convert(x.Label, Object)", sortExpression.Body.ToString());
        }

        [Fact(DisplayName = "GetCriteriaAsExpression_ShouldReturnCriteriaFromODataAsEqualsExpression_WhenODataExpressionModelHas'Eq'OperatorType")]
        public async Task Should_Convert_EqLambdaExpression()
        {
            //Arrange
            var criteria = new List<ODataFilterModel>
            {
                new ODataFilterModel
                {
                    Key = "Label",
                    OperatorType = ODataOperatorType.eq,
                    Value = "lxabel2",
                }
            };

            var oDataModel = new ODataModel
            {
                Filter = criteria
            };

            //Act
            var expression = oDataModel.GetFilterAsExpression<BookModel>();

            //Assert
            Assert.Equal("(x.Label == \"lxabel2\")", expression.Body.ToString());
        }

        [Fact(DisplayName = "GetCriteriaAsExpression_ShouldReturnCriteriaFromODataAsNotEqualsExpression_WhenODataExpressionModelHas'Ne'OperatorType")]
        public async Task Should_Convert_NeLambdaExpression()
        {
            //Arrange
            var criteria = new List<ODataFilterModel>
            {
                new ODataFilterModel
                {
                    Key = "Label",
                    OperatorType = ODataOperatorType.ne,
                    Value = "lxabel2",
                }
            };

            var oDataModel = new ODataModel
            {
                Filter = criteria
            };

            //Act
            var expression = oDataModel.GetFilterAsExpression<BookModel>();

            //Assert
            Assert.Equal("(x.Label != \"lxabel2\")", expression.Body.ToString());
        }

        [Fact(DisplayName = "GetCriteriaAsExpression_ShouldReturnCriteriaFromODataAsLessThanExpression_WhenODataExpressionModelHas'Lt'OperatorType")]
        public async Task Should_Convert_LtLambdaExpression()
        {
            //Arrange
            var criteria = new List<ODataFilterModel>
            {
                new ODataFilterModel
                {
                    Key = "Age",
                    OperatorType = ODataOperatorType.lt,
                    Value = "7"
                }
            };

            var oDataModel = new ODataModel
            {
                Filter = criteria
            };

            //Act
            var expression = oDataModel.GetFilterAsExpression<ReaderModel>();

            //Assert
            Assert.Equal( "(x.Age < 7)", expression.Body.ToString());
        }

        [Fact(DisplayName = "GetCriteriaAsExpression_ShouldReturnCriteriaFromODataAsGreaterThanExpression_WhenODataExpressionModelHas'Gt'OperatorType")]
        public async Task Should_Convert_GtLambdaExpression()
        {
            //Arrange
            var criteria = new List<ODataFilterModel>
            {
                new ODataFilterModel
                {
                    Key = "Age",
                    OperatorType = ODataOperatorType.gt,
                    Value = "7"
                }
            };

            var oDataModel = new ODataModel
            {
                Filter = criteria
            };

            //Act
            var expression = oDataModel.GetFilterAsExpression<ReaderModel>();

            //Assert
            Assert.Equal("(x.Age > 7)", expression.Body.ToString());
        }

        [Fact(DisplayName = "GetCriteriaAsExpression_ShouldReturnCriteriaFromODataAsGreaterThanOrEqualToExpression_WhenODataExpressionModelHas'Ge'OperatorType")]
        public async Task Should_Convert_GeLambdaExpression()
        {
            //Arrange
            var criteria = new List<ODataFilterModel>
            {
                new ODataFilterModel
                {
                    Key = "Age",
                    OperatorType = ODataOperatorType.ge,
                    Value = "7"
                }
            };

            var oDataModel = new ODataModel
            {
                Filter = criteria
            };

            //Act
            var expression = oDataModel.GetFilterAsExpression<ReaderModel>();

            //Assert
            Assert.Equal("(x.Age >= 7)", expression.Body.ToString());
        }

        [Fact(DisplayName = "GetCriteriaAsExpression_ShouldReturnCriteriaFromODataAsLessThanOrEqualToExpression_WhenODataExpressionModelHas'Le'OperatorType")]
        public async Task Should_Convert_LeLambdaExpression()
        {
            //Arrange
            var criteria = new List<ODataFilterModel>
            {
                new ODataFilterModel
                {
                    Key = "Age",
                    OperatorType = ODataOperatorType.le,
                    Value = "7"
                }
            };

            var oDataModel = new ODataModel
            {
                Filter = criteria
            };

            //Act
            var expression = oDataModel.GetFilterAsExpression<ReaderModel>();

            //Assert
            Assert.Equal("(x.Age <= 7)", expression.Body.ToString());
        }

        [Fact(DisplayName = "GetCriteriaAsExpression_ShouldReturnCriteriaFromODataAsStartswithExpression_WhenODataExpressionModelHas'Startswith'OperatorType")]
        public async Task Should_Convert_StartswithLambdaExpression()
        {
            //Arrange
            var criteria = new List<ODataFilterModel>
            {
                new ODataFilterModel
                {
                    Key = "Label",
                    OperatorType = ODataOperatorType.startswith,
                    Value = "l"
                }
            };

            var oDataModel = new ODataModel
            {
                Filter = criteria
            };

            //Act
            var expression = oDataModel.GetFilterAsExpression<BookModel>();

            //Assert
            Assert.Equal( "x.Label.StartsWith(\"l\")", expression.Body.ToString());
        }

        [Fact(DisplayName = "GetCriteriaAsExpression_ShouldReturnCriteriaFromODataAsEndswithExpression_WhenODataExpressionModelAs'Endswith'OperatorType")]
        public async Task Should_Convert_EndswithLambdaExpression()
        {
            //Arrange
            var criteria = new List<ODataFilterModel>
            {
                new ODataFilterModel
                {
                    Key = "Label",
                    OperatorType = ODataOperatorType.endswith,
                    Value = "l"
                }
            };

            var oDataModel = new ODataModel
            {
                Filter = criteria
            };

            //Act
            var expression = oDataModel.GetFilterAsExpression<BookModel>();

            //Assert
            Assert.Equal( "x.Label.EndsWith(\"l\")", expression.Body.ToString());
        }

        [Fact(DisplayName = "GetCriteriaAsExpression_ShouldReturnCriteriaFromODataAsContainsExpression_WhenODataExpressionModel'Contains'OperatorType")]
        public async Task Should_Convert_ContainsLambdaExpression()
        {
            //Arrange
            var criteria = new List<ODataFilterModel>
            {
                new ODataFilterModel
                {
                    Key = "Label",
                    OperatorType = ODataOperatorType.contains,
                    Value = "l"
                }
            };

            var oDataModel = new ODataModel
            {
                Filter = criteria
            };

            //Act
            var expression = oDataModel.GetFilterAsExpression<BookModel>();

            //Assert
            Assert.Equal("x.Label.Contains(\"l\")", expression.Body.ToString());
        }
    }
}
    