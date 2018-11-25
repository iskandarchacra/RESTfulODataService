using RESTfulODataService.Models.OData;
using System.Collections.Generic;

namespace RESTfulODataService.Services.Validation.ODataElements
{
    public interface IODataElementsValidator
    {
        bool IsValidFilter<T>(out List<ODataFilterModel> oDataExpressionsList);

        bool IsValidOrderByElements<T>();

        bool IsValidExpand<T>();
    }
}
