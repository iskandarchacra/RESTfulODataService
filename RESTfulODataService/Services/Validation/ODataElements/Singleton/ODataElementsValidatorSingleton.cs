using RESTfulODataService.Services.Parser;
using RESTfulODataService.Services.RegularExpressions;
using System.Collections.Concurrent;

namespace RESTfulODataService.Services.Validation.ODataElements.Singleton
{
    public class ODataElementsValidatorSingleton : IODataElementsValidatorSingleton
    {
        private static ConcurrentDictionary<string, ODataElementsValidator> instances = new ConcurrentDictionary<string, ODataElementsValidator>();
        private readonly IRegexUtilizer regexUtilizer;
        private readonly IODataParser oDataParser;

        public ODataElementsValidatorSingleton(IRegexUtilizer regexUtilizer, IODataParser oDataParser)
        {
            this.regexUtilizer = regexUtilizer;
            this.oDataParser = oDataParser;
        }

        public ODataElementsValidator Create(string oDataQueryValue)
        {
            if (instances.ContainsKey(oDataQueryValue) 
                && instances.TryGetValue(oDataQueryValue, out ODataElementsValidator value))
            {
                return new ODataElementsValidator(regexUtilizer, oDataParser, oDataQueryValue);
            }

            var oDataElementsValidator = new ODataElementsValidator(regexUtilizer, oDataParser, oDataQueryValue);

            if (!instances.TryAdd(oDataQueryValue, oDataElementsValidator))
            {
                return Create(oDataQueryValue);
            }

            return oDataElementsValidator;
        }
    }
}
