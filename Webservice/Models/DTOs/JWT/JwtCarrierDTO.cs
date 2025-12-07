namespace Models.DTOs.JWT
{
    public class JwtCarrierDTO
    {
        public string AccessToken { get; set; }
        public string expiry { get; set; }
        public string TokenType { get; set; } = "Bearer";
    }
}
