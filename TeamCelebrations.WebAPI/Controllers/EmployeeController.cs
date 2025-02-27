using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text.Json;
using TeamCelebrations.Data.DataAccess;
using TeamCelebrations.Data.Entities;
using TeamCelebrations.Data.Requests;
using TeamCelebrations.Data.Responses;
using TeamCelebrations.WebAPI.Constants;

namespace TeamCelebrations.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(DataContext dataContext) : CustomControllerBase(dataContext), IUserController<EmployeeSignUpRequest>
    {

        #region IUSERCONTROLLER METHODS

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
                    DNI = employeeSignUpRequest.DNI,
                    PhoneNumber = employeeSignUpRequest.PhoneNumber,
                    PhoneCodeId = employeeSignUpRequest.PhoneCodeId,
                    BirthDate = employeeSignUpRequest.BirthDate,
                    //HireDate = employeeSignUpRequest.HireDate,
                    UnitId = employeeSignUpRequest.UnitId
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
                if (
                    ex.InnerException != null
                    && ex.InnerException.Message.Contains(UserControllerConstants.EMAIL_ALREADY_USED_EXCEPTION_MESSAGE_1)
                    && ex.InnerException.Message.Contains(UserControllerConstants.EMAIL_ALREADY_USED_EXCEPTION_MESSAGE_2)
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
                var employee = await _dataContext!.Employees!.FirstOrDefaultAsync(e => e.Email == logInRequest.Email);

                if (employee == null)
                {
                    return NotFound(new { message = "Account not found." });
                }
                else
                {
                    if (logInRequest.PasswordHash == null || logInRequest.PasswordHash.Length == 0)
                    {
                        return BadRequest(new { message = "Password is required." });
                    }

                    if (employee.IsLocked && employee.UnlockDate.ToUniversalTime() > DateTime.UtcNow)
                    {
                        return Unauthorized(new
                        {
                            message = "Account is locked.",
                            unlockDate = employee.UnlockDate.ToUniversalTime()
                        });
                    }
                    else if (employee.IsLocked && employee.UnlockDate.ToUniversalTime() <= DateTime.UtcNow)
                    {
                        // Unlocking account
                        employee.IsLocked = false;
                        employee.UnlockDate = DateTime.MinValue;
                        employee.LogInAttempts = 0;
                    }

                    // Comparing bytes of the password
                    using (SHA256 sha256Hash = SHA256.Create())
                    {
                        byte[] bytes = logInRequest.PasswordHash!;
                        for (int i = 0; i < bytes.Length; i++)
                        {
                            if (bytes[i] != employee.PasswordHash![i])
                            {
                                employee.LogInAttempts++;

                                // Locking account then N failed attempts
                                if (employee.LogInAttempts == UserControllerConstants.MAX_LOGIN_ATTEMPTS)
                                {
                                    employee.IsLocked = true;
                                    employee.UnlockDate = DateTime.UtcNow.AddMinutes(UserControllerConstants.LOG_IN_UNLOCK_MINUTES);

                                    await _dataContext.SaveChangesAsync();

                                    return Unauthorized(new
                                    {
                                        message = "Account is locked.",
                                        unlockDate = employee.UnlockDate.ToUniversalTime()
                                    });
                                }

                                await _dataContext.SaveChangesAsync();

                                return Unauthorized(new
                                {
                                    message = "Invalid password.",
                                    remainingLogInAttempts = UserControllerConstants.MAX_LOGIN_ATTEMPTS - employee.LogInAttempts
                                });
                            }
                        }
                    }

                    await _dataContext.SaveChangesAsync();
                    string token = "Token"; // ((IUserController)this).GenerateJwtToken(_configuration, student.Id!, student.GetType());

                    if (string.IsNullOrEmpty(token))
                    {
                        return BadRequest(new { message = "Error generating token." });
                    }

                    return Ok(new LogInResponse
                    {
                        Id = employee.Id,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        Email = employee.Email,
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

                student.VerificationCode = new Random().Next(UserControllerConstants.VERIFICATION_MIN_RANGE_VALUE, UserControllerConstants.VERIFICATION_MAX_RANGE_VALUE);
                student.VerificationCodeExpiration = DateTime.UtcNow.AddMinutes(UserControllerConstants.VERIFICATION_CODE_EXPIRED_MINUTES);
                student.IsEmailVerified = false;

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
                if (code < UserControllerConstants.VERIFICATION_MIN_RANGE_VALUE || code > UserControllerConstants.VERIFICATION_MAX_RANGE_VALUE)
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

                student.IsEmailVerified = true;

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
                else if (resetPasswordRequest.VerificationCode < UserControllerConstants.VERIFICATION_MIN_RANGE_VALUE
                    || resetPasswordRequest.VerificationCode > UserControllerConstants.VERIFICATION_MAX_RANGE_VALUE)
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
                else if (!student.IsEmailVerified)
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

                student.VerificationCode = UserControllerConstants.VERIFICATION_CODE_NULL_VALUE;
                student.VerificationCodeExpiration = DateTime.MinValue;
                student.IsEmailVerified = false;
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