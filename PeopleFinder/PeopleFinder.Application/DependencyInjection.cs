using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PeopleFinder.Application.Services.Authentication;
using PeopleFinder.Application.Services.Authorization;
using PeopleFinder.Application.Services.ProfileServices;
using PeopleFinder.Application.Services.Recommendation;
using PeopleFinder.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PeopleFinder.Application.Common.Interfaces.FileStorage;
using PeopleFinder.Application.Common.Settings;
using PeopleFinder.Application.Models.File;
using PeopleFinder.Application.Services.ChatServices;
using PeopleFinder.Application.Services.FileStorage;
using PeopleFinder.Application.Services.Messages;
using PeopleFinder.Application.Services.RelationshipServices;

namespace PeopleFinder.Application
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
   
            
            services.Configure<FileSettings>(configuration.GetSection("FileSettings"));
            
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IRecommendationService,RecommendationService>();
            services.AddScoped<IRelationshipService, RelationshipService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IChatService, ChatService>();
            
            services.AddSingleton<IFileUrlService, FileUrlService>();
            services.AddSingleton<IFileTypeResolver, FileTypeResolver>();
            
            services.Configure<FileStorageSettings>(configuration.GetSection("FileStorageSettings"));
            
            
            
            services.AddScoped<IAccessVerificationService, AccessVerificationService>();
            

            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en");

            return services;
        }
    }
}
