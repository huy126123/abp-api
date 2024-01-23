using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using BookStore.Authors;
using BookStore.Books;
using BookStore.BookTypes;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace BookStore;

public class BookStoreDataSeederContributor
    : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Book, Guid> _bookRepository;
    private readonly IRepository<BookType, Guid> _bookTypeRepository;
    private readonly IRepository<Author, Guid> _authorRepository;

    public BookStoreDataSeederContributor(IRepository<Book, Guid> bookRepository, IRepository<BookType, Guid> bookTypeRepository, IRepository<Author, Guid> authorRepository)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;

        _bookTypeRepository = bookTypeRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _bookRepository.GetCountAsync() > 0)
        {
            return;
        }
        var action = await _bookTypeRepository.InsertAsync(
                new BookType
                {
                    Name = "Action",

                },
                autoSave: true
            );
        var fantasy = await _bookTypeRepository.InsertAsync(
                new BookType
                {
                    Name = "Fantasy",

                },
                autoSave: true
            );
        var orwell = await _authorRepository.InsertAsync(
            new Author
            {
                Name = "George Orwell",
                BirthDate = new DateTime(1903, 06, 25),
                ShortBio = "Orwell produced literary criticism and poetry, fiction and polemical journalism; and is best known for the allegorical novella Animal Farm (1945) and the dystopian novel Nineteen Eighty-Four (1949)."

            },
                autoSave: true  
        );

        var douglas = await _authorRepository.InsertAsync(
            new Author
            {
                Name = "Douglas Adams",
                BirthDate = new DateTime(1952, 03, 11),
                ShortBio = "Douglas Adams was an English author, screenwriter, essayist, humorist, satirist and dramatist. Adams was an advocate for environmentalism and conservation, a lover of fast cars, technological innovation and the Apple Macintosh, and a self-proclaimed 'radical atheist'."

            },
                autoSave: true
        );


        await _bookRepository.InsertAsync(
            new Book
            {
                AuthorId = orwell.Id,
                BookTypeId = action.Id, 
                Name = "1984",
                
                PublishDate = new DateTime(1949, 6, 8),
                Price = 19.84f
            },
            autoSave: true
        );
        await _bookRepository.InsertAsync(
            new Book
            {
                AuthorId = douglas.Id,
                BookTypeId = fantasy.Id, 
                Name = "The Hitchhiker's Guide to the Galaxy",
                
                PublishDate = new DateTime(1995, 9, 27),
                Price = 42.0f
            },
            autoSave: true
        );
          
        
        
    }
}
