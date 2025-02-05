using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using TeamCelebrations.Data.Requests;

namespace TeamCelebrations.WebAPI.Controllers
{
    public interface IUserController
    {
        Task<ActionResult> SignUp(SignUpRequest signUpRequest);

        //Task<ActionResult> LogIn(LogInRequest logInRequest);

        Task<ActionResult> SendResetPasswordEmail(string email);

        Task<ActionResult> ResetPasswordVerification(string email, int code);

        Task<ActionResult> ResetPassword(ResetPasswordRequest resetPasswordRequest);
    }
}