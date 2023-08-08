using System;
using System.ComponentModel.DataAnnotations;

namespace CatalogService.Models.Dto
{
    public class ProductUpdateDto
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public string Title { get; set; }

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

        [Required]
        public Guid BrandIdentifier { get; set; }

        [Required]
        public Guid CategoryIdentifier { get; set; }
    }
}
