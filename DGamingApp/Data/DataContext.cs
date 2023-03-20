using DGamingApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace DGamingApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<AppUser> Users { get; set; }
    }
}
