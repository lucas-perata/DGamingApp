using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DGamingApp.Entities
{
    public class AppUserRole :  IdentityUserRole<int> 
    {
        public AppUser user {get; set;}
        public AppRole Role {get; set;}
    }
}