using API.Data_Layer;
using API.Data_Layer.DTOs;
using API.Models;
using API.Repository_Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController(IUserRepository userRepository) : BaseApiController
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

        
    }
}
