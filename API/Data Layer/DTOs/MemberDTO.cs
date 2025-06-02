using API.Models;

namespace API.Data_Layer.DTOs
{
    public class MemberDTO
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public int Age { get; set; }
        public string? PhotoUrl { get; set; } //returns main photo
        public DateOnly DateOfBirth { get; set; }
        public string? KnownAs { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastActive { get; set; }
        public string? Gender { get; set; }
        public string? AboutSection { get; set; }
        public string? Interests { get; set; }
        public string? LookingFor { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public List<PhotoDTO>? Photos { get; set; }
    }
}
