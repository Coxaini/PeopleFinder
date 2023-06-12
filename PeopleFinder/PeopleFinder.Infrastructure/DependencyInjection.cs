using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PeopleFinder.Application.Common.Interfaces.Authentication;
using PeopleFinder.Domain.Repositories;
using PeopleFinder.Domain.Repositories.Common;
using PeopleFinder.Infrastructure.Authentication;
using PeopleFinder.Infrastructure.Authentication.Requirements;
using PeopleFinder.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Azure.Core.Diagnostics;
using Azure.Identity;
using PeopleFinder.Application.Common.Interfaces.ConnectionStorage;
using PeopleFinder.Application.Common.Interfaces.FileStorage;
using PeopleFinder.Application.Services.FileStorage;
using PeopleFinder.Application.Services.Security;
using PeopleFinder.Infrastructure.ConnectionStorage;
using PeopleFinder.Infrastructure.FileStorage;
using PeopleFinder.Infrastructure.Persistence;
using PeopleFinder.Infrastructure.Security;

namespace PeopleFinder.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration, bool isDevelopment)
        {
            services.AddAuth(configuration);
            services.AddPersistence(configuration);
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            /*if (isDevelopment)
            {
                services.AddFolderStorage(configuration);
            }
            else
            {*/
                services.AddAzureStorage(configuration);
            //
            
            services.AddSingleton<IConnectionStorage, UserConnectionMemoryStorage>();
            
            return services;
        }
        
        private static IServiceCollection AddFolderStorage(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddSingleton<IFileStorageManager, FileStorageManager>();
            return services;
        }
        private static IServiceCollection AddAzureStorage(this IServiceCollection services,ConfigurationManager configuration)
        {
            var keyVaultUrl = new Uri(configuration.GetValue<string>("KeyVaultUrl")!);

            var azureCredentials = new DefaultAzureCredential();
            configuration.AddAzureKeyVault(keyVaultUrl, azureCredentials);
            
            services.AddSingleton<IFileStorageManager, AzureBlobStorage>();
            return services;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services,
        IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new Exception("Database name not found in a configuration");

            services.AddDbContext<PeopleFinderDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
                // o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
            });
            
            services.BuildServiceProvider().GetService<PeopleFinderDbContext>()?.Database.Migrate();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        private static IServiceCollection AddAuth(this IServiceCollection services,
        IConfiguration configuration)
        {
             var jwtSettings = new JwtSettings();
             configuration.Bind("JwtSettings", jwtSettings);

            services.AddSingleton(Options.Create(jwtSettings));
           
            
            
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddSingleton<IRefreshTokenGenerator, RefreshTokenGenerator>();
           

            services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Secret))
                };
                
                
            });
            /*services.AddSingleton<IAuthorizationHandler, UserIdVerificationFromBodyHandler>();

            services.AddSingleton<IAuthorizationHandler, UserIdFromRouteHandler>();*/

            /*services.AddAuthorization(options =>
            {
                options.AddPolicy("UserIdInJson", policy =>
                {
                    policy.AddRequirements(new UserIdVerificationFromBodyRequirement());
                });
                options.AddPolicy("UserIdInRoute", policy =>
                {
                    policy.AddRequirements(new UserIdFromRouteRequirement());
                });
            });*/

            return services;
        }
    }
}
