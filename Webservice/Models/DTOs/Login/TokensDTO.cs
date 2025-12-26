
namespace Models.DTOs.Login
{
    public record TokensDTO
    {
        public required string AccessToken { get; set; }

        public required DateTime AccessTokenExpityUTC { get; set; }

        public string TokenType { get; set; } = "Bearer";

        public required string RefreshToken { get; set; }
        
        public required DateTime RefreshTokenExpityUTC { get; set; }
    }
}
