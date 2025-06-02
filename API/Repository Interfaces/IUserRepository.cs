using API.Data_Layer.DTOs;
using API.Models;

namespace API.Repository_Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser?> GetUserByIdAsync(int id);
        Task<AppUser?> GetUserByUsernameAsync(string username);
        Task<IEnumerable<MemberDTO>> GetMembersAsync();
        Task<MemberDTO?> GetMemberByUsernameAsync(string username);
    }
}
