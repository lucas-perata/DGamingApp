using DGamingApp.Data;
using DGamingApp.Helpers;
using DGamingApp.Interfaces;
using DGamingApp.Repository;
using DGamingApp.Services;
using DGamingApp.SignalIR;
using Microsoft.EntityFrameworkCore;


namespace DGamingApp.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            services.AddCors();

            //Repos
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPhotoService, PhotoService>();
            
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<LogUserActivity>();
            services.AddScoped<ILikesRepository, LikesRepository>(); 
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddSignalR();
            services.AddSingleton<PresenceTracker>(); 

            return services;
        }
    }
}
