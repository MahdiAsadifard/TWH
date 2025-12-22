namespace Core.Token
{
    public record JwtCarrierDTO
    {
        public required string AccessToken { get; set; }
        public required string expiry { get; set; }
        public string TokenType { get; set; } = "Bearer";
    }
}
