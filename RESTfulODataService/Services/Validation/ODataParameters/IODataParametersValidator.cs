using RESTfulODataService.Models.OData;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace RESTfulODataService.Services.Validation.ODataParameters
{
    public interface IODataParametersValidator
    {
        bool IsValid<T>(List<KeyValuePair<string, StringValues>> queryList, out List<ODataFilterModel> oDataExpressionsList, out List<string> errors); 
    }
}
