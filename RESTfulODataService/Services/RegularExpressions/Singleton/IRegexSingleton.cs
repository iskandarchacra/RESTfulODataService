using System.Text.RegularExpressions;

namespace RESTfulODataService.Services.RegularExpressions.Singleton
{
    public interface IRegexSingleton
    {
        Regex Create(string regexExpression);
    }
}
