namespace TeamCelebrations.Data.Requests
{
    public class AdministratorUpdateRequest
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime HireDate { get; set; }
    }
}