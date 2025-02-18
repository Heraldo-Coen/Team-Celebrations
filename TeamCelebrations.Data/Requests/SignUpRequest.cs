namespace TeamCelebrations.Data.Requests
{
    public class SignUpRequest
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public byte[]? PasswordHash { get; set; }

        public string? PhoneNumber { get; set; }

        public Guid PhoneCodeId { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime HireDate { get; set; }
    }
}