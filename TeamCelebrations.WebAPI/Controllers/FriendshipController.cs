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
    public class FriendshipController(DataContext dataContext) : CustomControllerBase(dataContext)
    {
        [HttpPost]
        [Route("SendFriendshipRequest")]
        public async Task<ActionResult> SendFriendshipRequest(FriendshipRequest friendshipRequest)
        {
            try
            {
                // Verify if the EmployeeId1 and EmployeeId2 have values
                if (friendshipRequest.EmployeeId1 == Guid.Empty || friendshipRequest.EmployeeId2 == Guid.Empty)
                {
                    return BadRequest(new { message = "Invalid EmployeeId." });
                }

                // Verify if the EmployeeId1 exits
                var employee1 = await _dataContext.Employees!.FirstOrDefaultAsync(e => e.Id == friendshipRequest.EmployeeId1);

                if (employee1 == null)
                {
                    return BadRequest(new { message = "EmployeeId1 does not exist." });
                }

                // Verify if the EmployeeId2 exits
                var employee2 = await _dataContext.Employees!.FirstOrDefaultAsync(e => e.Id == friendshipRequest.EmployeeId2);

                if (employee2 == null)
                {
                    return BadRequest(new { message = "EmployeeId2 does not exist." });
                }

                await _dataContext.Friendships!.AddAsync(new Friendship()
                {
                    EmployeeId1 = friendshipRequest.EmployeeId1!.Value,
                    EmployeeId2 = friendshipRequest.EmployeeId2!.Value
                });

                await _dataContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("AcceptFriendshipRequest")]
        public async Task<ActionResult> AcceptFriendshipRequest(FriendshipRequest friendshipRequest)
        {
            try
            {
                // Verify if the EmployeeId1 and EmployeeId2 have values
                if (friendshipRequest.EmployeeId1 == Guid.Empty || friendshipRequest.EmployeeId2 == Guid.Empty)
                {
                    return BadRequest(new { message = "Invalid EmployeeId." });
                }

                // Verify if the EmployeeId1 exits
                var employee1 = await _dataContext.Employees!.FirstOrDefaultAsync(e => e.Id == friendshipRequest.EmployeeId1);

                if (employee1 == null)
                {
                    return BadRequest(new { message = "EmployeeId1 does not exist." });
                }

                // Verify if the EmployeeId2 exits
                var employee2 = await _dataContext.Employees!.FirstOrDefaultAsync(e => e.Id == friendshipRequest.EmployeeId2);

                if (employee2 == null)
                {
                    return BadRequest(new { message = "EmployeeId2 does not exist." });
                }

                // Verify if the friendship request exists
                var friendship = await _dataContext.Friendships!.FirstOrDefaultAsync(f => f.EmployeeId1 == friendshipRequest.EmployeeId1 && f.EmployeeId2 == friendshipRequest.EmployeeId2);

                if (friendship == null)
                {
                    return BadRequest(new { message = "Friendship request does not exist." });
                }

                friendship.IsAccepted = true;
                friendship.AcceptanceDate = DateTime.UtcNow;

                await _dataContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetFriendshipRequests")]
        public async Task<ActionResult> GetFriendshipRequests(Guid employeeId)
        {
            try
            {
                // Verify if the EmployeeId exists
                var employee = await _dataContext.Employees!.FirstOrDefaultAsync(e => e.Id == employeeId);

                if (employee == null)
                {
                    return BadRequest(new { message = "EmployeeId does not exist." });
                }
                
                var friendshipRequests = await _dataContext.Friendships!.Where(f => f.EmployeeId2 == employeeId && f.IsAccepted == false).ToListAsync();

                return Ok(friendshipRequests);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}