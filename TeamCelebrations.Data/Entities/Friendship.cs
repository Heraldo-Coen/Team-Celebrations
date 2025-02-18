using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TeamCelebrations.Data.Entities
{
    public class Friendship : BaseEntity
    {
        [Required]
        public Guid EmployeeId1 { get; set; }

        [Required]
        public Guid EmployeeId2 { get; set; }

        [ForeignKey(nameof(EmployeeId1))]
        public virtual Employee Employee1 { get; set; }

        [ForeignKey(nameof(EmployeeId2))]
        public virtual Employee Employee2 { get; set; }
    }
}