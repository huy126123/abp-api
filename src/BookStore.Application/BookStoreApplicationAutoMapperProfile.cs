using AutoMapper;
using BookStore.Authors;
using BookStore.Books;
using BookStore.BookTypes;

namespace BookStore;

public class BookStoreApplicationAutoMapperProfile : Profile
{
    public BookStoreApplicationAutoMapperProfile()
    {
        CreateMap<Book, BookDto>();
        CreateMap<CreateUpdateBookDto, Book>();
        CreateMap<BookType, BookTypeDto>();
        CreateMap<CreateUpdateBookTypeDto, BookType>();

        CreateMap<Author, AuthorDto>();
        CreateMap<CreateUpdateAuthorDto, Author>();

        CreateMap<Author, AuthorLookupDto>();
        CreateMap<BookType, BookTypeLookupDto>();
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
    }
}
