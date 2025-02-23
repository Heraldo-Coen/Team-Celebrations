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
    public class PhoneCodeController(DataContext dataContext) : CustomControllerBase(dataContext)
    {
        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult> Create(PhoneCode phoneCode)
        {
            try
            {
                await _dataContext!.PhoneCodes!.AddAsync(new PhoneCode()
                {
                    Code = phoneCode.Code,
                    Length = phoneCode.Length,
                    CountryName = phoneCode.CountryName,
                    CountryCode = phoneCode.CountryCode
                });

                await _dataContext.SaveChangesAsync();

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
    }
}