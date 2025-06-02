using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{

    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        public required string Url { get; set; }
        public bool IsMainPhoto { get; set; }
        public string? PublicId { get; set; }

        //Nav props
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; } = null!;
    }
}