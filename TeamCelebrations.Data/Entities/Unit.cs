using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TeamCelebrations.Data.Entities
{
    // For Office, Managements and Sub-Managements - Departments
    public class Unit : BaseEntity
    {
        public string? Name { get; set; }

        public Guid HigherUnitId { get; set; }

        [ForeignKey(nameof(HigherUnitId))]
        public virtual Unit HigherUnit { get; set; }
    }
}