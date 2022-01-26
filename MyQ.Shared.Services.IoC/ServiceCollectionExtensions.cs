using Microsoft.Extensions.DependencyInjection;
using MyQ.Shared.Services.Abstractions;

namespace MyQ.Shared.Services.IoC
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSharedServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IJsonService, JsonService>()
                .AddScoped<IFileProvider, FileProvider>();
        }
    }
}
