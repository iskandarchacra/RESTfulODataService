using api.emoney.com.Core.Services.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RESTfulODataService.Extensions;
using RESTfulODataService.Sample.Models;

namespace RESTfulODataService.Sample.Services.JsonConverters
{
    public class ReaderJsonConverter : EntityJsonConverter<ReaderModel>
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var reader = GetJObject(value, out JObject readerJObject);

            if (!string.IsNullOrEmpty(reader.Href))
            {
                if (reader.BookReaders == null)
                {
                    readerJObject.AddRelationalJProperty(
                        baseModel: reader,
                        relJPropertyName: JsonConverterConstants.BOOKS,
                        relUriPath: JsonConverterConstants.BOOKS_URI);
                }
            }

            readerJObject.WriteTo(writer);
        }
    }
}
