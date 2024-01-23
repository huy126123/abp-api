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

namespace BookStore.BookTypes
{
    [Authorize(BookStorePermissions.BookTypes.Default)]

    public class BookTypeAppService : CrudAppService<
         BookType, //The entity
         BookTypeDto, //Used to show books
         Guid, //Primary key of the entity
         GetBookTypeListDto, //Used for filtering and sorting
         CreateUpdateBookTypeDto>, //Used for creating a book
         IBookTypeAppService
    {

        public BookTypeAppService(IRepository<BookType, Guid> repository)
            : base(repository)
        {
            GetPolicyName = BookStorePermissions.BookTypes.Default;
            GetListPolicyName = BookStorePermissions.BookTypes.Default;
            CreatePolicyName = BookStorePermissions.BookTypes.Create;
            UpdatePolicyName = BookStorePermissions.BookTypes.Edit;
            DeletePolicyName = BookStorePermissions.BookTypes.Delete;
        }

        
        public override async Task<PagedResultDto<BookTypeDto>> GetListAsync(GetBookTypeListDto input)
        {
            var queryable = await Repository.GetQueryableAsync();

            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                var filter = input.Filter.ToLower();
                queryable = queryable.Where(bookType =>
                    bookType.Name.ToLower().Contains(filter));
            }

            // Default sorting
            if (string.IsNullOrWhiteSpace(input.Sorting))
            {
                queryable = queryable.OrderBy(bookType => bookType.Name);
            }
            else
            {
                // Apply dynamic sorting based on input.Sorting
                // You need to implement this part based on your sorting requirements
            }

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            var items = await AsyncExecuter.ToListAsync(
                queryable.Skip(input.SkipCount).Take(input.MaxResultCount));

            return new PagedResultDto<BookTypeDto>(
                totalCount,
                ObjectMapper.Map<List<BookType>, List<BookTypeDto>>(items)
            );
        }

        public override async Task<BookTypeDto> GetAsync(Guid id)
        {
            var bookType = await Repository.GetAsync(id);
            if (bookType == null)
            {
                throw new EntityNotFoundException(typeof(BookType), id);
            }

            // Manually fetch the BookType and Author

            var bookTypeDto = ObjectMapper.Map<BookType, BookTypeDto>(bookType);
            // Assuming you have a AuthorName in your BookDto
            return bookTypeDto;
        }

    }
}
