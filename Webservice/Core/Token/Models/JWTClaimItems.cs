using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Token.Models
{
    public record JWTClaimItems
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Uri { get; set; }
        public required string Email { get; set; }

    }
}
