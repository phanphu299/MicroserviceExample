
namespace OrderApi.Misc.Extensions
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using OrderApi.Repositories;
    public static class ServiceCollectionExtensions
    {
        public static void RegisterDependencies(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}
