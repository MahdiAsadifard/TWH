namespace Models.DTOs.User
{
    public record UserResponseDTO
    {
        public required string Uri { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public string? Phone { get; set; }

        public required string Email { get; set; }

        public bool Disabled { get; set; }
    }
}
