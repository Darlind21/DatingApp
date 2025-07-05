using API.Business_Layer.Infrastructure;
using API.Data_Layer;
using API.Data_Layer.DTOs;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    public class AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper ) : BaseApiController
    {

        [HttpPost("register")] //account/register
        public async Task<ActionResult<UserDTO>> Register (RegisterDTO registerDTO)
        {
            if(await UserExists(registerDTO.UserName)) return BadRequest("Username is taken");

            var user = mapper.Map<AppUser>(registerDTO);

            user.UserName = registerDTO.UserName.ToLower();

            var result = await userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await userManager.AddToRoleAsync(user, "Member");

            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            return new UserDTO
            {
                Username = user.UserName,
                Token = await tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> LogIn (LoginDTO loginDTO)
        {
            var user = await userManager.Users
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(x => x.NormalizedUserName == loginDTO.UserName.ToUpper());

            if (user == null || user.UserName == null) return Unauthorized("Invalid Username");

            var result = await userManager.CheckPasswordAsync(user, loginDTO.Password);

            if (!result) return Unauthorized();

            return new UserDTO
            {
                Username = user.UserName,
                KnownAs = user.KnownAs,
                Token = await tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMainPhoto)?.Url,
                Gender = user.Gender
            };
        }



        private async Task<bool> UserExists(string username)
        {
            return await userManager.Users.AnyAsync(x => x.NormalizedUserName == username.ToUpper());  
        }
    }
}
