using System.Text;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {

            services.AddIdentityCore<AppUser>(opt=> 
            {
                opt.Password.RequireNonAlphanumeric =false;
            })
                .AddRoles<AppRole>()
                .AddRoleManager<RoleManager<AppRole>>()
                .AddEntityFrameworkStores<DataContext>(); //stworzenie wszystkich tabel związanych z tożsamością w naszej bazie danych 


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => 
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding
                .UTF8.GetBytes(config["TokenKey"])),

                ValidateIssuer = false,
                ValidateAudience = false
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                       var accessToken = context.Request.Query["access_token"];

                       var path = context.HttpContext.Request.Path;
                       if(!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs")) //in Program.cs
                       {
                        context.Token = accessToken;  //to daje naszemu sygnałowi-hubowi dostęp do naszego tokena,ponieważ dodajego go do kontekstu
                       } 
                       return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization(opt=>
            {
                opt.AddPolicy("RequireAdminRole",policy => policy.RequireRole("Admin"));
                opt.AddPolicy("ModeratePhotoRole",policy => policy.RequireRole("Admin","Moderator"));

            });
            
            return services;
        

        }
        
    }
}