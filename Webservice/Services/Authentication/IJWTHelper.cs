using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Authentication
{
    public interface IJWTHelper
    {
        public string GenerateJWTToken(UserRecord user);
    }
}
