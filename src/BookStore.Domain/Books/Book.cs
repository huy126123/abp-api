

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace BookStore.Books
{
    public class Book : AuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
       
        public DateTime PublishDate { get; set; }
        public float Price { get; set; }

        public Guid BookTypeId { get; set; }
        public Guid AuthorId { get; set; }

        // public string BookType { get; set; }
        // Add any additional properties or methods here
    }
}
