namespace Core.Token
{
    public record JWTOptions
    {
        public const string OptionName = "JWT";
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string ExpiryInMinutes { get; set; } = string.Empty;
        public string EnableJWE { get; set; } = string.Empty;
        public string RefreshTokenExpiryInMinutes { get; set; } = string.Empty;
        public int RefreshTokenMinLength { get; set; }
    }
}
