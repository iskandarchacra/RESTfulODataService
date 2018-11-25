using api.emoney.com.Core.Services.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RESTfulODataService.Extensions;
using RESTfulODataService.Sample.Models;

namespace RESTfulODataService.Sample.Services.JsonConverters
{
    public class AuthorJsonConverter : EntityJsonConverter<AuthorModel>
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var author = GetJObject(value, out JObject authorJObject);

            if (!string.IsNullOrEmpty(author.Href))
            {
                if (author.Books == null)
                {
                    authorJObject.AddRelationalJProperty(
                        baseModel: author,
                        relJPropertyName: JsonConverterConstants.BOOKS,
                        relUriPath: JsonConverterConstants.BOOKS_URI);
                }
            }

            authorJObject.WriteTo(writer);
        }
    }
}
