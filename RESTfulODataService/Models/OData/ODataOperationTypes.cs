namespace RESTfulODataService.Models.OData
{
    public enum ODataOperatorType
    {
        eq,
        ne,
        lt,
        gt,
        ge,
        le,
        startswith,
        endswith,
        contains
    }

    public enum ODataComparisonOperatorType
    {
        eq,
        ne,
        lt,
        gt,
        ge,
        le
    }

    public enum ODataSubstringOperatorType
    {
        startswith,
        endswith,
        contains
    }

    public enum FilterJoinType
    {
        DefaultValue,
        And,
        Or
    }
}
