using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Services;

namespace BookStore.Authors
{
    public interface IAuthorAppService : ICrudAppService<
       AuthorDto,
       Guid,
       GetAuthorListDto,
       CreateUpdateAuthorDto,
       CreateUpdateAuthorDto>
    {
    }
}
