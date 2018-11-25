using api.emoney.com.Core.Services.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RESTfulODataService.Extensions;
using RESTfulODataService.Sample.Models;

namespace RESTfulODataService.Sample.Services.JsonConverters
{
    public class BookJsonConverter : EntityJsonConverter<BookModel>
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var book = GetJObject(value, out JObject bookJOBject);

            if (!string.IsNullOrEmpty(book.Href))
            {
                if (book.BookReaders == null)
                {
                    bookJOBject.AddRelationalJProperty(
                        baseModel: book,
                        relJPropertyName: JsonConverterConstants.READERS,
                        relUriPath: JsonConverterConstants.READERS_URI);
                }

                if (book.Chapters == null)
                {
                    bookJOBject.AddRelationalJProperty(
                        baseModel: book,
                        relJPropertyName: JsonConverterConstants.CHAPTERS,
                        relUriPath: JsonConverterConstants.CHAPTERS_URI);
                }
            }

            bookJOBject.RemoveJsonProperty(book.Author, JsonConverterConstants.AUTHOR);
            bookJOBject.RemoveJsonProperty(book.AuthorId, JsonConverterConstants.AUTHOR_ID);
            bookJOBject.WriteTo(writer);
        }
    }
}
