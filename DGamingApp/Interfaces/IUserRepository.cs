using DGamingApp.Dto;
using DGamingApp.Entities;

namespace DGamingApp.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetUsers();
        Task<AppUser> GetUserById(int id);
        Task<AppUser> GetUserByName(string name);
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<MemberDto> GetMember(string username);
        Task<IEnumerable<MemberDto>> GetMembersAsync();
    }
}
