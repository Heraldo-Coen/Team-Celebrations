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
    public class UnitController(DataContext dataContext) : CustomControllerBase(dataContext)
    {
        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult> Create(UnitRequest unitRequest)
        {
            try
            {
                if (unitRequest.HigherUnitId != null && unitRequest.HigherUnitId != Guid.Empty)
                {
                    // Verify if HigherUnitId exists
                    var higherUnitExists = await _dataContext.Units!.AnyAsync(u => u.Id == unitRequest.HigherUnitId);
                    if (!higherUnitExists)
                    {
                        return BadRequest(new { message = "Invalid HigherUnitId." });
                    }
                }

                await _dataContext.Units!.AddAsync(new Unit()
                {
                    Name = unitRequest.Name!,
                    Acronym = unitRequest.Acronym!,
                    HigherUnitId = unitRequest.HigherUnitId
                });

                await _dataContext.SaveChangesAsync();

                return Ok();
            }
            catch (DbUpdateException ex) when (ex.InnerException != null)
            {
                return Conflict(new { message = "Foreign key constraint violated." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var units = await _dataContext.Units!.ToListAsync();

                if (units.Count == 0)
                {
                    return NoContent();
                }

                return Ok(units);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}