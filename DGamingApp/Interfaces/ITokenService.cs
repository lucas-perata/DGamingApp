using DGamingApp.Entities;

namespace DGamingApp.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user); 
    }
}
