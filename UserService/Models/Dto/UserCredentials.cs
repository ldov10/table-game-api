using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Dto
{
    public class UserCredentials
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public string Username { get; set;}

        [Required(AllowEmptyStrings = false)]
        [StringLength(200)]
        public string Password { get; set;}
    }
}
