// Ignore Spelling: DNI

using System.Diagnostics.Contracts;

namespace TeamCelebrations.Data.Responses
{
    public class LogInResponse
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? DNI { get; set; }

        public string? PhoneNumber { get; set; }

        public bool IsPhoneVerified { get; set; }

        public int PhoneCode { get; set; }

        public DateTime BirthDate { get; set; }

        public Guid UnitId { get; set; }

        public ContractResponse? LastContract { get; set; }

        public string? Token { get; set; }
    }
}