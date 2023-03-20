using DGamingApp.Data;
using DGamingApp.Entities;
using DGamingApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DGamingApp.Repository
{
    public class UserRepository : IUserRepository
    {
        public readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<AppUser> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<AppUser>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
