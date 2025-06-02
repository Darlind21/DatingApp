using API.Business_Layer.Infrastructure;
using API.Data_Layer;
using API.Data_Layer.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    public class AccountController(DataDbContext context, ITokenService tokenService) : BaseApiController
    {

        [HttpPost("register")] //account/register
        public async Task<ActionResult<UserDTO>> Register (RegisterDTO registerDTO)
        {
            if(await UserExists(registerDTO.UserName))
            {
                return BadRequest("Username is taken");
            }
            return Ok();
            //using var hmac = new HMACSHA512();

            //var user = new AppUser
            //{
            //    UserName = registerDTO.UserName.ToLower(),
            //    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
            //    PasswordSalt = hmac.Key
            //};

            //context.Users.Add(user);
            //await context.SaveChangesAsync();

            //return Ok(new UserDTO
            //{
            //    Username = user.UserName,
            //    Token = tokenService.CreateToken(user)
            //});
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> LogIn (LoginDTO loginDTO)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(x => x.UserName == loginDTO.UserName.ToLower());

            if (user == null) return Unauthorized("Invalid Username");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

            for (int i = 0; i <computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Invalid Password");
                }
            }

            return new UserDTO
            {
                Username = user.UserName,
                Token = tokenService.CreateToken(user)
            };
        }



        private async Task<bool> UserExists(string username)
        {
            return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());  
        }
    }
}
