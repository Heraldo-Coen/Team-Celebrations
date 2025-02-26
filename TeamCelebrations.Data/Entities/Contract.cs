using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamCelebrations.Data.Entities
{
    public class Contract : BaseEntity
    {
        [Required]
        public Guid EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee? Employee { get; set; }

        public DateTime? ContractStartDate { get; set; }

        public DateTime? ContractEndDate { get; set; }

        public DateTime? RenewalDate { get; set; }

        public ContractStatus? Status { get; set; }

        public ContractType? Type { get; set; }

        public enum ContractStatus
        {
            Active,    
            Expired,   
            Renewed,  
            Terminated
        }

        public enum ContractType
        {
            /// <summary>
            /// Undefined
            /// </summary>
            Permanent, 

            Temporary,

            /// <summary>
            /// Service provider
            /// </summary>
            Freelancer,

            /// <summary>
            /// Practice and volunteer
            /// </summary>
            Internship
        }
    }
}