namespace Models.DTOs.Login
{
    public record LoginRequestDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
