using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CatalogService.Models.Entities
{
    public class Product: BaseEntity
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [Range(0.0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(2000)]
        public string Description { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(200)]
        public string ShortDescription { get; set; }

        [Required]
        [Range(0, 18)]
        public int Age { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int MinPlayers { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int MaxPlayers { get; set; }

        public long BrandId { get; set; }

        public long CategoryId { get; set; }

        [Required]
        [Range(0, 5)]
        public double Rating { get; set; } = 0;

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [Required]
        public Brand Brand { get; set; }

        [Required]
        public Category Category { get; set; }
    }
}
