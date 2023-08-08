using System;
using System.ComponentModel.DataAnnotations;

namespace OrderService.Models.Entities
{
    public class BaseEntity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public Guid Identifier { get; set; } = Guid.NewGuid();
    }
}
