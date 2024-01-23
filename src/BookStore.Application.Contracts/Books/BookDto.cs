using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using BookStore.BookTypes;


using System.ComponentModel.DataAnnotations;

namespace BookStore.Books
{
    public class CreateUpdateBookDto
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; } = string.Empty;
        
        public DateTime PublishDate { get; set; } = DateTime.Now;
        public float Price { get; set; }
        public Guid BookTypeId { get; set; }
        public Guid AuthorId { get; set; }
        //public string BookType { get; set; }
    }

    public class BookDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        
        public DateTime PublishDate { get; set; }
        public float Price { get; set; }
        public Guid BookTypeId { get; set; }
        public string BookType { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; }

    }

    public class GetBookListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; } = "";
        public string BookType { get; set; } = "";
        public string AuthorName { get; set; } = "";
    }
}
