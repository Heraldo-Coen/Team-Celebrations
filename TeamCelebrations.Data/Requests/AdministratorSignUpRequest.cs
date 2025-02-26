using static TeamCelebrations.Data.Entities.Administrator;

namespace TeamCelebrations.Data.Requests
{
    public class AdministratorSignUpRequest : SignUpRequest
    {
        public AdministratorRole Role { get; set; }
    }
}