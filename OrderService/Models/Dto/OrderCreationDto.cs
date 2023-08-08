using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace OrderService.Models.Dto
{
    public class OrderCreationDto
    {
        [Required]
        public List<OrderCreationProductDto> Products { get; set; }

        [Required]
        public OrderCreationAddressDto Address { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(200)]
        public string Comments { get; set; }
    }
}
