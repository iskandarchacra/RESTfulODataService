using RESTfulODataService.Models.OData;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using RESTfulODataService.Services.RegularExpressions;

namespace RESTfulODataService.Services.Parser
{
    public class ODataParser : IODataParser
    {
        private readonly IRegexUtilizer regexUtilizer;

        private const string STARTSWITH = "startswith";
        private const string ENDSWITH = "endswith";
        private const string CONTAINS = "contains";
        private const string TOP = "$top";
        private const string SKIP = "$skip";
        private const string ORDERBY = "$orderby";
        private const string EXPAND = "$expand";
        private const string ASC = "asc";

        public ODataParser(IRegexUtilizer regexUtilizer)
        {
            this.regexUtilizer = regexUtilizer;
        }

        public ODataFilterModel ParseOData(string queryFilter)
        {
            var queryValue = regexUtilizer.SplitOnApostrophes(queryFilter);

            if (queryValue.Length == 1)
            {
                queryValue = regexUtilizer.SplitOnSpaceFollowedByDigit(queryFilter.Trim());
            }

            string [] query = null;
            var queryValLower = queryValue[0].ToLowerInvariant();

            if (queryValLower.Contains(STARTSWITH) 
                || queryValLower.Contains(ENDSWITH) 
                || queryValLower.Contains(CONTAINS))
            {
                query = regexUtilizer.SplitOnWords(queryValue[0].Trim());

                return new ODataFilterModel
                {
                    Key = query[1].First().ToString().ToUpper() + string.Join("", query[1].Skip(1)),
                    OperatorType = (ODataOperatorType)Enum.Parse(typeof(ODataOperatorType), query[0].ToLower()),
                    Value = queryValue[1]
                };
            }

            else
            {
                query = regexUtilizer.SplitOnWhiteSpace(queryValue[0]);
            }

            if (query.Length > 2 && bool.TryParse(query[2], out bool result))
            {
                return new ODataFilterModel
                {
                    Key = query[0].First().ToString().ToUpper() + string.Join("", query[0].Skip(1)),
                    OperatorType = (ODataOperatorType) Enum.Parse( typeof(ODataOperatorType) , query[1].ToLower()),
                    Value = query[2]
                };
            }

            if (queryValue.Length < 2)
            {
                return null;
            }

            return new ODataFilterModel
            {
                Key = query[0].First().ToString().ToUpper() + string.Join("", query[0].Skip(1)),
                OperatorType = (ODataOperatorType)Enum.Parse(typeof(ODataOperatorType), query[1].ToLower()),
                Value = queryValue[1]
            };
        }

        public ODataModel ParseQuery(List<KeyValuePair<string, StringValues>> queryList)
        {
            var oDataModel = new ODataModel
            {
                OrderBy = "created"
            };

            foreach(var query in queryList)
            {
                var key = query.Key.ToLowerInvariant();

                if (key.Equals(TOP))
                {
                    oDataModel.Top = int.Parse(query.Value);
                }

                else if (key.Equals(SKIP))
                {
                    oDataModel.Skip = int.Parse(query.Value);
                }

                else if (key.Equals(ORDERBY))
                {
                    oDataModel.OrderBy = query.Value;

                    var sort = query.Value.ToString().Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

                    if (sort.Count() == 2)
                    {
                        if (sort[1].ToLower().Equals(ASC))
                        {
                            oDataModel.Reverse = false;
                        }

                        else
                        {
                            oDataModel.Reverse = true;
                        }

                        oDataModel.OrderBy = sort[0];
                    }
                }

                else if (key.Equals(EXPAND))
                {
                    var propertiesToExpand = regexUtilizer.SplitOnCommasAndRemoveWhiteSpace(query.Value);

                    oDataModel.ExpandProperties = propertiesToExpand;
                }
            }

            return oDataModel;
        }
    }
}