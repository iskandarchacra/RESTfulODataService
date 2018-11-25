using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace RESTfulODataService.Services.RegularExpressions.Singleton
{
    /// <summary>
    /// 
    /// </summary>
    public class RegexSingleton : IRegexSingleton
    {
        private static ConcurrentDictionary<string, Regex> instances = new ConcurrentDictionary<string, Regex>();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="regexExpression"></param>
        /// <returns></returns>
        public Regex Create(string regexExpression)
        {
            if (instances.ContainsKey(regexExpression) && instances.TryGetValue(regexExpression, out Regex value))
            {
                return value;
            }

            var regex = new Regex(regexExpression);

            if (!instances.TryAdd(regexExpression, regex))
            {
                return Create(regexExpression);
            }

            return regex;           
        }
    }
}
