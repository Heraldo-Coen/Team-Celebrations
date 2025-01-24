using System.ComponentModel.DataAnnotations;

namespace TeamCelebrations.Data.Entities
{
    public class Employee : User
    {
        [Required]
        public DateTime BirthDate { get; set; } = DateTime.MinValue;

        public string? PhoneNumber { get; set; }

        [Required]
        public DateTime HireDate { get; set; } = DateTime.MinValue;
    }
}