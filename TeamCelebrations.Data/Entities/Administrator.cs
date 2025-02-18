using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TeamCelebrations.Data.Entities
{
    public class Administrator : User
    {
        [Required]
        public AdministratorRole Role { get; set; }
        
        public enum AdministratorRole {
            /// <summary>
            /// Full access to all features and settings.
            /// Can manage administrator and roles.
            /// </summary>
            Administrator,

            /// <summary>
            /// Limited access to manage content and users.
            /// Can review and delete inappropriate content.
            /// </summary>
            Moderator, 

            /// <summary>
            /// Can create, edit, and publish content.
            /// Cannot manage users.
            /// </summary>
            Editor,
        }
    }
}