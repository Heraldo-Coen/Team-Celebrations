using System.ComponentModel.DataAnnotations;

namespace TeamCelebrations.Data.Entities
{
    public class Employee : User
    {
        public string? PhoneNumber { get; set; }

        public bool? IsPhoneVerified { get; set; } = false;

        [Required]
        public DateTime BirthDate { get; set; } = DateTime.MinValue;

        [Required]
        public DateTime HireDate { get; set; } = DateTime.MinValue;
    }
}