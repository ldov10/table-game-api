using System.ComponentModel.DataAnnotations;
using System;

namespace CartService.Models.Dto
{
    public class CartItemDto
    {
        [Required]
        public Guid ProductIdentifier { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
