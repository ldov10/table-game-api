using System;
using System.ComponentModel.DataAnnotations;
using OrderService.Models.Enums;

namespace OrderService.Models.Entities
{
    public class Order : BaseEntity
    {
        [Required]
        public Guid UserIdentifier { get; set; }

        [Required]
        public OrderStates OrderState { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(200)]
        public string Comments { get; set; }

        public long AddressId { get; set; }

        [Required]
        public Address Address { get; set; }
    }
}
