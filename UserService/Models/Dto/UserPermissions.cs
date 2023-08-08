using UserService.Models.Enums;

namespace UserService.Models.Dto
{
    public class UserPermissions
    {
        public Roles Role { get; set; }

        public bool IsActive { get; set; }
    }
}
