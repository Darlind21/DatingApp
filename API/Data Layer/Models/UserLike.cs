using API.Models;

namespace API.Data_Layer.Models
{
    public class UserLike
    {
        public AppUser SourceUser { get; set; } = null!;
        public int SourceUserId { get; set; }
        public AppUser TargetUser { get; set; }
        public int TargetUserId { get; set; }
    }
}
