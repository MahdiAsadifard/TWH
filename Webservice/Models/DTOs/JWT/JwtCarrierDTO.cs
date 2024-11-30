using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.JWT
{
    public class JwtCarrierDTO
    {
        public string AccessToken { get; set; }
        public string expiry { get; set; }
        public string TokenType { get; set; } = "Bearer";
    }
}
