using Microsoft.EntityFrameworkCore;
using RESTfulODataService.Sample.Models;

namespace RESTfulODataService.Sample.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

        public DbSet<BookModel> Books { get; set; }
        public DbSet<ReaderModel> Readers { get; set; }
        public DbSet<AuthorModel> Authors { get; set; }
        public DbSet<BookReaderModel> BookReaders { get; set; }
        public DbSet<ChapterModel> Chapters { get; set; }
    }
}
