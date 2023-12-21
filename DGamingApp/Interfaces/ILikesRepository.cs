using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DGamingApp.Dto;
using DGamingApp.Entities;
using DGamingApp.Helpers;

namespace DGamingApp.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int targetUserId); 
        Task<AppUser> GetUserWithLikes(int userId); 
        Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);
    }
}