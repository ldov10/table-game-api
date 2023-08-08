using System.ComponentModel.DataAnnotations;

namespace CatalogService.Models.Entities
{
    public class Image : BaseEntity
    {
        [Required]
        public byte[] Data { get; set; }

        public long ProductId { get; set; }

        [Required]
        public Product Product { get; set; }
    }
}
