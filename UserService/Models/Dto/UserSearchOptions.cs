using System;
using System.Collections.Generic;
using UserService.Models.Enums;

namespace UserService.Models.Dto
{
    public class UserSearchOptions
    {
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public Roles? Role { get; set; }

        public bool? IsActive { get; set; }
    }
}
