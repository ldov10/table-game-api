using System;
using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Entities
{
    public class Review : BaseEntity
    {
        [Required]
        public Guid ProductIdentifier { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(300)]
        public string Description { get; set; }

        public long UserId { get; set; }

        [Required]
        public User User { get; set; }
    }
}
