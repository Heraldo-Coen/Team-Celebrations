using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using TeamCelebrations.Data.DataAccess;
using TeamCelebrations.Data.Entities;
using TeamCelebrations.Data.Requests;
using TeamCelebrations.Data.Responses;

namespace TeamCelebrations.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(DataContext dataContext) : CustomControllerBase(dataContext), IUserController
    {

        #region IUSERCONTROLLER METHODS

        public Task<ActionResult> SignUp(SignUpRequest signUpRequest)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("SignUp")]
        public async Task<ActionResult> SignUp(EmployeeSignUpRequest employeeSignUpRequest)
        {
            try
            {
                await _dataContext!.Employees!.AddAsync(new Employee()
                {
                    FirstName = employeeSignUpRequest!.FirstName,
                    LastName = employeeSignUpRequest.LastName,
                    Email = employeeSignUpRequest.Email,
                    PasswordHash = employeeSignUpRequest.PasswordHash,
                    PhoneNumber = employeeSignUpRequest.PhoneNumber,
                    BirthDate = employeeSignUpRequest.BirthDate,
                    HireDate = employeeSignUpRequest.HireDate,
                });

                await _dataContext.SaveChangesAsync();
                /*
                BackgroundJob.Schedule(() =>
                    Services.EmailServices.EmailAuthenticator.WelcomeStudentEmail(signUpRequest.Email!, $"{signUpRequest.FirstName} {signUpRequest.LastName}"),
                    new DateTimeOffset(DateTime.UtcNow)
                );
                */
                return Ok();
            }
            catch (Exception ex)
            {
                //ex.InnerException = {"23505: duplicate key value violates unique constraint \"IX_Employees_Email\"\r\n\r\nDETAIL: Detail redacted as it may contain sensitive data. Specify 'Include Error Detail' in the connection string to include this information."}
                if (
                    ex.InnerException != null
                    //&& ex.InnerException.Message.Contains(IUserController.EMAIL_ALREADY_USED_EXCEPTION_MESSAGE_1)
                    //&& ex.InnerException.Message.Contains(IUserController.EMAIL_ALREADY_USED_EXCEPTION_MESSAGE_2)
                )
                {
                    return Conflict(new { message = "Email already used." });
                }

                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("LogIn")]
        public async Task<ActionResult> LogIn(LogInRequest logInRequest)
        {
            try
            {
                var student = await _dataContext!.Employees!.FirstOrDefaultAsync(e => e.Email == logInRequest.Email);

                if (student == null)
                {
                    return NotFound(new { message = "Account not found." });
                }
                else
                {
                    if (logInRequest.PasswordHash == null || logInRequest.PasswordHash.Length == 0)
                    {
                        return BadRequest(new { message = "Password is required." });
                    }

                    if (student.IsLocked && student.UnlockDate.ToUniversalTime() > DateTime.UtcNow)
                    {
                        return Unauthorized(new
                        {
                            message = "Account is locked.",
                            unlockDate = student.UnlockDate.ToUniversalTime()
                        });
                    }
                    else if (student.IsLocked && student.UnlockDate.ToUniversalTime() <= DateTime.UtcNow)
                    {
                        // Unlocking account
                        student.IsLocked = false;
                        student.UnlockDate = DateTime.MinValue;
                        student.LogInAttempts = 0;
                    }

                    // Comparing bytes of the password
                    using (SHA256 sha256Hash = SHA256.Create())
                    {
                        byte[] bytes = logInRequest.PasswordHash!;
                        for (int i = 0; i < bytes.Length; i++)
                        {
                            if (bytes[i] != student.PasswordHash![i])
                            {
                                student.LogInAttempts++;

                                // Locking account then N failed attempts
                                if (student.LogInAttempts == IUserController.MAX_LOGIN_ATTEMPTS)
                                {
                                    student.IsLocked = true;
                                    student.UnlockDate = DateTime.UtcNow.AddMinutes(IUserController.LOG_IN_UNLOCK_MINUTES);

                                    await _dataContext.SaveChangesAsync();

                                    return Unauthorized(new
                                    {
                                        message = "Account is locked.",
                                        unlockDate = student.UnlockDate.ToUniversalTime()
                                    });
                                }

                                await _dataContext.SaveChangesAsync();

                                return Unauthorized(new
                                {
                                    message = "Invalid password.",
                                    remainingLogInAttempts = IUserController.MAX_LOGIN_ATTEMPTS - student.LogInAttempts
                                });
                            }
                        }
                    }

                    await _dataContext.SaveChangesAsync();
                    string token = ""; // ((IUserController)this).GenerateJwtToken(_configuration, student.Id!, student.GetType());

                    if (string.IsNullOrEmpty(token))
                    {
                        return BadRequest(new { message = "Error generating token." });
                    }

                    return Ok(new LogInResponse
                    {
                        Id = student.Id,
                        FirstName = student.FirstName,
                        LastName = student.LastName,
                        Email = student.Email,
                        Token = token,
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("SendResetPasswordEmail")]
        public async Task<ActionResult> SendResetPasswordEmail(string email)
        {
            try
            {
                var student = await _dataContext!.Employees!.FirstOrDefaultAsync(e => e.Email == email);

                if (student == null)
                {
                    return NotFound(new { message = "Account not found." });
                }

                student.VerificationCode = new Random().Next(IUserController.VERIFICATION_MIN_RANGE_VALUE, IUserController.VERIFICATION_MAX_RANGE_VALUE);
                student.VerificationCodeExpiration = DateTime.UtcNow.AddMinutes(IUserController.VERIFICATION_CODE_EXPIRED_MINUTES);
                student.IsVerified = false;

                await _dataContext.SaveChangesAsync();
                /*
                BackgroundJob.Schedule(() =>
                    Services.EmailServices.EmailAuthenticator.ResetPasswordEmail(student.Email!, $"{student.FirstName} {student.LastName}", student.VerificationCode),
                    new DateTimeOffset(DateTime.UtcNow)
                );
                */
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("ResetPasswordVerification")]
        public async Task<ActionResult> ResetPasswordVerification(string email, int code)
        {
            try
            {
                if (code < IUserController.VERIFICATION_MIN_RANGE_VALUE || code > IUserController.VERIFICATION_MAX_RANGE_VALUE)
                {
                    return BadRequest(new { message = "Invalid code." });
                }

                var student = await _dataContext!.Employees!.FirstOrDefaultAsync(e => e.Email == email);

                if (student == null)
                {
                    return NotFound(new { message = "Account not found." });
                }
                else if (student.VerificationCode != code)
                {
                    return BadRequest(new { message = "Invalid code." });
                }
                else if (student.VerificationCodeExpiration.ToUniversalTime() < DateTime.UtcNow)
                {
                    return BadRequest(new { message = "Code expired." });
                }

                student.IsVerified = true;

                await _dataContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            try
            {
                if (resetPasswordRequest.NewPasswordHash == null || resetPasswordRequest.NewPasswordHash.Length == 0)
                {
                    return BadRequest(new { message = "New password is required." });
                }
                else if (resetPasswordRequest.VerificationCode < IUserController.VERIFICATION_MIN_RANGE_VALUE
                    || resetPasswordRequest.VerificationCode > IUserController.VERIFICATION_MAX_RANGE_VALUE)
                {
                    return BadRequest(new { message = "Invalid code." });
                }

                var student = await _dataContext!.Employees!.FirstOrDefaultAsync(student => student.Email == resetPasswordRequest.Email);

                if (student == null)
                {
                    return NotFound(new { message = "Account not found." });
                }
                else if (student.VerificationCode != resetPasswordRequest.VerificationCode)
                {
                    return BadRequest(new { message = "Invalid code." });
                }
                else if (student.VerificationCodeExpiration.ToUniversalTime() < DateTime.UtcNow)
                {
                    return BadRequest(new { message = "Code expired." });
                }
                else if (!student.IsVerified)
                {
                    return BadRequest(new { message = "Code not verified." });
                }

                bool isDiferent = false;

                //Comparando bytes de la contraseña actual y la nueva
                for (int i = 0; i < resetPasswordRequest.NewPasswordHash.Length; i++)
                {
                    if (student.PasswordHash![i] != resetPasswordRequest.NewPasswordHash[i])
                    {
                        isDiferent = true;
                        break;
                    }
                }

                if (!isDiferent)
                {
                    return BadRequest(new { message = "New password is the same as the current password." });
                }

                student.VerificationCode = IUserController.VERIFICATION_CODE_NULL_VALUE;
                student.VerificationCodeExpiration = DateTime.MinValue;
                student.IsVerified = false;
                student.PasswordHash = resetPasswordRequest.NewPasswordHash;

                await _dataContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        
        #endregion
    }
}