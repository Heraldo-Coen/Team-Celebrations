// Ignore Spelling: DNI

namespace TeamCelebrations.Data.Requests
{
    public class EmployeeSignUpRequest : SignUpRequest
    {
        public string DNI { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public Guid PhoneCodeId { get; set; }

        public DateTime BirthDate { get; set; }

        public Guid UnitId { get; set; }

        public ContractRequest? LastContract { get; set; }
    }
}