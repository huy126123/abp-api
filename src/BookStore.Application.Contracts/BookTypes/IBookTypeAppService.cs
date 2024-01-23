using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Services;

namespace BookStore.BookTypes
{
    public interface IBookTypeAppService : ICrudAppService<
       BookTypeDto,
       Guid,
       GetBookTypeListDto,
       CreateUpdateBookTypeDto,
       CreateUpdateBookTypeDto>
    {
    }
}
