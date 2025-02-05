namespace TeamCelebrations.Data.Responses
{
    public class LogInResponse
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime HireDate { get; set; }

        public string? Token { get; set; }
    }
}