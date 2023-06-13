
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace PeopleFinder.Mappers
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            
            var config = TypeAdapterConfig.GlobalSettings;
            
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();
            config.Scan(Assembly.GetExecutingAssembly());
            
            
            return services;
        }
    }
}