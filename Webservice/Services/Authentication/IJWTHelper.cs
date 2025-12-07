using Models.Models;

namespace Services.Authentication
{
    public interface IJWTHelper
    {
        public string GenerateJWTToken(UserRecord user);
    }
}
