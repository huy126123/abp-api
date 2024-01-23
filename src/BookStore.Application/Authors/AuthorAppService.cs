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

namespace BookStore.Authors
{
    [Authorize(BookStorePermissions.Authors.Default)]
    public class AuthorAppService : CrudAppService<
         Author, //The entity
         AuthorDto, //Used to show books
         Guid, //Primary key of the entity
         GetAuthorListDto, //Used for filtering and sorting
         CreateUpdateAuthorDto>, //Used for creating a book
         IAuthorAppService
    {

        public AuthorAppService(IRepository<Author, Guid> repository)
            : base(repository)
        {
            GetPolicyName = BookStorePermissions.Authors.Default;
            GetListPolicyName = BookStorePermissions.Authors.Default;
            CreatePolicyName = BookStorePermissions.Authors.Create;
            UpdatePolicyName = BookStorePermissions.Authors.Edit;
            DeletePolicyName = BookStorePermissions.Authors.Delete;
        }

        public override async Task<PagedResultDto<AuthorDto>> GetListAsync(GetAuthorListDto input)
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

            return new PagedResultDto<AuthorDto>(
                totalCount,
                ObjectMapper.Map<List<Author>, List<AuthorDto>>(items)
            );
        }

        public override async Task<AuthorDto> GetAsync(Guid id)
        {
            var author = await Repository.GetAsync(id);
            if (author == null)
            {
                throw new EntityNotFoundException(typeof(Author), id);
            }

            // Manually fetch the BookType and Author

            var authorDto = ObjectMapper.Map<Author, AuthorDto>(author);
            // Assuming you have a AuthorName in your BookDto
            return authorDto;
        }

    }
}
