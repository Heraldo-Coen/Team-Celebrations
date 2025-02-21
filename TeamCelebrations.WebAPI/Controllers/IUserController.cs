using Microsoft.AspNetCore.Mvc;
using TeamCelebrations.Data.Requests;

namespace TeamCelebrations.WebAPI.Controllers
{
    public interface IUserController
    {
        // Constants for locking user account
        const int LOG_IN_UNLOCK_MINUTES = 10;
        const int MAX_LOGIN_ATTEMPTS = 3;

        // Constants for verification code
        const int VERIFICATION_CODE_EXPIRED_MINUTES = 5;
        const int VERIFICATION_MIN_RANGE_VALUE = 50000000;
        const int VERIFICATION_MAX_RANGE_VALUE = 99999999;
        const int VERIFICATION_CODE_NULL_VALUE = 10000000;
        const int RESET_PASSWORD_UNLOCK_MINUTES = 60;
        const int MAX_RESET_PASSWORD_ATTEMPTS = 3;

        #region METHODS

        Task<ActionResult> SignUp(SignUpRequest signUpRequest);

        Task<ActionResult> SignUp(EmployeeSignUpRequest employeeSignUpRequest);

        Task<ActionResult> LogIn(LogInRequest logInRequest);

        Task<ActionResult> SendResetPasswordEmail(string email);

        Task<ActionResult> ResetPasswordVerification(string email, int code);

        Task<ActionResult> ResetPassword(Data.Requests.ResetPasswordRequest resetPasswordRequest);

        #endregion
    }
}