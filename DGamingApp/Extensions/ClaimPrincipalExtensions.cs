using System.Security.Claims;

namespace DGamingApp.Extensions
{
    public static class ClaimPrincipalExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }
    }
}