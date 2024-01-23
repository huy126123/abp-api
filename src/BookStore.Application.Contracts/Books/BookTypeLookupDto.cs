using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace BookStore.Books
{
    public class BookTypeLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
