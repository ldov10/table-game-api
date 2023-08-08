using System;
using System.ComponentModel.DataAnnotations;

namespace CatalogService.Models.Entities
{
    public class Bookmark : BaseEntity
    {
        [Required]
        public Guid UserIdentifier { get; set; }

        public long ProductId { get; set; }

        [Required]
        public Product Product { get; set; }
    }
}
