using System.ComponentModel.DataAnnotations;

namespace API.Data_Layer.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string? KnownAs { get; set; }

        [Required]
        public string?  Gender { get; set; }

        [Required]
        public string? DateOfBirth { get; set; }

        [Required]
        public string? City { get; set; }

        [Required]
        public string? Country { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(30)]
        public string Password { get; set; } = string.Empty;
    }
}
