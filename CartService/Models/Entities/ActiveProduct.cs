using System;
using System.ComponentModel.DataAnnotations;

namespace CartService.Models.Entities
{
    public class ActiveProduct : BaseEntity
    {
        [Required]
        public Guid ProductIdentifier { get; set; }
    }
}
