using Microsoft.Extensions.DependencyInjection;
using RESTfulODataService.Services.Parser;
using RESTfulODataService.Services.RegularExpressions;
using RESTfulODataService.Services.RegularExpressions.Singleton;
using RESTfulODataService.Services.Validation.ODataElements;
using RESTfulODataService.Services.Validation.ODataElements.Singleton;
using RESTfulODataService.Services.Validation.ODataParameters;

namespace RESTfulODataService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRESTfulODataService(this IServiceCollection services)
        {
            #region OData Services and Validators

            services.AddSingleton<IODataParametersValidator, ODataParametersValidator>();
            services.AddSingleton<IODataElementsValidatorSingleton, ODataElementsValidatorSingleton>();
            services.AddSingleton<IODataElementsValidator, ODataElementsValidator>();
            services.AddSingleton<IODataParser, ODataParser>();
            services.AddSingleton<IODataService, ODataService>();

            #endregion

            #region Regex and Resources

            services.AddSingleton<IRegexSingleton, RegexSingleton>();
            services.AddSingleton<IRegexUtilizer, RegexUtilizer>();
            
            #endregion
        }
    }
}
