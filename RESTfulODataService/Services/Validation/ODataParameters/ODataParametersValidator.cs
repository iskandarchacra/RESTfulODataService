using RESTfulODataService.Models.OData;
using RESTfulODataService.Services.Validation.ODataElements.Singleton;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace RESTfulODataService.Services.Validation.ODataParameters
{
    public class ODataParametersValidator : IODataParametersValidator
    {
        private readonly IODataElementsValidatorSingleton oDataElementsValidatorSingleton;

        public const string TOP = "$top";
        public const string SKIP = "$skip";
        public const string ORDER_BY = "$orderby";
        public const string FILTER = "$filter";
        public const string EXPAND = "$expand";

        public ODataParametersValidator(IODataElementsValidatorSingleton oDataElementsValidatorSingleton)
        {
            this.oDataElementsValidatorSingleton = oDataElementsValidatorSingleton;
        }

        public bool IsValid<T>(List<KeyValuePair<string, StringValues>> queryList, out List<ODataFilterModel> oDataExpressionsList, out List<string> errors)
        {
            var oDataKeysDictionary = new Dictionary<string, StringValues>();
            errors = new List<string>();
            oDataExpressionsList = new List<ODataFilterModel>();

            for (var i = 0; i < queryList.Count; i++)
            {
                var key = queryList[i].Key.ToLower().Trim();
                var value = queryList[i].Value;
                var oDataElementsValidator = oDataElementsValidatorSingleton.Create(value);

                if (oDataKeysDictionary.TryGetValue(key, out StringValues existingValue))
                {
                    errors.Add("The query parameter " + key + " has been provided more than once.");

                    return false;
                }

                else
                {
                    oDataKeysDictionary.TryAdd(key, value);
                }

                if (!key.Equals(TOP) 
                    && !key.Equals(SKIP)
                    && !key.Equals(ORDER_BY) 
                    && !key.Equals(FILTER)
                    && !key.Equals(EXPAND))
                {
                    errors.Add("The query key " + key + " provided is not a valid OData parameter.");

                    return false;
                }

                if ((key.Equals(TOP) || key.Equals(SKIP)) 
                    && !int.TryParse(value.ToString(), out int intValue))
                {
                    errors.Add("Please provide an integer value after using $top or $skip OData parameter keys.");

                    return false;
                }

                if (key.Equals(FILTER) 
                    && !oDataElementsValidator.IsValidFilter<T>(out oDataExpressionsList))
                {
                    errors.Add("Invalid OData parameter values after key $filter");

                    return false;
                }

                if (key.Equals(ORDER_BY) 
                    && !oDataElementsValidator.IsValidOrderByElements<T>())
                {
                    errors.Add("Invalid OData parameter values after key $orderBy");

                    return false;
                }

                if (key.Equals(EXPAND)
                    && !oDataElementsValidator.IsValidExpand<T>())
                {
                    errors.Add("Invalid property provided after key $expand.");

                    return false;
                }
            }

            return true;
        }
    }
}