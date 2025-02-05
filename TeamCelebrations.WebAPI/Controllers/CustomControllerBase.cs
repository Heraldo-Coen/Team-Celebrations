using Microsoft.AspNetCore.Mvc;
using TeamCelebrations.Data.DataAccess;

namespace TeamCelebrations.WebAPI.Controllers
{
    public class CustomControllerBase(DataContext dataContext) : ControllerBase
    {
        protected readonly DataContext _dataContext = dataContext ?? throw new ArgumentNullException(nameof(_dataContext), "Error: Data Base connection.");

        /*
        protected Result<NullableAttribute> VerifyToken(string scopeClaim, string scopeClassName)
        {
            try
            {
                if (string.IsNullOrEmpty(scopeClaim))
                {
                    return new(false, "Scope token is null or empty");
                }
                else if (scopeClaim != scopeClassName)
                {
                    return new(false, "Invalid scope token");
                }

                return new(true, "Token verified");
            }
            catch (Exception ex)
            {
                return new(false, $"{ex.Message}");
            }
        }*/
    }
}
