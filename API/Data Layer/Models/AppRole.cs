using Microsoft.AspNetCore.Identity;

namespace API.Data_Layer.Models
{
    public class AppRole : IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; } = [];

    }
}
