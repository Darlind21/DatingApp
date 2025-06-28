using API.Models;
using Microsoft.AspNetCore.Identity;

namespace API.Data_Layer.Models
{
    public class AppUserRole : IdentityUserRole<int>
    {
        public AppUser User { get; set; } = null!;
        public AppRole Role { get; set; } = null!;
    }
}
