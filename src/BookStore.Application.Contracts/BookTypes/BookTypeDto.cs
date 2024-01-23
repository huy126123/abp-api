using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace BookStore.BookTypes
{
    public class CreateUpdateBookTypeDto
    {
        public string Name { get; set; }
        
    }

    public class BookTypeDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        
    }

    public class GetBookTypeListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; } = "";
    }
}
