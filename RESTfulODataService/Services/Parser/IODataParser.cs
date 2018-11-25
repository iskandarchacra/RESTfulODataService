using RESTfulODataService.Models.OData;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace RESTfulODataService.Services.Parser
{
    public interface IODataParser
    {
        ODataFilterModel ParseOData(string filterQuery);

        ODataModel ParseQuery(List<KeyValuePair<string, StringValues>> queryList);
    }
}
