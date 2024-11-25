using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.DTOs.User
{
    public class UserResponseDTO
    {
        public string Uri { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? Phone { get; set; }

        public string Email { get; set; }

        public bool Disabled { get; set; }
    }
}
