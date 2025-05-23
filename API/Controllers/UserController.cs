using API.Data_Layer;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class UsersController(DataDbContext context) : BaseApiController
    {

        [Authorize]
        [HttpGet("{id}")] //api/Users/6
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            var user = await context.Users.FindAsync(id);
            if (user != null)
            {
                return Ok(user);
            }

            return NotFound();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetAll()
        {
            var users = await context.Users.ToListAsync();

            if (users != null)
            {
                return Ok(users);
            }

            return NotFound();
        }

        
    }
}
