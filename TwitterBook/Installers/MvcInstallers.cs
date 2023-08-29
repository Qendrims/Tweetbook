using System.Text;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TwitterBook.Authorization;
using TwitterBook.Domain;
using TwitterBook.Filters;
using TwitterBook.Options;
using TwitterBook.Services;

namespace TwitterBook.Installers
{
    public class MvcInstallers : IInstaller
    {
        public void InstallServices(IConfiguration configuration, IServiceCollection services)
        {
            var jwtSettings = new JwtSettings();
            var token = jwtSettings.Secret;
            configuration.Bind(nameof(jwtSettings), jwtSettings);
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddSession();
            services.AddSingleton(jwtSettings);
            services.AddControllersWithViews();
            services.AddMvc(options =>
                {
                    options.EnableEndpointRouting = false;
                    options.Filters.Add<ValidationFilter>();
                }
            );
            services.AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<Program>();
            });
            
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };

            services.AddSingleton(tokenValidationParameters);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            ).AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = tokenValidationParameters;
            });

            // services.AddAuthorization(options =>
            // {
            //     options.AddPolicy("TagViewer", builder =>
            //     {
            //         builder.RequireClaim("tags.View","true");
            //     });
            // });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("MustWorkForCompany",
                    policy => { policy.AddRequirements(new WorksForCompanyRequirement("company.com")); });
            });
            services.AddHttpContextAccessor();
            services.AddSingleton<IAuthorizationHandler, WorksForCompanyHandler>();
            services.AddSingleton<IUriService>
                (provider =>
                {
                    var accesor = provider.GetRequiredService<IHttpContextAccessor>();
                    var request = accesor.HttpContext.Request;
                    var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent(), "/");
                    return new UriService(absoluteUri);
                });
            
        }
    }
}