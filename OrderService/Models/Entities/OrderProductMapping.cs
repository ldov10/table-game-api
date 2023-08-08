using System;
using System.ComponentModel.DataAnnotations;

namespace OrderService.Models.Entities
{
    public class OrderProductMapping : BaseEntity
    {
        [Required]
        public Guid OrderIdentifier { get; set; }

        [Required]
        public Guid ProductIdentifier { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

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
