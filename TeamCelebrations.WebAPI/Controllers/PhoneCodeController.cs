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
        public async Task<ActionResult> Create(PhoneCodeRequest phoneCodeRequest)
        {
            try
            {
                if(phoneCodeRequest.Code == 0)
                {
                    return BadRequest(new { message = "Invalid Code." });
                }
                else if (phoneCodeRequest.Length == 0)
                {
                    return BadRequest(new { message = "Invalid Length." });
                }
                else if (string.IsNullOrEmpty(phoneCodeRequest.CountryName))
                {
                    return BadRequest(new { message = "Invalid CountryName." });
                }
                else if (string.IsNullOrEmpty(phoneCodeRequest.CountryCode))
                {
                    return BadRequest(new { message = "Invalid CountryCode." });
                }

                await _dataContext!.PhoneCodes!.AddAsync(new PhoneCode()
                {
                    Code = phoneCodeRequest.Code,
                    Length = phoneCodeRequest.Length,
                    CountryName = phoneCodeRequest.CountryName,
                    CountryCode = phoneCodeRequest.CountryCode
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

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var phoneCodes = await _dataContext!.PhoneCodes!.Select( p => new PhoneCodeResponse
                {
                    Id = p.Id,
                    Code = p.Code,
                    Length = p.Length,
                    CountryName = p.CountryName,
                    CountryCode = p.CountryCode
                }).ToListAsync();

                if (phoneCodes.Count == 0)
                {
                    return NoContent();
                }

                return Ok(phoneCodes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}