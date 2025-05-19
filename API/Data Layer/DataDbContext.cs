using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data_Layer
{
    public class DataDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<AppUser> Users { get; set; }

    }
}
