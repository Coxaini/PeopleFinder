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
using PeopleFinder.Application.Models.File;
using PeopleFinder.Application.Services.FileStorage;
using PeopleFinder.Application.Services.FriendsService;

namespace PeopleFinder.Application
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
   
            
            services.Configure<ImageSettings>(configuration.GetSection("ImageSettings"));
            
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IRecommendationService,RecommendationService>();
            services.AddScoped<IRelationshipService, RelationshipService>();
            services.AddScoped<IFileService, FileService>();
            
            services.AddScoped<IAccessVerificationService, AccessVerificationService>();
            

            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en");

            return services;
        }
    }
}
