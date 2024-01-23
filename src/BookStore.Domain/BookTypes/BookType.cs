using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace BookStore.BookTypes
{
    public class BookType : AuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        

        // Add any additional properties or methods here
    }
}
