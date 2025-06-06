using API.Business_Layer.Interfaces;
using API.Data_Layer;
using API.Data_Layer.DTOs;
using API.Extensions;
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
    public class UsersController(IUserRepository userRepository, IMapper mapper,
        IPhotoService photoService) : BaseApiController
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
            var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());

            if (user == null) return BadRequest("Could not find user");

            //since we got user obj by the db using ef, then it is tracking the user obj when we update the user via .Map() method
            mapper.Map(memberUpdateDTO, user);

            //user will not be able to send a request where no changes have been made in client-side so no point in trying to avoid a BadRequest
            if (await userRepository.SaveAllAsync()) return NoContent();
            else return BadRequest("User not updated");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDTO>> AddPhoto (IFormFile file)// when sending a request(e.g. postman) the key name has match the parameter name
                                                                           // ("file" in this case)(case insensitive)
        {
            var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());

            if (user == null) return BadRequest("Cannot update user!");

            var result = await photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            user.Photos.Add(photo);

            if (await userRepository.SaveAllAsync())
                return CreatedAtAction(nameof(GetUserByUsername),
                    new { username = user.UserName }, mapper.Map<PhotoDTO>(photo));

            return BadRequest("Problem adding photo!");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto([FromRoute(Name = "photoId")]int photoId)
        {
            var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());

            if (user == null) return BadRequest("Could not find user");

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null || photo.IsMainPhoto) return BadRequest("Cannot use this main photo");

            var currentMainPhoto = user.Photos
                .FirstOrDefault(x => x.IsMainPhoto);

            if (currentMainPhoto != null) currentMainPhoto.IsMainPhoto = false;
            photo.IsMainPhoto = true;

            if (await userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Problem setting main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto (int photoId)
        {
            var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());

            //The below is to stop compiler from shouting that user may be null
            if (user == null) return BadRequest("User not found!");

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null || photo.IsMainPhoto) return BadRequest("This photo cannot be deleted!");

            if(photo.PublicId != null)
            {
                var result = await photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if (await userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Problem deleting photo");
        }
    }
}
