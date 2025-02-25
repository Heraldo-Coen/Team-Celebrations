namespace TeamCelebrations.Data.Requests
{
    public class PhoneCodeRequest
    {
        public int Code { get; set; }

        public int Length { get; set; }

        public string? CountryName { get; set; }

        public string? CountryCode { get; set; }
    }
}