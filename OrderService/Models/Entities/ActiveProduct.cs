using System.ComponentModel.DataAnnotations;
using System;

namespace OrderService.Models.Entities
{
    public class ActiveProduct : BaseEntity
    {
        [Required]
        public Guid ProductIdentifier { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [Range(0.0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(200)]
        public string ShortDescription { get; set; }
    }
}
