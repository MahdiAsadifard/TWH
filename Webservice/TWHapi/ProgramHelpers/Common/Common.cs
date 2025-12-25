using Core.Token.Models;
using Models.Models;

namespace TWHapi.ProgramHelpers
{
    public static class Common
    {
        public static JWTClaimItems GetJwtClaimItems(UserRecord user)
        {
            return new JWTClaimItems
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Uri = user.Uri,
                Email = user.Email,
            };
        }
    }
}
