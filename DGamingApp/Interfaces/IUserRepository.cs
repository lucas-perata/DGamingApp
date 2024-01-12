using DGamingApp.Dto;
using DGamingApp.Entities;
using DGamingApp.Helpers;

namespace DGamingApp.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetUsers();
        Task<AppUser> GetUserById(int id);
        Task<AppUser> GetUserByName(string name);
        void Update(AppUser user);
        Task<MemberDto> GetMember(string username);
        Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
    }
}
