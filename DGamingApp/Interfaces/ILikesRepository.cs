using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DGamingApp.Dto;
using DGamingApp.Entities;

namespace DGamingApp.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int targetUserId); 
        Task<AppUser> GetUserWithLikes(int userId); 
        Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId);
    }
}