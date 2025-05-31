using API.Data_Layer;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //Controller that is only responsible for returning errors to the client
    public class ErrorController(DataDbContext context) : BaseApiController
    {
        [Authorize]
        [HttpGet("auth")]
        public IActionResult GetAuth()
        {
            return Ok("secret text");
        }

        [HttpGet("not-found")]
        public IActionResult GetNotFound()
        {
            var result = context.Users.Find(-1);

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("server-error")]
        public ActionResult<AppUser> GetServerError()
        {
            var result = context.Users.Find(-1) ?? throw new Exception("A bad thing has happened");

            return result;
        }

        [HttpGet("bad-request")]
        public IActionResult GetBadRequest()
        {
            return BadRequest("Not a good request");
        }
    }
}
