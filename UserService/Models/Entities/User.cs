using System.ComponentModel.DataAnnotations;
using UserService.Models.Enums;

namespace UserService.Models.Entities
{
    public class User : BaseEntity
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(200)]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        [EmailAddress]
        [RegularExpression(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z")]
        public string Email { get; set; }

        [Required]
        public Roles Role { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public RefreshToken RefreshToken { get; set; }
    }
}
