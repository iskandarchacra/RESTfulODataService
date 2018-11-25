using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace RESTfulODataService.Extensions
{
    public static class TypeExtensions
    {
        public static Expression ToConstantExpression(this Type queryType, string queryValue)
        {
            if (queryType == typeof(bool))
            {
                return Expression.Constant(bool.Parse(queryValue));
            }

            else if (queryType.GetTypeInfo().IsEnum)
            {
                return Expression.Constant(Enum.Parse(queryType, queryValue));
            }

            else if (queryType == typeof(int))
            {
                return Expression.Constant(int.Parse(queryValue));
            }

            else if (queryType == typeof(double))
            {
                return Expression.Constant(double.Parse(queryValue));
            }

            else if (queryType == typeof(DateTime))
            {
                return Expression.Constant(DateTime.Parse(queryValue));
            }

            return Expression.Constant(queryValue);
        }

        public static PropertyInfo GetNestedProperty(this Type baseType, string propertyName)
        {
            string[] parts = propertyName.Split('.');

            var baseTypeProperty = baseType.GetProperty(parts[0]);

            if (baseTypeProperty != null &&
                baseTypeProperty.PropertyType.IsGenericType 
                && (baseTypeProperty.PropertyType.GetGenericTypeDefinition() == typeof(List<>)))
            {
                return (parts.Length > 1)
                    ? GetNestedProperty(baseType.GetProperty(parts[0]).PropertyType.GetGenericArguments()[0], parts.Skip(1).Aggregate((a, i) => a + "." + i))
                    : baseType.GetProperty(propertyName);
            }

            return (parts.Length > 1)
                ? GetNestedProperty(baseTypeProperty.PropertyType, parts.Skip(1).Aggregate((a, i) => a + "." + i))
                : baseType.GetProperty(propertyName);
        }
    }
}