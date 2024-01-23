using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace BookStore.Authors
{
    
    public class CreateUpdateAuthorDto
    {
        public string Name { get;  set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string ShortBio { get; set; }

    }

    public class AuthorDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string ShortBio { get; set; }

    }

    public class GetAuthorListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; } = "";
    }
}
