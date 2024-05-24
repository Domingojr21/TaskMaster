using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TaskMaster.Infraestructure.Identity.Context;
using TaskMaster.Infraestructure.Identity.Services;
using System.Text;
using TaskMaster.Infrastructure.Identity.Entities;
using TaskMaster.Core.Domain.Settings;
using Newtonsoft.Json;
using TaskMaster.Core.Application.Wrappers;

namespace TaskMaster.Infrastructure.Identity
{
    public static class ServiceRegistation
    {
        public static void AddIdentityInfrastructureForApi(this IServiceCollection services, IConfiguration configuration)
        {
          
            ContextConfiguration(services, configuration);

            #region Identity
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();

            services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JWTSettings:Issuer"],
                    ValidAudience = configuration["JWTSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                };
                options.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";
                        return c.Response.WriteAsync(c.Exception.ToString());
                    },
                    OnChallenge = c =>
                    {
                        c.HandleResponse(); 
                        c.Response.StatusCode = 401;
                        c.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new Response<string> ("You are not Authorized" ));
                        return c.Response.WriteAsync(result);
                    },
                    OnForbidden = c =>
                    {
                        c.Response.StatusCode = 403;
                        c.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new Response<string> ("You are not Authorized to access this resource" ));
                        return c.Response.WriteAsync(result);
                    }
                };

            });

            #endregion
            ServiceConfiguration(services);
          
        }

        #region "Private methods"

        private static void ContextConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            #region Contexts
           
                services.AddDbContext<IdentityContext>(options =>
                {
                    options.EnableSensitiveDataLogging();
                    options.UseSqlServer(configuration.GetConnectionString("IdentityConnectionString"),
                    m => m.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName));
                });
            
            #endregion
        }

        private static void ServiceConfiguration(this IServiceCollection services)
        {
            #region Services

            services.AddTransient<IAccountService, AccountService>();
            #endregion
        }
        #endregion

    }
}
