using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RESTfulODataService.Models.OData;
using RESTfulODataService.Models.RESTful;
using RESTfulODataService.Services.Validation.ODataParameters;
using RESTfulODataService.Services.Parser;
using RESTfulODataService.Models.Sorting;

namespace RESTfulODataService.Extensions
{
    public static class ODataExtensions
    {
        /// <summary>
        /// Tries to filter a List of IRESTfulItemResult with OData.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="itemsList">List to be filtered.</param>
        /// <param name="oDataParamsValidator">ODataParametersValidator service.</param>
        /// <param name="oDataParser">ODataParser service.</param>
        /// <param name="httpRequest">HTTPRequest used to retrieve the route being called</param>
        /// <param name="count">Total number of items in the List. If OData $filter was used, it is the total number of items in the list after filtering.</param>
        /// <returns>IEnumerable of items after being filtered by OData.</returns>
        public static IEnumerable<T> TryFilterWithOData<T>(this List<T> itemsList, IODataParametersValidator oDataParamsValidator, IODataParser oDataParser, HttpRequest httpRequest, out int count) where T : class, IRESTfulItemResult
        {
            var isOData = httpRequest.TryParseODataParameters<T>(oDataParamsValidator, oDataParser, out ODataModel parsedOData);

            count = itemsList.Count();

            if (!isOData)
            {
                return itemsList;
            }

            var filteredResult = itemsList.FilterWithOData(parsedOData, out count);

            return filteredResult;
        }

        /// <summary>
        /// Tries to filter a IQueryable of IRESTfulItemResult with OData.
        /// </summary>
        /// <typeparam name="T">IRESTfulItemResult</typeparam>
        /// <param name="itemsList">IQueryable to be filtered.</param>
        /// <param name="oDataParamsValidator">ODataParametersValidator service.</param>
        /// <param name="oDataParser">ODataParser service.</param>
        /// <param name="httpRequest">HTTPRequest used to retrieve the route being called</param>
        /// <param name="count">Total number of items in the IQueryable. If OData $filter was used, it is the total number of items in the IQueryable after filtering.</param>
        /// <returns>Tuple of IEnumerable of items after being filtered by OData and count.</returns>
        public static async Task<(IEnumerable<T> filteredQuery, int count)> TryFilterWithODataAsync<T>(this IQueryable<T> itemsList, IODataParametersValidator oDataParamsValidator, IODataParser oDataParser, HttpRequest httpRequest) where T : class, IRESTfulItemResult
        {
            var isOData = httpRequest.TryParseODataParameters<T>(oDataParamsValidator, oDataParser, out ODataModel parsedOData);

            if (!isOData)
            {
                return (itemsList, itemsList.Count());
            }

            var (query, count) = itemsList.FilterWithODataAsync(parsedOData);

            return (await query, count);
        }

        /// <summary>
        /// Returns boolean value of validity of OData Parameters provided. If they are valid, it outputs an ODataModel representation of the parameters.
        /// </summary>
        /// <typeparam name="T">IRESTfulItemResult</typeparam>
        /// <param name="httpRequest"></param>
        /// <param name="oDataParamsValidator">ODataParametersValidator service.</param>
        /// <param name="oDataParser">ODataParser service.</param>
        /// <param name="oData">Null if OData parameters are invalid.</param>
        /// <returns></returns>
        public static bool TryParseODataParameters<T>(this HttpRequest httpRequest, IODataParametersValidator oDataParamsValidator, IODataParser oDataParser, out ODataModel oData) where T : class, IRESTfulItemResult
        {
            var queryStrings = httpRequest.Query.ToList();
            var oDataQueryStrings = queryStrings.Where(s => s.Key.TrimStart()[0] == '$').ToList();

            oData = null;

            if (oDataQueryStrings.Count == 0)
            {
                return false;
            }

            if (!oDataParamsValidator.IsValid<T>(oDataQueryStrings, out List<ODataFilterModel> oDataExpressions, out List<string> errors))
            {
                return false;
            }

            oData = oDataParser.ParseQuery(oDataQueryStrings);
            oData.Filter = oDataExpressions;

            return true;
        }

