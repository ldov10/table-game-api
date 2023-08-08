using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace OrderService.Models.Dto
{
    public class OrderCreationProductDto
    {
        [Required]
        public Guid ProductIdentifier { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
