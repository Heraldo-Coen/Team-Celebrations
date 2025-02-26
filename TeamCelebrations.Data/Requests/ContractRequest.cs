using static TeamCelebrations.Data.Entities.Contract;

namespace TeamCelebrations.Data.Requests
{
    public class ContractRequest
    {
        public Guid EmployeeId { get; set; }

        public DateTime? ContractStartDate { get; set; }

        public DateTime? ContractEndDate { get; set; }

        public DateTime? RenewalDate { get; set; }

        public ContractStatus? Status { get; set; }

        public ContractType? Type { get; set; }
    }
}