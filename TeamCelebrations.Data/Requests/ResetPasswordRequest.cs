namespace TeamCelebrations.Data.Requests
{
    public class ResetPasswordRequest
    {
        public string? Email { get; set; }

        public byte[]? NewPasswordHash { get; set; }

        public int VerificationCode { get; set; }
    }
}