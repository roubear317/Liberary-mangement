using libraryManagment.Core.Model;
using libraryManagment.EF.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace libraryManagment.api.Extention_Services
{
    public static class IdentityService
    {

        public static IServiceCollection AddIdentityService(this IServiceCollection services,IConfiguration config)
        {
            var builder = services.AddIdentityCore<ApplicationUser>();

            builder = new IdentityBuilder(builder.UserType, builder.Services);

            builder.AddEntityFrameworkStores<AppDbcontext>();

            builder.AddSignInManager<SignInManager<ApplicationUser>>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options=> { options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = config["Jwt:Issuer"],
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))

            }; options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    var logger = services.BuildServiceProvider().GetService<ILoggerFactory>().CreateLogger("JwtBearer");
                    logger.LogError($"Authentication failed: {context.Exception.Message}");
                    return Task.CompletedTask;
                 
                },
                OnTokenValidated = context =>
                {
                    var logger = services.BuildServiceProvider().GetService<ILoggerFactory>().CreateLogger("JwtBearer");
                    logger.LogInformation("Token validated successfully");
                    return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    var logger = services.BuildServiceProvider().GetService<ILoggerFactory>().CreateLogger("JwtBearer");
                    logger.LogWarning("Authorization failed, challenge triggered");
                    return Task.CompletedTask;
                }
            };
            }


            );


            return services;

        }



    }
}
