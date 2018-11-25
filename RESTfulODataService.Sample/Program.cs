using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RESTfulODataService.Sample.Data;
using RESTfulODataService.Sample.Models;

namespace RESTfulODataService.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<LibraryDbContext>();
                try
                {
                    AddTestData(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();
        }


        private static void AddTestData(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<LibraryDbContext>();
            context.Database.EnsureCreated();
            var book1 = new BookModel
            {
                Id = "book1",
                Title = "First Book Title",
                TotalPages = 200,
                Ratings = 3.5,
                Type = BookType.Novel,
                Label = "First Book Label",
                Created = DateTime.UtcNow,

            };
            var book2 = new BookModel
            {
                Id = "book2",
                Title = "Second Book Title",
                TotalPages = 100,
                Ratings = 4.8,
                Type = BookType.Fantasy,
                Label = "Second Book Label",
                Created = DateTime.UtcNow.AddDays(-5)
            };
            var book3 = new BookModel
            {
                Id = "book3",
                Title = "Third Book Title",
                TotalPages = 150,
                Ratings = 2.5,
                Type = BookType.Poem,
                Label = "Third Book Label",
                Created = DateTime.UtcNow.AddMonths(-1)
            };

            var chapter1 = new ChapterModel
            {
                Id = "b1c1",
                Name = "First Chapter",
                Book = book1,
                BookId = "book1"
            };

            var chapter2 = new ChapterModel
            {
                Id = "b1c2",
                Name = "Second Chapter",
                Book = book1,
                BookId = "book1"
            };

            var chapter3 = new ChapterModel
            {
                Id = "b2c1",
                Name = "First Chapter",
                Book = book2,
                BookId = "book2"
            };

            var chapter4 = new ChapterModel
            {
                Id = "b2c2",
                Name = "Second Chapter",
                Book = book2,
                BookId = "book2"
            };

            book1.Chapters = new List<ChapterModel>
            {
                chapter1, chapter2
            };

            book2.Chapters = new List<ChapterModel>
            {
                chapter3, chapter4
            };

            var author1 = new AuthorModel
            {
                Id = "author1",
                Name = "John Doe",
                Books = new List<BookModel> { book1, book2, book3 }
            };

            book1.Author = author1;
            book1.AuthorId = author1.Id;
            book2.Author = author1;
            book2.AuthorId = author1.Id;
            book3.Author = author1;
            book3.AuthorId = author1.Id;

            var reader1 = new ReaderModel
            {
                Id = "reader1",
                Name = "Jimmy"
            };

            var reader2 = new ReaderModel
            {
                Id = "reader2",
                Name = "Vanessa"
            };

            var bookReader1 = new BookReaderModel
            {
                Book = book1,
                BookId = book1.Id,
                Reader = reader1,
                ReaderId = reader1.Id
            };

            var bookReader2 = new BookReaderModel
            {
                Book = book1,
                BookId = book1.Id,
                Reader = reader2,
                ReaderId = reader2.Id
            };

            var bookReader3 = new BookReaderModel
            {
                Book = book2,
                BookId = book2.Id,
                Reader = reader2,
                ReaderId = reader2.Id
            };

            context.Books.Add(book1);
            context.Books.Add(book2);
            context.Books.Add(book3);
            context.Chapters.Add(chapter1);
            context.Chapters.Add(chapter2);
            context.Chapters.Add(chapter3);
            context.Chapters.Add(chapter4);
            context.Authors.Add(author1);
            context.Readers.Add(reader1);
            context.Readers.Add(reader2);

            context.BookReaders.Add(bookReader1);
            context.BookReaders.Add(bookReader2);
            context.BookReaders.Add(bookReader3);
     

            context.SaveChanges();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
