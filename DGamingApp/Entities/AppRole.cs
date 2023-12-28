using Microsoft.AspNetCore.Identity;

namespace DGamingApp.Entities
{
    public class AppRole : IdentityRole<int> 
    {
        public ICollection<AppUserRole> UserRoles {get; set;}
    }
}