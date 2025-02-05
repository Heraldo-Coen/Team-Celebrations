using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamCelebrations.Data.Entities
{
    public abstract class User : BaseEntity
    {
        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public string? LastName { get; set; }

        private string? _email = string.Empty;

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string? Email
        {
            get
            {
                return _email;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _email = value.ToLower();
                }
            }
        }

        [Required]
        public byte[]? PasswordHash { get; set; }

        [Required]
        public bool IsConnected { get; set; } = false;

        [Required]
        public DateTime LastConnectionDate { get; set; } = DateTime.MinValue;

        [Required]
        public int LogInAttempts { get; set; } = 0;

        /// <value>value="True": User Blocked</value>
        [Required]
        public bool IsLocked { get; set; } = false;

        [Required]
        public DateTime UnlockDate { get; set; } = DateTime.MinValue;

        /// <remarks>Default Value = 10000000</remarks>
        [Range(10000000, 99999999)]
        public int VerificationCode { get; set; } = 10000000;

        /// <summary>
        /// For email verification
        /// </summary>
        public bool IsVerified { get; set; } = false;

        public DateTime VerificationCodeExpiration { get; set; } = DateTime.MinValue;

        [Required]
        public int ResetPasswordAttempts { get; set; } = 0;
    }
}
