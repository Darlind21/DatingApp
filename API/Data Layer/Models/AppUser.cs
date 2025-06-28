using API.Data_Layer.Models;
using API.Extensions;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class AppUser : IdentityUser<int> //<int> tells the framework we want the id as int since the default is string 
        //AppUser will inherit built in props like Id,Username, password hashing etc
    {
        public DateOnly DateOfBirth { get; set; }
        public required string KnownAs { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public required string Gender { get; set; }
        public string? AboutSection { get; set; }
        public string? Interests { get; set; }
        public string? LookingFor { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public List<Photo> Photos { get; set; } = [];
        public List<UserLike> LikedByUsers { get; set; } = [];
        public List<UserLike> LikedUsers { get; set; } = [];
        public List<Message> MessagesSent { get; set; } = [];
        public List<Message> MessagesReceived { get; set; } = [];
        public ICollection<AppUserRole> UserRoles { get; set; } = [];


        //It has to have the word "Get" for automapper to work on MemberDTO class to use this method to calculate and then set the age property inside the DTO
        //Commented method for automapper to avoid getting complete user obj to make the method work 
        //public int GetAge()
        //{
        //    return this.DateOfBirth.CalculateAge();
        //}
    }
}
