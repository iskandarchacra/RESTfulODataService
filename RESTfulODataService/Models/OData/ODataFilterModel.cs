namespace RESTfulODataService.Models.OData
{
    public class ODataFilterModel
    {
        public string Key { get; set; }

        public ODataOperatorType OperatorType { get;set; }

        public string Value { get; set; }

        public FilterJoinType JoinType { get; set; }
    }
}
