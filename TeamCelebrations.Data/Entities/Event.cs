using System.ComponentModel.DataAnnotations.Schema;

namespace TeamCelebrations.Data.Entities
{
    public class Event : BaseEntity
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime Date { get; set; } = DateTime.MinValue;

        public Guid? EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee? Employee { get; set; }

        public Guid? AdministratorId { get; set; }

        [ForeignKey(nameof(AdministratorId))]
        public virtual Employee? Administrator { get; set; }
    }
}