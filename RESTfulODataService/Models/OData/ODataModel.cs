using System.Collections.Generic;

namespace RESTfulODataService.Models.OData
{
    public class ODataModel
    {
        public int Top { get; set; } = 25;

        public int Skip { get; set; } = 0;

        public List<ODataFilterModel> Filter { get; set; }

        public string[] ExpandProperties { get; set; }

        public string OrderBy { get; set; }

        public bool Reverse { get; set; } = false;
    }
}
