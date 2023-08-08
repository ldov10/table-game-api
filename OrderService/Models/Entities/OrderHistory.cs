using System.ComponentModel.DataAnnotations;
using System;
using OrderService.Models.Enums;

namespace OrderService.Models.Entities
{
    public class OrderHistory : BaseEntity
    {
        [Required]
        public Guid UserIdentifier { get; set; }

        [Required]
        public UserRoles UserRole { get; set; }

        [Required]
        public OrderStates OldState { get; set; }

        [Required]
        public OrderStates NewState { get; set;}

        public long OrderId { get; set; }

        [Required]
        public Order Order { get; set; }
    }
}
