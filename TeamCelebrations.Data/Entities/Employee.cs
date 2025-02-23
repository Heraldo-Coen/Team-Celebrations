// Ignore Spelling: DNI

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamCelebrations.Data.Entities
{
    public class Employee : User
    {
        [StringLength(8)]
        public string? DNI { get; set; }

        public string? PhoneNumber { get; set; }

        public Guid PhoneCodeId { get; set; }

        public bool IsPhoneVerified { get; set; } = false;

        /// <summary>
        /// Employee must accept by administrators to be able to login.
        /// </summary>
        public bool IsAccepted { get; set; } = false; 

        [Required]
        public DateTime BirthDate { get; set; } = DateTime.MinValue;

        [Required]
        public DateTime HireDate { get; set; } = DateTime.MinValue;

        [Required]
        public Guid UnitId { get; set; }

        [ForeignKey(nameof(PhoneCodeId))]
        public virtual PhoneCode? PhoneCode { get; set; }

        [ForeignKey(nameof(UnitId))]
        public virtual Unit? Unit { get; set; }
    }
}