using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TeamCelebrations.Data.Entities
{
    /// <summary>
    /// For Office, Managements and Sub-Managements - Departments
    /// </summary>
    public class Unit : BaseEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Acronym { get; set; } = string.Empty;

        public Guid? HigherUnitId { get; set; }

        [ForeignKey(nameof(HigherUnitId))]
        public virtual Unit? HigherUnit { get; set; }
    }
}