using Newtonsoft.Json.Linq;
using RESTfulODataService.Models.RESTful;
using System.Collections.Generic;

namespace RESTfulODataService.Extensions
{
    public static class JObjectExtensions
    {
        private const string DATE_MODIFIED = "modified";

        public static void AddRelationalJProperty(this JObject jObject, IRESTfulItemResult baseModel, string relJPropertyName, string relUriPath)
        {
            var newProperty = new JProperty(relJPropertyName, JObject.Parse($"{{\"href\": \"{baseModel.Href}\\{relUriPath}\"}}"));

            jObject.Add(newProperty);
        }

        public static void RemoveJsonListProperty<T>(this JObject jsonObject, List<T> manyToManyProperty, string propertyName)
        {
            if (manyToManyProperty != null)
            {
                jsonObject.Property(propertyName)
                    .Remove();
            }
        }

        public static void RemoveJsonProperty<T>(this JObject jsonObject, T property, string propertyName)
        {
            if (property != null)
            {
                jsonObject.Property(propertyName)
                    .Remove();
            }
        }
    }
}
