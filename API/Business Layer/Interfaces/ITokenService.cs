using API.Models;

namespace API.Business_Layer.Infrastructure
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
