namespace Models.DTOs.User
{
    public record UserResponseDTO
    {
        public string Uri { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? Phone { get; set; }

        public string Email { get; set; }

        public bool Disabled { get; set; }
    }
}
