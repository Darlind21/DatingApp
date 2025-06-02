using API.Data_Layer.DTOs;
using API.Models;
using API.Repository_Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;



namespace API.Data_Layer.Repositories
{
    public class UserRepository(DataDbContext context, IMapper mapper) : IUserRepository
    {
        public async Task<MemberDTO?> GetMemberByUsernameAsync(string username)
        {
            return await context.Users
                .Where(u => u.UserName == username)
                .ProjectTo<MemberDTO>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<MemberDTO>> GetMembersAsync()
        {
            return await context.Users
                .ProjectTo<MemberDTO>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<AppUser?> GetUserByIdAsync(int id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<AppUser?> GetUserByUsernameAsync(string username)
        {
            return await context.Users
                .Include(u => u.Photos)
                .SingleOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await context.Users
                .Include(u => u.Photos)
                .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0; //returns the number of changes in the database
        }

        public void Update(AppUser user)
        {
            context.Entry(user).State = EntityState.Modified;
        }
    }
}
