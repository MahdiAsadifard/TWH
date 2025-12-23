namespace Models.Models
{
    public record UserRefreshTokens
    {
        public required string Token { get; set; }
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public required DateTime ExpiryUtc { get; set; }
    }
}
