namespace Core.Token
{
    public class JWTOptions
    {
        public const string OptionName = "JWT";
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Expiry { get; set; } = string.Empty;
        public string EnableJWE { get; set; } = string.Empty;
    }
}
