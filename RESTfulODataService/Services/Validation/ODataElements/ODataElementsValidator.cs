using RESTfulODataService.Extensions;
using RESTfulODataService.Models.OData;
using RESTfulODataService.Services.Parser;
using RESTfulODataService.Services.RegularExpressions;
using System;
using System.Collections.Generic;

namespace RESTfulODataService.Services.Validation.ODataElements
{
    public class ODataElementsValidator : IODataElementsValidator
    {
        private readonly IRegexUtilizer regexUtilizer;
        private readonly IODataParser oDataParser;
        public const string ASCENDING = "asc";
        public const string DESCENDING = "desc";
        private readonly string[] splitOnAndOrOr;
        private readonly string[] splitOrderByOData;
        private readonly string[] splitOnComma;
        private readonly string splitOrderBySecondElement;

        private static HashSet<(string, string)> propertyTypeSet = new HashSet<(string, string)>();

        public ODataElementsValidator(IRegexUtilizer regexUtilizer, IODataParser oDataParser, string queryValue)
        {
            this.regexUtilizer = regexUtilizer;
            this.oDataParser = oDataParser;
            this.splitOnAndOrOr = regexUtilizer.SplitOnAndOrOrOutsideApostrophes(queryValue);
            this.splitOrderByOData = regexUtilizer.SplitOnWords(queryValue);
            this.splitOnComma = regexUtilizer.SplitOnCommasAndRemoveWhiteSpace(queryValue);
            this.splitOrderBySecondElement = "";
                
            if (splitOrderByOData.Length > 1)
            {
                splitOrderBySecondElement = splitOrderByOData[1];
            }
        }

        public bool IsValidFilter<T>(out List<ODataFilterModel> oDataExpressionsList)
        {
            oDataExpressionsList = new List<ODataFilterModel>();

            for (var i = 0; i < splitOnAndOrOr.Length; i = i + 2)
            {
                var splitComparisonOData = regexUtilizer
                    .SplitOnWhiteSpacesOutsideApostrophes(splitOnAndOrOr[i].Trim());
                var splitSubstringOData = regexUtilizer
                    .SplitOnApostrophes(splitOnAndOrOr[i].Trim());
                var trimmedODataSubstringKey = splitSubstringOData[0].Trim();
                var splitODataSubstringKey = trimmedODataSubstringKey.Split('(')[0];
                var splitODataComparisonKey = "";

                if (splitComparisonOData.Length > 1)
                {
                    splitODataComparisonKey = splitComparisonOData[1];
                }

                if (!Enum.TryParse(splitODataSubstringKey, out ODataSubstringOperatorType substringType)
                    && !Enum.TryParse(splitODataComparisonKey, out ODataComparisonOperatorType comparisonType))
                {
                    return false;
                }

                else if (Enum.TryParse(splitODataSubstringKey, out substringType)
                     && (splitSubstringOData.Length != 3
                     || !IsValidSubstringElements(splitSubstringOData, trimmedODataSubstringKey, splitSubstringOData[2].Trim())))
                {
                    return false;
                }

                else if (Enum.TryParse(splitODataComparisonKey, out comparisonType)
                     && (splitComparisonOData.Length != 3
                     || !IsValidComparisonElements(splitComparisonOData, splitComparisonOData[2].Trim())))
                {
                    return false;
                }

                var parsedOData = oDataParser.ParseOData(splitOnAndOrOr[i]);

                if (parsedOData == null)
                {
                    return false;
                }

                if (!IsValidProperty<T>(parsedOData.Key, out List<string> errors))
                {
                    return false;
                }

                if ((i + 1) < splitOnAndOrOr.Length && (i + 1) % 2 == 1)
                {
                    if (splitOnAndOrOr[i + 1].ToLowerInvariant().Equals(" and "))
                    {
                        parsedOData.JoinType = FilterJoinType.And;
                    }

                    if (splitOnAndOrOr[i + 1].ToLowerInvariant().Equals(" or "))
                    {
                        parsedOData.JoinType = FilterJoinType.Or;
                    }
                }

                oDataExpressionsList.Add(parsedOData);
            }

            return true;
        }

        public bool IsValidExpand<T>() 
        {
            foreach (var property in splitOnComma)
            {
                if (!IsValidProperty<T>(property.Trim(), out List<string> errors))
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsValidOrderByElements<T>() 
        {
            if (!(splitOrderByOData.Length >= 1 && splitOrderByOData.Length <= 2))
            {
                return false;
            }

            splitOrderByOData[0] = char.ToUpper(splitOrderByOData[0][0]) + splitOrderByOData[0].Substring(1);

            if (!IsValidProperty<T>(splitOrderByOData[0], out List<string> errors))
            {
                return false;
            }

            var orderArrangement = splitOrderBySecondElement.ToLower();

            if (splitOrderByOData.Length == 1
                || (orderArrangement.Equals(ASCENDING)
                || orderArrangement.Equals(DESCENDING)))
            {
                return true;
            }

            return false;
        }

        private bool IsValidComparisonElements(string[] splitComparisonOData, string splitComparisonThirdElement)
        {
            var isBeginningWithApostrophe = splitComparisonThirdElement.StartsWith('\'');
            var isEndingWithApostrophe = splitComparisonThirdElement.EndsWith('\'');

            if ((isBeginningWithApostrophe && !isEndingWithApostrophe)
                || (!isBeginningWithApostrophe && isEndingWithApostrophe))
            {
                return false;
            }

            return true;
        }

        private bool IsValidSubstringElements(string[] splitSubstringOdata, string trimmedODataSubstringKey, string splitSubstringThirdElement)
        {
            if (!trimmedODataSubstringKey.EndsWith(',')
                || !splitSubstringThirdElement.Equals(")"))
            {
                return false;
            }

            return true;
        }

        private bool IsValidProperty<T>(string propertyName, out List<string> errors)
        {
            errors = new List<string>();

            var entityType = typeof(T);
            var keyValueTuple = (entityType.ToString(), propertyName);

            if (propertyTypeSet.Contains(keyValueTuple))
            {
                return true;
            }

            var property = entityType.GetNestedProperty(propertyName);

            if (property == null)
            {
                return false;
            }

            propertyTypeSet.Add(keyValueTuple);

            return true;
        }
    }
}