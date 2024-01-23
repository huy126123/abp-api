using BookStore.Authors;
using BookStore.Books;
using BookStore.BookTypes;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace BookStore.MongoDB;

[ConnectionStringName("Default")]
public class BookStoreMongoDbContext : AbpMongoDbContext
{
    /* Add mongo collections here. Example:
     * public IMongoCollection<Question> Questions => Collection<Question>();
     */
    public IMongoCollection<Book> Books => Collection<Book>();
    public IMongoCollection<BookType> BookTypes => Collection<BookType>();
    public IMongoCollection<Author> Authors => Collection<Author>();
    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        //modelBuilder.Entity<YourEntity>(b =>
        //{
        //    //...
        //});
    }
}
