using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TeamCelebrations.Data.Entities
{
    public class Notification : BaseEntity
    {
        public string? Title { get; set; }

        public string? Content { get; set; }

        public bool IsRead { get; set; } = false;

        [Required]
        public Guid EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee? Employee { get; set; }

        /// <summary>
        /// If the notification is related to an event.
        /// </summary>
        public Guid? EventId { get; set; }

        [ForeignKey(nameof(EventId))]
        public virtual Event? Event { get; set; }
    }
}