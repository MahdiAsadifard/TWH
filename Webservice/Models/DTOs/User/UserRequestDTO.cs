using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.DTOs.User
{
    public class UserRequestDTO
    {
        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public string? Phone { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

        public bool Disabled { get; set; }
    }
}
