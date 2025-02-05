namespace TeamCelebrations.Data.Requests
{
    public class LogInRequest
    {
        public string? Email { get; set; }

        public byte[]? PasswordHash { get; set; }

        public string? PhoneNumber { get; set; }
    }
}