        /// <summary>
        /// Filters a List of IRESTfulItemResult by OData.
        /// </summary>
        /// <typeparam name="T">IRESTfulItemResult</typeparam>
        /// <param name="enumerableQuery">List to be filtered by OData.</param>
        /// <param name="oData">OData values to filter the List by.</param>
        /// <param name="count">Total number of items in the List. If OData $filter was used, it is the total number of items in the list after filtering.</param>
        /// <returns>List of items after being filtered by OData.</returns>
        public static List<T> FilterWithOData<T>(this List<T> enumerableQuery, ODataModel oData, out int count) where T : class, IRESTfulItemResult
        {
            var query = enumerableQuery.AsQueryable();

            if (oData.Filter != null && oData.Filter.Count > 0)
            {
                var criteriaExpression = oData.GetFilterAsExpression<T>();

                query = query
                    .Where(criteriaExpression);
            }

            count = query.Count();

            var orderByExpression = oData.GetOrderByAsExpression<T>();
            var orderBy = Sorting<T>.Create(orderByExpression, oData.Reverse);

            if (!orderBy.Reverse)
            {
                query = query.OrderBy(orderBy.OrderBy);
            }

            else
            {
                query = query.OrderByDescending(orderBy.OrderBy);
            }

            if (oData.Skip > 0)
            {
                query = query.Skip(oData.Skip);
            }

            if (oData.Top > 0)
            {
                query = query.Take(oData.Top);
            }

            return query.ToList();
        }

        /// <summary>
        /// Filters a IQueryable of IRESTfulItemResult by OData.
        /// </summary>
        /// <typeparam name="T">IRESTfulItemResult</typeparam>
        /// <param name="enumerableQuery">IQueryable to be filtered by OData.</param>
        /// <param name="oData">OData values to filter the IQueryable by.</param>
        /// <returns>Tuple of List of items after being filtered by OData and count.</returns>
        public static (Task<List<T>> query, int count) FilterWithODataAsync<T>(this IQueryable<T> enumerableQuery, ODataModel oData) where T : class, IRESTfulItemResult
        {
            if (oData.Filter != null && oData.Filter.Count > 0)
            {
                var criteriaExpression = oData.GetFilterAsExpression<T>();

                enumerableQuery = enumerableQuery
                    .Where(criteriaExpression);
            }

            var count = enumerableQuery.Count();
            if (oData.ExpandProperties != null && oData.ExpandProperties.Count() > 0)
            {
                foreach (var property in oData.ExpandProperties)
                {
                    enumerableQuery = enumerableQuery.Include(property);
                }
            }

            var orderByExpression = oData.GetOrderByAsExpression<T>();
            var orderBy = Sorting<T>.Create(orderByExpression, oData.Reverse);

            if (!orderBy.Reverse)
            {
                enumerableQuery = enumerableQuery.OrderBy(orderBy.OrderBy);
            }

            else
            {
                enumerableQuery = enumerableQuery.OrderByDescending(orderBy.OrderBy);
            }

            if (oData.Skip > 0)
            {
                enumerableQuery = enumerableQuery.Skip(oData.Skip);
            }

            if (oData.Top > 0)
            {
                enumerableQuery = enumerableQuery.Take(oData.Top);
            }

            return (enumerableQuery.ToListSafeAsync(), count);
        }

        /// <summary>
        /// Returns $filter as an expression to be used in Linq to filter the query.
        /// </summary>
        /// <typeparam name="T">IRESTfulItemResult</typeparam>
        /// <param name="oData">OData containing the $filter to be used.</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetFilterAsExpression<T>(this ODataModel oData) where T : IRESTfulItemResult
        {
            var oDataExpressionList = oData.Filter;
            var resultExpression = oDataExpressionList[0].ToExpression<T>();

            if (oDataExpressionList.Count > 1)
            {
                for (int i = 1; i < oDataExpressionList.Count; i++)
                {
                    var concatExpr = oDataExpressionList[i].ToExpression<T>();

                    if (oDataExpressionList[i - 1].JoinType == FilterJoinType.And)
                    {
                        var param = resultExpression.Parameters[0];

                        resultExpression = Expression.Lambda<Func<T, bool>>(Expression.AndAlso(resultExpression.Body, Expression.Invoke(concatExpr, param)), param);
                    }

                    if (oDataExpressionList[i - 1].JoinType == FilterJoinType.Or)
                    {
                        var param = resultExpression.Parameters[0];

                        resultExpression = Expression.Lambda<Func<T, bool>>(Expression.OrElse(resultExpression.Body, Expression.Invoke(concatExpr, param)), param);
                    }
                }
            }

            return resultExpression;
        }

