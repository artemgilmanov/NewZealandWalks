using Microsoft.AspNetCore.Identity;

namespace NewZealandWalksAPI.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
