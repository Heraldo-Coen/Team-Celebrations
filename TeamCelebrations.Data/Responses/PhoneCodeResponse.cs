namespace TeamCelebrations.Data.Responses
{
    /// <summary>
    /// For Employee user
    /// </summary>
    public class PhoneCodeResponse
    {
        public Guid Id { get; set; }

        public int Code { get; set; }

        public int Length { get; set; }

        public string? CountryName { get; set; }

        public string? CountryCode { get; set; }
    }
}