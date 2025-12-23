namespace Core.Token
{
    public record JwtCarrierDTO
    {
        public required string AccessToken { get; set; }
        public required string Expiry { get; set; }
        public string TokenType { get; set; } = "Bearer";
        public required string RefreshToken { get; set; } = string.Empty;
    }
}
