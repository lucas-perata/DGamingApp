using DGamingApp.Entities;

namespace DGamingApp.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user); 
    }
}
