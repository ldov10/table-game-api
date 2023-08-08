using System;
using System.ComponentModel.DataAnnotations;

namespace CartService.Models.Entities
{
    public class Cart : BaseEntity
    {
        [Required]
        public Guid UserIdentifier { get; set; }

        [Required]
        public Guid ProductIdentifier { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}