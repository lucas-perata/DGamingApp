using DGamingApp.Extensions;
using DGamingApp.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DGamingApp.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return; 

            var id = resultContext.HttpContext.User.GetUserId();

            var repo = resultContext.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
            var user = await repo.GetUserById(int.Parse(id));
            user.LastActive = DateTime.UtcNow;
            await repo.SaveAllAsync();
        }
    }
}