        /// <summary>
        /// Returns $orderBy as an expression to be used in Linq to sort the query.
        /// </summary>
        /// <typeparam name="T">IRESTfulItemResult</typeparam>
        /// <param name="oData">OData containing the $orderBy to be used.</param>
        /// <returns></returns>
        public static Expression<Func<T, object>> GetOrderByAsExpression<T>(this ODataModel oData) where T : IRESTfulItemResult
        {
            var arg = Expression.Parameter(typeof(T), "x");
            var property = Expression.PropertyOrField(arg, oData.OrderBy);

            return Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), arg);
        }

        /// <summary>
        /// Converts ODataFilterModel to Expression
        /// </summary>
        /// <typeparam name="T">IRESTfulItemResult</typeparam>
        /// <param name="queryFilter">$filter of the OData query.</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> ToExpression<T>(this ODataFilterModel queryFilter) where T : IRESTfulItemResult
        {
            var arg = Expression.Parameter(typeof(T), "x");

            if (queryFilter.OperatorType == ODataOperatorType.startswith)
            {
                return Expression.Lambda<Func<T, bool>>(Expression.Call(Expression.PropertyOrField(arg, queryFilter.Key),
                    typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }),
                    typeof(string).ToConstantExpression(queryFilter.Value)), arg);
            }

            else if (queryFilter.OperatorType == ODataOperatorType.endswith)
            {
                return Expression.Lambda<Func<T, bool>>(Expression.Call(Expression.PropertyOrField(arg, queryFilter.Key),
                    typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }),
                    typeof(string).ToConstantExpression(queryFilter.Value)), arg);
            }

            else if (queryFilter.OperatorType == ODataOperatorType.contains)
            {
                return Expression.Lambda<Func<T, bool>>(Expression.Call(Expression.PropertyOrField(arg, queryFilter.Key),
                    typeof(string).GetMethod("Contains", new Type[] { typeof(string) }),
                    typeof(string).ToConstantExpression(queryFilter.Value)), arg);
            }

            BinaryExpression expression = null;

            var memberExpression = Expression.PropertyOrField(arg, queryFilter.Key);

            var propertyType = typeof(T).GetProperty(queryFilter.Key).PropertyType;

            if (queryFilter.OperatorType == ODataOperatorType.eq)
            {
                if (propertyType.IsGenericType
                    && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propertyType = propertyType.GetGenericArguments()[0];
                    expression = Expression.Equal(Expression.Property(memberExpression, "Value"), propertyType.ToConstantExpression(queryFilter.Value));
                }

                else
                {
                    expression = Expression.Equal(memberExpression, propertyType.ToConstantExpression(queryFilter.Value));
                }
            }

            else if (queryFilter.OperatorType == ODataOperatorType.lt)
            {
                if (propertyType.IsGenericType
                    && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propertyType = propertyType.GetGenericArguments()[0];
                    expression = Expression.LessThan(Expression.Property(memberExpression, "Value"), propertyType.ToConstantExpression(queryFilter.Value));
                }

                else
                {
                    expression = Expression.LessThan(memberExpression, propertyType.ToConstantExpression(queryFilter.Value));
                }
            }

            else if (queryFilter.OperatorType == ODataOperatorType.gt)
            {
                if (propertyType.IsGenericType
                    && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propertyType = propertyType.GetGenericArguments()[0];
                    expression = Expression.GreaterThan(Expression.Property(memberExpression, "Value"), propertyType.ToConstantExpression(queryFilter.Value));
                }

                else
                {
                    expression = Expression.GreaterThan(memberExpression, propertyType.ToConstantExpression(queryFilter.Value));
                }
            }

            else if (queryFilter.OperatorType == ODataOperatorType.ne)
            {
                if (propertyType.IsGenericType
                    && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propertyType = propertyType.GetGenericArguments()[0];
                    expression = Expression.NotEqual(Expression.Property(memberExpression, "Value"), propertyType.ToConstantExpression(queryFilter.Value));
                }

                else
                {
                    expression = Expression.NotEqual(memberExpression, propertyType.ToConstantExpression(queryFilter.Value));
                }
            }

            else if (queryFilter.OperatorType == ODataOperatorType.ge)
            {
                if (propertyType.IsGenericType
                    && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propertyType = propertyType.GetGenericArguments()[0];
                    expression = Expression.GreaterThanOrEqual(Expression.Property(memberExpression, "Value"), propertyType.ToConstantExpression(queryFilter.Value));
                }

                else
                {
                    expression = Expression.GreaterThanOrEqual(memberExpression, propertyType.ToConstantExpression(queryFilter.Value));
                }
            }

            else if (queryFilter.OperatorType == ODataOperatorType.le)
            {
                if (propertyType.IsGenericType
                    && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propertyType = propertyType.GetGenericArguments()[0];
                    expression = Expression.LessThanOrEqual(Expression.Property(memberExpression, "Value"), propertyType.ToConstantExpression(queryFilter.Value));
                }

                else
                {
                    expression = Expression.LessThanOrEqual(memberExpression, propertyType.ToConstantExpression(queryFilter.Value));
                }
            }

            return Expression.Lambda<Func<T, bool>>(expression, arg);
        }
    }
}
