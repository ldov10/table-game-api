using System;
using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Entities
{
    public class RefreshToken : BaseEntity
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(88)]
        public string Token { get; set; }

        [Required]
        public DateTime CreatedDtm { get; set; }

        public long UserId { get; set; }

        [Required]
        public User User { get; set; }
    }
}
