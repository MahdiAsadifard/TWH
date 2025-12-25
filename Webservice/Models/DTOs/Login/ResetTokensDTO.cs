
namespace Models.DTOs.Login
{
    public record ResetTokensDTO
    {
        public required string AccessToken { get; set; }

        public required TimeSpan AccessTokenExpityMinutes { get; set; }

        public required string RefreshToken { get; set; }
        
        public required TimeSpan RefreshTokenExpityMinutes { get; set; }
    }
}
