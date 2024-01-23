using BookStore.Authors;
using BookStore.BookTypes;
using BookStore.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace BookStore.Books
{
    //[Authorize(BookStorePermissions.Books.Default)]
    public class BookAppService : CrudAppService<
         Book, //The entity
         BookDto, //Used to show books
         Guid, //Primary key of the entity
         GetBookListDto, //Used for filtering and sorting
         CreateUpdateBookDto>, //Used for creating a book
         IBookAppService
    {
        private readonly IRepository<BookType, Guid> _bookTypeRepository;
        private readonly IRepository<Author, Guid> _authorRepository;
        public BookAppService( IRepository<Book, Guid> repository, IRepository<BookType, Guid> bookTypeRepository, IRepository<Author, Guid> authorRepository)
            : base(repository)
        {
            _authorRepository = authorRepository;
            _bookTypeRepository = bookTypeRepository;
            GetPolicyName = BookStorePermissions.Books.Default;
            GetListPolicyName = BookStorePermissions.Books.Default;
            CreatePolicyName = BookStorePermissions.Books.Create;
            UpdatePolicyName = BookStorePermissions.Books.Edit;
            DeletePolicyName = BookStorePermissions.Books.Delete;
        }
        
        public override async Task<BookDto> GetAsync(Guid id)
        {
            var book = await Repository.GetAsync(id);
            if (book == null)
            {
                throw new EntityNotFoundException(typeof(Book), id);
            }

            // Manually fetch the BookType and Author
            var author = await _authorRepository.GetAsync(book.AuthorId);
            var bookType = await _bookTypeRepository.GetAsync(book.BookTypeId);
            var bookDto = ObjectMapper.Map<Book, BookDto>(book);
            bookDto.BookType = bookType?.Name; // Assuming you have a BookType in your BookDto
            bookDto.AuthorName = author?.Name;// Assuming you have a AuthorName in your BookDto
            return bookDto;
        }


        public override async Task<PagedResultDto<BookDto>> GetListAsync(GetBookListDto input)
        {
            var queryable = await Repository.GetQueryableAsync();

            
            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                var filter = input.Filter.ToLower();
                queryable = queryable.Where(book =>
                    book.Name.ToLower().Contains(filter));
            }

            if (!string.IsNullOrWhiteSpace(input.BookType))
            {
                var qr = await _bookTypeRepository
                    .GetQueryableAsync();
                var bookTypeIds = qr
                    .Where(bt => bt.Name.ToLower().Contains(input.BookType.ToLower()))
                    .Select(bt => bt.Id)
                    .ToList();

                queryable = queryable.Where(book => bookTypeIds.Contains(book.BookTypeId));
            }
            if (!string.IsNullOrWhiteSpace(input.AuthorName))
            {
                var qr = await _authorRepository
                    .GetQueryableAsync();
                var authorIds = qr
                    .Where(bt => bt.Name.ToLower().Contains(input.AuthorName.ToLower()))
                    .Select(bt => bt.Id)
                    .ToList();

                queryable = queryable.Where(book => authorIds.Contains(book.AuthorId));
            }

            // Default sorting
            if (string.IsNullOrWhiteSpace(input.Sorting))
            {
                queryable = queryable.OrderBy(book => book.Name);
            }
            else
            {
                // Apply dynamic sorting based on input.Sorting
                // You need to implement this part based on your sorting requirements
            }
            //var items = await AsyncExecuter.ToListAsync(
            //  queryable.Skip(input.SkipCount)
            //             .Take(input.MaxResultCount)
            //);

            var totalCount = await AsyncExecuter.CountAsync(queryable);
            var books = await AsyncExecuter.ToListAsync(queryable.Skip(input.SkipCount)
                         .Take(input.MaxResultCount));

            // Manually populate BookType names
            var bookDtos = ObjectMapper.Map<List<Book>, List<BookDto>>(books);
            foreach (var bookDto in bookDtos)
            {
                var author = await _authorRepository.GetAsync(bookDto.AuthorId);
                var bookType = await _bookTypeRepository.GetAsync(bookDto.BookTypeId);
                bookDto.BookType = bookType?.Name; // Assuming you have a BookTypeName in your BookDto
                bookDto.AuthorName = author?.Name;
            }

            return new PagedResultDto<BookDto>(totalCount, bookDtos);
        }


        public async Task<ListResultDto<AuthorLookupDto>> GetAuthorLookupAsync()
        {
            var authors = await _authorRepository.GetListAsync();

            return new ListResultDto<AuthorLookupDto>(
                ObjectMapper.Map<List<Author>, List<AuthorLookupDto>>(authors)
            );
        }
        public async Task<ListResultDto<BookTypeLookupDto>> GetBookTypeLookupAsync()
        {
            var bookTypes = await _bookTypeRepository.GetListAsync();

            return new ListResultDto<BookTypeLookupDto>(
                ObjectMapper.Map<List<BookType>, List<BookTypeLookupDto>>(bookTypes)
            );
        }
        [Authorize(BookStorePermissions.Books.Create)]

        public override async Task<BookDto> CreateAsync(CreateUpdateBookDto input)
        {
            // Ensure the BookType exists
            var bookType = await _bookTypeRepository.FindAsync(input.BookTypeId);
            if (bookType == null)
            {
                throw new EntityNotFoundException(typeof(BookType), input.BookTypeId);
            }

            var author = await _authorRepository.FindAsync(input.AuthorId);
            if (author == null)
            {
                throw new EntityNotFoundException(typeof(Author), input.AuthorId);
            }

            // Map and create the Book
            var book = ObjectMapper.Map<CreateUpdateBookDto, Book>(input);
            await Repository.InsertAsync(book, autoSave: true);

            return ObjectMapper.Map<Book, BookDto>(book);
        }
    }
}
