using API.Models;

namespace API.Business_Layer.Infrastructure
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
