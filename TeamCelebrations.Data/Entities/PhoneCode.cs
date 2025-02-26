using System.ComponentModel.DataAnnotations;

namespace TeamCelebrations.Data.Entities
{
    public class PhoneCode : BaseEntity
    {
        [Required]
        public int Code { get; set; }
        
        [Required]
        public int Length { get; set; }

        [Required]
        public string? CountryName { get; set; }

        [Required]
        public string? CountryCode { get; set; }
    }
}