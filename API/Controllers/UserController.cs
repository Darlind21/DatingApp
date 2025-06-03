using API.Data_Layer;
using API.Data_Layer.DTOs;
using API.Models;
using API.Repository_Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize]
    public class UsersController(IUserRepository userRepository, IMapper mapper) : BaseApiController
    {
        [HttpGet("{username}")] //api/Users/6
        public async Task<ActionResult<MemberDTO>> GetUserByUsername(string username)
        {
            var user = await userRepository.GetMemberByUsernameAsync(username);

            if (user == null) return NotFound();
            
            return Ok(user);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetAll()
        {
            var users = await userRepository.GetMembersAsync();

            return Ok(users);
        }



        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDTO memberUpdateDTO)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (username == null) return BadRequest("No username found in token");

            var user = await userRepository.GetUserByUsernameAsync(username);

            if (user == null) return BadRequest("Could not find user");

            //since we got user obj by the db using ef, then it is tracking the user obj when we update the user via .Map() method
            mapper.Map(memberUpdateDTO, user);

            //user will not be able to send a request where no changes have been made in client-side so no point in trying to avoid a BadRequest
            if (await userRepository.SaveAllAsync()) return NoContent();
            else return BadRequest("User not updated");
        }
    }
}
