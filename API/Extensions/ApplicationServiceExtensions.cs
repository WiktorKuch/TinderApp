using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using API.SignalR;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration config)
        {
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });

            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();
            //services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<IPhotoService,PhotoService>();
            services.AddScoped<LogUserActivity>();
            //services.AddScoped<ILikesRepository,LikesRepository>();
           // services.AddScoped<IMessageRepository,MessageRepository>();
            services.AddSignalR();
            services.AddSingleton<PresenceTracker>(); //globalnie dostępny słownik,nie chcemy aby zostało zniszczone po zakończeniu żądania Http
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            return services;

        }
        

    }
}