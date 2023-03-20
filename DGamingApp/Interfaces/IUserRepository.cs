using DGamingApp.Entities;

namespace DGamingApp.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetUsers();
        Task<AppUser> GetUserById(int id);
    }
}
